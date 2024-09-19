namespace OpenCredentialPublisher.Wallet.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    public sealed class RequestBodyRewindMiddleware
    {
        readonly RequestDelegate _next;

        public RequestBodyRewindMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try { context.Request.EnableBuffering(); } catch { }
            await _next(context);
        }
    }

    public static class BodyRewindExtensions
    {
        public static IApplicationBuilder EnableRequestBodyRewind(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<RequestBodyRewindMiddleware>();
        }
    }
}

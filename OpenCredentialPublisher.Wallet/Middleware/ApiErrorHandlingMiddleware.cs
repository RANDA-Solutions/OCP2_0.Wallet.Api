using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Middleware
{

    public static class ApiErrorHandlingMiddlewareExtensions
    {        
        public static IApplicationBuilder UseApiErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiErrorHandlingMiddleware>();
        }
    }

    public class ApiErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private static readonly HashSet<string> CorsHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,
        };
        public ApiErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ApiErrorHandlingMiddleware> logger)
        {
            try
            {
                //mdoTemp DO NOT LEAVE THIS UNCOMMENTED !!! lol
                var request = context.Request;
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                //get body string here...
                var requestContent = Encoding.UTF8.GetString(buffer);
                logger.Log(LogLevel.Debug, context.Request.Path + " - " + requestContent);


                request.Body.Position = 0;  //rewinding the stream to 0

                // run the next step(s) in the pipeline and handle any exceptions below
                await next(context);
                
            }
            catch (Exception ex)
            {
                //var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
                //telemetry.TrackException(ex);

                logger.Log(LogLevel.Error, context.Request.Path + " - " + ex.ToString());
                await HandleExceptionAsync(context, 400, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode = 400, Exception ex = null)
        {
            if (context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                //response already written earlier in the pipeline
                return Task.CompletedTask;
            }

            ClearResponse(context, statusCode);

            context.Response.ContentType = "application/json";
            
            if (ex == null)
            {
                return Task.CompletedTask;
            }
            else
            {
                string errorType = ex.GetType().ToString();
                string baseErrorType = ex.GetBaseException().GetType().ToString();

                if (ex is ApiModelNotFoundException)
                {
                    context.Response.StatusCode = 404;

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = new
                        {
                            message = ex.GetFullMessage()
                        }
                    }));
                }
                else if (ex is ApiModelUnauthorizedAccessException)
                {
                    context.Response.StatusCode = 401;

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = new
                        {
                            message = ex.GetFullMessage()
                        }
                    }));
                }
                else if (ex is ApplicationException)
                {
                    context.Response.StatusCode = 500;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = new
                        {
                            message = ex.GetFullMessage(),
                            errorType = ex.GetType().ToString()
                        }
                    }));
                }
                else if (ex is ApiModelValidationException)
                {
                    context.Response.StatusCode = 400;
                    //if (context.Request.Path.ToString().Contains("/issuer/"))
                    //{
                    //    var x = 1;
                    //}

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = new
                        {
                            message = ex.GetFullMessage(),
                            errorType = ex.GetType().ToString(),
                            details = (ex as ApiModelValidationException).ModelState.Where(m => m.Key != "id" && m.Value.Errors.Count > 0).ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                    )
                        }
                    }));
                }
                else
                {
                    context.Response.StatusCode = 500;

                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = new
                        {
                            //message = "Something went wrong.",
                            message = ex.GetFullMessage(),
                            errorType = ex.GetType().ToString(),
                            details = new { exception = ex.GetFullMessage() }
                        }
                    }));

                }
            }

        }

        private static Task HandleStreamAsync(HttpContext context, int statusCode, string msg)
        {
            if (context.Response.HasStarted)
            {
                //response already written earlier in the pipeline
                return Task.CompletedTask;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message = string.IsNullOrEmpty(msg) ? ((HttpStatusCode)statusCode).ToString() : msg
                }
            }));

        }
        private static void ClearResponse(HttpContext context, int statusCode)
        {
            var headers = new HeaderDictionary();

            // Make sure problem responses are never cached.
            headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            headers.Append(HeaderNames.Pragma, "no-cache");
            headers.Append(HeaderNames.Expires, "0");

            foreach (var header in context.Response.Headers)
            {
                // Because the CORS middleware adds all the headers early in the pipeline,
                // we want to copy over the existing Access-Control-* headers after resetting the response.
                if (CorsHeaderNames.Contains(header.Key))
                {
                    headers.Add(header);
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            foreach (var header in headers)
            {
                context.Response.Headers.Add(header);
            }
        }
    }
}

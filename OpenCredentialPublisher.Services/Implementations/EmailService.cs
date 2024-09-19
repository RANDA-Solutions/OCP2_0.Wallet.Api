using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using OpenCredentialPublisher.Data.Custom.Settings;
using OpenCredentialPublisher.Data.Settings;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly HostSettings _hostSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _mailSettings = configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();
            _hostSettings = configuration.GetSection(nameof(HostSettings)).Get<HostSettings>();
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendEmailAsync(email, subject, htmlMessage, false);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage, bool hideClosing)
        {
            await SendEmailAsync(email, subject, htmlMessage, hideClosing ? "OpenCredentialPublisher.Services.Resources.Templates.email-no-closing.html" : "OpenCredentialPublisher.Services.Resources.Templates.email.html");
        }

        private async Task SendEmailAsync(string email, string subject, string htmlMessage, string template)
        {
            var assembly = typeof(EmailService).Assembly;
            var emailTemplateResourceStream = assembly.GetManifestResourceStream(template);

            if (emailTemplateResourceStream == null) return;

            string messageTemplate;

            using (TextReader reader = new StreamReader(emailTemplateResourceStream))
            {
                messageTemplate = await reader.ReadToEndAsync();
            }

            var messageHtml = string.Format(messageTemplate, _hostSettings.DnsName, subject, htmlMessage, _mailSettings.SupportEmail, DateTime.UtcNow.Year);

            var message = new MimeMessage()
            {
                Subject = subject,
            };

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = messageHtml
            };

            await using (var resource = assembly.GetManifestResourceStream("OpenCredentialPublisher.Services.Resources.Images.ky-lewallet_logo-white.png"))
            {
                if (resource != null)
                {
                    var logo = await bodyBuilder.LinkedResources.AddAsync("ky-lewallet_logo-white.png", resource, ContentType.Parse("img/png"));
                    logo.ContentId = "logoId";
                }

                message.Body = bodyBuilder.ToMessageBody();
                message.To.Add(new MailboxAddress(email, email));

                if (_mailSettings.RedirectToInternal)
                {
                    message.To.Clear();
                    message.To.Add(new MailboxAddress(_mailSettings.RedirectAddress, _mailSettings.RedirectAddress));
                }

                message.From.Add(new MailboxAddress(_mailSettings.From, _mailSettings.From));

                using var client = new SmtpClient();
                await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, options: _mailSettings.UseSSL ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_mailSettings.User, _mailSettings.Password);

                try
                {
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    var builder = new StringBuilder();

                    builder.AppendLine(ex.Message);
                    builder.AppendLine($"Server: {_mailSettings.Server}");
                    builder.AppendLine($"Port: {_mailSettings.Port}");
                    builder.AppendLine($"User: {_mailSettings.User}");
                    builder.AppendLine($"From: {_mailSettings.From}");
                    builder.AppendLine($"Enable SSL: {_mailSettings.UseSSL}");
                    builder.AppendLine($"To: {email}");
                    _logger.LogError(ex, builder.ToString());
                }
                await client.DisconnectAsync(true);
            }
        }
    }
}

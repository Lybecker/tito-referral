using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebAPI.Configuration;

namespace WebAPI.Services
{
    public class GmailSender : IMailSender
    {
        private readonly TitoConfiguration _config;
        private readonly ILogger<GmailSender> _logger;

        public GmailSender(TitoConfiguration config, ILogger<GmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string emailRecipient, IDictionary<string, string> replaceMap, string subject, string templateName)
        {
            var templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "EmailTemplates", templateName + ".html");

            if (!System.IO.File.Exists(templatePath))
            {
                var message = $"Cannot find email tempate '{templatePath}'";
                _logger.LogError(message);
                throw new Exception(message);
            }

            var template = System.IO.File.ReadAllText(templatePath);

            foreach (var item in replaceMap)
            {
                template = template.Replace($"*|{item.Key}|*", item.Value, StringComparison.InvariantCultureIgnoreCase);
            }

            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(_config.GmailEmailUsername, _config.GmailEmailPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress($"{_config.FromName} <{_config.FromEmail}> "),
                Subject = subject,
                Body = template,
                IsBodyHtml = true
            };

            mailMessage.To.Add(emailRecipient);
            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed sending email via Gmail");
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
    }
}
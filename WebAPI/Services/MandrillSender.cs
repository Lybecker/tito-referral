using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Microsoft.Extensions.Logging;
using WebAPI.Configuration;

namespace WebAPI.Services
{
    public class MandrillSender : IMailSender
    {
        private readonly TitoConfiguration _config;
        private readonly ILogger<MandrillSender> _logger;

        public MandrillSender(TitoConfiguration config, ILogger<MandrillSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string emailRecipient, IDictionary<string, string> replaceMap, string subject, string templateName)
        {
            var api = new MandrillApi(_config.MandrillAppApiKey);
            var message = new MandrillMessage
            {
                FromEmail = _config.FromEmail,
                FromName = _config.FromName,
                Subject = subject
            };

            message.AddTo(emailRecipient);

            foreach (var item in replaceMap)
            {
                message.AddGlobalMergeVars(item.Key, item.Value);
            }

            try
            {
                var result = await api.Messages.SendTemplateAsync(message, templateName);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error sending email via MandrillApp", ex);
            }
        }
    }
}
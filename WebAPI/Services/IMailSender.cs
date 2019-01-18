using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface IMailSender
    {
        Task SendAsync(string emailRecipient, IDictionary<string, string> replaceMap, string subject, string templateName);
    }
}
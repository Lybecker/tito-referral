using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Configuration;
using WebAPI.Model;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitoWebHookController : ControllerBase
    {
        private readonly ITitoClient _titoClient;
        private readonly TitoConfiguration _config;
        private readonly IDiscount_CodeBuilder _discount_CodeBuilder;
        private readonly ITitoTicketDiscountLinkGenerator _titoTicketDiscountLinkGenerator;
        private readonly ILogger<TitoWebHookController> _logger;

        public TitoWebHookController(ITitoClient titoClient, TitoConfiguration config, IDiscount_CodeBuilder discount_CodeBuilder, ITitoTicketDiscountLinkGenerator titoTicketDiscountLinkGenerator, ILogger<TitoWebHookController> logger)
        {
            _titoClient = titoClient;
            _config = config;
            _discount_CodeBuilder = discount_CodeBuilder;
            _titoTicketDiscountLinkGenerator = titoTicketDiscountLinkGenerator;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TicketCompletedEvent ticketCompletedEvent, [FromHeader(Name = "X-Webhook-Name")] string webhookName)
        {
            if (!webhookName.ToString().Equals("ticket.completed", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning("Request with invalid X-Webhook-Name value");
                return BadRequest("only ticket.completed X-Webhook-Name header supported.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request", ModelState);
                return BadRequest(ModelState);
            }

            var eventName = _config.Event.EventName;

            if (!ticketCompletedEvent.@event.slug.Equals(eventName, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning($"The request is for event '{ticketCompletedEvent.@event.slug}', but the system is configured for event '{eventName}'");
                return BadRequest($"Unknown event '{ticketCompletedEvent.@event.slug}'");
            }

            var discount = _discount_CodeBuilder.Build(ticketCompletedEvent);

            discount = await _titoClient.CreateDiscountCodeAsync(eventName, discount);

            var directDiscountLink = _titoTicketDiscountLinkGenerator.DiscountTicketLink(discount);


            var message = $"Thanks {ticketCompletedEvent.First_name} for joining us at MicroCPH conference, we appriciate your support for out community event. We would like your friends to join too, so please share this link with referral code {directDiscountLink}, then we will reward your friends with 10% discount and a specialied MicroCPH beer, exclusively for MicroCPH. Please use #MicroCPH when using social media. Kind Regards, the MicroCPH organizers.";


            //TODO: send email
            //TODO: logging/monitoring
            //TODO: validate config

            return Ok();
        }
    }

    public class EmailSender
    {
        private readonly TitoConfiguration _config;

        public EmailSender(TitoConfiguration config)
        {
            _config = config;
        }
        public async Task SendAsync()
        {
            var client = new SmtpClient("mysmtpserver")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("username", "password")
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("whoever@me.com")
            };
            mailMessage.To.Add("receiver@me.com");
            mailMessage.Subject = "subject";
            mailMessage.Body = "body";
            await client.SendMailAsync(mailMessage);
        }
    }
}
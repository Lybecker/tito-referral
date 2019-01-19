using System;
using System.Linq;
using System.Collections.Generic;
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
        private readonly IMailSender _mailSender;
        private readonly ILogger<TitoWebHookController> _logger;

        public TitoWebHookController(ITitoClient titoClient, TitoConfiguration config, IDiscount_CodeBuilder discount_CodeBuilder, ITitoTicketDiscountLinkGenerator titoTicketDiscountLinkGenerator, IMailSender mailSender, ILogger<TitoWebHookController> logger)
        {
            _titoClient = titoClient;
            _config = config;
            _discount_CodeBuilder = discount_CodeBuilder;
            _titoTicketDiscountLinkGenerator = titoTicketDiscountLinkGenerator;
            _mailSender = mailSender;
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

            bool discountAlreadyExists = false; // re-assigned ticket

            (discount, discountAlreadyExists) = await _titoClient.CreateDiscountCodeAsync(eventName, discount);

            var directDiscountLink = _titoTicketDiscountLinkGenerator.DiscountTicketLink(discount);

            var map = new Dictionary<string, string>() {
                    { "{firstName}", ticketCompletedEvent.First_name },
                    { "{directDiscountLink}", directDiscountLink }
                };

            await _mailSender.SendAsync(ticketCompletedEvent.Email, map,
                subject: "We look forward to seeing you, please help spread the word about the MicroCPH conference",
                templateName: "Referral Template");

            if (IsValidReferral(ticketCompletedEvent) && !discountAlreadyExists)
            {
                var referrerCode = ticketCompletedEvent.discount_code_used;

                var tickets = await _titoClient.GetTicketsAsync(eventName);

                var referrer = tickets.FirstOrDefault(x => x.reference.Equals(referrerCode, StringComparison.InvariantCultureIgnoreCase));

                if (referrer != null)
                {
                    // send email to referrer to say thanks
                    map = new Dictionary<string, string>() {
                        { "{firstName}", referrer.first_name },
                        { "{attendee_first_name}", ticketCompletedEvent.First_name },
                        { "{directDiscountLink}", _titoTicketDiscountLinkGenerator.DiscountTicketLink(referrerCode) }
                    };

                    await _mailSender.SendAsync(referrer.email, map,
                        subject: "You referred one. Thanks!",
                        templateName: "Referral Thanks");
                }
            }

            return Ok();
        }

        private bool IsValidReferral(TicketCompletedEvent ticketCompletedEvent)
        {
            if (string.IsNullOrEmpty(ticketCompletedEvent.discount_code_used))
                return false;

            // The ticket purchased must be applicable
            return _config.Event.TicketIds.Contains(ticketCompletedEvent.release_slug);
        }
    }
}
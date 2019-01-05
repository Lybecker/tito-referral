using System;
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

            //TODO: send email
            //TODO: logging/monitoring
            //TODO: validate config

            return Ok();
        }
    }
}
using System;
using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class Discount_CodeBuilder : IDiscount_CodeBuilder
    {
        private readonly TitoConfiguration _config;
        private readonly char _seperator = '-';

        public Discount_CodeBuilder(TitoConfiguration config)
        {
            _config = config;
        }

        public Discount_Code Build(TicketCompletedEvent ticketCompletedEvent)
        {
            return new Discount_Code() {
                code = GenerateCode(ticketCompletedEvent),
                type = _config.Event.DiscountPct ? DiscountTypes.PercentOffDiscountCode : DiscountTypes.MoneyOffDiscountCode,
                value = _config.Event.DiscountValue,
                release_ids = _config.Event.TicketIds
            };
        }

        private string GenerateCode(TicketCompletedEvent ticketCompletedEvent)
        {
            string name = ticketCompletedEvent.First_name;

            if (string.IsNullOrEmpty(name))
                name = "JohnDoe";

            return $"{ticketCompletedEvent.reference}{_seperator}{name}";
        }

        public string GetAttendeeReferrerCode(string discountCode)
        {
            if (!discountCode.Contains(_seperator))
                throw new ArgumentException($"{nameof(discountCode)} does not contain a '{_seperator}' seperator.");

            return discountCode.Substring(0, discountCode.LastIndexOf(_seperator));
        }
    }
}
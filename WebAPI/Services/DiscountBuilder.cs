using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class Discount_CodeBuilder : IDiscount_CodeBuilder
    {
        private readonly ITitoConfiguration _config;

        public Discount_CodeBuilder(ITitoConfiguration config)
        {
            _config = config;
        }

        public Discount_Code Build(TicketCompletedEvent ticketCompletedEvent)
        {
            return new Discount_Code() {
                code = GenerateCode(ticketCompletedEvent),
                type = _config.Event.Discount.Pct ? DiscountTypes.PercentOffDiscountCode : DiscountTypes.MoneyOffDiscountCode,
                value = _config.Event.Discount.Value,
                release_ids = _config.Event.Discount.TicketIds
            };
        }

        private static string GenerateCode(TicketCompletedEvent ticketCompletedEvent)
        {
            string name = ticketCompletedEvent.First_name;

            if (string.IsNullOrEmpty(name))
                name = "JohnDoe";


            return $"{ticketCompletedEvent.reference}-{name}";
        }
    }
}
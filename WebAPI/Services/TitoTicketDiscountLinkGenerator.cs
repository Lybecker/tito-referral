using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class TitoTicketDiscountLinkGenerator : ITitoTicketDiscountLinkGenerator
    {
        private readonly ITitoConfiguration _config;

        public TitoTicketDiscountLinkGenerator(ITitoConfiguration config)
        {
            _config = config;
        }

        public string DiscountTicketLink(Discount_Code discount)
        {
            return $"https://ti.to/{_config.AccountName}/{_config.Event.EventName}/discount/{discount.code}";
        }
    }
}
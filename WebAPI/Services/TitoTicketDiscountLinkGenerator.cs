using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class TitoTicketDiscountLinkGenerator : ITitoTicketDiscountLinkGenerator
    {
        private readonly TitoConfiguration _config;

        public TitoTicketDiscountLinkGenerator(TitoConfiguration config)
        {
            _config = config;
        }

        public string DiscountTicketLink(Discount_Code discount)
        {
            return DiscountTicketLink(discount.code);
        }

        public string DiscountTicketLink(string discountCode)
        {
            return $"https://ti.to/{_config.AccountName}/{_config.Event.EventName}/discount/{discountCode}";
        }
    }
}
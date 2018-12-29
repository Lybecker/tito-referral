using WebAPI.Model;

namespace WebAPI.Services
{
    public interface ITitoTicketDiscountLinkGenerator
    {
        string DiscountTicketLink(Discount_Code discount);
    }
}
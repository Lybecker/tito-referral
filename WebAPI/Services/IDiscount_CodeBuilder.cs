using WebAPI.Model;

namespace WebAPI.Services
{
    public interface IDiscount_CodeBuilder
    {
        Discount_Code Build(TicketCompletedEvent ticketCompletedEvent);
        string GetAttendeeReferrerCode(string discountCode);
    }
}
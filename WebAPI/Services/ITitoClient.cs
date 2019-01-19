using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Model.Ticket;

namespace WebAPI.Services
{
    public interface ITitoClient
    {
        Task<(Discount_Code, bool)> CreateDiscountCodeAsync(string eventName, Discount_Code discount);
        Task<Discount_Code> GetDiscountCodeAsync(string eventName, int id);
        Task<string> GetEventsAsync();
        Task<IEnumerable<Ticket>> GetTicketsAsync(string eventName);
    }
}
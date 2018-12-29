using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Services
{
    public interface ITitoClient
    {
        Task<Discount_Code> CreateDiscountCodeAsync(string eventName, Discount_Code discount);
        Task<Discount_Code> GetDiscountCodeAsync(string eventName, int id);
        Task<string> GetEventsAsync();
    }
}
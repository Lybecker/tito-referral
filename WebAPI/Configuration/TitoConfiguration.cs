using System.ComponentModel.DataAnnotations;
using WebAPI.Validation;

namespace WebAPI.Configuration
{
    public class TitoConfiguration
    {
        public TitoConfiguration()
        {
            Event = new Event();
        }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string ApiToken { get; set; }
        [Required, ValidateObject]
        public Event Event { get; set; }
    }

    public class Event
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        public string WebHookSecurityToken { get; set; }
        [Required]
        public Discount Discount { get; set; }
    }

    public class Discount
    {
        public bool Pct { get; set; }
        public decimal Value { get; set; }
        public string[] TicketIds { get; set; }
    }
}
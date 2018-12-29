namespace WebAPI.Configuration
{
    public class TitoConfiguration : ITitoConfiguration
    {
        public TitoConfiguration()
        {
            Event = new Event();
        }
        public string AccountName { get; set; }
        public string ApiToken { get; set; }
        public Event Event { get; set; }
    }

    public class Event
    {
        public string EventName { get; set; }
        public string WebHookSecurityToken { get; set; }
        public Discount Discount { get; set; }
    }

    public class Discount
    {
        public bool Pct { get; set; }
        public decimal Value { get; set; }
        public string[] TicketIds { get; set; }
    }
}

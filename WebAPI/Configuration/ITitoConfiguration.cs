namespace WebAPI.Configuration
{
    public interface ITitoConfiguration
    {
        string AccountName { get; set; }
        string ApiToken { get; set; }
        Event Event { get; set; }
    }
}
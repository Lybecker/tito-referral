namespace WebAPI.Model
{
    public class TitoRelease
    {
        public int id { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public object metadata { get; set; }
    }
}
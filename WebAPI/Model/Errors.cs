namespace WebAPI.Model
{
    public class RootErrors
    {
        public Errors Errors { get; set; }
    }

    public class Errors
    {
        public string[] Code { get; set; }
    }
}

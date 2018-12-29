namespace WebAPI.Services
{
    public interface ITitoRequestVerifyer
    {
        bool VerifyPayload(string payload, string signature);
    }
}
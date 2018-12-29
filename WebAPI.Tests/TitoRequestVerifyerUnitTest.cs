using WebAPI.Configuration;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests
{
    public class TitoRequestVerifyerUnitTest
    {
        [Fact]
        public void VerifyPayload_Success()
        {
            var config = new TitoConfiguration()
            {
                Event = new Event()
                {
                    EventName = "anders-test",
                    WebHookSecurityToken = "G4Ajq-hVDgg50Ixj5ls1MA"
                }
            };

            var sut = new TitoRequestVerifyer(config);

            var payload = System.IO.File.ReadAllText("payload.json");

            Assert.True(sut.VerifyPayload(payload, "uNSfuqrNb5saIFeIv2XPrYRMpIfyGmLNeEkw7gX4H+o="));
        }

        [Fact]
        public void VerifyPayload_Invalid()
        {
            var config = new TitoConfiguration()
            {
                Event = new Event()
                {
                    EventName = "anders-test",
                    WebHookSecurityToken = "G4Ajq-hVDgg50Ixj5ls1MA"
                }
            };

            var sut = new TitoRequestVerifyer(config);

            var payload = System.IO.File.ReadAllText("payload.json") + "Anders";

            Assert.False(sut.VerifyPayload(payload, "uNSfuqrNb5saIFeIv2XPrYRMpIfyGmLNeEkw7gX4H+o="));
        }
    }
}
using WebAPI.Configuration;
using WebAPI.Model;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests
{
    public class Discount_CodeBuilderUnitTest
    {
        [Fact]
        public void Create_Success()
        {
            var config = new TitoConfiguration()
            {
                Event = new Event()
                {
                    EventName = "anders-test",
                    DiscountPct = true,
                    DiscountValue = 1,
                    TicketIds = new string[0]
                }
            };

            var ticketCompletedEvent = new TicketCompletedEvent()
            {
                First_name = "refAnders",
                reference = "ABC1-1"
            };

            var sut = new Discount_CodeBuilder(config);

            var discountCode = sut.Build(ticketCompletedEvent);

            Assert.Equal(ticketCompletedEvent.reference, discountCode.code);
        }
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using WebAPI.Configuration;
using WebAPI.Services;

namespace TitoReferral.TestConsole
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Testing 1 2 3!");

            try
            {
                ITitoConfiguration config = LoadConfiguration(); 

                var tito = new TitoClient(new HttpClient(), config, null);
                //Console.WriteLine(tito.GetEventsAsync().Result);

                //var discount = new Discount_Code() { code = "fisk2", type = DiscountTypes.MoneyOffDiscountCode, value = 1 };
                //discount.release_ids = new[] { "53qhmtt-c5q", "7khfdtb0mso" };
                //var r = tito.CreateDiscountCodeAsync(config.Event.EventName, discount).Result;

                var dis = tito.GetDiscountCodeAsync(config.Event.EventName, 6783926).Result;
                var generator = new TitoTicketDiscountLinkGenerator(config);
                var link = generator.DiscountTicketLink(dis);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static TitoConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var config = new TitoConfiguration();
            configuration.Bind("Tito", config);
            return config;
        }
    }
}
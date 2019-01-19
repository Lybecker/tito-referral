using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using WebAPI.Configuration;
using WebAPI.Model;
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
                TitoConfiguration config = LoadConfiguration();
                var sender = new GmailSender(config, null);

                var map = new Dictionary<string, string>() {
                    {"{firstName}", "Anders" },
                    {"{directDiscountLink}","http://dr.dk" }
                };

                sender.SendAsync(args[0], map, "test mail", "Referral Template").Wait();
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
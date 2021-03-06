﻿using System.Linq;
using WebAPI.Configuration;
using WebAPI.Validation;
using Xunit;

namespace WebAPI.Tests
{
    public class ValidationExtensionsTests
    {
        public TitoConfiguration GetValidateConfiguration()
        {
            return new TitoConfiguration()
            {
                AccountName = "CNUG",
                ApiToken = "ABC123",
                FromEmail = "na@na.com",
                FromName = "Anders",
                GmailEmailUsername = "na",
                GmailEmailPassword = "P@ssword",
                MandrillAppApiKey = "API Key",
                Event = new Event()
                {
                    EventName = "anders-test",
                    WebHookSecurityToken = "G4Ajq-hVDgg50Ixj5ls1MA",
                    DiscountPct = false,
                    DiscountValue = 10
                }
            };

        }

        [Fact]
        public void EventName_Missing()
        {
            var config = GetValidateConfiguration();
            config.Event.EventName = "";

            var result = config.Validate().ToArray();

            Assert.Single(result);
            Assert.Contains("EventName field is required", result[0]);
        }

        [Fact]
        public void Multiple_Missing()
        {
            var config = GetValidateConfiguration();
            config.Event.EventName = "";
            config.ApiToken = null;

            var result = config.Validate().ToArray();

            Assert.Equal(2, actual: result.Length);
        }
    }
}
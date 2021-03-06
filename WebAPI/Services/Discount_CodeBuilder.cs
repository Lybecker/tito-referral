﻿using System;
using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class Discount_CodeBuilder : IDiscount_CodeBuilder
    {
        private readonly TitoConfiguration _config;

        public Discount_CodeBuilder(TitoConfiguration config)
        {
            _config = config;
        }

        public Discount_Code Build(TicketCompletedEvent ticketCompletedEvent)
        {
            return new Discount_Code() {
                code = GenerateCode(ticketCompletedEvent),
                type = _config.Event.DiscountPct ? DiscountTypes.PercentOffDiscountCode : DiscountTypes.MoneyOffDiscountCode,
                value = _config.Event.DiscountValue,
                release_ids = _config.Event.TicketIds
            };
        }

        private string GenerateCode(TicketCompletedEvent ticketCompletedEvent)
        {
            return $"{ticketCompletedEvent.reference}";
        }
    }
}
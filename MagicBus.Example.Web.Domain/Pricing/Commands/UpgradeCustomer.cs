﻿using System;

namespace MagicBus.Example.Web.Domain.Pricing.Commands
{
    public class UpgradeCustomer : ICommand
    {
        public Guid CustomerId { get; set; }
    }
}
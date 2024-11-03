﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EventHighway.Core.Models.EventAddresses;
using Microsoft.EntityFrameworkCore;

namespace EventHighway.Core.Brokers.Storages
{
    internal partial class StorageBroker
    {
        public DbSet<EventAddress> EventAddresses { get; set; }

        public async ValueTask<EventAddress> InsertEventAddressAsync(EventAddress eventAddress) =>
            await this.InsertAsync(eventAddress);
    }
}

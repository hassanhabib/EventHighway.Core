﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.EventAddresses.V2;

namespace EventHighway.Core.Services.Processings.EventAddresses.V2
{
    internal interface IEventAddressV2ProcessingService
    {
        ValueTask<EventAddressV2> RetrieveEventAddressV2ByIdAsync(Guid eventAddressV2Id);
    }
}

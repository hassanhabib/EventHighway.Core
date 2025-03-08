﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EventHighway.Core.Models.EventListeners.V2;
using EventHighway.Core.Models.ListenerEvents.V2;

namespace EventHighway.Core.Services.Orchestrations.EventListeners.V2
{
    internal interface IEventListenerV2OrchestrationService
    {
        ValueTask<IQueryable<EventListenerV2>> RetrieveEventListenerV2sByEventAddressIdAsync(Guid eventAddressId);
        ValueTask<ListenerEventV2> AddListenerEventV2Async(ListenerEventV2 listenerEventV2);
    }
}

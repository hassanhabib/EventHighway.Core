﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.Events.V1;

namespace EventHighway.Core.Tests.Acceptance.Brokers
{
    public partial class ClientBroker
    {
        public async ValueTask<EventV1> SubmitEventV2Async(EventV1 eventV2) =>
            await this.eventHighwayClient.EventV2s.SubmitEventV2Async(eventV2);

        public async ValueTask FireScheduledPendingEventV2sAsync() =>
            await this.eventHighwayClient.EventV2s.FireScheduledPendingEventV2sAsync();

        public async ValueTask<EventV1> RemoveEventV2ByIdAsync(Guid eventV2Id) =>
            await this.eventHighwayClient.EventV2s.RemoveEventV2ByIdAsync(eventV2Id);
    }
}

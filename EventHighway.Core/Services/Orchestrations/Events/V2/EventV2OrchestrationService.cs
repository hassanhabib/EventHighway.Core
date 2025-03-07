﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using EventHighway.Core.Brokers.Loggings;
using EventHighway.Core.Models.EventCall.V2;
using EventHighway.Core.Models.Events.V2;
using EventHighway.Core.Services.Processings.EventCalls.V2;
using EventHighway.Core.Services.Processings.Events.V2;

namespace EventHighway.Core.Services.Orchestrations.Events.V2
{
    internal partial class EventV2OrchestrationService : IEventV2OrchestrationService
    {
        private readonly IEventV2ProcessingService eventV2ProcessingService;
        private readonly IEventCallV2ProcessingService eventCallV2ProcessingService;
        private readonly ILoggingBroker loggingBroker;

        public EventV2OrchestrationService(
            IEventV2ProcessingService eventV2ProcessingService,
            IEventCallV2ProcessingService eventCallV2ProcessingService,
            ILoggingBroker loggingBroker)
        {
            this.eventV2ProcessingService = eventV2ProcessingService;
            this.eventCallV2ProcessingService = eventCallV2ProcessingService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<EventV2>> RetrieveScheduledPendingEventV2sAsync() =>
        TryCatch(async () =>
        {
            return await this.eventV2ProcessingService
                .RetrieveScheduledPendingEventV2sAsync();
        });

        public async ValueTask<EventCallV2> RunEventCallV2Async(EventCallV2 eventCallV2)
        {
            return await this.eventCallV2ProcessingService.RunEventCallV2Async(eventCallV2);
        }
    }
}

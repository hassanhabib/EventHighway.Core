﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EventHighway.Core.Models.Clients.EventListeners.V2.Exceptions;
using EventHighway.Core.Models.Services.Foundations.EventListeners.V2;
using EventHighway.Core.Models.Services.Orchestrations.EventListeners.V2.Exceptions;
using EventHighway.Core.Services.Orchestrations.EventListeners.V2;
using Xeptions;

namespace EventHighway.Core.Clients.EventListeners.V2
{
    internal class EventListenerV2sClient : IEventListenerV2sClient
    {
        private readonly IEventListenerV2OrchestrationService eventListenerV2OrchestrationService;

        public EventListenerV2sClient(IEventListenerV2OrchestrationService eventListenerV2OrchestrationService) =>
            this.eventListenerV2OrchestrationService = eventListenerV2OrchestrationService;

        public async ValueTask<EventListenerV2> RegisterEventListenerV2Async(
            EventListenerV2 eventListenerV2)
        {
            try
            {
                return await this.eventListenerV2OrchestrationService
                    .AddEventListenerV2Async(eventListenerV2);
            }
            catch (EventListenerV2OrchestrationValidationException
                eventListenerV2OrchestrationValidationException)
            {
                throw CreateEventListenerV2ClientDependencyValidationException(
                    eventListenerV2OrchestrationValidationException.InnerException
                        as Xeption);
            }
            catch (EventListenerV2OrchestrationDependencyValidationException
                eventListenerV2OrchestrationDependencyValidationException)
            {
                throw CreateEventListenerV2ClientDependencyValidationException(
                    eventListenerV2OrchestrationDependencyValidationException.InnerException
                        as Xeption);
            }
        }

        private static EventListenerV2ClientDependencyValidationException
            CreateEventListenerV2ClientDependencyValidationException(
                Xeption innerException)
        {
            return new EventListenerV2ClientDependencyValidationException(
                message: "Event listener client validation error occurred, fix the errors and try again.",
                innerException: innerException);
        }

        private static EventListenerV2ClientDependencyException CreateEventListenerV2ClientDependencyException(
            Xeption innerException)
        {
            return new EventListenerV2ClientDependencyException(
                message: "Event listener client dependency error occurred, contact support.",
                innerException: innerException);
        }

        private static EventListenerV2ClientServiceException CreateEventListenerV2ClientServiceException(
            Xeption innerException)
        {
            return new EventListenerV2ClientServiceException(
                message: "Event listener client service error occurred, contact support.",
                innerException: innerException);
        }
    }
}

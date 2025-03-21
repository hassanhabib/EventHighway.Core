﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.EventCall.V2;
using EventHighway.Core.Models.Services.Foundations.EventListeners.V2;
using EventHighway.Core.Models.Services.Foundations.Events.V2;
using EventHighway.Core.Models.Services.Foundations.ListenerEvents.V2;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EventHighway.Core.Tests.Unit.Services.Coordinations.V2
{
    public partial class EventV2CoordinationServiceTests
    {
        [Fact]
        public async Task ShouldSubmitImmediateEventV2WhenScheduledDateIsNullOrInPastAsync()
        {
            // given
            int randomDays = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset retrievedDateTimeOffset = randomDateTimeOffset;
            EventV2 randomEventV2 = CreateRandomEventV2();
            EventV2 inputEventV2 = randomEventV2;

            inputEventV2.ScheduledDate =
                retrievedDateTimeOffset.AddDays(randomDays);

            EventV2 inputScheduledEventV2 = inputEventV2;
            inputScheduledEventV2.Type = EventV2Type.Scheduled;
            EventV2 submittedEventV2 = inputScheduledEventV2;
            EventV2 expectedEventV2 = submittedEventV2.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTimeOffsetAsync())
                    .ReturnsAsync(retrievedDateTimeOffset);

            this.eventV2OrchestrationServiceMock.Setup(service =>
                service.SubmitEventV2Async(inputScheduledEventV2))
                    .ReturnsAsync(submittedEventV2);

            // when
            EventV2 actualEventV2 =
                await this.eventV2CoordinationService
                    .SubmitEventV2Async(inputEventV2);

            // then
            actualEventV2.Should().BeEquivalentTo(expectedEventV2);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTimeOffsetAsync(),
                    Times.Once);

            this.eventV2OrchestrationServiceMock.Verify(service =>
                service.SubmitEventV2Async(inputScheduledEventV2),
                    Times.Once);

            this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                service.RetrieveEventListenerV2sByEventAddressIdAsync(
                    It.IsAny<Guid>()),
                        Times.Never);

            this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                service.AddListenerEventV2Async(
                    It.IsAny<ListenerEventV2>()),
                        Times.Never);

            this.eventV2OrchestrationServiceMock.Verify(service =>
                service.RunEventCallV2Async(
                    It.IsAny<EventCallV2>()),
                        Times.Never);

            this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                service.ModifyListenerEventV2Async(
                    It.IsAny<ListenerEventV2>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.eventV2OrchestrationServiceMock.VerifyNoOtherCalls();
            this.eventListenerV2OrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ScheduledDates))]
        public async Task ShouldSubmitScheduledEventV2WhenScheduledDateIsInFutureAsync(
            DateTimeOffset randomDateTimeOffset,
            DateTimeOffset? scheduledDate)
        {
            // given
            var mockSequence = new MockSequence();
            EventV2 randomEventV2 = CreateRandomEventV2();
            EventV2 inputEventV2 = randomEventV2;
            inputEventV2.ScheduledDate = scheduledDate;
            EventV2 inputImmediateEventV2 = inputEventV2;
            inputImmediateEventV2.Type = EventV2Type.Immediate;
            EventV2 submittedEventV2 = inputImmediateEventV2;
            EventV2 expectedEventV2 = submittedEventV2.DeepClone();

            IQueryable<EventListenerV2> randomEventListenerV2s =
                CreateRandomEventListenerV2s();

            IQueryable<EventListenerV2> retrievedEventListenerV2s =
                randomEventListenerV2s;

            List<ListenerEventV2> inputListenerEventV2s =
                retrievedEventListenerV2s.Select(eventListenerV2 =>
                    new ListenerEventV2
                    {
                        EventListenerId = eventListenerV2.Id,
                        EventId = inputImmediateEventV2.Id,
                        Status = ListenerEventV2Status.Pending,
                        EventAddressId = inputImmediateEventV2.EventAddressId,
                        CreatedDate = inputImmediateEventV2.CreatedDate,
                        UpdatedDate = inputImmediateEventV2.UpdatedDate
                    }).ToList();

            List<ListenerEventV2> addedListenerEventV2s =
                inputListenerEventV2s.DeepClone();

            List<ListenerEventV2> modifiedListenerEventV2s =
                addedListenerEventV2s;

            List<ListenerEventV2> expectedListenerEventV2s =
                modifiedListenerEventV2s.DeepClone();

            List<EventCallV2> expectedInputCallEventV2s =
                retrievedEventListenerV2s.Select(
                    retrievedEventListenerV2 =>
                        new EventCallV2
                        {
                            Endpoint = retrievedEventListenerV2.Endpoint,
                            Content = inputImmediateEventV2.Content,
                            Secret = retrievedEventListenerV2.HeaderSecret,
                        }).ToList();

            var ranEventCallV2s = new List<EventCallV2>();

            this.dateTimeBrokerMock.InSequence(mockSequence).Setup(broker =>
                broker.GetDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.eventV2OrchestrationServiceMock
                .InSequence(mockSequence).Setup(service =>
                    service.SubmitEventV2Async(inputImmediateEventV2))
                        .ReturnsAsync(submittedEventV2);

            this.eventListenerV2OrchestrationServiceMock
                .InSequence(mockSequence).Setup(service =>
                    service.RetrieveEventListenerV2sByEventAddressIdAsync(
                        inputImmediateEventV2.EventAddressId))
                            .ReturnsAsync(retrievedEventListenerV2s);

            for (int index = 0; index < inputListenerEventV2s.Count; index++)
            {
                this.eventListenerV2OrchestrationServiceMock
                    .InSequence(mockSequence).Setup(service =>
                        service.AddListenerEventV2Async(
                            It.Is(SameListenerEventAs(inputListenerEventV2s[index]))))
                                .ReturnsAsync(addedListenerEventV2s[index]);

                var ranEventCall = new EventCallV2
                {
                    Endpoint = expectedInputCallEventV2s[index].Endpoint,
                    Content = expectedInputCallEventV2s[index].Content,
                    Response = GetRandomString()
                };

                this.eventV2OrchestrationServiceMock
                    .InSequence(mockSequence).Setup(service =>
                        service.RunEventCallV2Async(
                            It.Is(SameEventCallAs(expectedInputCallEventV2s[index]))))
                                .ReturnsAsync(ranEventCall);

                ranEventCallV2s.Add(item: ranEventCall);

                this.dateTimeBrokerMock.InSequence(mockSequence).Setup(broker =>
                    broker.GetDateTimeOffsetAsync())
                        .ReturnsAsync(randomDateTimeOffset);

                addedListenerEventV2s[index].UpdatedDate = randomDateTimeOffset;
                addedListenerEventV2s[index].Status = ListenerEventV2Status.Success;
                addedListenerEventV2s[index].Response = ranEventCallV2s[index].Response;

                this.eventListenerV2OrchestrationServiceMock
                    .InSequence(mockSequence).Setup(service =>
                        service.ModifyListenerEventV2Async(
                            It.Is(SameListenerEventAs(addedListenerEventV2s[index]))))
                                .ReturnsAsync(modifiedListenerEventV2s[index]);
            }

            // when
            EventV2 actualEventV2 =
                await this.eventV2CoordinationService
                    .SubmitEventV2Async(inputEventV2);

            // then
            actualEventV2.Should().BeEquivalentTo(expectedEventV2);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTimeOffsetAsync(),
                    Times.Exactly(callCount: inputListenerEventV2s.Count + 1));

            this.eventV2OrchestrationServiceMock.Verify(service =>
                service.SubmitEventV2Async(inputImmediateEventV2),
                    Times.Once);

            this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                service.RetrieveEventListenerV2sByEventAddressIdAsync(
                    inputImmediateEventV2.EventAddressId),
                        Times.Once);

            for (int index = 0; index < inputListenerEventV2s.Count; index++)
            {
                this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                    service.AddListenerEventV2Async(
                        It.Is(SameListenerEventAs(inputListenerEventV2s[index]))),
                            Times.Once);

                this.eventV2OrchestrationServiceMock.Verify(service =>
                    service.RunEventCallV2Async(
                        It.Is(SameEventCallAs(expectedInputCallEventV2s[index]))),
                            Times.Once);

                this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                    service.ModifyListenerEventV2Async(
                        It.Is(SameListenerEventAs(addedListenerEventV2s[index]))),
                            Times.Once);
            }

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.eventV2OrchestrationServiceMock.VerifyNoOtherCalls();
            this.eventListenerV2OrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

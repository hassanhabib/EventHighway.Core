﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.EventListeners.V2;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EventHighway.Core.Tests.Unit.Services.Processings.EventListeners.V2
{
    public partial class EventListenerV2ProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddEventListenerV2Async()
        {
            // given
            EventListenerV2 randomEventListenerV2 =
                CreateRandomEventListenerV2();

            EventListenerV2 inputEventListenerV2 =
                randomEventListenerV2;

            EventListenerV2 addedEventListenerV2 =
                inputEventListenerV2;

            EventListenerV2 expectedEventListenerV2 =
                addedEventListenerV2.DeepClone();

            this.eventListenerV2ServiceMock.Setup(broker =>
                broker.AddEventListenerV2Async(
                    inputEventListenerV2))
                        .ReturnsAsync(addedEventListenerV2);

            // when
            EventListenerV2 actualEventListenerV2 =
                await this.eventListenerV2ProcessingService
                    .AddEventListenerV2Async(
                        inputEventListenerV2);

            // then
            actualEventListenerV2.Should().BeEquivalentTo(
                expectedEventListenerV2);

            this.eventListenerV2ServiceMock.Verify(broker =>
                broker.AddEventListenerV2Async(
                    inputEventListenerV2),
                        Times.Once);

            this.eventListenerV2ServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

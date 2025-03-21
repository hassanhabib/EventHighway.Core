﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.EventListeners.V2;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EventHighway.Core.Tests.Unit.Clients.EventListeners.V2
{
    public partial class EventListenerV2sClientTests
    {
        [Fact]
        public async Task ShouldRemoveEventListenerV2ByIdAsync()
        {
            // given
            Guid randomEventListenerV2Id = GetRandomId();
            Guid inputEventListenerV2Id = randomEventListenerV2Id;

            EventListenerV2 randomEventListenerV2 =
                CreateRandomEventListenerV2();

            EventListenerV2 removedEventListenerV2 =
                randomEventListenerV2;

            EventListenerV2 expectedEventListenerV2 =
                removedEventListenerV2.DeepClone();

            this.eventListenerV2OrchestrationServiceMock.Setup(service =>
                service.RemoveEventListenerV2ByIdAsync(
                    inputEventListenerV2Id))
                        .ReturnsAsync(removedEventListenerV2);

            // when
            EventListenerV2 actualEventListenerV2 =
                await this.eventListenerV2SClient
                    .RemoveEventListenerV2ByIdAsync(
                        inputEventListenerV2Id);

            // then
            actualEventListenerV2.Should()
                .BeEquivalentTo(expectedEventListenerV2);

            this.eventListenerV2OrchestrationServiceMock.Verify(service =>
                service.RemoveEventListenerV2ByIdAsync(
                    inputEventListenerV2Id),
                        Times.Once);

            this.eventListenerV2OrchestrationServiceMock
                .VerifyNoOtherCalls();
        }
    }
}

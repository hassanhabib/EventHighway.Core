﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EventHighway.Core.Models.EventCall.V2;
using EventHighway.Core.Models.Orchestrations.Events.V2.Exceptions;
using FluentAssertions;
using Moq;
using Xeptions;

namespace EventHighway.Core.Tests.Unit.Services.Orchestrations.Events.V2
{
    public partial class EventV2OrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(EventCallV2DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRunIfEventCallV2DependencyExceptionOccursAndLogItAsync(
            Xeption eventCallV2DependencyException)
        {
            // given
            EventCallV2 someEventCallV2 = CreateRandomEventCallV2();

            var expectedEventV2OrchestrationDependencyException =
                new EventV2OrchestrationDependencyException(
                    message: "Event dependency error occurred, contact support.",
                    innerException: eventCallV2DependencyException.InnerException as Xeption);

            this.eventCallV2ProcessingServiceMock.Setup(service =>
                service.RunEventCallV2Async(It.IsAny<EventCallV2>()))
                    .ThrowsAsync(eventCallV2DependencyException);

            // when
            ValueTask<EventCallV2> runEventCallV2Task =
                this.eventV2OrchestrationService.RunEventCallV2Async(
                    someEventCallV2);

            EventV2OrchestrationDependencyException actualEventV2OrchestrationDependencyException =
                await Assert.ThrowsAsync<EventV2OrchestrationDependencyException>(
                    runEventCallV2Task.AsTask);

            // then
            actualEventV2OrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedEventV2OrchestrationDependencyException);

            this.eventCallV2ProcessingServiceMock.Verify(service =>
                service.RunEventCallV2Async(It.IsAny<EventCallV2>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEventV2OrchestrationDependencyException))),
                        Times.Once);

            this.eventCallV2ProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.eventV2ProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

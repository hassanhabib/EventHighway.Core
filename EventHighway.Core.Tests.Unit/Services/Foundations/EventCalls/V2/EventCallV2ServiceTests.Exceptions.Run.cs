﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using EventHighway.Core.Models.EventCall.V2;
using EventHighway.Core.Models.EventCall.V2.Exceptions;
using FluentAssertions;
using Moq;
using RESTFulSense.Exceptions;
using Xeptions;

namespace EventHighway.Core.Tests.Unit.Services.Foundations.EventCalls.V2
{
    public partial class EventCallV2ServiceTests
    {
        [Theory]
        [MemberData(nameof(CriticalDependencyExceptions))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRunIfCriticalDependencyErrorOccursAndLogItAsync(
            Xeption criticalDependencyException)
        {
            // given
            EventCallV2 someEventCallV2 = CreateRandomEventCallV2();

            var failedEventCallV2ConfigurationException =
                new FailedEventCallV2ConfigurationException(
                    message: "Failed event call configuration error occurred, contact support.",
                    innerException: criticalDependencyException);

            var expectedEventCallV2DependencyException =
                new EventCallV2DependencyException(
                    message: "Event call dependency error occurred, contact support.",
                    innerException: failedEventCallV2ConfigurationException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<EventCallV2> addEventCallV2Task =
                this.eventCallV2Service.RunEventCallV2Async(someEventCallV2);

            EventCallV2DependencyException actualEventCallV2DependencyException =
                await Assert.ThrowsAsync<EventCallV2DependencyException>(
                    addEventCallV2Task.AsTask);

            // then
            actualEventCallV2DependencyException.Should()
                .BeEquivalentTo(expectedEventCallV2DependencyException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedEventCallV2DependencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfBadRequestErrorOccursAndLogItAsync()
        {
            // given
            EventCallV2 someEventCallV2 = CreateRandomEventCallV2();

            HttpResponseBadRequestException httpBadRequestException =
                CreateHttpBadRequestException();

            var invalidEventCallV2Exception =
                new InvalidEventCallV2Exception(
                    message: "Event call is invalid, fix the errors and try again.",
                    innerException: httpBadRequestException,
                    data: httpBadRequestException.Data);

            var expectedEventCallV2DependencyValidationException =
                new EventCallV2DependencyValidationException(
                    message: "Event call validation error occurred, fix the errors and try again.",
                    innerException: invalidEventCallV2Exception);

            this.apiBrokerMock.Setup(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(httpBadRequestException);

            // when
            ValueTask<EventCallV2> addEventCallV2Task =
                this.eventCallV2Service.RunEventCallV2Async(someEventCallV2);

            EventCallV2DependencyValidationException actualEventCallV2DependencyValidationException =
                await Assert.ThrowsAsync<EventCallV2DependencyValidationException>(
                    addEventCallV2Task.AsTask);

            // then
            actualEventCallV2DependencyValidationException.Should()
                .BeEquivalentTo(expectedEventCallV2DependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEventCallV2DependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfHttpConflictErrorOccursAndLogItAsync()
        {
            // given
            EventCallV2 someEventCallV2 = CreateRandomEventCallV2();
            var httpConflictException = new HttpResponseConflictException();

            var alreadyExistsEventCallV2Exception =
                new AlreadyExistsEventCallV2Exception(
                    message: "Event call with same id already exists, try again.",
                    innerException: httpConflictException);

            var expectedEventCallV2DependencyValidationException =
                new EventCallV2DependencyValidationException(
                    message: "Event call validation error occurred, fix the errors and try again.",
                    innerException: alreadyExistsEventCallV2Exception);

            this.apiBrokerMock.Setup(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpConflictException);

            // when
            ValueTask<EventCallV2> addEventCallV2Task =
                this.eventCallV2Service.RunEventCallV2Async(someEventCallV2);

            EventCallV2DependencyValidationException actualEventCallV2DependencyValidationException =
                await Assert.ThrowsAsync<EventCallV2DependencyValidationException>(
                    addEventCallV2Task.AsTask);

            // then
            actualEventCallV2DependencyValidationException.Should()
                .BeEquivalentTo(expectedEventCallV2DependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEventCallV2DependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfHttpUnprocessableErrorOccursAndLogItAsync()
        {
            // given
            EventCallV2 someEventCallV2 = CreateRandomEventCallV2();
            var httpUnprocessableEntityException = new HttpResponseUnprocessableEntityException();

            var failedEventCallV2RequestException =
                new FailedEventCallV2RequestException(
                    message: "Failed event call request error occurred, fix the errors and try again.",
                    innerException: httpUnprocessableEntityException);

            var expectedEventCallV2DependencyValidationException =
                new EventCallV2DependencyValidationException(
                    message: "Event call validation error occurred, fix the errors and try again.",
                    innerException: failedEventCallV2RequestException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpUnprocessableEntityException);

            // when
            ValueTask<EventCallV2> addEventCallV2Task =
                this.eventCallV2Service.RunEventCallV2Async(someEventCallV2);

            EventCallV2DependencyValidationException actualEventCallV2DependencyValidationException =
                await Assert.ThrowsAsync<EventCallV2DependencyValidationException>(
                    addEventCallV2Task.AsTask);

            // then
            actualEventCallV2DependencyValidationException.Should()
                .BeEquivalentTo(expectedEventCallV2DependencyValidationException);

            this.apiBrokerMock.Verify(broker =>
                broker.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEventCallV2DependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

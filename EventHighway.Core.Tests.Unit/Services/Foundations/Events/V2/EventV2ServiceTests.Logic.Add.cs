﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.Events.V1;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EventHighway.Core.Tests.Unit.Services.Foundations.Events.V2
{
    public partial class EventV2ServiceTests
    {
        [Fact]
        public async Task ShouldAddEventV2Async()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            EventV1 randomEventV2 =
                CreateRandomEventV2(
                    dates: randomDateTimeOffset);

            EventV1 inputEventV2 = randomEventV2;
            EventV1 insertedEventV2 = inputEventV2;

            EventV1 expectedEventV2 =
                insertedEventV2.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEventV1Async(inputEventV2))
                    .ReturnsAsync(insertedEventV2);

            // when
            EventV1 actualEventV2 =
                await this.eventV2Service
                    .AddEventV2Async(inputEventV2);

            // then
            actualEventV2.Should().BeEquivalentTo(
                expectedEventV2);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventV1Async(inputEventV2),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

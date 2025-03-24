﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EventHighway.Core.Models.Services.Foundations.ListenerEvents.V1;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace EventHighway.Core.Tests.Unit.Services.Foundations.ListenerEvents.V2
{
    public partial class ListenerEventV2ServiceTests
    {
        [Fact]
        public async Task ShouldModifyListenerEventV2Async()
        {
            // given
            var mockSequence = new MockSequence();

            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            int randomDaysAgo =
                GetRandomNegativeNumber();

            ListenerEventV1 randomListenerEventV2 =
                CreateRandomListenerEventV2(dates: randomDateTimeOffset);

            ListenerEventV1 inputListenerEventV2 =
                randomListenerEventV2;

            inputListenerEventV2.CreatedDate =
                randomDateTimeOffset.AddDays(randomDaysAgo);

            ListenerEventV1 storageListenerEventV2 =
                inputListenerEventV2.DeepClone();

            int randomSecondsAgo =
                GetRandomNegativeNumber();

            DateTimeOffset storageUpdatedDate =
                randomDateTimeOffset.AddSeconds(
                    randomSecondsAgo);

            storageListenerEventV2.UpdatedDate =
                storageUpdatedDate;

            ListenerEventV1 persistedListenerEventV2 =
                inputListenerEventV2;

            ListenerEventV1 expectedListenerEventV2 =
                persistedListenerEventV2.DeepClone();

            Guid inputListenerEventV2Id =
                inputListenerEventV2.Id;

            this.dateTimeBrokerMock
                .InSequence(mockSequence).Setup(broker =>
                    broker.GetDateTimeOffsetAsync())
                        .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock
                .InSequence(mockSequence).Setup(broker =>
                    broker.SelectListenerEventV1ByIdAsync(
                        inputListenerEventV2Id))
                            .ReturnsAsync(
                                storageListenerEventV2);

            this.storageBrokerMock
                .InSequence(mockSequence).Setup(broker =>
                    broker.UpdateListenerEventV1Async(
                        inputListenerEventV2))
                            .ReturnsAsync(persistedListenerEventV2);

            // when
            ListenerEventV1 actualListenerEventV2 =
                await this.listenerEventV2Service
                    .ModifyListenerEventV2Async(
                        inputListenerEventV2);

            // then
            actualListenerEventV2.Should().BeEquivalentTo(
                expectedListenerEventV2);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectListenerEventV1ByIdAsync(
                    inputListenerEventV2Id),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateListenerEventV1Async(
                    inputListenerEventV2),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace EventHighway.Core.Models.Services.Foundations.EventAddresses.V2.Exceptions
{
    public class FailedEventAddressV2ServiceException : Xeption
    {
        public FailedEventAddressV2ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace EventHighway.Core.Models.Services.Foundations.Events.V1.Exceptions
{
    public class FailedEventV1ServiceException : Xeption
    {
        public FailedEventV1ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

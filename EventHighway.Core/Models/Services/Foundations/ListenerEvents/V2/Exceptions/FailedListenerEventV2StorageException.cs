﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace EventHighway.Core.Models.Services.Foundations.ListenerEvents.V2.Exceptions
{
    public class FailedListenerEventV2StorageException : Xeption
    {
        public FailedListenerEventV2StorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

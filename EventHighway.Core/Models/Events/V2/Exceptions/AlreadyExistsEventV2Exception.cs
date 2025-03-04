﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace EventHighway.Core.Models.Events.V2.Exceptions
{
    public class AlreadyExistsEventV2Exception : Xeption
    {
        public AlreadyExistsEventV2Exception(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

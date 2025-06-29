using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Interfaces
{
    /// <summary>
    /// Container for internally-fired event constants.
    /// </summary>
    public abstract class IEvents
    {
        public const string Contruction     = "Metreos.ApplicationControl.StaticConstruction";
        public const string Destruction     = "Metreos.ApplicationControl.InstanceDestruction";

        public abstract class Fields
        {
            public const string ErrorCode   = "ErrorCode";
            public const string ErrorText   = "ErrorText";
        }
    }
}

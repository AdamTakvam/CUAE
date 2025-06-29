using System;
using System.Diagnostics;

namespace Metreos.LoggingFramework
{
    public abstract class ILog
    {
        public const string LongTimestampFormat     = "yyyy:MM:dd::HH:mm:ss(ff)";
        public const string ShortTimestampFormat    = "HH:mm:ss.fff";
    }
}

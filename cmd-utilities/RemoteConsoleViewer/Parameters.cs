using System;

using Metreos.Core;

namespace Metreos.RemoteConsoleViewer
{
    public abstract class Parameters
    {
        public const string HelpParam       = "h";
        public const string Port            = "port";
        public const string Username        = "u";
        public const string Password        = "p";

        public abstract class Help
        {
            public const string HelpParam   = "-" + Parameters.HelpParam;
            public const string Host        = "<hostname>";
            public const string Port        = "-" + Parameters.Port + ":<number>";
            public const string Username    = "-" + Parameters.Username + ":<username>";
            public const string Password    = "-" + Parameters.Password + ":<password>";
        }

        public abstract class Defaults
        {
            public const int Port           = 8140;
        }
    }
}

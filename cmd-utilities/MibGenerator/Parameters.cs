using System;

using Metreos.Core;

namespace Metreos.MibGen
{
    public abstract class Parameters
    {
        public const string HelpParam       = "h";
        public const string Target          = "t";

        public abstract class Help
        {
            public const string HelpParam   = "-" + Parameters.HelpParam;
            public const string Target      = "-" + Parameters.Target + ":<directory>";
        }
    }
}

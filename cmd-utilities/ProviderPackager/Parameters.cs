using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.ProviderPackager
{
    public abstract class Parameters
    {
        public const string HelpParam           = "h";
        public const string Output              = "o";
        public const string RefFile             = "r";
        public const string ResFile             = "m";
        public const string WebFile             = "w";
        public const string DocFile             = "d";
        public const string DocManifest         = "dm";
        public const string ServiceFile         = "s";
        public const string ServRefFile         = "sr";
        public const string ServManifest        = "sm";

        public abstract class Help
        {
            public const string ProvFile        = "<filename>";
            public const string HelpParam       = "-" + Parameters.HelpParam;
            public const string Output          = "-" + Parameters.Output + ":<filename>";
            public const string RefFile         = "-" + Parameters.RefFile + ":<filename>";
            public const string ResFile         = "-" + Parameters.ResFile + ":<filename>";
            public const string WebFile         = "-" + Parameters.WebFile + ":<filename>";
            public const string DocFile         = "-" + Parameters.DocFile + ":<filename>";
            public const string DocManifest     = "-" + Parameters.DocManifest + ":<filename>";
            public const string ServiceFile     = "-" + Parameters.ServiceFile + ":<filename>";
            public const string ServRefFile     = "-" + Parameters.ServRefFile + ":<filename>";
            public const string ServManifest    = "-" + Parameters.ServManifest + ":<filename>";
        }
    }
}

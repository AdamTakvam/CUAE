using System;

using Metreos.Core;

namespace Metreos.PackageGenerator
{
    public abstract class Parameters
    {        
        public const string PARAM_HELP              = "h";
        public const string PARAM_DEBUG             = "d";
        public const string PARAM_OVERWRITE         = "y";
        public const string PARAM_SOURCE            = "src";
        public const string PARAM_DESTINATION       = "dest";
        public const string PARAM_SEARCH            = "search";
        public const string PARAM_REF               = "ref";

        public const string PARAM_HELP_HELP         = "-" + PARAM_HELP;
        public const string PARAM_OVERWRITE_HELP    = "-" + PARAM_OVERWRITE;
        public const string PARAM_SOURCE_HELP       = "-" + PARAM_SOURCE + ":<file>";
        public const string PARAM_DESTINATION_HELP  = "-" + PARAM_DESTINATION + ":<directory>";
        public const string PARAM_SEARCH_HELP       = "-" + PARAM_SEARCH + ":<directory>";
        public const string PARAM_REF_HELP          = "-" + PARAM_REF + ":<file>";
    }
}

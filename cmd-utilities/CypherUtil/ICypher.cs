using System;

using Metreos.Core;

namespace Metreos.CypherUtil
{
	public abstract class ICypher
	{
        public const string PARAM_HELP              = "h";
        public const string PARAM_DEBUG             = "x";
        public const string PARAM_ENCRYPT           = "e";
        public const string PARAM_DECRYPT           = "d";
        public const string PARAM_KEY               = "k";
        public const string PARAM_VERIFY            = "v";

        public const string PARAM_HELP_HELP         = "-" + PARAM_HELP;
        public const string PARAM_ENCRYPT_HELP      = "-" + PARAM_ENCRYPT + ":<string>";
        public const string PARAM_DECRYPT_HELP      = "-" + PARAM_DECRYPT + ":<string>";
        public const string PARAM_KEY_HELP          = "-" + PARAM_KEY + ":<key>";
        public const string PARAM_VERIFY_HELP       = "-" + PARAM_VERIFY;
	}
}

using System;

namespace Metreos.Native.Mail
{
	/// <summary>
	/// Summary description for IMail.
	/// </summary>
	public abstract class IMail
	{
        // Default values
        public const bool DEF_SEND_HTML     = false;
        public const string DEF_SUBJECT     = "<no subject>";
        public const int DEF_PORT           = 25;

        // Parameters
		public const string MAIL_SERVER     = "mailServer";
        public const string FROM            = "from";
        public const string TO              = "to";
        public const string SUBJECT         = "subject";
        public const string BODY            = "body";
        public const string ATTACH_PATH     = "attachmentPath";
        public const string SEND_AS_HTML    = "SendAsHtml";
        public const string USERNAME        = "username";
        public const string PASSWORD        = "password";
        public const string AUTH_MODE       = "authenticationMode";
	}
}

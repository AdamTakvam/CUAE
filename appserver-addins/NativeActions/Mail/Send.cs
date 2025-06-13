using System;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;
using Metreos.Interfaces;

using Package = Metreos.Interfaces.PackageDefinitions.Mail.Actions.Send;

namespace Metreos.Native.Mail
{
	/// <summary>Sends an email via SMTP or SMTPS</summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Mail.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Mail.Globals.PACKAGE_DESCRIPTION)]
	public class Send : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.To.DISPLAY, Package.Params.To.DESCRIPTION, true, Package.Params.To.DEFAULT)]
		public string To { set { to = value; } }
		private string to;
		
		[ActionParamField(Package.Params.From.DISPLAY, Package.Params.From.DESCRIPTION, false, Package.Params.From.DEFAULT)]
		public string From { set { from = value; } }
		private string from;

		[ActionParamField(Package.Params.MailServer.DISPLAY, Package.Params.MailServer.DESCRIPTION, false, Package.Params.MailServer.DEFAULT)]
		public string MailServer { set { mailServer = value; } }
		private string mailServer;

        [ActionParamField(Package.Params.Port.DISPLAY, Package.Params.Port.DESCRIPTION, false, Package.Params.Port.DEFAULT)]
        public int Port { set { port = value; } }
        private int port = IMail.DEF_PORT;

		[ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, false, Package.Params.Username.DEFAULT)]
		public string Username { set { username = value; } }
		private string username;

		[ActionParamField(Package.Params.Password.DISPLAY, Package.Params.Password.DESCRIPTION, false, Package.Params.Password.DEFAULT)]
		public string Password { set { password = value; } }
		private string password;

		[ActionParamField(Package.Params.Subject.DISPLAY, Package.Params.Subject.DESCRIPTION, false, Package.Params.Subject.DEFAULT)]
		public string Subject { set { subject = value; } }
		private string subject = IMail.DEF_SUBJECT;

		[ActionParamField(Package.Params.Body.DISPLAY, Package.Params.Body.DESCRIPTION, false, Package.Params.Body.DEFAULT)]
		public string Body { set { body = value; } }
		private string body;

        [ActionParamField(Package.Params.SendAsHtml.DISPLAY, Package.Params.SendAsHtml.DESCRIPTION, false, Package.Params.SendAsHtml.DEFAULT)]
        public bool SendAsHtml { set { sendAsHtml = value; } }
        private bool sendAsHtml = false;

        [ActionParamField(Package.Params.SSL.DISPLAY, Package.Params.SSL.DESCRIPTION, false, Package.Params.SSL.DEFAULT)]
        public bool SSL { set { ssl = value; } }
        private bool ssl = false;

        [ActionParamField(Package.Params.AttachmentPath.DISPLAY, Package.Params.AttachmentPath.DESCRIPTION, false, Package.Params.AttachmentPath.DEFAULT)]
        public StringCollection AttachmentPaths { set { attachPaths = value; } }
        private StringCollection attachPaths;

        public Send() {}

        public bool ValidateInput()
        {
            if((subject == null) && (body == null) && (attachPaths == null)) { return false; }
            if((from == null) && (username == null)) { return false; }

            return true;
        }

        public void Clear()
        {
            to = null;
            from = null;
            mailServer = null;
            port = IMail.DEF_PORT;
            username = null;
            password = null;
		    subject = IMail.DEF_SUBJECT;
            body = null;
            sendAsHtml = false;
            ssl = false;
            attachPaths = null;
        }

		[Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            if(mailServer == null)
            {
                // Parse out server from destination address 
                int index = to.IndexOf("@");
                if(index == -1)
                {
                    log.Write(TraceLevel.Error, "Invalid destination address: " + to);
                    return IApp.VALUE_FAILURE;
                }

                mailServer = to.Substring(index+1);

                log.Write(TraceLevel.Info, "No mail server specified, using: " + mailServer);
            }

            try
            {
                using(MailMessage message = new MailMessage(from, to, subject, body))
                {
                    message.IsBodyHtml = sendAsHtml;

                    if(attachPaths != null)
                    {
                        foreach(string attachment in attachPaths)
                        {
                            message.Attachments.Add(new Attachment(attachment));
                        }
                    }

                    SmtpClient client = new SmtpClient(mailServer, port);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = ssl;

                    if(username != null && password != null)
                        client.Credentials = new NetworkCredential(username, password);

                    client.Send(message);
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Warning, "Failed to send email. Error: " + e.Message);
                return IApp.VALUE_FAILURE;
            }

			return IApp.VALUE_SUCCESS;
		}
	}
}

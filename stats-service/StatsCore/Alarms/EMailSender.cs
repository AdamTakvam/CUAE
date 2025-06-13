using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;

namespace Metreos.Stats.Alarms
{
    public class EMailSender : AlarmSenderBase
    {
        private readonly IConfig.Severity triggerLevel;

        private readonly SmtpClient client;
        private readonly string to;
        private readonly string from;
        private readonly string localMachineName;

        public EMailSender(LogWriter log)
            : base(log)
        {
            int port = Convert.ToInt32(Config.SmtpManager.Port);
            if(port == 0)
                port = IStats.SmtpSender.Port;

            string mailServer = Config.SmtpManager.Server;
            string username = Config.SmtpManager.Username;
            string password = Config.SmtpManager.Password;

            this.to = Config.SmtpManager.Recipient;
            this.from = Config.SmtpManager.Sender;
            this.triggerLevel = Config.SmtpManager.TriggerLevel;
            this.localMachineName = Config.ApplicationServer.ServerName;

            if(mailServer == null || to == null || from == null || mailServer == "" || to == "" || from == "")
                throw new SenderNotConfiguredException("No SMTP target configured");

            this.client = new SmtpClient(mailServer, port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Config.SmtpManager.UseSSL;

            if(username != null && password != null)
                client.Credentials = new NetworkCredential(username, password);

            log.Write(TraceLevel.Info, "Testing connection to SMTP server: {0}:{1}", mailServer, port);

            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(mailServer, port);
                tcpClient.Client.Close();
                tcpClient.Close();
            }
            catch(Exception e)
            {
                string msg = String.Format("Failed to connect to SMTP server: {0}:{1}", mailServer, port);
                throw new SenderNotConfiguredException(msg, e);
            }

            log.Write(TraceLevel.Info, "Successfully connected to SMTP server");
        }

        /// <summary>Send Alarm Notification Email</summary>
        /// <param name="action">SMTP action with SMTP server and account info</param>
        /// <param name="data">Information about this alarm</param>
        /// <param name="bClear">true if clear alarm notification</param>
        /// <returns>true if sent</returns>
        public override void SendAlarm(AlarmData data, bool cleared)
        {
            // Check if severity is beyond threshold
            if(data.Severity > triggerLevel)
                return;

            // Use different subject line for trigger and clear email notifications
            string subject = null;
            if(cleared)
                subject = string.Format("CUAE Alarm Cleared Notification - Event Code ({0}) {1}", data.AlarmCode, localMachineName);
            else
                subject = string.Format("CUAE Alarm - Event Code ({0}) {1}", data.AlarmCode, localMachineName);

            // Build HTML mail body
            StringBuilder sb = new StringBuilder();

            if(cleared)
                sb.Append("<b>The following triggered alarm has been cleared.</b><br><br>");

            sb.Append("<table border=\"0\">");

            sb.Append("<tr>");
            if(data.Severity == IConfig.Severity.Green)
                sb.AppendFormat("<td><b>Severity:</b></td><td><font color=\"#00FF00\"><b>{0}</b></font></td>", data.Severity.ToString());
            else if(data.Severity == IConfig.Severity.Yellow)
                sb.AppendFormat("<td><b>Severity:</b></td><td><font color=\"#FFFF00\"><b>{0}</b></font></td>", data.Severity.ToString());
            else if(data.Severity == IConfig.Severity.Red)
                sb.AppendFormat("<td><b>Severity:</b></td><td><font color=\"#FF0000\"><b>{0}</b></font></td>", data.Severity.ToString());
            else
                sb.AppendFormat("<td><b>Severity:</b></td><td><font color=\"#000000\"><b>{0}</b></font></td>", data.Severity.ToString());

            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td><b>Event Code:</b></td><td>{0}</td>", data.AlarmCode);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td><b>Description:</b></td><td>{0}</td>", data.Description);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td><b>Time:</b></td><td>{0}</td>", data.CreatedTimeStamp);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td><b>Source:</b></td><td>{0}</td>", localMachineName);
            sb.Append("</tr>");
            sb.Append("</table>");

            try
            {
                using(MailMessage message = new MailMessage(from, to, subject, sb.ToString()))
                {
                    message.IsBodyHtml = true;

                    // Mark Red alarms "urgent"
                    if(data.Severity == IConfig.Severity.Red)
                        message.Priority = MailPriority.High;

                    client.Send(message);
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to send SMTP alarm: " + e.Message);
            }
        }

        public override void Dispose()
        {
        }
    }
}

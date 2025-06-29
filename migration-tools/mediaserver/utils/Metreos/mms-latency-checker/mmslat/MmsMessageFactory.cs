//
// MmsMessageFactory.cs
//
using System;
using System.Text;


namespace mmslat
{	
	public class MessageFactory
	{		       
        //public static string ServerConnect(int clientID, int transID)
        public static string ServerConnect(int transID)
        {
            StringBuilder s = MessageFactory.StartMessage(Const.idConnect);
            s.Append(MessageFactory.tagHeartbeatInterval(65535));            
            return MessageFactory.TerminateMessage(s, -1, transID);
        }

        public static string ServerDisconnect(int clientID, int transID)
        {
            StringBuilder s = MessageFactory.StartMessage(Const.idDisconnect);
            return MessageFactory.TerminateMessage(s, clientID, transID);
        }

        public static string FullConnectToNewConference
        ( int clientID, int transID, string ip, int port, bool isHairpin)
        {
            StringBuilder s = MessageFactory.StartMessage(Const.idConnect);
            s.Append(tagField(Const.idIpAddress, ip));
            s.Append(tagField(Const.idPort, port.ToString()));
            s.Append(tagField(Const.idConnectionID, szero));
            s.Append(tagField(Const.idConferenceID, szero));
            s.Append(tagField(Const.idHairpin, (isHairpin? 1: 0).ToString()));
            return MessageFactory.TerminateMessage(s, clientID, transID);
        }

        public static string FullConnectToExistingConference
        ( int clientID, int transID, string ip, int port, int conferenceID)
        {
            StringBuilder s = MessageFactory.StartMessage(Const.idConnect);
            s.Append(tagField(Const.idIpAddress, ip));
            s.Append(tagField(Const.idPort, port.ToString()));
            s.Append(tagField(Const.idConnectionID, szero));
            s.Append(tagField(Const.idConferenceID, conferenceID.ToString()));
            return MessageFactory.TerminateMessage(s, clientID, transID);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static StringBuilder StartMessage(string command)
        {
            StringBuilder s = new StringBuilder(MessageFactory.msgHeader);
            s.Append(MessageFactory.tagMessage(command));
            return s;
        }

        private static string TerminateMessage(StringBuilder s, int clientID, int transID)
        {
            if (clientID != -1)
                s.Append(MessageFactory.tagClientID(clientID));
            s.Append(MessageFactory.tagTransactionID(transID));
            s.Append(MessageFactory.tagServerID(1));
            s.Append(MessageFactory.msgTrailer);
            return s.ToString();
        }

        private static string tagMessage(string id)
        {
          return "<messageId>" + id + "</messageId>";
        }

        private static string tagHeartbeatInterval(int seconds)
        {
          return tagField(Const.idHeartbeatInterval, seconds.ToString());
        }

        private static string tagClientID(int id)
        {
            return tagField(Const.idClientID, id.ToString());
        }

        private static string tagServerID(int id)
        {
            return tagField(Const.idServerID, id.ToString());
        }

        private static string tagTransactionID(int id)
        {
            return tagField(Const.idTransactionId, id.ToString());
        }

        private static string tagField(string fieldname, string content)
        {
            StringBuilder s = new StringBuilder("<field name=\""); // <field name="
            s.Append(fieldname);   // name
            s.Append("\">");       // ">
            s.Append(content);     // content
            s.Append("</field>");  // </field>
            return s.ToString();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -   

        private const string msgHeader  = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><message>";
        private const string msgTrailer = "</message>";
        private const string szero = "0";

	}
}

using System;
using System.Collections;
using System.Diagnostics;

using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Transactions;


namespace Metreos.MMSTestTool.Messaging
{
	/// <summary>
	/// Summary description for XmlMMSMessageFactory.
	/// </summary>
	public class XmlMMSMessageFactory : MMSMessageFactory
	{
		public XmlMMSMessageFactory()
		{
		}
		

		public override MediaServerMessage CreateServerConnectMessage(string heartbeatInterval, string heartbeatPayload, string transId, string serverId)
		{
			MediaServerMessage message = new MediaServerMessage();
			message.MessageId = IMediaServer.MSG_MS_CONNECT;
			message.AddField(IMediaServer.FIELD_MS_MACHINE_NAME,Environment.MachineName);
			
			if (heartbeatInterval != null)
				message.AddField(IMediaServer.FIELD_MS_HEARTBEAT_INTERVAL,heartbeatInterval);
			
			if (heartbeatPayload != null)
				message.AddField(IMediaServer.FIELD_MS_HEARTBEAT_PAYLOAD,heartbeatPayload);
			
			Debug.Assert(transId != string.Empty, "WARNING: No transactionId specified for server connect...");
			message.AddField(IMediaServer.FIELD_MS_TRANSACTION_ID, transId);
			
            if (SessionManager.msInfo.useServerId)
                message.AddField(IMediaServer.FIELD_MS_SERVER_ID,serverId);
			
			return message;
		}
		
        public override MediaServerMessage CreateServerDisconnectMessage(string transId)
        {
            MediaServerMessage message = new MediaServerMessage();
            message.MessageId = IMediaServer.MSG_MS_DISCONNECT;
            if (SessionManager.msInfo.useClientId && SessionManager.msInfo.clientId != string.Empty)
                message.AddField(IMediaServer.FIELD_MS_CLIENT_ID, SessionManager.msInfo.clientId);
            message.AddField(IMediaServer.FIELD_MS_TRANSACTION_ID,transId);

            return message;
        }

		public override MediaServerMessage CreateHeartbeatMessage(MsTransactionInfo transaction, string heartbeatId)
		{
			MediaServerMessage message = new MediaServerMessage();
			
			message.MessageId = IMediaServer.MSG_MS_HEARTBEAT;
			message.AddField(IMediaServer.FIELD_MS_HEARTBEAT_ID,heartbeatId);
			if (SessionManager.msInfo.useClientId)
                message.AddField(IMediaServer.FIELD_MS_CLIENT_ID, SessionManager.msInfo.clientId);

			return message;
		}
		public override MediaServerMessage CreateMessage(MsTransactionInfo transaction, Command command)
		{
			MediaServerMessage message = new MediaServerMessage();
						
			message.MessageId = command.CommandType;
			foreach (ParameterField field in command.parameters)
				message.AddField(field.Name,field.Value);

            if (!message.IsFieldPresent(IMediaServer.FIELD_MS_TRANSACTION_ID))
                message.AddField(IMediaServer.FIELD_MS_TRANSACTION_ID,transaction.Id);

            if (SessionManager.msInfo.useClientId)
            {
                 if (!message.IsFieldPresent(IMediaServer.FIELD_MS_CLIENT_ID) && SessionManager.msInfo.clientId != string.Empty)
                    message.AddField(IMediaServer.FIELD_MS_CLIENT_ID, SessionManager.msInfo.clientId);
            }
			
            if (SessionManager.msInfo.useServerId)
            {
                if (!message.IsFieldPresent(IMediaServer.FIELD_MS_SERVER_ID))
                    message.AddField(IMediaServer.FIELD_MS_SERVER_ID, SessionManager.msInfo.serverId);
            }

			return message;
		}
		


		/// <summary>
		/// returns an ArrayList of ParameterFields
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public override ParameterContainer DecodeMessage(MediaServerMessage message)
		{
			ParameterContainer fields = new ParameterContainer();
			fields.Add(new ParameterField(IMediaServer.FIELD_MS_MESSAGE_ID, message.MessageId));
			foreach(Field field in message.Fields)
			{
				fields.Add(new ParameterField(field.Name, field.Value));
			}
				
			return fields;
		}

	}
}

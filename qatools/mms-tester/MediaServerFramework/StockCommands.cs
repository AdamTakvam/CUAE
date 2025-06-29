using System;

using Metreos.MmsTester.Core;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.MediaServerFramework
{
	/// <summary>
	/// Provides common base commands to send to the Media Server.
	/// </summary>
	public class StockCommands
	{
        public static InternalMessage CreateConnectToMediaServer(string machineName, string queueName, string heartbeatInterval, string transanctionId)
        {
            // Build the the connect to media server message
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_CONNECT;
            im.AddField(new Field(MmsProtocol.FIELD_MS_CLIENT_MACHINE_NAME, machineName));
            im.AddField(new Field(MmsProtocol.FIELD_MS_CLIENT_QUEUE_NAME, queueName));
            im.AddField(new Field(MmsProtocol.FIELD_MS_HEARTBEAT_INTERVAL, heartbeatInterval));
            im.AddField(new Field(MmsProtocol.FIELD_MS_TRANSACTION_ID, transanctionId));

            return im;
        }
        
        public static InternalMessage CreateConnect(string clientIp, string clientPort, string sessionTimeout, string commandTimeout, string transactionId)
        {
            // Build the connect message for the media server
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_CONNECT;
            im.AddField(new Field(MmsProtocol.FIELD_MS_IP_ADDRESS, clientIp));
            im.AddField(new Field(MmsProtocol.FIELD_MS_PORT, clientPort));
            im.AddField(new Field(MmsProtocol.FIELD_MS_SESSION_TIMEOUT, sessionTimeout));
            im.AddField(new Field(MmsProtocol.FIELD_MS_COMMAND_TIMEOUT, commandTimeout));
            im.AddField(new Field(MmsProtocol.FIELD_MS_TRANSACTION_ID, transactionId));

            return im;
        }

        public static InternalMessage CreateConnectToConference(string clientIp, string clientPort, string connectionId, string conferenceId, string transactionId)
        {
            // Build the connect to conference message for the media server      
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_CONNECT;
            im.AddField(new Field(MmsProtocol.FIELD_MS_IP_ADDRESS, clientIp));
            im.AddField(new Field(MmsProtocol.FIELD_MS_PORT, clientPort));
            im.AddField(new Field(MmsProtocol.FIELD_MS_CONNECTION_ID, connectionId));
            im.AddField(new Field(MmsProtocol.FIELD_MS_CONFERENCE_ID, conferenceId));
            im.AddField(new Field(MmsProtocol.FIELD_MS_TRANSACTION_ID, transactionId));
            
            return im;
        }

        public static InternalMessage CreateDisconnect(string connectionId, string transactionId)
        {
            // Build the disconnect message for the media server
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_DISCONNECT;
            im.AddField(new Field(MmsProtocol.FIELD_MS_CONNECTION_ID, connectionId));
            im.AddField(new Field(MmsProtocol.FIELD_MS_TRANSACTION_ID, transactionId));

            return im;
        }

        public static InternalMessage CreatePlayToConnection(string connectionId, string fileName, string transactionId)
        {
            // Build the create play to connection message for the media server
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_PLAY_ANN;
            im.AddField(new Field(MmsProtocol.FIELD_MS_CONNECTION_ID, connectionId));
            im.AddField(new Field(MmsProtocol.FIELD_MS_TRANSACTION_ID, transactionId));
            im.AddField(new Field(MmsProtocol.FIELD_MS_FILENAME, fileName));

            return im;
        }
	}
}

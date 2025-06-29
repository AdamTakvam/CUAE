using System;
using System.Collections;

using Metreos.Samoa.Core;
using Metreos.MmsTester.Conduit;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.AdapterFramework;
using Metreos.MmsTester.Core;
using Metreos.MmsTester.ClientFramework;
using Metreos.MmsTester.MediaServerFramework;

namespace Metreos.MmsTester.Custom.Clients
{
	/// <summary>
	/// Emulates the Application Server, in that commands relating to media are sent
	/// </summary>
	[Client("Application Server Emulator")]
	public class  ApplicationServerEmulator: ClientBase
	{
        // ArrayList mediaServer;
        int numberOfTotalPossibleConnections;

        // Media Server reference
        MediaServer mediaServer;

        // Reference to the conduit
        Conduit.Conduit conduit;

		public ApplicationServerEmulator(string machineName, string machineQueue, string machineGuid, int numberOfTotalPossibleConnections, Conduit.Conduit conduit)
		{
            this.numberOfTotalPossibleConnections = numberOfTotalPossibleConnections;
            this.conduit = conduit;
            mediaServer =  new MediaServer(machineName, machineQueue, numberOfTotalPossibleConnections, machineGuid);
		}

        public override bool AssociateAdapter(AdapterBase adapter)
        {
          this.adapter = adapter;
          return true;
        }

        public override bool Send(InternalMessage im)
        {
            switch (im.MessageId)
            {
                case IMessaging.CONNECT_TO_MEDIASERVER:

                    string machineName;
                    string queueName;
                    string heartBeatInterval;
                    string port;
                    string ipAddress;
                    string sessionTimeout;
                    string commandTimeout;
                    string conferenceId;
                    string connectionId;
                    string fileName;
                    string transactionId;

                    im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_MACHINE_NAME, out machineName);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_QUEUE_NAME, out queueName);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_HEARTBEAT_INTERVAL, out heartBeatInterval); 
                    im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
                    
                    im = StockCommands.CreateConnectToMediaServer(machineName, queueName, heartBeatInterval, transactionId);
                    
                    // Log event with media server
                    mediaServer.ConnectingToMediaServer(im);
                    
                    adapter.Send(im, new IAdapterTypes.ResponseFromMediaServerDelegate( LogResponse ), IMessaging.CONNECT_TO_MEDIASERVER );

                    break;


                case MmsProtocol.MSG_MS_CONNECT:


                    im.GetFieldByName(MmsProtocol.FIELD_MS_PORT, out port);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_IP_ADDRESS, out ipAddress);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_SESSION_TIMEOUT, out sessionTimeout); 
                    im.GetFieldByName(MmsProtocol.FIELD_MS_COMMAND_TIMEOUT, out commandTimeout);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
                    
                    im = StockCommands.CreateConnect(ipAddress, port, sessionTimeout, commandTimeout, transactionId);

                    // Log event with media server
                    mediaServer.ConnectingAConnection(im);

                    adapter.Send(im, new IAdapterTypes.ResponseFromMediaServerDelegate( LogResponse ), MmsProtocol.MSG_MS_CONNECT );
                    break;

                case IMessaging.CONNECT_TO_CONFERENCE:


                    im.GetFieldByName(MmsProtocol.FIELD_MS_PORT, out port);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_IP_ADDRESS, out ipAddress);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_CONNECTION_ID, out connectionId);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_CONFERENCE_ID, out conferenceId);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
                    
                    im = StockCommands.CreateConnectToConference(ipAddress, port, connectionId, conferenceId, transactionId);
                    
                    // Log event with media server
                    mediaServer.ConnectingToConference(im);
                    
                    adapter.Send(im, new IAdapterTypes.ResponseFromMediaServerDelegate( LogResponse ), IMessaging.DISCONNECT_FROM_CONFERENCE );
                    break;

                case MmsProtocol.MSG_MS_PLAY_ANN:

                    im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_MACHINE_NAME, out connectionId);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_CLIENT_QUEUE_NAME, out fileName);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);
                    
                    im = StockCommands.CreatePlayToConnection(connectionId, fileName, transactionId);

                    // Log event with media server
                    mediaServer.PreparingToPlayToConnection(im);

                    adapter.Send(im, new IAdapterTypes.ResponseFromMediaServerDelegate( LogResponse ),  MmsProtocol.MSG_MS_PLAY_ANN );
                    break;

                case MmsProtocol.MSG_MS_DISCONNECT:


                    im.GetFieldByName(MmsProtocol.FIELD_MS_CONNECTION_ID, out connectionId);
                    im.GetFieldByName(MmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);    
                    
                    im = StockCommands.CreateDisconnect(connectionId, transactionId);

                    // Log event with media server
                    mediaServer.DisconnectingAConnection(im);

                    adapter.Send(im, new IAdapterTypes.ResponseFromMediaServerDelegate( LogResponse ), MmsProtocol.MSG_MS_DISCONNECT );

                    break;
            }

            return true;
        }

        public override InternalMessage RequestMediaServerInfo(InternalMessage im)
        {      
            return mediaServer.GetFullInfo();
        }
        public InternalMessage LogResponse(InternalMessage im, string messageType)
        {
            switch (messageType)
            {
                case IMessaging.CONNECT_TO_MEDIASERVER:
                break;
    
                case MmsProtocol.MSG_MS_CONNECT:
                    mediaServer.ConnectedAConnection(im);
                break;

                case IMessaging.CONNECT_TO_CONFERENCE:
                break;

                case MmsProtocol.MSG_MS_PLAY_ANN:
                break;

                case MmsProtocol.MSG_MS_DISCONNECT:
                break;
            }

            conduit.SendMediaServerUpdate(im);

            InternalMessage success = new InternalMessage();
            success.MessageId = IMessaging.SUCCESS;


            return success;
        }
	}
}

using System;
using System.Text;
using System.Collections;

using Metreos.Samoa.Core;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.Core;

namespace Metreos.MmsTester.MediaServerFramework
{
	/// <summary>
	/// Encapsulates the functionality and limitiations of the media server
	/// </summary>
	public class MediaServer
	{
        public delegate bool OutgoingMessageDelegate(InternalMessage im);
        public delegate bool IncomingMessageDelegate(InternalMessage im);

        public MediaServerInformationTable mediaServerTable;

        private Hashtable incomingMessageMap;
        private Hashtable outgoingMessageMap;

        public string guid;

		public MediaServer( string mediaServerMachineName, string mediaServerQueueName, int numberOfTotalPossibleConnections, string guid )
		{
			mediaServerTable =  new MediaServerInformationTable(mediaServerMachineName, mediaServerQueueName, numberOfTotalPossibleConnections);
		
            this.outgoingMessageMap = new Hashtable();
            this.incomingMessageMap = new Hashtable();
            this.guid = guid;

            BuildIncomingMessageMap();
            BuildOutgoingMessageMap();
        }

        public InternalMessage GetFullInfo()
        {
            InternalMessage im = new InternalMessage();

            im.AddField(new Field(IMessaging.NUM_CONNECTIONS, mediaServerTable.connections.Length.ToString()));
            im.AddField(new Field(IMessaging.NUM_CONFERENCES, mediaServerTable.conferences.Count.ToString()));
            for(int i = 0; i < mediaServerTable.connections.Length; i++)
            {
                //MediaServer.Connection connection = mediaServerTable.connections[i];
                im.AddField(new Field(IMessaging.CONNECTION_NUM + i, mediaServerTable.connections[i].isConnecting + ";" +
                                                                        mediaServerTable.connections[i].isConnectedToConference +  ";" +
                                                                        mediaServerTable.connections[i].playing +  ";" +
                                                                        mediaServerTable.connections[i].recording +  ";" +
                                                                        mediaServerTable.connections[i].isMuted));                                                                                                                  
            }

            for(int i = 0; i < mediaServerTable.conferences.Count; i++)
            {
                MediaServerInformationTable.Conference table = (MediaServerInformationTable.Conference)mediaServerTable.conferences[i]; 
                
                StringBuilder listOfConnectionsInConference = new StringBuilder();
                
                for(int j = 0; j < table.connections.Count; j++)
                {
                    //listOfConnectionsInConference.Append(table.connections[i]
                }
                //im.AddField(new Field(IMessaging.CONFERENCE_NUM + i, table.connections[);

            }

            return im;
                                                                            
        }

        #region Outgoing Events

        public void OutgoingMessage(InternalMessage im)
        {
            OutgoingMessageDelegate whichFunction = (OutgoingMessageDelegate)outgoingMessageMap[im.MessageId];

            if(whichFunction(im))
            {
                //SendEventUpToConduit
            }
            else
            {
                //Log error
            }

        }

        #endregion Outgoing Events

        #region Incoming Events

        public void IncomingMessage(InternalMessage im)
        {
            IncomingMessageDelegate whichFunction = (IncomingMessageDelegate) incomingMessageMap[im.MessageId];

            if(whichFunction(im))
            {
                //SendEventUpToConduit
            }
            else
            {
                //Log error
            }
        }

        #endregion IncomingEvents

        
        #region Handle Outgoing Events

        public bool ConnectingToMediaServer(InternalMessage im)
        {
            mediaServerTable.ConnectingToServer(im);
            return true;
        }

        public bool ConnectingAConnection(InternalMessage im)
        {

            int handle;
            if(mediaServerTable.IsAvailableDisconnectedConnection(out handle))
            {
                if(mediaServerTable.MoveToConnectingAConnection(im, handle))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ConnectingToConference(InternalMessage im)
        {

            if(mediaServerTable.MoveToConnectingToConference(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DisconnectingAConnection(InternalMessage im)
        {
            if(mediaServerTable.MoveToDisconnectionAConnection(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DisconnectingFromConference(InternalMessage im)
        {
            return true;
        }

        public bool DisconnectionFromConference(InternalMessage im)
        {
            return true;
        }

        public bool PreparingToPlayToConnection(InternalMessage im)
        {
            return true;
        }

        public bool PreparingToPlayToConference(InternalMessage im)
        {
            return true;
        }
        #endregion Handle Outgoing Events
	    
        #region Handle Incoming Events

        public bool ConnectedToMediaServer(InternalMessage im)
        {
            if(mediaServerTable.MoveToConnectedToMediaServer(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ConnectedAConnection(InternalMessage im)
        {
            if(mediaServerTable.MoveToConnectedAConnection(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ConnectedToConference(InternalMessage im)
        {
            if(mediaServerTable.MoveToConnectedToConference(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DisconnectedAConnection(InternalMessage im)
        {
            if(mediaServerTable.MoveToDisconnectedAConnection(im))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool DisconnectedFromConference(InternalMessage im)
        {
            return true;
        }


        public bool PreparedToPlayToConnection(InternalMessage im)
        {
            return true;
        }


        public bool PreparedToPlayToConference(InternalMessage im)
        {
            return true;
        }



        #endregion Handle Incoming Events

        #region Maps

        private void BuildOutgoingMessageMap()
        {
            outgoingMessageMap[IMessaging.CONNECT_TO_MEDIASERVER] = new OutgoingMessageDelegate( ConnectingToMediaServer );
            outgoingMessageMap[MmsProtocol.MSG_MS_CONNECT] = new OutgoingMessageDelegate( ConnectingAConnection );
            outgoingMessageMap[IMessaging.CONNECT_TO_CONFERENCE] = new OutgoingMessageDelegate( ConnectingToConference );
            outgoingMessageMap[MmsProtocol.MSG_MS_DISCONNECT] = new OutgoingMessageDelegate( DisconnectingAConnection );
            outgoingMessageMap[IMessaging.DISCONNECT_FROM_CONFERENCE] = new OutgoingMessageDelegate( DisconnectingFromConference );
            outgoingMessageMap[MmsProtocol.MSG_MS_PLAY_ANN] = new OutgoingMessageDelegate( PreparingToPlayToConnection );
            outgoingMessageMap[IMessaging.PLAY_TO_CONFERENCE] = new OutgoingMessageDelegate( PreparingToPlayToConference );
            
        }

        private void BuildIncomingMessageMap()
        {
            incomingMessageMap[IMessaging.CONNECT_TO_MEDIASERVER] = new IncomingMessageDelegate( ConnectedToMediaServer );
            incomingMessageMap[MmsProtocol.MSG_MS_CONNECT] = new IncomingMessageDelegate( ConnectedAConnection );
            incomingMessageMap[IMessaging.CONNECT_TO_CONFERENCE] = new IncomingMessageDelegate( ConnectedToConference);
            incomingMessageMap[MmsProtocol.MSG_MS_DISCONNECT] = new IncomingMessageDelegate( DisconnectedAConnection );
            incomingMessageMap[IMessaging.DISCONNECT_FROM_CONFERENCE] = new IncomingMessageDelegate( DisconnectedFromConference );
            incomingMessageMap[MmsProtocol.MSG_MS_PLAY_ANN] = new IncomingMessageDelegate( PreparedToPlayToConnection );
            incomingMessageMap[IMessaging.PLAY_TO_CONFERENCE] = new IncomingMessageDelegate( PreparedToPlayToConference );
        }

        #endregion Maps
    }
}

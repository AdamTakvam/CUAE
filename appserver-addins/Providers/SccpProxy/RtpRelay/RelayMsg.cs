using System;
using System.Text;
using System.Collections;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
    /// <summary>
    /// The sole purpose of this class is to convey information 
    ///   about messages to be sent by the relay manager via
    ///   the relay manager queue.
    /// </summary>
	public class RelayMsg
	{
        private MsgApi.MsgTypes type;
        public MsgApi.MsgTypes Type { get { return type; } }

        private IDictionary fields;
        public IDictionary Fields { get { return fields; } }

        private int connectionId = -1;
        public int ConnectionId { get { return connectionId; } }

        private int relayObjectId = -1;
        public int RelayObjectId { get { return relayObjectId; } }

		public RelayMsg(int relayObjectId, int connectionId, MsgApi.MsgTypes type)
		{
            this.relayObjectId = relayObjectId;
            this.connectionId = connectionId;
            this.type = type;

            fields = new Hashtable();
		}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Type: ");
            sb.Append(type.ToString());
            sb.Append("\n");
            
            foreach(DictionaryEntry de in fields)
            {
                sb.Append(de.Key.ToString());
                sb.Append(": ");
                sb.Append(de.Value.ToString());
                sb.Append("\n");
            }

            return sb.ToString();
        }

	}
}

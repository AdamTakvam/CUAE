using System;

using Metreos.Messaging;
using Metreos.Messaging.Ipc;

namespace LoggingCore
{
    /// <summary>Basic data structure encapsulating a log message. Contains
    /// the required logic to build a LogMessage object from a flatmap and
    /// to serialize to a flatmap.</summary>
	public class LogMessage
	{
        private const uint MSG_DATA_KEY     = 1;
        private const uint MSG_CATEGORY_KEY = 2;

        public string msgData;
        public string msgCategory;

        public LogMessage()
        {}

        public LogMessage(string data, string category)
        {
            msgData     = data;
            msgCategory = category;
        }

        /// <summary>Serialize this log message to a flatmap byte array.</summary>
        /// <returns>A byte array with the serialized flatmap.</returns>
        public byte[] ToFlatmapByteArray()
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(MSG_DATA_KEY, msgData);
            flatmap.Add(MSG_CATEGORY_KEY, msgCategory);
            return flatmap.ToFlatmap();
        }

        /// <summary>Deserialize a flatmap byte array into a valid LogMessage
        /// object.</summary>
        /// <param name="rawMsg">The byte array to deserialize. Must contain a
        /// valid flatmap.</param>
        /// <returns>A LogMessage object.</returns>
        public static LogMessage FromByteArray(byte[] rawMsg)
        {
            // TODO: Validate that we have a valid LogMessage flatmap.

            FlatmapList flatmap = Flatmap.FromFlatmap(rawMsg);
            
            Flatmap.MapEntry msgDataEntry      = flatmap.Find(MSG_DATA_KEY, 1);
            Flatmap.MapEntry msgCategoryEntry  = flatmap.Find(MSG_CATEGORY_KEY, 1);
            
            LogMessage msg  = new LogMessage();
            msg.msgData     = msgDataEntry.dataValue as string;
            msg.msgCategory = msgCategoryEntry.dataValue as string;

            return msg;
        }
	}
}

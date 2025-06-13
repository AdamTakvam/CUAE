using System;
using System.Net;
using System.Diagnostics;
using System.Text;

using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents an SCCP message.
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// These are the values that Cisco has assigned to each SCCP
		/// message for which we perform special handling.
		/// </summary>
		public enum Type
		{
			Keepalive=0,
			Register=1,
			IpPort=2,
			KeypadButton=3,
			Offhook=6,
			Onhook=7,
			SpeeddialStatReq=10,
			LineStatReq=11,
			TimeDateReq=13,
			ButtonTemplateReq=14,
			CapabilitiesRes=16,
			ServerReq=18,
			Alarm=32,
			OpenReceiveChannelAck=34,
			ConnectionStatisticsRes=35,
			SoftkeySetReq=37,
			SoftkeyEvent=38,
			Unregister=39,
			SoftkeyTemplateReq=40,
			RegisterTokenReq=41,
			HeadsetStatus=43,
			RegisterAvailableLines=45,
			DeviceToUserData=46,
			DeviceToUserDataRes=47,
			ServiceUrlStatReq=51,
			FeatureStatReq=52,
			RegisterAck=129,
			StartTone=130,
			StopTone=131,
			SetRinger=133,
			SetLamp=134,
			SetSpeakerMode=136,
			SetMicroMode=137,
			StartMediaTransmission=138,
			StopMediaTransmission=139,
			CallInfo=143,
			ForwardStat=144,
			SpeeddialStat=145,
			LineStat=146,
			ConfigStat=147,
			DefineTimeDate=148,
			StartSessionTransmission=149,
			StopSessionTransmission=150,
			ButtonTemplate=151,
			Version=152,
			DisplayText=153,
			ClearDisplay=154,
			CapabilitiesReq=155,
			RegisterReject=157,
			ServerRes=158,
			Reset=159,
			KeepaliveAck=256,
			StartMulticastMediaReception=257,
			StartMulticastMediaTransmission=258,
			StopMulticastMediaReception=259,
			StopMulticastMediaTransmission=260,
			OpenReceiveChannel=261,
			CloseReceiveChannel=262,
			ConnectionStatisticsReq=263,
			SoftkeyTemplateRes=264,
			SoftkeySetRes=265,
			SelectSoftkeys=272,
			CallState=273,
			DisplayPromptStatus=274,
			ClearPromptStatus=275,
			DisplayNotify=276,
			ClearNotify=277,
			ActivateCallPlane=278,
			DeactivateCallPlane=279,
			UnregisterAck=280,
			BackspaceReq=281,
			RegisterTokenAck=282,
			RegisterTokenReject=283,
			StartMediaFailureDetection=284,
			DialedNumber=285,
			UserToDeviceData=286,
			FeatureStat=287,
			DisplayPriorityNotify=288,
			ClearPriorityNotify=289,
			ServiceUrlStat=303,
			CallSelectStat=304,
			None=999999999
		}

		/// <summary>
		/// Message constructor.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address of where message came from.</param>
		/// <param name="fromLocal">Address on localhost where this message was sent to.</param>
		public Message(byte[] rawMessage, IPEndPoint fromRemote,
			IPEndPoint fromLocal, QueueProcessor qp, Connection connection )
		{
			Assertion.Check(rawMessage != null,
				"SccpProxyProvider: rawMessage cannot be null");
			Assertion.Check(fromRemote != null,
				"SccpProxyProvider: fromRemote cannot be null");
			Assertion.Check(fromLocal != null,
				"SccpProxyProvider: fromLocal cannot be null");

			this.rawMessage = rawMessage;
			this.fromRemote = fromRemote;
			this.fromLocal = fromLocal;
			this.toRemote = null;	// Don't know where message will be sent
			this.toLocal = null;	// Don't know from what address message will be sent
			this.qp = qp;
			this.connection = connection;
		}

		public Connection Connection { get { return connection; } }

		private QueueProcessor qp;

		private Connection connection;

		private long created = HPTimer.Now();

		public long Delay { get { return HPTimer.NsSince( created ); } }

		/// <summary>
		/// Binary, raw SCCP message, starting at the packet-length field.
		/// </summary>
		private byte[] rawMessage;

		public byte[] xContents
		{
			get { return rawMessage; }
		}

		public int xLength
		{
			get { return rawMessage.Length; }
		}

		/// <summary>
		/// Address from which message came--who sent it.
		/// </summary>
		private IPEndPoint fromRemote;

		/// <summary>
		/// Property that has the value of where the message came from.
		/// </summary>
		public IPEndPoint FromRemote
		{
			get
			{
				return fromRemote;
			}
		}

		/// <summary>
		/// Address (on local host) to which message was sent.
		/// </summary>
		private IPEndPoint fromLocal;

		/// <summary>
		/// Property that has the value of the address (on local host) to which
		/// this message was sent.
		/// </summary>
		public IPEndPoint FromLocal
		{
			get
			{
				return fromLocal;
			}
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message was sent to the proxy.
		/// </summary>
		/// <remarks>
		/// Client connections (between proxy and SCCP clients) all have the
		/// same local address--our listen port--while CCM connections
		/// (between proxy and CCMs) share the same remote addresses (several
		/// clients can be proxied to the same CCM).
		/// </remarks>
		public abstract IPEndPoint FromUniqueAddress
		{
			get;
		}

		/// <summary>
		/// Address to which message is to be sent.
		/// </summary>
		private IPEndPoint toRemote;

		/// <summary>
		/// Property that has the value of where the message is to be sent.
		/// </summary>
		public IPEndPoint ToRemote
		{
			get
			{
				return toRemote;
			}
			set
			{
				toRemote = value;
			}
		}

		/// <summary>
		/// Address (on local host) from which message is sent.
		/// </summary>
		private IPEndPoint toLocal;

		/// <summary>
		/// Property that has the value of the address (on local host) from
		/// which this message is sent.
		/// </summary>
		public IPEndPoint ToLocal
		{
			get
			{
				return toLocal;
			}
			set
			{
				toLocal = value;
			}
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message is to be sent from the proxy.
		/// </summary>
		/// <remarks>
		/// Client connections (between proxy and SCCP clients) all have the
		/// same local address--our listen port--while CCM connections
		/// (between proxy and CCMs) share the same remote addresses (several
		/// clients can be proxied to the same CCM).
		/// </remarks>
		public abstract IPEndPoint ToUniqueAddress
		{
			get;
		}

		/// <summary>
		/// Read-only property that has the value of the message type for this message.
		/// </summary>
		public Type MessageType
		{
			get
			{
				if (_type == Type.None)
					_type = TypeField(rawMessage);
				return _type;
			}
		}

		private Type _type = Type.None;

		/// <summary>
		/// Returns whether this message of of the specified type.
		/// </summary>
		/// <param name="type">Type to compare against the type of the message.</param>
		/// <returns>Whether this message of of the specified type.</returns>
		public bool IsType(Type type)
		{
			return MessageType == type;
		}

		/// <summary>
		/// Returns whether this is a keepalive type message or response.
		/// </summary>
		/// <returns></returns>
		public bool IsKeepalive()
		{
			return IsType(Type.Keepalive) ||
				IsType(Type.KeepaliveAck);
		}

		/// <summary>
		/// Static method that returns the SCCP message type of the message
		/// contained in this byte array.
		/// </summary>
		/// <param name="message">Byte array containing an SCCP message.</param>
		/// <returns>SCCP message type.</returns>
		private static Type TypeField(byte[] message)
		{
			return (Message.Type)BitConverter.ToInt32(message, 8);
		}

		/// <summary>
		/// Convert string containing name of a message type to an enum of that
		/// type.
		/// </summary>
		/// <param name="messageTypeString">String containing name of message type.</param>
		/// <param name="messageType">Enum of message type.</param>
		/// <returns>Whether the string corresponds to an enum name.</returns>
		public static bool StringToType(string messageTypeString,
			out Type messageType)
		{
			bool defined = Enum.IsDefined(typeof(Type), messageTypeString);

			if (defined)
			{
				messageType = (Type)Enum.Parse(typeof(Type), messageTypeString);
			}
			else
			{
				messageType = Type.None; // Just to set it to something.
			}

			return defined;
		}

		/// <summary>
		/// Returns the name of the message, e.g., "Register," or number of
		/// the message if name not known.
		/// </summary>
		/// <returns>The name or number of the message.</returns>
		public override string ToString()
		{
			Type type = MessageType;
			string name = Enum.GetName(typeof(Type), type);
			if (name == null)
				name = type.ToString();
			return name;
		}

		public int GetInt32(int offset)
		{
			return BitConverter.ToInt32( rawMessage, offset );
		}

		public void PutInt32(int value, int offset, int length)
		{
			Array.Copy(BitConverter.GetBytes(value), 0, rawMessage, offset, length );
		}

		/// <summary>
		/// Return string from zero-delimited string in byte-array buffer,
		/// stripping off any trailing null characters..
		/// </summary>
		/// <param name="buffer">Buffer containing string.</param>
		/// <param name="offset">Offset into buffer where string is located.</param>
		/// <param name="length">Maximum length of string in byte array.</param>
		/// <returns>The string.</returns>
		public string StringByteArrayToString(int offset, int maxLength)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < maxLength; i++)
			{
				int c = rawMessage[offset+i]&255;
				if (c == 0)
					break;
				sb.Append((char)c);
			}
			return sb.ToString();

//			string str = (new ASCIIEncoding()).GetString(rawMessage, offset, length);
//			int index = str.IndexOf('\0');
//			if (index >= 0)
//			{
//				str = str.Substring(0, index);
//			}
//			return str;
		}

		/// <summary>
		/// Return string version of IP address in byte array at given offset.
		/// </summary>
		/// <remarks>
		/// For example, 0x0a, 0x01, 0x0a, 0x34 would return "10.1.10.52".
		/// </remarks>
		/// <param name="buffer">Buffer containing IP address.</param>
		/// <param name="offset">Offset into the buffer where IP address starts</param>
		/// <returns>"Dot" version of IP address as string.</returns>
		public IPAddress GetIpAddress(int offset)
		{
			int x = GetInt32(offset);
			IPAddress ip = new IPAddress(x&0xffffffff);
			//Console.WriteLine( "--------------------- {0}", ip );
			return ip;
		}

		/// <summary>
		/// Copy byte-array version of IP address from string into provided byte array at given offset.
		/// </summary>
		/// <remarks>
		/// For example, "10.1.10.52" would copy 0x0a, 0x01, 0x0a, 0x34 into the byte array.
		/// </remarks>
		/// <param name="ipAddressString">"Dot" version of IP address as string.</param>
		/// <param name="buffer">Byte array into which the IP-address byte array is copied.</param>
		/// <param name="offset">Offset into the byte array where IP address is copied.</param>
		public void PutIpAddress(IPAddress addr, int offset)
		{
//			IPAddress ipAddress = IPAddress.Parse(ipAddressString);
//			byte[] ipAddressByteArray = ipAddress.GetAddressBytes();
			Array.Copy(addr.GetAddressBytes(), 0, rawMessage, offset, 4);
		}

		public void PutBytes(byte[] buf, int bufOffset, int offset, int length)
		{
			Array.Copy( buf, bufOffset, rawMessage, offset, length );
		}

		public void BlockQueue()
		{
			qp.Block();
		}

		public void UnblockQueue()
		{
			qp.Unblock();
		}
	}
}
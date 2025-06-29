using System;
using System.Reflection;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using Metreos.Utilities;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an abstract SCCP message.
	/// </summary>
	/// <remarks>
	/// An SCCP message is represented internally by message-specific public
	/// fields--a raw version of the SCCP message is not maintained.
	/// Encoding to and decoding from raw format is done only when needed,
	/// i.e., when accessing (get/set) the Raw property.
	/// </remarks>
	public abstract class SccpMessage : Message
	{
		/// <summary>
		/// Constructs an SccpMessage of the specified type.
		/// </summary>
		/// <param name="type"></param>
		public SccpMessage(Type type)
		{
			this.type = type;
		}

		/// <summary>
		/// Enumerated type of the SCCP message.
		/// </summary>
		private readonly Type type;

		/// <summary>
		/// Property whose value is the enumerated type of the SCCP message.
		/// </summary>
		public Type MessageType { get { return type; } }

		/// <summary>
		/// Property whose value is the line number that this message
		/// references. If not applicable--the message does not contain a
		/// line-number field--return 0.
		/// </summary>
		public virtual uint Line { get { return 0; } }

		/// <summary>
		/// Property whose value is the call id, or call reference, that this
		/// message references. If not applicable--the message does not contain
		/// a line-number field--return 0.
		/// </summary>
		public virtual uint CallId { get { return 0; } }

		/// <summary>
		/// Property whose value is the conference id that this message
		/// references. If not applicable--the message does not contain a
		/// line-number field--return 0.
		/// </summary>
		public virtual uint ConferenceId { get { return 0; } }

		/// <summary>
		/// Property whose value is the passthru party id that this message
		/// references. If not applicable--the message does not contain a
		/// line-number field--return 0.
		/// </summary>
		public virtual uint PassthruPartyId { get { return 0; } }

		/// <summary>
		/// Property that represents a raw SCCP message.
		/// </summary>
		/// <remarks>Getting the value of this property causes the internal
		/// message representation to be encoded into an SCCP message;
		/// setting it causes the value to be decoded into the internal
		/// representation.</remarks>
		/// <value>Encoding if can be encoded; null otherwise.</value>
		internal byte[] Raw
		{
			get
			{
				// Start encoder, which encodes common fields including message
				// type.
				Encoder encoder = new Encoder(type);

				// Encode message by typically invoking subclass's Encode
				// method.
				Encode(encoder);

				// Return byte array containing encoded SCCP message.
				return encoder.Encoding();
			}
			set
			{
				// Perform message-specific decoding.
				Decode(new Decoder(value));
			}
		}

		/// <summary>
		/// Encodes to SCCP message.
		/// </summary>
		/// <remarks>
		/// This is virtual rather than abstract because some message do not
		/// contain fields (other than message type) and therefore do not need
		/// encoding.
		/// </remarks>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal virtual void Encode(Encoder encoder) { }

		/// <summary>
		/// Decodes from SCCP message
		/// </summary>
		/// <remarks>
		/// This is virtual rather than abstract because some message do not
		/// contain fields (other than message type) and therefore do not need
		/// decoding.
		/// </remarks>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal virtual void Decode(Decoder decoder) { }
		
		/// <summary>
		/// These are the values that Cisco has assigned to the SCCP messages.
		/// </summary>
		/// <remarks>
		/// The corresponding SccpMessage subclasses for these messages MUST
		/// have the same name as the enum name.
		/// </remarks>
		public enum Type
		{
			//				whether client Receives this message from or Writes it to the CallManager
			ActivateCallPlane = 0x116,				// R
			Alarm = 0x20,							//		W
			BackspaceReq = 0x119,					// R
			ButtonTemplate = 0x97,					// R
			ButtonTemplateReq = 0xe,				//		W
			CallInfo = 0x8f,						// R
			CallSelectStat = 0x130,					// R
			CallState = 0x111,						// R
			CapabilitiesReq = 0x9b,					// R
			CapabilitiesRes = 0x10,					//		W
			ClearDisplay = 0x9a,					// R
			ClearNotify = 0x115,					// R
			ClearPriorityNotify = 0x121,			// R
			ClearPromptStatus = 0x113,				// R
			CloseReceiveChannel = 0x106,			// R
			ConfigStat = 0x93,						// R
			ConnectionStatisticsReq = 0x107,		// R
			ConnectionStatisticsRes = 0x23,			//		W
			DeactivateCallPlane = 0x117,			// R
			DefineTimeDate = 0x94,					// R
			DeviceToUserData = 0x2e,				//		W
			DeviceToUserDataRes = 0x2f,				//		W
			DialedNumber = 0x11d,					// R
			DisplayNotify = 0x114,					// R
			DisplayPriorityNotify = 0x120,			// R
			DisplayPromptStatus = 0x112,			// R
			DisplayText = 0x99,						// R
			FeatureStat = 0x11f,					// R
			FeatureStatReq = 0x34,					//		W
			ForwardStat = 0x90,						// R
			HeadsetStatus = 0x2b,					//		W
			IpPort = 0x2,							//		W
			Keepalive = 0x0,						// R	W
			KeepaliveAck = 0x100,					// R	W
			KeypadButton = 0x3,						// R	W
			LineStat = 0x92,						// R
			LineStatReq = 0xb,						//		W
			OffhookSccp = 0x6,						// R	W
			Onhook = 0x7,							// R	W
			OpenReceiveChannel = 0x105,				// R
			OpenReceiveChannelAck = 0x22,			//		W
			Register = 0x1,							//		W
			RegisterAvailableLines = 0x2d,			//		W
			RegisterAck = 0x81,						// R
			RegisterReject = 0x9d,					// R
			RegisterTokenAck = 0x11a,				// R
			RegisterTokenReject = 0x11b,			// R
			RegisterTokenReq = 0x29,				//		W
			Reset = 0x9f,							// R
			SelectSoftkeys = 0x110,					// R
			ServerReq = 0x12,						//		W
			ServerRes = 0x9e,						// R
			ServiceUrlStat = 0x12f,					// R
			ServiceUrlStatReq = 0x33,				//		W
			SetLamp = 0x86,							// R
			SetRinger = 0x85,						// R
			SetSpeakerMode = 0x88,					// R
			SetMicroMode = 0x89,					// R
			SoftkeyEvent = 0x26,					//		W
			SoftkeySetReq = 0x25,					//		W
			SoftkeySetRes = 0x109,					// R
			SoftkeyTemplateReq = 0x28,				//		W
			SoftkeyTemplateRes = 0x108,				// R
			SpeeddialStat = 0x91,					// R
			SpeeddialStatReq = 0xa,					//		W
			StartMediaFailureDetection = 0x11c,		// R
			StartMediaTransmission = 0x8a,			// R
			StartMulticastMediaReception = 0x101,	// R
			StartMulticastMediaTransmission = 0x102,// R
			StartSessionTransmission = 0x95,		// R
			StartTone = 0x82,						// R
			StopMediaTransmission = 0x8b,			// R
			StopMulticastMediaReception = 0x103,	// R
			StopMulticastMediaTransmission = 0x104,	// R
			StopSessionTransmission = 0x96,			// R
			StopTone = 0x83,						// R
			TimeDateReq = 0xd,						//		W
			Unregister = 0x27,						//		W
			UnregisterAck = 0x118,					// R
			UserToDeviceData = 0x11e,				// R
			Version_ = 0x98,						// R
            UserToDeviceDataVersion1 = 0x13F,
		}

		/// <summary>
		/// Size constants.
		/// </summary>
		internal abstract class Const
		{
			public const int UintSize = 4;
			public const int UshortSize = 2;
			public const int ByteSize = 1;	// Pedantic, aren't I?
			public const int EnumSize = UintSize;
			public const int MaxAlarmTextSize = 80;
			public const int MaxButtonTemplateSize = 42;
			public const int DirectoryNameSize = 40;
			public const int DirectoryNumberSize = 24;
			public const int MaxMediaCapabilities = 18;
			public const int DeviceNameSize = 16;
			public const int UserDeviceDataSize = 1024;
			public const int DisplayNotifyTextSize = 32;
			public const int DisplayPromptTextSize = 32;
			public const int DisplayTextSize = 32;
			public const int KeySize = 16;
			public const int DateTemplateSize = 6;
			public const int MaxServers = 5;
			public const int MaxServiceUrlSize = 256;
			public const int MaxSoftkeyIndexes = 16;
			public const int MaxSoftkeySetDefinitions = 16;
			public const int MaxSoftkeyDefinitions = 32;
			public const int SoftkeyLabelSize = 16;
			public const int MaxVersionSize = 16;
		}

		#region Static utility methods

		/// <summary>
		/// Returns the SCCP message type of the message contained in the
		/// specified byte array.
		/// </summary>
		/// <param name="message">Byte array containing an SCCP
		/// message.</param>
		/// <returns>SCCP message type.</returns>
		public static Type TypeField(byte[] message)
		{
			return (SccpMessage.Type)BitConverter.ToInt32(message, 8);
		}

		/// <summary>
		/// Returns string version of IP address in byte array at given offset.
		/// </summary>
		/// <remarks>
		/// For example, 0x0a, 0x01, 0x0a, 0x34 would return "10.1.10.52".
		/// </remarks>
		/// <param name="buffer">Buffer containing IP address.</param>
		/// <param name="offset">Offset into the buffer where IP address
		/// starts.</param>
		/// <returns>"Dot" version of IP address as string.</returns>
		internal static string IPByteArrayToString(byte[] buffer, int offset)
		{
			long ipInt = BitConverter.ToInt32(buffer, offset);
			IPAddress ipAddress = new IPAddress(ipInt & 0xffffffff);
			string ip = ipAddress.ToString();

			return ip;
		}

		/// <summary>
		/// Copies byte-array version of IP address from string into provided
		/// byte array at given offset.
		/// </summary>
		/// <remarks>
		/// For example, "10.1.10.52" would copy 0x0a, 0x01, 0x0a, 0x34 into
		/// the byte array.
		/// </remarks>
		/// <param name="ipAddressString">"Dot" version of IP address as
		/// string.</param>
		/// <param name="buffer">Byte array into which the IP-address byte
		/// array is copied.</param>
		/// <param name="offset">Offset into the byte array where IP address is
		/// copied.</param>
		internal static void IPStringToByteArray(string ipAddressString,
			byte[] buffer, int offset)
		{
			IPAddress ipAddress = IPAddress.Parse(ipAddressString);

			byte[] ipAddressByteArray = ipAddress.GetAddressBytes();

			Array.Copy(ipAddressByteArray, 0, buffer, offset, 4);
		}

		/// <summary>
		/// Returns string from zero-delimited string in byte-array buffer,
		/// stripping off any trailing null characters.
		/// </summary>
		/// <param name="buffer">Buffer containing string.</param>
		/// <param name="offset">Offset into buffer where string is
		/// located.</param>
		/// <param name="length">Maximum length of string in byte
		/// array.</param>
		/// <returns>The string.</returns>
		internal static string StringByteArrayToString(byte[] buffer,
			long offset, long length)
		{
			string str = (new ASCIIEncoding()).GetString(buffer,
				(int)offset, (int)length);
			int index = str.IndexOf('\0');
			if (index >= 0)
			{
				str = str.Substring(0, index);
			}

			return str;
		}

		/// <summary>
		/// Return zero-delimited string in byte-array buffer from string,
		/// padded out with null characters.
		/// </summary>
		/// <param name="str">String.</param>
		/// <param name="length">Maximum length of string in byte
		/// array.</param>
		/// <returns>Buffer containing string.</returns>
		internal static byte[] StringToStringByteArray(string str, long length)
		{
			byte[] buffer = new byte[length];
			byte[] strBytes = (new ASCIIEncoding()).GetBytes(str);
			Array.Copy(strBytes, buffer, Math.Min(length, strBytes.Length));

			return buffer;
		}

		#endregion
	}

	#region Coder/Decoder/Encoder

	/// <summary>
	/// Represents the commonality between an SCCP encoder and decoder.
	/// </summary>
	public abstract class Coder
	{
		/// <summary>
		/// Stream by which a raw SCCP message is accessed via a byte-array
		/// back-store.
		/// </summary>
		protected MemoryStream stream;

		/// <summary>
		/// Advance coder by the specified number of bytes.
		/// </summary>
		/// <param name="n">Number of bytes to advance coder.</param>
		internal abstract void Advance(long i);
	}

	/// <summary>
	/// Represents an SCCP decoder.
	/// </summary>
	/// <remarks>
	/// This class provides the tools to convert a raw SCCP message into its
	/// corresponding internal respresentation in the form of member fields.
	/// </remarks>
	internal class Decoder : Coder
	{
		/// <summary>
		/// Constructs a Decoder and begins the decode process on the SCCP
		/// message in the specified byte array.
		/// </summary>
		internal Decoder(byte[] buffer)
		{
			stream = new MemoryStream(buffer, false);

			// Skip over the length, reserved, and type fields.
			uint dummy;
			Decode(out dummy);	// SCCP packet length. TBD - Don't decode past end.
			Decode(out dummy);	// Reserved field of 0. TBD - Verify?
			Decode(out dummy);	// Type (we've already looked at this).
		}

		/// <summary>
		/// Returns a byte array of the specified length from the stream.
		/// </summary>
		/// <param name="n">Number of bytes to return.</param>
		/// <returns>Byte array from stream.</returns>
		private byte[] Read(long n)
		{
			byte[] buffer = new byte[n];
			stream.Read(buffer, 0, buffer.Length);
			return buffer;
		}

		/// <summary>
		/// Advances decoder by the specified number of bytes.
		/// </summary>
		/// <remarks>
		/// TBD - optimize by merely moving pointer(s), e.g., MemoryStream
		/// position.
		/// </remarks>
		/// <param name="n">Number of bytes to advance decoder.</param>
		internal override void Advance(long i)
		{
			Read(i);
		}

		/// <summary>
		/// Property whose value is the value of the number of bytes remaining
		/// to be decoded.
		/// </summary>
		internal long Remaining { get { return stream.Length - stream.Position; } }

		/// <summary>
		/// Property whose value is the value of whether there is more data to
		/// decode.
		/// </summary>
		internal bool More { get { return Remaining > 0; } }

		/// <summary>
		/// Decodes a uint as a bool.
		/// </summary>
		/// <param name="b">Decoded bool.</param>
		/// <param name="trueValue">Value to compare raw value against to
		/// determine whether bool set to true.</param>
		internal void Decode(out bool b, uint trueValue)
		{
			b = Decode() == trueValue;
		}

		/// <summary>
		/// Decodes a byte (8-bit unsigned integral).
		/// </summary>
		/// <param name="n">Decoded byte.</param>
		internal void Decode(out byte n)
		{
			n = Read(1)[0];
		}

		/// <summary>
		/// Decodes a uint (32-bit unsigned integral).
		/// </summary>
		/// <param name="n">Decoded uint.</param>
		internal void Decode(out uint n)
		{
			// Force alignment on the next 32-bit boundary.
			long alignmentAdjustment = stream.Position % 4;
			if (alignmentAdjustment > 0)
			{
				Advance(alignmentAdjustment);
			}

			n = BitConverter.ToUInt32(Read(SccpMessage.Const.UintSize), 0);
		}

		/// <summary>
		/// Decodes a ushort (16-bit unsigned integral).
		/// </summary>
		/// <param name="n">Decoded ushort.</param>
		internal void Decode(out ushort n)
		{
			// Force alignment on the next 16-bit boundary.
			long alignmentAdjustment = stream.Position % 2;
			if (alignmentAdjustment > 0)
			{
				Advance(alignmentAdjustment);
			}

			n = BitConverter.ToUInt16(Read(SccpMessage.Const.UshortSize), 0);
		}

		/// <summary>
		/// Decodes an implied uint (32-bit unsigned integral).
		/// </summary>
		/// <remarks>Useful for decoding enums because return value can be cast
		/// but an out parameter cannot.</remarks>
		/// <returns>Decoded implied uint.</returns>
		internal uint Decode()
		{
			uint n;

			Decode(out n);

			return n;
		}

		/// <summary>
		/// Decodes a string.
		/// </summary>
		/// <param name="s">Decoded string.</param>
		/// <param name="maxLength">Number of bytes within which zero-delimited
		/// string is located. Actual string may be smaller.</param>
		internal void Decode(out string s, long maxLength)
		{
			s = SccpMessage.StringByteArrayToString(Read(maxLength), 0,
				maxLength);
		}

		/// <summary>
		/// Decodes a byte array.
		/// </summary>
		/// <param name="buffer">Decoded byte array.</param>
		/// <param name="length">Length of "real" data in the maximum
		/// extent.</param>
		/// <param name="maxLength">Number of bytes within which byte array is
		/// located. Actual byte array may be smaller.</param>
		internal void Decode(out byte[] buffer, long length, long maxLength)
		{
			byte[] maxBuffer = Read(maxLength);
			buffer = new byte[length];
			Array.Copy(maxBuffer, buffer, Math.Min(buffer.Length,
				maxBuffer.Length));
		}

		/// <summary>
		/// Decodes a ushort array.
		/// </summary>
		/// <param name="buffer">Decoded ushort array.</param>
		/// <param name="length">Number of ushorts in array.</param>
		/// <param name="maxLength">Number of ushorts within which ushort array
		/// is located. Actual ushort array may be smaller.</param>
		internal void Decode(out ushort[] array, long length, long maxLength)
		{
			array = new ushort[length];
			for (uint i = 0; i < length; ++i)
			{
				Decode(out array[i]);
			}
			Advance(SccpMessage.Const.UshortSize * (maxLength - length));
		}

		/// <summary>
		/// Decodes a uint as an IP address.
		/// </summary>
		/// <param name="ipAddress">Decoded IP address.</param>
		internal void Decode(out IPAddress ipAddress)
		{
			uint ipAddress_;
			Decode(out ipAddress_);

			ipAddress = new IPAddress((long)ipAddress_);
		}

		/// <summary>
		/// Decodes an IPEndPoint address (IP address and port).
		/// </summary>
		/// <param name="ipAddress">Decoded address.</param>
		internal void Decode(out IPEndPoint address)
		{
			IPAddress ipAddress;
			Decode(out ipAddress);

			uint port;
			Decode(out port);

			address = new IPEndPoint(ipAddress, (int)port);
		}

		/// <summary>
		/// Decodes a sid (a.k.a. client identifier and device name).
		/// </summary>
		/// <param name="sid">Decoded sid.</param>
		internal void Decode(out Sid sid)
		{
			sid = new Sid();
			sid.Decode(this);
		}

		/// <summary>
		/// Decodes an SccpMessageStruct subclass.
		/// </summary>
		/// <param name="obj">Decoded message structure.</param>
		/// <param name="type">Specific SccpMessageStruct subclass, e.g.,
		/// typeof(SessionType).</param>
		internal void Decode(SccpMessageStruct obj, Type type) 
		{
			ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
			if (ci == null)
			{
				Debug.Fail(type.ToString() + " missing a default constructor");
			}
			obj = ci.Invoke(null) as SccpMessageStruct;
			obj.Decode(this);
		}
	}

	/// <summary>
	/// Represents an SCCP encoder.
	/// </summary>
	/// <remarks>
	/// This class provides the tools to convert the internal respresentation
	/// of an SCCP message into a raw SCCP message.
	/// </remarks>
	internal class Encoder : Coder
	{
		/// <summary>
		/// Constructs an Encoder and begins the encode process of the SCCP
		/// message in the internal MemoryStream.
		/// </summary>
		internal Encoder(SccpMessage.Type type)
		{
			stream = new MemoryStream();

			Encode((uint)0);	// Place-holder for SCCP packet length.
			Encode((uint)0);	// Reserved field of 0.
			Encode((uint)type);
		}

		/// <summary>
		/// Encodes a byte (8-bit unsigned integral).
		/// </summary>
		/// <param name="n">Byte to encode.</param>
		internal void Encode(byte n)
		{
			stream.WriteByte(n);
		}

		/// <summary>
		/// Constants referenced within this class.
		/// </summary>
		private abstract class Const
		{
			public const int HeaderSize = 4 + 4;	// Length & reserved fields
		}

		/// <summary>
		/// Advances encoder by the specified number of bytes, writing 0 into
		/// each byte.
		/// </summary>
		/// <remarks>
		/// TBD - optimize by merely moving pointer(s), e.g., MemoryStream
		/// position.
		/// </remarks>
		/// <param name="n">Number of bytes to advance encoder.</param>
		internal override void Advance(long n)
		{
			while (n-- > 0)
			{
				stream.WriteByte(0);
			}
		}

		/// <summary>
		/// Encodes a ushort (16-bit unsigned integral).
		/// </summary>
		/// <param name="n">Ushort to encode.</param>
		internal void Encode(ushort n)
		{
			// Force alignment on the next 16-bit boundary.
			long alignmentAdjustment = stream.Position % 2;
			if (alignmentAdjustment > 0)
			{
				Advance(alignmentAdjustment);
			}

			stream.Write(BitConverter.GetBytes(n), 0,
				SccpMessage.Const.UshortSize);
		}

		/// <summary>
		/// Encodes a uint (32-bit unsigned integral).
		/// </summary>
		/// <param name="n">Uint to encode.</param>
		internal void Encode(uint n)
		{
			// Force alignment on the next 32-bit boundary.
			long alignmentAdjustment = stream.Position % 4;
			if (alignmentAdjustment > 0)
			{
				Advance(alignmentAdjustment);
			}

			stream.Write(BitConverter.GetBytes(n), 0,
				SccpMessage.Const.UintSize);
		}

		/// <summary>
		/// Encodes a string.
		/// </summary>
		/// <param name="str">String to encode.</param>
		internal void Encode(string str)
		{
			Encode(str, str.Length);
		}

		/// <summary>
		/// Encodes a string, padded with null characters.
		/// </summary>
		/// <param name="str">String to encode.</param>
		/// <param name="maxLength">Number of bytes within which zero-delimited
		/// string is located. Actual string may be smaller.</param>
		internal void Encode(string str, long maxLength)
		{
			if (str != null)
			{
				byte[] buf =
					SccpMessage.StringToStringByteArray(str, maxLength);
				stream.Write(buf, 0, (int)maxLength);
			}
			else
			{
				Advance(maxLength);
			}
		}

		/// <summary>
		/// Encodes a byte array.
		/// </summary>
		/// <param name="buffer">Byte array to encode.</param>
		internal void Encode(byte[] buffer)
		{
			Encode(buffer, buffer.Length);
		}

		/// <summary>
		/// Encodes a byte array, padded with zero bytes.
		/// </summary>
		/// <param name="buffer">Byte array to encode.</param>
		/// <param name="maxLength">Number of bytes within which byte array is
		/// located. Actual byte array may be smaller.</param>
		internal void Encode(byte[] buffer, long maxLength)
		{
			int bufferLength;

			if (buffer != null)
			{
				bufferLength = buffer.Length;
				stream.Write(buffer, 0, bufferLength);
			}
			else
			{
				bufferLength = 0;
			}

			Advance(maxLength - bufferLength);
		}

		/// <summary>
		/// Encodes a ushort array, padded with zero ushorts.
		/// </summary>
		/// <param name="buffer">Ushort array to encode.</param>
		internal void Encode(ushort[] buffer)
		{
			Encode(buffer, buffer.Length);
		}

		/// <summary>
		/// Encodes a ushort array.
		/// </summary>
		/// <param name="buffer">Ushort array to encode.</param>
		/// <param name="maxLength">Number of ushorts within which ushort array
		/// is located. Actual ushort array may be smaller.</param>
		internal void Encode(ushort[] buffer, long maxLength)
		{
			int bufferLength;

			if (buffer != null)
			{
				bufferLength = buffer.Length;
				foreach(ushort n in buffer)
				{
					Encode(n);
				}
			}
			else
			{
				bufferLength = 0;
			}

			Advance(maxLength - bufferLength);
		}

		/// <summary>
		/// Encodes an IP address as a uint.
		/// </summary>
		/// <param name="ipAddress">IP address to encode, or null.</param>
		internal void Encode(IPAddress ipAddress)
		{
			if (ipAddress != null)
			{
				Encode(BitConverter.ToUInt32(ipAddress.GetAddressBytes(), 0));
			}
			else
			{
				Advance(SccpMessage.Const.UintSize);
			}
		}

		/// <summary>
		/// Encodes an IPEndPoint address (IP address and port).
		/// </summary>
		/// <param name="ipAddress">Address to encode, or null.</param>
		internal void Encode(IPEndPoint address)
		{
			if (address != null)
			{
				Encode(address.Address);
				Encode((uint)address.Port);
			}
			else
			{
				Advance(SccpMessage.Const.UintSize +
					SccpMessage.Const.UintSize);
			}
		}

		/// <summary>
		/// Encodes a sid (a.k.a. client identifier and device name).
		/// </summary>
		/// <param name="sid">sid to encode, or null.</param>
		internal void Encode(Sid sid)
		{
			if (sid != null)
			{
				sid.Encode(this);
			}
			else
			{
				new Sid().Advance(this);
			}
		}

		/// <summary>
		/// Encodes an SccpMessageStruct subclass.
		/// </summary>
		/// <param name="obj">SccpMessage structure to encode.</param>
		/// <param name="type">Specific SccpMessageStruct subclass, e.g.,
		/// typeof(SessionType).</param>
		internal void Encode(SccpMessageStruct obj, Type type) 
		{
			if(obj != null)
			{
				obj.Encode(this);
			}
			else
			{
				// Instantiate message structure just to advance the encoding
				// by the correct amount.
				ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
				if (ci == null)
				{
					Debug.Fail(
						type.ToString() + " missing a default constructor");
				}
				obj = ci.Invoke(null) as SccpMessageStruct;
				obj.Advance(this);
			}
		}

		/// <summary>
		/// Returns a byte array containing the endcoding.
		/// </summary>
		/// <remarks>This method is typically invoked once there is nothing
		/// else to encode, but encoding could continue and this method invoked
		/// again, ad infinitum.</remarks>
		/// <returns>Encoding.</returns>
		internal byte[] Encoding()
		{
			byte[] buffer = new byte[stream.Position];
			stream.Position = 0;
			stream.Read(buffer, 0, buffer.Length);

			Debug.Assert(stream.Position >= Const.HeaderSize,
				"SccpStack: missing header");

			byte[] packetLengthBytes = BitConverter.GetBytes(
				(uint)(stream.Position - Const.HeaderSize));

			Array.Copy(packetLengthBytes, buffer, packetLengthBytes.Length);

			return buffer;
		}
	}

	#endregion

	/// <summary>
	/// Represents a structure (a group of one or more fields)
	/// within an SCCP message.
	/// </summary>
	/// <remarks>
	/// This class provides controls for encoding and decoding a message
	/// structure.
	/// </remarks>
	public abstract class SccpMessageStruct
	{
		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal abstract void Decode(Decoder decoder);

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal abstract void Encode(Encoder encoder);

		/// <summary>
		/// Advances coder by the size of this structure.
		/// </summary>
		/// <param name="coder">Coder to advance.</param>
		internal void Advance(Coder coder)
		{
			Advance(coder, 1);
		}

		/// <summary>
		/// Advances coder by the size of multiple instances of this structure.
		/// </summary>
		/// <param name="coder">Coder to advance.</param>
		/// <param name="n">The number of instances of this structure to
		/// advance the coder.</param>
		internal void Advance(Coder coder, long n)
		{
			coder.Advance(SizeOf() * n);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal abstract long SizeOf();
	}

	#region Common classes/enums contained by SccpMessage subclasses

	/// <summary>
	/// Different kinds of media.
	/// </summary>
	public enum PayloadType
	{
		G711Alaw64k = 2,
		G711Alaw56k = 3,
		G711Ulaw64k = 4,
		G711Ulaw56k = 5,
		G72264k = 6,
		G72256k = 7,
		G72248k = 8,
		G7231 = 9,
		G728 = 10,
		G729 = 11,
		G729AnnexA = 12,
		G729AnnexB = 15,
		G729AnnexAwAnnexB = 16,
		GsmFullRate = 18,
		GsmHalfRate = 19,
		GsmENhancedFullRate = 20,
		WideBand256k = 25,
		Data64 = 32,
		Data56 = 33,
		Gsm = 80,
		G72632k = 82,
		G72624k = 83,
		G72616k = 84,
		H261 = 100,
		H263 = 101,
		T120 = 105,
		H224 = 106,
		Xv150Mr = 111,
		Rfc2833DynPayload = 257,
	}

	/// <summary>
	/// G.723.1 has these two bit rates.
	/// </summary>
	/// <remarks>
	/// I thought about making this something else, e.g., boolean or float, but
	/// decided to leave as an enum.
	/// </remarks>
	public enum G723BitRate
	{
		NotApplicable,
		_5_3khz = 1,
		_6_4khz = 2,
	}

	/// <summary>
	/// Represents an SCCP Client IDentifier, a.k.a., sid or device name.
	/// </summary>
	public class Sid : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a Sid based on the MAC address of the current host's
		/// first NIC.
		/// </summary>
		public Sid() : this(GetSid()) { }

		/// <summary>
		/// Constructs a Sid with the reserved field set to 0 and the instance
		/// set to 1 which it should normally be.
		/// </summary>
		/// <remarks>
		/// This is the constructor that one should typically use.
		/// </remarks>
		/// <param name="deviceName"></param>
		public Sid(string deviceName) : this(deviceName, 1) { }

		/// <summary>
		/// Constructs a Sid with the reserved field always set to 0, which it
		/// should always be.
		/// </summary>
		/// <param name="deviceName"></param>
		/// <param name="instance"></param>
		public Sid(string deviceName, uint instance) : this(deviceName, 0, instance) { }

		/// <summary>
		/// Constructs a Sid.
		/// </summary>
		/// <param name="deviceName">Uniquely identifies a client.</param>
		/// <param name="reserved">Reserved field; set to 0.</param>
		/// <param name="instance">Instance of the client. Normally set to
		/// 1.</param>
		public Sid(string deviceName, uint reserved, uint instance)
		{
			this.deviceName = deviceName;
			this.reserved = reserved;
			this.instance = instance;
		}

		/// <summary>
		/// Uniquely identifies a client.
		/// </summary>
		public string deviceName;

		/// <summary>
		/// Reserved field; always set to 0.
		/// </summary>
		public uint reserved;

		/// <summary>
		/// Instance of the client. Normally set to 1.
		/// </summary>
		public uint instance;

		/// <summary>
		/// Returns SID based on MAC address for this host.
		/// </summary>
		/// <returns>Sid.</returns>
		public static string GetSid()
		{
			return "SEP" + SccpStack.GetMacAddress();
		}

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out deviceName, SccpMessage.Const.DeviceNameSize);
			decoder.Decode(out reserved);
			decoder.Decode(out instance);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(deviceName, SccpMessage.Const.DeviceNameSize);
			encoder.Encode(reserved);
			encoder.Encode(instance);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return SccpMessage.Const.DeviceNameSize +
				Marshal.SizeOf(reserved) +
				Marshal.SizeOf(instance);
		}
	}

	/// <summary>
	/// Statistics processing mode.
	/// </summary>
	public enum StatsProcessing
	{
		Clear = 0,
		NoClear = 1,
	}

	/// <summary>
	/// Represents user-related data.
	/// </summary>
	public class UserAndDeviceData : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a UserAndDeviceData for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public UserAndDeviceData() : this(0, 1, 0, 0, null) { }

		/// <summary>
		/// Constructs a UserAndDeviceData object.
		/// </summary>
		/// <param name="applicationId">Set to 0 for call processing; else set
		/// to the application id.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="transactionId">Differentiates entities in the same
		/// application.</param>
		/// <param name="data">User-related data.</param>
		public UserAndDeviceData(uint applicationId, uint lineNumber,
			uint callReference, uint transactionId, byte[] data)
		{
			this.applicationId = applicationId;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
			this.transactionId = transactionId;
			this.data = data;
		}

		/// <summary>
		/// Set to 0 for call processing; else set to the application id.
		/// </summary>
		public uint applicationId;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Differentiates entities in the same application.
		/// </summary>
		public uint transactionId;

		/// <summary>
		/// User-related data.
		/// </summary>
		public byte[] data;

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			uint dataLength;

			decoder.Decode(out applicationId);
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
			decoder.Decode(out transactionId);
			decoder.Decode(out dataLength);
			decoder.Decode(out data, dataLength,
				SccpMessage.Const.UserDeviceDataSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			uint dataLength = (uint)(data == null ? 0 : data.Length);

			encoder.Encode(applicationId);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
			encoder.Encode(transactionId);
			encoder.Encode(dataLength);
			encoder.Encode(data, SccpMessage.Const.UserDeviceDataSize);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return Marshal.SizeOf(applicationId) +
				Marshal.SizeOf(lineNumber) +
				Marshal.SizeOf(callReference) +
				Marshal.SizeOf(transactionId) +
				SccpMessage.Const.UserDeviceDataSize;
		}
	}

    /// <summary>
    /// Represents user-related data.
    /// </summary>
    public class UserAndDeviceDataVersion1 : SccpMessageStruct
    {
        /// <summary>
        /// Constructs a UserAndDeviceDataVersion1 for assigning fields later.
        /// Defaults to line number 1, the first one.
        /// </summary>
        public UserAndDeviceDataVersion1() : this(0, 1, 0, 0, 0, 0, 0, 0, 0, null) { }

        /// <summary>
        /// Constructs a UserAndDeviceData object.
        /// </summary>
        /// <param name="applicationId">Set to 0 for call processing; else set
        /// to the application id.</param>
        /// <param name="lineNumber">Line number.</param>
        /// <param name="callReference">Uniquely identifies calls on the same
        /// device.</param>
        /// <param name="transactionId">Differentiates entities in the same
        /// application.</param>
        /// <param name="data">User-related data.</param>
        public UserAndDeviceDataVersion1(uint applicationId, uint lineNumber,
            uint callReference, uint transactionId, uint seqFlag,
            uint displayPriority, uint conferenceId, uint appInstanceId,
            uint routingId, byte[] data)
        {
            this.applicationId = applicationId;
            this.lineNumber = lineNumber;
            this.callReference = callReference;
            this.transactionId = transactionId;
            this.sequenceFlag = seqFlag;
            this.displayPriority = displayPriority;
            this.conferenceId = conferenceId;
            this.appInstanceId = appInstanceId;
            this.routingId = routingId;
            this.data = data;
        }

        /// <summary>
        /// Set to 0 for call processing; else set to the application id.
        /// </summary>
        public uint applicationId;

        /// <summary>
        /// Line number.
        /// </summary>
        public uint lineNumber;

        /// <summary>
        /// Uniquely identifies calls on the same device.
        /// </summary>
        public uint callReference;

        /// <summary>
        /// Differentiates entities in the same application.
        /// </summary>
        public uint transactionId;

        public uint sequenceFlag;

        public uint displayPriority;

        public uint conferenceId;

        public uint appInstanceId;

        public uint routingId;

        /// <summary>
        /// User-related data.
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Decodes this structure from raw message to internal member fields.
        /// </summary>
        /// <param name="decoder">Keeps track of decoding progress.</param>
        internal override void Decode(Decoder decoder)
        {
            uint dataLength;

            decoder.Decode(out applicationId);
            decoder.Decode(out lineNumber);
            decoder.Decode(out callReference);
            decoder.Decode(out transactionId);
            decoder.Decode(out sequenceFlag);
            decoder.Decode(out displayPriority);
            decoder.Decode(out conferenceId);
            decoder.Decode(out appInstanceId);
            decoder.Decode(out routingId);
            decoder.Decode(out dataLength);
            decoder.Decode(out data, dataLength,
                SccpMessage.Const.UserDeviceDataSize);
        }

        /// <summary>
        /// Encodes from internal member fields to raw message.
        /// </summary>
        /// <param name="encoder">Keeps track of encoding progress.</param>
        internal override void Encode(Encoder encoder)
        {
            uint dataLength = (uint) (data == null ? 0 : data.Length);

            encoder.Encode(applicationId);
            encoder.Encode(lineNumber);
            encoder.Encode(callReference);
            encoder.Encode(transactionId);
            encoder.Encode(sequenceFlag);
            encoder.Encode(displayPriority);
            encoder.Encode(conferenceId);
            encoder.Encode(appInstanceId);
            encoder.Encode(routingId);
            encoder.Encode(dataLength);
            encoder.Encode(data, SccpMessage.Const.UserDeviceDataSize);
        }

        /// <summary>
        /// Returns the size of this aggregate in an actual SCCP message.
        /// </summary>
        /// <returns>Aggregate size.</returns>
        internal override long SizeOf()
        {
            return Marshal.SizeOf(applicationId) +
				Marshal.SizeOf(lineNumber) +
				Marshal.SizeOf(callReference) +
				Marshal.SizeOf(transactionId) +
                Marshal.SizeOf(sequenceFlag) + 
                Marshal.SizeOf(displayPriority) + 
                Marshal.SizeOf(conferenceId) + 
                Marshal.SizeOf(appInstanceId) + 
                Marshal.SizeOf(routingId) + 
				SccpMessage.Const.UserDeviceDataSize;
        }
    }
    
    /// <summary>
	/// Represents an encryption algorithm and key material.
	/// </summary>
	/// <remarks>The internal representation here is, IMO, organized better
	/// than the same information in the SCCP message.</remarks>
	public class MediaEncryptionKey : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a MediaEncryptionKey for assigning fields later.
		/// </summary>
		public MediaEncryptionKey() : this(MediaEncryptionKey.Algorithm.None, null) { }

		/// <summary>
		/// Constructs a MediaEncryptionKey object.
		/// </summary>
		/// <param name="algorithm">Encryption algorithm.</param>
		/// <param name="material">Key material.</param>
		public MediaEncryptionKey(Algorithm algorithm,  KeyMaterial material)
		{
			this.algorithm = algorithm;
			this.material = material;
		}

		/// <summary>
		/// Encryption algorithm.
		/// </summary>
		public enum Algorithm
		{
			None,
			Aes128Counter,
		}

		/// <summary>
		/// Represents encryption key material.
		/// </summary>
		public class KeyMaterial
		{
			/// <summary>
			/// Constructs an "empty" KeyMaterial object to be filled in later.
			/// </summary>
			public KeyMaterial() : this(null, null) { }

			/// <summary>
			/// Constructs a KeyMaterial object.
			/// </summary>
			/// <param name="key">Encryption key.</param>
			/// <param name="salt">Salt data (used to further scramble the
			/// encryption).</param>
			public KeyMaterial(byte[] key, byte[] salt)
			{
				this.key = key;
				this.salt = salt;
			}

			/// <summary>
			/// Encryption key.
			/// </summary>
			public byte[] key;

			/// <summary>
			/// Salt data (used to further scramble the encryption).
			/// </summary>
			public byte[] salt;
		}

		/// <summary>
		/// Encryption algorithm.
		/// </summary>
		public Algorithm algorithm;

		/// <summary>
		/// Key material.
		/// </summary>
		public KeyMaterial material;

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			ushort keyLen;
			ushort saltLen;

			algorithm = (Algorithm)decoder.Decode();
			decoder.Decode(out keyLen);
			decoder.Decode(out saltLen);
			material = new KeyMaterial();
			decoder.Decode(out material.key, keyLen,
				SccpMessage.Const.KeySize);
			decoder.Decode(out material.salt, saltLen,
				SccpMessage.Const.KeySize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			ushort keyLen;
			ushort saltLen;

			if (material == null)
			{
				keyLen = 0;
				saltLen = 0;
			}
			else
			{
				keyLen = (ushort)(material.key == null ?
					0 : material.key.Length);
				saltLen = (ushort)(material.salt == null ?
					0 : material.salt.Length);
			}

			encoder.Encode((uint)algorithm);
			encoder.Encode(keyLen);
			encoder.Encode(saltLen);

			if (material != null)
			{
				encoder.Encode(material.key, SccpMessage.Const.KeySize);
			}
			else
			{
				encoder.Advance(SccpMessage.Const.KeySize);
			}

			if (material != null)
			{
				encoder.Encode(material.salt, SccpMessage.Const.KeySize);
			}
			else
			{
				encoder.Advance(SccpMessage.Const.KeySize);
			}
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return SccpMessage.Const.EnumSize +
				SccpMessage.Const.UshortSize +
				SccpMessage.Const.UshortSize +
				SccpMessage.Const.KeySize +
				SccpMessage.Const.KeySize;
		}
	}

	/// <summary>
	/// Device type as reported to CallManager by client.
	/// </summary>
	public enum DeviceType
	{
		Station30spplus = 1,
		Station12spplus = 2,
		Station12sp = 3,
		Station12 = 4,
		Station30vip = 5,
		StationTelecaster = 6,
		StationTelecasterMgr = 7,
		StationTelecasterBus = 8,
		StationPolycom = 9,
		Station130spplus = 20,
		StationPhoneApplication = 21,
		AnalogAccess = 30,
		DigitalAccessTitan1 = 40,
		DigitalAccessTitan2 = 42,
		DigitalAccessLennon = 43,
		AnalogAccessElvis = 47,
		ConferenceBridge = 50,
		ConferenceBridgeYoko = 51,
		H225 = 60,
		H323Phone = 61,
		H323Trunk = 62,
		MusicOnHold = 70,
		Pilot = 71,
		TapiPort = 72,
		TapiRoutePoint = 73,
		VoiceInbox = 80,
		VoiceInboxAdmin = 81,
		LineAnnunciator = 82,
		SoftwareMtpDixieland = 83,
		CiscoMediaServer = 84,
		RouteList = 90,
		LoadSimulator = 100,
		Ipste1 = 107,
		MediaTerminationPoint = 110,
		MediaTerminationPointYoko = 111,
		MediaTerminationPointDixieland = 112,
		MediaTerminationPointSummit = 113,
		MgcpStation = 120,
		MgcpTrunk = 121,
		RasProxy = 122,
		Trunk = 125,
		Annunciator = 126,
		MonitorBridge = 127,
		Recorder = 128,
		MonitorBridgeYoko = 129,
		UnknownMgcpGateway = 254,
		Ipste2 = 30035,
	}

	/// <summary>
	/// Protocol versions.
	/// </summary>
	/// <remarks>
	/// These enumerated values apparently have no relation to the numbers
	/// normally used to describe SCCP protocol versions. For example, the
	/// Parche enumerated value is 5 but the normal version number is 4.X.
	/// </remarks>
	public enum ProtocolVersion
	{
		Sp30 = 1,
		Bravo = 2,
		Hawkbill = 3,
		Seaview = 4,
		Parche = 5,
	}

	/// <summary>
	/// Stimuli, a.k.a., events.
	/// </summary>
	public enum DeviceStimulus
	{
		LastNumberRedial = 0x01,
		SpeedDial = 0x02,
		Hold = 0x03,
		Transfer = 0x04,
		ForwardAll = 0x05,
		ForwardBusy = 0x06,
		ForwardNoAnswer = 0x07,
		Display = 0x08,
		Line = 0x09,
		T120Chat = 0x0a,
		T120Whiteboard = 0x0b,
		T120ApplicationSharing = 0x0c,
		T120FileTransfer = 0x0d,
		Video = 0x0e,
		Voicemail = 0x0f,
		AnswerRelease = 0x10,
		AutoAnswer = 0x11,
		Select = 0x12,
		Privacy = 0x13,
		ServiceUrl = 0x14,
		MaliciousCall = 0x1b,
		GenericAppB1 = 0x21,
		GenericAppB2 = 0x22,
		GenericAppB3 = 0x23,
		GenericAppB4 = 0x24,
		GenericAppB5 = 0x25,
	}

	/// <summary>
	/// Represents extra qualifiers to incoming payload type.
	/// </summary>
	public class MediaQualifierIncoming : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a MediaQualifierIncoming object with default field
		/// settings.
		/// </summary>
		public MediaQualifierIncoming() : this(false, G723BitRate.NotApplicable) { }

		/// <summary>
		/// Constructs a MediaQualifierIncoming object.
		/// </summary>
		/// <param name="echoCancellation">Whether echo cancellation is
		/// enabled.</param>
		/// <param name="g723BitRate">Bit rate. Only used with G.723.1.</param>
		public MediaQualifierIncoming(bool echoCancellation,
			G723BitRate g723BitRate)
		{
			this.echoCancellation = echoCancellation;
			this.g723BitRate = g723BitRate;
		}

		/// <summary>
		/// Whether echo cancellation is enabled.
		/// </summary>
		public bool echoCancellation;

		/// <summary>
		/// Bit rate. Only used with G.723.1.
		/// </summary>
		public G723BitRate g723BitRate;

		/// <summary>
		/// Used to translate between SCCP message field and our bool.
		/// </summary>
		private enum EchoCancellation
		{
			Off = 0,
			On = 1,
		}

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out echoCancellation, (uint)EchoCancellation.On);
			g723BitRate = (G723BitRate)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)(echoCancellation ?
				EchoCancellation.On : EchoCancellation.Off));
			encoder.Encode((uint)g723BitRate);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return SccpMessage.Const.UintSize +
				Marshal.SizeOf(g723BitRate);
		}
	}

	/// <summary>
	/// Represents extra qualifiers to outgoing payload type.
	/// </summary>
	public class MediaQualifierOutgoing : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a MediaQualifierOutgoing object with default field
		/// settings.
		/// </summary>
		public MediaQualifierOutgoing() : this(0, false, 1, G723BitRate.NotApplicable) { }

		/// <summary>
		/// Constructs a MediaQualifierOutgoing object.
		/// </summary>
		/// <param name="precedence">Precedence of the RTP stream.</param>
		/// <param name="silenceSuppression">Whether silence suppression is
		/// enabled.</param>
		/// <param name="maxFramesPerPacket">Most frames allowed in an RTP
		/// packet.</param>
		/// <param name="g723BitRate">Bit rate. Only used with G.723.1.</param>
		public MediaQualifierOutgoing(uint precedence, bool silenceSuppression,
			ushort maxFramesPerPacket, G723BitRate g723BitRate)
		{
			this.precedence = precedence;
			this.silenceSuppression = silenceSuppression;
			this.maxFramesPerPacket = maxFramesPerPacket;
			this.g723BitRate = g723BitRate;
		}

		/// <summary>
		/// Precedence of the RTP stream.
		/// </summary>
		public uint precedence;

		/// <summary>
		/// Whether silence suppression is enabled.
		/// </summary>
		public bool silenceSuppression;

		/// <summary>
		/// Most frames allowed in an RTP packet.
		/// </summary>
		public ushort maxFramesPerPacket;

		/// <summary>
		/// Bit rate. Only used with G.723.1.
		/// </summary>
		public G723BitRate g723BitRate;

		/// <summary>
		/// Used to translate between SCCP message field and our bool.
		/// </summary>
		private enum SilenceSuppression
		{
			Off = 0,
			On = 1,
		}

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out precedence);
			decoder.Decode(out silenceSuppression,
				(uint)SilenceSuppression.On);
			decoder.Decode(out maxFramesPerPacket);
			g723BitRate = (G723BitRate)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(precedence);
			encoder.Encode((uint)(silenceSuppression ?
				SilenceSuppression.On : SilenceSuppression.Off));
			encoder.Encode(maxFramesPerPacket);
			encoder.Encode((uint)g723BitRate);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return Marshal.SizeOf(precedence) +
				SccpMessage.Const.UintSize +
				Marshal.SizeOf(maxFramesPerPacket) +
				SccpMessage.Const.EnumSize;
		}
	}

	/// <summary>
	/// Represents the type of an extra-SCCP session.
	/// </summary>
	public class SessionType : SccpMessageStruct
	{
		/// <summary>
		/// Constructs a SessionType object with all capabilities set to false.
		/// </summary>
		public SessionType() : this(false, false, false, false, false) { }

		/// <summary>
		/// Constructs a SessionType object.
		/// </summary>
		/// <param name="chat">Whether this is a chat session.</param>
		/// <param name="whiteboard">Whether this is a shared-whiteboard
		/// session.</param>
		/// <param name="applicationSharing">Whether this is an
		/// application-sharing session.</param>
		/// <param name="fileTransfer">Whether this is a file-transfer
		/// session.</param>
		/// <param name="video">Whether this is a video session.</param>
		public SessionType(bool chat, bool whiteboard, bool applicationSharing,
			bool fileTransfer, bool video)
		{
			this.chat = chat;
			this.whiteboard = whiteboard;
			this.applicationSharing = applicationSharing;
			this.fileTransfer = fileTransfer;
			this.video = video;
		}

		/// <summary>
		/// Whether this is a chat session.
		/// </summary>
		public bool chat;

		/// <summary>
		/// Whether this is a shared-whiteboard session.
		/// </summary>
		public bool whiteboard;

		/// <summary>
		/// Whether this is an application-sharing session.
		/// </summary>
		public bool applicationSharing;

		/// <summary>
		/// Whether this is a file-transfer session.
		/// </summary>
		public bool fileTransfer;

		/// <summary>
		/// Whether this is a video session.
		/// </summary>
		public bool video;

		/// <summary>
		/// Private enumeration used to translate between encoding in SCCP
		/// message and the SessionType booleans in our message object.
		/// </summary>
		private enum SessionType_
		{
			Chat                = 0x01,
			Whiteboard          = 0x02,
			ApplicationSharing  = 0x04,
			FileTransfer        = 0x08,
			Video               = 0x10,
		}

		/// <summary>
		/// Decodes this structure from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			uint sessionType;

			decoder.Decode(out sessionType);

			// Set booleans according to whether corresponding bits are set.
			chat = (sessionType & (uint)SessionType_.Chat) != 0;
			whiteboard = (sessionType & (uint)SessionType_.Whiteboard) != 0;
			applicationSharing =
				(sessionType & (uint)SessionType_.ApplicationSharing) != 0;
			fileTransfer =
				(sessionType & (uint)SessionType_.FileTransfer) != 0;
			video = (sessionType & (uint)SessionType_.Video) != 0;
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			uint sessionType = 0;

			// Set bits according to whether corresponding booleans are set.
			if (chat)
			{
				sessionType |= (uint)SessionType_.Chat;
			}
			if (whiteboard)
			{
				sessionType |= (uint)SessionType_.Whiteboard;
			}
			if (applicationSharing)
			{
				sessionType |= (uint)SessionType_.ApplicationSharing;
			}
			if (fileTransfer)
			{
				sessionType |= (uint)SessionType_.FileTransfer;
			}
			if (video)
			{
				sessionType |= (uint)SessionType_.Video;
			}

			encoder.Encode(sessionType);
		}

		/// <summary>
		/// Returns the size of this aggregate in an actual SCCP message.
		/// </summary>
		/// <returns>Aggregate size.</returns>
		internal override long SizeOf()
		{
			return SccpMessage.Const.UintSize;
		}
	}

	/// <summary>
	/// Softkey definition.
	/// </summary>
	public class SoftkeyDefinition
	{
		/// <summary>
		/// Constructs a SoftkeyDefinition for assigning fields later.
		/// </summary>
		public SoftkeyDefinition() : this(null, 0) { }

		/// <summary>
		/// Constructs a SoftkeyDefinition with label and event as parameters.
		/// </summary>
		/// <param name="label">Label, such as "Redial".</param>
		/// <param name="event_">Event number associated with this
		/// event.</param>
		public SoftkeyDefinition(string label, uint event_)
		{
			this.label = label;
			this.event_ = event_;	// Can use SoftkeyEventType
		}

		/// <summary>
		/// Label, such as "Redial".
		/// </summary>
		public string label;

		/// <summary>
		/// Event number associated with this event.
		/// </summary>
		public uint event_;
	}

	/// <summary>
	/// Various type of tones that the phone can generate.
	/// </summary>
	public enum Tone
	{
		Silence = 0x00,
		Dtmf1 = 0x01,
		Dtmf2 = 0x02,
		Dtmf3 = 0x03,
		Dtmf4 = 0x04,
		Dtmf5 = 0x05,
		Dmtf6 = 0x06,
		Dtmf7 = 0x07,
		Dtmf8 = 0x08,
		Dtmf9 = 0x09,
		Dtmf0 = 0x0a,
		DtmfStar = 0x0e,
		DtmfPound = 0x0f,
		DtmfA = 0x10,
		DtmfB = 0x11,
		DtmfC = 0x12,
		DtmfD = 0x13,
		InsideDial = 0x21,
		OutsideDial = 0x22,
		LineBusy = 0x23,
		Alerting = 0x24,
		Reorder = 0x25,
		RecorderWarning = 0x26,
		RecorderDetected = 0x27,
		Reverting = 0x28,
		ReceiverOffhook = 0x29,
		PartialDial = 0x2A,
		NoSuchNumber = 0x2B,
		BusyVerification = 0x2C,
		CallWaiting = 0x2D,
		Confirmation = 0x2E,
		CampOnIndication = 0x2F,
		RecallDial = 0x30,
		ZipZip = 0x31,
		Zip = 0x32,
		BeepBonk = 0x33,
		Music = 0x34,
		Hold = 0x35,
		Test = 0x36,
		AddCallWaiting = 0x40,
		PriorityCallWait = 0x41,
		Recall = 0x42,
		BargIn = 0x43,
		DistinctAlert = 0x44,
		PriorityAlert = 0x45,
		ReminderRing = 0x46,
		PrecedenceRingBack = 0x47,
		Preemption = 0x48,
		Mf1 = 0x50,
		Mf2 = 0x51,
		Mf3 = 0x52,
		Mf4 = 0x53,
		Mf5 = 0x54,
		Mf6 = 0x55,
		Mf7 = 0x56,
		Mf8 = 0x57,
		Mf9 = 0x58,
		Mf0 = 0x59,
		Mfkp1 = 0x5a,
		Mfst = 0x5b,
		Mfkp2 = 0x5c,
		Mfstp = 0x5d,
		Mfst3p = 0x5e,
		Milliwatt = 0x5f,
		MilliwattTest = 0x60,
		High = 0x61,
		FlashOverride = 0x62,
		Flash = 0x63,
		Priority = 0x64,
		Immediate = 0x65,
		PreampWarn = 0x66,
		_2105hz = 0x67,
		_2600hz = 0x68,
		_440hz = 0x69,
		_300hz = 0x6a,
		MlppPala = 0x77,
		MlppIca = 0x78,
		MlppVca = 0x79,
		MlppBpa = 0x7a,
		MlppBnea = 0x7b,
		MlppUpa = 0x7c,
		None = 0x7f,
	}

	/// <summary>
	/// Types of softkey events.
	/// </summary>
	/// <remarks>
	/// These are constants rather than an enum because I've seen other values,
	/// e.g., 28, 29 and 30.
	/// </remarks>
	public abstract class SoftkeyEventType
	{
		public const int Redial = 1;
		public const int NewCall = 2;
		public const int Hold = 3;
		public const int Transfer = 4;
		public const int CallForwardAll = 5;
		public const int CallForwardBusy = 6;
		public const int CallForwardNoAnswer = 7;
		public const int Backspace = 8;
		public const int Endcall = 9;
		public const int Resume = 10;
		public const int Answer = 11;
		public const int Info = 12;
		public const int Conference = 13;
		public const int Park = 14;
		public const int Join = 15;
		public const int MeetMeConference = 16;
		public const int CallPickup = 17;
		public const int GroupCallPickup = 18;
		public const int DropLastConferee = 19;
		public const int Callback = 20;
		public const int Barge = 21;
	}

	#endregion

	/// <summary>
	/// Line context to display.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ActivateCallPlane : SccpMessage
	{
		/// <summary>
		/// Constructs an ActivateCallPlane for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public ActivateCallPlane() : this(1) { }

		/// <summary>
		/// Constructs an ActivateCallPlane.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public ActivateCallPlane(uint lineNumber) :
			base(SccpMessage.Type.ActivateCallPlane)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
		}
	}

	/// <summary>
	/// Reports an alarm condition.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class Alarm : SccpMessage
	{
		/// <summary>
		/// Constructs an Alarm for assigning fields later.
		/// </summary>
		public Alarm() : this(Alarm.Severity.Unknown, null, 0, 0) { }

		/// <summary>
		/// Constructs an Alarm with just the alarm text.
		/// </summary>
		/// <param name="text">Alarm text.</param>
		public Alarm(string text) : this(Alarm.Severity.Unknown, text) { }

		/// <summary>
		/// Constructs an Alarm with param1 and param2 always set to 0.
		/// </summary>
		/// <param name="severity">Alarm severity.</param>
		/// <param name="text">Alarm text.</param>
		public Alarm(Severity severity, string text) :
			this(severity, text, 0) { }

		/// <summary>
		/// Constructs an Alarm with param2 always set to 0.
		/// </summary>
		/// <param name="severity">Alarm severity.</param>
		/// <param name="text">Alarm text.</param>
		/// <param name="param1">Optional parameter to describe alarm.</param>
		public Alarm(Severity severity, string text, uint param1) :
			this(severity, text, param1, 0) { }

		/// <summary>
		/// Constructs an Alarm.
		/// </summary>
		/// <param name="severity">Alarm severity.</param>
		/// <param name="text">Alarm text.</param>
		/// <param name="param1">Optional first parameter to describe
		/// alarm.</param>
		/// <param name="param2">Optional second parameter to describe
		/// alarm.</param>
		public Alarm(Severity severity, string text, uint param1, uint param2) :
			base(SccpMessage.Type.Alarm)
		{
			this.severity = severity;
			this.text = text;
			this.param1 = param1;
			this.param2 = param2;
		}

		/// <summary>
		/// Alarm severity.
		/// </summary>
		public enum Severity
		{
			Critical = 0,
			Warning = 1,
			Informational = 2,
			Unknown = 4,
			Major = 7,
			Minor = 8,
			Marginal = 10,
			TraceInfo = 20,
		}

		/// <summary>
		/// Constants defined for Alarm.param1.
		/// </summary>
		public abstract class Param1Const
		{
			public const int TftpFile = 0x00000800;
			public const int FixedTftp = 0x00010000;
			public const int Composite = TftpFile | FixedTftp;
		}

		/// <summary>
		/// Alarm severity.
		/// </summary>
		public Severity severity;

		/// <summary>
		/// Alarm text.
		/// </summary>
		public string text;

		/// <summary>
		/// Optional first parameter to describe alarm.
		/// </summary>
		public uint param1;

		/// <summary>
		/// Optional second parameter to describe alarm.
		/// </summary>
		public uint param2;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			severity = (Severity)decoder.Decode();
			decoder.Decode(out text, Const.MaxAlarmTextSize);
			decoder.Decode(out param1);
			decoder.Decode(out param2);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)severity);
			encoder.Encode(text, Const.MaxAlarmTextSize);
			encoder.Encode(param1);
			encoder.Encode(param2);
		}
	}

	/// <summary>
	/// Backspace the last dialed digit.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class BackspaceReq : SccpMessage
	{
		/// <summary>
		/// Constructs a BackspaceReq for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public BackspaceReq() : this(1, 0) { }

		/// <summary>
		/// Constructs a BackspaceReq.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public BackspaceReq(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.BackspaceReq)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Update button template.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ButtonTemplate : SccpMessage
	{
		/// <summary>
		/// Constructs a ButtonTemplate for assigning fields later.
		/// </summary>
		public ButtonTemplate() : this(null) { }

		/// <summary>
		/// Constructs a ButtonTemplate with list of buttons as parameter;
		/// assuming offset is 0 and total buttons is same as number of
		/// these buttons.
		/// </summary>
		/// <param name="buttons">Softkey template buttons.</param>
		public ButtonTemplate(ArrayList buttons) :
			this(buttons == null ? 0 : (uint)buttons.Count, buttons) { }

		/// <summary>
		/// Constructs a ButtonTemplate with total buttons and list of
		/// template buttons as parameters and assuming offset is 0.
		/// </summary>
		/// <param name="buttons">Softkey template buttons.</param>
		/// <param name="total">Total in template, not just this
		/// message.</param>
		public ButtonTemplate(uint total, ArrayList buttons) :
			this(0, total, buttons) { }

		/// <summary>
		/// Constructs a ButtonTemplate with offset, total buttons, and list of
		/// template buttons as parameters.
		/// </summary>
		/// <param name="buttons">Softkey template buttons.</param>
		/// <param name="offset">Table offset of softkeys. Normally 0.</param>
		/// <param name="total">Total in template, not just this
		/// message.</param>
		public ButtonTemplate(uint offset, uint total, ArrayList buttons) :
			base(SccpMessage.Type.ButtonTemplate)
		{
			this.offset = offset;
			this.total = total;
			this.buttons = buttons;
		}

		/// <summary>
		/// Button definition.
		/// </summary>
		public class Definition : SccpMessageStruct
		{
			/// <summary>
			/// Constructs a Definition for assigning fields later.
			/// </summary>
			public Definition() : this(0, 0) { }

			/// <summary>
			/// Constructs a Definition with instance and definition as
			/// parameters.
			/// </summary>
			/// <param name="instance">Instance number or button value.</param>
			/// <param name="definition">Button definition.</param>
			public Definition(byte instance, byte definition)
			{
				this.instance = instance;
				this.definition = definition;
			}

			/// <summary>
			/// Instance number or button value.
			/// </summary>
			public byte instance;

			/// <summary>
			/// Definition of a button instance.
			/// </summary>
			/// <remarks>
			/// Use a value from the ButtonDefinition enumeration. Left as a
			/// primitive integral rather than enumeration in case we encounter
			/// an unexpected value in the future.
			/// </remarks>
			public byte definition;

			/// <summary>
			/// Definition of a button instance.
			/// </summary>
			public enum ButtonDefinition : byte
			{
				LastNumberRedial = DeviceStimulus.LastNumberRedial,
				SpeedDial = DeviceStimulus.SpeedDial,
				ServiceUrl = DeviceStimulus.ServiceUrl,
				Hold = DeviceStimulus.Hold,
				Transfer = DeviceStimulus.Transfer,
				ForwardAll = DeviceStimulus.ForwardAll,
				ForwardBusy = DeviceStimulus.ForwardBusy,
				ForwardNoAnswer = DeviceStimulus.ForwardNoAnswer,
				Display = DeviceStimulus.Display,
				Line = DeviceStimulus.Line,
				T120Chat = DeviceStimulus.T120Chat,
				T120Whiteboard = DeviceStimulus.T120Whiteboard,
				T120ApplicationSharing = DeviceStimulus.T120ApplicationSharing,
				T120FileTransfer = DeviceStimulus.T120FileTransfer,
				Video = DeviceStimulus.Video,
				Voicemail = DeviceStimulus.Voicemail,
				AnswerRelease = DeviceStimulus.AnswerRelease,
				Privacy = DeviceStimulus.Privacy,
				Keypad = 0xf0,
				Aec = 0xfd,
				Undefined = 0xff,	// (To specify an undefined button.)
			}

			/// <summary>
			/// Decodes this structure from raw message to internal member fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				decoder.Decode(out instance);
				decoder.Decode(out definition);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				encoder.Encode(instance);
				encoder.Encode(definition);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return Marshal.SizeOf(instance) +
					Marshal.SizeOf(definition);
			}
		}

		/// <summary>
		/// Table offset of buttons. Normally 0.
		/// </summary>
		public uint offset;

		/// <summary>
		/// Total for client, not this message.
		/// </summary>
		public uint total;

		/// <summary>
		/// ArrayList of Definition objects.
		/// </summary>
		public ArrayList buttons;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			uint count;

			decoder.Decode(out offset);
			decoder.Decode(out count);
			decoder.Decode(out total);

			buttons = new ArrayList();
			for (uint i = 0; i < count; ++i)
			{
				Definition definition = new Definition();
				definition.Decode(decoder);
				buttons.Add(definition);
			}
			new Definition().Advance(decoder,
				Const.MaxButtonTemplateSize - count);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			uint count = (uint)(buttons == null ? 0 : buttons.Count);

			encoder.Encode(offset);
			encoder.Encode(count);
			encoder.Encode(total);
			if (buttons != null)
			{
				foreach (Definition definition in buttons)
				{
					definition.Encode(encoder);
				}
			}
			new Definition().Advance(encoder,
				Const.MaxButtonTemplateSize - count);
		}
	}

	/// <summary>
	/// Request update of button template definitions.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class ButtonTemplateReq : SccpMessage
	{
		/// <summary>
		/// Constructs a ButtonTemplateReq.
		/// </summary>
		public ButtonTemplateReq() :
			base(SccpMessage.Type.ButtonTemplateReq) { }

		// (no fields)
	}

	/// <summary>
	/// Call-related information
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class CallInfo : SccpMessage
	{
		/// <summary>
		/// Constructs a CallInfo for assigning fields later.
		/// </summary>
		/// <remarks>
		/// There are just way too many fields to provide a constructor that
		/// accepts them all as parameters.
		/// </remarks>
		public CallInfo() : base(SccpMessage.Type.CallInfo) { }

		/// <summary>
		/// Type of call associated with this message.
		/// </summary>
		public enum CallType
		{
			Inbound = 1,
			Outbound = 2,
			Forward = 3,
		}

		/// <summary>
		/// Whether the call has been authenticated.
		/// </summary>
		public enum SecurityStatus
		{
			Unknown = 0,
			NotAuthenticated = 1,
			Authenticated = 2
		}

		/// <summary>
		/// Restrictions placed on call identifiers.
		/// </summary>
		public class RestrictInfo
		{
			/// <summary>
			/// Constructs an RestrictInfo for assigning fields later.
			/// </summary>
			public RestrictInfo() :
				this(false, false, false, false, false, false, false, false) { }

			/// <summary>
			/// Constructs an RestrictInfo.
			/// </summary>
			public RestrictInfo(bool cgpn, bool cgpd, bool cdpn, bool cdpd,
				bool ocgpn, bool ocgpd, bool lcgpn, bool lcgpd)
			{
				this.cgpn = cgpn;
				this.cgpd = cgpd;
				this.cdpn = cdpn;
				this.cdpd = cdpd;
				this.ocgpn = ocgpn;
				this.ocgpd = ocgpd;
				this.lcgpn = lcgpn;
				this.lcgpd = lcgpd;
			}


			public bool cgpn;
			public bool cgpd;
			public bool cgp		// Combine bools.
			{
				get
				{
					return cgpn && cgpd;
				}
				set
				{
					cgpn = value;
					cgpd = value;
				}
			}
			public bool cdpn;
			public bool cdpd;
			public bool cdp		// Combine bools.
			{
				get
				{
					return cdpn && cdpd;
				}
				set
				{
					cdpn = value;
					cdpd = value;
				}
			}
			public bool ocgpn;
			public bool ocgpd;
			public bool ocgp		// Combine bools.
			{
				get
				{
					return ocgpn && ocgpd;
				}
				set
				{
					ocgpn = value;
					ocgpd = value;
				}
			}
			public bool lcgpn;
			public bool lcgpd;
			public bool lcgp		// Combine bools.
			{
				get
				{
					return lcgpn && lcgpd;
				}
				set
				{
					lcgpn = value;
					lcgpd = value;
				}
			}

			public override bool Equals(object obj)
			{
				RestrictInfo r = obj as RestrictInfo;
				if (r == null)
					return false;

				return (	cgpn	== r.cgpn &&
							cgpd	== r.cgpd &&
							cdpn	== r.cdpn &&
							cdpd	== r.cdpd &&
							ocgpn	== r.ocgpn &&
							ocgpd	== r.ocgpd &&
							lcgpn	== r.lcgpn &&
							lcgpd	== r.lcgpd );
			}

			public override int GetHashCode()
			{
				// Set bits according to whether corresponding booleans are set.
				uint restrictInfo_ = 0;
				if (cgpn)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Cgpn;
				}
				if (cgpd)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Cgpd;
				}
				if (cdpn)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Cdpn;
				}
				if (cdpd)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Cdpd;
				}
				if (ocgpn)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Ocgpn;
				}
				if (ocgpd)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Ocgpd;
				}
				if (lcgpn)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Lcgpn;
				}
				if (lcgpd)
				{
					restrictInfo_ |= (uint)RestrictInfo_.Lcgpd;
				}

				return (int)restrictInfo_;
			}


		}

		/// <summary>
		/// Name of the calling party.
		/// </summary>
		public string callingPartyName;

		/// <summary>
		/// Directory number of the calling party as a string.
		/// </summary>
		public string callingPartyNumber;

		/// <summary>
		/// Name of the called party.
		/// </summary>
		public string calledPartyName;

		/// <summary>
		/// Directory number of the called party as a string.
		/// </summary>
		public string calledPartyNumber;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Type of call associated with this message.
		/// </summary>
		public CallType callType;

		/// <summary>
		/// Name of the original called party for forwarded calls.
		/// </summary>
		public string originalCalledPartyName;

		/// <summary>
		/// Directory number (as a string) of the original called party for
		/// forwarded calls.
		/// </summary>
		public string originalCalledPartyNumber;

		/// <summary>
		/// Name of the last redirecting party for forwarded calls.
		/// </summary>
		public string lastRedirectingPartyName;

		/// <summary>
		/// Directory number (as a string) of the last redirecting party for
		/// forwarded calls.
		/// </summary>
		public string lastRedirectingPartyNumber;

		/// <summary>
		/// Reason for the first redirection.
		/// </summary>
		public uint originalCdpnRedirectReason;

		/// <summary>
		/// Reason for the last redirection.
		/// </summary>
		public uint lastRedirectReason;

		/// <summary>
		/// Voice-mail box number (as a string) of the calling party.
		/// </summary>
		public string cgpnVoiceMailbox;

		/// <summary>
		/// Voice-mail box number (as a string) of the called party.
		/// </summary>
		public string cdpnVoiceMailbox;

		/// <summary>
		/// Voice-mail box number (as a string) of the original called party.
		/// </summary>
		public string originalCdpnVoiceMailbox;

		/// <summary>
		/// Voice-mail box number (as a string) of the last redirected party.
		/// </summary>
		public string lastRedirectingVoiceMailbox;

		/// <summary>
		/// Uniquely identifies a call across a shared line.
		/// </summary>
		public uint callInstance;

		/// <summary>
		/// Status of signaling security for call.
		/// </summary>
		public SecurityStatus securityStatus;

		/// <summary>
		/// Restrictions placed on call identifiers.
		/// </summary>
		public RestrictInfo restrictInfo;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Private enumeration used to translate between encoding in SCCP
		/// message and the RestrictInfo booleans in our message object.
		/// </summary>
		private enum RestrictInfo_
		{
			Cgpn     = 0x00000001,
			Cgpd     = 0x00000002,
			Cdpn     = 0x00000004,
			Cdpd     = 0x00000008,
			Ocgpn    = 0x00000010,
			Ocgpd    = 0x00000020,
			Lcgpn    = 0x00000040,
			Lcgpd    = 0x00000080,
		}

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out callingPartyName, Const.DirectoryNameSize);
			decoder.Decode(out callingPartyNumber, Const.DirectoryNumberSize);
			decoder.Decode(out calledPartyName, Const.DirectoryNameSize);
			decoder.Decode(out calledPartyNumber, Const.DirectoryNumberSize);
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
			callType = (CallType)decoder.Decode();
			decoder.Decode(out originalCalledPartyName,
				Const.DirectoryNameSize);
			decoder.Decode(out originalCalledPartyNumber,
				Const.DirectoryNumberSize);
			decoder.Decode(out lastRedirectingPartyName,
				Const.DirectoryNameSize);
			decoder.Decode(out lastRedirectingPartyNumber,
				Const.DirectoryNumberSize);
			decoder.Decode(out originalCdpnRedirectReason);
			decoder.Decode(out lastRedirectReason);
			decoder.Decode(out cgpnVoiceMailbox, Const.DirectoryNumberSize);
			decoder.Decode(out cdpnVoiceMailbox, Const.DirectoryNumberSize);
			decoder.Decode(out originalCdpnVoiceMailbox,
				Const.DirectoryNumberSize);
			decoder.Decode(out lastRedirectingVoiceMailbox,
				Const.DirectoryNumberSize);
			
			// ProtocolVersion.Parche ->

			decoder.Decode(out callInstance);
			securityStatus = (SecurityStatus)decoder.Decode();

			// Set booleans according to whether corresponding bits are set.
			uint restrictInfo_;
			decoder.Decode(out restrictInfo_);
			restrictInfo = new RestrictInfo(
				(restrictInfo_ & (uint)RestrictInfo_.Cgpn) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Cgpd) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Cdpn) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Cdpd) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Ocgpn) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Ocgpd) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Lcgpn) != 0,
				(restrictInfo_ & (uint)RestrictInfo_.Lcgpd) != 0);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(callingPartyName, Const.DirectoryNameSize);
			encoder.Encode(callingPartyNumber, Const.DirectoryNumberSize);
			encoder.Encode(calledPartyName, Const.DirectoryNameSize);
			encoder.Encode(calledPartyNumber, Const.DirectoryNumberSize);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
			encoder.Encode((uint)callType);
			encoder.Encode(originalCalledPartyName, Const.DirectoryNameSize);
			encoder.Encode(originalCalledPartyNumber,
				Const.DirectoryNumberSize);
			encoder.Encode(lastRedirectingPartyName, Const.DirectoryNameSize);
			encoder.Encode(lastRedirectingPartyNumber,
				Const.DirectoryNumberSize);
			encoder.Encode(originalCdpnRedirectReason);
			encoder.Encode(lastRedirectReason);
			encoder.Encode(cgpnVoiceMailbox, Const.DirectoryNumberSize);
			encoder.Encode(cdpnVoiceMailbox, Const.DirectoryNumberSize);
			encoder.Encode(originalCdpnVoiceMailbox,
				Const.DirectoryNumberSize);
			encoder.Encode(lastRedirectingVoiceMailbox,
				Const.DirectoryNumberSize);
			encoder.Encode(callInstance);
			encoder.Encode((uint)securityStatus);

			// Set bits according to whether corresponding booleans are set.
			uint restrictInfo_ = 0;
			if (restrictInfo.cgpn)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Cgpn;
			}
			if (restrictInfo.cgpd)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Cgpd;
			}
			if (restrictInfo.cdpn)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Cdpn;
			}
			if (restrictInfo.cdpd)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Cdpd;
			}
			if (restrictInfo.ocgpn)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Ocgpn;
			}
			if (restrictInfo.ocgpd)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Ocgpd;
			}
			if (restrictInfo.lcgpn)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Lcgpn;
			}
			if (restrictInfo.lcgpd)
			{
				restrictInfo_ |= (uint)RestrictInfo_.Lcgpd;
			}
			encoder.Encode(restrictInfo_);
		}

		public override bool Equals(object obj)
		{
			CallInfo ci = obj as CallInfo;
			if (ci == null)
				return false;

			return(
				( (callingPartyName == null && ci.callingPartyName == null) || (callingPartyName != null && callingPartyName.Equals(ci.callingPartyName)) ) &&
				( (callingPartyNumber == null && ci.callingPartyNumber == null) || (callingPartyNumber != null && callingPartyNumber.Equals(ci.callingPartyNumber)) ) &&
				( (calledPartyName == null && ci.calledPartyName == null) || (calledPartyName != null && calledPartyName.Equals(ci.calledPartyName)) ) &&
				( (calledPartyNumber == null && ci.calledPartyNumber == null) || (calledPartyNumber != null && calledPartyNumber.Equals(ci.calledPartyNumber)) ) &&
				( lineNumber == ci.lineNumber ) &&
				( callReference == ci.callReference ) &&
				( (originalCalledPartyName == null && ci.originalCalledPartyName == null) || (originalCalledPartyName != null && originalCalledPartyName.Equals(ci.originalCalledPartyName)) ) &&
				( (originalCalledPartyNumber == null && ci.originalCalledPartyNumber == null) || (originalCalledPartyNumber != null && originalCalledPartyNumber.Equals(ci.originalCalledPartyNumber)) ) &&
				( (lastRedirectingPartyName == null && ci.lastRedirectingPartyName == null) || (lastRedirectingPartyName != null && lastRedirectingPartyName.Equals(ci.lastRedirectingPartyName)) ) &&
				( (lastRedirectingPartyNumber == null && ci.lastRedirectingPartyNumber == null) || (lastRedirectingPartyNumber != null && lastRedirectingPartyNumber.Equals(ci.lastRedirectingPartyNumber)) ) &&
				( originalCdpnRedirectReason == ci.originalCdpnRedirectReason ) &&
				( lastRedirectReason == ci.lastRedirectReason ) &&
				( (cgpnVoiceMailbox == null && ci.cgpnVoiceMailbox == null) || (cgpnVoiceMailbox != null && cgpnVoiceMailbox.Equals(ci.cgpnVoiceMailbox)) ) &&
				( (originalCdpnVoiceMailbox == null && ci.originalCdpnVoiceMailbox == null) || (originalCdpnVoiceMailbox != null && originalCdpnVoiceMailbox.Equals(ci.originalCdpnVoiceMailbox)) ) &&
				( (lastRedirectingVoiceMailbox == null && ci.lastRedirectingVoiceMailbox == null) || (lastRedirectingVoiceMailbox != null && lastRedirectingVoiceMailbox.Equals(ci.lastRedirectingVoiceMailbox)) ) &&
				( callInstance == ci.callInstance ) &&
				( securityStatus == ci.securityStatus ) &&
				( (restrictInfo == null && ci.restrictInfo == null) || (restrictInfo != null && restrictInfo.Equals(ci.restrictInfo)) )
				);
		}

		public override int GetHashCode()
		{
			Encoder encoder = new Encoder(MessageType);
			Encode(encoder);
			return System.Text.Encoding.ASCII.GetString(encoder.Encoding()).GetHashCode();
		}


	}

	/// <summary>
	/// Call select stat.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class CallSelectStat : SccpMessage
	{
		/// <summary>
		/// Constructs a CallSelectStat for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public CallSelectStat() : this(1, 0) { }

		/// <summary>
		/// Constructs a CallSelectStat.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public CallSelectStat(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.CallSelectStat)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Reports call-related information.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class CallState : SccpMessage
	{
		/// <summary>
		/// Constructs a CallState for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public CallState() : this(CallState.State.Idle, 1, 0, false, null) { }

		/// <summary>
		/// Constructs a CallState.
		/// </summary>
		/// <param name="callState">Call state.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="privacy">Whether call info is hidden from the
		/// user.</param>
		/// <param name="precedence">Precedence.</param>
		public CallState(State callState, uint lineNumber, uint callReference,
			bool privacy,  Precedence precedence) :
			base(SccpMessage.Type.CallState)
		{
			this.callState = callState;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
			this.privacy = privacy;
			this.precedence = precedence;
		}

		/// <summary>
		/// Call state.
		/// </summary>
		public enum State
		{
			Idle = 0,
			Offhook = 1,
			Onhook = 2,
			RingOut = 3,
			RingIn = 4,
			Connected = 5,
			Busy = 6,
			Congestion = 7,
			Hold = 8,
			CallWaiting = 9,
			CallTransfer = 10,
			CallPark = 11,
			Proceed = 12,
			CallRemoteMultiline = 13,
			InvalidNumber = 14
		}

		/// <summary>
		/// Precedence.
		/// </summary>
		public class Precedence : SccpMessageStruct
		{
			/// <summary>
			/// Constructs a Precedence for assigning fields later.
			/// </summary>
			public Precedence() :
				this(PrecedenceStruct.MlppPrecedence.Routine, 0) { }

			/// <summary>
			/// Constructs a Precedence.
			/// </summary>
			public Precedence(PrecedenceStruct.MlppPrecedence level,
				uint domain)
			{
				precedence = new PrecedenceStruct(level, domain);
			}

			/// <summary>
			/// Precedence.
			/// </summary>
			public PrecedenceStruct precedence;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				precedence = new PrecedenceStruct();
				precedence.level =
					(PrecedenceStruct.MlppPrecedence)decoder.Decode();
				decoder.Decode(out precedence.domain);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				encoder.Encode((uint)precedence.level);
				encoder.Encode(precedence.domain);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return Marshal.SizeOf(precedence.level) +
					Marshal.SizeOf(precedence.domain);
			}
		}

		/// <summary>
		/// Call state.
		/// </summary>
		public State callState;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Whether call info is hidden from the user.
		/// </summary>
		public bool privacy;

		/// <summary>
		/// Precedence.
		/// </summary>
		public Precedence precedence;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			callState = (State)decoder.Decode();
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);

			// ProtocolVersion.Hawkbill ->
			bool public_;
			decoder.Decode(out public_, 0);
			privacy = !public_;

			// ProtocolVersion.Parche ->
			if (decoder.More)
			{
				(precedence = new Precedence()).Decode(decoder);
			}
			else
			{
				precedence =
					new Precedence(PrecedenceStruct.MlppPrecedence.Routine, 0);
			}
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)callState);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
			encoder.Encode((uint)(privacy ? 1 : 0));
			encoder.Encode(precedence, typeof(Precedence));
		}
	}

	/// <summary>
	/// Requests current client capabilities.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class CapabilitiesReq : SccpMessage
	{
		/// <summary>
		/// Constructs a CapabilitiesReq.
		/// </summary>
		public CapabilitiesReq() : base(SccpMessage.Type.CapabilitiesReq) { }

		// (no fields)
	}

	/// <summary>
	/// Client capabilities.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class CapabilitiesRes : SccpMessage
	{
		/// <summary>
		/// Constructs a CapabilitiesRes for assigning fields later.
		/// </summary>
		public CapabilitiesRes() : this(null) { }

		/// <summary>
		/// Constructs a CapabilitiesRes with list of capabilities as
		/// parameter.
		/// </summary>
		/// <param name="capabilities">Arraylist of MediaCapability
		/// objects.</param>
		public CapabilitiesRes(ArrayList capabilities) :
			base(SccpMessage.Type.CapabilitiesRes)
		{
			this.capabilities = capabilities;
		}

		/// <summary>
		/// Extra capability-specific parameters.
		/// </summary>
		public class PayloadParams : SccpMessageStruct
		{
			/// <summary>
			/// Constructs a PayloadParams object for a codec other than
			/// G.723.1.
			/// </summary>
			public PayloadParams() : this(G723BitRate.NotApplicable) { }

			/// <summary>
			/// Constructs a PayloadParams.
			/// </summary>
			/// <param name="g723BitRate">Bit rate. For G.723.1 only.</param>
			public PayloadParams(G723BitRate g723BitRate)
			{
				this.g723BitRate = g723BitRate;
			}

			/// <summary>
			/// Bit rate. For G.723.1 only.
			/// </summary>
			public G723BitRate g723BitRate;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				uint firstUint;
				decoder.Decode(out firstUint);
				switch (firstUint)
				{
					case (uint)G723BitRate._5_3khz:
					case (uint)G723BitRate._6_4khz:
						g723BitRate = (G723BitRate)firstUint;
						break;

					default:
						g723BitRate = G723BitRate.NotApplicable;
						break;
				}
				decoder.Advance(4);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				switch (g723BitRate)
				{
					case G723BitRate._5_3khz:
					case G723BitRate._6_4khz:
						encoder.Encode((uint)g723BitRate);
						break;

					default:
						encoder.Advance(4);
						break;
				}
				encoder.Advance(4);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return SccpMessage.Const.EnumSize;
			}
		}

		/// <summary>
		/// Multimedia capability.
		/// </summary>
		public class MediaCapability : SccpMessageStruct
		{
			/// <summary>
			/// Constructs a MediaCapability using typical codec, i.e., 20ms
			/// G.711 uLaw 64k.
			/// </summary>
			public MediaCapability() : this(PayloadType.G711Ulaw64k, 20) { }

			/// <summary>
			/// Constructs a MediaCapability for codecs without "payload
			/// parameters" (IOW, not G.723.1).
			/// </summary>
			/// <param name="payloadType">Payload-type enumeration.</param>
			/// <param name="millisecondsPerPacket">Packetization in
			/// milliseconds.</param>
			public MediaCapability(PayloadType payloadType,
				uint millisecondsPerPacket)
				: this(payloadType, millisecondsPerPacket,
				G723BitRate.NotApplicable) { }

			/// <summary>
			/// Constructs MediaCapability for all codecs, even those with
			/// "payload parameters".
			/// </summary>
			/// <param name="payloadType">Payload-type enumeration.</param>
			/// <param name="millisecondsPerPacket">Packetization in
			/// milliseconds.</param>
			/// <param name="g723BitRate">Bit rate for G.723.1 (5.3kHz or
			/// 6.4kHz)</param>
			public MediaCapability(PayloadType payloadType,
				uint millisecondsPerPacket, G723BitRate g723BitRate)
			{
				this.payloadType = payloadType;
				this.millisecondsPerPacket = millisecondsPerPacket;
				payloadParams = new PayloadParams(g723BitRate);
			}

			/// <summary>
			/// Payload-type enumeration.
			/// </summary>
			public PayloadType payloadType;

			/// <summary>
			/// Packetization in milliseconds.
			/// </summary>
			public uint millisecondsPerPacket;

			/// <summary>
			/// Extra capability-specific parameters.
			/// </summary>
			public PayloadParams payloadParams;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				payloadType = (PayloadType)decoder.Decode();
				decoder.Decode(out millisecondsPerPacket);
				(payloadParams = new PayloadParams()).Decode(decoder);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				encoder.Encode((uint)payloadType);
				encoder.Encode(millisecondsPerPacket);
				if (payloadType != PayloadType.G7231)
				{
					payloadParams.g723BitRate = G723BitRate.NotApplicable;
				}
				encoder.Encode(payloadParams, typeof(PayloadParams));
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return SccpMessage.Const.EnumSize +
					Marshal.SizeOf(millisecondsPerPacket) +
					new PayloadParams().SizeOf();
			}
		}

		/// <summary>
		/// Arraylist of MediaCapability objects.
		/// </summary>
		public ArrayList capabilities;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			uint count;

			decoder.Decode(out count);

			capabilities = new ArrayList();
			for (uint i = 0; i < count; ++i)
			{
				MediaCapability capability = new MediaCapability();
				capability.Decode(decoder);
				capabilities.Add(capability);
			}
			new MediaCapability().Advance(decoder,
				Const.MaxMediaCapabilities - count);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			uint count = (uint)(capabilities == null ? 0 : capabilities.Count);

			encoder.Encode(count);
			if (capabilities != null)
			{
				foreach (MediaCapability capability in capabilities)
				{
					capability.Encode(encoder);
				}
			}
			new MediaCapability().Advance(encoder,
				Const.MaxMediaCapabilities - count);
		}
	}

	/// <summary>
	/// Clear display.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ClearDisplay : SccpMessage
	{
		/// <summary>
		/// Constructs a ClearDisplay.
		/// </summary>
		public ClearDisplay() : base(SccpMessage.Type.ClearDisplay) { }

		// (no fields)
	}

	/// <summary>
	/// Clear notify message before expiration of timeout.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ClearNotify : SccpMessage
	{
		/// <summary>
		/// Constructs a ClearNotify.
		/// </summary>
		public ClearNotify() : base(SccpMessage.Type.ClearNotify) { }

		// (no fields)
	}

	/// <summary>
	/// Clear notify message before expiration of timeout.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ClearPriorityNotify : SccpMessage
	{
		/// <summary>
		/// Constructs a ClearPriorityNotify for assigning priority later.
		/// </summary>
		public ClearPriorityNotify() : this(0) { }

		/// <summary>
		/// Constructs a ClearPriorityNotify.
		/// </summary>
		/// <param name="priority">Priority. 0 clears all notify
		/// message.</param>
		public ClearPriorityNotify(uint priority) :
			base(SccpMessage.Type.ClearPriorityNotify)
		{
			this.priority = priority;
		}

		/// <summary>
		/// Priority. 0 clears all notify message.
		/// </summary>
		public uint priority;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out priority);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(priority);
		}
	}

	/// <summary>
	/// Clear status prompt display.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ClearPromptStatus : SccpMessage
	{
		/// <summary>
		/// Constructs a ClearPromptStatus for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public ClearPromptStatus() : this(1, 0) { }

		/// <summary>
		/// Constructs a ClearPromptStatus.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public ClearPromptStatus(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.ClearPromptStatus)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Terminate reception of RTP stream.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class CloseReceiveChannel : SccpMessage
	{
		/// <summary>
		/// Constructs a CloseReceiveChannel for assigning fields later.
		/// </summary>
		public CloseReceiveChannel() : this(0, 0, 0) { }

		/// <summary>
		/// Constructs a CloseReceiveChannel.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public CloseReceiveChannel(uint conferenceId, uint passthruPartyId, uint callReference) :
			base(SccpMessage.Type.CloseReceiveChannel)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// property whose value is the conference id from the message.
		/// </summary>
		public override uint ConferenceId { get { return conferenceId; } }

		/// <summary>
		/// Property whose value is the passthruPartyId from the message.
		/// </summary>
		public override uint PassthruPartyId { get { return passthruPartyId; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			
			// ProtocolVersion.Parche ->
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Configuration information.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ConfigStat : SccpMessage
	{
		/// <summary>
		/// Constructs a ConfigStat for assigning fields later.
		/// </summary>
		public ConfigStat() : this(null, null, null, 0, 0) { }

		/// <summary>
		/// Constructs a ConfigStat.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="userName">Name of the user associated with the
		/// phone.</param>
		/// <param name="serverName">Name of the CallManager servcer associated
		/// with the phone.</param>
		/// <param name="numberLines">Number of line-select buttons on the
		/// phone.</param>
		/// <param name="numberSpeedDials">Number of speed-dial buttons
		/// available on the phone.</param>
		public ConfigStat(Sid sid, string userName, string serverName,
			uint numberLines, uint numberSpeedDials) :
			base(SccpMessage.Type.ConfigStat)
		{
			this.sid = sid;
			this.userName = userName;
			this.serverName = serverName;
			this.numberLines = numberLines;
			this.numberSpeedDials = numberSpeedDials;
		}

		/// <summary>
		/// An SCCP Client IDentifier, a.k.a., sid or device name.
		/// </summary>
		public Sid sid;

		/// <summary>
		/// Name of the user associated with the phone.
		/// </summary>
		public string userName;

		/// <summary>
		/// Name of the CallManager servcer associated with the phone.
		/// </summary>
		public string serverName;

		/// <summary>
		/// Number of line-select buttons on the phone.
		/// </summary>
		public uint numberLines;

		/// <summary>
		/// Number of speed-dial buttons available on the phone.
		/// </summary>
		public uint numberSpeedDials;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out sid);
			decoder.Decode(out userName, Const.DirectoryNameSize);
			decoder.Decode(out serverName, Const.DirectoryNameSize);
			decoder.Decode(out numberLines);
			decoder.Decode(out numberSpeedDials);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(sid);
			encoder.Encode(userName, Const.DirectoryNameSize);
			encoder.Encode(serverName, Const.DirectoryNameSize);
			encoder.Encode(numberLines);
			encoder.Encode(numberSpeedDials);
		}
	}

	/// <summary>
	/// Request connection statistics.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ConnectionStatisticsReq : SccpMessage
	{
		/// <summary>
		/// Constructs a ConnectionStatisticsReq for assigning fields later.
		/// </summary>
		public ConnectionStatisticsReq() :
			this(null, 0, StatsProcessing.Clear) { }

		/// <summary>
		/// Constructs a ConnectionStatisticsReq.
		/// </summary>
		/// <param name="directoryNumber">Directory number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="processingMode">Statistics-processing mode.</param>
		public ConnectionStatisticsReq(string directoryNumber,
			uint callReference, StatsProcessing processingMode) :
			base(SccpMessage.Type.ConnectionStatisticsReq)
		{
			this.directoryNumber = directoryNumber;
			this.callReference = callReference;
			this.processingMode = processingMode;
		}

		/// <summary>
		/// Directory number.
		/// </summary>
		public string directoryNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Statistics-processing mode.
		/// </summary>
		public StatsProcessing processingMode;

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out directoryNumber, Const.DirectoryNumberSize);
			decoder.Decode(out callReference);
			processingMode = (StatsProcessing)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(directoryNumber, Const.DirectoryNumberSize);
			encoder.Encode(callReference);
			encoder.Encode((uint)processingMode);
		}
	}

	/// <summary>
	/// Connection statistics.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class ConnectionStatisticsRes : SccpMessage
	{
		/// <summary>
		/// Constructs a ConnectionStatisticsRes for assigning fields later.
		/// </summary>
		public ConnectionStatisticsRes() :
			this(null, 0, StatsProcessing.Clear, 0, 0, 0, 0, 0, 0, 0) { }

		/// <summary>
		/// Constructs a ConnectionStatisticsRes.
		/// </summary>
		/// <param name="directoryNumber">Directory number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="processingMode">Statistics-processing mode.</param>
		/// <param name="numberPacketsSent">Number of RTP packets sent during
		/// this connection.</param>
		/// <param name="numberBytesSent">Number of bytes sent in RTP packets
		/// during this connection.</param>
		/// <param name="numberPacketsReceived">Number of RTP packets received
		/// during this connection.</param>
		/// <param name="numberBytesReceived">Number of bytes received in RTP
		/// packets during this connection.</param>
		/// <param name="numberPacketsLost">Number of RTP packets not received
		/// during this connection.</param>
		/// <param name="jitter">Observed jitter during this connection.</param>
		/// <param name="latency">Maximum observed latency during this
		/// connection.</param>
		public ConnectionStatisticsRes(string directoryNumber,
			uint callReference, StatsProcessing processingMode,
			uint numberPacketsSent, uint numberBytesSent,
			uint numberPacketsReceived, uint numberBytesReceived,
			uint numberPacketsLost, uint jitter, uint latency) :
			base(SccpMessage.Type.ConnectionStatisticsRes)
		{
			this.directoryNumber = directoryNumber;
			this.callReference = callReference;
			this.processingMode = processingMode;
			this.numberPacketsSent = numberPacketsSent;
			this.numberBytesSent = numberBytesSent;
			this.numberPacketsReceived = numberPacketsReceived;
			this.numberBytesReceived = numberBytesReceived;
			this.numberPacketsLost = numberPacketsLost;
			this.jitter = jitter;
			this.latency = latency;
		}

		/// <summary>
		/// Directory number.
		/// </summary>
		public string directoryNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Statistics-processing mode.
		/// </summary>
		public StatsProcessing processingMode;

		/// <summary>
		/// Number of RTP packets sent during this connection.
		/// </summary>
		public uint numberPacketsSent;

		/// <summary>
		/// Number of bytes sent in RTP packets during this connection.
		/// </summary>
		public uint numberBytesSent;

		/// <summary>
		/// Number of RTP packets received during this connection.
		/// </summary>
		public uint numberPacketsReceived;

		/// <summary>
		/// Number of bytes received in RTP packets during this connection.
		/// </summary>
		public uint numberBytesReceived;

		/// <summary>
		/// Number of RTP packets not received during this connection.
		/// </summary>
		public uint numberPacketsLost;

		/// <summary>
		/// Observed jitter during this connection.
		/// </summary>
		public uint jitter;

		/// <summary>
		/// Maximum observed latency during this connection.
		/// </summary>
		public uint latency;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out directoryNumber, Const.DirectoryNumberSize);
			decoder.Decode(out callReference);
			processingMode = (StatsProcessing)decoder.Decode();
			decoder.Decode(out numberPacketsSent);
			decoder.Decode(out numberBytesSent);
			decoder.Decode(out numberPacketsReceived);
			decoder.Decode(out numberBytesReceived);
			decoder.Decode(out numberPacketsLost);
			decoder.Decode(out jitter);
			decoder.Decode(out latency);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(directoryNumber, Const.DirectoryNumberSize);
			encoder.Encode(callReference);
			encoder.Encode((uint)processingMode);
			encoder.Encode(numberPacketsSent);
			encoder.Encode(numberBytesSent);
			encoder.Encode(numberPacketsReceived);
			encoder.Encode(numberBytesReceived);
			encoder.Encode(numberPacketsLost);
			encoder.Encode(jitter);
			encoder.Encode(latency);
		}
	}

	/// <summary>
	/// Return to null display context.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DeactivateCallPlane : SccpMessage
	{
		/// <summary>
		/// Constructs a DeactivateCallPlane.
		/// </summary>
		public DeactivateCallPlane() :
			base(SccpMessage.Type.DeactivateCallPlane) { }

		// (no fields)
	}

	/// <summary>
	/// Current date and time.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DefineTimeDate : SccpMessage
	{
		/// <summary>
		/// Constructs a DefineTimeDate for assigning fields later.
		/// </summary>
		public DefineTimeDate() : this(null, 0) { }

		/// <summary>
		/// Constructs a DefineTimeDate.
		/// </summary>
		/// <param name="stationTime">Same as Microsoft's SYSTEMTIME
		/// structure.</param>
		/// <param name="systemTime">System time in Microsoft format.</param>
		public DefineTimeDate(StationTime stationTime, uint systemTime) :
			base(SccpMessage.Type.DefineTimeDate)
		{
			this.stationTime = stationTime;
			this.systemTime = systemTime;
		}

		/// <summary>
		/// Same as Microsoft's SYSTEMTIME structure.
		/// </summary>
		public class StationTime : SccpMessageStruct	// TBD - maybe represent as single DateTime object.
		{
			public uint year;
			public uint month;
			public uint dayOfWeek;
			public uint day;
			public uint hour;
			public uint minute;
			public uint second;
			public uint millisecond;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				decoder.Decode(out year);
				decoder.Decode(out month);
				decoder.Decode(out dayOfWeek);
				decoder.Decode(out day);
				decoder.Decode(out hour);
				decoder.Decode(out minute);
				decoder.Decode(out second);
				decoder.Decode(out millisecond);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				encoder.Encode(year);
				encoder.Encode(month);
				encoder.Encode(dayOfWeek);
				encoder.Encode(day);
				encoder.Encode(hour);
				encoder.Encode(minute);
				encoder.Encode(second);
				encoder.Encode(millisecond);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return Marshal.SizeOf(year) +
					Marshal.SizeOf(month) +
					Marshal.SizeOf(dayOfWeek) +
					Marshal.SizeOf(day) +
					Marshal.SizeOf(hour) +
					Marshal.SizeOf(minute) +
					Marshal.SizeOf(second) +
					Marshal.SizeOf(millisecond);
			}
		}

		/// <summary>
		/// Same as Microsoft's SYSTEMTIME structure.
		/// </summary>
		public StationTime stationTime;

		/// <summary>
		/// System time in Microsoft format.
		/// </summary>
		public uint systemTime;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(stationTime = new StationTime()).Decode(decoder);
			decoder.Decode(out systemTime);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(stationTime, typeof(StationTime));
			encoder.Encode(systemTime);
		}
	}

	/// <summary>
	/// User-related data.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DeviceToUserData : SccpMessage
	{
		/// <summary>
		/// Constructs a DeviceToUserData for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public DeviceToUserData() : this(0, 1, 0, 0, null) { }

		/// <summary>
		/// Constructs a DeviceToUserData.
		/// </summary>
		/// <param name="applicationId">Set to 0 for call processing; else set
		/// to the application id.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="transactionId">Differentiates entities in the same
		/// application.</param>
		/// <param name="data">User-related data.</param>
		public DeviceToUserData(uint applicationId, uint lineNumber,
			uint callReference, uint transactionId, byte[] data) :
			base(SccpMessage.Type.DeviceToUserData)
		{
			this.data = new UserAndDeviceData(applicationId, lineNumber,
				callReference, transactionId, data);
		}

		/// <summary>
		/// User-related data
		/// </summary>
		public UserAndDeviceData data;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(data = new UserAndDeviceData()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(data, typeof(UserAndDeviceData));
		}
	}

	/// <summary>
	/// Used for callback-busy application.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class DeviceToUserDataRes : SccpMessage
	{
		/// <summary>
		/// Constructs a DeviceToUserDataRes for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public DeviceToUserDataRes() : this(0, 1, 0, 0, null) { }

		/// <summary>
		/// Constructs a DeviceToUserDataRes for assigning fields later.
		/// </summary>
		/// <param name="applicationId">Set to 0 for call processing; else set
		/// to the application id.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="transactionId">Differentiates entities in the same
		/// application.</param>
		/// <param name="data">User-related data.</param>
		public DeviceToUserDataRes(uint applicationId, uint lineNumber,
			uint callReference, uint transactionId, byte[] data) :
			base(SccpMessage.Type.DeviceToUserDataRes)
		{
			this.data = new UserAndDeviceData(applicationId, lineNumber,
				callReference, transactionId, data);
		}

		/// <summary>
		/// User-related data
		/// </summary>
		public UserAndDeviceData data;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(data = new UserAndDeviceData()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(data, typeof(UserAndDeviceData));
		}
	}

	/// <summary>
	/// Number as dialed (as opposed to the translated number).
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DialedNumber : SccpMessage
	{
		/// <summary>
		/// Constructs a DialedNumber for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public DialedNumber() : this(null, 1, 0) { }

		/// <summary>
		/// Constructs a DialedNumber.
		/// </summary>
		/// <param name="dialedNumber">Directory number of the called
		/// party.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public DialedNumber(string dialedNumber, uint lineNumber,
			uint callReference) :
			base(SccpMessage.Type.DialedNumber)
		{
			this.dialedNumber = dialedNumber;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Directory number of the called party.
		/// </summary>
		public string dialedNumber;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out dialedNumber, Const.DirectoryNumberSize);
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(dialedNumber, Const.DirectoryNumberSize);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Display text string.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DisplayNotify : SccpMessage
	{
		/// <summary>
		/// Constructs a DisplayNotify for assigning fields later.
		/// </summary>
		public DisplayNotify() : this(0, null) { }

		/// <summary>
		/// Constructs a DisplayNotify.
		/// </summary>
		/// <param name="timeout">How long to display the text in
		/// seconds.</param>
		/// <param name="text">Text to display.</param>
		public DisplayNotify(uint timeout, string text) :
			base(SccpMessage.Type.DisplayNotify)
		{
			this.timeout = timeout;
			this.text = text;
		}

		/// <summary>
		/// How long to display the text in seconds.
		/// </summary>
		public uint timeout;

		/// <summary>
		/// Text to display.
		/// </summary>
		public string text;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out timeout);
			decoder.Decode(out text, Const.DisplayNotifyTextSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(timeout);
			encoder.Encode(text, Const.DisplayNotifyTextSize);
		}
	}

	/// <summary>
	/// Display notify message.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DisplayPriorityNotify : SccpMessage
	{
		/// <summary>
		/// Constructs a DisplayPriorityNotify for assigning fields later.
		/// </summary>
		public DisplayPriorityNotify() : this(0, 0, null) { }

		/// <summary>
		/// Constructs a DisplayPriorityNotify.
		/// </summary>
		/// <param name="timeout">Timeout value.</param>
		/// <param name="priority">Priority.</param>
		/// <param name="text">Text to display.</param>
		public DisplayPriorityNotify(uint timeout, uint priority, string text) :
			base(SccpMessage.Type.DisplayPriorityNotify)
		{
			this.timeout = timeout;
			this.priority = priority;
			this.text = text;
		}

		/// <summary>
		/// Timeout value.
		/// </summary>
		public uint timeout;

		/// <summary>
		/// Priority.
		/// </summary>
		public uint priority;

		/// <summary>
		/// Text to display.
		/// </summary>
		public string text;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out timeout);
			decoder.Decode(out priority);
			decoder.Decode(out text, Const.DisplayNotifyTextSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(timeout);
			encoder.Encode(priority);
			encoder.Encode(text, Const.DisplayNotifyTextSize);
		}
	}

	/// <summary>
	/// Display call-related status.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DisplayPromptStatus : SccpMessage
	{
		/// <summary>
		/// Constructs a DisplayPromptStatus for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public DisplayPromptStatus() :
			this(0, null, 1, 0) { }

		/// <summary>
		/// Constructs a DisplayPromptStatus.
		/// </summary>
		/// <param name="timeout">How long to display the text in
		/// seconds.</param>
		/// <param name="text">Text to display.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public DisplayPromptStatus(uint timeout, string text, uint lineNumber,
			uint callReference) :
			base(SccpMessage.Type.DisplayPromptStatus)
		{
			this.timeout = timeout;
			this.text = text;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// How long to display the text in seconds.
		/// </summary>
		public uint timeout;

		/// <summary>
		/// Text to display.
		/// </summary>
		public string text;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out timeout);
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
			decoder.Decode(out text, Const.DisplayPromptTextSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(timeout);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
			encoder.Encode(text, Const.DisplayPromptTextSize);
		}
	}

	/// <summary>
	/// Display text.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class DisplayText : SccpMessage
	{
		/// <summary>
		/// Constructs a DisplayText for assigning fields later.
		/// </summary>
		public DisplayText() : this(null) { }

		/// <summary>
		/// Constructs a DisplayText.
		/// </summary>
		/// <param name="text">Text to display.</param>
		public DisplayText(string text) : base(SccpMessage.Type.DisplayText)
		{
			this.text = text;
		}

		/// <summary>
		/// Text to display.
		/// </summary>
		public string text;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out text, Const.DisplayTextSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(text, Const.DisplayTextSize);
		}
	}

	/// <summary>
	/// Feature stat.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class FeatureStat : SccpMessage
	{
		/// <summary>
		/// Constructs a FeatureStat for assigning fields later.
		/// </summary>
		public FeatureStat() : this(null) { }

		/// <summary>
		/// Constructs a FeatureStat.
		/// </summary>
		/// <param name="feature">Feature-related data.</param>
		public FeatureStat(FeatureStruct feature) :
			base(SccpMessage.Type.FeatureStat)
		{
			this.feature = feature;
		}

		/// <summary>
		/// Feature-related data.
		/// </summary>
		public FeatureStruct feature;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			feature = new FeatureStruct();
			decoder.Decode(out feature.number);
			decoder.Decode(out feature.id);
			decoder.Decode(out feature.label, Const.DirectoryNameSize);
			decoder.Decode(out feature.status);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(feature.number);
			encoder.Encode(feature.id);
			encoder.Encode(feature.label, Const.DirectoryNameSize);
			encoder.Encode(feature.status);
		}
	}

	/// <summary>
	/// Feature stat request.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class FeatureStatReq : SccpMessage
	{
		/// <summary>
		/// Constructs a FeatureStatReq for assigning the index later.
		/// </summary>
		public FeatureStatReq() : this(0) { }

		/// <summary>
		/// Constructs a FeatureStatReq.
		/// </summary>
		/// <param name="index">Index at which the feature is being
		/// requested.</param>
		public FeatureStatReq(int index) :
			base(SccpMessage.Type.FeatureStatReq)
		{
			this.index = (uint)index;
		}

		/// <summary>
		/// Index at which the feature is being requested.
		/// </summary>
		public uint index;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out index);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(index);
		}
	}

	/// <summary>
	/// Forward stat.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ForwardStat : SccpMessage
	{
		/// <summary>
		/// Constructs a ForwardStat for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public ForwardStat() : this(0, 1, 0, null, 0, null, 0, null) { }

		/// <summary>
		/// Constructs a ForwardStat.
		/// </summary>
		/// <param name="activeForward">Number of call-forward modes active on
		/// the line.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="forwardAllActive">Whether forward-all is
		/// enabled.</param>
		/// <param name="forwardAllDirectoryNumber">Directory number of the
		/// line to which all calls are forwarded.</param>
		/// <param name="forwardBusyActive">Whether forward-busy is
		/// enabled.</param>
		/// <param name="forwardBusyDirectoryNumber">Directory number of the
		/// line to which all calls are forwarded when this line is
		/// busy.</param>
		/// <param name="forwardNoAnswerActive">Whether forward-no-answer is
		/// enabled.</param>
		/// <param name="forwardNoAnswerDirectoryNumber">Directory number of
		/// the line to which all calls are forwarded when the user does not
		/// answer.</param>
		public ForwardStat(uint activeForward, uint lineNumber,
			uint forwardAllActive, string forwardAllDirectoryNumber,
			uint forwardBusyActive, string forwardBusyDirectoryNumber,
			uint forwardNoAnswerActive,
			string forwardNoAnswerDirectoryNumber) :
			base(SccpMessage.Type.ForwardStat)
		{
			this.activeForward = activeForward;
			this.lineNumber = lineNumber;
			this.forwardAllActive = forwardAllActive;
			this.forwardAllDirectoryNumber = forwardAllDirectoryNumber;
			this.forwardBusyActive = forwardBusyActive;
			this.forwardBusyDirectoryNumber = forwardBusyDirectoryNumber;
			this.forwardNoAnswerActive = forwardNoAnswerActive;
			this.forwardNoAnswerDirectoryNumber =
				forwardNoAnswerDirectoryNumber;
		}

		/// <summary>
		/// Number of call-forward modes active on the line.
		/// </summary>
		public uint activeForward;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Whether forward-all is enabled.
		/// </summary>
		public uint forwardAllActive;

		/// <summary>
		/// Directory number of the line to which all calls are forwarded.
		/// </summary>
		public string forwardAllDirectoryNumber;

		/// <summary>
		/// Whether forward-busy is enabled.
		/// </summary>
		public uint forwardBusyActive;

		/// <summary>
		/// Directory number of the line to which all calls are forwarded when
		/// this line is busy.
		/// </summary>
		public string forwardBusyDirectoryNumber;

		/// <summary>
		/// Whether forward-no-answer is enabled.
		/// </summary>
		public uint forwardNoAnswerActive;

		/// <summary>
		/// Directory number of the line to which all calls are forwarded when
		/// the user does not answer.
		/// </summary>
		public string forwardNoAnswerDirectoryNumber;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out activeForward);
			decoder.Decode(out lineNumber);
			decoder.Decode(out forwardAllActive);
			decoder.Decode(out forwardAllDirectoryNumber, Const.DirectoryNumberSize);
			decoder.Decode(out forwardBusyActive);
			decoder.Decode(out forwardBusyDirectoryNumber, Const.DirectoryNumberSize);
			decoder.Decode(out forwardNoAnswerActive);
			decoder.Decode(out forwardNoAnswerDirectoryNumber, Const.DirectoryNumberSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(activeForward);
			encoder.Encode(lineNumber);
			encoder.Encode(forwardAllActive);
			encoder.Encode(forwardAllDirectoryNumber, Const.DirectoryNumberSize);
			encoder.Encode(forwardBusyActive);
			encoder.Encode(forwardBusyDirectoryNumber, Const.DirectoryNumberSize);
			encoder.Encode(forwardNoAnswerActive);
			encoder.Encode(forwardNoAnswerDirectoryNumber, Const.DirectoryNumberSize);
		}
	}

	/// <summary>
	/// Whether headset is on or off.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class HeadsetStatus : SccpMessage
	{
		/// <summary>
		/// Constructs a HeadsetStatus for with the on field set to false.
		/// </summary>
		public HeadsetStatus() : this(false) { }

		/// <summary>
		/// Constructs a HeadsetStatus.
		/// </summary>
		/// <param name="on">Whether the headset is enabled.</param>
		public HeadsetStatus(bool on) : base(SccpMessage.Type.HeadsetStatus)
		{
			this.on = on;
		}

		/// <summary>
		/// Whether the headset is enabled.
		/// </summary>
		public bool on;

		/// <summary>
		/// Used to translate between SCCP message field and our bool.
		/// </summary>
		private enum HeadsetStatus_
		{
			On = 1,
			Off = 2,
		}

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out on, (uint)HeadsetStatus_.On);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)(on ? HeadsetStatus_.On : HeadsetStatus_.Off));
		}
	}

	/// <summary>
	/// UDP port of RTP stream to CallManager.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class IpPort : SccpMessage
	{
		/// <summary>
		/// Constructs an IpPort for port 0.
		/// </summary>
		public IpPort() : this(0) { }

		/// <summary>
		/// Constructs an IpPort.
		/// </summary>
		/// <param name="port">UDP port to be used with the RTP stream.</param>
		public IpPort(uint port) : base(SccpMessage.Type.IpPort)
		{
			this.port = port;
		}

		/// <summary>
		/// UDP port to be used with the RTP stream.
		/// </summary>
		public uint port;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out port);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(port);
		}
	}

	/// <summary>
	/// Shows that link is active.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class Keepalive : SccpMessage
	{
		/// <summary>
		/// Constructs a Keepalive.
		/// </summary>
		public Keepalive() : base(SccpMessage.Type.Keepalive) { }

		// (no fields)
	}

	/// <summary>
	/// Shows that link is active.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class KeepaliveAck : SccpMessage
	{
		/// <summary>
		/// Constructs a KeepaliveAck.
		/// </summary>
		public KeepaliveAck() : base(SccpMessage.Type.KeepaliveAck) { }

		// (no fields)
	}

	/// <summary>
	/// A keypad button is pressed.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class KeypadButton : SccpMessage
	{
		/// <summary>
		/// Constructs a KeypadButton for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public KeypadButton() : this(Button.Zero, 1, 0) { }

		/// <summary>
		/// Constructs a KeypadButton.
		/// </summary>
		/// <param name="button">The button pressed.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public KeypadButton(Button button, uint lineNumber, uint callReference) :
			base(SccpMessage.Type.KeypadButton)
		{
			this.button = button;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Keypad button.
		/// </summary>
		public enum Button
		{
			Zero,
			One,
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			A,
			B,
			C,
			D,
			Star,
			Pound,
		}

		/// <summary>
		/// The button pressed.
		/// </summary>
		public Button button;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			button = (Button)decoder.Decode();

			// ProtocolVersion.Parche ->

			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)button);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Directory number associated with line-select button.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class LineStat : SccpMessage
	{
		/// <summary>
		/// Constructs a LineStat for assigning fields later.
		/// </summary>
		public LineStat() : this(null) { }

		/// <summary>
		/// Constructs a LineStat.
		/// </summary>
		/// <param name="line">Line information.</param>
		public LineStat(Line line) : base(SccpMessage.Type.LineStat)
		{
			this.line = line;
		}

		/// <summary>
		/// Line information.
		/// </summary>
		public Line line;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			line = new Line();
			decoder.Decode(out line.number);
			decoder.Decode(out line.directoryNumber,
				Const.DirectoryNumberSize);
			decoder.Decode(out line.fullyQualifiedDisplayName,
				Const.DirectoryNameSize);

			// ProtocolVersion.Hawkbill ->
			decoder.Decode(out line.label, Const.DirectoryNameSize);

			// ProtocolVersion.Parche ->
			if (decoder.More)
			{
				uint options;
				decoder.Decode(out options);
				line.displayOptions = new Line.DisplayOptions(
					(options & 0x00000001) != 0, (options & 0x00000002) != 0,
					(options & 0x00000004) != 0, (options & 0x00000008) != 0);
			}
			else
			{
				line.displayOptions =
					new Line.DisplayOptions(true, false, true, true);
			}
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(line.number);
			encoder.Encode(line.directoryNumber, Const.DirectoryNumberSize);
			encoder.Encode(line.fullyQualifiedDisplayName,
				Const.DirectoryNameSize);
			encoder.Encode(line.label, Const.DirectoryNameSize);
			uint options = 0;
			options |= line.displayOptions.originalDialedNumber ?
				(uint)0x00000001 : 0;
			options |= line.displayOptions.redirectedDialedNumber ?
				(uint)0x00000002 : 0;
			options |= line.displayOptions.callingLineId ?
				(uint)0x00000004 : 0;
			options |= line.displayOptions.callingNameId ?
				(uint)0x00000008 : 0;
			encoder.Encode(options);
		}
	}

	/// <summary>
	/// Request directory number of indicated line.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class LineStatReq : SccpMessage
	{
		/// <summary>
		/// Constructs a LineStatReq for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public LineStatReq() : this(1) { }

		/// <summary>
		/// Constructs a LineStatReq with label and event as parameters.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public LineStatReq(int lineNumber) : base(SccpMessage.Type.LineStatReq)
		{
			this.lineNumber = (uint)lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
		}
	}

	/// <summary>
	/// Client is in offhook condition.	Called OffhookSccp to distinguish from
	/// OffhookClient.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class OffhookSccp : SccpMessage
	{
		/// <summary>
		/// Constructs an OffhookSccp for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public OffhookSccp() : this(1, 0) { }

		/// <summary>
		/// Constructs an OffhookSccp.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public OffhookSccp(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.OffhookSccp)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Client is in onhook condition.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class Onhook : SccpMessage
	{
		/// <summary>
		/// Constructs an Onhook for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public Onhook() : this(1, 0) { }

		/// <summary>
		/// Constructs an Onhook.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public Onhook(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.Onhook)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Begin receiving unicast RTP stream.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class OpenReceiveChannel : SccpMessage
	{
		/// <summary>
		/// Constructs an OpenReceiveChannel for assigning fields later.
		/// </summary>
		public OpenReceiveChannel() :
			this(0, 0, 0, PayloadType.G711Ulaw64k, null, 0, null) { }

		/// <summary>
		/// Constructs an OpenReceiveChannel.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payload">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="qualifier">Extra qualifiers to incoming payload
		/// type.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		/// <param name="mediaEncryption">Media encryption information.</param>
		public OpenReceiveChannel(uint conferenceId, uint passthruPartyId,
			uint packetSize, PayloadType payload,
			MediaQualifierIncoming qualifier, uint callReference,
			MediaEncryptionKey mediaEncryption) :
			base(SccpMessage.Type.OpenReceiveChannel)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.packetSize = packetSize;
			this.payload = payload;
			this.qualifier = qualifier;
			this.callReference = callReference;
			this.mediaEncryption = mediaEncryption;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP packet.
		/// </summary>
		public PayloadType payload;

		/// <summary>
		/// Extra qualifiers to incoming payload type.
		/// </summary>
		public MediaQualifierIncoming qualifier;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Media encryption information.
		/// </summary>
		public MediaEncryptionKey mediaEncryption;

		/// <summary>
		/// property whose value is the conference id from the message.
		/// </summary>
		public override uint ConferenceId { get { return conferenceId; } }

		/// <summary>
		/// Property whose value is the passthruPartyId from the message.
		/// </summary>
		public override uint PassthruPartyId { get { return passthruPartyId; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out packetSize);
			payload = (PayloadType)decoder.Decode();
			(qualifier = new MediaQualifierIncoming()).Decode(decoder);

			// ProtocolVersion.Parche ->
			decoder.Decode(out callReference);
			(mediaEncryption = new MediaEncryptionKey()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(packetSize);
			encoder.Encode((uint)payload);
			encoder.Encode(qualifier, typeof(MediaQualifierIncoming));
			encoder.Encode(callReference);
			encoder.Encode(mediaEncryption, typeof(MediaEncryptionKey));
		}
	}

	/// <summary>
	/// Where to send unicast RTP stream.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class OpenReceiveChannelAck : SccpMessage
	{
		/// <summary>
		/// Constructs an OpenReceiveChannelAck for assigning fields later.
		/// </summary>
		public OpenReceiveChannelAck() : this(Status.Ok, null, 0, 0) { }

		/// <summary>
		/// Constructs an OpenReceiveChannelAck.
		/// </summary>
		/// <param name="status">Status.</param>
		/// <param name="address">IPEndPoint address of where to send the RTP
		/// stream.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public OpenReceiveChannelAck(Status status, IPEndPoint address,
			uint passthruPartyId, uint callReference) :
			base(SccpMessage.Type.OpenReceiveChannelAck)
		{
			this.status = status;
			this.address = address;
			this.passthruPartyId = passthruPartyId;
			this.callReference = callReference;
		}

		/// <summary>
		/// Whether the OpenReceiveChannel has been accepted.
		/// </summary>
		public enum Status
		{
			Ok = 0,
			Error = 1,
		}

		/// <summary>
		/// Status.
		/// </summary>
		public Status status;

		/// <summary>
		/// IPEndPoint address of where to send the RTP stream.
		/// </summary>
		public IPEndPoint address;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			status = (Status)decoder.Decode();
			decoder.Decode(out address);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)status);
			encoder.Encode(address);
			encoder.Encode(passthruPartyId);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Register with CallManager.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class Register : SccpMessage
	{
		/// <summary>
		/// Constructs a Register for assigning fields later.
		/// </summary>
		public Register() : this(null) { }

		/// <summary>
		/// Constructs a Register with the sid as parameter, defaulting device
		/// type to Cisco IP Phone 7960.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		public Register(Sid sid) :
			this(sid, DeviceType.StationTelecasterMgr) { }

		/// <summary>
		/// Constructs a Register with just the major parameters, defaulting to
		/// Parche.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="deviceType">Type of SCCP client device.</param>
		public Register(Sid sid, DeviceType deviceType) :
			this(sid, deviceType, ProtocolVersion.Parche) { }

		/// <summary>
		/// Constructs a Register with just the major parameters, using the
		/// first NIC's IP address.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="deviceType">Type of SCCP client device.</param>
		/// <param name="protocolVersion">Version of the SCCP protocol that the
		/// client supports.</param>
		public Register(Sid sid, DeviceType deviceType,
			ProtocolVersion protocolVersion) :
			this(sid, IpUtility.ResolveHostname(Dns.GetHostName()),
			deviceType, protocolVersion) { }

		/// <summary>
		/// Constructs a Register with just the major parameters; all others
		/// default to 0.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="ipAddress">IPAddress of the client.</param>
		/// <param name="deviceType">Type of SCCP client device.</param>
		/// <param name="protocolVersion">Version of the SCCP protocol that the
		/// client supports.</param>
		public Register(Sid sid, IPAddress ipAddress, DeviceType deviceType,
			ProtocolVersion protocolVersion) :
			this(sid, ipAddress, deviceType, 0, 0, protocolVersion, 0, 0) { }

		/// <summary>
		/// Constructs a Register with all parameters.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="ipAddress">IPAddress of the client.</param>
		/// <param name="deviceType">Type of SCCP client device.</param>
		/// <param name="maxStreams">Maximum number of simultaneous RTP duplex
		/// streams that the client can handle.</param>
		/// <param name="activeStreams">Active stream that the CallManager
		/// needs to clean up resources for call preservation.</param>
		/// <param name="protocolVersion">Version of the SCCP protocol that the
		/// client supports.</param>
		/// <param name="maxConferences">Maximum number of simultaneous
		/// conferences that the client can handle.</param>
		/// <param name="activeConferences">Number of active
		/// conferences.</param>
		public Register(Sid sid, IPAddress ipAddress, DeviceType deviceType,
			uint maxStreams, uint activeStreams,
			ProtocolVersion protocolVersion, uint maxConferences,
			uint activeConferences) :
			base(SccpMessage.Type.Register)
		{
			this.sid = sid;
			this.ipAddress = ipAddress;
			this.deviceType = deviceType;
			this.maxStreams = maxStreams;
			this.activeStreams = activeStreams;
			this.protocolVersion = protocolVersion;
			this.maxConferences = maxConferences;
			this.activeConferences = activeConferences;
		}

		/// <summary>
		/// An SCCP Client IDentifier, a.k.a., sid or device name.
		/// </summary>
		public Sid sid;

		/// <summary>
		/// IPAddress of the client.
		/// </summary>
		public IPAddress ipAddress;

		/// <summary>
		/// Type of SCCP client device.
		/// </summary>
		public DeviceType deviceType;

		/// <summary>
		/// Maximum number of simultaneous RTP duplex streams that the client
		/// can handle.
		/// </summary>
		public uint maxStreams;

		/// <summary>
		/// Active stream that the CallManager needs to clean up resources
		/// for call preservation.
		/// </summary>
		/// <remarks>
		/// I don't understand this Cisco explanation.
		/// </remarks>
		public uint activeStreams;

		/// <summary>
		/// Version of the SCCP protocol that the client supports.
		/// </summary>
		public ProtocolVersion protocolVersion;

		/// <summary>
		/// Maximum number of simultaneous conferences that the client can
		/// handle.
		/// </summary>
		public uint maxConferences;

		/// <summary>
		/// Number of active conferences.
		/// </summary>
		public uint activeConferences;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			uint uintProtocolVersion;

			decoder.Decode(out sid);
			decoder.Decode(out ipAddress);
			deviceType = (DeviceType)decoder.Decode();
			decoder.Decode(out maxStreams);
			decoder.Decode(out activeStreams);
			decoder.Decode(out uintProtocolVersion);
			uintProtocolVersion &= 0xff;	// Look at lower-order byte.
			if (Enum.IsDefined(typeof(ProtocolVersion),
				(int)uintProtocolVersion))
			{
				protocolVersion = (ProtocolVersion)uintProtocolVersion;
			}
			else
			{
				// (Just set it to something.)
				protocolVersion = ProtocolVersion.Parche;
				// TBD - maybe should log a diagnostic or something?
			}
			decoder.Decode(out maxConferences);
			decoder.Decode(out activeConferences);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(sid);
			encoder.Encode(ipAddress);
			encoder.Encode((uint)deviceType);
			encoder.Encode(maxStreams);
			encoder.Encode(activeStreams);
			encoder.Encode((uint)protocolVersion);
			encoder.Encode(maxConferences);
			encoder.Encode(activeConferences);
		}
	}

	/// <summary>
	/// Register available lines.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class RegisterAvailableLines : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterAvailableLines for assigning fields later;
		/// default is 1 line.
		/// </summary>
		public RegisterAvailableLines() : this(1) { }

		/// <summary>
		/// Constructs a RegisterAvailableLines with maximum number of lines
		/// as parameter.
		/// </summary>
		/// <param name="maxNumberAvailableLines">Maximum number of
		/// lines.</param>
		public RegisterAvailableLines(int maxNumberAvailableLines) :
			base(SccpMessage.Type.RegisterAvailableLines)
		{
			this.maxNumberAvailableLines = (uint)maxNumberAvailableLines;
		}

		/// <summary>
		/// Maximum number of lines.
		/// </summary>
		public uint maxNumberAvailableLines;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out maxNumberAvailableLines);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(maxNumberAvailableLines);
		}
	}

	/// <summary>
	/// Registration is complete.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class RegisterAck : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterAck for assigning fields later.
		/// </summary>
		public RegisterAck() :
			this(0, null, 0, 0, false, false, false, false, false, false) { }

		/// <summary>
		/// Constructs a RegisterAck.
		/// </summary>
		/// <param name="keepaliveInterval1">How often, in seconds, that the
		/// client must send Keepalives to the primary CallManager.</param>
		/// <param name="dateTemplate">How to display the date and time,
		/// e.g., "M/D/YA" for "10/03/99 1:22PM".</param>
		/// <param name="keepaliveInterval2">How often, in seconds, that the
		/// client must send Keepalives to the secondary CallManager.</param>
		/// <param name="maxProtocolVersion">Maximum protocol version.</param>
		/// <param name="internationalization">Whether
		/// internationalization for softkey and text.</param>
		/// <param name="multipleActiveCall">Whether multiple-active-call
		/// capability.</param>
		/// <param name="mediaEncryption">Whether media encryption
		/// support.</param>
		/// <param name="internalCallManagerMedia">Whether media terminated by
		/// CallManager media device - Gravity
		/// Probe.</param>
		/// <param name="authenticatedSignaling">Whether
		/// signaling-authenticated capability.</param>
		/// <param name="restrictPresentationInformation">Whether
		/// restrict-presentation-information capability.</param>
		public RegisterAck(uint keepaliveInterval1, string dateTemplate,
			uint keepaliveInterval2, uint maxProtocolVersion,
			bool internationalization, bool multipleActiveCall,
			bool mediaEncryption, bool internalCallManagerMedia,
			bool authenticatedSignaling, bool restrictPresentationInformation) :
			base(SccpMessage.Type.RegisterAck)
		{
			this.keepaliveInterval1 = keepaliveInterval1;
			this.dateTemplate = dateTemplate;
			this.keepaliveInterval2 = keepaliveInterval2;
			this.maxProtocolVersion = maxProtocolVersion;
			this.internationalization = internationalization;
			this.multipleActiveCall = multipleActiveCall;
			this.mediaEncryption = mediaEncryption;
			this.internalCallManagerMedia = internalCallManagerMedia;
			this.authenticatedSignaling = authenticatedSignaling;
			this.restrictPresentationInformation =
				restrictPresentationInformation;
		}

		/// <summary>
		/// How often, in seconds, that the client must send Keepalives to the
		/// primary CallManager.
		/// </summary>
		public uint keepaliveInterval1;

		/// <summary>
		/// How to display the date and time, e.g., "M/D/YA" for
		/// "10/03/99 1:22PM".
		/// </summary>
		public string dateTemplate;

		/// <summary>
		/// How often, in seconds, that the client must send Keepalives to the
		/// secondary CallManager.
		/// </summary>
		public uint keepaliveInterval2;

		/// <summary>
		/// Maximum protocol version.
		/// </summary>
		public uint maxProtocolVersion;

		/// <summary>
		/// Whether internationalization for softkey and text.
		/// </summary>
		public bool internationalization;

		/// <summary>
		/// Whether multiple-active-call capability.
		/// </summary>
		public bool multipleActiveCall;

		/// <summary>
		/// Whether media encryption support.
		/// </summary>
		/// <remarks>
		/// (Janus)
		/// </remarks>
		public bool mediaEncryption;

		/// <summary>
		/// Whether media terminated by CallManager media device - Gravity
		/// Probe.
		/// </summary>
		public bool internalCallManagerMedia;

		/// <summary>
		/// Whether signaling-authenticated capability.
		/// </summary>
		public bool authenticatedSignaling;

		/// <summary>
		/// Whether restrict-presentation-information capability.
		/// </summary>
		public bool restrictPresentationInformation;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out keepaliveInterval1);
			decoder.Decode(out dateTemplate, Const.DateTemplateSize);
			decoder.Decode(out keepaliveInterval2);

			decoder.Decode(out maxProtocolVersion);
			internationalization =
				(maxProtocolVersion & (uint)0x80000000) != 0;
			multipleActiveCall = (maxProtocolVersion & (uint)0x40000000) != 0;
			mediaEncryption = (maxProtocolVersion & (uint)0x20000000) != 0;
			internalCallManagerMedia =
				(maxProtocolVersion & (uint)0x10000000) != 0;
			authenticatedSignaling =
				(maxProtocolVersion & (uint)0x08000000) != 0;
			restrictPresentationInformation =
				(maxProtocolVersion & (uint)0x04000000) != 0;
			maxProtocolVersion &= (uint)0x0000ffff;
			// maxProtocolVersion was added after Bravo so, if missing, this
			// must be Bravo.
			if (maxProtocolVersion == 0)
			{
				maxProtocolVersion = (uint)ProtocolVersion.Bravo;
			}
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(keepaliveInterval1);
			encoder.Encode(dateTemplate, Const.DateTemplateSize);
			encoder.Encode(keepaliveInterval2);
			maxProtocolVersion &= (uint)0x0000ffff;
			maxProtocolVersion |= internationalization ? (uint)0x80000000 : 0;
			maxProtocolVersion |= multipleActiveCall ? (uint)0x40000000 : 0;
			maxProtocolVersion |= mediaEncryption ? (uint)0x20000000 : 0;
			maxProtocolVersion |= internalCallManagerMedia ? (uint)0x10000000 : 0;
			maxProtocolVersion |=
				authenticatedSignaling ? (uint)0x08000000 : 0;
			maxProtocolVersion |=
				restrictPresentationInformation ? (uint)0x04000000 : 0;
			encoder.Encode(maxProtocolVersion);
		}
	}

	/// <summary>
	/// Registration has been rejected.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class RegisterReject : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterReject for assigning fields later.
		/// </summary>
		public RegisterReject() : this(null) { }

		/// <summary>
		/// Constructs a RegisterReject.
		/// </summary>
		/// <param name="text">Textual reason for rejection.</param>
		public RegisterReject(string text) :
			base(SccpMessage.Type.RegisterReject)
		{
			this.text = text;
		}

		/// <summary>
		/// Textual reason for rejection.
		/// </summary>
		public string text;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out text, Const.DisplayTextSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(text, Const.DisplayTextSize);
		}
	}

	/// <summary>
	/// Token request has been acknowledged.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class RegisterTokenAck : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterTokenAck.
		/// </summary>
		public RegisterTokenAck() : base(SccpMessage.Type.RegisterTokenAck) { }

		// (no fields)
	}

	/// <summary>
	/// Token request has been rejected. Client should try later.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class RegisterTokenReject : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterTokenReject for assigning fields later.
		/// </summary>
		public RegisterTokenReject() : this(0) { }

		/// <summary>
		/// Constructs a RegisterTokenReject.
		/// </summary>
		/// <param name="waitTimeBeforeNextReg">How long client should wait (in
		/// seconds) before making another token request.</param>
		public RegisterTokenReject(uint waitTimeBeforeNextReg) :
			base(SccpMessage.Type.RegisterTokenReject)
		{
			this.waitTimeBeforeNextReg = waitTimeBeforeNextReg;
		}

		/// <summary>
		/// How long client should wait (in seconds) before making another
		/// token request.
		/// </summary>
		public uint waitTimeBeforeNextReg;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out waitTimeBeforeNextReg);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(waitTimeBeforeNextReg);
		}
	}

	/// <summary>
	/// Request to register.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class RegisterTokenReq : SccpMessage
	{
		/// <summary>
		/// Constructs a RegisterTokenReq for assigning fields later.
		/// </summary>
		public RegisterTokenReq() :
			this(null, null, DeviceType.StationTelecasterMgr) { }

		/// <summary>
		/// Constructs a RegisterTokenReq.
		/// </summary>
		/// <param name="sid">An SCCP Client IDentifier, a.k.a., sid or device
		/// name.</param>
		/// <param name="ipAddress">IPAddress of the client.</param>
		/// <param name="deviceType">Type of SCCP client device.</param>
		public RegisterTokenReq(Sid sid, IPAddress ipAddress,
			DeviceType deviceType) :
			base(SccpMessage.Type.RegisterTokenReq)
		{
			this.sid = sid;
			this.ipAddress = ipAddress;
			this.deviceType = deviceType;
		}

		/// <summary>
		/// An SCCP Client IDentifier, a.k.a., sid or device name.
		/// </summary>
		public Sid sid;

		/// <summary>
		/// IPAddress of the client.
		/// </summary>
		public IPAddress ipAddress;

		/// <summary>
		/// Type of SCCP client device.
		/// </summary>
		public DeviceType deviceType;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out sid);
			decoder.Decode(out ipAddress);
			deviceType = (DeviceType)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(sid);
			encoder.Encode(ipAddress);
			encoder.Encode((uint)deviceType);
		}
	}

	/// <summary>
	/// Command for client to unregister.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class Reset : SccpMessage
	{
		/// <summary>
		/// Constructs a Reset for assigning reset type later.
		/// </summary>
		public Reset() : this(Reset.ResetType.Reset) { }

		/// <summary>
		/// Constructs a Reset.
		/// </summary>
		/// <param name="type">Type of reset.</param>
		public Reset(ResetType type) :
			base(SccpMessage.Type.Reset)
		{
			this.type = type;
		}

		/// <summary>
		/// Whether to reset or restart.
		/// </summary>
		public enum ResetType
		{
			Reset = 1,
			Restart = 2,
		}

		/// <summary>
		/// Type of reset.
		/// </summary>
		public ResetType type;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			type = (ResetType)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)type);
		}
	}

	/// <summary>
	/// Update softkey mapping.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SelectSoftkeys : SccpMessage
	{
		/// <summary>
		/// Constructs a SelectSoftkeys for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public SelectSoftkeys() : this(1, 0, 0, 0) { }

		/// <summary>
		/// Constructs a SelectSoftkeys.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="reference">An index to the specific call instance
		/// within a single line instance.</param>
		/// <param name="softkeySetIndex">New softkey set number for this
		/// context.</param>
		/// <param name="validKeyMask">Bit mask representing valid softkeys for
		/// this set where least-significant bit represents softkey 0.</param>
		public SelectSoftkeys(uint lineNumber, uint reference,
			uint softkeySetIndex, uint validKeyMask) :
			base(SccpMessage.Type.SelectSoftkeys)
		{
			this.lineNumber = lineNumber;
			this.reference = reference;
			this.softkeySetIndex = softkeySetIndex;
			this.validKeyMask = validKeyMask;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// An index to the specific call instance within a single line
		/// instance.
		/// </summary>
		public uint reference;

		/// <summary>
		/// New softkey set number for this context.
		/// </summary>
		public uint softkeySetIndex;

		/// <summary>
		/// Bit mask representing valid softkeys for this set where
		/// least-significant bit represents softkey 0.
		/// </summary>
		public uint validKeyMask;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return reference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out lineNumber);
			decoder.Decode(out reference);
			decoder.Decode(out softkeySetIndex);
			decoder.Decode(out validKeyMask);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(reference);
			encoder.Encode(softkeySetIndex);
			encoder.Encode(validKeyMask);
		}
	}

	/// <summary>
	/// Request list of CallManagers.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class ServerReq : SccpMessage
	{
		/// <summary>
		/// Constructs a ServerReq.
		/// </summary>
		public ServerReq() : base(SccpMessage.Type.ServerReq) { }

		// (no fields)
	}

	/// <summary>
	/// List of CallManagers.
	/// </summary>
	/// <remarks>CallManager to client.
	/// The internal representation is, IMO, organized better than the
	/// same information in the SCCP message.</remarks>
	public class ServerRes : SccpMessage
	{
		/// <summary>
		/// Constructs a ServerRes for assigning fields later.
		/// </summary>
		public ServerRes() : this(null) { }

		/// <summary>
		/// Constructs a ServerRes.
		/// </summary>
		/// <param name="servers">Arraylist of Server objects.</param>
		public ServerRes(ArrayList servers) : base(SccpMessage.Type.ServerRes)
		{
			this.servers = servers;
		}

		/// <summary>
		/// Binds CallManager name and address.
		/// </summary>
		public class Server
		{
			public string name;
			public IPEndPoint address;
		}

		/// <summary>
		/// ArrayList of Server objects.
		/// </summary>
		public ArrayList servers;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			// Decode separate name, port, and address lists first,
			// then combine them in the server list.
			ArrayList names = new ArrayList();
			for (int i = 0; i < Const.MaxServers; ++i)
			{
				string name;
				decoder.Decode(out name, Const.DirectoryNumberSize);
				names.Add(name);
			}
			ArrayList ports = new ArrayList();
			for (int i = 0; i < Const.MaxServers; ++i)
			{
				uint port;
				decoder.Decode(out port);
				ports.Add((int)port);
			}
			ArrayList addresses = new ArrayList();
			for (int i = 0; i < Const.MaxServers; ++i)
			{
				IPAddress ipAddress;
				decoder.Decode(out ipAddress);
				addresses.Add(ipAddress);
			}

			servers = new ArrayList();
			for (int i = 0; i < Const.MaxServers; ++i)
			{
				Server server = new Server();
				server.name = (string)names[i];
				server.address =
					new IPEndPoint((IPAddress)addresses[i], (int)ports[i]);
				servers.Add(server);
			}
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			int count = servers == null ? 0 : servers.Count;

			if (servers != null)
			{
				foreach (Server server in servers)
				{
					encoder.Encode(server.name, Const.DirectoryNumberSize);
				}
				encoder.Advance(
					Const.DirectoryNumberSize * (Const.MaxServers - count));
				foreach (Server server in servers)
				{
					encoder.Encode((uint)server.address.Port);
				}
				encoder.Advance(Const.UintSize * (Const.MaxServers - count));
				foreach (Server server in servers)
				{
					encoder.Encode(server.address.Address);
				}
				encoder.Advance(Const.UintSize * (Const.MaxServers - count));
			}
			else
			{
				encoder.Advance(Const.DirectoryNumberSize * Const.MaxServers);
				encoder.Advance(Const.UintSize * Const.MaxServers);
				encoder.Advance(Const.UintSize * Const.MaxServers);
			}
		}
	}

	/// <summary>
	/// Service URL stat
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class ServiceUrlStat : SccpMessage
	{
		/// <summary>
		/// Constructs a ServiceUrlStat for assigning fields later.
		/// </summary>
		public ServiceUrlStat() : this(null) { }

		/// <summary>
		/// Constructs a ServiceUrlStat.
		/// </summary>
		/// <param name="serviceUrl">Service URL status for a particular
		/// button.</param>
		public ServiceUrlStat(ServiceUrl serviceUrl) :
			base(SccpMessage.Type.ServiceUrlStat)
		{
			this.serviceUrl = serviceUrl;
		}

		/// <summary>
		/// Service URL status for a particular button.
		/// </summary>
		public ServiceUrl serviceUrl;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			serviceUrl = new ServiceUrl();
			decoder.Decode(out serviceUrl.number);
			decoder.Decode(out serviceUrl.url, Const.MaxServiceUrlSize);
			decoder.Decode(out serviceUrl.displayName,
				Const.DirectoryNameSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(serviceUrl.number);
			encoder.Encode(serviceUrl.url, Const.MaxServiceUrlSize);
			encoder.Encode(serviceUrl.displayName, Const.DirectoryNameSize);
		}
	}

	/// <summary>
	/// Service URL stat request.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class ServiceUrlStatReq : SccpMessage
	{
		/// <summary>
		/// Constructs a ServiceUrlStatReq for assigning index later.
		/// </summary>
		public ServiceUrlStatReq() : this(0) { }

		/// <summary>
		/// Constructs a ServiceUrlStatReq.
		/// </summary>
		/// <param name="index">Index of service URL being requested.</param>
		public ServiceUrlStatReq(int index) :
			base(SccpMessage.Type.ServiceUrlStatReq)
		{
			this.index = (uint)index;
		}

		/// <summary>
		/// Index of service URL being requested.
		/// </summary>
		public uint index;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out index);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(index);
		}
	}

	/// <summary>
	/// Set mode for specific lamp.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SetLamp : SccpMessage
	{
		/// <summary>
		/// Constructs a SetLamp for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public SetLamp() : this(DeviceStimulus.LastNumberRedial, 1,
			SetLamp.Mode.Off) { }

		/// <summary>
		/// Constructs a SetLamp.
		/// </summary>
		/// <param name="stimulus">Stimulus.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="mode">What to do with lamp.</param>
		public SetLamp(DeviceStimulus stimulus, uint lineNumber, Mode mode) :
			base(SccpMessage.Type.SetLamp)
		{
			this.stimulus = stimulus;
			this.lineNumber = lineNumber;
			this.mode = mode;
		}

		/// <summary>
		/// What to do with lamp.
		/// </summary>
		public enum Mode
		{
			Off = 1,	// Off (on 0%)
			On = 2,		// On (on 100%)
			Wink = 3,	// Hold, Wink (on 80%) = 448 ms on / 64 ms off
			Flash = 4,	// E-Mail, Flash (on 50%) = 32 ms on / 32 ms off
			Blink = 5,	// Blink (on 50%) = 512 ms on / 512 ms off
		}

		/// <summary>
		/// Stimulus.
		/// </summary>
		public DeviceStimulus stimulus;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// What to do with lamp.
		/// </summary>
		public Mode mode;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			stimulus = (DeviceStimulus)decoder.Decode();
			decoder.Decode(out lineNumber);
			mode = (Mode)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)stimulus);
			encoder.Encode(lineNumber);
			encoder.Encode((uint)mode);
		}
	}

	/// <summary>
	/// Set specified audible ringing mode.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SetRinger : SccpMessage
	{
		/// <summary>
		/// Constructs a SetRinger for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public SetRinger() :
			this(SetRinger.Mode.Off, SetRinger.Duration.Normal, 1, 0) { }

		/// <summary>
		/// Constructs a SetRinger.
		/// </summary>
		/// <param name="mode">What kind of ringer.</param>
		/// <param name="duration">Duration of ring.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public SetRinger(Mode mode, Duration duration, uint lineNumber,
			uint callReference) : base(SccpMessage.Type.SetRinger)
		{
			this.mode = mode;
			this.duration = duration;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// What kind of ring.
		/// </summary>
		public enum Mode
		{
			Off = 1,
			Inside = 2,
			Outside = 3,
			Feature = 4,
			FlashOnly = 5,
			Precedence = 6,
		}

		/// <summary>
		/// How long to ring.
		/// </summary>
		public enum Duration
		{
			Normal = 1,
			Single = 2,
		}

		/// <summary>
		/// What kind of ringer.
		/// </summary>
		public Mode mode;

		/// <summary>
		/// How long to ring.
		/// </summary>
		public Duration duration;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			mode = (Mode)decoder.Decode();

			// ProtocolVersion.Seaview ->
			if (decoder.More)
			{
				duration = (Duration)decoder.Decode();
				decoder.Decode(out lineNumber);
			}
			else
			{
				duration = Duration.Normal;
				lineNumber = 1;
			}

			// ProtocolVersion.Parche ->
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)mode);
			encoder.Encode((uint)duration);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Set speaker on/off.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SetSpeakerMode : SccpMessage
	{
		/// <summary>
		/// Constructs a SetSpeakerMode for assigning fields later.
		/// </summary>
		public SetSpeakerMode() : this(false) { }

		/// <summary>
		/// Constructs a SetSpeakerMode.
		/// </summary>
		/// <param name="on">Whether speaker should be turned on.</param>
		public SetSpeakerMode(bool on) :
			base(SccpMessage.Type.SetSpeakerMode)
		{
			this.on = on;
		}

		/// <summary>
		/// Whether speaker should be turned on.
		/// </summary>
		public bool on;

		/// <summary>
		/// Used to translate between SCCP message field and our bool.
		/// </summary>
		private enum SpeakerMode
		{
			On = 1,
			Off = 2,
		}

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out on, (uint)SpeakerMode.On);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)(on ? SpeakerMode.On : SpeakerMode.Off));
		}
	}

	/// <summary>
	/// Set microphone on/off.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SetMicroMode : SccpMessage
	{
		/// <summary>
		/// Constructs a SetMicroMode for assigning fields later.
		/// </summary>
		public SetMicroMode() : this(false) { }

		/// <summary>
		/// Constructs a SetMicroMode.
		/// </summary>
		/// <param name="on">Whether microphone should be turned on.</param>
		public SetMicroMode(bool on) : base(SccpMessage.Type.SetMicroMode)
		{
			this.on = on;
		}

		/// <summary>
		/// Whether microphone should be turned on.
		/// </summary>
		public bool on;

		/// <summary>
		/// Used to translate between SCCP message field and our bool.
		/// </summary>
		private enum MicrophoneMode
		{
			On = 1,
			Off = 2,
		}

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out on, (uint)MicrophoneMode.On);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(
				(uint)(on ? MicrophoneMode.On : MicrophoneMode.Off));
		}
	}

	/// <summary>
	/// Reports softkey events.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class SoftkeyEvent : SccpMessage
	{
		/// <summary>
		/// Constructs a SoftkeyEvent for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public SoftkeyEvent() : this(0, 1, 0) { }

		/// <summary>
		/// Constructs a SoftkeyEvent.
		/// </summary>
		/// <param name="event_">Which softkey was pressed.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public SoftkeyEvent(uint event_, uint lineNumber, uint callReference) :
			base(SccpMessage.Type.SoftkeyEvent)
		{
			this.event_ = event_;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Which softkey was pressed.
		/// </summary>
		/// <remarks>
		/// Can use SoftkeyEventType.
		/// </remarks>
		public uint event_;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out event_);
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(event_);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Requests softkey set update/download.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class SoftkeySetReq : SccpMessage
	{
		/// <summary>
		/// Constructs a SoftkeySetReq.
		/// </summary>
		public SoftkeySetReq() : base(SccpMessage.Type.SoftkeySetReq) { }

		// (no fields)
	}

	/// <summary>
	/// Softkey sets.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SoftkeySetRes : SccpMessage
	{
		/// <summary>
		/// Constructs a SoftkeySetRes for assigning fields later.
		/// </summary>
		public SoftkeySetRes() : this(null) { }

		/// <summary>
		/// Constructs a SoftkeySetRes.
		/// </summary>
		/// <param name="set_">The requested softkey set.</param>
		public SoftkeySetRes(Set set_) :
			base(SccpMessage.Type.SoftkeySetRes)
		{
			this.set_ = set_;
		}

		/// <summary>
		/// Softkey set.
		/// </summary>
		public class Set : SccpMessageStruct
		{
			/// <summary>
			/// Softkey definition.
			/// </summary>
			public class Definition : SccpMessageStruct
			{
				/// <summary>
				/// Set of indexes into the softkey template.
				/// </summary>
				public byte[] templateIndex;

				/// <summary>
				/// Set of indexes into the softket description information.
				/// </summary>
				public ushort[] infoIndex;

				/// <summary>
				/// Decodes this structure from raw message to internal member
				/// fields.
				/// </summary>
				/// <param name="decoder">Keeps track of decoding
				/// progress.</param>
				internal override void Decode(Decoder decoder)
				{
					decoder.Decode(out templateIndex, Const.MaxSoftkeyIndexes,
						Const.MaxSoftkeyIndexes);
					decoder.Decode(out infoIndex, Const.MaxSoftkeyIndexes,
						Const.MaxSoftkeyIndexes);
				}

				/// <summary>
				/// Encodes this aggregate from internal member fields to raw
				/// message.
				/// </summary>
				/// <param name="encoder">Keeps track of encoding
				/// progress.</param>
				internal override void Encode(Encoder encoder)
				{
					encoder.Encode(templateIndex, Const.MaxSoftkeyIndexes);
					encoder.Encode(infoIndex, Const.MaxSoftkeyIndexes);
				}

				/// <summary>
				/// Returns the size of this aggregate in an actual SCCP
				/// message.
				/// </summary>
				/// <returns>Aggregate size.</returns>
				internal override long SizeOf()
				{
					return Const.ByteSize * Const.MaxSoftkeyIndexes +
						Const.UshortSize * Const.MaxSoftkeyIndexes;
				}
			}

			/// <summary>
			/// Table offset of softkeys. Normally 0.
			/// </summary>
			public uint offset;

			/// <summary>
			/// Total in this set, not just in this message.
			/// </summary>
			public uint total;

			/// <summary>
			/// Arraylist of Definition objects.
			/// </summary>
			public ArrayList definitions;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				uint count;

				decoder.Decode(out offset);
				decoder.Decode(out count);
				decoder.Decode(out total);
				definitions = new ArrayList();
				for (uint i = 0; i < count; ++i)
				{
					Definition definition = new Definition();
					definition.Decode(decoder);
					definitions.Add(definition);
				}
				new Definition().Advance(decoder,
					Const.MaxSoftkeySetDefinitions - count);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				uint count =
					(uint)(definitions == null ? 0 : definitions.Count);

				encoder.Encode(offset);
				encoder.Encode(count);
				encoder.Encode(total);
				if (definitions != null)
				{
					foreach (Definition definition in definitions)
					{
						definition.Encode(encoder);
					}
				}
				new Definition().Advance(encoder,
					Const.MaxSoftkeySetDefinitions - count);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return Marshal.SizeOf(offset) +
					Const.UintSize +
					Marshal.SizeOf(total) +
					new Definition().SizeOf() * Const.MaxSoftkeySetDefinitions;
			}
		}

		/// <summary>
		/// The requested softkey set.
		/// </summary>
		public Set set_;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(set_ = new Set()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(set_, typeof(Set));
		}
	}

	/// <summary>
	/// Request current softkey template.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class SoftkeyTemplateReq : SccpMessage
	{
		/// <summary>
		/// Constructs a SoftkeyTemplateReq.
		/// </summary>
		public SoftkeyTemplateReq() :
			base(SccpMessage.Type.SoftkeyTemplateReq) { }

		// (no fields)
	}

	/// <summary>
	/// Softkey template information.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SoftkeyTemplateRes : SccpMessage
	{
		/// <summary>
		/// Constructs a SoftkeyTemplateRes for assigning fields later.
		/// </summary>
		public SoftkeyTemplateRes() : this(null) { }

		/// <summary>
		/// Constructs a SoftkeyTemplateRes with list of templates as
		/// parameter.
		/// </summary>
		/// <param name="softkeyTemplate">Softkey template information.</param>
		public SoftkeyTemplateRes(SoftkeyTemplate softkeyTemplate) :
			base(SccpMessage.Type.SoftkeyTemplateRes)
		{
			this.softkeyTemplate = softkeyTemplate;
		}

		/// <summary>
		/// Softkey template.
		/// </summary>
		public class SoftkeyTemplate : SccpMessageStruct
		{
			/// <summary>
			/// Constructs a SoftkeyTemplate for assigning fields later.
			/// </summary>
			public SoftkeyTemplate() : this(null) { }

			/// <summary>
			/// Constructs a SoftkeyTemplate with list of definitions as
			/// parameter; assuming offset is 0 and total definitions is same
			/// as number of these definitions.
			/// </summary>
			/// <param name="definitions">Softkey template definitions.</param>
			public SoftkeyTemplate(ArrayList definitions) :
				this(definitions == null ? 0 : (uint)definitions.Count, definitions) { }

			/// <summary>
			/// Constructs a SoftkeyTemplate with total definitions and list of
			/// template definitions as parameters and assuming offset is 0.
			/// </summary>
			/// <param name="definitions">Softkey template definitions.</param>
			/// <param name="total">Total in template, not just this
			/// message.</param>
			public SoftkeyTemplate(uint total, ArrayList definitions) :
				this(0, total, definitions) { }

			/// <summary>
			/// Constructs a SoftkeyTemplate with offset, total definitions,
			/// and list of template definitions as parameters.
			/// </summary>
			/// <param name="definitions">Softkey template definitions.</param>
			/// <param name="offset">Table offset of softkeys. Normally 0.</param>
			/// <param name="total">Total in template, not just this
			/// message.</param>
			public SoftkeyTemplate(uint offset, uint total,
				ArrayList definitions)
			{
				this.offset = offset;
				this.total = total;
				this.definitions = definitions;
			}

			/// <summary>
			/// Softkey definition.
			/// </summary>
			public class Definition : SccpMessageStruct
			{
				/// <summary>
				/// Constructs a Definition for assigning fields later.
				/// </summary>
				public Definition() : this(null, 0) { }

				/// <summary>
				/// Constructs a Definition with label and event as parameters.
				/// </summary>
				/// <param name="label">Label.</param>
				/// <param name="event_">Event.</param>
				public Definition(string label, uint event_)
				{
					definition = new SoftkeyDefinition(label, event_);
				}

				private SoftkeyDefinition definition;

				public string label
				{
					get { return definition.label; }
					set { definition.label = value; }
				}

				public uint event_
				{
					get { return definition.event_; }
					set { definition.event_ = value; }
				}

				/// <summary>
				/// Decodes this structure from raw message to internal member
				/// fields.
				/// </summary>
				/// <param name="decoder">Keeps track of decoding
				/// progress.</param>
				internal override void Decode(Decoder decoder)
				{
					decoder.Decode(out definition.label,
						Const.SoftkeyLabelSize);
					decoder.Decode(out definition.event_);
				}

				/// <summary>
				/// Encodes this aggregate from internal member fields to raw
				/// message.
				/// </summary>
				/// <param name="encoder">Keeps track of encoding
				/// progress.</param>
				internal override void Encode(Encoder encoder)
				{
					encoder.Encode(definition.label, Const.SoftkeyLabelSize);
					encoder.Encode(definition.event_);
				}

				/// <summary>
				/// Returns the size of this aggregate in an actual SCCP
				/// message.
				/// </summary>
				/// <returns>Aggregate size.</returns>
				internal override long SizeOf()
				{
					return Const.SoftkeyLabelSize +
						Marshal.SizeOf(definition.event_);
				}
			}

			/// <summary>
			/// Table offset of softkeys. Normally 0.
			/// </summary>
			public uint offset;

			/// <summary>
			/// Total in template, not just this message.
			/// </summary>
			public uint total;

			/// <summary>
			/// ArrayList of Definition objects.
			/// </summary>
			public ArrayList definitions;

			/// <summary>
			/// Decodes this structure from raw message to internal member
			/// fields.
			/// </summary>
			/// <param name="decoder">Keeps track of decoding progress.</param>
			internal override void Decode(Decoder decoder)
			{
				uint count;

				decoder.Decode(out offset);
				decoder.Decode(out count);
				decoder.Decode(out total);
				definitions = new ArrayList();
				for (uint i = 0; i < count; ++i)
				{
					Definition definition = new Definition();
					definition.Decode(decoder);
					definitions.Add(definition);
				}
				new Definition().Advance(decoder,
					Const.MaxSoftkeyDefinitions - count);
			}

			/// <summary>
			/// Encodes this aggregate from internal member fields to raw
			/// message.
			/// </summary>
			/// <param name="encoder">Keeps track of encoding progress.</param>
			internal override void Encode(Encoder encoder)
			{
				uint count =
					(uint)(definitions == null ? 0 : definitions.Count);

				encoder.Encode(offset);
				encoder.Encode(count);
				encoder.Encode(total);
				if (definitions != null)
				{
					foreach (Definition definition in definitions)
					{
						definition.Encode(encoder);
					}
				}
				new Definition().Advance(encoder,
					Const.MaxSoftkeyDefinitions - count);
			}

			/// <summary>
			/// Returns the size of this aggregate in an actual SCCP message.
			/// </summary>
			/// <returns>Aggregate size.</returns>
			internal override long SizeOf()
			{
				return Marshal.SizeOf(offset) +
					Const.UintSize +
					Marshal.SizeOf(total) +
					new Definition().SizeOf() * Const.MaxSoftkeyDefinitions;
			}
		}

		/// <summary>
		/// Softkey template information.
		/// </summary>
		public SoftkeyTemplate softkeyTemplate;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(softkeyTemplate = new SoftkeyTemplate()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(softkeyTemplate, typeof(SoftkeyTemplate));
		}
	}

	/// <summary>
	/// Directory number assigned to specified speed-dial button.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class SpeeddialStat : SccpMessage
	{
		/// <summary>
		/// Constructs a SpeeddialStat for assigning fields later.
		/// </summary>
		public SpeeddialStat() : this(0, null, null) { }

		/// <summary>
		/// Constructs a SpeeddialStat.
		/// </summary>
		/// <param name="number">Speed-dial button number.</param>
		/// <param name="directoryNumber">Directory number associated with this
		/// speed-dial button.</param>
		/// <param name="displayName">the display name to be associated with
		/// this speed-dial button.</param>
		public SpeeddialStat(uint number, string directoryNumber,
			string displayName) :
			base(SccpMessage.Type.SpeeddialStat)
		{
			speeddial = new Speeddial(number, directoryNumber, displayName);
		}

		/// <summary>
		/// Speed-dial-button information.
		/// </summary>
		public Speeddial speeddial;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			speeddial = new Speeddial();
			decoder.Decode(out speeddial.number);
			decoder.Decode(out speeddial.directoryNumber,
				Const.DirectoryNumberSize);
			decoder.Decode(out speeddial.displayName, Const.DirectoryNameSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(speeddial.number);
			encoder.Encode(speeddial.directoryNumber,
				Const.DirectoryNumberSize);
			encoder.Encode(speeddial.displayName, Const.DirectoryNameSize);
		}
	}

	/// <summary>
	/// Request directory number assigned to specified speed-dial button.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class SpeeddialStatReq : SccpMessage
	{
		/// <summary>
		/// Constructs a SpeeddialStatReq for assigning fields later.
		/// </summary>
		public SpeeddialStatReq() : this(0) { }

		/// <summary>
		/// Constructs a SpeeddialStatReq.
		/// </summary>
		/// <param name="number">Number of the speed-dial button about which
		/// information is being requested.</param>
		public SpeeddialStatReq(int number) :
			base(SccpMessage.Type.SpeeddialStatReq)
		{
			this.number = (uint)number;
		}

		/// <summary>
		/// Number of the speed-dial button about which information is being
		/// requested.
		/// </summary>
		public uint number;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out number);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(number);
		}
	}

	/// <summary>
	/// Start media failure detection.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartMediaFailureDetection : SccpMessage
	{
		/// <summary>
		/// Constructs a StartMediaFailureDetection for assigning fields later.
		/// </summary>
		public StartMediaFailureDetection() :
			this(0, 0, 0, PayloadType.G711Ulaw64k, null, 0) { }

		/// <summary>
		/// Constructs a StartMediaFailureDetection.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payload">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="qualifier">Extra qualifiers to incoming payload
		/// type.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StartMediaFailureDetection(uint conferenceId,
			uint passthruPartyId, uint packetSize, PayloadType payload,
			MediaQualifierIncoming qualifier, uint callReference) :
			base(SccpMessage.Type.StartMediaFailureDetection)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.packetSize = packetSize;
			this.payload = payload;
			this.qualifier = qualifier;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP
		/// packet.
		/// </summary>
		public PayloadType payload;

		/// <summary>
		/// Extra qualifiers to incoming payload type.
		/// </summary>
		public MediaQualifierIncoming qualifier;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out packetSize);
			payload = (PayloadType)decoder.Decode();
			(qualifier = new MediaQualifierIncoming()).Decode(decoder);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(packetSize);
			encoder.Encode((uint)payload);
			encoder.Encode(qualifier, typeof(MediaQualifierIncoming));
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Begin transmitting audio stream to remote RTP address/port.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartMediaTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StartMediaTransmission for assigning fields later.
		/// </summary>
		public StartMediaTransmission() :
			this(0, 0, null, 0, PayloadType.G711Ulaw64k, null, 0, null) { }

		/// <summary>
		/// Constructs a StartMediaTransmission.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="address">IPEndPoint address of where to send the RTP
		/// stream.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payload">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="qualifier">Extra qualifiers to outgoing payload
		/// type.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StartMediaTransmission(uint conferenceId, uint passthruPartyId,
			IPEndPoint address, uint packetSize, PayloadType payload,
			MediaQualifierOutgoing qualifier, uint callReference,
			MediaEncryptionKey mediaEncryption) :
			base(SccpMessage.Type.StartMediaTransmission)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.address = address;
			this.packetSize = packetSize;
			this.payload = payload;
			this.qualifier = qualifier;
			this.callReference = callReference;
			this.mediaEncryption = mediaEncryption;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// IPEndPoint address of where to send the RTP stream.
		/// </summary>
		public IPEndPoint address;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP
		/// packet.
		/// </summary>
		public PayloadType payload;

		/// <summary>
		/// Extra qualifiers to outgoing payload type.
		/// </summary>
		public MediaQualifierOutgoing qualifier;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		public MediaEncryptionKey mediaEncryption;

		/// <summary>
		/// property whose value is the conference id from the message.
		/// </summary>
		public override uint ConferenceId { get { return conferenceId; } }

		/// <summary>
		/// Property whose value is the passthruPartyId from the message.
		/// </summary>
		public override uint PassthruPartyId { get { return passthruPartyId; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out address);
			decoder.Decode(out packetSize);
			payload = (PayloadType)decoder.Decode();
			(qualifier = new MediaQualifierOutgoing()).Decode(decoder);

			// ProtocolVersion.Parche ->
			decoder.Decode(out callReference);
			(mediaEncryption = new MediaEncryptionKey()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(address);
			encoder.Encode(packetSize);
			encoder.Encode((uint)payload);
			encoder.Encode(qualifier, typeof(MediaQualifierOutgoing));
			encoder.Encode(callReference);
			encoder.Encode(mediaEncryption, typeof(MediaEncryptionKey));
		}
	}

	/// <summary>
	/// Begin monitoring RTP stream from specified multicast port.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartMulticastMediaReception : SccpMessage
	{
		/// <summary>
		/// Constructs a StartMulticastMediaReception for assigning fields
		/// later.
		/// </summary>
		public StartMulticastMediaReception() :
			this(0, 0, null, 0, PayloadType.G711Ulaw64k, null, 0) { }

		/// <summary>
		/// Constructs a StartMulticastMediaReception.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="address">IPEndPoint address of where to receive the
		/// multicast RTP stream.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payload">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="qualifier">Extra qualifiers to incoming payload
		/// type.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StartMulticastMediaReception(uint conferenceId,
			uint passthruPartyId, IPEndPoint address, uint packetSize,
			PayloadType payload, MediaQualifierIncoming qualifier,
			uint callReference) :
			base(SccpMessage.Type.StartMulticastMediaReception)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.address = address;
			this.packetSize = packetSize;
			this.payload = payload;
			this.qualifier = qualifier;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// IPEndPoint address of where to send the multicast RTP stream.
		/// </summary>
		public IPEndPoint address;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP
		/// packet.
		/// </summary>
		public PayloadType payload;

		/// <summary>
		/// Extra qualifiers to incoming payload type.
		/// </summary>
		public MediaQualifierIncoming qualifier;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out address);
			decoder.Decode(out packetSize);
			payload = (PayloadType)decoder.Decode();
			(qualifier = new MediaQualifierIncoming()).Decode(decoder);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(address);
			encoder.Encode(packetSize);
			encoder.Encode((uint)payload);
			encoder.Encode(qualifier, typeof(MediaQualifierIncoming));
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Start transmission to multicast address.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartMulticastMediaTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StartMulticastMediaTransmission for assigning fields
		/// later.
		/// </summary>
		public StartMulticastMediaTransmission() :
			this(0, 0, null, 0, PayloadType.G711Ulaw64k, null, 0) { }

		/// <summary>
		/// Constructs a StartMulticastMediaTransmission.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="address">IPEndPoint address of where to send the
		/// multicast RTP stream.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payload">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="qualifier">Extra qualifiers to outgoing payload
		/// type.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StartMulticastMediaTransmission(uint conferenceId,
			uint passthruPartyId, IPEndPoint address, uint packetSize,
			PayloadType payload, MediaQualifierOutgoing qualifier,
			uint callReference) :
			base(SccpMessage.Type.StartMulticastMediaTransmission)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.address = address;
			this.packetSize = packetSize;
			this.payload = payload;
			this.qualifier = qualifier;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// IPEndPoint address of where to send the multicast RTP stream.
		/// </summary>
		public IPEndPoint address;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP
		/// packet.
		/// </summary>
		public PayloadType payload;

		/// <summary>
		/// Extra qualifiers to outgoing payload type.
		/// </summary>
		public MediaQualifierOutgoing qualifier;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out address);
			decoder.Decode(out packetSize);
			payload = (PayloadType)decoder.Decode();
			(qualifier = new MediaQualifierOutgoing()).Decode(decoder);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(address);
			encoder.Encode(packetSize);
			encoder.Encode((uint)payload);
			encoder.Encode(qualifier, typeof(MediaQualifierOutgoing));
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Begin indicated session type to indicated remote IP address.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartSessionTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StartSessionTransmission for assigning fields later.
		/// </summary>
		public StartSessionTransmission() : this(null, null) { }

		/// <summary>
		/// Constructs a StartSessionTransmission.
		/// </summary>
		/// <param name="remoteIpAddress">IPAddress of the remote host that
		/// receives the RTP packets.</param>
		/// <param name="sessionType">Type of session.</param>
		public StartSessionTransmission(IPAddress remoteIpAddress,
			SessionType sessionType) :
			base(SccpMessage.Type.StartSessionTransmission)
		{
			this.remoteIpAddress = remoteIpAddress;
			this.sessionType = sessionType;
		}

		/// <summary>
		/// IPAddress of the remote host that receives the RTP packets.
		/// </summary>
		public IPAddress remoteIpAddress;

		/// <summary>
		/// Type of session.
		/// </summary>
		public SessionType sessionType;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out remoteIpAddress);
			(sessionType = new SessionType()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(remoteIpAddress);
			encoder.Encode(sessionType, typeof(SessionType));
		}
	}

	/// <summary>
	/// Play specified tone.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StartTone : SccpMessage
	{
		/// <summary>
		/// Constructs a StartTone for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public StartTone() : this(Tone.Silence, StartTone.Direction.User, 1, 0) { }

		/// <summary>
		/// Constructs a StartTone.
		/// </summary>
		/// <param name="tone">Tone to start.</param>
		/// <param name="direction">Direction of the tone.</param>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StartTone(Tone tone, Direction direction, uint lineNumber,
			uint callReference) :
			base(SccpMessage.Type.StartTone)
		{
			this.tone = tone;
			this.direction = direction;
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Which direction to play tone.
		/// </summary>
		public enum Direction
		{
			User = 0,
			Network = 1,
			All = 2,
		}

		/// <summary>
		/// Tone to start.
		/// </summary>
		public Tone tone;

		/// <summary>
		/// Direction of the tone.
		/// </summary>
		public Direction direction;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			tone = (Tone)decoder.Decode();

			// ProtocolVersion.Hawkbill ->
			direction = (Direction)decoder.Decode();

			// ProtocolVersion.Parche ->
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)tone);
			encoder.Encode((uint)direction);
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Stop transmitting audio stream to remote entity.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StopMediaTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StopMediaTransmission for assigning fields later.
		/// </summary>
		public StopMediaTransmission() : this(0, 0, 0) { }

		/// <summary>
		/// Constructs a StopMediaTransmission.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StopMediaTransmission(uint conferenceId, uint passthruPartyId,
			uint callReference) :
			base(SccpMessage.Type.StopMediaTransmission)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// property whose value is the conference id from the message.
		/// </summary>
		public override uint ConferenceId { get { return conferenceId; } }

		/// <summary>
		/// Property whose value is the passthruPartyId from the message.
		/// </summary>
		public override uint PassthruPartyId { get { return passthruPartyId; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);

			// ProtocolVersion.Parche ->
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Stop receiving multicast stream.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StopMulticastMediaReception : SccpMessage
	{
		/// <summary>
		/// Constructs a StopMulticastMediaReception for assigning fields
		/// later.
		/// </summary>
		public StopMulticastMediaReception() : this(0, 0, 0) { }

		/// <summary>
		/// Constructs a StopMulticastMediaReception.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StopMulticastMediaReception(uint conferenceId,
			uint passthruPartyId, uint callReference) :
			base(SccpMessage.Type.StopMulticastMediaReception)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Stop being source of RTP stream in multicast conference.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StopMulticastMediaTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StopMulticastMediaTransmission for assigning fields
		/// later.
		/// </summary>
		public StopMulticastMediaTransmission() : this(0, 0, 0) { }

		/// <summary>
		/// Constructs a StopMulticastMediaTransmission.
		/// </summary>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StopMulticastMediaTransmission(uint conferenceId,
			uint passthruPartyId, uint callReference) :
			base(SccpMessage.Type.StopMulticastMediaTransmission)
		{
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.callReference = callReference;
		}

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out conferenceId);
			decoder.Decode(out passthruPartyId);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(conferenceId);
			encoder.Encode(passthruPartyId);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// End indicated session.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StopSessionTransmission : SccpMessage
	{
		/// <summary>
		/// Constructs a StopSessionTransmission for assigning fields later.
		/// </summary>
		public StopSessionTransmission() : this(null, null) { }

		/// <summary>
		/// Constructs a StopSessionTransmission.
		/// </summary>
		/// <param name="remoteIpAddress">IPAddress of the remote host that
		/// receives the RTP packets.</param>
		/// <param name="sessionType">Type of session.</param>
		public StopSessionTransmission(IPAddress remoteIpAddress,
			SessionType sessionType) :
			base(SccpMessage.Type.StopSessionTransmission)
		{
			this.remoteIpAddress = remoteIpAddress;
			this.sessionType = sessionType;
		}

		/// <summary>
		/// IPAddress of the remote host that receives the RTP packets.
		/// </summary>
		public IPAddress remoteIpAddress;

		/// <summary>
		/// Type of session.
		/// </summary>
		public SessionType sessionType;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out remoteIpAddress);
			(sessionType = new SessionType()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(remoteIpAddress);
			encoder.Encode(sessionType, typeof(SessionType));
		}
	}

	/// <summary>
	/// Stop playing the current tone.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class StopTone : SccpMessage
	{
		/// <summary>
		/// Constructs a StopTone for assigning fields later.
		/// Defaults to line number 1, the first one.
		/// </summary>
		public StopTone() : this(1, 0) { }

		/// <summary>
		/// Constructs a StopTone.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="callReference">Uniquely identifies calls on the same
		/// device.</param>
		public StopTone(uint lineNumber, uint callReference) :
			base(SccpMessage.Type.StopTone)
		{
			this.lineNumber = lineNumber;
			this.callReference = callReference;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callReference;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			// ProtocolVersion.Parche ->
			decoder.Decode(out lineNumber);
			decoder.Decode(out callReference);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(lineNumber);
			encoder.Encode(callReference);
		}
	}

	/// <summary>
	/// Request current date/time.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class TimeDateReq : SccpMessage
	{
		/// <summary>
		/// Constructs a TimeDateReq.
		/// </summary>
		public TimeDateReq() : base(SccpMessage.Type.TimeDateReq) { }

		// (no fields)
	}

	/// <summary>
	/// Request that CallManager remove this client from its list of registered
	/// endpoints.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class Unregister : SccpMessage
	{
		/// <summary>
		/// Constructs an Unregister.
		/// </summary>
		public Unregister() : base(SccpMessage.Type.Unregister) { }

		// (no fields)
	}

	/// <summary>
	/// Acknowledges that client was removed from list of registered endpoints.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class UnregisterAck : SccpMessage
	{
		/// <summary>
		/// Constructs an UnregisterAck with status of Ok.
		/// </summary>
		public UnregisterAck() : this(Status.Ok) { }

		/// <summary>
		/// Constructs an UnregisterAck.
		/// </summary>
		/// <param name="status">Status of the unregistration request.</param>
		public UnregisterAck(Status status) :
			base(SccpMessage.Type.UnregisterAck)
		{
			this.status = status;
		}

		/// <summary>
		/// Status of the unregistration request.
		/// </summary>
		public enum Status
		{
			Ok = 0,
			Error = 1,
			Nak = 2,
		}

		/// <summary>
		/// Status of the unregistration request.
		/// </summary>
		public Status status;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			status = (Status)decoder.Decode();
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode((uint)status);
		}
	}

	/// <summary>
	/// Send user-related data.
	/// </summary>
	/// <remarks>Client to CallManager.</remarks>
	public class UserToDeviceData : SccpMessage
	{
		/// <summary>
		/// Constructs a UserToDeviceData for assigning fields later.
		/// </summary>
		public UserToDeviceData() : this(null) { }

		/// <summary>
		/// Constructs a UserToDeviceData.
		/// </summary>
		/// <param name="data">User-related data.</param>
		public UserToDeviceData(UserAndDeviceData data) :
			base(SccpMessage.Type.UserToDeviceData)
		{
			this.data = data;
		}

		/// <summary>
		/// User-related data.
		/// </summary>
		public UserAndDeviceData data;

		/// <summary>
		/// Property whose value is the line number from the message.
		/// </summary>
		public override uint Line { get { return data.lineNumber; } }

		/// <summary>
		/// Property whose value is the call reference from the message.
		/// </summary>
		public override uint CallId { get { return data.callReference; } }

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			(data = new UserAndDeviceData()).Decode(decoder);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(data, typeof(UserAndDeviceData));
		}
	}

    /// <summary>
    /// Send user-related data.
    /// </summary>
    /// <remarks>Client to CallManager.</remarks>
    public class UserToDeviceDataVersion1 : SccpMessage
    {
        /// <summary>
        /// Constructs a UserToDeviceData for assigning fields later.
        /// </summary>
        public UserToDeviceDataVersion1() : this(null) { }

        /// <summary>
        /// Constructs a UserToDeviceData.
        /// </summary>
        /// <param name="data">User-related data.</param>
        public UserToDeviceDataVersion1(UserAndDeviceDataVersion1 data)
            :
            base(SccpMessage.Type.UserToDeviceDataVersion1)
        {
            this.data = data;
        }

        /// <summary>
        /// User-related data.
        /// </summary>
        public UserAndDeviceDataVersion1 data;

        /// <summary>
        /// Property whose value is the line number from the message.
        /// </summary>
        public override uint Line { get { return data.lineNumber; } }

        /// <summary>
        /// Property whose value is the call reference from the message.
        /// </summary>
        public override uint CallId { get { return data.callReference; } }

        /// <summary>
        /// Decodes from raw message to internal member fields.
        /// </summary>
        /// <param name="decoder">Keeps track of decoding progress.</param>
        internal override void Decode(Decoder decoder)
        {
            (data = new UserAndDeviceDataVersion1()).Decode(decoder);
        }

        /// <summary>
        /// Encodes from internal member fields to raw message.
        /// </summary>
        /// <param name="encoder">Keeps track of encoding progress.</param>
        internal override void Encode(Encoder encoder)
        {
            encoder.Encode(data, typeof(UserAndDeviceDataVersion1));
        }
    }
    /// <summary>
	/// Version number of software that client should use.
	/// </summary>
	/// <remarks>CallManager to client.</remarks>
	public class Version_ : SccpMessage
	{
		/// <summary>
		/// Constructs a Version_ for assigning the version string later.
		/// </summary>
		public Version_() : this(null) { }

		/// <summary>
		/// Constructs a Version_.
		/// </summary>
		/// <param name="version">String that identifies the version of
		/// software that the client should use.</param>
		public Version_(string version) : base(SccpMessage.Type.Version_)
		{
			this.version = version;
		}

		/// <summary>
		/// String that identifies the version of software that the client
		/// should use.
		/// </summary>
		public string version;

		/// <summary>
		/// Decodes from raw message to internal member fields.
		/// </summary>
		/// <param name="decoder">Keeps track of decoding progress.</param>
		internal override void Decode(Decoder decoder)
		{
			decoder.Decode(out version, Const.MaxVersionSize);
		}

		/// <summary>
		/// Encodes from internal member fields to raw message.
		/// </summary>
		/// <param name="encoder">Keeps track of encoding progress.</param>
		internal override void Encode(Encoder encoder)
		{
			encoder.Encode(version, Const.MaxVersionSize);
		}
	}
}

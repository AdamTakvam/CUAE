using System;

namespace Metreos.Core.IPC.Flatmaps
{
	/// <summary>
	/// Class for IPC Flatmap message header extension.
	/// </summary>
	public class HeaderExtension
	{
		// Length of int fields in header-extension byte array.
		private const int IntLengthInHeaderExtension = 4;

		// Length of message type in header-extension byte array.
		private const int MessageTypeByteLength = IntLengthInHeaderExtension;

		//
		// List of header-extension fields.
		// Additional fields may be added in the future.
		//

		/// <summary>Message type.</summary>
		public int messageType;

		/// <summary>
		/// Construct header-extension object from a flatmap
		/// header-extension byte array.
		/// </summary>
		/// <param name="array">Flatmap header-extension byte array.</param>
		public HeaderExtension(byte[] array)
			: this(BitConverter.ToInt32(array, 0))
		{
		}

		/// <summary>
		/// Simple constructor with values for all header-extension fields.
		/// </summary>
		/// <param name="messageType">Message type.</param>
		public HeaderExtension(int messageType)
		{
			this.messageType  = messageType;
		}

		/// <summary>
		/// Return header extension as a byte array as expected by the
		/// FlatmapList class.
		/// </summary>
		/// <returns>Header extension as a byte array.</returns>
		public byte[] ToArray()
		{
			// (Byte array is sized to hold all fields that will be copied
			// into it.)
			byte[] headerExtension = new byte[MessageTypeByteLength];

			// Copy extension-header fields to byte array at specific offset.
			// Addition fields may be added in the future.
			Buffer.BlockCopy(BitConverter.GetBytes(messageType), 0,
				headerExtension, 0, headerExtension.Length);

			return headerExtension;
		}  
	}
}

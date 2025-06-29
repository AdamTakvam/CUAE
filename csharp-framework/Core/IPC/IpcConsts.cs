using System;

namespace Metreos.Core.IPC
{
	/// <summary>
	/// Common constants used in Metreos IPC implementations
	/// </summary>
	public abstract class IpcConsts
	{
        /// <summary>
        /// Set of valid values for Reserved (payload type) header field
        /// </summary>
        [Flags]
        public enum PayloadType : uint
        {
            Flatmap = 0x00,
            XML     = 0x40
        }

        /// <summary>
        /// Length field in packet header is a 32-bit integer. This field
        /// is necessary because TCP is a streaming protocol--packet
        /// boundaries are not significant.
        /// </summary>
        public const int LengthOfLength = 4;

        /// <summary>
        /// There is a 32-bit reserved field in the header after the length
        /// field that isn't used for anything.
        /// </summary>
        public const int LengthOfReserved = 4;

        /// <summary>
        /// Packet header length. Payload immediately follows header.
        /// </summary>
        public const int LengthOfHeader = LengthOfLength + LengthOfReserved;

        /// <summary>
        /// Upper limit to packet size (including header). Length field could express
        /// larger payload, but we have decided that anything larger is a
        /// mistake and must be the result of a bug somewhere.
        /// </summary>
        /// <remarks>Now that we have SFTP, this is probably too high</remarks>
        public const int MaxPacketLength = 1024 * 1024;

        /// <summary>The maximum body length we'll accept.</summary>
        public const int MaxBodyLength = MaxPacketLength - LengthOfHeader;
	}
}

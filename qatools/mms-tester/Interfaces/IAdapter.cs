using System;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Defines the adapter interface
	/// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// This is to be used before cleanup is called, to allow a graceful shutdown
        /// </summary>
        void SignalShutdown();

        /// <summary>
        /// Prepare the adapter for use
        /// </summary>
        /// <param name="mediaServerHandle">The handle used to connect to the media server</param>
        /// <returns>Status of the initialization</returns>
        bool Initialize(string mediaServerHandle);

        /// <summary>
        /// Release resources associated with the adapter
        /// </summary>
        /// <returns>Status of the resource releasing</returns> 
        void Cleanup();

        /// <summary>
        /// Restart adapter. Called after an initialize and shutdown cycle has occurred once
        /// </summary>
        /// <param name="mediaServerHandle">The handle used to connect to the media server</param>
        /// <returns>Status of the initialization</returns>
        bool Restart(string mediaServerHandle);

        /// <summary>
        /// Send on the transport layer defined by the adapter.  
        /// </summary>
        /// <param name="resultMessage">Verbose error status reporting</param>
        /// <returns>Status of the send</returns>
        bool Send(InternalMessage im, IAdapterTypes.ResponseFromMediaServerDelegate incomingResponseCallback, string messageType);

    }
}

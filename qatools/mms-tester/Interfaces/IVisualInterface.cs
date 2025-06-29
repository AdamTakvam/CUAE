using System;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Provides the common external commands needed to be exposed
	/// </summary>
    public interface IVisualInterface
    {
        /// <summary>
        /// This is to be used before cleanup is called, to allow a graceful shutdown
        /// </summary>
        void SignalShutdown();

        /// <summary>
        /// Prepare the visual interface for use
        /// </summary>
        /// <returns>Status of the initialization</returns>
        bool Initialize();

        /// <summary>
        /// Release resources associated with the visual interface
        /// </summary>
        /// <returns>Status of the resource releasing</returns> 
        void Cleanup();

        /// <summary>
        /// Restart visual interface. Called after an initialize and shutdown cycle has occurred once
        /// </summary>
        /// <returns>Status of the initialization</returns>
        bool Restart();

        /// <summary>
        /// Send on the transport layer defined by the visual interface.  
        /// </summary>
        /// <param name="resultMessage">Verbose error status reporting</param>
        /// <returns>Status of the send</returns>
        bool Send(InternalMessage im, IConduit.ConduitDelegate incomingResponseCallback);

        /// <summary>
        /// Registers a callback with the Conduit
        /// </summary>
        /// <param name="incomingEventDelegate">The callback that the Conduit will call</param>
        /// <returns>Status of the register event</returns>
        bool RegisterEvent(IConduit.ConduitDelegate incomingEventDelegate);

        /// <summary>
        /// This function should be what is called by the call back for the register event
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        bool HandleEvent(InternalMessage im);

    }
}

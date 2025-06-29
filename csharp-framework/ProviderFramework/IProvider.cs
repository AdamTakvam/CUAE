using System;

using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.ProviderFramework
{
    /// <summary>
    /// Interface to be implemented by all providers.
    /// </summary>
    public interface IProvider : IDisposable
    {
        /// <summary>Initialize a provider.</summary>
        /// <returns>True if successfull, false otherwise.</returns>
        bool InitializeProvider(Logger logger);

        /// <summary>
        /// Get the name of this provider.
        /// </summary>
        /// <returns>A string containing the provider's name.</returns>
        string GetName();

        /// <summary>
        /// Forces the provider to register its namespace
        /// Used when re-enabling providers.
        /// </summary>
        void RegisterNamespace();

        object GetMessageQueue();

        void SetRouterQueueWriter(object writer);

        IConfig.Status TaskStatus { get; }
    }
}

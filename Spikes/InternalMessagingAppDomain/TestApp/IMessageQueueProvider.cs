using System;

namespace TestApp
{
    /// <summary>
    /// Interface to be implemented by all message queue providers. This
    /// interface is used by message queue clients. A message queue provider
    /// implements these methods and thus provides message queueing
    /// services.
    /// </summary>
    public interface IMessageQueueProvider
    {
        /// <summary>
        /// Send a message to the message queue provider.
        /// </summary>
        /// <param name="message">The message to be sent. Cannot be null.</param>
        void Send(InternalMessage message);

        /// <summary>
        /// Received a message from the message queue provider.
        /// Block for the time specified by the timeout parameter.
        /// </summary>
        /// <returns>True if a message was received, false if no message was received.</returns>
        /// <param name="timeout">Amount of time to block while waiting for a message to arrive.</param>
        /// <param name="message">Message received from the queue. Null if no message was received.</param>
        bool Receive(System.TimeSpan timeout, out InternalMessage message);

        /// <summary>
        /// Retrieve the queue ID for this provider.
        /// </summary>
        /// <returns>A string containing the queue ID.</returns>
        string GetQueueId();

        /// <summary>
        /// Purge all messages currently in the queue.
        /// </summary>
        void Purge();

        /// <summary>
        /// Release any system-level resources that are currently retained
        /// by this queue.
        /// </summary>
        void ReleaseResources();

        /// <summary>
        /// Remove the queue.
        /// </summary>
        void Delete();

        /// <summary>
        /// Retrieve a MessageQueueWriter for this type of message queue
        /// provider.
        /// </summary>
        /// <returns>A MessageQueueWriter object.</returns>
        MessageQueueWriter GetMessageQueueWriter();
    }
}

using System;
using System.Diagnostics;
using System.Messaging;
using System.Reflection;

using Metreos.LoggingFramework;

namespace Metreos.Messaging
{
    /// <summary>
    /// Primary facet used to communicate between independent threads
    /// and processes. MessageQueue is a proxy for various providers
    /// that deliver the actual message queue functionality.
    /// </summary>
    /// 
    /// <remarks>
    /// It is necessary to pass in an appropriate provider to the
    /// MessageQueue constructor.
    /// </remarks>
    public class MessageQueue : MarshalByRefObject, IDisposable
    {
        private IMessageQueueProvider provider;
        private string queueId;
       
        public int Length { get { return provider.GetQueueLength(); } }

        public MessageQueue(string queueId, string providerAssemblyName, LogWriter log)
        {
            Debug.Assert(queueId != null, "The queue ID can not be null.");
            Debug.Assert(queueId != "", "The queue ID can not be empty.");

            this.queueId = queueId;

            Assembly a = Assembly.GetExecutingAssembly();

            this.provider = 
                (IMessageQueueProvider)a.CreateInstance(providerAssemblyName, true, BindingFlags.Default, null, new object[] { log }, System.Globalization.CultureInfo.CurrentCulture, null);

            a = null;
        }


        public string QueueId
        {
            get
            {
                return queueId;
            }
            set
            {
                queueId = value;
            }
        }


        public MessageQueueWriter GetWriter()
        {
            return this.provider.GetMessageQueueWriter();
        }


        public void Send(InternalMessage message)
        {
            Debug.Assert(message != null, "message is null");

            provider.Send(message);
        }


        public bool Receive(out InternalMessage message)
        {
            if(provider.Receive(System.TimeSpan.Zero, out message) == false)
            {
                return false;
            }

            return true;
        }


        public bool Receive(System.TimeSpan timeout, out InternalMessage message)
        {
            if(provider.Receive(timeout, out message) == false)
            {
                return false;
            }

            return true;
        }
        

        public void Purge()
        {
            provider.Purge();
        }


        public void ReleaseResources()
        {
            provider.ReleaseResources();
        }


        public void Dispose()
        {
            provider.Delete();
        }

        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }


        #endregion
    }
}

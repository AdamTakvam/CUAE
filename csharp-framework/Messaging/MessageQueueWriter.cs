using System;
using System.Diagnostics;

namespace Metreos.Messaging
{
    public sealed class MessageQueueWriter : MarshalByRefObject, IDisposable
    {
        private LoggingFramework.LogWriter log;
        private IMessageQueueProvider queue;

        internal MessageQueueWriter(IMessageQueueProvider provider, LoggingFramework.LogWriter log)
        {
            this.log = log;
            this.queue = provider;
        }

        public void PostMessage(InternalMessage im)
        {
            long time = Metreos.Utilities.HPTimer.Now();
            queue.Send(im);
            time = Metreos.Utilities.HPTimer.MillisSince(time);
            if(time > 32)
                log.Write(TraceLevel.Warning, "DIAG: PostMessage: time={0}, msg={1}, src={2}", time, im.MessageId, im.Source);
        }

        public void Dispose()
        {
            queue.ReleaseResources();
        }

        public MessageQueueWriter Clone()
        {
            return queue.GetMessageQueueWriter();
        }

        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion
    }
}

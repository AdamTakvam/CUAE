using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Configuration;

namespace TaskQueueTest 
{
    public class PrimaryTask : PrimaryTaskBase
    {
        private readonly MessageQueueWriter parentQ;

        public PrimaryTask(MessageQueueWriter parentQ)
            : base(IConfig.ComponentType.Core, "ChildTask", "", Config.Instance)
        {
            this.parentQ = parentQ;
        }

        protected override void RefreshConfiguration(string proxy)
        {}

        protected override void OnStartup()
        {}

        protected override void OnShutdown()
        {}

        protected override bool HandleMessage(InternalMessage message)
        {
            parentQ.PostMessage(message);
            return true;
        }

        public MessageQueueWriter GetQueueWriter()
        {
            return this.taskQueue.GetWriter();
        }
    }
}

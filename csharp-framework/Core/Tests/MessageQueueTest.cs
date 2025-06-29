using System;

using Metreos.Samoa;

namespace Metreos.Samoa.Core.Tests
{
    public class MockMessageQueueProvider : Core.IMessageQueueProvider
    {
        private System.Collections.Queue messages = new System.Collections.Queue();

        public void Send(Core.InternalMessage message)
        {
            messages.Enqueue(message);
        }

        public bool Receive(System.TimeSpan timeout, out Core.InternalMessage message)
        {
            message = (Core.InternalMessage)messages.Dequeue();

            // Mock provider will always return true.
            return true;
        }

        public string GetQueueId()
        { return "None"; }

        public void Purge()
        {}

        public void ReleaseResources()
        {}

        public void Delete()
        {}
    }

    /// <summary>
    /// Unit tests for Metreos.Samoa.Core.MessageQueue;
    /// </summary>
    public class MessageQueueTest
    {
        public MessageQueueTest()
        {
        }

        /// <summary>
        /// Test the send/receive proxy functionality using a mock
        /// message queue provider.
        /// </summary>
        public void testMessageQueueSendAndReceive()
        {
            Core.MessageQueue mq = new Core.MessageQueue(new MockMessageQueueProvider());

            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "Blah";
            im.Source = "Another.Blah";
            im.AddField(new Field("name", "value"));

            mq.Send(im);

            Core.InternalMessage im2;

            mq.Receive(out im2);

            csUnit.Assert.True(im.MessageId == im2.MessageId);
            csUnit.Assert.True(im.Source == im2.Source);
            csUnit.Assert.True(im.Fields[0].Name == im2.Fields[0].Name);
            csUnit.Assert.True(im.Fields[0].Value == im2.Fields[0].Value);

            mq.Cleanup();

            mq = null;
            im = null;
        }
    }
}

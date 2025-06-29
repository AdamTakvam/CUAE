using System;

using Metreos.Samoa.Core;

namespace Metreos.Samoa.Core.Tests
{
    public class MessageQueueWriterTest
    {
        MessageQueue q;

        public MessageQueueWriterTest()
        {
            q = new MessageQueue(new MsmqMessageQueueProvider("testGetWriterFromMessageQueue"));
        }

        public void testGetWriterFromMessageQueue()
        {
            q.Purge();

            MessageQueueWriter writer = q.GetWriter();

            InternalMessage im = new Core.InternalMessage();
            im.MessageId = "Test";
            im.Source = "Test";

            writer.PostMessage(im);

            im = null;

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals("Test", im.MessageId);

            writer.Cleanup();
        }

        public void testWriterCleanup()
        {
            q.Purge();

            MessageQueueWriter writer = q.GetWriter();

            InternalMessage im;

            for(int i = 0; i < 5; i++)
            {
                im = new Core.InternalMessage();
                im.MessageId = "testWriterCleanup";
                im.Source = "Test";

                writer.PostMessage(im);

                im = null;
            }

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals("testWriterCleanup", im.MessageId);

            writer.Cleanup();

            im = null;

            for(int i = 0; i < 4; i++)
            {
                csUnit.Assert.True(q.Receive(out im));
                csUnit.Assert.Equals("testWriterCleanup", im.MessageId);

                im = null;
            }

            im = null;
        }

        public void testCleanup()
        {
            q.Cleanup();
        }
    }
}

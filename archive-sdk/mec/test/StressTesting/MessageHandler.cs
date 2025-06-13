using System;
using System.Threading;
using System.Collections;

using System.Windows.Forms;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for MessageHandler.
	/// </summary>
	public class MessageHandler
	{
        private Queue queue;
        private Queue safeQueue;
        private Thread readingThread;

        private AutoResetEvent incomingMessage;
        
        private RichTextBox recepient;

        private bool allowWriting;
        private bool shutdownRequested;

        public MessageHandler(RichTextBox recepient, ref bool allowWriting)
		{
			queue = new Queue();
            safeQueue = Queue.Synchronized(queue);
		
            readingThread = new Thread(new ThreadStart( Run ));

            incomingMessage = new AutoResetEvent(false);

            this.recepient = recepient;
            this.allowWriting = allowWriting;
            this.shutdownRequested = false;

            readingThread.Start();
            
        }

        public void Run()
        {
            while(shutdownRequested == false)
            {
                incomingMessage.WaitOne();
            
                while(safeQueue.Count > 0 && allowWriting)
                {
                    string messageToWrite = (string) safeQueue.Dequeue();
                    recepient.AppendText(messageToWrite);
                }
            }
        }

        public void Shutdown()
        {
            shutdownRequested = true;
        }

        public void WriteMessage(string message)
        {
            safeQueue.Enqueue(message);
            incomingMessage.Set();
        }
	}
}

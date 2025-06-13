using System;
using System.Threading;

namespace MCPTest
{
    public class ThreadInfo : IDisposable
    {
        public Thread thread;

        public string currActionId;
        public readonly object responseLock;
        public bool actionSuccess;

        public string connId1;
        public string connId2;
        public string confId;

        public ThreadInfo()
        {
            this.responseLock = new object();
        }

        public void Reset()
        {
            currActionId = null;
            actionSuccess = false;
            connId1 = null;
            connId2 = null;
            confId = null;
        }

        public void Dispose()
        {
            if(thread != null)
                thread.Abort();
        }
    }
}

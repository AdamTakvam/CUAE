using System;
using System.Collections;

namespace Metreos.RecordAgent
{
    public delegate void OnStartRecordDelegate(uint callIdentifier);
    public delegate void OnStartRecordNowDelegate(uint callIdentifier);

    /// <summary>
    /// Summary description for NotifierManager.
    /// </summary>
    public class NotifierManager : IDisposable
    {
        static NotifierManager instance = null;
        static readonly object padlock = new object();
        private Notifier lastNotifier = null;

        public event OnStartRecordDelegate onStartRecord;
        public event OnStartRecordNowDelegate onStartRecordNow;

        public NotifierManager()
        {
        }

        public static NotifierManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NotifierManager();
                    }
                    return instance;
                }
            }
        }

        public void AddNotifier(uint callIdentifier, bool inbound, string name, string number, int x0, int y0)
        {
            int xo = 0;
            int yo = 0;
            if (lastNotifier != null && lastNotifier.IsDisposed == false)
            {
                // offset it
                xo = lastNotifier.Right;
                yo = lastNotifier.Top;
            }

            lastNotifier = new Notifier(this);
            lastNotifier.ShowOptions(false);
            lastNotifier.SetDimensions(x0, y0);

            lastNotifier.Notify(callIdentifier, inbound, name, number, xo, yo);	
        }

        public void Dispose()
        {
        }

        public void StartRecord(uint callIdentifier)
        {
            if (this.onStartRecord != null)
                this.onStartRecord(callIdentifier);
        }

        public void StartRecordNow(uint callIdentifier)
        {
            if (this.onStartRecordNow != null)
                this.onStartRecordNow(callIdentifier);
        }
    }
}

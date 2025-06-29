using System;
using System.Threading;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for SforceManager.
	/// </summary>
	public class SforceManager
	{
        #region static public variables
        public static SforceServiceWrapper binding = null;
        #endregion

        static SforceManager instance = null;
        static readonly object sforcelock = new object();
        private bool isEnabled = false;
        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }
 
        public SforceManager()
        {
            if (SforceManager.binding == null) 
                SforceManager.binding = new SforceServiceWrapper();
        }

        public static SforceManager Instance
        {
            get
            {
                lock (sforcelock)
                {
                    if (instance == null)
                    {
                        instance = new SforceManager();
                    }
                    return instance;
                }
            }
        }

        public void DoSearchByPhoneNumber(string pn)
        {
            CallerInfo callerInfo = new CallerInfo(SforceManager.binding);
            callerInfo.DoScreenPop(pn); 
        }
	}
}

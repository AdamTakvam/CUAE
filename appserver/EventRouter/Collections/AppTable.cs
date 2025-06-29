using System;
using System.Collections;

namespace Metreos.AppServer.EventRouter.Collections
{
	internal sealed class AppTable
	{
        // App name -> AppInfo
        private Hashtable appTable;

        public AppInfo this[string appName]
        {
            get { return appTable[appName] as AppInfo; }
            set { appTable[appName] = value; }
        }

        public int Count { get { return appTable.Count; } }
 
        public AppTable()
        {
            appTable = new Hashtable();
        }

        public void Add(string appName, AppInfo appInfo)
        {
            if(appTable != null)
            {
                if(!appTable.Contains(appName))
                {
                    appTable.Add(appName, appInfo);
                }
            }
        }

        public void Remove(string appName)
        {
            if(appTable != null)
            {
                appTable.Remove(appName);
            }
        }

        public void Clear()
        {
            IDictionaryEnumerator de = appTable.GetEnumerator();
            while(de.MoveNext())
            {
                AppInfo appInfo = de.Value as AppInfo;
                if(appInfo != null)
                {
                    try
                    {
                        appInfo.Clear();
                    }
                    catch {}
                }
            }

            appTable.Clear();
        }
	}
}

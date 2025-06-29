using System;
using System.Collections;

namespace Metreos.AppServer.ApplicationManager.Collections
{
	internal sealed class AppInfoCollection
	{
        // AppName (string) -> AppInfo (object)
        private Hashtable apps;

        public AppInfo this[string appName]
        {
            get { return apps[appName] as AppInfo; }
        }

        public int Count
        {
            get { return apps.Count; } 
        }

		public AppInfoCollection()
		{
            apps = new Hashtable();
		}

        public bool Add(AppInfo appInfo)
        {
            if(appInfo == null) { return false; }
            if(appInfo.Name == null) { return false; }

            if(apps.Contains(appInfo.Name) == false)
            {
                apps.Add(appInfo.Name, appInfo);
                return true;
            }
            
            return false;
        }

        public bool Contains(string appName)
        {
            return apps.Contains(appName);
        }

        public void Remove(string appName)
        {
            if(apps.Contains(appName))
            {
                apps.Remove(appName);
            }
        }

        public void Clear()
        {
            apps.Clear();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return apps.GetEnumerator();
        }
	}
}

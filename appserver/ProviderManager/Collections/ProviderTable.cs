using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Configuration;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.AppServer.ProviderManager.Collections
{
	internal sealed class ProviderTable : Loggable, IEnumerable
	{
        // Provider namespace -> Provider name
        private readonly Hashtable providerNamespaces;

        // Provider name -> ProviderInfo
        private readonly Hashtable providerTable;

        public int Count { get { return providerTable.Count; } }

        public object SyncRoot { get { return providerTable.SyncRoot; } }

        public ProviderInfo this[string providerName]
        {
            get { return providerTable[providerName] as ProviderInfo; }
            set { providerTable[providerName] = value; }
        }

        public ProviderTable()
            : base(Config.ProviderManager.LogLevel, IConfig.CoreComponentNames.PROV_MANAGER)
        {
            providerNamespaces = new Hashtable();
            providerTable = Hashtable.Synchronized(new Hashtable());
        }

        public ProviderInfo GetByNs(string Namespace)
        {
            string name = providerNamespaces[Namespace] as string;            
            if(name != null)
                return providerTable[name] as ProviderInfo;
            return null;
        }

        public StringCollection GetNamespaces(string providerName)
        {
            StringCollection ns = new StringCollection();
            foreach(DictionaryEntry de in providerNamespaces)
            {
                string currNs = de.Key as string;
                string currProvName = de.Value as string;

                if(currProvName == providerName)
                    ns.Add(currNs);
            }
            return ns;
        }

        public void Add(ProviderInfo providerInfo)
        {           
            if(providerInfo.Name == null)
                throw new ArgumentException("Provider has no name", "providerInfo");

            providerTable[providerInfo.Name] = providerInfo;
        }

        public void RegisterNamespace(string providerNamespace, string providerName)
        {
            providerNamespaces[providerNamespace] = providerName;
        }

        public void Remove(string providerName)
        {
            providerTable.Remove(providerName);

            RemoveNamespaces(providerName);
        }

        public void RemoveNamespaces(string providerName)
        {
            StringCollection defunctNamespaces = new StringCollection();
            foreach(DictionaryEntry de in providerNamespaces)
            {
                string provName = de.Value as string;
                if(provName == providerName)
                {
                    defunctNamespaces.Add(de.Key as string);
                }
            }

            foreach(string provNs in defunctNamespaces)
            {
                providerNamespaces.Remove(provNs);
            }
        }

        public void Clear()
        {
            providerTable.Clear();
            providerNamespaces.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return providerTable.Values.GetEnumerator();
        }

        #endregion
    }
}

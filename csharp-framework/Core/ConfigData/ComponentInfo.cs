using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class ComponentInfo
    {
        public uint ID = 0;
        public string name;
        public string displayName;
        public IConfig.ComponentType type;
        public ComponentGroup[] groups;
        public IConfig.Status status;
        public string author;
        public string copyright;
        public DateTime created;
        public string authorUrl;
        public string supportUrl;
        public string description;
        public string version;
        public bool replicatedDb = false;

        public double Version  
        {
            get
            { 
                try { return double.Parse(version); }
                catch(Exception) { return 0; }
            }
            set { version = value.ToString(); }
        }
    }
}

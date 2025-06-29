using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class ComponentGroup
    {
        public uint ID = 0;
        public uint failoverGroupID = 0;
        public uint alarmGroupID = 0;
        public string name;
        public string description;
        public IConfig.ComponentType componentType = IConfig.ComponentType.Unspecified;
    }
}

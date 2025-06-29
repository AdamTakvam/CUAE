using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    public class SnmpOid
    {
        public string Name { get { return name; } }
        private readonly string name;

        public string Description { get { return description; } }
        private readonly string description;

        public IConfig.SnmpOidType Type { get { return type; } }
        private readonly IConfig.SnmpOidType type;

        public IConfig.SnmpSyntax DataType { get { return dataType; } }
        private readonly IConfig.SnmpSyntax dataType;

        public bool Ignore { get { return ignore; } }
        private readonly bool ignore;

        public SnmpOid(string name, string description, IConfig.SnmpOidType type,
            IConfig.SnmpSyntax dataType, bool ignore)
        {
            this.name = name;
            this.description = description;
            this.type = type;
            this.dataType = dataType;
            this.ignore = ignore;
        }
    }
}

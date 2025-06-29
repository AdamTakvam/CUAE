using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class ExtensionParam
    {
        public uint ID = 0;
        public string name;
        public FormatType type;
        public string description;
        public string Value;

        public ExtensionParam() {}

        public ExtensionParam(string name, string description, IConfig.StandardFormat formatName)
            : this(name, description, new FormatType(formatName)) {}

        public ExtensionParam(string name, string description, FormatType type)
        {
            this.name = name;
            this.type = type;
            this.description = description;
        }
    }
}

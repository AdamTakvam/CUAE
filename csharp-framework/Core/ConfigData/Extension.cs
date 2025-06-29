using System;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class Extension
    {
        public uint ID = 0;
        public string name;
        public string description;
        public bool synchronous = false;

        // Array of ExtensionParam objects
        public ExtensionParam[] parameters;

        // Constructors
        public Extension()
            : this(null, null) {}

        public Extension(string name) 
            : this(name, null) {}

        public Extension(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}

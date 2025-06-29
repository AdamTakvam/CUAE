using System;
using System.Collections;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore
{
    internal sealed class IconInfo
    {
        public string data;
        public iconTypeType type;
    }

    internal sealed class ActionInfo
    {
        // List of resultDataType objects
        public ArrayList resultData;

        // List of actionParamType objects
        public ArrayList Params;

        // List of IconInfo objects
        public ArrayList icons;

        public string name;
        public string[] asyncCallback;
        public bool allowCustomParams;
        public string displayName;
        public string description;
        public actionTypeType type;
        public returnValueType returnValue;
    }

    internal sealed class EventInfo
    {
        // List of eventParam objects
        public ArrayList Params;

        // List of IconInfo objects
        public ArrayList icons;

        public string name;
        public string expects;
        public string displayName;
        public string description;
        public eventTypeType type;
    }

    internal sealed class TypeInfo
    {
        public string name;
        public string displayName;
        public bool serializable = false;
        public string description;
        public Hashtable inputTypes;    // type (string) -> description (string)
        public ArrayList methods;       // TypeMethod list
        public ArrayList properties;    // TypeMethod list (parameters unused)
        public ArrayList indexers;      // TypeIndexer list
    }

    internal sealed class TypeMethod
    {
        public string name;
        public string returnType;
        public string description;
        public ArrayList parameters;    // ParameterInfo list
    }

    internal sealed class TypeIndexer
    {
        public string returnType;
        public string indexType;
        public string description;
    }
}

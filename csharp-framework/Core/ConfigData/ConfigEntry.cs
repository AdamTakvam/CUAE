using System;
using System.Net;
using System.Diagnostics;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class ConfigEntry
    {
        public abstract class Defaults
        {
            public const bool Required  = false;
            public const bool ReadOnly  = false;
        }

        public uint ID = 0;
        public uint metaID = 0;
        public string name;
        public object Value;
        public string displayName;
        public string description;
        public int minValue = 0;
        public int maxValue = 0;
        public bool required = Defaults.Required;
        public bool readOnly = Defaults.ReadOnly;
        public IConfig.ComponentType componentType;
        public FormatType formatType;
            
        public ConfigEntry() {}

        /// <summary>Deprecated</summary>
        public ConfigEntry(string name, object Value, string description, IConfig.StandardFormat formatName, bool required)
            : this(name, name, Value, description, new FormatType(formatName), required, 0, 0) {}

        /// <summary>Deprecated</summary>
        public ConfigEntry(string name, object Value, string description, FormatType formatType, bool required)
            : this(name, name, Value, description, formatType, required, 0, 0) {}

        /// <summary>Deprecated</summary>
        public ConfigEntry(string name, object Value, string description, int minValue, int maxValue)
            : this(name, name, Value, description, new FormatType(IConfig.StandardFormat.Number), Defaults.Required, minValue, maxValue) {}

        /// <summary>Declares a config entry with a pre-defined value type</summary>
        public ConfigEntry(string name, string displayName, object Value, string description, IConfig.StandardFormat formatName, bool required)
            : this(name, displayName, Value, description, new FormatType(formatName), required, 0, 0) {}

        /// <summary>Declares a config entry with a custom-defined enumerable value type</summary>
        public ConfigEntry(string name, string displayName, object Value, string description, FormatType formatType, bool required)
            : this(name, displayName, Value, description, formatType, required, 0, 0) {}

        /// <summary>Declares a numeric config value</summary>
        public ConfigEntry(string name, string displayName, object Value, string description, int minValue, int maxValue, bool required)
            : this(name, displayName, Value, description, new FormatType(IConfig.StandardFormat.Number), required, minValue, maxValue) {}

        protected ConfigEntry(string name, string displayName, object Value, string description, FormatType formatType, bool required, int minValue, int maxValue)
        {
            this.name = name;
            this.displayName = displayName;
            this.description = description;
            this.formatType = formatType;
            this.required = required;
            this.minValue = minValue;
            this.maxValue = maxValue;

            if(ParseValue(Value) == false)
            {
                throw new ArgumentException("Value '" + Value + "' is not convertible to the specified format: " + formatType.name);
            }
        }
	
        public bool ParseValue(object rawValue)
        {
            if(formatType == null) { return false; }
            
            if(rawValue == null) 
            {
                Value = null;
                return true; 
            }

            IConfig.StandardFormat format;
            try
            {
                format = (IConfig.StandardFormat)Enum.Parse(typeof(IConfig.StandardFormat), formatType.name, true);
            }
            catch
            {
                // Custom (enum) value type. Treat as string.
                Value = rawValue.ToString();
                return true;
            }

            try
            {
                switch(format)
                {
                    case IConfig.StandardFormat.Array:
                    case IConfig.StandardFormat.HashTable:
                    case IConfig.StandardFormat.DataTable:
                        Value = rawValue;
                        return true;
                    case IConfig.StandardFormat.Bool:
                        Value = bool.Parse(rawValue.ToString());
                        return true;
                    case IConfig.StandardFormat.DateTime:
                        Value = DateTime.Parse(rawValue.ToString());
                        return true;
                    case IConfig.StandardFormat.IP_Address:
                        Value = ResolveHostname(rawValue.ToString());
                        return true;
                    case IConfig.StandardFormat.Number:
                        try { Value = int.Parse(rawValue.ToString()); }
                        catch { Value = double.Parse(rawValue.ToString()); }
                        return true;
                    case IConfig.StandardFormat.TraceLevel:
                        Value = (TraceLevel) Enum.Parse(typeof(TraceLevel), rawValue.ToString(), true);
                        return true;
                    case IConfig.StandardFormat.Password:
                        // TODO:  de-crypt rawValue
                        Value = rawValue.ToString();
                        return true;
                    default:
                        Value = rawValue.ToString();
                        return true;
                }
            }
            catch {}
            return false;
        }

        /// <summary>Resolves hostname or IP address string to IPAddress object</summary>
        /// <remarks>Duplicate of Metreos.Configuration.Config.ResolveHostname(). Deprecate me!</remarks>
        /// <param name="hostname">Hostname or IP address string</param>
        /// <returns>IPAddress</returns>
        private static IPAddress ResolveHostname(string hostname)
        {
            if(hostname == null)
                return null;

            try
            {
                IPAddress[] iph = System.Net.Dns.GetHostAddresses(hostname);
                if(iph == null || iph.Length == 0)
                    return null;

                return iph[0];
            }
            catch
            {
                return null;
            }
        }
    }
}

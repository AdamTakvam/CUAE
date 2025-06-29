using System;
using System.Collections.Specialized;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
    [Serializable]
    public class FormatType
    {
        public uint ID = 0;
        public string name;
        public string description;
        public StringCollection enumValues;

        public bool Custom { get { return enumValues != null; } }

        public FormatType() {}

        public FormatType(IConfig.StandardFormat name)
        {
            InitStandardFormat(name);
        }

        public FormatType(string name, string description, StringCollection enumValues)
        {
            if(enumValues == null)
            {
                throw new ArgumentException("Custom formats must be enumerable", "enumValues");
            }

            string[] standardNames = Enum.GetNames(typeof(IConfig.StandardFormat));                
            if(Array.IndexOf(standardNames, name) != -1)
            {
                throw new ArgumentException("You cannot create a custom format with the same name as a standard format: " + name, "name");
            }

            this.name = name;
            this.description = description;
            this.enumValues = enumValues;
        }

        public void InitStandardFormat(IConfig.StandardFormat formatName)
        {
            this.name = formatName.ToString();
            this.ID = (uint)formatName;
        }

        public static IConfig.StandardFormat ParseToStandardFormat(string formatName)
        {
            IConfig.StandardFormat sFormat = 0;
            try
            {
                sFormat = (IConfig.StandardFormat) Enum.Parse(typeof(IConfig.StandardFormat), formatName, true);
            }
            catch 
            {
                return 0;
            }

            return sFormat;
        }
    }
}

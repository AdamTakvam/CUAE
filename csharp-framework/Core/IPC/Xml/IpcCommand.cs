using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace Metreos.Core.IPC.Xml 
{   
    /// <summary>
    /// This class is the raw deserialized XML representation of an OAM command.
    /// For parameter details, see: Metreos.Interfaces.IManagement
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("command", IsNullable=false)]
    public class commandType 
    {
        public string this[string paramName]
        {
            get
            {
                foreach(paramType pType in param)
                {
                    if(String.Compare(pType.name, paramName, true) == 0)
                    {
                        return pType.Value;
                    }
                }
                return null;
            }
        }

        public byte[] ByteData
        {
            get
            {
                if((data == null) || (data.Data == null))
                    return null;

                return Convert.FromBase64String(data.Data);
            }
            set
            {
                if(value != null)
                {
                    string base64Str = Convert.ToBase64String(value);
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    data = doc.CreateCDataSection(base64Str);
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("param")]
        public paramType[] param;
        
        public System.Xml.XmlCDataSection data;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;

        public commandType() {}

        public commandType(string name, StringDictionary parameters)
        {
            this.name = name;
            
            if(parameters != null)
            {
                int i = 0;
                param = new paramType[parameters.Count];

                foreach(DictionaryEntry de in parameters)
                {
                    param[i] = new paramType(de.Key as string, de.Value as string);
                    i++;
                }
            }
        }

        public StringDictionary GetParameters()
        {
            if(param == null) { return null; }

            StringDictionary dict = new StringDictionary();
            for(int i=0; i<param.Length; i++)
            {
                dict[param[i].name] = param[i].Value;
            }

            return dict;
        }
    }
    
    public class paramType 
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;

        public paramType() {}

        public paramType(string name, string Value)
        {
            this.name = name;
            this.Value = Value;
        }
    }
}

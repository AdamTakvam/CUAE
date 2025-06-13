using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.HttpTypes.Types.FormCollection;

namespace Metreos.Types.Http
{
	/// <summary> Creates a collection to use when determining what parameters were passed in </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public class FormCollection : IVariable, IEnumerable    
    {
        public object Value;
        private string rawBody;

        private static XmlSerializer serializer = new XmlSerializer(typeof(NameValueCollection));

        /// <summary> Accessor to get query parameter values.  Returns null on if a not a parameter in this collection </summary>
        [TypeMethod(Package.Indexers.String.DESCRIPTION)]
        public string this[string name]
        {
            get
            {
                NameValueCollection values = Value as NameValueCollection;
                
                for(int i = 0; i < values.Count; i++)
                {
                    if(0 == String.Compare(values[i].Name, name, true))
                    {
                        return values[i].Value;
                    }
                }

                return null;
            }
        }

        /// <summary> Accesses the requested index of the inner NameValueCollection </summary>
        [TypeMethod(Package.Indexers.Int32.DESCRIPTION)]
        public string this[int index]
        {
            get
            {
                NameValueCollection values = Value as NameValueCollection;

                return values[index].Value;
            }
        }

        /// <summary> Returns the number of query parameters contained in the inner NameValueCollection </summary>
        [TypeMethod(Package.CustomProperties.Count.DESCRIPTION)]
        public int Count
        {
            get
            {
                NameValueCollection values = Value as NameValueCollection;

                return values.Count;
            }
        }

        [TypeMethod(Package.CustomMethods.GetNameAt_Int32.DESCRIPTION)]        
        public string GetNameAt(int index)
        {
            NameValueCollection values = Value as NameValueCollection;
            
            return values[index].Name;
        }

        [TypeMethod(Package.CustomMethods.GetValues_String.DESCRIPTION)]
        public string[] GetValues(string name)
        {
            NameValueCollection values = Value as NameValueCollection;
                
            for(int i = 0; i < values.Count; i++)
            {
                if(0 == String.Compare(values[i].Name, name, true))
                {
                    return values[i].GetValues();
                }
            }

            return null;
        }

        [TypeMethod(Package.CustomMethods.GetValues_Int32.DESCRIPTION)]
        public string[] GetValues(int i)
        {
            NameValueCollection values = Value as NameValueCollection;
                
            return values[i].GetValues();
        }

        public FormCollection()
        {
            Reset();
        }

        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string newValue)
        {
            rawBody = newValue;
            if(newValue == null || newValue == String.Empty) return true;
            
            string decodedForm = System.Web.HttpUtility.UrlDecode(newValue);

            string[] keyValuePairs = decodedForm.Split(IHttp.PARAM_DELIMITER);

            if(keyValuePairs == null)
            {
                return true;
            }

            foreach(string pair in keyValuePairs)
            {
                // Can't find the equal sign, or its on the key boundary, 
                // making key empty. Value can be empty string though.
                int index = pair.IndexOf(IHttp.PARAM_EQUATES);

                if(index == -1 || index == 0)
                {
                    return true;
                }
                else
                {
                    string key = pair.Substring(0, index);
                
                    string paramValue;
                    if(index < pair.Length - 1)
                    {
                        paramValue = pair.Substring(index + 1);
                    }
                    else
                    {
                        paramValue = String.Empty;
                    }

                    NameValueCollection values = Value as NameValueCollection;

                    if(key != null) key = System.Web.HttpUtility.UrlDecode(key);
                    if(paramValue != null) paramValue = System.Web.HttpUtility.UrlDecode(paramValue);

                    if(! values.Contains(key))
                    {
                        values.Add(new NameValuePair(key, paramValue));
                    }
                    else
                    {
                        NameValuePair nameValueRef = values[key];
                        nameValueRef.AddValue(paramValue);
                    }
                }
            }

            return true;
        }


        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            return rawBody;
        }

        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            rawBody = null;
            if(Value == null)
            {
                Value = new NameValueCollection();
            }
            else
            {
                NameValueCollection values = Value as NameValueCollection;
                values.Clear();
            }
        }
        #region IEnumerable Members

        [TypeMethod(Package.CustomMethods.GetEnumerator.DESCRIPTION)]        
        public IEnumerator GetEnumerator()
        {
            if(Value != null)
            {
                return (Value as IEnumerable).GetEnumerator();
            }
            return null;
        }

        #endregion
    }
}

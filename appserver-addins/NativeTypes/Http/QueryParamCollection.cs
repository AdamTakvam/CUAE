using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.HttpTypes.Types.QueryParamCollection;

namespace Metreos.Types.Http
{
	/// <summary> Creates a collection to use when determining what parameters were passed in </summary>
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public class QueryParamCollection : IVariable
    {
        public object Value;

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

        /// <summary> Retrieves the name of the query parameter at the given index </summary>
        [TypeMethod(Package.CustomMethods.GetNameAt_Int32.DESCRIPTION)]
        public string GetNameAt(int index)
        {
            NameValueCollection values = Value as NameValueCollection;
            
            return values[index].Name;
        }

        public QueryParamCollection()
        {
            Reset();
        }

        /// <summary> 
        ///           The string can either be a query or Xml 
        ///           representing that query in the form of a NameValueCollection
        /// </summary>
        //[TypeInput("string", "A xml-serialized NameValueCollection")]
        [TypeInput("string", Package.CustomMethods.Parse_String.DESCRIPTION)]
        public bool Parse(string newValue)
        {
             return PopulateWithQueryString(newValue);
        }

        /// <summary> As we are given a serialized NameValueCollection, go ahead and overwrite current "Value" </summary>
        private bool PopulateWithXml(XmlTextReader reader)
        {
            try   
            {
                Value = serializer.Deserialize(reader) as NameValueCollection;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary> 
        ///           An encoded query string is expected to come in here, which is 
        ///           then parsed and the params are then placed into the inner hashtable
        /// </summary>
        private bool PopulateWithQueryString(string query)
        {

            if(query == null || query == String.Empty) return true;

            bool startsWithDelimiter = query.IndexOf(IHttp.PARAM_START) == 0;

            if(! startsWithDelimiter)
            {
                query = '?' + query;
            }

            // Creating this URI is to take advantadge of the encoding/decoding abilities of the 
            // System.Uri class
            UriBuilder convertedToUri 
                = new UriBuilder(IHttp.HTTP_PROTOCOL, IHttp.DUMMY_HOST, IHttp.DUMMY_PORT, IHttp.DUMMY_PAGE, query);

            // Here we get the decoded query string
            string decodedParam = convertedToUri.Query;

            // Rip off beginning '?'
            decodedParam = decodedParam.Substring(1);
            

            string[] keyValuePairs = decodedParam.Split(IHttp.PARAM_DELIMITER);

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
                        // Invalid query string. Can not have duplicate params in query string.
                        // TODO: be nice to be able to log this...
                        continue;
                    }
                }
            }

            return true;
        }

        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, Value);
            writer.Close();

            return sb.ToString();
        }

        /// <summary> Clears the query parameters </summary>
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
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
	}
}

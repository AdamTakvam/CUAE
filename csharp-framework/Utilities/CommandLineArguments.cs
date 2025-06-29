using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Metreos.Utilities
{

    /// <summary>
    /// This class parses your command line arguments, 
    /// find all parameters starting with -, -- or / and all 
    /// the values linked. I assumed that a value could be 
    /// separated from a parameter with a space, a : or a =. The 
    /// parser also look for enclosing characters like ' or " 
    /// and remove them. Of course if you have a value like 
    /// 'Mike's house', only the first and last ' will be removed.
    /// </summary>
    /// <remarks>
    /// Currently unsupported:
    ///    - Using a standalone parameter with a ':' in any part
    ///      of its value will yield bogus results.
    /// </remarks>
    public sealed class CommandLineArguments
    {
        private Hashtable parameters;
        private StringCollection standaloneParameters;

        /// <summary>Creates a CommandLineArguments object by parsing the input string</summary>
        /// <param name="args">The command line args[] just as passed into your main() method</param>
        public CommandLineArguments(string[] args)
        {
            parameters = new Hashtable();
            standaloneParameters = new StringCollection();
            
            LoadArgumentsFromStringArray(args);
        }

        /// <summary>Adds more command line parameters to this instance</summary>
        /// <param name="args">Additional parameters (not passed into constructor)</param>
        public void LoadArgumentsFromStringArray(string[] args)
        {
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase|RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase|RegexOptions.Compiled);

            string parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach(string arg in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                if(arg.StartsWith("-") || arg.StartsWith("/"))
                {
                    Parts = Spliter.Split(arg,3);

                    switch(Parts.Length)
                    {
                        case 1:
                            // Found standalone parameters.
                            standaloneParameters.Add(Parts[0]);
                            break;

                        case 2:
                            // Found just a parameter

                            // The last parameter is still waiting. 
                            // With no value, set it to true.
                            if(parameter != null)
                            {
                                AddParameter(parameter, "true");
                            }
                            parameter = Parts[1];
                            break;

                        case 3:
                            // parameter with enclosed value

                            // The last parameter is still waiting. 
                            // With no value, set it to true.
                            if(parameter != null)
                            {
                                AddParameter(parameter, "true");
                            }

                            parameter = Parts[1];

                            // Remove possible enclosing characters (",')
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            AddParameter(parameter, Parts[2]);

                            parameter = null;
                            break;
                    }
                }
                else
                {
                    standaloneParameters.Add(arg);
                }
            }
            // In case a parameter is still waiting
            if(parameter != null)
            {
                if(parameters.ContainsKey(parameter) == false)
                {
                    AddParameter(parameter, "true");
                }
            }
        }

    
        private void AddParameter(string key, string Value)
        {
            StringCollection paramList;

            key = key.ToLower();

            if(parameters.ContainsKey(key) == false)
            {
                paramList = new StringCollection();
                parameters.Add(key, paramList);
            }
            else
            {
                paramList = (StringCollection)parameters[key];
            }

            paramList.Add(Value);
        }


        /// <summary>Retrieve a parameter value if it exists</summary>
        public string GetSingleParam(string param)
        {
            StringCollection values = (StringCollection)parameters[param];
            if(values != null)
            {
                return values[0];
            }
            else
            {
                return "";
            }
        }

        /// <summary>Determines whether the specified parameter was specified</summary>
        /// <param name="param">Parameter name</param>
        /// <returns>Whether or not it has a value</returns>
        public bool IsParamPresent(string param)
        {
            StringCollection values = (StringCollection)parameters[param];
            return values != null ? true : false;
        }


        /// <summary>Returns values not specifically associated with a parameter name</summary>
        /// <returns>Collection of stand-alone values</returns>
        public StringCollection GetStandAloneParameters()
        {
            return standaloneParameters;
        }


        /// <summary>Retrieve a parameter value if it exists</summary>
        public string[] this [string param]
        {
            get 
            { 
                StringCollection values = (StringCollection)parameters[param];
                string[] retValues = null;

                if(values != null)
                {
                    retValues = new string[values.Count];
                    values.CopyTo(retValues, 0);
                }

                return retValues;
            }
        }
    }
}

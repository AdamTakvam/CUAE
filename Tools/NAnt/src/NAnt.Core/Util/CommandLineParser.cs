// NAnt - A .NET build tool
// Copyright (C) 2001 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Gert Driesen (gert.driesen@ardatis.com)

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Text;

namespace NAnt.Core.Util {
    /// <summary>
    /// Commandline parser.
    /// </summary>
    public class CommandLineParser {
        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser" /> class
        /// using possible arguments deducted from the specific <see cref="Type" />.
        /// </summary>
        /// <param name="argumentSpecification">The <see cref="Type" /> from which the possible command-line arguments should be retrieved.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentSpecification" /> is a null reference.</exception>
        public CommandLineParser(Type argumentSpecification) {
            if (argumentSpecification == null) {
                throw new ArgumentNullException("argumentSpecification");
            }

            _argumentCollection = new CommandLineArgumentCollection();

            foreach (PropertyInfo propertyInfo in argumentSpecification.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                if (propertyInfo.CanWrite || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)) {
                    CommandLineArgumentAttribute attribute = GetCommandLineAttribute(propertyInfo);
                    if (attribute is DefaultCommandLineArgumentAttribute) {
                        Debug.Assert(_defaultArgument == null);
                        _defaultArgument = new CommandLineArgument(attribute, propertyInfo);
                    } else if (attribute != null) {
                        _argumentCollection.Add(new CommandLineArgument(attribute, propertyInfo));
                    }
                }
            }

            _argumentSpecification = argumentSpecification;
        }
        
        #endregion Public Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Gets a logo banner using version and copyright attributes defined on the 
        /// <see cref="Assembly.GetEntryAssembly()" /> or the 
        /// <see cref="Assembly.GetCallingAssembly()" />.
        /// </summary>
        /// <value>
        /// A logo banner.
        /// </value>
        public virtual string LogoBanner {
            get {
                string productName;
                string informationalVersion;
                Version assemblyVersion;
                string configurationInformation = null;
                string copyrightInformation = null;
                string companyInformation = null;
                DateTime releaseDate;

                Assembly assembly = Assembly.GetEntryAssembly();
                if (assembly == null) {
                    assembly = Assembly.GetCallingAssembly();
                }

                // get product name
                object[] productAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (productAttributes.Length > 0) {
                    AssemblyProductAttribute productAttribute = (AssemblyProductAttribute) productAttributes[0];
                    productName = productAttribute.Product;
                } else {
                    productName = assembly.GetName().Name;
                }

                // get informational version
                object[] informationalVersionAttributes = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                if (informationalVersionAttributes.Length > 0) {
                    AssemblyInformationalVersionAttribute informationalVersionAttribute = (AssemblyInformationalVersionAttribute) informationalVersionAttributes[0];
                    informationalVersion = informationalVersionAttribute.InformationalVersion;
                } else {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
                    informationalVersion = info.FileMajorPart + "." + info.FileMinorPart + "." + info.FileBuildPart + "." + info.FilePrivatePart;
                }

                // get assembly version 
                assemblyVersion = assembly.GetName().Version;

                // determine release date using build number of assembly 
                // version (specified as number of days passed since 1/1/2000)
                releaseDate = new DateTime(2000, 1, 1).AddDays(assemblyVersion.Build);

                // get configuration information
                object[] configurationAttributes = assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);
                if (configurationAttributes.Length > 0) {
                    AssemblyConfigurationAttribute configurationAttribute = (AssemblyConfigurationAttribute) configurationAttributes[0];
                    configurationInformation = configurationAttribute.Configuration;
                }
                
                // get copyright information
                object[] copyrightAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (copyrightAttributes.Length > 0) {
                    AssemblyCopyrightAttribute copyrightAttribute = (AssemblyCopyrightAttribute) copyrightAttributes[0];
                    copyrightInformation = copyrightAttribute.Copyright;
                }

                // get company information
                object[] companyAttributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (companyAttributes.Length > 0) {
                    AssemblyCompanyAttribute companyAttribute = (AssemblyCompanyAttribute) companyAttributes[0];
                    companyInformation = companyAttribute.Company;
                }

                StringBuilder logoBanner = new StringBuilder();

                logoBanner.AppendFormat(CultureInfo.InvariantCulture,
                    "{0} {1} (Build {2}; {3}; {4})", productName, 
                    informationalVersion, assemblyVersion.ToString(4),
                    configurationInformation, releaseDate.ToShortDateString()); 
                logoBanner.Append(Environment.NewLine);

                // output copyright information
                if (!StringUtils.IsNullOrEmpty(copyrightInformation)) {
                    logoBanner.Append(copyrightInformation);
                    logoBanner.Append(Environment.NewLine);
                }

                // output company information
                if (!StringUtils.IsNullOrEmpty(companyInformation)) {
                    logoBanner.Append(companyInformation);
                    logoBanner.Append(Environment.NewLine);
                }

                return logoBanner.ToString();
            }
        }

        /// <summary>
        /// Gets the usage instructions.
        /// </summary>
        /// <value>The usage instructions.</value>
        public virtual string Usage {
            get {
                StringBuilder helpText = new StringBuilder();
                Assembly assembly = Assembly.GetEntryAssembly();
                if (assembly == null) {
                    assembly = Assembly.GetCallingAssembly();
                }

                // Add usage instructions to helptext

                if (helpText.Length > 0) {
                    helpText.Append(Environment.NewLine);
                }

                helpText.Append("Usage : " + assembly.GetName().Name + " [options]");

                if (_defaultArgument != null) {
                    helpText.Append(" <" + _defaultArgument.LongName + ">");
                    if (_defaultArgument.AllowMultiple) {
                        helpText.Append(" <" + _defaultArgument.LongName + ">");
                        helpText.Append(" ...");
                    }
                }

                helpText.Append(Environment.NewLine);

                // Add options to helptext

                helpText.Append("Options : ");
                helpText.Append(Environment.NewLine);
                helpText.Append(Environment.NewLine);

                foreach (CommandLineArgument argument in _argumentCollection) {
                    string valType = "";

                    if (argument.ValueType == typeof(string)) {
                        valType = ":<text>";
                    } else if (argument.ValueType == typeof(bool)) {
                        valType = "[+|-]";
                    } else if (argument.ValueType == typeof(FileInfo)) {
                        valType = ":<filename>";
                    } else if (argument.ValueType == typeof(int)) {
                        valType = ":<number>";
                    } else {
                        valType = ":" + argument.ValueType.FullName;
                    }

                    string optionName = argument.LongName;

                    if (argument.ShortName != null) {
                        if (argument.LongName.StartsWith(argument.ShortName)) {
                            optionName = optionName.Insert(argument.ShortName.Length, "[") + "]";
                        }
                        helpText.AppendFormat(CultureInfo.InvariantCulture, "  -{0,-30}{1}", optionName + valType, argument.Description);

                        if (!optionName.StartsWith(argument.ShortName)) {
                            helpText.AppendFormat(CultureInfo.InvariantCulture, " (Short format: /{0})", argument.ShortName);
                        }
                    } else {
                        helpText.AppendFormat(CultureInfo.InvariantCulture, "  -{0,-30}{1}", optionName + valType, argument.Description);
                    }
                    helpText.Append(Environment.NewLine);
                }

                return helpText.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether no arguments were specified on the
        /// command line.
        /// </summary>
        public bool NoArgs {
            get {
                foreach(CommandLineArgument argument in _argumentCollection) {
                    if (argument.SeenValue) {
                        return true;
                    }
                }

                if (_defaultArgument != null) {
                    return _defaultArgument.SeenValue;
                }

                return false;
            }
        }

        #endregion Public Instance Properties

        #region Public Instance Methods

        /// <summary>
        /// Parses an argument list.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        /// <param name="destination">The destination object on which properties will be set corresponding to the specified arguments.</param>
        /// <exception cref="ArgumentNullException"><paramref name="destination" /> is a null reference.</exception>
        /// <exception cref="ArgumentException">The <see cref="Type" /> of <paramref name="destination" /> does not match the argument specification that was used to initialize the parser.</exception>
        public void Parse(string[] args, object destination) {
            if (destination == null) {
                throw new ArgumentNullException("destination");
            }

            if (!_argumentSpecification.IsAssignableFrom(destination.GetType())) {
                throw new ArgumentException("Type of destination does not match type of argument specification.");
            }

            ParseArgumentList(args);

            // check for missing required arguments
            foreach (CommandLineArgument arg in _argumentCollection) {
                arg.Finish(destination);
            }

            if (_defaultArgument != null) {
                _defaultArgument.Finish(destination);
            }
        }

        #endregion Public Instance Methods

        #region Private Instance Methods

        private void ParseArgumentList(string[] args) {
            if (args != null) {
                foreach (string argument in args) {
                    if (argument.Length > 0) {
                        switch (argument[0]) {
                            case '-':
                            case '/':
                                int endIndex = argument.IndexOfAny(new char[] {':', '+', '-'}, 1);
                                string option = argument.Substring(1, endIndex == -1 ? argument.Length - 1 : endIndex - 1);
                                string optionArgument;

                                if (option.Length + 1 == argument.Length) {
                                    optionArgument = null;
                                } else if (argument.Length > 1 + option.Length && argument[1 + option.Length] == ':') {
                                    optionArgument = argument.Substring(option.Length + 2);
                                } else {
                                    optionArgument = argument.Substring(option.Length + 1);
                                }
                                
                                CommandLineArgument arg = _argumentCollection[option];
                                if (arg == null) {
                                    throw new CommandLineArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown argument '{0}'", argument));
                                } else {
                                    if (arg.IsExclusive && args.Length > 1) {
                                        throw new CommandLineArgumentException(string.Format(CultureInfo.InvariantCulture, "Commandline argument '-{0}' cannot be combined with other arguments.", arg.LongName));
                                    } else {
                                        arg.SetValue(optionArgument);
                                    }
                                }
                                break;
                            default:
                                if (_defaultArgument != null) {
                                    _defaultArgument.SetValue(argument);
                                } else {
                                    throw new CommandLineArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown argument '{0}'", argument));
                                }
                                break;
                        }
                    }
                }
            }
        }

        #endregion Private Instance Methods

        #region Private Static Methods

        /// <summary>
        /// Returns the <see cref="CommandLineArgumentAttribute" /> that's applied 
        /// on the specified property.
        /// </summary>
        /// <param name="propertyInfo">The property of which applied <see cref="CommandLineArgumentAttribute" /> should be returned.</param>
        /// <returns>
        /// The <see cref="CommandLineArgumentAttribute" /> that's applied to the 
        /// <paramref name="propertyInfo" />, or a null reference if none was applied.
        /// </returns>
        private static CommandLineArgumentAttribute GetCommandLineAttribute(PropertyInfo propertyInfo) {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(CommandLineArgumentAttribute), false);
            if (attributes.Length == 1)
                return (CommandLineArgumentAttribute) attributes[0];

            Debug.Assert(attributes.Length == 0);
            return null;
        }

        #endregion Private Static Methods

        #region Private Instance Fields

        private CommandLineArgumentCollection _argumentCollection; 
        private CommandLineArgument _defaultArgument;
        private Type _argumentSpecification;

        #endregion Private Instance Fields
    }
}

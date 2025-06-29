using System;
using System.Data;
using System.Diagnostics;

using MySql.Data.MySqlClient;
using Metreos.DatabaseScraper.Common;
using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.ApplicationFramework.Collections;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;

namespace Metreos.Native.YourNamespace
{
	/// <summary> 
	///     A native action class
	/// </summary>
	[PackageDecl("Metreos.Native.YourNamespace")]
	public class NativeAction : INativeAction // The only requirement of a native action is that it implements INativeAction
	{
        /// <summary>
        ///     Inputs to a native action are defined by creating a public, settable property,
        ///     and decorating with the ActionParamField attribute.
        ///     Create as many action parameters as needed.
        ///     
        ///     Note that the type (in this case 'object') can be any primitive or native type you have defined
        /// </summary>
        [ActionParamField("A description of the input item", false /* Is this input required? */)]
        public object SomeInput { set { someInput = value; } }
        private object someInput;

        /// <summary>
        ///     Outputs to a native action are defined by creating a public, gettable property,
        ///     and decorting with the ResultDataField attribute.
        ///     Create as many result parameters as needed.
        ///     
        ///     As with action parameters, the type can be any type mapping to a native type or primitive in the application environment
        /// </summary>
        [ResultDataField("A description of the output item")]
        public ErrorDataCollection SomeOutput { get { return someOutput; } }
        private ErrorDataCollection someOutput;

        /// <summary>
        ///     Must exist per INativeAction definition.
        ///     The Application Runtime will set the Log property when it is 1st constructed
        /// </summary>
        public LogWriter Log { set { log = value; } }
        private LogWriter log;
		
        // One native action instance is shared throughout a script instance.  This constructor
        // is called with the script starts
        public NativeAction()
		{
			Clear();
		}

        /// <summary>
        ///     Clear is called after every invokation of the native action
        ///     'Execute' method to clear it back to it's original state
        /// </summary>
        public void Clear()
        {
            someInput = null;
            someOutput = null;
        }

        /// <summary>
        ///     ValidateInput gives one a chance to perform custom validations to the inputs
        ///     of the action before Execute is invoked, but after all the action parameters have been set.
        ///     
        /// </summary>
        /// <returns>true if the inputs are acceptable, false if not</returns>
        public bool ValidateInput()
        {
            bool valid = true;
            if(someInput == "The unacceptable string")
            {
                valid = false;
            }

            return valid;
        }

        /// <summary>
        ///     The execute method is the entry point into the native action.  
        ///     The action parameters have been assigned to by the ARE...
        ///     It's up to the coder to make something useful out of it.
        ///     Be sure to assign to any result parameters before exiting.
        /// </summary>
        /// <returns>A string representing the branch condition</returns>
        //[Action(Name,  AllowCustomParams, DisplayName, Description)]
        [Action("Query", false, "Query", "Queries the specified time frame for errors")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            // TODO:  Make something useful

            return "Success";
        }
	}
}

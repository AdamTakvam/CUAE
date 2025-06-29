using System;

namespace Metreos.MMSTestTool.Commands
{

    /// <summary>
    /// Command, Script, and TestFixture objects inherit from this class, which defines fields, properties,
    /// and methods that all of the above share.
    /// </summary>
	public abstract class CommandBase
	{

        /// <summary>
        /// returns the name of this element
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        
        /// <summary>
        /// Used to determine whether or not this element is sent to the MMS
        /// </summary>
        public bool Execute
        {
            get { return execute; }
            set { execute = value; }
        }

        /// <summary>
        /// Returns the string value of the result of this action.
        /// </summary>
        public string Result
        {
            get { return result; }
        }
	
        protected string name = string.Empty;
        protected string result = Constants.NOT_EXECUTED;
        // boolean used to determine if the Fixture is to be executed
        protected bool execute;
    }
}

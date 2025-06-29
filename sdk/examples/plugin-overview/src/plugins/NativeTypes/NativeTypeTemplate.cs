using System;
using Metreos.ApplicationFramework;

namespace Metreos.Types.YourNamespace
{
	/// <summary>
	///     NativeType Template
	/// </summary>
	public class NativeType : IVariable // IVariable implement
	{
		public NativeType()
		{
        }

        #region IVariable Members

        /// <summary>
        ///     IVariable requires that only this method be overridden.
        ///     This method is called when the runtime environment 
        ///     initializes the variable if the 'DefaultValue' field
        ///     has been specified in the developer, or if the variable
        ///     is being initialized with a configuration item of the 
        ///     application.
        /// </summary>
        /// <param name="str">
        ///     The initial value specified
        /// </param>
        /// <returns>
        ///     <c>true</c> if the string could be used to initialize,
        ///     otherwise <c>false</c>
        /// </returns>
        public bool Parse(string str)
        {
            // In this case, we are doing nothing with the string.
            return true;
        }

        /// <summary>
        ///     When a type other than string is sent as the initial value,
        ///     (possible when the 'InitializeWith' field specifies an event
        ///     parameter) the Application Runtime Enviroment will attempt
        ///     to find a method named Parse and with an argument 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>
        ///     <c>true</c> if the non-string could be used to initialize,
        ///     otherwise <c>false</c>
        /// </returns>
        public bool Parse(object nonStringType)
        {
            // In this case, we are doing nothing with the non-string.
            return true;
        }

        #endregion
    }
}

using System;
using Metreos.ApplicationFramework;
using InnerErrorDataCollection = Metreos.DatabaseScraper.Common.ErrorDataCollection;

namespace Metreos.Types.DatabaseScraper
{
	/// <summary>
	///     A collection of elements which are essentially wrappers 
	///     of the columns defined by the 'errors' table.
	/// </summary>
	public class ErrorDataCollection : IVariable
	{
        public InnerErrorDataCollection Collection { get { return collection; } }
        private InnerErrorDataCollection collection;

		public ErrorDataCollection()
		{
			collection = new InnerErrorDataCollection();
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
            // Do nothing.  We don't intend to allow developer to be able
            // to initialize this type via config or InitializeWith
            return true;
        }

        /// <summary>
        ///     When a type other than string is sent as the initial value,
        ///     (possible when the 'InitializeWith' field specifies an event
        ///     parameter) the Application Runtime Enviroment will attempt
        ///     to find a method named Parse and with an argument 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public bool Parse(InnerErrorDataCollection collection)
        {
            if(collection != null)
            {
                this.collection = collection;
            }
            return true;
        }

        #endregion
    }
}

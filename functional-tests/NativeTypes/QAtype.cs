using System;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Types.FunctionalTests
{
	/// <summary>
	/// Summary description for QAtype.
	/// </summary>
	public class QAtype : IVariable // IVariable implement
	{
		public string myStr;
		//To use a log in a Native type, 1st create the LogWriter 
		//(Verbose means that this log will not filter anything you send it, 
		//and “Native” is the name it will show up as in your AppServer log)
        //
		public LogWriter log = new LogWriter(TraceLevel.Verbose, "Native");

		public QAtype()
		{
           myStr="";     
		}

		#region IVariable Members

		public override string ToString ()
		{   //create a custom statement here
			string str=this.myStr.ToString();
			return "QAtype string: " + str;			
		}
		
		// this is the entry point for the value into the class
		public bool Parse(string str)
		{

          //add code to test the validity of the string here.
			if (str.ToUpper().IndexOf("INVALID") == -1)
			{
				myStr=str;
				return true; //Convert.ToBoolean(IApp.VALUE_SUCCESS);//true; //
			} 
			else 
			{
			  //throw new System.Exception("String Contains the 'INVALID' substring.");
			  log.Write(System.Diagnostics.TraceLevel.Warning, "QAtype String Parse ERROR: the string failed the parse test? " + str);
              return false; //Convert.ToBoolean(IApp.VALUE_FAILURE); //false; //
			}

			//return true;
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
			string str=nonStringType.ToString();
			if (str.ToUpper().IndexOf("INVALID") == -1)
			{
				myStr=str;
				return true; //Convert.ToBoolean(IApp.VALUE_SUCCESS);//true; //
			} 
			else 
			{
				//throw new System.Exception("String Contains the 'INVALID' substring.");
				log.Write(System.Diagnostics.TraceLevel.Warning, "QAtype Object Parse ERROR: the string failed the parse test? " + str);
				return false; //Convert.ToBoolean(IApp.VALUE_FAILURE); //false; //
			}
		}
        


		#endregion

	}
}

using System;
using System.Data;
using System.Diagnostics;

using MySql.Data.MySqlClient;
using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.ApplicationFramework.Collections;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;

using System.IO;

namespace Metreos.Native.FunctionalTests
{
	/// <summary> 
	///     A native action class
	/// </summary>
	[PackageDecl("Metreos.Native.FunctionalTests")]
	public class FileSize : INativeAction // The only requirement of a native action is that it implements INativeAction
	{
		/// <summary>
		///     Inputs to a native action are defined by creating a public, settable property,
		///     and decorating with the ActionParamField attribute.
		///     Create as many action parameters as needed.
		///     
		///     Note that the type (in this case 'object') can be any primitive or native type you have defined
		/// </summary>
		[ActionParamField("Filename", true)]
		public object SomeInput { set { someInput = value; } }
		private object someInput;

		/// <summary>
		///     Outputs to a native action are defined by creating a public, gettable property,
		///     and decorting with the ResultDataField attribute.
		///     Create as many result parameters as needed.
		///     
		///     As with action parameters, the type can be any type mapping to a native type or primitive in the application environment
		/// </summary>
		[ResultDataField("File size")]
		public long SomeOutput { get { return someOutput; } }
		private long someOutput;

		/// <summary>
		///     Must exist per INativeAction definition.
		///     The Application Runtime will set the Log property when it is 1st constructed
		/// </summary>
		public LogWriter Log { set { log = value; } }
		private LogWriter log;
		
		// One native action instance is shared throughout a script instance.  This constructor
		// is called when the script starts
		public FileSize()
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
			//someOutput = null;
			someOutput = 0;
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
			if(someInput.ToString() == "Whatever.txt")
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
		[Action("FileSize", false, "FileSize", "Returns the size of a file")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility) 
		{
			// TODO:  Make something useful

			//Input
			//string Path="C:\\temp\\dev\\c\\output\\Whatever.txt";
			//Console.WriteLine("testing...");
			     
			log.Write(System.Diagnostics.TraceLevel.Info, "FileName: " + someInput);
			     
			if (File.Exists(someInput.ToString())) 
			{

				//long size=0;
				FileStream file_stream = new FileStream(someInput.ToString(), FileMode.Open);
				//Output
				//Console.WriteLine("size: " + file_stream.Length );
				someOutput=file_stream.Length;
				log.Write(System.Diagnostics.TraceLevel.Info, "size: " + file_stream.Length);
				file_stream.Close();
			} 
			else 
			{  
				//Output
				//Console.WriteLine("Error: File Not Found - " + Path);
			}
    			 
			return someOutput.ToString();
		}
	}
}
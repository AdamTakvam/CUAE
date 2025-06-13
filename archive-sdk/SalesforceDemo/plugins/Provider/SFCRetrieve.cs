using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;

namespace Cisco.ASG.VoiceConActions
{
	/// <summary> 
	///     A native action class
	/// </summary>
	[PackageDecl("Cisco.ASG.VoiceConActions", "VoiceCon Actions")]
	public class SFCRetrieve : INativeAction // The only requirement of a native action is that it implements INativeAction
	{
		/// <summary>
		///     Inputs to a native action are defined by creating a public, settable property,
		///     and decorating with the ActionParamField attribute.
		///     Create as many action parameters as needed.
		///     
		///     Note that the type (in this case 'object') can be any primitive or native type you have defined
		/// </summary>
		[ActionParamField("User", true /* Is this input required? */)]
		public string User { set { mUser = value; } }
		private string mUser;
		[ActionParamField("Password", true /* Is this input required? */)]
		public string Password { set { mPassword = value; } }
		private string mPassword;
		[ActionParamField("Proxy", false /* Is this input required? */)]
		public string Proxy { set { mProxy = value; } }
		private string mProxy;
		[ResultDataField("AccountList", "Account List")]
		public DataTable AccountList { get { return mAccountList; } }
		private DataTable mAccountList;

		/// <summary>
		///     Must exist per INativeAction definition.
		///     The Application Runtime will set the Log property when it is 1st constructed
		/// </summary>
		public LogWriter Log { set { log = value; } }
		private LogWriter log;
		
		// One native action instance is shared throughout a script instance.  This constructor
		// is called with the script starts
		public SFCRetrieve()
		{
			Clear();
		}

		/// <summary>
		///     Clear is called after every invokation of the native action
		///     'Execute' method to clear it back to it's original state
		/// </summary>
		public void Clear()
		{
			mUser = null;
			mPassword = null;
			mProxy = null;

			//
			// Create a DataTable with three columns called "index", 
			// "id" & "name".  This is an output parameter.
			//
			mAccountList = new DataTable();
			mAccountList.Columns.Add("index");
			mAccountList.Columns.Add("id");
			mAccountList.Columns.Add("name");
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

			if(mUser == "The unacceptable string")
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
		[Action("SFCRetrieve", false, "SalesForce.com Retrieve", "SalesForce.com Retrieve")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility) 
		{
			// TODO:  Make something useful
			try
			{
				SforceService sfs = new SforceService();

				//
				// This is required for allowing lab machines at Cisco to 
				// access SFC.  In general, proxy will not be required.
				//
				if (mProxy != null)
				{
					log.Write(System.Diagnostics.TraceLevel.Info, "Setting proxy");
					sfs.Proxy = new System.Net.WebProxy(mProxy);
				}

				//
				// Connect to SFC
				//
				LoginResult lr = new LoginResult();
				log.Write(System.Diagnostics.TraceLevel.Info, "Logging in");
				lr = sfs.login(mUser, mPassword);

				//
				// Retrieve account information
				//
				QueryResult qr = new QueryResult();
				sfs.Url = lr.serverUrl;
				sfs.SessionHeaderValue = new SessionHeader();
				sfs.SessionHeaderValue.sessionId = lr.sessionId;
				log.Write(System.Diagnostics.TraceLevel.Info, "Running query");
				qr = sfs.query("select Id, Name from Account");
				log.Write(System.Diagnostics.TraceLevel.Info, "Results returned = " + qr.size.ToString());

				//
				// Populate DataTable to be returned back to the designer
				//
				for (int i=0; i<qr.size; i++)
				{
					object[] row = new object[3];
					row[0] = i + 1;
					row[1] = ((Account)qr.records[i]).Id;
					row[2] = ((Account)qr.records[i]).Name;
					mAccountList.Rows.Add(row);
				}

				return IApp.VALUE_SUCCESS;
			}
			catch (Exception e)
			{
				log.Write(System.Diagnostics.TraceLevel.Error, e.StackTrace);
				return IApp.VALUE_FAILURE;
			}
		}
	}
}
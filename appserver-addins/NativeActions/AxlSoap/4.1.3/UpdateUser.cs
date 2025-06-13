using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;

using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.AxlSoap;
using Metreos.AxlSoap413;
using Metreos.Types.AxlSoap413;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap413.Actions.UpdateUser;

namespace Metreos.Native.AxlSoap413
{
	/// <summary> Wraps up the 'updateUser' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
	public class UpdateUser : INativeAction
	{     
		[ActionParamField(Package.Params.UpdateUserId.DISPLAY, Package.Params.UpdateUserId.DESCRIPTION, true, Package.Params.UpdateUserId.DEFAULT)]
		public string UpdateUserId { set { userid = value; } }

		[ActionParamField(Package.Params.AllowCallParkRetrieval.DISPLAY, Package.Params.AllowCallParkRetrieval.DESCRIPTION, false, Package.Params.AllowCallParkRetrieval.DEFAULT)]
		public bool AllowCallParkRetrieval { set { allowCallParkRetrieval = value; allowCallParkRetrievalSpecified = true;} }

		[ActionParamField(Package.Params.AssociatedDevices.DISPLAY, Package.Params.AssociatedDevices.DESCRIPTION, false, Package.Params.AssociatedDevices.DEFAULT)]
		public StringCollection AssociatedDevices { set { associatedDevices = value; } }

		[ActionParamField(Package.Params.AuthenticationProxyRights.DISPLAY, Package.Params.AuthenticationProxyRights.DESCRIPTION, false, Package.Params.AuthenticationProxyRights.DEFAULT)]
		public bool AuthenticationProxyRights { set { authenticationProxyRights = value; authenticationProxyRightsSpecified = true; } }

		[ActionParamField(Package.Params.CallingNumberModAllowed.DISPLAY, Package.Params.CallingNumberModAllowed.DESCRIPTION, false, Package.Params.CallingNumberModAllowed.DEFAULT)]
		public bool CallingNumberModAllowed { set { callingNumberModAllowed = value; callingNumberModAllowedSpecified = true; } }

		[ActionParamField(Package.Params.EnableCTI.DISPLAY, Package.Params.EnableCTI.DESCRIPTION, false, Package.Params.EnableCTI.DEFAULT)]
		public bool EnableCTI { set { enableCTI = value; enableCTISpecified = true; } }

		[ActionParamField(Package.Params.EnableCTISuperProvider.DISPLAY, Package.Params.EnableCTISuperProvider.DESCRIPTION, false, Package.Params.EnableCTISuperProvider.DEFAULT)]
		public bool EnableCTISuperProvider { set { enableCTISuperProvider = value; enableCTISuperProviderSpecified = true; } }

		[ActionParamField(Package.Params.UserLocale.DISPLAY, Package.Params.UserLocale.DESCRIPTION, false, Package.Params.UserLocale.DEFAULT)]
		public XCountry UserLocale { set { locale = value; localeSpecified = true; } }

		[ActionParamField(Package.Params.Department.DISPLAY, Package.Params.Department.DESCRIPTION, false, Package.Params.Department.DEFAULT)]
		public string Department { set { department = value; } }

		[ActionParamField(Package.Params.Extension.DISPLAY, Package.Params.Extension.DESCRIPTION, false, Package.Params.Extension.DEFAULT)]
		public string Extension { set { extension = value; } }

		[ActionParamField(Package.Params.FirstName.DISPLAY, Package.Params.FirstName.DESCRIPTION, false, Package.Params.FirstName.DEFAULT)]
		public string FirstName { set { firstname = value; } }

		[ActionParamField(Package.Params.LastName.DISPLAY, Package.Params.LastName.DESCRIPTION, false, Package.Params.LastName.DEFAULT)]
		public string LastName { set { lastname = value; } }

		[ActionParamField(Package.Params.IAQExtension.DISPLAY, Package.Params.IAQExtension.DESCRIPTION, false, Package.Params.IAQExtension.DEFAULT)]
		public string IAQExtension { set { iaqExtension = value; } }

		[ActionParamField(Package.Params.Manager.DISPLAY, Package.Params.Manager.DESCRIPTION, false, Package.Params.Manager.DEFAULT)]
		public string Manager { set { manager = value; } }

		[ActionParamField(Package.Params.UserPassword.DISPLAY, Package.Params.UserPassword.DESCRIPTION, false, Package.Params.UserPassword.DEFAULT)]
		public string UserPassword { set { userPassword = value; } }

		[ActionParamField(Package.Params.UserPin.DISPLAY, Package.Params.UserPin.DESCRIPTION, false, Package.Params.UserPin.DEFAULT)]
		public string UserPin { set { pin = value; } }
	
		[ActionParamField(Package.Params.TelephoneNumber.DISPLAY, Package.Params.TelephoneNumber.DESCRIPTION, false, Package.Params.TelephoneNumber.DEFAULT)]
		public string TelephoneNumber { set { telephoneNumber = value; } }
	
		[ActionParamField(Package.Params.PhoneProfiles.DISPLAY, Package.Params.PhoneProfiles.DESCRIPTION, false, Package.Params.PhoneProfiles.DEFAULT)]
		public StringCollection PhoneProfiles { set { PhoneProfiles = value; } }
	
		[ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
		public string CallManagerIP { set { callManagerIP = value; } }

		[ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
		public string AdminUsername { set { username = value; } }

		[ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
		public string AdminPassword { set { password = value; } }

		[ResultDataField(Package.Results.UpdateUserResponse.DISPLAY, Package.Results.UpdateUserResponse.DESCRIPTION)]
		public updateUserResponse UpdateUserResponse { get { return response; } }

		[ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
		public string FaultMessage { get { return message; } }

		[ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
		public int FaultCode { get { return code; } }

		public LogWriter Log { set { log = value; } }

		private bool allowCallParkRetrieval;
		private bool allowCallParkRetrievalSpecified;
		private StringCollection associatedDevices;
		private string associatedPC;
		private bool authenticationProxyRights;
		private bool authenticationProxyRightsSpecified;
		private bool callingNumberModAllowed;
		private bool callingNumberModAllowedSpecified;
		private string department;
		private bool enableCTI;
		private bool enableCTISpecified;
		private bool enableCTISuperProvider;
		private bool enableCTISuperProviderSpecified;
		private string extension;
		private string firstname;
		private string iaqExtension;
		private string lastname;
		private string manager;
		private string userPassword;
		private StringCollection phoneProfiles;
		private string pin;
		private string telephoneNumber;
		private string userid;
		private XCountry locale;
		private bool localeSpecified;
		private string username;
		private string password;
		private string message;
		private string callManagerIP;
		private int code;
		private LogWriter log;
		private updateUserResponse response;

		public UpdateUser()
		{
			Clear();	
		}

		public void Clear()
		{
			this.allowCallParkRetrieval		= false;
			this.allowCallParkRetrievalSpecified = false;
			this.associatedDevices			= null;
			this.associatedPC				= null;
			this.authenticationProxyRights	= false;
			this.authenticationProxyRightsSpecified = false;
			this.callingNumberModAllowed	= false;
			this.callingNumberModAllowedSpecified = false;
			this.department					= null;
			this.enableCTI					= false;
			this.enableCTISpecified			= false;
			this.enableCTISuperProvider		= false;
			this.enableCTISuperProviderSpecified = false;
			this.extension					= null;
			this.firstname					= null;
			this.iaqExtension				= null;
			this.lastname					= null;
			this.manager					= null;
			this.password					= null;
			this.phoneProfiles				= null;
			this.pin						= null;
			this.telephoneNumber			= null;
			this.userid						= null;
			this.locale						= XCountry.UnitedStates;
			this.localeSpecified			= false;
			this.username                   = IAxlSoap.DefaultCcmAdmin;
			this.userPassword               = null;
			this.callManagerIP              = null;
			this.response                   = new updateUserResponse();
			this.message                    = String.Empty;
			this.code                       = 0;
		}

		public bool ValidateInput()
		{
			return true;
		} 

		public enum Result
		{
			success,
			failure,
			fault,
		}

		[ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
		[Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
		public string Execute(SessionData sessionData, IConfigUtility configUtility) 
		{
			AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
                         
			updateUser user = new updateUser();
			
			user.allowCallParkRetrieval = allowCallParkRetrieval;
			user.allowCallParkRetrievalSpecified = allowCallParkRetrievalSpecified;

			// Couldn't find way to clear associated devices in testing
			if(associatedDevices != null)
			{
				user.associatedDevices = new string[associatedDevices.Count];
				associatedDevices.CopyTo(user.associatedDevices, 0);
			}
			user.associatedPC = associatedPC;
			user.authenticationProxyRights = authenticationProxyRights;
			user.authenticationProxyRightsSpecified = authenticationProxyRightsSpecified;
			user.callingNumberModAllowed = callingNumberModAllowed;
			user.callingNumberModAllowedSpecified = callingNumberModAllowedSpecified;
			user.department = department;
			user.enableCTI = enableCTI;
			user.enableCTISpecified = enableCTISpecified;
			user.enableCTISuperProvider = enableCTISuperProvider;
			user.enableCTISuperProviderSpecified = enableCTISuperProviderSpecified;
			user.extension = user.extension;
			user.firstname = firstname;
			user.iaqExtension = iaqExtension;
			user.lastname = lastname;
			user.manager = manager;
			user.password = userPassword;
			user.pin = pin;
			user.telephoneNumber = telephoneNumber;
			user.userid = userid;
			user.locale = locale;
			user.localeSpecified = localeSpecified;

			// Didn't find way to clear associated profiles through testing. 
			// Also, tried XPhoneProfile overload-only string method worked
			if(phoneProfiles != null)
			{
				user.phoneProfiles = new UpdateUserReqPhoneProfiles();
				string[] updatedPhoneProfiles = new string[phoneProfiles.Count];
				phoneProfiles.CopyTo(updatedPhoneProfiles, 0);
				user.phoneProfiles.Items = updatedPhoneProfiles;
			}

			try
			{
				response = client.updateUser(user);
			}
			catch(System.Web.Services.Protocols.SoapException e)
			{
				IAxlSoap.ReportSoapError(e, log, ref code, ref message);

				return Result.fault.ToString();
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, Metreos.Utilities.Exceptions.FormatException(e));
				return IApp.VALUE_FAILURE;
			}

			return IApp.VALUE_SUCCESS;
		}
	}
}

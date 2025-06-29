using System;
using System.Threading;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;
using System.Data;
namespace devicecreator
{
	/// <summary>  </summary>
	public class Worker
	{
        private AXLAPIService service;
        private int writeWait;
        private ArrayList errors;

        public Worker(AXLAPIService service, int maxAxlWrite)
		{
            this.writeWait = 60 * 1000 / maxAxlWrite + 300;
            this.service = service;  	
            this.errors = new ArrayList();
        }
        
        public bool Generate(   long deviceStart, int numPhones, string password, 
                                string pin, bool associateDp)
        {
            for(int i = 0; i < numPhones; i++)
            {
                Thread.Sleep(writeWait);

                string username = ConvertMACToUser(deviceStart + i);

                addUser user = new addUser();
                user.newUser = new XUser();
                user.newUser.userid = username;
                user.newUser.firstname = username;
                user.newUser.lastname = username;
                user.newUser.password = password;
                user.newUser.pin = pin;

                if(associateDp)
                {
                    user.newUser.phoneProfiles = new XUserPhoneProfiles();
                    user.newUser.phoneProfiles.Items = new string[1];
                    user.newUser.phoneProfiles.Items[0] = ConvertMACToDP(deviceStart + i);
                }

                try
                {
                    service.addUser(user);
                    Console.WriteLine("Added user {0}", username);
                }
                catch(Exception e)
                { 
                    ReportError(String.Format("Could not add user.\nReason:\n{0}", GetDetail(e)));
                }
                
            }

            if(errors.Count > 0)
            {
                ReportErrors();
            }
            
            return true;
        }
        
        private void ReportError(string error)
        {
            Console.WriteLine(error);
            errors.Add(error);
        }
        
        private void ReportErrors()
        {
            Console.WriteLine("Dumping all errors:\n");
            foreach(string error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private string GetDetail(Exception e)
        {
            string exceptionMsg = e.ToString();
            if(e is System.Web.Services.Protocols.SoapException)
            {
                System.Web.Services.Protocols.SoapException soapy = e as System.Web.Services.Protocols.SoapException;
                if(soapy.Detail != null)
                {
                    exceptionMsg = soapy.Detail.InnerText;
                }
            }

            return exceptionMsg;
        }

        private string ConvertMACToUser(long deviceMac)
        {
            return "U" + deviceMac.ToString("x").PadLeft(12, '0').ToUpper();
        }

        private string ConvertMACToDP(long deviceMac)
        {
            return "DP" + deviceMac.ToString("x").PadLeft(12, '0').ToUpper();
        }

        private string ConvertMACToString(long deviceMac)
        {
            return deviceMac.ToString("x").PadLeft(12, '0');
        }
	}
}
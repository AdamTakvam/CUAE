using System;
using System.Management;
using System.Net;

namespace Metreos.Homer
{
	/// <summary>
	/// Summary description for HttpRequester.
	/// </summary>
	class HttpRequester
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            HttpRequester requester = new HttpRequester();
            string sData = requester.GetRegistrationData();
            requester.SendRegistrationData("http://www.metreos.com/licensemanager/recordagent", sData);
		}

        private string GetRegistrationData()
        {
            string sData = DateTime.Now.ToUniversalTime().ToString("MMddyyhhmmss");
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (((bool)mo["IPEnabled"]) == true && (mo["IPAddress"].ToString().Length > 0))
                {
                    sData += "-";
                    sData += mo["MACAddress"].ToString();
                }
            }
            return sData;
        }

        private void SendRegistrationData(string sUri, string sData)
        {
            string url = sUri + "?data=" + sData;
            Uri uri = new Uri(url);

            try
            {
                HttpWebRequest request= (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response= (HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch //(WebException we)
            {
                // may need to handle this case when we are serious about 
                //Console.WriteLine(we.Response);
            }
        }
	}
}

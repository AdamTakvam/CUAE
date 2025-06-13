using System;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Collections;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.Providers.CiscoDeviceListX
{
    internal sealed class HttpDlxReader : DeviceListXReader
    {
        // Definitions
        private const string URL_SUFFIX         = "/ccmadmin/reports/devicelistx.asp";

        // variables
        private DeviceListXInfo dlxInfo;
        
        public HttpDlxReader(LogWriter log, CallManagerCluster cluster) : base(log, cluster)
        {
            dlxInfo = new DeviceListXInfo();
        }

        internal override bool Initialize()
        {
            bool success = true;

            dlxInfo.ccmIP = cluster.PublisherIP;

            dlxInfo.username = cluster.PublisherUsername;

            if ((cluster.PublisherPassword == null) || (cluster.PublisherPassword == String.Empty))
            {
                log.Write(TraceLevel.Warning, "CallManager cluster '{0}' has no password configured",
                    cluster.Name);
                dlxInfo.password = string.Empty;
            }
            else
                dlxInfo.password = cluster.PublisherPassword;

            if (cluster.Version >= 5.0)
            {
                log.Write(TraceLevel.Warning, "The HTTP DeviceListX reader can not be used with CallManager version 5.0 or greater.");
                success = false;
            }                          
            else if (cluster.Version > 4.0)
            {
                dlxInfo.url = String.Format("https://{0}{1}", cluster.PublisherIP, URL_SUFFIX);
            }
            else
            {
                dlxInfo.url = String.Format("http://{0}{1}", cluster.PublisherIP, URL_SUFFIX);
            }

            return success; 
            //log.Write(TraceLevel.Info, "Adding '{0} v{1}' to DeviceListX cache list", 
              //  cluster.publisherIP, cluster.version.ToString());
        } // Initialize

        internal override bool RetrieveDeviceList(ref bool shutdownFlag)
        {
            bool success = true;
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            StreamReader reader = null;

            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(dlxInfo.url);
            }
            catch(Exception)
            {
                log.Write(TraceLevel.Error, "Invalid url provided: " + dlxInfo.url);
                success = false;
            }

            if (success)
            {
                webRequest.Credentials = new NetworkCredential(dlxInfo.username, dlxInfo.password);
    
                try
                {
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    reader = new StreamReader(responseStream);
                }
                catch(Exception e)
                {      
                    if (reader != null) { reader.Close(); }
                    if (webResponse != null) { webResponse.Close(); }

                    log.Write(TraceLevel.Error, 
                        "Error while reading DeviceListX data from '{0}': {1}", dlxInfo.url, e.Message);
                    success = false;
                }
            }

            if(shutdownFlag)
                return true;

            if (success)
            {
                XmlSerializer serializer = null;
                try
                {
                    serializer = new XmlSerializer(typeof(DeviceList));
                    base.deviceList = (DeviceList) serializer.Deserialize(reader);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Error parsing device list data from '{0}': {1}",
                        dlxInfo.url, e.Message);
                    success = false;
                }
                finally
                {
                    reader.Close();
                    webResponse.Close();
                }

                // check to see if an error message was returned in the XML.
                if (deviceList.IsError)
                {
                    success = false;
                    // CCM Docs only mention 1001 and 1002 as possible
                    string errorNumber  = deviceList.Error.number;
                    // CCM Docs only mention 1001 as 
                    //      Too many simultaneous requests for Device List. 
                    //      Please wait at least 60 seconds and try again.
                    // CCM Docs only mention 1002 as 
                    //      Too many consecutive requests for Device List. 
                    //      Please wait at least 60 seconds and try again. 
                    string errorMsg     = deviceList.Error.Value;
                    log.Write(TraceLevel.Error, 
                        "DeviceListX service for {0} returned an error message:  ({1}) {2}", dlxInfo.ccmIP, errorNumber, errorMsg);
                }
            }

            return success;
        }
    }
}

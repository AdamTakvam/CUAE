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
        
        public bool Generate(   long deviceStart, int baseDn, int numPhones, 
                                string description, int increment, string css, 
                                string partition, string phoneTemplateName)
        {
            for(int i = 0; i < numPhones; i++)
            {
                string lineId = null;

                getLine getLine = new getLine();
                getLine.pattern = (i * increment + baseDn).ToString();
                getLine.routePartitionName = partition;
                getLine.routeFilterName = String.Empty;

                try
                {
                    getLineResponse getLineResponse = service.getLine(getLine);
                    lineId = getLineResponse.@return.directoryNumber.uuid;
                }
                catch(Exception e)
                {
                    if(e is System.Web.Services.Protocols.SoapException)
                    {
                        System.Web.Services.Protocols.SoapException soapy = e as System.Web.Services.Protocols.SoapException;

                        if(soapy.Detail != null && soapy.Detail.InnerText.IndexOf("The specified Directory Number was not found") > -1)
                        {
                            // This is ok, the line simply doesn't exist
                        }
                        else
                        {
                            ReportError(String.Format("Unable to retrieve an existing line.  \n{0}", GetDetail(e)));
                        }
                    }  
                }

                Thread.Sleep(75);

                if(lineId == null)
                {
                    addLine line = new addLine();
                    line.newLine = new XNPDirectoryNumber();
                    line.newLine.pattern = (i * increment + baseDn).ToString();
                    line.newLine.Item = partition;
                    line.newLine.Item3 = css;
                    try
                    {
                        addLineResponse lineResponse = service.addLine(line);
                        lineId = lineResponse.@return;
                    }
                    catch(Exception e)
                    {
                        ReportError(String.Format("Unable to create line number {0}:{1}.  \n{2}", (i + baseDn), partition, GetDetail(e)));
                    }
                }

                Thread.Sleep(writeWait);

                XPhoneLines lines = null;
                
                if(lineId != null)
                {
                    lines = new XPhoneLines();
                    lines.Items = new XLine[1];
                    XLine oneLine = new XLine();
                    oneLine.index = "1";   
                    oneLine.e164Mask = null; // Had to
                    oneLine.ringSetting = XRingSetting.UseSystemDefault; // Had to
                    oneLine.Item = new XNPDirectoryNumber();
                    oneLine.Item.uuid = lineId;
                    lines.Items[0] = oneLine; 
                }
                else
                {
                    lines = new XPhoneLines();
                    lines.Items = new XLine[0];
                    
                    ReportError(String.Format("Phone {0} will not have a line.", ConvertMACToDP(deviceStart + i)));
                }

                addDeviceProfile phone = new addDeviceProfile();
                XPhoneProfile xipphone = new XPhoneProfile();
                xipphone.description = description + " " + (baseDn + i);
                xipphone.name = ConvertMACToDP(deviceStart + i);
                xipphone.lines = lines;
                xipphone.@class = XClass.Phone;
                xipphone.addOnModules = null;
                
                xipphone.protocol = XDeviceProtocol.Ciscostation; // THIS MUST BE THIS FOR 7960!!
                xipphone.Item1 = XModel.Cisco7960;
                xipphone.Item = XProduct.Cisco7960;
                xipphone.Item2 = css;
                xipphone.Item3 = "Default";
                xipphone.Item8 = phoneTemplateName;
                xipphone.ownerUserId = null;
                
                xipphone.protocolSide = XProtocolSide.User;
                phone.newProfile = xipphone;

                try
                {
                    service.addDeviceProfile(phone);
                    Console.WriteLine("{0} added with line number {1}", ConvertMACToDP(deviceStart + i), lineId == null ? "-" : (i * increment + baseDn).ToString());
                }
                catch(Exception e)
                {
                    ReportError(String.Format("Unable to add the phone '{0}'.  \n{1}", ConvertMACToDP(deviceStart + i), GetDetail(e)));
                }

                Thread.Sleep(writeWait);
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
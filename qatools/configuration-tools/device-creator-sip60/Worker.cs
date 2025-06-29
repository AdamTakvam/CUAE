using System;
using System.Threading;
using System.Collections;
using Metreos.AxlSoap501;
using Metreos.Utilities;
using System.Data;
namespace devicecreator
{
	/// <summary>  </summary>
	public class Worker
	{
        private AXLAPIService service;
        private int writeWait;
        private ArrayList callerDns;
        private ArrayList receiverDns;
        private ArrayList errors;

        public Worker(AXLAPIService service, int maxAxlWrite)
		{
            this.writeWait = 60 * 1000 / maxAxlWrite + 300;
            this.service = service;  	
            this.callerDns = new ArrayList();
            this.receiverDns = new ArrayList();
            this.errors = new ArrayList();
        }
        
        public bool Generate(   long deviceStart, int baseDn, int numPhones, 
                                string description, int increment, string css, 
                                string partition, string phoneTemplateName)
        {
            for(int i = 0; i < numPhones; i++)
            {
                string lineId = null;
                
                ulong sequence = 0;
                bool sequenceSpecified = false;

                try
                {
                    //getLineResponse getLineResponse = service.getLine(getLine);
                    Metreos.AxlSoap501.AXLAPIService.GetLineResReturn response = service.getLine(null, (i * increment + baseDn).ToString(), null, partition, null, null, ref sequence, ref sequenceSpecified);
                    lineId = response.directoryNumber.uuid;
                }
                catch(Exception e)
                {
                    if(e is System.Web.Services.Protocols.SoapException)
                    {
                        System.Web.Services.Protocols.SoapException soapy = e as System.Web.Services.Protocols.SoapException;

                        if(soapy.Detail != null && soapy.Detail.InnerText.IndexOf("was not found") > -1)
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
                    Metreos.AxlSoap501.AXLAPIService.XNPDirectoryNumber line = new Metreos.AxlSoap501.AXLAPIService.XNPDirectoryNumber();
                    line.pattern = (i * increment + baseDn).ToString();
                    line.Item = partition;
                    line.Item3 =  css; //CSS!;
                    try
                    {
                        lineId = service.addLine(line, ref sequence, ref sequenceSpecified);
                    }
                    catch(Exception e)
                    {
                        ReportError(String.Format("Unable to create line number {0}:{1}.  \n{2}", (i + baseDn), partition, GetDetail(e)));
                    }
                }

                Thread.Sleep(writeWait);

                Metreos.AxlSoap501.AXLAPIService.XPhoneLines lines = null;
                
                string deviceName = ConvertMACToSEP(deviceStart + i);
                deviceName = deviceName.ToUpper();

                if(lineId != null)
                {
                    lines = new Metreos.AxlSoap501.AXLAPIService.XPhoneLines();
                    lines.Items = new Metreos.AxlSoap501.AXLAPIService.XLine[1];
                    Metreos.AxlSoap501.AXLAPIService.XLine oneLine = new Metreos.AxlSoap501.AXLAPIService.XLine();
                    oneLine.index = "1";
                    oneLine.e164Mask = null; // Had to
                    oneLine.ringSetting = Metreos.AxlSoap501.AXLAPIService.XRingSetting.UseSystemDefault; // Had to
                    oneLine.Item = new Metreos.AxlSoap501.AXLAPIService.XNPDirectoryNumber();
                    oneLine.Item.uuid = lineId;
                    lines.Items[0] = oneLine; 
                }
                else
                {
                    lines = new Metreos.AxlSoap501.AXLAPIService.XPhoneLines();
                    lines.Items = new Metreos.AxlSoap501.AXLAPIService.XLine[0];
                    
                    ReportError(String.Format("Phone {0} will not have a line.", deviceName));
                }
                
                Metreos.AxlSoap501.AXLAPIService.XIPPhone xipphone = new Metreos.AxlSoap501.AXLAPIService.XIPPhone();
                xipphone.description = description + " " + (baseDn + i);
               xipphone.name = deviceName;
                xipphone.lines = lines;
                xipphone.@class = Metreos.AxlSoap501.AXLAPIService.XClass.Phone;
                xipphone.addOnModules = null;
                
                xipphone.protocol = Metreos.AxlSoap501.AXLAPIService.XDeviceProtocol.SIP; // THIS MUST BE THIS FOR 7960!!
                //xipphone.Item1 = Metreos.AxlSoap501.AXLAPIService.XModel.ThirdPartySipAdvanced;
                //xipphone.Item = Metreos.AxlSoap501.AXLAPIService.XProduct.ThirdPartySipAdvanced;
                xipphone.Item1 = Metreos.AxlSoap501.AXLAPIService.XModel.Cisco7961GGE;
                xipphone.Item = Metreos.AxlSoap501.AXLAPIService.XProduct.Cisco7961GGE;
                xipphone.Item2 = css;
                xipphone.Item3 = "Default";
                //xipphone.Item8 = "Third-party SIP Device (Advanced)"; // Template
                xipphone.Item8 = "Standard 7961G-GE SIP"; // Template
                //xipphone.securityProfileName = "Standard SIP Profile for Auto Registration";
                xipphone.securityProfileName = "Cisco 7961G-GE - Standard SIP Non-Secure Profile";
                xipphone.protocolSide = Metreos.AxlSoap501.AXLAPIService.XProtocolSide.User;

                try
                {
                    service.addPhone(xipphone, ref sequence, ref sequenceSpecified);
                    Console.WriteLine("{0} added with line number {1}", deviceName, lineId == null ? "-" : (i * increment + baseDn).ToString());
                }
                catch(Exception e)
                {
                    ReportError(String.Format("Unable to add the phone '{0}'.  \n{1}", deviceName, GetDetail(e)));
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

        private string ConvertMACToSEP(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        private string ConvertMACToString(long deviceMac)
        {
            return deviceMac.ToString("x").PadLeft(12, '0');
        }
	}
}
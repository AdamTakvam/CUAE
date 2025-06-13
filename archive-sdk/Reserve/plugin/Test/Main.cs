using System;
using System.IO;
using System.Text;
using System.Collections;
using Metreos.Utilities;
using System.Xml.Serialization;
using System.Xml;

using Metreos.Common.Reserve;

namespace Test
{
	class Class1
	{
        private const string Usage = @"
-t:  Type  Reserve | Release | Display | SerializeSaveBoRequest | SerializeSaveBoResponse | TririgaUserManagement | Simulation |
-d:  DeviceName 
-p:  DeviceProfile
-o:  timeout
-url: url
-u:  ccmuser
-f:  First Name
-l:  Last Name
-ccmip: CCMIP
-poll: Poll Amount

Simulation Params:
-bhca:  BHCA of Reservations
-time:  Test time (s)
-loginDuration:  Length of time to leave user logged in (s)
-startMAC:  MAC Address of synchronized device/profile/user
-count:   Count of synchronized device/profile/users
-reserveUrl:  URL to issue reserve commands to
-releaseUrl:  URL to issue release commands to  


Tririga Integration Params
-iu: Integration User
-ri: RecordId
-st: SecurityToken

-du: Device:User:First:Last

-ip: Integration Pass
-ih: Integration Host
";
		[STAThread]
		static void Main(string[] args)
		{
			CommandLineArguments parser = new CommandLineArguments(args);
            
            bool isConfirm = false;
            bool isReserve = false;
            string deviceName = null;
            string deviceProfile = null;
            string url = null;
            string ccmuser = null;
            string timeoutStr = null;
            string first = null;
            string last = null;
            string ccmIP = null;
            string intUser = null;
            string recordId = null;
            string securToken = null;

            try
            {
                if(parser.GetSingleParam("t") == "Simulation")
                {
                    int bhca = int.Parse(parser["bhca"][0]);
                    int time = int.Parse(parser["time"][0]);
                    int loginDuration = int.Parse(parser["loginduration"][0]);
                    string baseMac = parser["startmac"][0];
                    if(baseMac.StartsWith("SEP"))
                    {
                        baseMac = baseMac.Substring(3);
                    }

                    long startMac = long.Parse(baseMac, System.Globalization.NumberStyles.HexNumber); 
                    int count = int.Parse(parser["count"][0]);
                    string reserveUrl = parser["reserveurl"][0];
                    string releaseUrl = parser["releaseurl"][0];

                    BHCC bhccTest = new BHCC();
                    bhccTest.Start(bhca, time, loginDuration, startMac, count, reserveUrl, releaseUrl);
                    return;
                }        
                if(parser.GetSingleParam("t") == "TririgaUserManagement")
                {
                    UserTest test = new UserTest();
                    test.FunctionalTest();
                    test.StressTest();
                    return; 
                }

                if(parser.GetSingleParam("t") == "SerializeSaveBoRequest")
                {
                    WriteSaveBoRequest();
                    return;
                }

                if(parser.GetSingleParam("t") == "SerializeSaveBoResponse")
                {
                    WriteSaveBoResponse();
                    return;
                }

                if(parser.GetSingleParam("t") == "SignOn")
                {
                    string tririgaIP = parser.GetSingleParam("ih");
                    string tririgaUser = parser.GetSingleParam("iu");
                    string tririgaPass = parser.GetSingleParam("ip");

                    SignOnOut(tririgaIP, tririgaUser, tririgaPass);

                    return;
                }

                isConfirm = parser.GetSingleParam("t") == "Display";
                
                if(isConfirm)
                {
                    if(parser.IsParamPresent("du"))
                    {
                        string[] items = parser["du"];
                        ArrayList list = new ArrayList();
                        foreach(string duCombo in items)
                        {
                            list.Add(duCombo.Split(new char[] {':'}));
                        }
                        object[] all = new object[list.Count];
                        list.CopyTo(all);

                        url = parser.GetSingleParam("url");

                        if(parser.IsParamPresent("ccmip"))
                        {
                            ccmIP = parser.GetSingleParam("ccmip");
                        }
                        int poll = int.Parse(parser.GetSingleParam("poll"));

                        DisplayConfirm(url, poll, all);
                    }
                }    
                else
                {
                    isReserve = parser.GetSingleParam("t") == "Reserve";

                    if(parser.IsParamPresent("d"))
                    {
                        deviceName = parser.GetSingleParam("d");
                    }
                    if(parser.IsParamPresent("p"))
                    {
                        deviceProfile = parser.GetSingleParam("p");
                    }

                    if(parser.IsParamPresent("o"))
                    {
                        timeoutStr = parser.GetSingleParam("o");
                    }
                    url = parser.GetSingleParam("url");
                    if(parser.IsParamPresent("u"))
                    {
                        ccmuser = parser.GetSingleParam("u");
                    }

                    if(parser.IsParamPresent("f"))
                    {
                        first = parser.GetSingleParam("f");
                    }

                    if(parser.IsParamPresent("l"))
                    {
                        last = parser.GetSingleParam("l");
                    }

                    if(parser.IsParamPresent("ri"))
                    {
                        recordId = parser.GetSingleParam("ri");
                    }
                    if(parser.IsParamPresent("st"))
                    {
                        securToken = parser.GetSingleParam("st");
                    }
                    if(parser.IsParamPresent("iu"))
                    {
                        intUser = parser.GetSingleParam("iu");
                    }
                    if(parser.IsParamPresent("ccmip"))
                    {
                        ccmIP = parser.GetSingleParam("ccmip");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to parse inputs.");
                Console.WriteLine(e);
                Console.WriteLine(Usage);
                return;
            }

            if(isReserve)
            {
                Reserve(url, deviceName, deviceProfile, ccmuser, timeoutStr, first, last,
                    intUser, securToken,recordId, ccmIP);
            }
            else if(!isConfirm)
            {
                Release(url, deviceName, ccmuser, intUser, securToken, recordId, ccmIP);
            }
		}

        private static void Reserve(string url, string deviceName, string deviceProfile, string ccmUser, 
            string timeout, string first, string last, string intUser, string securToken, string recordId, string ccmIP)
        {
            ReserveRequestType request = new ReserveRequestType();
            request.DeviceProfile = deviceProfile;
            request.DeviceName = deviceName;
            request.CcmUser = ccmUser;
            request.Timeout = timeout;
            request.First = first;
            request.Last = last;
            request.IntegrationUser = intUser;
            request.SecurityToken = securToken;
            request.RecordId = recordId;
            request.CcmIP = ccmIP;

            object responseObj;
            UrlStatus status = Web.XmlTransaction(url, request, typeof(ReserveResponseType), out responseObj);

            ReserveResponseType response = (ReserveResponseType) responseObj;
            if(status == UrlStatus.Success)
            {
                Console.WriteLine("Received Response!");
                XmlSerializer serializer = new XmlSerializer(typeof(ReserveResponseType));
                StringWriter writer = new System.IO.StringWriter();
                serializer.Serialize(writer, response);
                Console.WriteLine("Contents:\n" + writer.ToString());
            }
            else
            {
                Console.WriteLine("Error!");
            }
        }

        private static void Release(string url, string deviceName, string ccmUser, string intUser, 
            string securToken, string recordId, string ccmIP)
        {
            ReleaseRequestType request = new ReleaseRequestType();
            request.DeviceName = deviceName;
            request.CcmIP = ccmIP;
            request.CcmUser = ccmUser;
            request.IntegrationUser = intUser;
            request.SecurityToken = securToken;
            request.RecordId = recordId;

            object responseObj;
            UrlStatus status = Web.XmlTransaction(url, request, typeof(ReleaseResponseType), out responseObj);

            ReleaseResponseType response = (ReleaseResponseType) responseObj;
            if(status == UrlStatus.Success)
            {
                Console.WriteLine("Received Response!");
                XmlSerializer serializer = new XmlSerializer(typeof(ReleaseResponseType));
                StringWriter writer = new System.IO.StringWriter();
                serializer.Serialize(writer, response);
                Console.WriteLine("Contents:\n" + writer.ToString());
            }
            else
            {
                Console.WriteLine("Error!");
            }
        }

        private static void DisplayConfirm(string url, int poll, object[] deviceUsers)
        {
            DisplayRequestType request = new DisplayRequestType();
            request.pollMinutes = poll;

            if(deviceUsers != null && deviceUsers.Length > 0)
            {
                ArrayList list = new ArrayList();

                foreach(string[] deviceUser in deviceUsers)
                {
                    string device = deviceUser[0];
                    string user = deviceUser[1];
                    string first = deviceUser[2];
                    string last = deviceUser[3];
                    string recordId = deviceUser[4];

                    DisplayItem item = new DisplayItem();
                    item.Value = device;
                    item.User = user;
                    item.First = first;
                    item.Last = last;
                    item.RecordId = recordId;
                    list.Add(item);
                }

                DisplayItem[] items = new DisplayItem[list.Count];
                list.CopyTo(items);
                request.items = items;

                string response;

                UrlStatus status = Web.XmlSerialize(url, request, out response);

                if(status == UrlStatus.Success)
                {
                    Console.WriteLine("Received Response!");
                    Console.WriteLine("Contents:\n" + response);
                }
                else
                {
                    Console.WriteLine("Error!");
                }
            }
        }

/*"<SignOnAction><Result>Success</Result><User><Id>2579463</Id><CompanyId>208133</CompanyId><SecurityToken>1147879729675</SecurityToken><FirstName>Seth</FirstName><LastName>Call</LastName></User></SignOnAction>"*/


        private static void SignOnOut(string tririgaIP, string username, string password)
        {
            XmlSerializer onSeri = new XmlSerializer(typeof(SignOnResponse));
            XmlSerializer outSeri = new XmlSerializer(typeof(SignOffResponse));

            SignOnInterfaceService service = new SignOnInterfaceService();
            service.SetUrl(tririgaIP, "80", null, false);

            string responseXml = service.signOn(username, password);
            Console.WriteLine(responseXml);

            StringReader reader = new StringReader(responseXml);
            SignOnResponse onResponse = onSeri.Deserialize(reader) as SignOnResponse;

            responseXml = service.signOut(onResponse.User.SecurityToken, Convert.ToInt64(onResponse.User.Id));
            Console.WriteLine(responseXml);
            reader = new StringReader(responseXml);
            SignOffResponse outResponse = outSeri.Deserialize(reader) as SignOffResponse;

            Console.WriteLine(outResponse.Result); 
        }

        private static void WriteSaveBoRequest()
        {
            XmlDocument doc = new XmlDocument();

            XmlSerializer saveRequest = new XmlSerializer(typeof(SaveBoRequest));
            SaveBoRequest request = new SaveBoRequest();
            request.BoRecord = new BoRecordRequest[1];
            request.BoRecord[0] = new BoRecordRequest();
            request.BoRecord[0].ActionName = String.Empty;
            request.BoRecord[0].BoId = String.Empty;
            request.BoRecord[0].RecordId = String.Empty;
            request.BoRecord[0].BoName = String.Empty;
            request.BoRecord[0].ModuleId = String.Empty;
            request.BoRecord[0].CompanyId = "208133";
            request.BoRecord[0].ParentName = String.Empty;
            request.BoRecord[0].ProjectId = String.Empty;
            request.BoRecord[0].GUIId = String.Empty;
            request.BoRecord[0].RecordInformation = new RecordInformation();
            request.BoRecord[0].RecordInformation.eyResultCodeTX = doc.CreateCDataSection("2");
            request.BoRecord[0].RecordInformation.eyResultMessageTX = doc.CreateCDataSection("No Cisco CallManager Username specified");
            request.BoRecord[0].RecordInformation.eyDiagnosticCodeTX = doc.CreateCDataSection("0");
            request.BoRecord[0].RecordInformation.eyDiagnosticMessageTX = doc.CreateCDataSection("Unknown Error");
            request.BoRecord[0].RecordInformation.eyLoggedInDeviceTX = doc.CreateCDataSection("SEP000011112222");
            request.BoRecord[0].RecordInformation.eyLoggedInUserTX = doc.CreateCDataSection("User123");
            using(FileStream stream = File.Open("tempRequest.xml", FileMode.Create))
            {
                saveRequest.Serialize(stream, request);
            }

        }

        private static void WriteSaveBoResponse()
        {
            XmlDocument doc = new XmlDocument();
            XmlSerializer saveResponse = new XmlSerializer(typeof(SaveBoResponse));
            SaveBoResponse response = new SaveBoResponse();
            response.BoRecord = new BoRecordResponse[1];
            response.BoRecord[0] = new BoRecordResponse();
            response.BoRecord[0].RecordId = "2582409";
            response.BoRecord[0].Message = doc.CreateCDataSection("asonuhaoeuatnouh sanohunsatoh santouh sa");
            
            using(FileStream stream = File.Open("tempResponse.xml", FileMode.Create))
            {
                saveResponse.Serialize(stream, response);

            }

            using(FileStream stream = File.Open("tempResponse.xml", FileMode.Open))
            {
                response = saveResponse.Deserialize(stream) as SaveBoResponse;
            }

            Console.WriteLine(response.BoRecord[0].Message.InnerText);
            Console.Read();
        }
	}
}

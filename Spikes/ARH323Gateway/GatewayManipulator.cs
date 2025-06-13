using System;
using System.Data;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities; // For DB access tool

namespace ARH323Gateway
{
	/// <summary> Handles the manipulation of H.323 gateways, and common constructs used for gateways </summary>
	public class GatewayManipulator
	{
        public enum RangeType
        {
            CallerReceiver,
            Caller,
            Receiver
        }

        public class DeviceRange
        {
            /// <summary> How many devices in this range </summary>
            public int deviceCount;

            /// <summary> Device Start SEP(hex converted to decimal...) </summary>
            public long deviceStart;

            public RangeType type;

            public DeviceRange(long deviceStart, int deviceCount, RangeType type)
            {
                this.deviceStart = deviceStart;
                this.deviceCount = deviceCount;
                this.type = type;
            }
        }

        public class Device
        {
            public string deviceName;
            public string deviceUuid;
            public string lineUuid;
            public string lineNumber;

            public Device(string deviceName, string deviceUuid, string lineUuid, string lineNumber)
            {
                this.deviceName = deviceName;
                this.deviceUuid = deviceUuid;
                this.lineUuid = lineUuid; 
                this.lineNumber = lineNumber;
            }
        }

        public class DeviceMetadata
        {
            public ArrayList pbxDevices;
            public ArrayList callingDevices;
            public ArrayList receiverDevices;

            public DeviceMetadata()
            {
                pbxDevices = new ArrayList();
                callingDevices = new ArrayList();
                receiverDevices = new ArrayList();
            }
        }
        
        private AXLAPIService service;
        private int writeWait;
        private DeviceMetadata deviceData;
		public GatewayManipulator(AXLAPIService service, int maxAxlWrite)
		{
            this.service = service;
            this.writeWait = 1000 * 60 / maxAxlWrite + 200; // Amount of time to wait inbetween writes to AXL
		}

        public bool CreateOraclePhase1(string routeListName, string routeGroupName, addH323Gateway[] gateways, addRoutePattern routeListPattern, DeviceRange[] devices, string translationPartition, string translationCss, string prefixOutDigits)
        {
            if(CreateCallRouteList(routeListName, routeGroupName, gateways, routeListPattern))
            {
                // Pull out all devices, store the information in metadata, validate before moving on
                if(ValidateDeviceRanges(devices))
                {
                    if(ReadDeviceRanges(devices))
                    {
                        
                        //CreateSimclientCSV();

                        // Metadata is complete, create translation patterns
                        if(CreateTranslationPatterns(translationPartition, translationCss, prefixOutDigits))
                        {
                            // With transpatterns in place, all CCM task are done
                            // Create MCE users on all MCEs
                            if(CreateAppSuiteAdminUsers(gateways))
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Unable to create all users.  Aborting...");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unable to create all tranlation patterns.  Aborting...");
                            return false;
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Unable to read in all devices.  Aborting...");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Device ranges didn't validate.  Aborting...");
                    return false;
                }
                
            }
            else
            {
                Console.WriteLine("Failure encountered in the CreateCallRouteList method.  Aborting...");
                return false;
            }
        }

        protected bool CreateTranslationPatterns(string translationPartition, string translationCss, string prefixOut)
        {
            // Convert receiver number to pbx number 
            for(int i = 0; i < deviceData.receiverDevices.Count; i++)
            {
                Device receiver = deviceData.receiverDevices[i] as Device;
                Device pbxExtension = deviceData.pbxDevices[i] as Device;
                string dialedNumber = receiver.lineNumber;
                string convertTo = pbxExtension.lineNumber;

                addTransPattern newTransPattern = new addTransPattern();
                newTransPattern.newPattern = new XNPTranslationPattern();
                newTransPattern.newPattern.CallingSearchSpace = translationCss;
                newTransPattern.newPattern.description = "Oracle Phase 1";
                newTransPattern.newPattern.pattern = dialedNumber;
                newTransPattern.newPattern.prefixDigitsOut = prefixOut;
                newTransPattern.newPattern.usage = XPatternUsage.Route;
                newTransPattern.newPattern.calledPartyTransformationMask = convertTo;

                try
                {
                    addTransPatternResponse newPatternResponse = service.addTransPattern(newTransPattern);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to create translation pattern pattern:{0} convert:{1}.  {2}", dialedNumber, convertTo, e);
                    return false;
                }

                WriteWait();
            }

            return true;
        }

        protected int GetLastAutoId(IDbConnection connection)
        {
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT LAST_INSERT_ID()";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        protected bool CreateAppSuiteAdminUsers(addH323Gateway[] gateways)
        {
            foreach(addH323Gateway gateway in gateways)
            {
                IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", gateway.gateway.name, 3306, "root", "metreos", true));

                try
                {
                    connection.Open();
                }
                catch
                {
                    Console.WriteLine("Unable to open a connection to {0}.  (Check that root/metreos can connect from outside of localhost, which is not a default setting)", gateway.gateway.name);
                    return false;
                }
                // Start making users
                
                int numUsers = deviceData.receiverDevices.Count;

                for(int i = 0; i < numUsers; i++)
                {
                    string iString = i.ToString();

                    SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, "as_users");
                    builder.AddFieldValue("username", iString);
                    builder.AddFieldValue("password", Security.EncryptPassword(iString));
                    builder.AddFieldValue("account_code", i);
                    builder.AddFieldValue("pin", i);
                    builder.AddFieldValue("first_name", iString);
                    builder.AddFieldValue("last_name", iString);
                    builder.AddFieldValue("email", iString);
                    builder.AddFieldValue("status", 1);
                    builder.AddFieldValue("created", new SqlBuilder.PreformattedValue("NOW()"));
                    builder.AddFieldValue("lockout_threshold", 3);
                    builder.AddFieldValue("lockout_duration", "0000-00-00 00:00:00");

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = builder.ToString();
                        command.ExecuteNonQuery();
                    }

                    int userId = GetLastAutoId(connection);

                    // Populate device for user, adding one line as well

                    SqlBuilder addDeviceSql = new SqlBuilder(SqlBuilder.Method.INSERT, "as_phone_devices");
                    addDeviceSql.AddFieldValue("as_users_id", userId);
                    addDeviceSql.AddFieldValue("is_primary_device", 1);
                    addDeviceSql.AddFieldValue("name", "Office Phone");
                    addDeviceSql.AddFieldValue("mac_address", "BOGUS" + i);

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = addDeviceSql.ToString();
                        command.ExecuteNonQuery();
                    }

                    int deviceId = GetLastAutoId(connection);

                    // Add line to device

                    SqlBuilder addLineSql = new SqlBuilder(SqlBuilder.Method.INSERT, "as_directory_numbers");
                    addLineSql.AddFieldValue("as_phone_devices_id", deviceId);
                    addLineSql.AddFieldValue("directory_number", deviceData.pbxDevices[i] as string);
                    addLineSql.AddFieldValue("is_primary_number", 1);

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = addLineSql.ToString();
                        command.ExecuteNonQuery();
                    }

                    // Add Extrenal number to user profile.

                    SqlBuilder externalNumberSql = new SqlBuilder(SqlBuilder.Method.INSERT, "as_external_numbers");
                    externalNumberSql.AddFieldValue("as_users_id", userId);
                    externalNumberSql.AddFieldValue("name", "Personal");
                    externalNumberSql.AddFieldValue("phone_number", deviceData.receiverDevices[i] as string);
                    externalNumberSql.AddFieldValue("ar_enabled", 1);
                    
                }
            }

            return true;
        }

        protected bool CreateCallRouteList(string routeListName, string routeGroupName, addH323Gateway[] gateways, addRoutePattern routeListPattern)
        {
            // list of ccm 'uuids' for accessing gateways in later commands
            ArrayList gatewayIds = new ArrayList();

            bool success = true;
            foreach(addH323Gateway h323Gateway in gateways)
            {
                // Check for gateway existence.  If it doesn't exist, create it.
                getH323Gateway gatewayCheck = new getH323Gateway();
                gatewayCheck.Item = h323Gateway.gateway.name;
                gatewayCheck.ItemElementName = ItemChoiceType16.name;

                bool exists = false;
                try
                {
                    getH323GatewayResponse response = service.getH323Gateway(gatewayCheck);
                    exists = true;
                    gatewayIds.Add(response.@return.gateway.uuid);
                }
                catch
                {
                    // Assumed to mean gateway doesn't exist
                }

                if(!exists)
                {
                    // Gateway doesn't exist, so we create it while holding onto the uuid
                    try
                    {
                        addH323GatewayResponse addResponse = service.addH323Gateway(h323Gateway);
                        gatewayIds.Add(addResponse.@return);
                        WriteWait();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Unable to create H.323 Gateway {0}.  {1}", h323Gateway.gateway.name, e);
                        success = false;
                    }
                }

                // Check for create failure
                if(!success)
                {
                   return false;
                }
            }

            // because gateways can't be added to a route group if they belong to a route pattern,
            // we will first iterate though route patterns and delete them if they exists
            // OK... no I won't because there is no way to see which gateway the route pattern points to!!!
            //            listRoutePlanByType listRoutePatterns = new listRoutePlanByType();
            //            listRoutePatterns.usage = XPatternUsage.Route;
            //            listRoutePlanByTypeResponse allRoutePatterns = service.listRoutePlanByType(listRoutePatterns);
            //            foreach(ListRoutePlanByTypeResRoutePlan routePattern in allRoutePatterns.@return)
            //            {
            //                routePattern.
            //            }

            // Check if route group has already been created
    
            getRouteGroup checkRouteGroup = new getRouteGroup();
            checkRouteGroup.ItemElementName = ItemChoiceType16.name;
            checkRouteGroup.Item = routeGroupName;

            bool found = false;
            string routeGroupUuid = null;
            try
            {
                getRouteGroupResponse foundRouteGroup = service.getRouteGroup(checkRouteGroup);
                routeGroupUuid = foundRouteGroup.@return.routeGroup.uuid;
                found = true;
            }
            catch
            {
            }

            if(!found)
            {
                // gateways are created, now create a route group with circular pattern
                addRouteGroup addRouteGroup = new addRouteGroup();
                addRouteGroup.routeGroup = new XRouteGroup();
                addRouteGroup.routeGroup.name = routeGroupName;
            
                XRouteGroupMember[] members = new XRouteGroupMember[gatewayIds.Count];
                for(int i = 0; i < gatewayIds.Count; i++)
                {
                    members[i] = new XRouteGroupMember();
                    members[i].deviceSelectionOrder = (i + 1).ToString(); // 1 based
                    members[i].Item = gatewayIds[i];
                    members[i].ItemElementName = ItemChoiceType26.deviceId;
                }
                addRouteGroup.routeGroup.members = members;
            
                try
                {
                    addRouteGroupResponse newRouteGroup = service.addRouteGroup(addRouteGroup);
                    routeGroupUuid = newRouteGroup.@return;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to add a route group {0}.  {1}", routeGroupName, e);
                    return false; // bail out
                }
            }
            
            string routeListUuid;
            found = false;

            // Check if route list already exists
            getRouteList checkRouteList = new getRouteList();
            checkRouteList.ItemElementName = ItemChoiceType16.name;
            checkRouteList.Item = routeListName;
            try
            {
                getRouteListResponse foundRouteList = service.getRouteList(checkRouteList);
                routeListUuid = foundRouteList.@return.routeList.uuid;
                found = true;
            }
            catch
            {
            }

            if(!found)
            {
                // Add route list, comprised of just route group
                addRouteList addRouteList = new addRouteList();
                addRouteList.routeList = new XRouteList();
                addRouteList.routeList.name = routeListName;
                addRouteList.routeList.routeListEnabled = true;
                addRouteList.routeList.routeListEnabledSpecified = true;
                XRouteListMember[] rlMembers = new XRouteListMember[1];
                rlMembers[0] = new XRouteListMember();
                rlMembers[0].uuid = routeGroupUuid;
            
                addRouteList.routeList.members = rlMembers;

                try
                {
                    addRouteListResponse newRouteList = service.addRouteList(addRouteList);
                    routeListUuid = newRouteList.@return;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to add route list {0}.  {1}", routeListName, e);
                    return false;
                }
            }

            found = false;

            // Check for route pattern ... // AXL-SOAP 4.1.3 proxy class needs work for this
            
//            getRoutePattern checkRoutePattern = new getRoutePattern();
//            checkRoutePattern.Item = routeListPattern.newPattern.pattern;
//            checkRoutePattern.ItemElementName = ItemChoiceType88.
            // Finally, add route pattern to account for range of numbers
            try
            {
                addRoutePatternResponse newRoutePattern = service.addRoutePattern(routeListPattern);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to create route pattern.  {0}", e);
                return false;
            }

            // Call Route List is now in place.  

            return true;
        }

        protected bool ValidateDeviceRanges(DeviceRange[] ranges)
        {
            int totalCallers = 0;
            int totalReceivers = 0;
            int totalPbxExtensions = 0;

            foreach(DeviceRange range in ranges)
            {
                if(range.type == RangeType.CallerReceiver)
                {
                    totalCallers += range.deviceCount / 2;
                    totalReceivers += range.deviceCount / 2;
                }
                else if(range.type == RangeType.Receiver)
                {
                    totalPbxExtensions += range.deviceCount;
                }
            }

            return (totalCallers == totalReceivers) && (totalCallers == totalPbxExtensions);
        }

        protected bool ReadDeviceRanges(DeviceRange[] ranges)
        {
            // Initialize deviceData
            deviceData = new DeviceMetadata();

            foreach(DeviceRange range in ranges)
            {
                for(int i = 0; i < range.deviceCount; i++)
                {
                    string deviceMac = ConvertMacToSep(range.deviceStart);

                    Device device;
                    if(!GetDeviceInfo(deviceMac, out device))
                    {
                        Console.WriteLine("Unable to retrieve device info.  Aborting...");
                        return false;
                    }

                    if(range.type == RangeType.CallerReceiver)
                    {
                        // Determine type of this device based on SimClient behavior
                        bool isCaller = i % 2 == 0;
                        
                        if(isCaller)
                        {
                            deviceData.callingDevices.Add(device);
                        }
                        else
                        {
                            deviceData.receiverDevices.Add(device);
                        }
                    }
                    else if(range.type == RangeType.Receiver)
                    {
                        deviceData.pbxDevices.Add(device);
                    }
                }
            }

            return true;
        }

        protected bool GetDeviceInfo(string deviceName, out Device device)
        {
            device = null;
            bool found = false;
            getPhone request = new getPhone();
            request.Item = deviceName;
            request.ItemElementName = ItemChoiceType5.phoneName;

            string lineUuid = null;
            string lineNumber = null;
            string deviceUuid = null;

            try
            {
                getPhoneResponse response = service.getPhone(request);

                deviceUuid = response.@return.device.uuid;

                // Determine if there is at least one line on this phone
                if( response.@return.device.lines != null &&
                    response.@return.device.lines.Items != null && 
                    response.@return.device.lines.Items.Length > 0)
                {
                    // If there is one line, get its uuid

                    XLine primaryLine = (XLine) response.@return.device.lines.Items[0];
                    lineUuid = primaryLine.Item.uuid; 

                    ReadWait();

                    getLine lookupLine = new getLine();
                    lookupLine.uuid = lineUuid;
                    getLineResponse getLineResponse = service.getLine(lookupLine);

                    lineNumber = getLineResponse.@return.directoryNumber.pattern;
                    found = true;
                }
                else
                {
                    Console.WriteLine("Unable to retrieve line info: {0}.", deviceName);
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to retrieve device or line info: {0}.  {1}", deviceName, e);
                return false;
            }

            if(found)
            {
                device = new Device(deviceName, deviceUuid, lineUuid, lineNumber); 
            }

            return found;
        }

        protected string ConvertMacToSep(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        protected void WriteWait()
        {
            System.Threading.Thread.Sleep(writeWait);
        }

        protected void ReadWait()
        {
            System.Threading.Thread.Sleep(100);
        }
	}
}

using System;
using System.Threading;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;
using System.Data;


// todo: determine no device found AXL response
namespace AROneExtern
{
	/// <summary>  </summary>
	public class Worker
	{
        private bool acceptAll;
        private AXLAPIService service;
        int writeWait;

        public Worker(string ccmIp, string ccmUser, string ccmPass, int axlWrite)
		{
            service = new AXLAPIService(ccmIp, ccmUser, ccmPass);
            acceptAll = false;
            this.writeWait = 60 * 1000 / axlWrite + 300;
        }
        
        public bool Generate()
        {
            acceptAll = false;

            // First connect to the Metreos Application Suite database, and grab all user accounts and their accompanying device profile
            using(IDbConnection connection = 
                      Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true)))
            {

                try
                {
                    connection.Open();
                }
                catch
                {
                    Console.WriteLine("Unable to open a connection to the Metreos database.\nThis tool must be run on the Metreos Communications Environment server.");
                    return false;
                }

                ArrayList users = GetUsers(connection);

                if(users.Count == 0)
                {
                    // No users were found!  Abort
                    Console.WriteLine("No users were found in the Application Suite database\nQuitting");
                    return true;
                }


                using(IDbConnection mceConnection = 
                          Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("mce", "127.0.0.1", 3306, "root", "metreos", true)))
                {

                    // Check for ActiveRelay presence, and retrieve the trigger parameters id
                    int triggerParamId = GetTriggerParametersId(mceConnection);
                    if(triggerParamId == 0)
                    {
                        return false;
                    }

                    // Prompt the user for the device pool they want these devices added
                    // Find existing device pools first
                    Hashtable devicePools = GetDevicePools(mceConnection);
                    DevicePool pool = PromptForDevicePool(devicePools, mceConnection);

                    if(pool == null)
                    {
                        Console.WriteLine("There are no SCCP device pools defined\nCreate a CallManager Telephony Server and SCCP device pool");
                        return false;
                    }

//                    listPhoneByName getAllPhones = new listPhoneByName();
//                    getAllPhones.searchString = "SEP%";
//                    listPhoneByNameResponse response = service.listPhoneByName(getAllPhones);

                    foreach(User user in users)
                    {
                        Console.WriteLine();
                        Console.WriteLine(">>>> User {0} ({1} {2}) found", user.username, user.first, user.last);

                        // See if the user has a device.
                        if(user.deviceId == 0 || user.deviceName == String.Empty || user.deviceName == null)
                        {
                            Console.WriteLine("This user does not have a device associated with their account profile\nSkipping user", user.username, user.first, user.last);
                            continue;
                        }

                        // See if the SCCP device name as configured on the phone actually exists
                        Console.WriteLine("Checking CallManager for device {0}", user.deviceName);
                        getPhone checkRealPhone = new getPhone();
                        checkRealPhone.ItemElementName = ItemChoiceType5.phoneName;
                        checkRealPhone.Item = user.deviceName;

                        getPhoneResponse checkRealPhoneRes = null;

                        try
                        {
                            Thread.Sleep(75);
                            checkRealPhoneRes = service.getPhone(checkRealPhone);
                        }
                        catch(Exception e)
                        {
                            string details = GetDetail(e);
                            // check if device not found--or something worse
                            if(details.IndexOf("") > -1)
                            {
                                Console.WriteLine("Device {0} not found in CallManager\nSkipping user", user.deviceName);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Unable to communicate with CallManager using AXL-SOAP");
                                bool abort = DisplayAbortPrompt();
                                if(abort)
                                {
                                    return false;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                        int numLines = 0;
                        if(checkRealPhoneRes != null &&
                            checkRealPhoneRes.@return != null &&
                            checkRealPhoneRes.@return.device != null &&
                            checkRealPhoneRes.@return.device.lines != null &&
                            checkRealPhoneRes.@return.device.lines.Items != null)
                        {
                            numLines = checkRealPhoneRes.@return.device.lines.Items.Length;
                        }

                        XLine foundLine1 = null;

                        if(numLines == 0)
                        {
                            // The device in CallManager has no lines, so a shared-line configuration is not possible
                            Console.WriteLine("The device {0} has no lines\nSkipping user", user.deviceName);
                            continue;
                        }
                        else
                        {
                            foreach(XLine line in checkRealPhoneRes.@return.device.lines.Items)
                            {
                                if(line.index == "0")
                                {
                                    foundLine1 = line;
                                    break;
                                }
                            }

                            if(foundLine1 == null)
                            {
                                Console.WriteLine("Unable to determine the first line on device {0}\nSkipping user", user.deviceName);
                                continue;
                            }
                        }
       
                        // Line one has been found
                        // Perform getLine to get pattern
                        string pattern = null;

                        getLineResponse response = null;

                        getLine getPrimaryLine = new getLine();
                        getPrimaryLine.uuid = foundLine1.Item.uuid;

                        try
                        {
                            Thread.Sleep(75);
                            response = service.getLine(getPrimaryLine);
                            if(response != null &&
                                response.@return != null &&
                                response.@return.directoryNumber != null &&
                                response.@return.directoryNumber.pattern != null &&
                                response.@return.directoryNumber.pattern != String.Empty)
                            {
                                pattern = response.@return.directoryNumber.pattern;
                            }
                        }
                        catch
                        {
                            // We have the UUID--unless some one JUST deleted the line, this is very odd
                            Console.WriteLine("Unable to query the line on device {0}\nSkipping user", user.deviceName);
                            continue;
                        }

                        if(pattern == null)
                        {
                            Console.WriteLine("The line number on device {0} is not queryable\nSkipping user", user.deviceName);
                            continue;
                        }

                        Console.WriteLine("Found primary line {0} on device {1}", pattern, user.deviceName);
                    
                        // Ask permission to create virtual SCCP phone and shared-line
                        if(!acceptAll)
                        {
                            AxlChoice choice = PromptCreateSccpPhone(pool, user, pattern, user.deviceName);

                            if(choice == AxlChoice.Abort)
                            {
                                return false;
                            }

                            if(choice == AxlChoice.All)
                            {
                                acceptAll = true;
                            }

                            if(choice == AxlChoice.No)
                            {
                                continue;
                            }
                        }

                        // Yes or All chosen                                       
                        XPhoneLines lines = new XPhoneLines();
                        lines.Items = new XLine[1];
                        lines.Items[0] = foundLine1;

                        addPhone phone = new addPhone();
                        XIPPhone xipphone = new XIPPhone();
                        xipphone.description = String.Format("Metreos ActiveRelay ({0} {1})", user.first, user.last);
                        xipphone.name = ConvertMACToString(pool.highestMac);
                        xipphone.lines = lines;
                        xipphone.@class = XClass.Phone;
                        xipphone.addOnModules = null;
                    
                        xipphone.protocol = XDeviceProtocol.Ciscostation; // THIS MUST BE THIS FOR 7960!!
                        xipphone.Item1 = XModel.Cisco7960;
                        xipphone.Item = XProduct.Cisco7960;
                        xipphone.Item2 = String.Empty;
                        xipphone.Item3 = "Default";
                        xipphone.Item8 = "Standard 7960";
                    
                        xipphone.protocolSide = XProtocolSide.User;
                        phone.newPhone = xipphone;

                        try
                        {
                            service.addPhone(phone);
                            Console.WriteLine("{0} added with line number {1}", ConvertMACToString(pool.highestMac), pattern);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(String.Format("Unable to add the phone '{0}'.  \n{1}", ConvertMACToString(pool.highestMac), GetDetail(e)));
                            Console.WriteLine("Skipping user");
                        }

                        // Add device pool field to Metreos database
                        SqlBuilder addDeviceEntry = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_call_manager_devices");
                        addDeviceEntry.AddFieldValue("mce_components_id", pool.id);
                        addDeviceEntry.AddFieldValue("device_name", pool.highestMac);
                        addDeviceEntry.AddFieldValue("directory_number", "0");
                        addDeviceEntry.AddFieldValue("status", 4);
                        addDeviceEntry.AddFieldValue("device_type", 1);

                        using(IDbCommand command = mceConnection.CreateCommand())
                        {
                            command.CommandText = addDeviceEntry.ToString();
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine("Added device {0} to pool {1}", pool.highestMac, pool.name);

                        // Check for data inconsistency
                        if(user.lineNumber != pattern)
                        {
                            if(user.lineId == 0)
                            {
                                SqlBuilder newLine = new SqlBuilder(SqlBuilder.Method.INSERT, "as_directory_numbers");
                                newLine.AddFieldValue("as_phone_devices_id", user.deviceId);
                                newLine.AddFieldValue("directory_number", pattern);
                                newLine.AddFieldValue("is_primary_number", 1);

                                using(IDbCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = newLine.ToString();
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                SqlBuilder updateLine = new SqlBuilder(SqlBuilder.Method.UPDATE, "as_directory_numbers");
                                updateLine.AddFieldValue("directory_number", pattern);
                                updateLine.where["as_directory_numbers_id"] = user.lineId;

                                using(IDbCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = updateLine.ToString();
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        // Add trigger param to ActiveRelay
                        AddTriggerParameter(connection, triggerParamId, pattern);

                        // Increment the device pool highest mac to next highest
                        pool.highestMac++;

                        Thread.Sleep(writeWait);

                    }
                }
            }
            return true;
        }


        private ArrayList GetUsers(IDbConnection connection)
        {
            ArrayList users = new ArrayList();

            // Query all users
            SqlBuilder allUsers = new SqlBuilder(SqlBuilder.Method.SELECT, "as_users");
            DataTable userTable = null;
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = allUsers.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    userTable = Metreos.Utilities.Database.GetDataTable(reader);
                }
            }

            if(userTable != null)
            {
                foreach(DataRow row in userTable.Rows)
                {
                    int userId = Convert.ToInt32( row["as_users_id"] );
                    string firstName = row["first_name"] as String;
                    string lastName = row["last_name"] as String;
                    string username = row["username"] as String;
                
                    int phoneDeviceId = 0;
                    string phoneDeviceName = null;
                    int lineId = 0;
                    string lineNumber = null;
                    
                    SqlBuilder findPrimaryDevice = new SqlBuilder(SqlBuilder.Method.SELECT, "as_phone_devices");
                    findPrimaryDevice.fieldNames.Add("as_phone_devices_id");
                    findPrimaryDevice.fieldNames.Add("mac_address");
                    findPrimaryDevice.where["as_users_id"] = userId;
                    findPrimaryDevice.where["is_primary_device"] = 1;

                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = findPrimaryDevice.ToString();
                        using(IDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                phoneDeviceName = reader["mac_address"] as String;
                                phoneDeviceId = Convert.ToInt32(reader["as_phone_devices_id"]);
                            }
                        }
                    }

                    if(phoneDeviceId != 0)
                    {
                        // Find primary line number for this device

                        SqlBuilder findPrimaryLine = new SqlBuilder(SqlBuilder.Method.SELECT, "as_directory_numbers");
                        findPrimaryLine.fieldNames.Add("directory_number");
                        findPrimaryLine.fieldNames.Add("as_directory_numbers_id");
                        findPrimaryLine.where["as_phone_devices_id"] = phoneDeviceId;
                        findPrimaryLine.where["is_primary_number"] = 1;
                        using(IDbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = findPrimaryLine.ToString();
                            using(IDataReader reader = command.ExecuteReader())
                            {
                                while(reader.Read())
                                {
                                    lineNumber = reader["directory_number"] as string;
                                    lineId = Convert.ToInt32(reader["as_directory_numbers_id"]);
                                }
                            }
                        }
                    }

                    users.Add(new User(firstName, lastName, username, phoneDeviceName, phoneDeviceId, lineNumber, lineId));
                }
            }

            return users;
        }

        private Hashtable GetDevicePools(IDbConnection connection)
        {
            Hashtable devicePools = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable();

            SqlBuilder getDevicePools = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            getDevicePools.fieldNames.Add("mce_components_id");
            getDevicePools.fieldNames.Add("name");
            getDevicePools.where["type"] = 100;

            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getDevicePools.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int componentId = Convert.ToInt32(reader["mce_components_id"]);
                        string poolName = reader["name"] as string;

                        devicePools[poolName] = componentId;
                    }
                }
            }

            return devicePools;
        }

        private AxlChoice PromptCreateSccpPhone(DevicePool pool, User user, string pattern, string userDeviceName)
        {
            while(true)
            {
                Console.WriteLine("A virtual MCE device {0} will be created with\none line shared with line {1} on device {2}", pool.highestMac, pattern, userDeviceName);
                Console.WriteLine("(y/n/always/abort)");

                string response = Console.ReadLine();

                if(String.Compare(response, "y", true) == 0 || String.Compare(response, "yes", true) == 0)
                {
                    return AxlChoice.Yes;
                }
                else if(String.Compare(response, "n", true) == 0 || String.Compare(response, "no", true) == 0)
                {
                    return AxlChoice.No;
                }
                else if(String.Compare(response, "always", true) == 0)
                {
                    return AxlChoice.All;
                }
                else if(String.Compare(response, "abort", true) == 0)
                {
                    return AxlChoice.Abort;
                }
            }
        }

        private DevicePool PromptForDevicePool(Hashtable pools, IDbConnection connection)
        {
            int poolId = 0;
            string poolName;

            if(pools.Count == 0) return null;

            while(true)
            {
                Console.WriteLine("The following device pools already exist\nChoose one by typing its name");

                foreach(string name in pools.Keys)
                {
                    Console.WriteLine("--> {0}", name);
                }

                Console.WriteLine("Choice:");
                string chosen = Console.ReadLine();

                if(pools.Contains(chosen))
                {
                    // Existing chosen
                    poolId = (int) pools[chosen];
                    poolName = chosen;
                    break;
                }
                else
                {
            
                    continue;
                }
                
            }

            long startMac = -1;
            while(true)
            {
                Console.WriteLine("Virtual SCCP Phones must have a device name\nPlease provide a start MAC address for virtual phones,\n(ex: FFFFFF000000)\nMAC:");
                string readLine = Console.ReadLine();
                if(readLine.Length != 12)
                {
                    try
                    {
                        startMac = long.Parse(readLine, System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("The MAC address must be hexidecimal");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("The MAC address must be 12 characters in length");
                    continue;
                }
            }

            return new DevicePool(poolName, poolId, DetermineHighestMac(poolId, connection, startMac));
        }

        private long DetermineHighestMac(int poolId, IDbConnection connection, long startMac)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_call_manager_devices");
            builder.fieldNames.Add("device_name");
            builder.where["mce_components_id"] = poolId;

            ArrayList existingDeviceMacs = new ArrayList();

            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = builder.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        existingDeviceMacs.Add(Convert.ToInt64(reader["device_name"]));
                    }
                }
            }

            return FindHighestMac(existingDeviceMacs, startMac) + 1;
        }

        private long FindHighestMac(ArrayList existingMacs, long startMac)
        {
            for(int i = 0; i < existingMacs.Count; i++)
            {
                long currentMac = (long) existingMacs[i];

                if(currentMac > startMac)
                {
                    startMac = currentMac;
                }
            }

            if(startMac < 0) startMac = 0;
            return startMac;
        }

        protected int GetLastAutoId(IDbConnection connection)
        {
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT LAST_INSERT_ID()";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private string ConvertMACToString(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
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

        private bool DisplayAbortPrompt()
        {
            while(true)
            {
                Console.WriteLine("Continue with next user? (y/n)");
                string response = Console.ReadLine();
                if(String.Compare(response, "y", true) == 0 || String.Compare(response, "yes", true) == 0)
                {
                    return false;
                }
                else if(String.Compare(response, "n", true) == 0 || String.Compare(response, "no", true) == 0)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
        }

        private int GetTriggerParametersId(IDbConnection connection)
        {
            int componentId = 0;

            SqlBuilder getCompId = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            getCompId.fieldNames.Add("mce_components_id");
            getCompId.where["name"] = "ActiveRelay";
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getCompId.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        componentId = Convert.ToInt32(reader["mce_components_id"]);
                    }
                }
            }

            if(componentId == 0)
            {
                Console.WriteLine("The ActiveRelay application is not installed\nQuitting");
                return 0;
            }

            int defaultPartitionId = 0;

            SqlBuilder getDefaultId = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_application_partitions");
            getDefaultId.fieldNames.Add("mce_application_partitions_id");
            getDefaultId.where["mce_components_id"] = componentId;
            getDefaultId.where["name"] = "Default";
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getDefaultId.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        defaultPartitionId = Convert.ToInt32(reader["mce_application_partitions_id"]);
                    }
                }
            }

            if(defaultPartitionId == 0)
            {
                Console.WriteLine("The default partition of ActiveRelay does not exist\nQuitting");
                return 0;
            }

            int scriptId = 0;

            SqlBuilder getScriptId = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_application_scripts");
            getScriptId.fieldNames.Add("mce_application_scripts_id");
            getScriptId.where["name"] = "IncomingCall";
            getScriptId.where["mce_components_id"] = componentId;
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getScriptId.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        scriptId = Convert.ToInt32(reader["mce_application_scripts_id"]);
                    }
                }
            }

            if(scriptId == 0)
            {
                Console.WriteLine("The IncomingCall script of ActiveRelay is not present--\nActiveRelay should be reinstalled\nQuitting");
                return 0;
            }

            int toParamId = 0;

            SqlBuilder getToParamId = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_application_script_trigger_parameters");
            getToParamId.fieldNames.Add("mce_application_script_trigger_parameters_id");
            getToParamId.where["name"] = "to";
            getToParamId.where["mce_application_scripts_id"] = scriptId;
            getToParamId.where["mce_application_partitions_id"] = defaultPartitionId;
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getToParamId.ToString();
                using(IDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        toParamId = Convert.ToInt32(reader["mce_application_scripts_id"]);
                    }
                }
            }

            if(toParamId == 0)
            {
                // Need to create the to param
                SqlBuilder createParam = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_application_script_trigger_parameters");
                createParam.AddFieldValue("name", "to");
                createParam.AddFieldValue("mce_application_scripts_id", scriptId);
                createParam.AddFieldValue("mce_application_partitions_id", defaultPartitionId);
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = createParam.ToString();
                    command.ExecuteNonQuery();
                }

                toParamId = GetLastAutoId(connection);
            }

            return toParamId;
        }

        private void AddTriggerParameter(IDbConnection connection, int triggerParamId, string pattern)
        {
            SqlBuilder insertTrigger = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_trigger_parameter_values");
            insertTrigger.AddFieldValue("mce_application_script_trigger_parameters_id", triggerParamId);
            insertTrigger.AddFieldValue("value", pattern);
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = insertTrigger.ToString();
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Added {0} to ActiveRelay trigger parameter", pattern);
        }

        private enum AxlChoice
        {
            Yes,
            No,
            All,
            Abort,
        }

        private class DevicePool
        {
            public string name;
            public int id;
            public long highestMac;

            public DevicePool(string name, int id, long highestMac)
            {
                this.name = name;
                this.id = id;
                this.highestMac = highestMac;
            }
        }

        private class User
        {
            public string first;
            public string last;
            public string username;

            public string deviceName;
            public int deviceId;

            public string lineNumber;
            public int lineId;

            public User(string first, string last, string username, string deviceName, int deviceId, string lineNumber, int lineId)
            {
                this.first = first;
                this.last = last;
                this.username = username;
                this.deviceName = deviceName;
                this.deviceId = deviceId;
                this.lineNumber = lineNumber;
                this.lineId = lineId;
            }
        }
	}
}
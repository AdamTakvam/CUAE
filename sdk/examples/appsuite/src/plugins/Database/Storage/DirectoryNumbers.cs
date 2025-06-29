using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using ReturnValues = Metreos.ApplicationSuite.Storage.Users.GetUserReturnValues;
using DeviceReturnValues = Metreos.ApplicationSuite.Storage.Devices.GetDeviceReturnValues;
using Method = Metreos.Utilities.SqlBuilder.Method;
using UserTable   = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;
using DeviceTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Devices;
using DirNumTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.DirectoryNumbers;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the directory numbers table, and any information which stem from that
    /// </summary>
    public class DirectoryNumbers : DbTable
    {
        public DirectoryNumbers(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region GetDeviceByPrimaryDN
        public bool GetDeviceByPrimaryDN(string dn, out uint deviceId)
        {
            deviceId = 0;
            if (dn == null)
                return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DirNumTable.TableName);
            builder.fieldNames.Add(DirNumTable.DeviceId);
            builder.where[DirNumTable.DirectoryNumber] = dn;
            builder.where[DirNumTable.IsPrimaryNumber] = true;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);
            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
                    
                if (table == null)
                    return false;

                if (table.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}{2}" , "Found multiple primary devices using directory number '", dn, "' as their primary number!");
                    return false;
                }
                if (table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}{2}", "Did not find any primary devices using directory number '", dn, "' as their primary number!");
                    return false;
                }

                DataRow row = table.Rows[0];
                deviceId = Convert.ToUInt32(row[DirNumTable.DeviceId]);
                return (deviceId < SqlConstants.StandardPrimaryKeySeed) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                            "Error encountered in the GetUserByPrimaryDN method, using directory number '{0}'\n" +
                            "Error message: {1}", dn, result.e.Message);
                return false;
            }
        }
        #endregion

        #region GetUserByDn

        /// <summary>
        ///     Returns a userId for a directory number, also indicating a database failure
        /// </summary>
        /// <param name="directoryNumber">
        ///     The directory number to search the database with
        /// </param>
        /// <param name="userId">
        ///     The ID of the user associated with this directory number
        /// </param>
        /// <returns>
        ///     <c>true</c> if the database operation was successful, otherwise <c>false</c>
        /// </returns>
        public ReturnValues GetUserByDn(string directoryNumber, out uint userId, out UserStatus userStatus)
        {
            ReturnValues result = ReturnValues.NoUser;

            userId = 0;
            userStatus = UserStatus.Ok;

            SqlBuilder directoryNumberLookup = new SqlBuilder(Method.SELECT, DirNumTable.TableName);
            directoryNumberLookup.fieldNames.Add(DirNumTable.DeviceId);
            directoryNumberLookup.where[DirNumTable.DirectoryNumber] = directoryNumber;

            SqlBuilder deviceLookup = new SqlBuilder(Method.SELECT, DeviceTable.TableName);
            deviceLookup.fieldNames.Add(DeviceTable.UserId);
            deviceLookup.where[DeviceTable.Id] = directoryNumberLookup;

            SqlBuilder userLookup = new SqlBuilder(Method.SELECT, UserTable.TableName);
            userLookup.fieldNames.Add(UserTable.Id);
            userLookup.fieldNames.Add(UserTable.Status);
            userLookup.where[UserTable.Id] = deviceLookup;

            AdvancedReadResultContainer readResult = ExecuteEasyQuery(userLookup);

            if(readResult.result == ReadResult.Success)
            {
                DataTable results = readResult.results;

                DataRow row = null;
                if(results != null && results.Rows.Count == 1)
                {
                    row = results.Rows[0];
                    userId = Convert.ToUInt32(row[UserTable.Id]);
                    userStatus = (UserStatus) Convert.ToInt32(row[UserTable.Status]);
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    row = results.Rows[0];
                    userId = Convert.ToUInt32(row[UserTable.Id]);
                    userStatus = (UserStatus) Convert.ToInt32(row[UserTable.Status]);
                    log.Write(TraceLevel.Error, 
                        "Duplicate users found in GetUserByDn with directory number '{0}'", 
                        directoryNumber);                    
                }

                if(userId != 0)
                {
                    result = ReturnValues.success;
                }
                else
                {
                    result = ReturnValues.NoUser;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUserByDn method with directory number of {0}\n" +
                    "Error message: {1}", directoryNumber, readResult.e.Message);

                result = ReturnValues.failure;
            }
             
            return result;
        }

        #endregion

        #region GetDeviceByDn

        /// <summary>
        ///     Returns a device name for a directory number, also indicating a database failure
        /// </summary>
        /// <param name="directoryNumber">
        ///     The directory number to search the database with
        /// </param>
        /// <param name="userId">
        ///     The device name of the device associated with this directory number
        /// </param>
        /// <returns>
        ///     <c>true</c> if the database operation was successful, otherwise <c>false</c>
        /// </returns>
        public DeviceReturnValues GetDeviceByDn(string directoryNumber, out string deviceName)
        {
            DeviceReturnValues result = DeviceReturnValues.NoDevice;

            deviceName = null;

            SqlBuilder directoryNumberLookup = new SqlBuilder(Method.SELECT, DirNumTable.TableName);
            directoryNumberLookup.fieldNames.Add(DirNumTable.DeviceId);
            directoryNumberLookup.where[DirNumTable.DirectoryNumber] = directoryNumber;

            SqlBuilder deviceLookup = new SqlBuilder(Method.SELECT, DeviceTable.TableName);
            deviceLookup.fieldNames.Add(DeviceTable.MacAddress);
            deviceLookup.where[DeviceTable.Id] = directoryNumberLookup;

            AdvancedReadResultContainer readResult = ExecuteEasyQuery(deviceLookup);

            if(readResult.result == ReadResult.Success)
            {
                DataTable results = readResult.results;

                DataRow row = null;
                if(results != null && results.Rows.Count == 1)
                {
                    row = results.Rows[0];
                    deviceName = Convert.ToString(row[DeviceTable.MacAddress]);
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    row = results.Rows[0];
                    deviceName = Convert.ToString(row[DeviceTable.MacAddress]);
                    log.Write(TraceLevel.Error, 
                        "Duplicate device names found in GetDeviceByDn with directory number '{0}'", 
                        directoryNumber);                    
                }

                if(deviceName != null && deviceName != string.Empty)
                {
                    result = DeviceReturnValues.success;
                }
                else
                {
                    result = DeviceReturnValues.NoDevice;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetDeviceByDn method with directory number of {0}\n" +
                    "Error message: {1}", directoryNumber, readResult.e.Message);
                result = DeviceReturnValues.failure;
            }

            return result;
        }

        #endregion
    }
}

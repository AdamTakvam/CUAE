using System;
using System.Threading;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;
using System.Data;
namespace AROneExtern
{
	/// <summary>  </summary>
	public class Worker
	{
        private ArrayList callerDns;
        private ArrayList receiverDns;

        public Worker()
		{
            this.callerDns = new ArrayList();
            this.receiverDns = new ArrayList();
        }
        
        public bool Generate(int deviceStart, int deviceCount, string linePrefix, string findMePrefix, int increment, string mceIp)
        {
            for(int i = deviceStart; i < deviceStart + deviceCount; i++)
            {
                if((i + deviceStart + 1) % increment == 0)
                {
                    receiverDns.Add(i.ToString());
                }
            }
            
            // With caller DNs and receiver DNs accumulated, we can make MCE accounts
            IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", mceIp, 3306, "root", "metreos", true));

            try
            {
                connection.Open();
            }
            catch
            {
                Console.WriteLine("Unable to open a connection to {0}.  (Check that root/metreos can connect from outside of localhost, which is not a default setting)", mceIp);
                return false;
            }

            // Start making users
            for(int i = 0; i < receiverDns.Count; i++)
            {
                string iString = i.ToString();

                SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, "as_users");
                builder.AddFieldValue("username", receiverDns[i] as string);
                builder.AddFieldValue("password", Security.EncryptPassword(receiverDns[i] as string));
                builder.AddFieldValue("account_code", receiverDns[i] as string);
                builder.AddFieldValue("pin", receiverDns[i] as string);
                builder.AddFieldValue("first_name", receiverDns[i] as string);
                builder.AddFieldValue("last_name", receiverDns[i] as string);
                builder.AddFieldValue("email", receiverDns[i] as string);
                builder.AddFieldValue("status", 1);
                builder.AddFieldValue("time_zone", "America/Chicago");
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
                addLineSql.AddFieldValue("directory_number", linePrefix + receiverDns[i]);
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
                externalNumberSql.AddFieldValue("phone_number", findMePrefix + receiverDns[i]);
                externalNumberSql.AddFieldValue("ar_enabled", 1);
                externalNumberSql.AddFieldValue("is_corporate", 1);
                
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = externalNumberSql.ToString();
                    command.ExecuteNonQuery();
                }
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

        private string ConvertMACToString(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }
	}
}
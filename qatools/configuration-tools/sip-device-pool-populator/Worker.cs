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
        
        public bool Generate(int baseDn , int deviceCount, string devicePool)
        {
            
            // With caller DNs and receiver DNs accumulated, we can make MCE accounts
            IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("mce", "127.0.0.1", 3306, "root", "metreos", true));

            try
            {
                connection.Open();
            }
            catch
            {   
                Console.WriteLine("Unable to open a connection to {0}.  (Check that root/metreos can connect from outside of localhost, which is not a default setting)", "127.0.0.1");
                return false;
            }

            // Get component ID for SIP

            SqlBuilder getComp = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            getComp.fieldNames.Add("mce_components_id");
            getComp.where["name"] = devicePool;

            int dpId = 0;
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = getComp.ToString();
                dpId = Convert.ToInt32(command.ExecuteScalar());
            }


            // Start making users
            for(int i = 0; i < deviceCount; i++)
            {
                // Create devices
                SqlBuilder insertOne = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_call_manager_devices");
                insertOne.AddFieldValue("mce_components_id", dpId);
                insertOne.AddFieldValue("device_name", (i + baseDn).ToString());
                insertOne.AddFieldValue("directory_number", "0");
                insertOne.AddFieldValue("status", 4);
                insertOne.AddFieldValue("device_type", 4);

                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = insertOne.ToString();
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
using System;
using System.Data;
using System.Collections;
using Metreos.Utilities;

namespace Metreos.FunctionalTests.App.ActiveRelay
{

    public class ActiveRelayUserImporter
    {
        public ActiveRelayUserImporter()
        {
        }

        public ArrayList DownloadUsers(string user, string pass, string ip)
        {
            ArrayList users = new ArrayList();
            using(IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", ip, 3306, user, pass, true, 5)))
            {
                connection.Open();

                // Query all users
                SqlBuilder allUsers = new SqlBuilder(SqlBuilder.Method.SELECT, "as_users");
                DataTable userTable = null;
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = allUsers.ToString();
                    using(IDataReader reader = command.ExecuteReader())
                    {
                        userTable = Utilities.Database.GetDataTable(reader);
                    }
                }

                if(userTable != null)
                {
                    foreach(DataRow row in userTable.Rows)
                    {
                        int userId = Convert.ToInt32( row["as_users_id"] );
                        int phoneDeviceId = 0;
                        string extension = null;
                       
                        SqlBuilder findPrimaryDevice = new SqlBuilder(SqlBuilder.Method.SELECT, "as_phone_devices");
                        findPrimaryDevice.fieldNames.Add("as_phone_devices_id");
                        findPrimaryDevice.where["as_users_id"] = userId;
                        findPrimaryDevice.where["is_primary_device"] = 1;

                        using(IDbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = findPrimaryDevice.ToString();
                            using(IDataReader reader = command.ExecuteReader())
                            {
                                while(reader.Read())
                                {
                                    phoneDeviceId = Convert.ToInt32(reader["as_phone_devices_id"]);
                                }
                            }
                        }

                        if(phoneDeviceId != 0)
                        {
                            // Find primary line number for this device

                            SqlBuilder findPrimaryLine = new SqlBuilder(SqlBuilder.Method.SELECT, "as_directory_numbers");
                            findPrimaryLine.fieldNames.Add("directory_number");
                            findPrimaryLine.where["as_phone_devices_id"] = phoneDeviceId;
                            findPrimaryLine.where["is_primary_number"] = 1;
                            using(IDbCommand command = connection.CreateCommand())
                            {
                                command.CommandText = findPrimaryLine.ToString();
                                using(IDataReader reader = command.ExecuteReader())
                                {
                                    while(reader.Read())
                                    {
                                        extension = reader["directory_number"] as string;
                                    }
                                }
                            }
                        }

                        if(extension != null)
                        {
                            // Find Find Me numbers for this account
                            ActiveRelayUser userItem = new ActiveRelayUser(extension);

                            SqlBuilder findFindMeNumbers = new SqlBuilder(SqlBuilder.Method.SELECT, "as_external_numbers");
                            findFindMeNumbers.fieldNames.Add("phone_number");
                            findFindMeNumbers.fieldNames.Add("delay_call_time");
                            findFindMeNumbers.fieldNames.Add("call_attempt_timeout");
                            findFindMeNumbers.fieldNames.Add("is_corporate");
                            findFindMeNumbers.fieldNames.Add("timeofday_enabled");
                            findFindMeNumbers.fieldNames.Add("timeofday_weekend");
                            findFindMeNumbers.fieldNames.Add("timeofday_start");
                            findFindMeNumbers.fieldNames.Add("timeofday_end");
                            findFindMeNumbers.where["as_users_id"] = userId;
                            findFindMeNumbers.where["ar_enabled"] = 1;

                            using(IDbCommand command = connection.CreateCommand())
                            {
                                command.CommandText = findFindMeNumbers.ToString();
                                using(IDataReader reader = command.ExecuteReader())
                                {
                                    while(reader.Read())
                                    {   
                                        int todStatus = Convert.ToInt32(reader["timeofday_weekend"]);

                                        FindMeNumber findMe = new FindMeNumber(
                                            reader["phone_number"] as string, 
                                            !Convert.ToBoolean(reader["is_corporate"]),
                                            Convert.ToInt32(reader["delay_call_time"]),
                                            Convert.ToInt32(reader["call_attempt_timeout"]),
                                            Convert.ToBoolean(reader["timeofday_enabled"]),
                                            1 == (todStatus | 1),
                                            2 == (todStatus | 2));
                                        userItem.findMe.Add(findMe);
                                    }
                                }
                            }

                            if(userItem.findMe.Count > 0)
                            {
                                users.Add(userItem);
                            }
                        }
                    }
                }

                return users;
            }
        }
    }

	/// <summary></summary>
	public class ActiveRelayUser
	{
        public string extension;
        public ArrayList findMe;

        public ActiveRelayUser(string extension)
		{
            this.extension = extension;
			this.findMe = new ArrayList();
		}
	}

    public class FindMeNumber
    {
        public string number;
        public bool confRequired;
        public int delayTime;
        public int giveUpTime;
        //public int startTime;
        //public int endTime;
        public bool todEnabled;
        public bool satEnabled;
        public bool sunEnabled;

        public FindMeNumber(string number, bool confRequired, int delayTime, int giveUpTime
            /*int startTime, int endTime*/, bool todEnabled, bool satEnabled, bool sunEnabled)
        {
            this.number = number;
            this.confRequired = confRequired;
            this.delayTime = delayTime;
            this.giveUpTime = giveUpTime;
            //this.startTime = startTime;
            //this.endTime = endTime;
            this.todEnabled = todEnabled;
            this.satEnabled = satEnabled;
            this.sunEnabled = sunEnabled;
        }
    }
}

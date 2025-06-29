using System;
using System.IO;
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
        private ArrayList errors;
		private ArrayList results;
		private ArrayList warnings;

        public Worker()
		{
            this.errors = new ArrayList();
			this.results = new ArrayList();
			this.warnings = new ArrayList();
        }
        
		#region Table Definitions

		/* as_users
		+-------------------------+------------------+------+-----+---------------------
		+----------------+
		| Field                   | Type             | Null | Key | Default
																		| Extra          |
		+-------------------------+------------------+------+-----+---------------------
		+----------------+
		| as_users_id             | int(10) unsigned |      | PRI | NULL
																		| auto_increment |
		| as_ldap_servers_id      | int(11) unsigned | YES  | MUL | NULL
																		|                |
		| username                | varchar(255)     |      |     |
		|                |
		| account_code            | int(11)          |      |     | 0
		|                |
		| pin                     | int(11)          |      |     | 0
		|                |
		| password                | varchar(255)     |      |     |
		|                |
		| first_name              | varchar(64)      |      |     |
		|                |
		| last_name               | varchar(64)      |      |     |
		|                |
		| email                   | varchar(255)     | YES  |     | NULL
																		|                |
		| status                  | int(11)          |      |     | 0
		|                |
		| created                 | timestamp        | YES  |     | CURRENT_TIMESTAMP
																		|                |
		| last_used               | timestamp        | YES  |     | 0000-00-00 00:00:00
		|                |
		| lockout_threshold       | int(11)          |      |     | 0
		|                |
		| lockout_duration        | time             |      |     | 00:00:00
		|                |
		| last_lockout            | timestamp        | YES  |     | 0000-00-00 00:00:00
		|                |
		| failed_logins           | int(11)          |      |     | 0
		|                |
		| current_active_sessions | int(11)          |      |     | 0
		|                |
		| max_concurrent_sessions | int(11)          |      |     | 0
		|                |
		| pin_change_required     | tinyint(1)       |      |     | 0
		|                |
		| external_auth_enabled   | tinyint(1)       |      |     | 0
		|                |
		| record                  | tinyint(1)       |      |     | 0
		|                |
		| recording_visible       | tinyint(1)       |      |     | 0
		|                |
		| external_auth_dn        | text             | YES  |     | NULL
																		|                |
		| ldap_synched            | tinyint(1)       |      |     | 0
		|                |
		| time_zone               | varchar(64)      |      |     |
		|                |
		| ar_transfer_number      | varchar(64)      |      |     |
		|                |
		| placed_calls            | int(11)          |      |     | 0
		|                |
		| successfull_calls       | int(11)          |      |     | 0
		|                |
		| total_call_time         | int(11)          |      |     | 0
		|                |
		+-------------------------+------------------+------+-----+---------------------
		+----------------+*/


		#endregion
        public bool Generate(	ArrayList users,
								string mysqlUser,
								string mysqlPass,
                                string outFile)
        {
			IDbTransaction transaction = null;
			IDbConnection masterConnection = null;

			bool success = true;
			try
			{
				masterConnection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, mysqlUser, mysqlPass, true));
				
				masterConnection.Open();

				transaction = masterConnection.BeginTransaction();
				IDbConnection connection = transaction.Connection;

				foreach(string[] userBits in users)
				{
					string firstname			= userBits[0];
					string lastname				= userBits[1];
					string email				= userBits[2];
					string xpid					= userBits[3];
					string password				= userBits[4];
					string accountCode			= userBits[5];
					string pin					= userBits[6];
					string phoneDisplayName		= userBits[7];
					string phoneDeviceName		= userBits[8];
					string findMeDisplayName	= userBits[9];
					string findMeNumber			= userBits[10];
					string findMeState			= userBits[11];
					string timeZone				= userBits[12];

					Console.WriteLine("\nProcessing user '{0}'", xpid);

					int userId = -1;

					// check for existing username
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = String.Format(@"
							
							SELECT as_users_id 
							FROM as_users 
							WHERE username = '{0}' 
								AND status != 8", /* 8 = deleted */
							xpid);

						using(IDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								userId = Convert.ToInt32(reader[0]);
							}
						}
					}

					// if user didn't exist, create the user
					if(userId == -1)
					{
						// create the user
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = String.Format(@"
							
								INSERT INTO as_users 
								(username, account_code, pin, password, first_name, last_name, 
								 email, status, lockout_threshold, lockout_duration, 
								 max_concurrent_sessions, time_zone)
								VALUES
								('{0}', {1}, {2}, '{3}', '{4}', '{5}',
							     '{6}', 1 /*active*/, 5, '24:00:00', 5, '{7}')",
								xpid, accountCode, pin, password, firstname, lastname,
								email, timeZone);
							command.ExecuteNonQuery();

							userId = GetLastAutoId(connection);

							ReportResult(String.Format("User {0} created", xpid));

						}
					}
					else // the user existed already
					{
						ReportWarning(String.Format("User {0} already exists in the Application Suite database.", xpid));
					}


					// first check if the user has any devices--if so, we skip
					// check if the device already exists in appsuiteadmin
					bool hasDevice = false;
					string macAddress = null;
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = String.Format(@"
							
							SELECT mac_address FROM as_phone_devices WHERE as_users_id = {0}", 
							userId);

						using(IDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								hasDevice = true;
								macAddress = Convert.ToString(reader[0]);
							}
						}
					}

					if(hasDevice && String.Compare(macAddress, phoneDeviceName, true) != 0)
					{
						ReportError(String.Format("Skipping user {0} who already has a device {1} associated with their account", xpid, macAddress));
						continue;
					}



					int deviceId = -1;

					// check if the device already exists in appsuiteadmin
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = String.Format(@"
							
							SELECT as_phone_devices_id FROM as_phone_devices WHERE mac_address = '{0}'", 
							phoneDeviceName);

						using(IDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								deviceId = Convert.ToInt32(reader[0]);
							}
						}
					}


					if(deviceId == -1) // device does not exist--we are free to add the device to the user
					{
						string createDevice = String.Format(@"

							INSERT INTO as_phone_devices 
								(as_users_id, is_primary_device, is_ip_phone, name, mac_address) 
							VALUES 
								({0}, {1}, {2}, '{3}', '{4}')",
							userId, 1, 1, phoneDisplayName, phoneDeviceName);
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = createDevice;
							command.ExecuteNonQuery();
						}

						deviceId = GetLastAutoId(connection);
						ReportResult(String.Format("Added to user {0} device {1}", xpid, phoneDeviceName));
					}
					else
					{
						// check if device already belongs to user or not--either way it's a possible error situation, because we skip this user
						string queryDevice = String.Format(@"

							SELECT as_phone_devices_id, username
							FROM as_phone_devices, as_users
							WHERE mac_address = '{0}' 
								AND as_phone_devices.as_users_id = as_users.as_users_id", 
							phoneDeviceName);

						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = queryDevice;
							using(IDataReader reader = command.ExecuteReader())
							{
								if(reader.Read())
								{
									int ownerDeviceId = Convert.ToInt32(reader[0]);
									string owner = Convert.ToString(reader[1]);
									
									if(String.Compare(owner, xpid, true) == 0)
									{
										deviceId = ownerDeviceId;
									}
									else
									{
										ReportWarning(String.Format("User {0} already has device {1} associated with his/her account. Skipping user {2}.", owner, phoneDeviceName, xpid));
									}
								}
								else
								{
									System.Diagnostics.Debug.Assert(false, String.Format("There must be a device {0} associated with a user, but none where found", phoneDeviceName));
								}
							}
						}
					}
					
					if(deviceId != -1)
					{

						// delete any lines associated with the device

						using(IDbCommand command = connection.CreateCommand())
						{
							string fetchLinesForDevice = String.Format(@"
								SELECT directory_number
								FROM as_directory_numbers
								WHERE as_directory_numbers.as_phone_devices_id = {0}", 
								deviceId);
							command.CommandText = fetchLinesForDevice;

							bool foundLine = false;
							string linesDeleted = String.Empty;

							using(IDataReader reader = command.ExecuteReader())
							{
								while(reader.Read())
								{
									foundLine = true;
									linesDeleted += Convert.ToString(reader[0]) + ",";
								}
							}

							if(foundLine)
							{
								ReportWarning(String.Format("Deleted the lines {0} from user {1}'s device {2}", linesDeleted, xpid, phoneDeviceName));

								using(IDbCommand deleteCommand = connection.CreateCommand())
								{
									string deleteLinesFromDevice = String.Format(@"
											DELETE FROM as_directory_numbers
											WHERE as_directory_numbers.as_phone_devices_id = {0}", 
										deviceId);
									deleteCommand.CommandText = deleteLinesFromDevice;
									deleteCommand.ExecuteNonQuery();
								}
							}
							else
							{
								// no lines found on device--nothing to report
							}

						}

						int lineNumberId = -1;
						// check if line number belongs to user
						string queryLineNumber = String.Format(@"

							SELECT as_directory_numbers_id 
							FROM as_directory_numbers 
							WHERE directory_number = '{0}'", 
							accountCode);
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = queryLineNumber;
							using(IDataReader reader = command.ExecuteReader())
							{
								if(reader.Read())
								{
									lineNumberId = Convert.ToInt32(reader[0]);
								}
							}
						}

						// line doesn't exist--add it to the user account
						if(lineNumberId == -1)
						{
							string createlineNumber = String.Format(@"
								INSERT INTO as_directory_numbers 
									(as_phone_devices_id, directory_number, is_primary_number) 
								VALUES ({0}, '{1}', {2})",
								deviceId, accountCode, 1);
							using(IDbCommand command = connection.CreateCommand())
							{
								command.CommandText = createlineNumber;
								command.ExecuteNonQuery();
							}

							ReportResult(String.Format("Added to user {0} line {1}", xpid, accountCode));

						}
						else // line does exist--found out who it belongs to 
							// (it can't belong to the user because we deleted all lines already, if any existed
						{
							string queryLines = String.Format(@"

							SELECT username
							FROM as_directory_numbers, as_phone_devices, as_users
							WHERE directory_number = '{0}'
								AND as_directory_numbers.as_phone_devices_id = as_phone_devices.as_phone_devices_id
								AND as_phone_devices.as_users_id = as_users.as_users_id", 
								accountCode);

							using(IDbCommand command = connection.CreateCommand())
							{
								command.CommandText = queryLines;
								using(IDataReader reader = command.ExecuteReader())
								{
									if(reader.Read())
									{
										string owner = Convert.ToString(reader[0]);
									
										ReportError(String.Format("Unable to associate to user {0} line {1} because it is already in use by user {2}", xpid, accountCode, owner));
									}
									else
									{
										System.Diagnostics.Debug.Assert(false, String.Format("There must be a line number {0} associated with a user, but none where found", accountCode));
									}
								}
							}
						}

						// check dummy number
						bool foundDummy = false;
						string queryDummyFindme = String.Format(@"
							SELECT as_external_numbers_id 
							FROM as_external_numbers 
							WHERE phone_number = '{0}' 
								AND as_users_id = {1}", 
								findMeNumber, userId);

						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = queryDummyFindme;
							using(IDataReader reader = command.ExecuteReader())
							{
								if(reader.Read())
								{
									foundDummy = true;
									ReportWarning(String.Format("Dummy AR Find Me number already found associated with user {0}", xpid));
								}
							}
						}

						if(!foundDummy)
						{
							string createDummyNumber = String.Format(@"
								INSERT INTO as_external_numbers 
									(as_users_id, name, phone_number) 
								VALUES ({0}, '{1}', '{2}')", 
								userId, findMeDisplayName, findMeNumber);

							using(IDbCommand command = connection.CreateCommand())
							{
								command.CommandText = createDummyNumber;
								command.ExecuteNonQuery();
							}

							ReportResult(String.Format("Added to user {0} Find Me Number {1}\n", xpid, findMeNumber));
						}
					}
				}

				transaction.Commit();
			}
			catch(Exception e)
			{
				if(transaction != null)
				{
					transaction.Rollback();
				}

				Console.WriteLine("Unhandled exception in writing to the database.  No data has been written to the database.\n\n {0}", Metreos.Utilities.Exceptions.FormatException(e));

				success = false;
			}
			finally
			{
				if(transaction != null)
				{
					transaction.Dispose();
					transaction = null;
				}
				if(masterConnection != null)
				{
					masterConnection.Close();
					masterConnection.Dispose();
					masterConnection = null;
				}
			}

			
			Console.WriteLine("Writing results, warnings, and errors to " + outFile);
			using(FileStream stream = File.Open(outFile, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine("\n\nAll Errors:\n\n");

					foreach(string error in errors)
					{
						writer.WriteLine(error);
					}

					writer.WriteLine("\n\nAll Warnings:\n\n");

					foreach(string warning in warnings)
					{
						writer.WriteLine(warning);
					}

					writer.WriteLine("\n\nAll Results:\n\n");

					foreach(string result in results)
					{
						writer.WriteLine(result);
					}

					
				}
			}

			return success;
        }
        
        
		private void ReportError(string error)
		{
			Console.WriteLine(error);
			errors.Add(error);
		}

		private void ReportResult(string result)
		{
			Console.WriteLine(result);
			results.Add(result);
		}

		private void ReportWarning(string warning)
		{
			Console.WriteLine(warning);
			warnings.Add(warning);
		}


		protected int GetLastAutoId(IDbConnection connection)
		{
			using(IDbCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT LAST_INSERT_ID()";
				return Convert.ToInt32(command.ExecuteScalar());
			}
		}
	}
}
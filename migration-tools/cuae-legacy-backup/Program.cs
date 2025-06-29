using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace CuaeLegacyBackupTool
{
	class Program
	{

		[STAThread]
		static int Main(string[] args)
		{
			int final = 0;
			string user_defined_path = null;
			StringCollection add_dbs = new StringCollection();

			if (args.Length > 0)
			{
				for (int x = 0; x < args.Length; x++)
				{
					switch (args[x])
					{
						case "-u":
							BehaviorCore.Config.DEFAULT_DB_USER = args[x+1];
							x++;
							break;
						case "-p":
							BehaviorCore.Config.DEFAULT_DB_PASSWORD = args[x+1];
							x++;
							break;
						case "-l":
							user_defined_path = args[x+1];
							x++;
							break;
						case "-h":
							UsageHelp();
							return 0;
						default:
							if (args[x].StartsWith("+"))
							{
								add_dbs.Add(args[x]);
								break;
							}
							UsageHelp();
							return -1;
					}
				}
			}


			// Create the database object
			string dsn = FormatDSN(BehaviorCore.Config.DEFAULT_DB_NAME, BehaviorCore.Config.DEFAULT_DB_HOST, 
				(ushort) UInt32.Parse(BehaviorCore.Config.DEFAULT_DB_PORT), BehaviorCore.Config.DEFAULT_DB_USER, 
				BehaviorCore.Config.DEFAULT_DB_PASSWORD, true, 0);
			IDbConnection db = new MySql.Data.MySqlClient.MySqlConnection(dsn);

			try
			{
				db.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Could not open a database connection.  Error: {0}", ex.Message);
				UsageHelp();
				return -1;
			}

			using (BehaviorCore.Behavior core = new BehaviorCore.Behavior())
			{

				if (user_defined_path != null && Directory.Exists(user_defined_path))
				{
					core.CuaePath = user_defined_path;
				}
				else
				{
					string guessed_path = GuessPath();
					Console.WriteLine("The determined path to the MCE/CUAE environment is {0}. Is this correct? [Y/N]", guessed_path);
					string response = Console.ReadLine();
					if (!response.Trim().ToLower().StartsWith("y"))
					{
						Console.WriteLine("Determine the correct path to the environment and run the tool again with the option '-l <path_to_environment>'");
						final = -1;
					}
					core.CuaePath = guessed_path;
				}

				if (final >= 0)
				{
					core.DbConnection = db;
					try
					{
						string[] dbs = new string[add_dbs.Count];
						add_dbs.CopyTo(dbs, 0);
						if (core.CreateBackup(dbs))
						{
							Console.WriteLine(String.Format("The backup was a success.  It can be located in {0} .", core.DestinationPath));
						}
						else
						{
							Console.WriteLine("The backup was aborted by the user.");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Backup FAILED: {0}", ex.Message);
						final = -1;
					}
				}
			}

			db.Close();
			return final;
		}


		static void UsageHelp()
		{
			Console.WriteLine("Usage: cuae-legacy-backup [-u <database_user>] [-p <database_password>] [-l <path_to_environment>] [+<database_name> ...]");
			Console.WriteLine("     To backup databases in addition to the main database, simply use +<database_name> for each database.  Example:");
			Console.WriteLine("     cuae-backup -u user -p password +db1 +db2");
		}

		static string GuessPath()
		{
			if (Directory.Exists(BehaviorCore.Config.DEFAULT_CUAE23_PATH))
				return BehaviorCore.Config.DEFAULT_CUAE23_PATH;
			else if (Directory.Exists(BehaviorCore.Config.DEFAULT_MCE_PATH))
				return BehaviorCore.Config.DEFAULT_MCE_PATH;
			else
				return BehaviorCore.Config.DEFAULT_CUAE_PATH;
		}


		/// <summary>Creates a connection string for a MySQL database</summary>
		/// <param name="dbName">The database name</param>
		/// <param name="host">The server address</param>
		/// <param name="port">The server port (default: 3306)</param>
		/// <param name="username">Database user</param>
		/// <param name="password">Database user's password</param>
		/// <param name="enablePooling">Indicates that the SQL connector should pool connections</param>
		/// <param name="connectionTimeout">Number of seconds before connection times out</param>
		/// <returns>Connection string</returns>
		static string FormatDSN(string dbName, string host, ushort port, string username, 
			string password, bool enablePooling, uint connectionTimeout)
		{
			//"DATABASE=SiteTracker;Driver=mysql;SERVER=localhost;UID=hamu;PWD
			//=naptra;PORT=3306;OPTION=131072;STMT=;"
			StringBuilder dsn = new StringBuilder();
            
			if(dbName == null) { return null; }

			dsn.Append("database=");
			dsn.Append(dbName);
            
			if(host != null)
			{
				dsn.Append("; server=");
				dsn.Append(host);
			}
			if(port != 0)
			{
				dsn.Append("; port=");
				dsn.Append(port);
			}
			if(username != null)
			{
				dsn.Append("; uid=");
				dsn.Append(username);
			}
			if(password != null)
			{
				dsn.Append("; pwd=");
				dsn.Append(password);
			}

			if(enablePooling)
			{
				dsn.Append("; pooling=true");
			}
			else
			{
				dsn.Append("; pooling=false");
				dsn.Append("; connection lifetime=0");
			}

			if(connectionTimeout > 0)
			{
				dsn.Append("; connection timeout=");
				dsn.Append(connectionTimeout);
			}

			return dsn.ToString();
		}
	}
}

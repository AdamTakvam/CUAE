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
        private AXLAPIService service;
        private int writeWait;
        private ArrayList callerDns;
        private ArrayList receiverDns;
        private ArrayList errors;
		private ArrayList results;

        public Worker(AXLAPIService service, int maxAxlWrite)
		{
            this.writeWait = 60 * 1000 / maxAxlWrite + 300;
            this.service = service;  	
            this.callerDns = new ArrayList();
            this.receiverDns = new ArrayList();
            this.errors = new ArrayList();
			this.results = new ArrayList();
        }
        
        public bool Generate(   long deviceStart, ArrayList users, ArrayList devices,
                                string description, string css, 
                                string partition, string phoneTemplateName, string outFile)
        {
			int i = 0;

			foreach(string user in users)
			{
				bool foundUser = false;
				string device = null;
				string first = null;
				string last = null;

				// Find user in devices maping
				foreach(string[] userdevice in devices)
				{
					string user2 = userdevice[0];

					if(user == userdevice[0])
					{
						// Found user
						foundUser = true;
						device = userdevice[1];
						first = userdevice[2];
						last = userdevice[3];

						// Get the user's everyday device, and line number
						getPhone getUserPhone = new getPhone();
						getUserPhone.Item = device;
						getUserPhone.ItemElementName = ItemChoiceType5.phoneName;
						
						string uuid = null;
						object mediaResourceGroup = null;
						try
						{
							Thread.Sleep(100);
							getPhoneResponse usersPhone = service.getPhone(getUserPhone);
							
							if(usersPhone.@return != null && 
								usersPhone.@return.device != null &&
								usersPhone.@return.device.lines.Items != null &&
								usersPhone.@return.device.lines.Items.Length > 0)
							{
								
								int minIndex = int.MaxValue;
								XLine primaryLineItem = null;
								foreach(XLine line in usersPhone.@return.device.lines.Items)
								{
									int index = -1;

									try
									{
										index = int.Parse(line.index);
				
										if(index < minIndex)
										{
											minIndex = index;
											primaryLineItem = line;
										}
									}
									catch
									{
										continue;
									}
								}

								if(primaryLineItem != null)
								{
									mediaResourceGroup = usersPhone.@return.device.Item5;
									uuid = primaryLineItem.Item.uuid;
								}
								else
								{
									// No lines found for the users
									string error = String.Format("No lines found on device {0} for user {1}.  Skipping...", device, user);
									Console.WriteLine(error);
									errors.Add(error);

									// Create device association with mysql
									continue;
								}
							}
							else
							{
								// No lines found for the users
								string error = String.Format("No lines found on device {0} for user {1}.  Skipping...", device, user);
								Console.WriteLine(error);
								errors.Add(error);

								continue;
							}
						}
						catch(Exception e)
						{
							// Unable to retrieve device defined for user
							string error = String.Format("Unable to locate device {0} for user {1}.\n\n{2}", device, user, GetDetail(e));
							Console.WriteLine(error);
							errors.Add(error);
							continue;
						}

						getLineResponse primaryLine = null;

						if(uuid != null)
						{
							getLine getLineRequest = new getLine();
							getLineRequest.uuid = uuid;
							try
							{
								Thread.Sleep(100);
								primaryLine = service.getLine(getLineRequest);
							}
							catch(Exception e)
							{
								string error = String.Format("Unable to retrieve line from device {0} for user {1}.\n\n{2}", device, user, GetDetail(e));
								Console.WriteLine(error);
								errors.Add(error);
								continue;
							}
						}

						string pattern = primaryLine.@return.directoryNumber.pattern;
					
						bool associated = CreateAppsuiteadminAssociation(user, device, pattern);

						if(!associated)
						{
							string error = String.Format("Unable or already associated device {0} and line number {1} for user {2}.", device, pattern, user);
							Console.WriteLine(error);
							errors.Add(error);
							
							bool continueOn = true;
							while(true)
							{
								Console.WriteLine("Add device into CallManager anyway (y/n)?");
								string userResponse = Console.ReadLine().ToLower();
								if(userResponse == "y" || userResponse == "yes")
								{
									// allow drop out of block
									continueOn = false;
									break;
								}
								else if(userResponse == "n" || userResponse == "no")
								{
									continueOn = true;
									break;
								}
							}

							if(continueOn)
							{
								continue;
							}
							
						}

						// Create the new device with the existing line
						Thread.Sleep(writeWait);

						// Define shared line
						XPhoneLines lines = null;
						lines = new XPhoneLines();
						lines.Items = new XLine[1];
						XLine oneLine = new XLine();
						oneLine.index = "1";   
						oneLine.e164Mask = null; // Had to
						oneLine.ringSetting = XRingSetting.UseSystemDefault; // Had to
						oneLine.Item = new XNPDirectoryNumber();
						oneLine.Item.uuid = uuid;
						lines.Items[0] = oneLine; 

						// Define new phone
						addPhone phone = new addPhone();
						XIPPhone xipphone = new XIPPhone();
						xipphone.description = description + " " + first + " " + last;
						xipphone.name = ConvertMACToSEP(deviceStart + i);
						xipphone.lines = lines;
						xipphone.@class = XClass.Phone;
						xipphone.addOnModules = null;
                
						xipphone.protocol = XDeviceProtocol.Ciscostation; // THIS MUST BE THIS FOR 7960!!
						xipphone.Item1 = XModel.Cisco7960;
						xipphone.Item = XProduct.Cisco7960;
						xipphone.Item2 = css;
						xipphone.Item3 = "Default";
						xipphone.Item8 = phoneTemplateName;
						xipphone.protocolSide = XProtocolSide.User;
						phone.newPhone = xipphone;

						xipphone.Item5 = mediaResourceGroup;

						try
						{
							service.addPhone(phone);
							Console.WriteLine("{0} added with line number {1} for user {2}", ConvertMACToSEP(deviceStart + i), pattern, user);
							results.Add(String.Format("{0},{1},{2}", user, ConvertMACToSEP(deviceStart + i), pattern));
							i++;
						}
						catch(Exception e)
						{
							ReportError(String.Format("Unable to add the phone {0} with line {1} for user {2}.  \n{3}", ConvertMACToSEP(deviceStart + i), pattern, user, GetDetail(e)));
						}

						break;
					}
				}

				if(!foundUser)
				{
					string error = String.Format("No device added for user {0}", user);
					Console.WriteLine(error);
					errors.Add(error);
				}
			}

           

			Console.WriteLine("Writing results and errors to " + outFile);
			using(FileStream stream = File.OpenWrite(outFile))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					foreach(string result in results)
					{
						writer.WriteLine(result);
					}

					writer.WriteLine("\n\nAll Errors:\n\n");

					foreach(string error in errors)
					{
						writer.WriteLine(error);
					}
				}
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
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0').ToUpper();
        }

		private bool CreateAppsuiteadminAssociation(string user, string device, string lineNumber)
		{
			// Prepare mysql connection
			IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true));
			
			try
			{
				connection.Open();
			}
			catch
			{
				Console.WriteLine("Unable to open a connection to {0}.  This tool must be run on the Application Server");
				return false;
			}

			string getUserQuery = String.Format("SELECT as_users_id FROM as_users WHERE username = '{0}' AND status = 1", user);
			
			bool foundDevice = false;

			try
			{
				int userId = -1;

				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = getUserQuery;
					using(IDataReader reader = command.ExecuteReader())
					{
						if(reader.Read())
						{
							userId = Convert.ToInt32(reader[0]);
						}
						else
						{
							Console.WriteLine("Unable to find user {0} in appsuiteadmin", user);
							return false;
						}
					}
				}

				int deviceId = -1;

				// check if device already belongs to user
				string queryDevice = String.Format("SELECT as_phone_devices_id FROM as_phone_devices WHERE mac_address = '{0}' AND as_users_id = {1}", device, userId);
				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = queryDevice;
					using(IDataReader reader = command.ExecuteReader())
					{
						if(reader.Read())
						{
							deviceId = Convert.ToInt32(reader[0]);
							foundDevice = true;
							Console.WriteLine("Found device {0} for user {1} already created in appsuiteadmin.", device, user);
						}
					}
				}
					
				if(deviceId == -1)
				{
					string createDevice = String.Format("INSERT INTO as_phone_devices (as_users_id, is_primary_device, is_ip_phone, name, mac_address) VALUES ({0}, {1}, {2}, '{3}', '{4}')",
						userId, 1, 1, "Office Phone", device);
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = createDevice;
						command.ExecuteNonQuery();
					}

					deviceId = GetLastAutoId(connection);
				}

				int lineNumberId = -1;
				// check if line number belongs to user
				string queryLineNumber = String.Format("SELECT as_directory_numbers_id FROM as_directory_numbers WHERE directory_number = '{0}' AND as_phone_devices_id = {1}", lineNumber, deviceId);
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


				if(lineNumberId == -1)
				{
					string createlineNumber = String.Format("INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number) VALUES ({0}, '{1}', {2})",
						deviceId, lineNumber, 1);
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = createlineNumber;
						command.ExecuteNonQuery();
					}
				}

				// check dummy number
				bool foundDummy = false;
				string queryDummyFindme = String.Format("SELECT as_external_numbers_id FROM as_external_numbers WHERE phone_number = '11111' AND as_users_id = {0}", userId);
				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = queryDummyFindme;
					using(IDataReader reader = command.ExecuteReader())
					{
						if(reader.Read())
						{
							foundDummy = true;
						}
					}
				}

				if(!foundDummy)
				{
					string createDummyNumber = String.Format("INSERT INTO as_external_numbers (as_users_id, name, phone_number) VALUES ({0}, '{1}', '{2}')", userId, "AR Dummy Number", "11111");
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = createDummyNumber;
						command.ExecuteNonQuery();
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to perform SQL query on user {0} with device {1} and line {2}.\n\n{3}", user, device, lineNumber, Metreos.Utilities.Exceptions.FormatException(e)));
				return false;
			}

			return !foundDevice;
		}

		protected int GetLastAutoId(IDbConnection connection)
		{
			using(IDbCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT LAST_INSERT_ID()";
				return Convert.ToInt32(command.ExecuteScalar());
			}
		}

        private string ConvertMACToADP(long deviceMac)
        {
            return "ADP" + deviceMac.ToString("x").PadLeft(12, '0').ToUpper();
        }
    
        private string ConvertMACToDP(long deviceMac)
        {
            return "DP" + deviceMac.ToString("x").PadLeft(12, '0').ToUpper();
        }

        private string ConvertMACToString(long deviceMac)
        {
            return deviceMac.ToString("x").PadLeft(12, '0');
        }
	}
}
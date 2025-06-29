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
        private ArrayList errors;
		private ArrayList results;
		private ArrayList dbUsers;
		private ArrayList existingVirtualDevices;
		private string[] baseMacs;
		private int numDevicesInRange;
		private bool readOnly;
		private ArrayList devicesFile;
		private string currentRange;
		private ArrayList usedRanges;
		private string css;
		private string description;
		private string phoneTemplateName;
		private bool prompt;
		private Hashtable unusedPhonesBeforeSync;
		
		// Error arrays
		private ArrayList virtual2LinePhones; // device
		private ArrayList deviceChangedUsers; // user | appsuitedevice | devicefiledevice
		private ArrayList invalidDeviceForUser; // user | device
		private ArrayList noLineCCMDeviceForUser; // user | device
		private ArrayList sccpPhonesWithUnusedLine; // device | line
		private ArrayList dupDevices; // needsDeviceUser | hasDeviceUser | devicename
		private ArrayList dupLines; // needsLineUser | hasLineUser | line number

        public Worker(AXLAPIService service, int maxAxlWrite)
		{
            this.writeWait = 60 * 1000 / maxAxlWrite + 300;
            this.service = service;  	
            this.errors = new ArrayList();
			this.results = new ArrayList();
			this.existingVirtualDevices = new ArrayList();
			this.deviceChangedUsers = new ArrayList();
			this.dbUsers = new ArrayList();
			this.invalidDeviceForUser = new ArrayList();
			this.noLineCCMDeviceForUser = new ArrayList();
			this.virtual2LinePhones = new ArrayList();
			this.sccpPhonesWithUnusedLine = new ArrayList();
			this.devicesFile = new ArrayList();
			this.unusedPhonesBeforeSync = new Hashtable();
			this.dupDevices = new ArrayList();
			this.dupLines = new ArrayList();
			this.baseMacs = new string[0];
			this.numDevicesInRange = 625;
			this.readOnly = true;
			this.currentRange = null;
			this.usedRanges = new ArrayList();
        }



		public class User
		{
			public int id;
			public string username;
			public string firstname;
			public string lastname;
			public string devicename;
			public string linenumber;
			public string lineUuid;
			public string cuaeDevicename;
			public object mediaResourceGroup;
			public bool hasDefaultDisabledFindme;
			public bool skip;
			public string skipReason;
			public ArrayList error;

			public User()
			{
				error = new ArrayList();
				username = String.Empty;
				firstname = String.Empty;
				lastname = String.Empty;
				devicename = String.Empty;
				linenumber = String.Empty;
				lineUuid = String.Empty;
				cuaeDevicename = String.Empty;
				mediaResourceGroup = String.Empty;
				hasDefaultDisabledFindme = false;
				skip = false;

			}
		}

		public class Device
		{
			public string devicename;
			public string deviceUuid;
			public string lineNumber;
			public string lineUuid;
			public bool moreThanOneLine;

			public Device()
			{
				devicename = String.Empty;
				deviceUuid = String.Empty;
				lineNumber = String.Empty;
				lineUuid   = String.Empty;
				moreThanOneLine = false;
			}
		}

		#region ReadUsersFromDb
		public bool ReadUsersFromDb(ArrayList users)
		{
			// Prepare mysql connection
			using(IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true)))
			{
				try
				{
					connection.Open();
				}
				catch
				{
					Console.WriteLine("Unable to open a connection to {0}.  This tool must be run on the Application Server");
					return false;
				}
			
				try
				{
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = "SELECT as_users_id, username, first_name, last_name FROM as_users WHERE status = 1";
						using(IDataReader reader = command.ExecuteReader())
						{
							while(reader.Read())
							{
								int as_users_id = Convert.ToInt32(reader[0]);
								string username = Convert.ToString(reader[1]);
								string firstname = Convert.ToString(reader[2]);
								string lastname = Convert.ToString(reader[3]);
								string macAddress = null;
								string lineNumber = null;
								bool hasDefaultDisableFindme = false;	

								AddDisabled(as_users_id, username);

								bool skipUser = false;
								foreach(User user in users)
								{
									if(String.Compare(user.username, username, false) == 0)
									{
										skipUser = true;
									}
								}

								if(!skipUser)
								{
									using(IDbConnection deviceConnection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true)))
									{
										deviceConnection.Open();

										using(IDbCommand deviceCommand = deviceConnection.CreateCommand())
										{
											deviceCommand.CommandText = "SELECT as_phone_devices_id, mac_address FROM as_phone_devices WHERE as_users_id = " + as_users_id;
											using(IDataReader deviceReader = deviceCommand.ExecuteReader())
											{
												if(deviceReader.Read())
												{
													int as_phone_devices_id = Convert.ToInt32(deviceReader[0]);
													macAddress = DBNull.Value == deviceReader[1] ? null : Convert.ToString(deviceReader[1]);

													using(IDbConnection lineConnection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true)))
													{
														lineConnection.Open();

														using(IDbCommand lineCommand = lineConnection.CreateCommand())
														{
															lineCommand.CommandText = "SELECT directory_number FROM as_directory_numbers WHERE as_phone_devices_id = " + as_phone_devices_id;
															using(IDataReader lineReader = lineCommand.ExecuteReader())
															{
																if(lineReader.Read())
																{
																	lineNumber = Convert.ToString(lineReader[0]);
																}
															}
														}
													}
												}
											}
										}
								

										using(IDbCommand findMeCommand = deviceConnection.CreateCommand())
										{
											findMeCommand.CommandText = String.Format("SELECT as_external_numbers_id FROM as_external_numbers WHERE as_users_id = {0} AND phone_number = '11111' AND ar_enabled = 0", as_users_id);
											using(IDataReader findMeReader = findMeCommand.ExecuteReader())
											{
												hasDefaultDisableFindme = findMeReader.Read();
											}
										}
									}
								


									User user = new User();
									user.id = as_users_id;
									user.username = username;
									user.firstname = firstname;
									user.lastname = lastname;
									user.devicename = macAddress;
									user.linenumber = lineNumber;
									user.hasDefaultDisabledFindme = hasDefaultDisableFindme;

									dbUsers.Add(user);
								}
							}
						}
					}
				}
				catch(Exception e)
				{
					Console.WriteLine("Unable to read in users. " + e);
					return false;
				}
			}

			return true;
		}
		#endregion

		#region ReadVirtualDevicesFromCCM
		
		public bool ReadVirtualDevicesFromCCM(string[] deviceStarts)
		{
			try
			{
				foreach(string deviceStart in deviceStarts)
				{
					listDeviceByNameAndClass getDevices = new listDeviceByNameAndClass();
					getDevices.searchString = "SEP" + deviceStart + "%";

					listDeviceByNameAndClassResponse response = null;
					try
					{
						Thread.Sleep(writeWait);
						response = service.listDeviceByNameAndClass(getDevices);
					}
					catch(Exception e)
					{
						ReportError(String.Format("Unable to retrieve devices by device query {0}.\n\nException: {1}", deviceStart, GetDetail(e)));
						return false;
					}

					if(response!= null && response.@return != null)
					{
						foreach(ListDeviceResDevice device in response.@return)
						{
							Device virtualPhone = new Device();
							virtualPhone.devicename = device.name;
							virtualPhone.deviceUuid = device.uuid;
							existingVirtualDevices.Add(virtualPhone);

							getPhone retrievePhone = new getPhone();
							retrievePhone.ItemElementName = ItemChoiceType5.phoneId;
							retrievePhone.Item = device.uuid;

							getPhoneResponse phoneResponse = null;

							try
							{
								Thread.Sleep(writeWait);
								phoneResponse = service.getPhone(retrievePhone);
								Console.WriteLine("\nRetrieved phone for inspection: {0}", device.name);
							}
							catch(Exception e)
							{
								ReportError(String.Format("Unable to retrieve phone {0}.\n\nException: {1}", device.name, GetDetail(e)));
								return false;
							}

							if(phoneResponse != null && 
								phoneResponse.@return != null && 
								phoneResponse.@return.device != null && 
								phoneResponse.@return.device.lines != null)
							{
								if(phoneResponse.@return.device.lines.Items == null || 
									phoneResponse.@return.device.lines.Items.Length == 0)
								{
									continue; // Not an error.
								}
								else if(phoneResponse.@return.device.lines.Items.Length > 1)
								{
									ReportError(String.Format("SCCP Phone {0} has more than one line.  This must be remedied by administrator", device.name));
									virtual2LinePhones.Add(device.name);
									virtualPhone.moreThanOneLine = true;
									continue;
								}
								else
								{
									string lineUuid = ((XLine) phoneResponse.@return.device.lines.Items[0]).Item.uuid;
								
									getLine retrieveLine = new getLine();
									retrieveLine.uuid = lineUuid;
									getLineResponse lineResponse = null;
	
									try
									{
										Thread.Sleep(writeWait);
										lineResponse = service.getLine(retrieveLine);
									}
									catch(Exception e)
									{
										ReportError(String.Format("Unable to retrieve line from device {0}.\n\nException: {1}", GetDetail(e)));
										return false;
									}

									if(lineResponse != null &&
										lineResponse.@return != null &&
										lineResponse.@return.directoryNumber != null)
									{
										string pattern = lineResponse.@return.directoryNumber.pattern;
										virtualPhone.lineNumber = pattern;
										virtualPhone.lineUuid = lineUuid;
									}
								}
							}
							else
							{
								ReportError(String.Format("Phone returned data came from AXL corrupted {0}", device.name));
								return false;
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Programmer error in CCM sync process. {0}", e));
				return false;
			}

			return true;
		}

		#endregion
	
		#region GetUsersCCMInfo
		public bool GetUsersCCMInfo(ArrayList devices)
		{
			foreach(User user in dbUsers)
			{
				// Check that user has right info in appsuiteadmin versus devices list
				foreach(string[] info in devices)
				{
					string usernameOnDevice = info[0];
					string deviceNameOnDevice = info[1];
					string firstname = info[2];
					string lastname = info[3];

					// if found user in devices files
					if(String.Compare(user.username, usernameOnDevice) == 0)
					{
						// use firstname/lastname in appsuiteadmin, unless none defined--then try to user devices file
						if(user.firstname == null || user.firstname == String.Empty)
						{
							user.firstname = firstname;
						}
						if(user.lastname == null || user.lastname == String.Empty)
						{
							user.lastname = lastname;
						}
						
						if( (user.devicename == null || user.devicename == String.Empty) && 
							(deviceNameOnDevice != null && deviceNameOnDevice != String.Empty))
						{
							// no device defined in appsuiteadmin, but defined in devices file
							using(IDbConnection connection = GetConnection())
							{
								if(connection == null)
								{
									return false;
								}

								bool added = AddDevice(connection, user.id, deviceNameOnDevice, user.username);

								if(!added)
								{
									string username = GetDevice(connection, deviceNameOnDevice);

									string skipReason = String.Format("Duplicate mac address between user: {0} and user: {1}.  Mac Address: {2}", user.username, username, deviceNameOnDevice);
									Console.WriteLine(skipReason);
									dupDevices.Add(new string[] {user.username, username, deviceNameOnDevice});
									user.skip = true;
									user.skipReason = skipReason;
									break;
								}

								user.devicename = deviceNameOnDevice;
							}
						}
						else if(
							(user.devicename != null && user.devicename != String.Empty) && 
							(deviceNameOnDevice == null || deviceNameOnDevice == String.Empty))
						{
							// device defined in appsuiteadmin, although not defined in devices file
							continue;
						}
						else if(
							(user.devicename == null || user.devicename == String.Empty) && 
							(deviceNameOnDevice == null || deviceNameOnDevice == String.Empty))
						{
							// no device defined in appsuiteadmin, and not defined in devices file
							continue;
						}
						else if(
							(user.devicename != null && user.devicename != String.Empty) && 
							(deviceNameOnDevice == null || deviceNameOnDevice == String.Empty) &&
							(String.Compare(user.devicename, deviceNameOnDevice) == 0))
						{
							// device defined in appsuiteadmin and devices file, 
							// and are the same value
							continue;
						}

						else if(
							(user.devicename != null && user.devicename != String.Empty) && 
							(deviceNameOnDevice != null && deviceNameOnDevice != String.Empty) &&
							(String.Compare(user.devicename, deviceNameOnDevice) != 0))
						{
							// device defined in appsuiteadmin and devices file, 
							// but not the same value
							bool useAppsuite;
							while(true)
							{
								Console.WriteLine("\n\nAppsuiteadmin and Devices file do not agree for user {0}", user.username);
								Console.Write("Appsuiteadmin: {0}, Devices: {1} (a/d):", user.devicename, deviceNameOnDevice);
								
								string response = Console.ReadLine().ToLower();
								
								if(response == "a")
								{
									useAppsuite = true;
									break;
								}
								else if(response == "d")
								{
									useAppsuite = false;
									break;
								}
								else
								{
									continue;
								}
							}

							if(useAppsuite)
							{
								// Do nothing
							}
							else
							{
								// Fix db
								// Fix local store

								using(IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true)))
								{
									try
									{
										connection.Open();
									}
									catch
									{
										Console.WriteLine("Unable to open a connection to {0}.  This tool must be run on the Application Server");
										return false;
									}
								
									bool removed = RemoveDevice(connection, user.id, user.devicename);
									if(!removed)
									{
										Console.WriteLine("Unable to remove device {0} from user account {1}.  Exiting...", user.devicename, user.username);
										return false;
									}

									bool added = AddDevice(connection, user.id, deviceNameOnDevice, user.username);
									if(!added)
									{
										string skipReason = String.Format("User {0} will be skipped in synchoronization because device {1} could not be associated with account", user.username, deviceNameOnDevice);
										Console.WriteLine();
										user.skip = true;
										user.skipReason = skipReason;
										continue;
									}
									else
									{
										// be careful not to move this line below the next...
										deviceChangedUsers.Add(new string[] {user.username, user.devicename, deviceNameOnDevice});
										user.devicename = deviceNameOnDevice;
									}
								}
							} // else if
							
						} // foreach
						
						
						break;
					}
					else // not found user in devices file
					{
					}
				}	
			}


			// Once every user has been verified to be in sync with the 'devices' file, 
			// we can then suck down every real device associated with the user
			foreach(User user in dbUsers)
			{
				if(!user.skip && (user.devicename != null && user.devicename != String.Empty))
				{
					getPhone retrievePhone = new getPhone();
					retrievePhone.ItemElementName = ItemChoiceType5.phoneName;
					retrievePhone.Item = user.devicename;

					getPhoneResponse phoneResponse = null;

					try
					{
						Thread.Sleep(writeWait);
						phoneResponse = service.getPhone(retrievePhone);
						Console.WriteLine("\nRetrieved phone for inspection: {0} for user: {1}", user.devicename, user.username);
					}
					catch(Exception e)
					{
						string skipReason = String.Format("Unable to retrieve phone {0} for user {1}.\n\nException: {2}", user.devicename, user.username, GetDetail(e));
						ReportError(skipReason);
						invalidDeviceForUser.Add(new string[] {user.username, user.devicename});
						user.skip = true;
						user.skipReason = skipReason;
						continue;
					}

					if(phoneResponse != null && 
						phoneResponse.@return != null && 
						phoneResponse.@return.device != null && 
						phoneResponse.@return.device.lines != null)
					{
						if(phoneResponse.@return.device.lines.Items == null ||
							phoneResponse.@return.device.lines.Items.Length == 0) // indicates no lines
						{
							string skipReason = String.Format("No lines on CCM device {0} for user {1}", user.devicename, user.username);
							noLineCCMDeviceForUser.Add(new string[] {user.username, user.devicename});
							user.skip = true;
							user.skipReason = skipReason;
							continue; // Not a fatal error.
						}
						else 
						{
							int minIndex = int.MaxValue;
							XLine primaryLineItem = null;
							foreach(XLine line in phoneResponse.@return.device.lines.Items)
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
								user.mediaResourceGroup = phoneResponse.@return.device.Item5;
								user.lineUuid = primaryLineItem.Item.uuid;

								getLine retrieveLine = new getLine();
								retrieveLine.uuid = user.lineUuid;
								getLineResponse lineResponse = null;
	
								try
								{
									Thread.Sleep(writeWait);
									lineResponse = service.getLine(retrieveLine);
								}
								catch(Exception e)
								{
									string skipReason = String.Format("Unable to retrieve line from device {0} for user {1} .\n\nException: {2}", user.devicename, user.username, GetDetail(e));
									ReportError(skipReason);
									user.skip = true;
									user.skipReason = skipReason;
									continue; // Not a fatal error.
								}

								if(lineResponse != null &&
									lineResponse.@return != null &&
									lineResponse.@return.directoryNumber != null)
								{
									string pattern = lineResponse.@return.directoryNumber.pattern;
									user.linenumber = pattern;
								}

								// sync up appsuiteadmin lines
								bool synchedLine = AddLine(user.id, user.username, user.devicename, user.linenumber);

								if(!synchedLine)
								{
									string skipReason = String.Format("Unable to add line {0} to user account {1}", user.linenumber, user.username);
									user.skipReason = skipReason;
									user.skip = true;
								}
							}
							else
							{
								// No lines found for the users
								string error = String.Format("No lines found on device {0} for user {1}.  Skipping...", user.devicename, user.username);
								ReportError(error);
								user.skip = true;
								user.skipReason = error;

								continue;
							}
						}
					}
					else
					{
						string skipReason = String.Format("Phone returned data came from AXL corrupted {0}", user.devicename);
						ReportError(skipReason);
						invalidDeviceForUser.Add(new string[] {user.username, user.devicename});
						user.skip = true;
						user.skipReason = skipReason;
						continue;
					}
				}
			}

			return true;
		}

		#endregion

		#region DetermineAssociations

		private void DetermineAssociations()
		{
			// Iterate through each user, determining if has line configured on sccp phone
			foreach(User user in dbUsers)
			{
				if(!user.skip)
				{
					foreach(Device device in existingVirtualDevices)
					{
						if((user.linenumber != null && user.linenumber != String.Empty) &&
							String.Compare(user.lineUuid, device.lineUuid) == 0)
						{
							user.cuaeDevicename = device.devicename;
							break;
						}
					}
				}
			}
		

			// Create available device ranges
			foreach(string baseMac in baseMacs)
			{
				long baseMacLong = long.Parse(baseMac + "000000", System.Globalization.NumberStyles.HexNumber); 

				ArrayList unusedDevices = new ArrayList();
				for(int i = 0; i < numDevicesInRange; i++)
				{
					string deviceName = "SEP" + (baseMacLong + i).ToString("x").PadLeft(12, '0').ToUpper();

					// hunt down to see if user has association
					bool foundUser = false;
					foreach(User user in dbUsers)
					{
						if(!user.skip)
						{
							if(String.Compare(user.cuaeDevicename, deviceName, true) == 0)
							{
								foundUser = true;
								break;
							}
						}
					}

					if(!foundUser)
					{
						bool problemPhone = false;
						foreach(Device device in existingVirtualDevices)
						{
							// check if the device has more than one line
							if(String.Compare(device.devicename, deviceName, true) == 0)
							{
								if(device.moreThanOneLine)
								{
									problemPhone = true;
									break;
								}

								// check if the device hase one line, and so therefore could be associated with a user
								// which could be possible if their is a different Application Server.
								if(device.lineNumber != null && device.lineNumber != String.Empty)
								{
									sccpPhonesWithUnusedLine.Add(new string[] {device.devicename, device.lineNumber});
									problemPhone = true;
									break;
								}
							}
						}
						
						if(!problemPhone)
						{
							unusedDevices.Add(deviceName);
						}
					}
				}

				unusedPhonesBeforeSync[baseMac] = unusedDevices;
			}
		}

		#endregion

		#region Synchronize
		private void Synchronize()
		{
			foreach(User user in dbUsers)
			{
				bool synched = SynchronizeCCM(user);

				if(!synched)
				{
					return;
				}
			}
		}

		private bool SynchronizeCCM(User user)
		{
			Console.WriteLine("\n\nPreparing to sync {0}", user.username);
			// Determine if unassociated user is associated.

			if(user.skip)
			{
				Console.WriteLine("Skipping user {0} for error reason\n'{1}'", user.username, user.skipReason);
				return true;
			}

			if(user.cuaeDevicename != null && user.cuaeDevicename != String.Empty)
			{
				Console.WriteLine("Skipping user {0} because already synchronized with device {1}", user.username, user.cuaeDevicename);
				return true;
			}

			if(user.linenumber == null || user.linenumber == String.Empty)
			{
				Console.WriteLine("Skipping user {0} due to unable to determine line association or device association in CallManager", user.username);
				return true;
			}

		
			// if cuaeDevicename still unidentified, but line number is defined...
			
			// Get an available device
			string devicename = CheckOutDevice();

			if(devicename == null)
			{
				return false;
			}

			bool deviceExists = false;
			// check if device is existing
			foreach(Device device in existingVirtualDevices)
			{
				if(device.devicename == devicename)
				{
					deviceExists = true;
					break;
				}
			}

			if(!deviceExists)
			{
				bool doit = true;
				if(prompt)
				{
					while(true)
					{
						Console.Write("ADD PHONE {0} FOR USER {1} WITH LINE {2}? (y/n/yesall/noall):", devicename, user.username, user.linenumber);
						string response = Console.ReadLine().ToLower();

						if(response == "y" || response == "yes")
						{
							doit = true;
							break;
						}
						else if(response == "n" || response == "no")
						{
							doit = false;
							break;
						}
						else if(response == "yesall")
						{
							doit = true;
							prompt = false;
							break;
						}
						else if(response == "noall")
						{
							prompt = false;
							return false;
						}
					}
				}
				else
				{
					Console.Write("ADD PHONE {0} FOR USER {1} WITH LINE {2}? (yes chosen)", devicename, user.username, user.linenumber);
				}

				// Create the new device with the existing line
				if(doit)
				{
					// Define shared line
					XPhoneLines lines = null;
					lines = new XPhoneLines();
					lines.Items = new XLine[1];
					XLine oneLine = new XLine();
					oneLine.index = "1";   
					oneLine.e164Mask = null; // Had to
					oneLine.ringSetting = XRingSetting.UseSystemDefault; // Had to
					oneLine.Item = new XNPDirectoryNumber();
					oneLine.Item.uuid = user.lineUuid;
					lines.Items[0] = oneLine; 

					// Define new phone
					addPhone phone = new addPhone();
					XIPPhone xipphone = new XIPPhone();
					xipphone.description = description + " " + user.firstname + " " + user.lastname;
					xipphone.name = devicename;
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

					xipphone.Item5 = user.mediaResourceGroup;

					try
					{
						Thread.Sleep(writeWait);

						service.addPhone(phone);
						user.cuaeDevicename = devicename;
					}
					catch(Exception e)
					{
						ReportError(String.Format("Unable to add the phone {0} with line {1} for user {2}.  \n{3}", devicename, user.linenumber, user.username, GetDetail(e)));
					}
				}
			}
			else
			{
				bool doit = true;
				if(prompt)
				{
					while(true)
					{
						Console.Write("UPDATE PHONE {0} FOR USER {1} WITH LINE {2}? (y/n/yesall/noall):", devicename, user.username, user.linenumber);
						string response = Console.ReadLine().ToLower();

						if(response == "y" || response == "yes")
						{
							doit = true;
							break;
						}
						else if(response == "n" || response == "no")
						{
							doit = false;
							break;
						}
						else if(response == "yesall")
						{
							doit = true;
							prompt = false;
							break;
						}
						else if(response == "noall")
						{
							prompt = false;
							return false;
						}
					}
				}
				else
				{
					Console.Write("UPDATE PHONE {0} FOR USER {1} WITH LINE {2}? (yes chosen)", devicename, user.username, user.linenumber);
				}
				
				if(doit)
				{
					// UPDATE PHONES
					updatePhone updatePhone = new updatePhone();
					updatePhone.ItemElementName = ItemChoiceType4.name;
					updatePhone.Item = devicename;
					updatePhone.description = description + " " + user.firstname + " " + user.lastname;
					UpdatePhoneReqLines lines = new UpdatePhoneReqLines();
					XLine line = new XLine();
					line.Item = new XNPDirectoryNumber();
					line.Item.uuid = user.lineUuid;
					line.index = "1";
					line.ringSetting = XRingSetting.UseSystemDefault;
					lines.Items = new XLine[] {line};
					updatePhone.lines = lines;

					try
					{
						Thread.Sleep(writeWait);

						service.updatePhone(updatePhone);
						user.cuaeDevicename = devicename;
					}
					catch(Exception e)
					{
						ReportError(String.Format("Unable to update the phone {0} with line {1} for user {2}.  \n{3}", devicename, user.linenumber, user.username, GetDetail(e)));
					}
				}
			}

			return true;
		}

		private string CheckOutDevice()
		{
			string device = null;

			if(currentRange == null)
			{
				currentRange = FindNextRange();
			}

			if(currentRange != null)
			{
				ArrayList unusedDevices = unusedPhonesBeforeSync[currentRange] as ArrayList;

				if(unusedDevices.Count == 0)
				{
					unusedPhonesBeforeSync.Remove(currentRange);
					currentRange = FindNextRange();
					unusedDevices = unusedPhonesBeforeSync[currentRange] as ArrayList;
				}
			
				device = unusedDevices[0] as string;
				unusedDevices.RemoveAt(0);
			}

			return device;
		}

		private string FindNextRange()
		{
			string range = null;
			foreach(DictionaryEntry dictEntry in unusedPhonesBeforeSync)
			{
				range = dictEntry.Key as string;
				break;
			}

			if(range == null)
			{
				range = PromptNewRange();
				
				if(range != null)
				{
					long baseMacLong = long.Parse(range + "000000", System.Globalization.NumberStyles.HexNumber); 

					ArrayList unusedDevices = new ArrayList();
					for(int i = 0; i < numDevicesInRange; i++)
					{
						string deviceName = "SEP" + (baseMacLong + i).ToString("x").PadLeft(12, '0').ToUpper();
						unusedDevices.Add(deviceName);
					}

					unusedPhonesBeforeSync[range] = unusedDevices;
				}
			}
			return range;
		}

		private string PromptNewRange()
		{
			string newDeviceRange = null;
			while(true)
			{
				Console.Write("\nEnter a 6 digit device MAC start (FFFFFF, CCCCCC, ...):");
				newDeviceRange = Console.ReadLine().ToUpper();
				bool is6DigitHex =  true;
				is6DigitHex &= newDeviceRange.Length == 6;
				foreach(char character in newDeviceRange)
				{
					is6DigitHex &= ((character >= '0' && character <= '9') || (character >= 'A' && character <= 'F'));
				}

				if(is6DigitHex)
				{
					bool successfulRead = ReadVirtualDevicesFromCCM(new string[] {newDeviceRange});
					if(!successfulRead)
					{
						newDeviceRange = null;
					}
					break;
				}
			}

			return newDeviceRange;
		}

		#endregion

        public bool Generate(string[] baseMacs, ArrayList users, ArrayList devices, string description, 
			string css, string template, string outFile, int numDevicesInRange, bool readOnly, bool prompt)
        {
			this.prompt = prompt;
			this.css = css;
			this.phoneTemplateName = template;
			this.description = description;
			this.devicesFile = devices;
			this.numDevicesInRange = numDevicesInRange;
			this.readOnly = readOnly;
			this.baseMacs = baseMacs;

			bool success = ReadUsersFromDb(users);
			if(!success)
			{
				Console.WriteLine("Failed ReadUsersFromDB.  Exiting...");
				return false;
			}

			success = GetUsersCCMInfo(devices);
			if(!success)
			{
				Console.WriteLine("Failed GetUsersCCMInfo.  Exiting...");
				return false;
			}

			success = ReadVirtualDevicesFromCCM(baseMacs);
			if(!success)
			{
				Console.WriteLine("Failed ReadVirtualDevicesFromCCM.  Exiting...");
				return false;
			}

			DetermineAssociations();

			Synchronize();
			
			#region Output Errors/Results
			Console.WriteLine("Writing results and errors to c:\\errors.txt");
			using(FileStream stream = File.OpenWrite("c:\\errors.txt"))
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

			OutputErrors(outFile);
			OutputUsers(dbUsers, outFile);
			#endregion

			Console.WriteLine("Hit any key to exit...");
			Console.Read();
            return true;
        }

		#region OutputDevices
//		private void OutputDevices(ArrayList users, ArrayList devices, string outFile)
//		{
//			ArrayList sameLinePhones = new ArrayList();
//			ArrayList notAssociatedPhones = new ArrayList();
//
//			#region Same Line Phones
//			ArrayList toRemove = new ArrayList();
//			ArrayList duplicatedPhones = devices.Clone() as ArrayList;
//
//			while(true)
//			{
//				toRemove.Clear();
//
//				if(duplicatedPhones.Count == 0)
//				{
//					break;
//				}
//
//				foreach(Device device in duplicatedPhones)
//				{
//					string lineNumber = device.lineNumber;
//
//					toRemove.Add(device);
//
//					foreach(Device otherDevice in duplicatedPhones)
//					{
//						if(device.devicename == otherDevice.devicename)
//						{
//							continue;
//						}
//
//						if(String.Compare(lineNumber, otherDevice.lineNumber, true) == 0)
//						{
//							toRemove.Add(otherDevice);
//						}
//					}
//					break;
//				}
//
//				foreach(Device device in toRemove)
//				{
//					duplicatedPhones.Remove(device);
//				}
//
//				if(toRemove.Count > 1)
//				{
//					foreach(Device device in toRemove)
//					{
//						sameLinePhones.Add(device);
//					}
//				}
//			}
//			#endregion
//
//			#region Not Associated Phones
//			foreach(Device device in devices)
//			{
//				bool foundAssociation = false;
//				foreach(User user in users)
//				{
//					if(String.Compare(device.devicename, user.cuaeDevicename, true) == 0)
//					{
//						foundAssociation = true;
//						break;
//					}
//				}
//
//				if(!foundAssociation)
//				{
//					notAssociatedPhones.Add(device);
//				}
//			}
//			#endregion
//
//			string allPhonesFile = null;
//			allPhonesFile = Path.GetFileNameWithoutExtension(outFile);
//			allPhonesFile = allPhonesFile + "_allDevices" + Path.GetExtension(outFile);
//
//			Console.WriteLine("Writing all phones dump to " + allPhonesFile);
//			using(FileStream stream = File.Open(allPhonesFile, FileMode.Create))
//			{
//				using(StreamWriter writer = new StreamWriter(stream))
//				{
//					writer.WriteLine("{0},{1}", "devicename", "line");
//
//					foreach(Device device in devices)
//					{
//						writer.WriteLine(String.Format("{0},{1}", device.devicename, device.lineNumber));
//					}
//				}
//			}
//
//			string sameLinePhonesFile = null;
//			sameLinePhonesFile = Path.GetFileNameWithoutExtension(outFile);
//			sameLinePhonesFile = sameLinePhonesFile + "_sameLineDevices" + Path.GetExtension(outFile);
//
//			Console.WriteLine("Writing same line devices dump to " + sameLinePhonesFile);
//			using(FileStream stream = File.Open(sameLinePhonesFile, FileMode.Create))
//			{
//				using(StreamWriter writer = new StreamWriter(stream))
//				{
//					writer.WriteLine("{0},{1}", "devicename", "line");
//
//					foreach(Device device in sameLinePhones)
//					{
//						writer.WriteLine(String.Format("{0},{1}", device.devicename, device.lineNumber));
//					}
//				}
//			}
//
//			string notAssociatedPhonesFile = null;
//			notAssociatedPhonesFile = Path.GetFileNameWithoutExtension(outFile);
//			notAssociatedPhonesFile = notAssociatedPhonesFile + "_notAssociatedDevices" + Path.GetExtension(outFile);
//
//			Console.WriteLine("Writing not associated devices dump to " + notAssociatedPhonesFile);
//			using(FileStream stream = File.Open(notAssociatedPhonesFile, FileMode.Create))
//			{
//				using(StreamWriter writer = new StreamWriter(stream))
//				{
//					writer.WriteLine("{0},{1}", "devicename", "line");
//
//					foreach(Device device in notAssociatedPhones)
//					{
//						writer.WriteLine(String.Format("{0},{1}", device.devicename, device.lineNumber));
//					}
//				}
//			}
//		}
		#endregion

		private void OutputErrors(string outFile)
		{
//			private ArrayList virtual2LinePhones; // device
//			private ArrayList deviceChangedUsers; // user | appsuitedevice | devicefiledevice
//			private ArrayList invalidDeviceForUser; // user | device
//			private ArrayList noLineCCMDeviceForUser; // user | device
//			private ArrayList sccpPhonesWithUnusedLine; // device | line
//			private ArrayList dupDevices; // needsDeviceUser | hasDeviceUser | devicename
//			private ArrayList dupLines; // needsLineUser | hasLineUser | line number
			
			OutputFile(outFile, "virtual_multiline",		new string[] {"device"},			virtual2LinePhones);
			OutputFile(outFile, "real_invalid",				new string[] {"user", "device"},	invalidDeviceForUser);
			OutputFile(outFile, "appsuite_switcheddevice",	new string[] {"user", "appsuitedevice", "devicefiledevice"},	deviceChangedUsers);
			OutputFile(outFile, "real_noline",				new string[] {"user", "device"},	noLineCCMDeviceForUser);
			OutputFile(outFile, "virtual_unassociated",		new string[] {"device", "line"},	sccpPhonesWithUnusedLine);
			OutputFile(outFile, "appsuite_dupdevice",		new string[] {"needsDeviceUser", "hasDeviceUser", "device"},	dupDevices);
			OutputFile(outFile, "appsuite_dupline",			new string[] {"needsLineUser", "hasLineUser", "line"},	dupLines);

			ArrayList allSkippedUsers = new ArrayList();
			foreach(User user in dbUsers)
			{
				if(user.skip)
				{
					allSkippedUsers.Add(new string[] { user.username, user.skipReason });
				}
			}

			OutputFile(outFile, "skippedusers",				new string[] {"user", "skipreason"}, allSkippedUsers);
		}
		
		private void OutputFile(string outFile, string fileAppend, string[] headers, ArrayList items)
		{
			string file = null;
			file = Path.GetFileNameWithoutExtension(outFile);
			file = file + "_" + fileAppend + Path.GetExtension(outFile);

			using(FileStream stream = File.Open(file, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					// Write header
					string headerLine = String.Empty;
					foreach(string header in headers)
					{
						headerLine += header + ",";
					}
					headerLine.Remove(headerLine.Length - 1, 1);
					
					writer.WriteLine(headerLine);

					foreach(object obj in items)
					{
						if(obj is String)
						{
							writer.WriteLine(obj as string);
						}
						else 
						{
							string[] itemsList = obj as string[];
							
							string line = String.Empty;
							foreach(string item in itemsList)
							{
								line += item + ",";
							}
							line.Remove(line.Length - 1, 1);
							writer.WriteLine(line);
						}
					}
				}
			}
		}
		private void OutputUsers(ArrayList users, string outFile)
		{
			// Sort users
			ArrayList goodUsers = new ArrayList();
			ArrayList appsuiteadminBustedUsers = new ArrayList();
			ArrayList ccmBustedUsers = new ArrayList();

			foreach(User user in users)
			{
				if(user.devicename == null || user.devicename == String.Empty || user.linenumber == null || user.linenumber == String.Empty)
				{
					appsuiteadminBustedUsers.Add(user);
					continue;
				}
				else if(user.cuaeDevicename == null || user.cuaeDevicename == String.Empty)
				{
					ccmBustedUsers.Add(user);
					continue;
				}
				else
				{
					goodUsers.Add(user);
					continue;
				}
			}

			string allUsersFile = null;
			allUsersFile = Path.GetFileNameWithoutExtension(outFile);
			allUsersFile = allUsersFile + "_allusers" + Path.GetExtension(outFile);
			using(FileStream stream = File.Open(allUsersFile, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "username", "real", "virtual", "line", "Has11111", "first", "last");

					foreach(User user in users)
					{
						writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", user.username, user.devicename, user.cuaeDevicename, user.linenumber, user.hasDefaultDisabledFindme,  user.firstname, user.lastname));
					}
				}
			}

			string goodUsersFile = null;
			goodUsersFile = Path.GetFileNameWithoutExtension(outFile);
			goodUsersFile = goodUsersFile + "_goodusers" + Path.GetExtension(outFile);
			using(FileStream stream = File.Open(goodUsersFile, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "username", "real", "virtual", "line", "Has11111", "first", "last");

					foreach(User user in goodUsers)
					{
						writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", user.username, user.devicename, user.cuaeDevicename, user.linenumber, user.hasDefaultDisabledFindme,  user.firstname, user.lastname));
					}
				}
			}

			string ccmBadUsers = null;
			ccmBadUsers = Path.GetFileNameWithoutExtension(outFile);
			ccmBadUsers = ccmBadUsers + "_invalidusers" + Path.GetExtension(outFile);
			using(FileStream stream = File.Open(ccmBadUsers, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "username", "real", "virtual", "line", "Has11111", "first", "last");

					foreach(User user in ccmBustedUsers)
					{
						writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", user.username, user.devicename, user.cuaeDevicename, user.linenumber, user.hasDefaultDisabledFindme,  user.firstname, user.lastname));
					}
				}
			}

			string appsBadUsers = null;
			appsBadUsers = Path.GetFileNameWithoutExtension(outFile);
			appsBadUsers = appsBadUsers + "_missinginfousers" + Path.GetExtension(outFile);
			using(FileStream stream = File.Open(appsBadUsers, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "username", "real", "virtual", "line", "Has11111", "first", "last");

					foreach(User user in appsuiteadminBustedUsers)
					{
						writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", user.username, user.devicename, user.cuaeDevicename, user.linenumber, user.hasDefaultDisabledFindme,  user.firstname, user.lastname));
					}
				}
			}
		}

		private User FindUser(string pattern, ArrayList users)
		{
			foreach(User user in users)
			{
				if(user.linenumber == pattern)
				{
					return user;
				}
			}

			return null;
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

		private IDbConnection GetConnection()
		{
			IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN("application_suite", "127.0.0.1", 3306, "root", "metreos", true));
			try
			{
				connection.Open();
				return connection;
			}
			catch
			{
				Console.WriteLine("Unable to open a connection to {0}.  This tool must be run on the Application Server");
				return null;
			}
		}

		private bool AddDevice(IDbConnection connection, int userId, string devicename, string username)
		{
			try
			{
				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = String.Format("INSERT INTO as_phone_devices (as_users_id, is_primary_device, is_ip_phone, name, mac_address) VALUES ({0}, 1, 1, '{1}', '{2}')", userId, "Office Phone", devicename);
					command.ExecuteNonQuery();
					return true;
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to add device {0} to user {1}.  Exception: {2}", devicename, username, e));
			}

			return false;
		}

		private string GetDevice(IDbConnection connection, string devicename)
		{
			string username = null;
			try
			{
				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = String.Format("SELECT username FROM as_users a, as_phone_devices b WHERE a.status = 1 AND b.mac_address = '{0}' AND a.as_users_id = b.as_users_id", devicename);
					using(IDataReader reader = command.ExecuteReader())
					{
						if(reader.Read())
						{
							username = Convert.ToString(reader[0]);
						}
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to get device {0}.  Exception: {1}", devicename, e));
			}

			return username;
		}

		private bool RemoveDevice(IDbConnection connection, int userId, string devicename)
		{
			bool removed = true;
			try
			{
				using(IDbCommand command = connection.CreateCommand())
				{
					command.CommandText = String.Format("DELETE FROM as_phone_devices WHERE as_users_id = {0} AND mac_address = '{1}'", userId, devicename);
					int rowsAffected = command.ExecuteNonQuery();
					if(rowsAffected == 0)
					{
						removed = false;
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to get device {0}.  Exception: {1}", devicename, e));
				removed = false;
			}

			return removed;
		}

		private void AddDisabled(int userId, string username)
		{
			try
			{
				using(IDbConnection connection = GetConnection())
				{
					// check dummy number
					bool foundLine = false;
					string queryDummyFindme = String.Format("SELECT count(*) FROM as_external_numbers WHERE as_users_id = {0}", userId);
					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = queryDummyFindme;
						foundLine = Convert.ToInt32(command.ExecuteScalar()) > 0;
					}

					if(!foundLine)
					{
						string createDummyNumber = String.Format("INSERT INTO as_external_numbers (as_users_id, name, phone_number) VALUES ({0}, '{1}', '{2}')", userId, "AR Dummy Number", "11111");
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = createDummyNumber;
							command.ExecuteNonQuery();
						}
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to set disabled 11111 line on user {0}.  Exception: {1}", username, e));
			}
		}

		private bool AddLine(int userId, string username, string devicename, string pattern)
		{
			bool addedLine = false;
			try
			{
				using(IDbConnection connection = GetConnection())
				{
					int as_phone_devices_id = -1;

					using(IDbCommand command = connection.CreateCommand())
					{
						command.CommandText = String.Format("SELECT as_phone_devices_id FROM as_phone_devices WHERE as_users_id = {0} AND mac_address = '{1}'", userId, devicename);
						using(IDataReader reader = command.ExecuteReader())
						{
							if(reader.Read())
							{
								as_phone_devices_id = Convert.ToInt32(reader[0]);
							}
							else
							{
								ReportError(String.Format("Unable to get device {0} for user {1}", devicename, username));
							}
						}
					}

					if(as_phone_devices_id != -1)
					{
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = String.Format("DELETE FROM as_directory_numbers WHERE as_phone_devices_id = {0}", as_phone_devices_id);
							command.ExecuteNonQuery();
						}

						try
						{
							using(IDbCommand command = connection.CreateCommand())
							{
								command.CommandText = String.Format("INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number) VALUES ({0}, '{1}', {2})", as_phone_devices_id, pattern, 1);
								command.ExecuteNonQuery();
								addedLine = true;
							}
						}
						catch
						{
							string otherUser = null;

							using(IDbCommand command = connection.CreateCommand())
							{
								command.CommandText = String.Format("SELECT username FROM as_users a, as_phone_devices b, as_directory_numbers c WHERE a.status = 1 AND a.as_users_id = b.as_users_id AND b.as_phone_devices_id = c.as_phone_devices_id AND c.directory_number = '{0}'", pattern);
								using(IDataReader reader = command.ExecuteReader())
								{
									if(reader.Read())
									{
										otherUser = Convert.ToString(reader[0]);
									}
								}
							}
							
							if(otherUser != null)
							{
								Console.WriteLine("Duplicate line entry found between user: {0} and user: {1}. Problem number:  {2}", username, otherUser, pattern); 
								dupLines.Add(new string[] {username, otherUser, pattern});
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				ReportError(String.Format("Unable to add the line {0} to device {1} for user {2}.  Exception: e", pattern, devicename, username, e));
			}

			return addedLine;
		}
	}
}
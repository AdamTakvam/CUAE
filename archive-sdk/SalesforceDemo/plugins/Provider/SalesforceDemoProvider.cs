using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Data; 
using System.Threading;

using MySql.Data.MySqlClient;

using Metreos.Core;            
using Metreos.ProviderFramework;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;



namespace Metreos.Providers.SalesforceDemo
{
	[ProviderDecl("Salesforce Demo Provider")] 
	[PackageDecl("Metreos.Providers.SalesforceDemo", "Communications with the Salesforce client and CUAE applications")]
	public class SalesforceProvider : ProviderBase 
	{
		/// <summary>
		///		This allows the provider to know if it has been configured enough such that it should attempt to poll the SF database 
		///		The username & passwords for the Salesforce API and local database both must be set before operation can occur
		///		for the SF caching feature
		/// </summary>
		protected bool IsCachingConfigured
		{ 
			get 
			{ 
				return 
					sfUsername != null && sfUsername != String.Empty && 
					sfPassword != null && sfPassword != String.Empty &&
					localUsername != null && localUsername != String.Empty && 
					localPassword != null && localPassword != String.Empty;
			}
		}

		#region Data Structs
		public class ContactInfo
		{
			public string ContactSFId;
			public string FirstName;
			public string LastName;
			public string PhoneNumber;
			public string Street;
			public string City;
			public string State;
			public string PostalCode;
			public string Country;
			public string Latitude;
			public string Longitude;
			public string AccountSFId;
			public string AccountName;
			public string AccountType;

			public ContactInfo()
			{
				this.ContactSFId = String.Empty;
				this.FirstName = String.Empty;
				this.LastName = String.Empty;
				this.PhoneNumber = String.Empty;
				this.Street = String.Empty;
				this.City = String.Empty;
				this.State = String.Empty;
				this.PostalCode = String.Empty;
				this.Country = String.Empty;
				this.Latitude = String.Empty;
				this.Longitude = String.Empty;
				this.AccountSFId = String.Empty;
				this.AccountName = String.Empty;
				this.AccountType = String.Empty;
			}
		}
		#endregion

		protected const string ProviderNamespace = "Metreos.Providers.SalesforceDemo"; 
		protected IpcListener server;
		protected Thread sfPollThread;
		protected IDbConnection cacheConnection;
		protected bool ending;

		protected int pollMins;
		protected string sfUsername;
		protected string sfPassword;
		protected string localUsername;
		protected string localPassword;
		protected string proxyUrl;
		protected DateTime lastPoll;
		protected bool isStarted;
		protected System.Threading.Timer pollTimer;

		protected string GetAllAccounts = "select Id, Name, Type, FirstName, LastName, Phone, BillingStreet, BillingCity, BillingState, BillingPostalCode, BillingCountry, Latitude__c, Longitude__c from Account a";
		protected string GetAllContacts = "select Id, AccountId, FirstName, LastName, Phone, MailingStreet, MailingCity, MailingState, MailingPostalCode, MailingCountry, Latitude__c, Longitude__c from Contact c";
	
		public SalesforceProvider(IConfigUtility configUtility) 
			: base(typeof(SalesforceProvider), "Salesforce Demo Provider", configUtility)
		{
			server = null;
			
			sfUsername = null;
			sfPassword = null;
			localUsername = null;
			localPassword = null;
			proxyUrl = null;
			pollMins = -1;
			lastPoll = DateTime.MinValue;
			ending = false;
			isStarted = false;
			sfPollThread = new Thread(new ThreadStart(SyncWorker));
			sfPollThread.IsBackground = true;
			pollTimer = new Timer(new TimerCallback(Poll), null, TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
		}

		#region ProviderBase Implementation
		protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
		{
			configItems = new ConfigEntry[6];
			ConfigEntry sfPollTime = new ConfigEntry("SfPollInterval", "Salesforce.com Poll Interval", "1440", 
				"Salesforce Database poll interval (min)", IConfig.StandardFormat.Number, true);
			ConfigEntry sfUsername = new ConfigEntry("SfUsername", "Salesforce.com API Username", String.Empty,
				"The username to access the Salesforce API", IConfig.StandardFormat.String, true);
			ConfigEntry sfPassword= new ConfigEntry("SfPassword", "Salesforce.com API Password", String.Empty,
				"The password to access the Salesforce API", IConfig.StandardFormat.Password, true);
			ConfigEntry proxyUrl = new ConfigEntry("ProxyURL", "Proxy URL for external HTTP access", String.Empty,
				"A URL to a proxy server, if required to gain access to internet", IConfig.StandardFormat.String, false);
			ConfigEntry localDbUsername= new ConfigEntry("LocalDbUsername", "Local Database Username", String.Empty,
				"The username to access the local CUAE database", IConfig.StandardFormat.String, true);
			ConfigEntry localDbPassword= new ConfigEntry("LocalDbPassword", "Local Database Password", String.Empty,
				"The password to access the local CUAE database", IConfig.StandardFormat.Password, true);
		
			configItems[0] = sfPollTime;
			configItems[1] = sfUsername;
			configItems[2] = sfPassword;
			configItems[3] = proxyUrl;
			configItems[4] = localDbUsername;
			configItems[5] = localDbPassword;

			extensions = new Extension[1];;   // you will most likely remove these when implementing your own
			Extension invokePoll = new Extension("Metreos.Providers.SalesforceDemo.Poll", "Invoke the Salesforce.com data poll manually");

			extensions[0] = invokePoll;

			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyIncomingCall", 
				new HandleMessageDelegate(this.NotifyIncomingCall));
			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyCallActive", 
				new HandleMessageDelegate(this.NotifyCallActive));
			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyCallInactive", 
				new HandleMessageDelegate(this.NotifyCallInactive));
			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyHangup", 
				new HandleMessageDelegate(this.NotifyHangup));
			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyInitiate", 
				new HandleMessageDelegate(this.NotifyInitiate));
			this.messageCallbacks.Add(ProviderNamespace + "." + "NotifyLogin", 
				new HandleMessageDelegate(this.NotifyLogin));
			this.messageCallbacks.Add(ProviderNamespace + "." + "CustomerLookup", 
				new HandleMessageDelegate(this.CustomerLookup));
			this.messageCallbacks.Add(ProviderNamespace + "." + "GetResellerContacts", 
				new HandleMessageDelegate(this.GetResellerContacts));
			this.messageCallbacks.Add(ProviderNamespace + "." + "ConferenceInitiated", 
				new HandleMessageDelegate(this.ConferenceInitiated));
			this.messageCallbacks.Add(ProviderNamespace + "." + "Poll", 
				new HandleMessageDelegate(this.InitiatePoll));

			return true;
		}


		protected override void RefreshConfiguration()
		{		
			int pollTime = this.pollMins; 
			string salesforceUsername = this.sfUsername;
			string salesforcePassword = this.sfPassword;
			string dbUsername = this.localUsername;
			string dbPassword = this.localPassword;


			this.pollMins = (int) this.GetConfigValue("SfPollInterval");
			this.sfUsername = this.GetConfigValue("SfUsername") as string;
			this.sfPassword = this.GetConfigValue("SfPassword") as string;
			this.localUsername = this.GetConfigValue("LocalDbUsername") as string;
			this.localPassword = this.GetConfigValue("LocalDbPassword") as string;
			this.proxyUrl = this.GetConfigValue("ProxyURL") as string;

			if(String.Compare(dbUsername, this.localUsername, false) != 0 ||
				String.Compare(dbPassword, this.localPassword, false) != 0)
			{
				// local db settings changed.  Create new connection
				if(cacheConnection != null)
				{
					cacheConnection.Close();
					cacheConnection.Dispose();
					cacheConnection = null;
				}

				if(IsCachingConfigured)
				{
					// Reconfigure database connection

					// check to see if the sf cache db exists
					bool dbKnownToExist = SalesforceDbExists();

					if(!dbKnownToExist)
					{
						dbKnownToExist = CreateCacheDatabase();
					}

					if(dbKnownToExist)
					{
						try
						{
							cacheConnection = CreateConnection();
						}
						catch(Exception e)
						{
							log.Write(TraceLevel.Error, "Unable to create a connection to the salesforce cache database. {0}", Metreos.Utilities.Exceptions.FormatException(e));					
						}
					}
				}
			}

			if(pollMins != pollTime)
			{
				// adjust timer
				if(isStarted)
				{
					TimeSpan newPeriod = TimeSpan.FromMinutes(pollMins);

					if(lastPoll != DateTime.MinValue)
					{
						TimeSpan elapsedSoFar = DateTime.Now.Subtract(lastPoll);
						if(newPeriod < elapsedSoFar)
						{
							// we are past due for a sync--so invoke one, and fix timer
							lock(this)
							{
								Monitor.Pulse(this);
							}

							pollTimer.Change(newPeriod, newPeriod);
						}
						else
						{
							// Figure out the rest of the time to wait, then fix timer
							TimeSpan untilNextPoll = newPeriod.Subtract(elapsedSoFar);

							pollTimer.Change(untilNextPoll , newPeriod);
						}
					}
					else
					{
						// this is the first time the poll logic has been executed
						lock(this)
						{
							Monitor.Pulse(this);
						}
						pollTimer.Change(newPeriod, newPeriod);
					}
				}
			}
			else
			{
				// timer is correct
			}
		}

		#region Salesforce Sync Logic
		private void SyncWorker()
		{
			bool firstTime = true; // first time logic boolean not so clean--it's here only 
			// to make the logic in on Startup cleaner
			while(true)
			{
				if(!firstTime)
				{
					lock(this)
					{
						Monitor.Wait(this);
					}
				}
				else
				{
					firstTime = false;
				}

				if(!ending)
				{
					SyncSalesforce();
				}
				else
				{
					break;
				}
			}
		}

		private void SyncSalesforce()
		{
			if(IsCachingConfigured)
			{
				QueryResult accountQueryResult;
				QueryResult contactQueryResult;

				if(RetrieveDataFromSalesforce(out accountQueryResult, out contactQueryResult))
				{
					PopulateSalesforceCache(accountQueryResult, contactQueryResult);
				}
			}
		}

		private bool RetrieveDataFromSalesforce(out QueryResult accountQueryResult, out QueryResult contactQueryResult)
		{
			bool querySuccess = false;

			using(SforceService service = new SforceService()) // should probably pool connections?
			{
				if (proxyUrl != null && proxyUrl != String.Empty)
				{
					service.Proxy = new System.Net.WebProxy(proxyUrl);
				}

				// Login to Salesforce
				LoginResult loginResult = null;
				
				log.Write(TraceLevel.Verbose, "Logging in to Salesforce");

				bool loginAttemptSuccess = true;
				bool loggedIn = true;

				try
				{
					loginResult = service.login(sfUsername, sfPassword);
				}
				catch(Exception e)
				{
					log.Write(TraceLevel.Error, "Unable to log into APEX API. {0}", e);
					loginAttemptSuccess = false;
				}

				if(loginResult == null)
				{
					log.Write(TraceLevel.Error, "Received null login result message.");
					loginAttemptSuccess = false;
				}

				if(loginAttemptSuccess)
				{
					if(loginResult.passwordExpired)
					{
						log.Write(TraceLevel.Error, "Apex Password has expired.  Can not log into APEX API");
						loggedIn = false;
					}
					else if(loginResult.sessionId == null || loginResult.sessionId == String.Empty)
					{
						log.Write(TraceLevel.Error, "Unable to log in to APEX API due to malformed sessionId");
						loggedIn = false;
					}
				}
				else
				{
					loggedIn = false;
				}

				accountQueryResult = null;
				contactQueryResult = null;

				if(loggedIn)
				{
					service.Url = loginResult.serverUrl;
					service.SessionHeaderValue = new SessionHeader();
					service.SessionHeaderValue.sessionId = loginResult.sessionId;

					log.Write(System.Diagnostics.TraceLevel.Verbose, "Executing full Accounts query");

					string query = GetAllAccounts;
					try
					{
						accountQueryResult = service.query(query);
					}
					catch(Exception e)
					{
						log.Write(TraceLevel.Error, "Unable to query Salesforce accounts with query: {0}. {1}", query, e);
						querySuccess = false;
					}

					if(accountQueryResult == null)
					{
						log.Write(TraceLevel.Error, "Received null query result message when retrieving accounts");
						querySuccess = false;
					}
					else
					{
						querySuccess = true;
					}

					if(querySuccess)
					{
						query = GetAllContacts;
						try
						{
							contactQueryResult = service.query(query);
						}
						catch(Exception e)
						{
							log.Write(TraceLevel.Error, "Unable to query Salesforce contacts with query: {0}. {1}", query, e);
							querySuccess = false;
						}

						if(contactQueryResult == null)
						{
							log.Write(TraceLevel.Error, "Received null query result message when retrieving contacts");
							querySuccess = false;
						}

					}

				}
			}

			return querySuccess;
		}

		private void PopulateSalesforceCache(QueryResult accountQueryResult, QueryResult contactQueryResult)
		{
			IDbTransaction transaction = null;
			try
			{
				log.Write(TraceLevel.Info, "Beginning synchronization of Salesforce.com to local database");
				if(cacheConnection.State == ConnectionState.Closed)
				{
					cacheConnection.Open();
				}
				else
				{
					log.Write(TraceLevel.Warning, "The cache connection was already open in sync logic");
				}
				transaction = cacheConnection.BeginTransaction();

				using(MySqlCommand command = transaction.Connection.CreateCommand() as MySqlCommand)
				{
					// first remove all rows from contact and accounts
					// remove contacts first, as they have foreign key references

					command.CommandText = "DELETE from contacts";
					command.ExecuteNonQuery();

					command.CommandText = "DELETE from accounts";
					command.ExecuteNonQuery();

					if(accountQueryResult.size > 0)
					{
						for(int i = 0; i < accountQueryResult.records.Length; i++)
						{
							Account account = accountQueryResult.records[i] as Account;

//							string insertCommand = String.Format(@"
//								INSERT INTO accounts 
//									(sfId, accountName, accountType, phoneNumber, street, city, state, zip, country, latitude, longitude) 
//								VALUES
//									('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", 
//								account.Id, account.Name, account.Type, account.Phone, account.BillingStreet, account.BillingCity, 
//								account.BillingState, account.BillingPostalCode, account.BillingCountry, account.Latitude__c, account.Longitude__c);

						string insertCommand = @"
								INSERT INTO accounts 
									(sfId, accountName, accountType, phoneNumber, street, city, state, zip, country, latitude, longitude) 
								VALUES
									(?sfId, ?accountName, ?accountType, ?phoneNumber, ?street, ?city, ?state, ?zip, ?country, ?latitude, ?longitude)";

							command.CommandText = insertCommand;
							command.Parameters.Add("?sfId", MySqlDbType.VarChar).Value = account.Id;
							command.Parameters.Add("?accountName", MySqlDbType.VarChar).Value = (account.Name == null ? String.Empty : account.Name);
							command.Parameters.Add("?accountType", MySqlDbType.VarChar).Value = (account.Type == null ? String.Empty : account.Type);
							command.Parameters.Add("?phoneNumber", MySqlDbType.VarChar).Value = (account.Phone == null ? String.Empty : account.Phone);
							command.Parameters.Add("?street", MySqlDbType.VarChar).Value = (account.BillingStreet == null ? String.Empty : account.BillingStreet);
							command.Parameters.Add("?city", MySqlDbType.VarChar).Value = (account.BillingCity == null ? String.Empty : account.BillingCity);
							command.Parameters.Add("?state", MySqlDbType.VarChar).Value = (account.BillingState == null ? String.Empty : account.BillingState);
							command.Parameters.Add("?zip", MySqlDbType.VarChar).Value = (account.BillingPostalCode == null ? String.Empty : account.BillingPostalCode);
							command.Parameters.Add("?country", MySqlDbType.VarChar).Value = (account.BillingCountry == null ? String.Empty : account.BillingCountry);
							command.Parameters.Add("?latitude", MySqlDbType.VarChar).Value = account.Latitude__c.ToString();
							command.Parameters.Add("?longitude", MySqlDbType.VarChar).Value = account.Longitude__c.ToString();
							command.ExecuteNonQuery();
							command.Parameters.Clear();

						}
					}

					if(contactQueryResult.size > 0)
					{
						
						for(int i = 0; i < contactQueryResult.records.Length; i++)
						{
							Contact contact = contactQueryResult.records[i] as Contact;

//							string insertCommand = String.Format(@"
//								INSERT INTO contacts 
//									(sfId, firstname, lastname, phoneNumber, street, city, state, zip, country, latitude, longitude, account_id) 
//								VALUES
//									('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')", 
//								contact.Id, contact.FirstName, contact.LastName, contact.Phone, contact.MailingStreet, contact.MailingCity, 
//								contact.MailingState, contact.MailingPostalCode, contact.MailingCountry, contact.Latitude__c, contact.Longitude__c, contact.AccountId);

							string insertCommand = @"
								INSERT INTO contacts 
									(sfId, firstname, lastname, phoneNumber, street, city, state, zip, country, latitude, longitude, account_id) 
								VALUES
									(?sfId, ?firstname, ?lastname, ?phoneNumber, ?street, ?city, ?state, ?zip, ?country, ?latitude, ?longitude, ?account_id)";
						
							// can't submit nulls for some reason with parameterized
							command.CommandText = insertCommand;
							command.Parameters.Add("?sfId", MySqlDbType.VarChar).Value = contact.Id;
							command.Parameters.Add("?firstname", MySqlDbType.VarChar).Value = (contact.FirstName == null ? String.Empty : contact.FirstName);
							command.Parameters.Add("?lastname", MySqlDbType.VarChar).Value = (contact.LastName == null ? String.Empty : contact.LastName);
							command.Parameters.Add("?phoneNumber", MySqlDbType.VarChar).Value = (contact.Phone == null ? String.Empty : contact.Phone);
							command.Parameters.Add("?street", MySqlDbType.VarChar).Value = (contact.MailingStreet == null ? String.Empty : contact.MailingStreet);
							command.Parameters.Add("?city", MySqlDbType.VarChar).Value = (contact.MailingCity == null ? String.Empty : contact.MailingCity);
							command.Parameters.Add("?state", MySqlDbType.VarChar).Value = (contact.MailingState == null ? String.Empty : contact.MailingState);
							command.Parameters.Add("?zip", MySqlDbType.VarChar).Value = (contact.MailingPostalCode == null ? String.Empty : contact.MailingPostalCode);
							command.Parameters.Add("?country", MySqlDbType.VarChar).Value = (contact.MailingCountry == null ? String.Empty : contact.MailingCountry);
							command.Parameters.Add("?latitude", MySqlDbType.VarChar).Value = contact.Latitude__c.ToString();
							command.Parameters.Add("?longitude", MySqlDbType.VarChar).Value = contact.Longitude__c.ToString();
							command.Parameters.Add("?account_id", MySqlDbType.VarChar).Value = (contact.AccountId == null ? String.Empty : contact.AccountId);
							command.ExecuteNonQuery();
							command.Parameters.Clear();

						}
					}
				}

				transaction.Commit();

				log.Write(TraceLevel.Info, "Ended synchronization of Salesforce.com to local database successfully");

			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Unexpected error encountered when synchronizing database.  No data was written for this poll. {0}", Metreos.Utilities.Exceptions.FormatException(e));
				if(transaction != null)
				{
					transaction.Rollback();
				}
			}
			finally
			{
				if(transaction != null)
				{
					transaction.Dispose();
					transaction = null;
				}

				try
				{
					cacheConnection.Close();
				}
				catch{}
			}
		}

		#region SF Cache Database Operations

		protected bool SalesforceDbExists()
		{
			string dsn = Database.FormatDSN("SalesforceCache", "127.0.0.1", 3306, localUsername, localPassword, true);

			try
			{
				using(IDbConnection testDb = Database.CreateConnection(Database.DbType.mysql, dsn))
				{
					testDb.Open();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}


		private void Poll(object state)
		{
			if(Monitor.TryEnter(this, TimeSpan.FromMinutes(5)))
			{
				Monitor.Pulse(this);
				Monitor.Exit(this);
			}
			else
			{
				log.Write(TraceLevel.Error, "Unable to acquire lock to request poll of SF synchronization logic");
			}
		}

		#endregion

		protected IDbConnection CreateConnection()
		{
			string dsn = Database.FormatDSN("SalesforceCache", "127.0.0.1", 3306, localUsername, localPassword, true);

			return Database.CreateConnection(Database.DbType.mysql, dsn);
		}



		protected bool CreateCacheDatabase()
		{
			bool success = false;
			try
			{
				using(IDbConnection connection = new MySql.Data.MySqlClient.MySqlConnection(
					String.Format("Database=mysql; Data Source=localhost;UserId={0}; PWD={1};", localUsername, localPassword)))
				{
					if(connection != null)
					{
						// create format of tables

						string accountTable = @"
						CREATE TABLE accounts(
							id INT(10) unsigned NOT NULL auto_increment,
							sfId VARCHAR(50) NOT NULL,
							accountName VARCHAR(50) NOT NULL DEFAULT '',
							accountType VARCHAR(50) NOT NULL DEFAULT '',
							phoneNumber VARCHAR(50) NOT NULL DEFAULT '',
							street VARCHAR(50) NOT NULL DEFAULT '',
							city VARCHAR(50) NOT NULL DEFAULT '',
							state VARCHAR(50) NOT NULL DEFAULT '',
							zip VARCHAR(50) NOT NULL DEFAULT '',
							country VARCHAR(50) NOT NULL DEFAULT '',
							latitude VARCHAR(50) NOT NULL DEFAULT '',
							longitude VARCHAR(50) NOT NULL DEFAULT '',
							PRIMARY KEY(id)
						);";

						string contactTable = @"
						CREATE TABLE contacts(
							id INT(10) unsigned NOT NULL auto_increment,
							sfId VARCHAR(50) NOT NULL,
							firstname VARCHAR(50) NOT NULL DEFAULT '',
							lastname VARCHAR(50) NOT NULL DEFAULT '',
							phoneNumber VARCHAR(50) NOT NULL DEFAULT '',
							street VARCHAR(50) NOT NULL DEFAULT '',
							city VARCHAR(50) NOT NULL DEFAULT '',
							state VARCHAR(50) NOT NULL DEFAULT '',
							zip VARCHAR(50) NOT NULL DEFAULT '',
							country VARCHAR(50) NOT NULL DEFAULT '',
							latitude VARCHAR(50) NOT NULL DEFAULT '',
							longitude VARCHAR(50) NOT NULL DEFAULT '',
							account_id VARCHAR(50) NOT NULL,
							PRIMARY KEY(id)
							);";
						connection.Open();
						using(IDbCommand command = connection.CreateCommand())
						{
							command.CommandText = "CREATE DATABASE SalesforceCache";
							command.ExecuteNonQuery();

							command.CommandText = "USE SalesforceCache";
							command.ExecuteNonQuery();

							command.CommandText = accountTable;
							command.ExecuteNonQuery();

							command.CommandText = contactTable;
							command.ExecuteNonQuery();
						}
					}

					success = true;
				}

			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Unable to create salesforce database. {0}", Metreos.Utilities.Exceptions.FormatException(e));
			}

			return success;
		}

		#endregion

        /// <summary>
        ///     If this method fires, then an event we fired was handled by no application.
        /// </summary>
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {      

        }

        /// <summary>
        ///     * Runs in "your" thread – not the application manager – 
        ///           so that it doesn't slow down the startup of other providers.
        ///     * You must call RegisterNamespace() here or applications can not use the actions of this provider.
        ///     * Perform possibly time-consuming actions, e.g., initializing stack.
        ///     * Note: Your provider should not send any events 
        ///           (and will not receive any actions) until this method completes.
        /// </summary>
        protected override void OnStartup()
        {
			isStarted = true;
			ending = false;

            RegisterNamespace();

			server = new IpcListener(10000, this.LogLevel);
			
			// Salesforce Client related
			server.MakeCallRequest += new Metreos.Providers.SalesforceDemo.IpcListener.MakeCallRequestDelegate(MakeCallRequest);
			server.HangupRequest += new Metreos.Providers.SalesforceDemo.IpcListener.HangupRequestDelegate(HangupRequest);
			server.AnswerRequest += new Metreos.Providers.SalesforceDemo.IpcListener.AnswerRequestDelegate(AnswerRequest);
			server.LoginRequest += new Metreos.Providers.SalesforceDemo.IpcListener.LoginDelegate(LoginRequest);
			
			// Cuae Client related
			server.ConferenceRequest += new Metreos.Providers.SalesforceDemo.IpcListener.ConferenceRequestDelegate(ConferenceRequest);

			server.Start();

			// start sync thread
			sfPollThread.Start(); // this thread will, due to 'first time' logic in worker thread, to the first SF database sync
			TimeSpan period = TimeSpan.FromMinutes(this.pollMins);
			pollTimer.Change(period, period); // set up the periodic timer
		
            base.OnStartup();
        }

        /// <summary>
        ///     Guaranteed to be called on a graceful shutdown of the Application Server
        /// </summary>
        protected override void OnShutdown()
        {
			isStarted = false;
			ending = true;
			lock(this)
			{
				Monitor.Pulse(this);
			}
			server.Stop();
        }

        #endregion

        #region Provider Events

		[Event(ProviderNamespace + "." + "MakeCallRequest", true, null, "MakeCall Request", "An outbound call was requested by the client")]
		[EventParam("To", typeof(System.String), true, "The number to call")]
		[EventParam("From", typeof(System.String), true, "The number of the line to call from")]
		[EventParam("DeviceName", typeof(System.String), true, "The device to call from")]
		private string MakeCallRequest(string to, string from, string deviceName)
		{
			string scriptId = System.Guid.NewGuid().ToString();

			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "MakeCallRequest", 
				EventMessage.EventType.Triggering, 
				scriptId);

			msg.AddField("To", to);
			msg.AddField("From", from);
			msg.AddField("DeviceName", deviceName); 
			palWriter.PostMessage(msg);
			
			return scriptId;
		}

		[Event(ProviderNamespace + "." + "HangupRequest", false, null, "Hangup Request", "An client has requested to hangup a call")]
		[EventParam("CallId", typeof(System.String), true, "The callId of the call to hangup")]
		private bool HangupRequest(string routingGuid, string callId)
		{
			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "HangupRequest",
				EventMessage.EventType.NonTriggering,
				routingGuid);
			msg.AddField("CallId", callId);

			palWriter.PostMessage(msg);

			// Probably doesn't make sense to return bool here
			return true;
		}

		[Event(ProviderNamespace + "." + "AnswerRequest", false, null, "Answer Request", "An client has requested to answer a call")]
		private bool AnswerRequest(string routingGuid)
		{
			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "AnswerRequest",
				EventMessage.EventType.NonTriggering,
				routingGuid);

			palWriter.PostMessage(msg);

			// Probably doesn't make sense to return bool here
			return true;
		}

		
		[Event(ProviderNamespace + "." + "LoginRequest", true, null, "Login Request", "An client has requested to login")]
		[EventParam("DeviceName", typeof(System.String), true, "The name of the device being logged into")]
		private bool LoginRequest(string deviceName)
		{
			string scriptId = System.Guid.NewGuid().ToString();
			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "LoginRequest",
				EventMessage.EventType.Triggering,
				scriptId);
			msg.AddField("DeviceName", deviceName);

			palWriter.PostMessage(msg);

			return false;
		}

        #endregion

        #region Actions
        
		[Action(ProviderNamespace + "." + "NotifyIncomingCall", false, "NotifyIncomingCall", "Notify a subscriber of an incoming call", false)]
		[ActionParam("To", typeof(string), true, false, "Final Called Party Number")]
		[ActionParam("From", typeof(string), false, false, "Calling Party Number")]
		[ActionParam("OriginalTo", typeof(string), true, false, "Original Called Party Number")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("CallId", typeof(string), true, false, "The ID of the call")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyIncomingCall(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string to = null;
			string from = null;
			string originalTo = null;
			string deviceName = null;
			string callId = null;
			action.InnerMessage.GetString("To", true, String.Empty, out to);
			action.InnerMessage.GetString("From", false, String.Empty, out from);
			action.InnerMessage.GetString("OriginalTo", true, String.Empty, out originalTo);
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("CallId", true, String.Empty, out callId);

			ContactInfo caller = new ContactInfo(); 
			ArrayList resellers = new ArrayList();

			#region Query for caller and resellers
			// dip into Salesforce cache for customer and reseller info
			if(cacheConnection != null)
			{
				try
				{
					if(cacheConnection.State == ConnectionState.Closed) 
					{
						cacheConnection.Open();
					}
					else
					{
						// this shouldn''t happen...
						log.Write(TraceLevel.Warning, "The cacheConnection is already open");
					}

					using(IDbCommand command = cacheConnection.CreateCommand())
					{
						// should be using parameterized
						command.CommandText = String.Format(@"
							SELECT 
								c.sfId, c.firstname, c.lastname, c.phoneNumber, c.street, c.city, c.state, 
								c.zip, c.country, c.latitude, c.longitude, a.sfId, a.accountName, a.accountType 
							FROM 
								contacts c, accounts a
							WHERE
								c.phoneNumber = '{0}' 
								AND
								c.account_id = a.sfId", from);

						log.Write(TraceLevel.Verbose, "Finding caller with query:\n{0}", command.CommandText);

						using(IDataReader reader = command.ExecuteReader())
						{
							// doesn't handle multiple returns currently
							if(reader.Read())
							{
								caller.ContactSFId = reader[0] as string;
								caller.FirstName = reader[1] as string;
								caller.LastName = reader[2] as string;
								caller.PhoneNumber = reader[3] as string;
								caller.Street = reader[4] as string;
								caller.City = reader[5] as string;
								caller.State = reader[6] as string;
								caller.PostalCode = reader[7] as string;
								caller.Country = reader[8] as string;
								caller.Latitude = reader[9] as string;
								caller.Longitude = reader[10] as string;
								caller.AccountSFId = reader[11] as string;
								caller.AccountName = reader[12] as string;
								caller.AccountType = reader[13] as string;
							}
						}


						command.CommandText = @"
							SELECT 
								c.sfId, c.firstname, c.lastname, c.phoneNumber, c.street, c.city, c.state, 
								c.zip, c.country, c.latitude, c.longitude, a.sfId, a.accountName, a.accountType 
							FROM 
								contacts c, accounts a
							WHERE
								a.accountType = 'Reseller'
								AND
								c.account_id = a.sfId";

						using(IDataReader reader = command.ExecuteReader())
						{
							// doesn't handle multiple returns currently
							while(reader.Read())
							{
								ContactInfo resellerInfo = new ContactInfo();
								resellerInfo.ContactSFId = reader[0] as string;
								resellerInfo.FirstName = reader[1] as string;
								resellerInfo.LastName = reader[2] as string;
								resellerInfo.PhoneNumber = reader[3] as string;
								resellerInfo.Street = reader[4] as string;
								resellerInfo.City = reader[5] as string;
								resellerInfo.State = reader[6] as string;
								resellerInfo.PostalCode = reader[7] as string;
								resellerInfo.Country = reader[8] as string;
								resellerInfo.Latitude = reader[9] as string;
								resellerInfo.Longitude = reader[10] as string;
								resellerInfo.AccountSFId = reader[11] as string;
								resellerInfo.AccountName = reader[12] as string;
								resellerInfo.AccountType = reader[13] as string;
								resellers.Add(resellerInfo);
							}
						}

					}
				}
				catch(Exception e)
				{
					log.Write(TraceLevel.Error, "Unable to query the local Salesforce.com database for contact and reseller info.  {0}", Metreos.Utilities.Exceptions.FormatException(e));
				}
			}
			#endregion

			server.SendIncomingCalltoCuaeClient(action.RoutingGuid, deviceName, from, callId, caller, resellers);

			bool isSubscriber = server.SendIncomingCall(action.RoutingGuid, deviceName, to, from, originalTo, callId);


			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}

		[Action(ProviderNamespace + "." + "NotifyCallActive", false, "NotifyCallActive", "Notify a subscriber of the call going active", false)]
		[ActionParam("To", typeof(string), true, false, "Final Called Party Number")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("CallId", typeof(string), true, false, "The ID of the call")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyCallActive(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string to = null;
			string deviceName = null;
			string callId = null;
			action.InnerMessage.GetString("To", true, String.Empty, out to);
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("CallId", true, String.Empty, out callId);

			bool isSubscriber = server.SendCallActive(deviceName, to, callId);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}

		[Action(ProviderNamespace + "." + "NotifyCallInactive", false, "NotifyCallInactive", "Notify a subscriber of the call going inactive", false)]
		[ActionParam("InUse", typeof(bool), true, false, "Is the call in use by remote party")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("CallId", typeof(string), true, false, "The ID of the call")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyCallInactive(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			bool inUse = false;
			string deviceName = null;
			string callId = null;
			action.InnerMessage.GetBoolean("InUse", true, false, out inUse);
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("CallId", true, String.Empty, out callId);

			bool isSubscriber = server.SendCallInactive(deviceName, inUse, callId);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}

		[Action(ProviderNamespace + "." + "NotifyHangup", false, "NotifyHangup", "Notify a subscriber of the call being hungup", false)]
		[ActionParam("Cause", typeof(string), true, false, "The end reason of the call")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("CallId", typeof(string), true, false, "The ID of the call")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyHangup(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string cause = null;
			string deviceName = null;
			string callId = null;
			action.InnerMessage.GetString("Cause", true, String.Empty, out cause);
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("CallId", true, String.Empty, out callId);

			bool isSubscriber = server.SendHangup(deviceName, cause, callId);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}	

		[Action(ProviderNamespace + "." + "NotifyInitiate", false, "NotifyInitiate", "Notify a subscriber of the call being initiated", false)]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("CallId", typeof(string), true, false, "The ID of the call")]
		[ActionParam("To", typeof(string), true, false, "Called party number")]
		[ActionParam("From", typeof(string), true, false, "Calling party number")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyInitiate(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string deviceName = null;
			string to = null; //"1000"; // Why oh why is this busted in JTAPI :(
			string from = null;
			string callId = null;

			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("CallId", true, String.Empty, out callId);
			action.InnerMessage.GetString("To", true, String.Empty, out to);
			action.InnerMessage.GetString("From", true, String.Empty, out from);

			bool isSubscriber = server.SendInitiateCall(action.RoutingGuid, callId, deviceName, to, from);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}	

		[Action(ProviderNamespace + "." + "NotifyLogin", false, "NotifyLogin", "Notify a subscriber of the results of the login", false)]
		[ActionParam("Lines", typeof(ArrayList), true, false, "Calling party number")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void NotifyLogin(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			ArrayList lines = action.InnerMessage.GetField("Lines") as ArrayList;

			string deviceName = null;
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);

			bool isSubscriber = server.SendLoginResponse(action.RoutingGuid, deviceName, lines);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}

		protected void InitiatePoll(ActionBase action)
		{
			SyncSalesforce();
		}


		protected enum CustomerLookupResults
		{
			Success,
			Failure,
			NoMatch,
			DuplicateMatches
		}

		[Action(ProviderNamespace + "." + "CustomerLookup", false, "Customer Lookup", "Looks up a customer based on phone number", false)]
		[ActionParam("PhoneNumber", typeof(string), true, false, "Phone Number of customer to be looked up")]
		[ResultData("ContactSFId", typeof(string), "Customer's SF Database ID")]
		[ResultData("FirstName", typeof(string), "Customer's first name")]
		[ResultData("LastName", typeof(string), "Customer's last name")]
		[ResultData("PhoneNumber", typeof(string), "Customer's phone number")]
		[ResultData("Street", typeof(string), "Street Address")]
		[ResultData("City", typeof(string), "City")]
		[ResultData("State", typeof(string), "State")]
		[ResultData("PostalCode", typeof(string), "Postal Code")]
		[ResultData("Country", typeof(string), "Country")]
		[ResultData("Latitude", typeof(string), "Contact Latitude")]
		[ResultData("Longitude", typeof(string), "Contact Longitude")]
		[ResultData("AccountSFId", typeof(string), "Account SF Database ID")]
		[ResultData("AccountName", typeof(string), "Account Name")]
		[ResultData("AccounType", typeof(string), "Account Type")]
		protected void CustomerLookup(ActionBase action)
		{
			string phoneNumber = null;
			
			action.InnerMessage.GetString("PhoneNumber", true, null, out phoneNumber);
		
			bool foundRow = false;
			ArrayList results = new ArrayList();

			if(cacheConnection != null)
			{
				try
				{
					if(cacheConnection.State == ConnectionState.Closed) 
					{
						cacheConnection.Open();
					}
					else
					{
						// this shouldn''t happen...
						log.Write(TraceLevel.Warning, "The cacheConnection is already open");
					}

					using(IDbCommand command = cacheConnection.CreateCommand())
					{
						// should be using parameterized
						command.CommandText = String.Format(@"
							SELECT 
								c.sfId, c.firstname, c.lastname, c.phoneNumber, c.street, c.city, c.state, 
								c.zip, c.country, c.latitude, c.longitude, a.sfId, a.accountName, a.accountType 
							FROM 
								contacts c, accounts a
							WHERE
								c.phoneNumber = '{0}' 
								AND
								c.account_id = a.sfId", phoneNumber);
						using(IDataReader reader = command.ExecuteReader())
						{
							// doesn't handle multiple returns currently
							if(reader.Read())
							{
								results.Add(new Metreos.Messaging.Field("ContactSFId", reader[0] as string));
								results.Add(new Metreos.Messaging.Field("FirstName", reader[1] as string));
								results.Add(new Metreos.Messaging.Field("LastName", reader[2] as string));
								results.Add(new Metreos.Messaging.Field("PhoneNumber", reader[3] as string));
								results.Add(new Metreos.Messaging.Field("Street", reader[4] as string));
								results.Add(new Metreos.Messaging.Field("City", reader[5] as string));
								results.Add(new Metreos.Messaging.Field("State", reader[6] as string));
								results.Add(new Metreos.Messaging.Field("PostalCode", reader[7] as string));
								results.Add(new Metreos.Messaging.Field("Country", reader[8] as string));
								results.Add(new Metreos.Messaging.Field("Latitude", reader[9] as string));
								results.Add(new Metreos.Messaging.Field("Longitude", reader[10] as string));
								results.Add(new Metreos.Messaging.Field("AccountSFId", reader[11] as string));
								results.Add(new Metreos.Messaging.Field("AccountName", reader[12] as string));
								results.Add(new Metreos.Messaging.Field("AccountType", reader[13] as string));

								foundRow = true;
							}
						}
					}
				}
				catch(Exception e)
				{
					log.Write(TraceLevel.Error, "Unable to query the local Salesforce.com database for contact info.  {0}", Metreos.Utilities.Exceptions.FormatException(e));
				}
			}

			action.SendResponse(foundRow, results);
		}


		[Action(ProviderNamespace + "." + "GetResellerContacts", false, "Reseller Lookup", "Looks up all resellers", false)]
		[ResultData("Resellers", typeof(ArrayList), "A list of all known contacts associated with resellers")]
		protected void GetResellerContacts(ActionBase action)
		{
		
			bool success = true;
			ArrayList results = new ArrayList();

			if(cacheConnection != null)
			{
				try
				{
					if(cacheConnection.State == ConnectionState.Closed) 
					{
						cacheConnection.Open();
					}
					else
					{
						// this shouldn''t happen...
						log.Write(TraceLevel.Warning, "The cacheConnection is already open");
					}

					using(IDbCommand command = cacheConnection.CreateCommand())
					{
						// should be using parameterized
						command.CommandText = @"
							SELECT 
								c.sfId, c.firstname, c.lastname, c.phoneNumber, c.street, c.city, c.state, 
								c.zip, c.country, c.latitude, c.longitude, a.sfId, a.accountName, a.accountType 
							FROM 
								contacts c, accounts a
							WHERE
								a.accountType = 'Reseller'
								AND
								c.account_id = a.sfId";

						using(IDataReader reader = command.ExecuteReader())
						{
							// doesn't handle multiple returns currently
							while(reader.Read())
							{
								results.Add(new string[] { 
															 reader[0] as string, /* Contact SF Id */
															 reader[1] as string, /* First Name */
															 reader[2] as string, /* Last Name */
															 reader[3] as string, /* Phone Number */
															 reader[4] as string, /* Street */
															 reader[5] as string, /* City */
															 reader[6] as string, /* State */
															 reader[7] as string, /* PostalCode */
															 reader[8] as string, /* Country */
															 reader[9] as string, /* Latitude */
															 reader[10] as string, /* Longitude */
															 reader[11] as string, /* Account SF ID */
															 reader[12] as string, /* Account Name */
															 reader[13] as string /* Account Type */
														 });

							}
						}
					}
				}
				catch(Exception e)
				{
					log.Write(TraceLevel.Error, "Unable to query the local Salesforce.com database for reseller contact info.  {0}", Metreos.Utilities.Exceptions.FormatException(e));
					success = false;
				}
			}

			action.SendResponse(success, new Metreos.Messaging.Field("Resellers", results));
		}


		#endregion

		#region Cuae Client Events
		[Event(ProviderNamespace + "." + "ConferenceRequest", false, null, "Confereence Request", "An client has requested to conference in a 3-party to an existing call")]
		[EventParam("JTAPICallId", typeof(System.String), true, "The JTAPI callIds of the call to merge to")]
		[EventParam("To", typeof(System.String), true, "The number to call")]
		private bool ConferenceRequest(string routingGuid, string callId, string to)
		{
			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "ConferenceRequest",
				EventMessage.EventType.NonTriggering,
				routingGuid);
			msg.AddField("JTAPICallId", callId);
			msg.AddField("To", to);

			palWriter.PostMessage(msg);

			// Probably doesn't make sense to return bool here
			return true;
		}
		#endregion

		#region Cuae Client Actions
		[Action(ProviderNamespace + "." + "ConferenceInitiated", false, "Conference Initiated", "Notify a subscriber of the initiation results of a conference", false)]
		[ActionParam("Success", typeof(bool), true, false, "Success")]
		[ActionParam("DeviceName", typeof(string), true, false, "Name of device being called")]
		[ActionParam("FirstPartyCallId", typeof(string), false, false, "1st-party CallId associated with the new conference call")]
		[ResultData("IsSubscriber", typeof(bool), "Returns true if there is a subscriber, false if not")]
		protected void ConferenceInitiated(ActionBase action)
		{
			// Extract the info sent down from application out of the action message

			bool success = false;
			string deviceName = null;
			string callId = null;
			action.InnerMessage.GetBoolean("Success", true, false,  out success);
			action.InnerMessage.GetString("DeviceName", true, String.Empty, out deviceName);
			action.InnerMessage.GetString("FirstPartyCallId", false, String.Empty, out callId);

			bool isSubscriber = server.SendConferenceInitiated(action.RoutingGuid, deviceName, callId, success);

			// Once done, respond back to the application with success or failure, and any further result data

			Metreos.Messaging.Field resultDataField = new Metreos.Messaging.Field("IsSubscriber", isSubscriber);
			
			action.SendResponse(true, resultDataField);
		}
		#endregion
	}
}
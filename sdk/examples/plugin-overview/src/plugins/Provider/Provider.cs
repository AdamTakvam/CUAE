using System;
using System.Data; 
using System.Diagnostics;
using System.Timers;

using MySql.Data.MySqlClient;

using Metreos.Core;              // Contains IConfigUtility, needed by the ProviderBase constructor
using Metreos.ProviderFramework; // Contains ProviderBase class
using Metreos.Core.ConfigData;   // Contains ConfigData
using Metreos.Interfaces;        // All common interfaces of the Metreos Enviroment are found here
using Metreos.Utilities;         // Has a helpful utilities for interacting with databases
using Metreos.Messaging;         // All messaging in and out of the provider to other components need this
using Metreos.PackageGeneratorCore.Attributes; // Contains attributes for creating Action/Event XML 
                                               // packages for the designer
using Metreos.PackageGeneratorCore.PackageXml; // Some of the arguments of the attributes reference the 
                                               // package XML types directly
using Metreos.DatabaseScraper.Common; // Type information shared between this provider and the native type
namespace Metreos.Providers.DatabaseScraper
{
    // pgen.exe, which creates an XML defining this provider, requires the presence of these two attributes
    // [ProviderDecl(DisplayName)]
    [ProviderDecl("Database Scraper")] 
    // [PackageDecl(ProviderNamespace, Description)] 
    [PackageDecl("Metreos.Providers.DatabaseScraper", "Monitors legacy error reporting database")]
	public class DatabaseScraper : ProviderBase 
	{
        #region Constants

        // -------------------
        // Action Names
        // -------------------
        public const string QueryAction  = "Metreos.Providers.DatabaseScraper.Query";
        public const string ScrapeAction = "Metreos.Providers.DatabaseScraper.Scrape";

        // -------------------
        // Action Parameters
        // -------------------
        public const string StartTimeParam = "Start";
        public const string EndTimeParam = "End";

        // -------------------
        // Event Name
        // -------------------
        public const string NewErrorsEvent = "Metreos.Providers.DatabaseScraper.NewErrors";

        // -------------------
        // Event Parameters
        // -------------------
        public const string ErrorData = "Data";

        // -------------------
        // Configuration Items
        // -------------------
        // A provider can easily expose configuration values to the administrator
        // of the application server.  Here we just define the names of those values.

        public const string ScrapePeriodName    = "Scrape Period";
        public const int    ScrapePeriodDefault = 5;
        public const string ScrapePeriodDesc    = "Amount of minutes to wait between scrapes";
        
        // ------------------
        // Database Settings
        // ------------------
        // For the purpose of this example provider, we will use static database settings.
        public const string DatabaseName        = "legacydb";
        public const string DatabaseAddress     = "localhost";
        public const int    DatabasePort        = 3306;
        public const string DatabaseUsername    = "root";
        public const string DatabasePassword    = "metreos";
        public const string ErrorsTable         = "errors";
        public const string TimeColumn          = "time";
        public const string DescriptionColumn   = "description";
        
        #endregion
 
        private Timer scrapeTimer;
        private ElapsedEventHandler timerFireDelegate;
        private bool firstScrape;
        private int errorCount;

        public DatabaseScraper(IConfigUtility configUtility) 
            : base(typeof(DatabaseScraper), "Database Scraper", configUtility)
        {
            this.firstScrape    = true;
            this.errorCount     = 0;
            scrapeTimer         = new Timer();
            timerFireDelegate   = new ElapsedEventHandler(ScrapeTimerFire);
            scrapeTimer.Elapsed += timerFireDelegate;
        }

        private void ScrapeTimerFire( object sender, ElapsedEventArgs e )
        {
            ScrapeDatabase();
        }

        #region ProviderBase Implementation
        /// <summary>
        ///     In the initialize method, there are two common tasks:
        ///     1.  Defining which methods handle which actions that originate from apps
        ///     2.  Defining configuration items 
        /// </summary>
        /// <returns> Return true if the provider could be successfully  </returns>
        protected override bool Initialize(out ConfigEntry[] configItems , out Extension[] extensions)
        {
            this.messageCallbacks.Add( QueryAction,  new HandleMessageDelegate( HandleQuery ) );
            this.messageCallbacks.Add( ScrapeAction, new HandleMessageDelegate( HandleScrape ) );

            ConfigEntry scrapePeriodEntry = new ConfigEntry(
                ScrapePeriodName,               // The name of the configuration item
                ScrapePeriodDefault,            // The default value 
                ScrapePeriodDesc,               // The description
                IConfig.StandardFormat.Number,  // The type of value
                true);                          // Required
            
            // If there is no database entry found already for this configuration item,
            // then the database entry will be created with the default value.
            // If the database entry is found, then it will leave the current value
			configItems = new ConfigEntry[1];
			configItems[0] = scrapePeriodEntry;

			extensions = null;
            return true;
        }

        /// <summary>
        ///     When a change is made to the provider configuration on the 
        ///     Web Management Console page, it is the responsibility of the 
        ///     provider to synchronize with those changes.
        ///     
        ///     The one exception is the LogLevel configuration item.  The
        ///     ProviderBase handles changing of theL ogLevel for you.
        ///       
        ///     All providers have a LogWriter at their disposal, as member 'log'.
        ///     Along with that LogWriter, all providers have the configuration
        ///     item 'LogLevel' which determines at what level to filter logging.  
        /// </summary>
        protected override void RefreshConfiguration()
        {
            bool getConfigSuccess = true;
            int newScrapePeriod = 0;

            // Retrieve the configured scrape period by it's configuration name
            try
            {
                object scrapePeriodObj = this.GetConfigValue( ScrapePeriodName );
                newScrapePeriod = Convert.ToInt32( scrapePeriodObj );
            }
            catch( Exception e )
            {
                log.Write(TraceLevel.Info, "Unable to determine the new scrape period.  Exception is: " + e);
                getConfigSuccess = false;
            }

            //newScrapePeriod
            if( getConfigSuccess )
            {
                SetScrapePeriod( newScrapePeriod );
            }
        }

        /// <summary>
        ///     Sets the period of the scrape timer
        /// </summary>
        /// <param name="scrapePeriod">
        ///     The period of the scrape timer in minutes
        /// </param>
        private void SetScrapePeriod( int scrapePeriod )
        {
            if(scrapePeriod != 0 && scrapeTimer != null)
            {
                scrapeTimer.Interval = scrapePeriod * /*seconds/minute*/ 60 * /*milliseconds/second*/ 1000;
            }
        }

        /// <summary>
        ///     If this method fires, then an event we fired was handled by no application.
        /// </summary>
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            log.Write(TraceLevel.Info, "There was no application to handle the {0} event", NewErrorsEvent);        
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
            RegisterNamespace();

            scrapeTimer.Start();

            // Do an initial scrape
            ScrapeDatabase();

            base.OnStartup();
        }

        /// <summary>
        ///     Guaranteed to be called on a graceful shutdown of the Application Server
        /// </summary>
        protected override void OnShutdown()
        {
            if(scrapeTimer != null)
            {
                scrapeTimer.Elapsed -= timerFireDelegate;
                scrapeTimer.Close();
                scrapeTimer.Dispose();
                scrapeTimer = null;
            }
        }

        #endregion

        #region Provider Events

        [Event(NewErrorsEvent, true, null, NewErrorsEvent, "Fires when new errors are found in the database")]
        [EventParam(ErrorData, typeof(ErrorDataCollection), true, "The new error(s) information")]
        private bool ScrapeDatabase()
        {
            log.Write(TraceLevel.Info, "Database scrape start");

            bool success = true;

            string dsn = Database.FormatDSN(
                DatabaseName, 
                DatabaseAddress, 
                DatabasePort, 
                DatabaseUsername, 
                DatabasePassword, 
                true);

            try
            {
                using(IDbConnection connection = new MySqlConnection(dsn))
                {
                    connection.Open();

                    int newErrorCount = -1;
                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = String.Format("SELECT COUNT(*) FROM {0}", ErrorsTable);
                        newErrorCount = Convert.ToInt32(command.ExecuteScalar());
                    }

                    if(!firstScrape)
                    {
                        if(newErrorCount > errorCount)
                        {
                            ErrorDataCollection newErrors = new ErrorDataCollection();

                            // There are new errors.  Let's grab the most recent errors
                            using(IDbCommand command = connection.CreateCommand())
                            {
                                int numRowsToReturn = newErrorCount - errorCount;

                                string latestErrors = String.Format(
                                    "SELECT {0}, {1} FROM {2} ORDER BY {1} DESC LIMIT {3}", 
                                    DescriptionColumn, TimeColumn, ErrorsTable, numRowsToReturn);

                                command.CommandText = latestErrors;
                                using(IDataReader reader = command.ExecuteReader())
                                {
                                    while(reader.Read())
                                    {
                                        string description = reader[DescriptionColumn] as string;
                                        DateTime time = Convert.ToDateTime(reader[TimeColumn]);
                                        newErrors.Add(new ErrorData(description, time));
                                    }
                                }

                                // There are new errors in the errors table!
                                // Send up the new errors event to the application,
                                // so that it can do what it wants with the event
                                EventMessage msg = CreateEventMessage(
                                    NewErrorsEvent, 
                                    EventMessage.EventType.Triggering, 
                                    System.Guid.NewGuid().ToString());
                            

                                msg.AddField(ErrorData, newErrors);
                                palWriter.PostMessage(msg);
                            }
                        }
                    }
                    else
                    {
                        firstScrape = false;
                    }

                    errorCount = newErrorCount;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to connect to the database.  Full exception is: " +
                    Metreos.Utilities.Exceptions.FormatException(e));

                success = false;
            }

            log.Write(TraceLevel.Info, "Database scrape end");

            return success;
        } 

        #endregion

        #region Actions
        // The HandleQuery action is implemented here, as well as in the native actions project (Query), to contrast
        // the differences between a provider and native action

        // When pgen.exe creates the XML to represent this action, the name of this action defined in the [Action] 
        // attribute is appended to the namespace of the provider, defined in the [PackageDecl] attribute.  So,
        // for example, the full name of this particular action is Metreos.Providers.DatabaseScraper.Query,
        // which not coincidentally is the same value as the 'QueryAction' constant.  With that constant, we define
        // a message callback handler in the 'Initialize' method, so that whenever this provider receives the action named,
        // Metreos.Providers.DatabaseScraper.Query, it knows to invoke this method

        //[Action(FullName, AllowCustomParams, DisplayName, Description, Asynchronous)]
        [Action(QueryAction, false, "Query", "Queries the specified time frame for errors", false)]
        //ActionParam(Name, System.Type, Required, AllowMulti, Description)]
        [ActionParam(StartTimeParam, typeof(DateTime), false, false, "The earliest time to search for errors")]
        //ActionParam(Name, System.Type, Required, AllowMulti, Description)]
        [ActionParam(EndTimeParam, typeof(DateTime), false, false, "The latest time to search for errors")]
        //ResultData(Name, DisplayName, System.Type, Description)] 
        [ResultData(ErrorData, ErrorData, typeof(ErrorDataCollection), "Errors that fall in the specified range")]
        private void HandleQuery(ActionBase action)
        {
            DateTime startTime;
            DateTime endTime;

            // We define the non-existence of the start and end time to mean search until the beginning
            // of time for the StartTime parameter, and until the end of time for the EndTime parameter.
            action.InnerMessage.GetDateTime(StartTimeParam, false, DateTime.MinValue, out startTime);
            action.InnerMessage.GetDateTime(EndTimeParam, false, DateTime.MaxValue, out endTime);

            if(startTime > endTime)
            {
                log.Write(TraceLevel.Error, "The EndTime can not be greater than the StartTime.");
                action.SendResponse(false);
            }

            ErrorDataCollection errors = new ErrorDataCollection();

            bool success = true;

            string dsn = Database.FormatDSN(
                DatabaseName, 
                DatabaseAddress, 
                DatabasePort, 
                DatabaseUsername, 
                DatabasePassword, 
                true);

            try
            {
                using(IDbConnection connection = new MySqlConnection(dsn))
                {
                    connection.Open();

                    using(IDbCommand command = connection.CreateCommand())
                    {                        
                        // Select all rows that fall between the start and end range
                        command.CommandText = String.Format(
                            "SELECT {0}, {1} FROM {2} " + 
                            "WHERE {1} > STR_TO_DATE('{3}', GET_FORMAT(DATETIME, 'USA')) " + 
                            "AND {1} < STR_TO_DATE('{4}', GET_FORMAT(DATETIME, 'USA'))",
                            DescriptionColumn,
                            TimeColumn,
                            ErrorsTable,
                            startTime.ToString("yyyy-MM-dd HH.mm.ss"),
                            endTime.ToString("yyyy-MM-dd HH.mm.ss"));

                        log.Write(TraceLevel.Info, command.CommandText);

                        using(IDataReader reader = command.ExecuteReader())
                        {
                            // Create ErrorData classes as we cycle thtrough the 
                            // results, and add them to the ErrorDataCollection
                            while(reader.Read())
                            {
                                string description = reader[DescriptionColumn] as string;
                                DateTime time = Convert.ToDateTime(reader[TimeColumn]);
                                errors.Add(new ErrorData(description, time));
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to connect to the database.  Full exception is: " +
                    Metreos.Utilities.Exceptions.FormatException(e));

                success = false;
            }

            if(success)
            {
                // Send a success response to the waiting action, while assigning the result data field
                // 'Data' with the value of the ErrorDataCollection for this query
                action.SendResponse(true, new Field(ErrorData, errors));
            }
            else
            {
                action.SendResponse(false);
            }
            
        }

        // By defining this action, we provide an application the means to force a scrape
        // of the legacy database.  

        //[Action(FullName, AllowCustomParams, DisplayName, Description, Asynchronous)]
        [Action(ScrapeAction, false, "Scrape", "Forces a scrape of the database", false)]
        private void HandleScrape(ActionBase action)
        {
            bool success = ScrapeDatabase();
            action.SendResponse(success);
        }
        #endregion
    }
}
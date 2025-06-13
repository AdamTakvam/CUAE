using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Configuration;

namespace CallMonitorReport
{
	/// <summary>
	/// Summary description for Reporter.
	/// </summary>
	public class Reporter
	{
        private abstract class Consts
        {
            public const string CONFIG_DATABASENAME			= "DatabaseName";
            public const string CONFIG_DATABASEHOSTNAME		= "DatabaseHostname";
            public const string CONFIG_DATABASEPORT			= "DatabasePort";
            public const string CONFIG_DATABASEUSERNAME		= "DatabaseUsername";
            public const string CONFIG_DATABASEPASSWORD		= "DatabasePassword";
            public const string CONFIG_AUDITLOGPATH         = "AuditLogPath";    
        }

        private abstract class DefaultValues
        {
            public const string DEFAULT_DATABASENAME         = "monitor_call";
            public const string DEFAULT_DATABASEHOSTNAME     = "localhost";
            public const ushort DEFAULT_DATABASEPORT         = 3306;
            public const string DEFAULT_DATABASEUSERNAME     = "root";
            public const string DEFAULT_DATABASEPASSWORD     = "metreos";
            public const string DEFAULT_AUDITLOGPATH         = "c:\\monitor_call\\AuditLog";
        }

        private string auditLogPath;
        private string databaseHostname;
        private string databaseName;
        private string databaseUsername;
        private string databasePassword;
        private ushort databasePort;

        private DbReader reader;
        private CsvWriter csvWriter;
        private HtmlWriter htmlWriter;

        private string fromDate;
        private string toDate;
        private DataTable calls;
        private ArrayList DIDCallStats;

        /// <summary>
        /// Constructor
        /// </summary>
        public Reporter()
		{
            ReadConfigDataFromConfigFile();
            reader = new DbReader(databaseHostname,
                                  databaseName,
                                  databaseUsername,
                                  databasePassword,
                                  databasePort);
            csvWriter = new CsvWriter(auditLogPath);
            htmlWriter = new HtmlWriter(auditLogPath);
            DIDCallStats = new ArrayList();
		}

        public bool ReadData(string fDate, string tDate)
        {
            if (!reader.IsDbConnected())
            {
                Console.Write("Error: Unable to connect to database.");
                return false;
            }

            if(Directory.Exists(auditLogPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(auditLogPath);
                }
                catch
                {
                    Console.WriteLine("Error: Could not create audit log folder {0}. Is the disk full?", auditLogPath);
                    return false;
                }
            }

            if (fDate == null)
            {
                fDate = reader.GetFirstLogDate();
                if (fDate == null)
                    fDate = tDate;
            }
            else if (tDate == null)
            {
                tDate = DateTime.Today.ToString("yyyy-MM-dd");
            }

            fromDate = fDate;
            toDate = tDate;

            calls = reader.GetMonitoredCallsByTimeInterval(fromDate, toDate);
            if (calls == null || calls.Rows.Count == 0)
            {
                Console.Write("No call records found between {0} and {1}", fromDate, toDate);
                reader.CloseDatabase();
                return false;
            }

            ArrayList DIDs = reader.GetDIDsByTimeInterval(fromDate, toDate);

            for (int i=0; i<DIDs.Count; i++)
            {
                string did = DIDs[i].ToString();
                int numCalls = reader.GetNumCallsByDID(did, fromDate, toDate);
                DIDCallStat dcs = new DIDCallStat(did, numCalls);
                DIDCallStats.Add(dcs);
            }

            reader.CloseDatabase();
            return true;
        }

        /// <summary>
        /// Write Csv Report
        /// </summary>
        public void WriteCsv()
        {
            csvWriter.WriteReport(calls, DIDCallStats, fromDate, toDate);
        }

        /// <summary>
        /// Write Html report
        /// </summary>
        public void WriteHtml()
        {
            htmlWriter.WriteReport(calls, DIDCallStats, fromDate, toDate);
        }

        /// <summary> 
        /// Read configuration data from application config file
        /// </summary>
        private void ReadConfigDataFromConfigFile()
        {
            // Set DB parameters default value
            auditLogPath = DefaultValues.DEFAULT_AUDITLOGPATH;
            databaseHostname = DefaultValues.DEFAULT_DATABASEHOSTNAME;
            databaseName = DefaultValues.DEFAULT_DATABASENAME;
            databaseUsername = DefaultValues.DEFAULT_DATABASEUSERNAME;
            databasePassword = DefaultValues.DEFAULT_DATABASEPASSWORD;
            databasePort = DefaultValues.DEFAULT_DATABASEPORT;

            // Read database config data from config file
            // Uset the same DB parameter names as logging-server
            auditLogPath = ConfigurationSettings.AppSettings[Consts.CONFIG_AUDITLOGPATH];
            databaseHostname = ConfigurationSettings.AppSettings[Consts.CONFIG_DATABASEHOSTNAME];
            databaseName = ConfigurationSettings.AppSettings[Consts.CONFIG_DATABASENAME];
            databaseUsername = ConfigurationSettings.AppSettings[Consts.CONFIG_DATABASEUSERNAME];
            databasePassword = ConfigurationSettings.AppSettings[Consts.CONFIG_DATABASEPASSWORD];
            try 
            {
                databasePort = ushort.Parse(ConfigurationSettings.AppSettings[Consts.CONFIG_DATABASEPORT]);
            }
            catch
            {
                // NFE				
            }
        }
	}
}

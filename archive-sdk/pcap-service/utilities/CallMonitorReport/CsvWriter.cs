using System;
using System.IO;
using System.Data;
using System.Collections;

namespace CallMonitorReport
{
	/// <summary>
	/// Summary description for CSVWriter.
	/// </summary>
	/// 

	public class CsvWriter
	{
        private string auditLogPath;

        public CsvWriter(string path)
		{
            auditLogPath = path;
		}

        public bool WriteReport(DataTable calls, ArrayList DIDstats, string fromDate, string toDate)
        {
            string fileTimeStamp = DateTime.Parse(fromDate).ToString("yyyy-MM-dd") + "_" + DateTime.Parse(toDate).ToString("yyyy-MM-dd");
            string fileName = System.IO.Path.Combine(auditLogPath, System.IO.Path.ChangeExtension(fileTimeStamp, ".csv"));
            StreamWriter writer;

            try
            {
                writer = File.CreateText(fileName);
                writer.AutoFlush = true;
            }
            catch
            {
                Console.WriteLine("Error: Could not generate CSV report. Is the disk full?");
                return false;
            }

            writer.WriteLine("#Metreos Call Monitor Audit Log [CSV]");
            writer.WriteLine("#");
            writer.WriteLine("#Start Date: {0}, End Date: {1}", fromDate, toDate);
            writer.WriteLine("#Total number of monitor calls during this period: {0}", calls.Rows.Count);
            writer.WriteLine("#Number of monitor calls by dial-in number:"); 

            for (int i=0; i<DIDstats.Count; i++)
            {
                DIDCallStat dcs = DIDstats[i] as DIDCallStat;
                writer.WriteLine("#{0}: {1}", dcs.did, dcs.numCalls.ToString());
            }

            writer.WriteLine("#");
            writer.WriteLine("#mc_did_number,mc_government_agent_number,mc_customer_number,mc_insurance_agent_number,mc_monitored_sid,mc_start_monitor_timestamp");

            foreach(DataRow row in calls.Rows)
            {
                writer.WriteLine("{0},{1},{2},{3},{4},{5}",                                 
                                row[0].ToString(),
                                row[1].ToString(),
                                row[2].ToString(),
                                row[3].ToString(),
                                row[4].ToString(),
                                row[5].ToString());
            }
            writer.WriteLine("#");
            writer.WriteLine("# End of File");

            writer.Close();

            Console.WriteLine("CSV report created.");
                        
            return true;
        }


	}
}

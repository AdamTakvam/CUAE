using System;
using System.IO;
using System.Data;
using System.Collections;

namespace CallMonitorReport
{
	/// <summary>
	/// Summary description for HtmlWriter.
	/// </summary>
	public class HtmlWriter
	{
        private string auditLogPath;

        public HtmlWriter(string path)
        {
            auditLogPath = path;
        }

        public bool WriteReport(DataTable calls, ArrayList DIDstats, string fromDate, string toDate)
        {
            string fileTimeStamp = DateTime.Parse(fromDate).ToString("yyyy-MM-dd") + "_" + DateTime.Parse(toDate).ToString("yyyy-MM-dd");
            string fileName = System.IO.Path.Combine(auditLogPath, System.IO.Path.ChangeExtension(fileTimeStamp, ".htm"));
            StreamWriter writer;

            try
            {
                writer = File.CreateText(fileName);
                writer.AutoFlush = true;
            }
            catch
            {
                Console.WriteLine("Error: Could not generate HTML report. Is the disk full?");
                return false;
            }

            writer.WriteLine("<html>");
            // head
            writer.WriteLine("<head>");
            writer.WriteLine("<title>Metreos Call Monitor Audit Log</title>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            // summary
            writer.WriteLine("<h2>Metreos Call Monitor Audit Log</h2>");
            writer.WriteLine("<hr />");
            writer.WriteLine("<h4>Start date: {0}, End date: {1}</h4>", fromDate, toDate);
            writer.WriteLine("<h4>Total number of monitor calls during this period: {0}</h4>", calls.Rows.Count);
            writer.WriteLine("<hr />");

            writer.WriteLine("<h4>Number of monitor calls by dial-in number:</h4>");           
            writer.WriteLine("<table class=\"ex\" cellspacing=\"0\" border=\"1\" width=\"100%\" cellpadding=\"3\">");
            writer.WriteLine("<tr><th align=\"left\" width=\"50%\">Dial-in number</th><th align=\"left\" width=\"50%\">Number of calls</th></tr>");

            for (int i=0; i<DIDstats.Count; i++)
            {
                DIDCallStat dcs = DIDstats[i] as DIDCallStat;
                writer.WriteLine("<tr><td valign=\"top\">{0}</td><td valign=\"top\">{1}</td></tr>", dcs.did, dcs.numCalls.ToString());
            }
            writer.WriteLine("</table><br /><hr />");          

            // records
            writer.WriteLine("<h4>Monitor Call Records:</h4>");           
            writer.WriteLine("<table class=\"ex\" cellspacing=\"0\" border=\"1\" width=\"100%\" cellpadding=\"3\">");
            writer.WriteLine("<tr><th align=\"left\" width=\"15%\">DID</th><th align=\"left\" width=\"15%\">Authority</th><th align=\"left\" width=\"15%\">Customer</th><th align=\"left\" width=\"15%\">Agent</th><th align=\"left\" width=\"15%\">Device Name</th><th align=\"left\" width=\"20%\">Monitor Start Time</th></tr>");
            foreach(DataRow row in calls.Rows)
            {
                writer.WriteLine("<tr><td valign=\"top\">{0}&nbsp;</td><td valign=\"top\">{1}&nbsp;</td><td valign=\"top\">{2}&nbsp;</td><td valign=\"top\">{3}&nbsp;</td><td valign=\"top\">{4}&nbsp;</td><td valign=\"top\">{5}&nbsp;</td></tr>", 
                                row[0].ToString(),
                                row[1].ToString(),
                                row[2].ToString(),
                                row[3].ToString(),
                                row[4].ToString(),
                                row[5].ToString());
            }
            writer.WriteLine("</table><br />");          
 
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");

            writer.Close();

            Console.WriteLine("HTML report created.");

            return true;
        }
	}
}

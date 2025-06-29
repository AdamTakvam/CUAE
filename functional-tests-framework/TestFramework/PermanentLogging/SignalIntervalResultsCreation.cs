using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for SignalResultsCreation.
	/// </summary>
	public class SignalIntervalResultsCreation : TestLogCreationBase
	{
        protected StringCollection times;
        protected string units;
        protected int interval;
        protected int numberOfSignalsSent;
        protected bool timesErrorFlag;

		public SignalIntervalResultsCreation(StringCollection info, string title, StringCollection times, string units, int interval, int numberOfSignalsSent) : base(info, title)
		{
            if(info == null)
            {
                this.info = new StringCollection();
                this.info.Add("Error: Results came in as null.  Perhaps threading violation?");
                this.times = new StringCollection();
                this.times.Add(DateTime.Now.ToLongTimeString());
            }
            else
            {
                if(info.Count == times.Count)
                {
                    this.times = times;
                    timesErrorFlag = false;
                }
                else
                {
                    this.times = new StringCollection();
                    for (int i = 0; i < info.Count; i++)
                    {
                        times.Add("Time/Record size mismatch");
                    }
                    timesErrorFlag = true;
                }
            }
            this.units = units;
            this.interval = interval;
            this.numberOfSignalsSent = numberOfSignalsSent;
		}
        public override string CreateXML()
        {
            //Call XML serializer
            string results = null;
            return results;
        }
        public override string CreateXHTML()
        {
            XHTMLBuilder = new StringBuilder("<div class='signalInterval'><span class='title'>" + title + "</span>");
            XHTMLBuilder.Append("<p><span class='info'>The amount of time used for messaging interval: <span>" + interval + " " + units + "</span></span></p>");
            
            if(timesErrorFlag == true)
            {
                XHTMLBuilder.Append("<p><span class='warning'>All signals did not have corresponding recorded times.  Setting signal times to 0</span></p>");
            }

            if(numberOfSignalsSent != info.Count)
            {
                XHTMLBuilder.Append("<p><span class='warning'>All signals were not received!</span><br/>");
                XHTMLBuilder.Append("<span class='warning'>Number of signals sent: <span>" + numberOfSignalsSent + "</span></span><br/>");
                XHTMLBuilder.Append("<span class='warning'>Number of signals received: <span class='warning'>" + info.Count + "</span></span></p>");
            }
            else
            {
                XHTMLBuilder.Append("<p><span class='verbose'>All signals were received.</span></p>");
            }

            if(info.Count != 0)
            {
                XHTMLBuilder.Append("<table border='0' class='signalInterval'>");
                XHTMLBuilder.Append("<th class='signalInterval'>Signal Number</th><th class='signalInterval'>" + units + "&nbspSince Last Signal</th><th class='signalInterval'>Time Received</th>");

                for(int i = 0; i < info.Count; i++)
                {
                    int evenOrOdd = i%2;

                    if(evenOrOdd == 0)
                    {
                        XHTMLBuilder.Append("<tr class='signalInterval'><td class='even'>" + i + "</td><td class='even'>" + info[i] + "</td><td class='even'>" + times[i] + "</td></tr>");
                    }
                    else
                    {
                        XHTMLBuilder.Append("<tr class='signalInterval'><td class='odd'>" + i + "</td><td class='odd'>" + info[i] + "</td><td class='odd'>" + times[i] + "</td></tr>");
                    }
                }
                XHTMLBuilder.Append("</table>");
            }
           

            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }


	}
}

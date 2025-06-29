using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Takes data of how long it takes to process all signals.
	/// </summary>
	public class AllSignalsSentCreation : TestLogCreationBase
	{
        protected StringCollection times;
        protected string units;
        protected int interval;
        protected int numberOfSignalsSent;
        protected bool timesErrorFlag;

		public AllSignalsSentCreation(StringCollection info, string title, StringCollection times, string units, int interval, int numberOfSignalsSent) : base(info, title)
		{
            if(info == null)
            {
                this.info = new StringCollection();
                this.info.Add("Error: Results came in as null.  Perhaps threading violation?");
                this.times = new StringCollection();
                this.times.Add(DateTime.Now.ToLongTimeString());
            }else
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
            XHTMLBuilder = new StringBuilder("<div class='allSignalsInterval'><p><span class='title'>" + title + "</span></p>");
            XHTMLBuilder.Append("<p><span class='info'>The amount of time used for messaging interval: <span class='interval'>" + interval + " " + units + "</span></span></p>");
            
            if(timesErrorFlag == true)
            {
                XHTMLBuilder.Append("<p><span class='warning'>All signals did not have corresponding recorded times.  Setting signal times to 0</span></p>");
            }
            

            if(info.Count != 0)
            {
                XHTMLBuilder.Append("<table border='0' class='allSignalsInterval'>");
                XHTMLBuilder.Append("<th class='allSignalsInterval'>Signal Group</th><th class='allSignalsInterval'>" + units + "&nbspto process all signals</th><th class='allSignalsInterval'>Time Received</th>");

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

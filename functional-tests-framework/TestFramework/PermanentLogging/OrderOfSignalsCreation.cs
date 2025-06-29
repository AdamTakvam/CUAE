using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for SignalResultsCreation.
	/// </summary>
	public class OrderOfSignalsCreation : TestLogCreationBase
	{
        bool signalMismatchFlag;

		public OrderOfSignalsCreation(StringCollection info, string title, bool signalMismatchFlag) : base(info, title)
		{
            if(info == null)
            {
                this.info = new StringCollection();
                this.info.Add("Error: Results came in as null.  Perhaps threading violation?");
                
            }  
            this.signalMismatchFlag = signalMismatchFlag;
		}

        public override string CreateXML()
        {
            //Call XML serializer
            string results = null;
            return results;
        }

        public override string CreateXHTML()
        {
            XHTMLBuilder = new StringBuilder("<div class='orderOfSignals'><span class='title'>" + title + "</span>");
            
            if(signalMismatchFlag == true)
            {
                XHTMLBuilder.Append("<p><span class='warning'>Less shutdown signals than expected occurred</span></p>");
            }

            if(info.Count != 0)
            {
                XHTMLBuilder.Append("<table border='0' class='orderOfSignals'>");
                XHTMLBuilder.Append("<th class='orderOfSignals'>Trigger Signal Number</th><th class='orderOfSignals'>Corresponding Shutdown Number</th>");

                for(int i = 0; i < info.Count; i++)
                {
                    int evenOrOdd = i%2;

                    if(evenOrOdd == 0)
                    {
                        XHTMLBuilder.Append("<tr class='orderOfSignals'><td class='even'>" + i + "</td><td class='even'>" + info[i] + "</td></tr>");
                    }
                    else
                    {
                        XHTMLBuilder.Append("<tr class='orderOfSignals'><td class='odd'>" + i + "</td><td class='odd'>" + info[i] + "</td></tr>");
                    }
                }
                XHTMLBuilder.Append("</table>");
            }
           

            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }


	}
}

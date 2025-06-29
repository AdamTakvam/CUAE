using System;
using System.Text;
using System.Collections.Specialized;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Generates simple table to display polls of the MSMQ's size at different times in a test
	/// </summary>
	public class MSMQSizeCreation : TestLogCreationBase
	{

		public MSMQSizeCreation(StringCollection info, string title) : base(info, title)
		{
            if(info == null)
            {
                this.info = new StringCollection();
                this.info.Add("Error: Results came in as null.  Perhaps threading violation?");
                
            }  
		}

        public override string CreateXML()
        {
            //Call XML serializer
            string results = null;
            return results;
        }

        public override string CreateXHTML()
        {
            XHTMLBuilder = new StringBuilder("<div class='msmqSizeCreation'><span class='title'>" + title + "</span>");

            if(info.Count != 0)
            {
                XHTMLBuilder.Append("<table border='0' class='msmqSizeCreation'>");
                XHTMLBuilder.Append("<th class='msmqSizeCreation'>Time MSMQ Polled</th><th class='msmqSizeCreation'>Size at that time</th>");

                for(int i = 0; i < info.Count; i = i + 2)
                {
                    int evenOrOdd = i%4;

                    if(evenOrOdd == 2)
                    {
                        XHTMLBuilder.Append("<tr class='msmqSizeCreation'><td class='even'>" + info[i] + "</td><td class='even'>" + info[i + 1] + "</td></tr>");
                    }
                    else
                    {
                        XHTMLBuilder.Append("<tr class='msmqSizeCreation'><td class='odd'>" + info[i] + "</td><td class='odd'>" + info[i + 1] + "</td></tr>");
                    }
                }
                XHTMLBuilder.Append("</table>");
            }
           

            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }

	}
}

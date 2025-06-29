using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Outputs test success/failure and messages
	/// </summary>
	public class TestOutcomeCreation : TestLogCreationBase
	{
        bool infoErrorFlag;
        string initial;
        string final;
        bool initialErrorFlag;
        bool finalErrorFlag;
        bool testFailedFlag;

		public TestOutcomeCreation(StringCollection info, string title, string initial, string final) : base(info, title)
		{
            if(info[0] == "Failure")
            {
                testFailedFlag = true;
            }
            else
            {
                testFailedFlag = false;
            }
            if(initial != null)
            {
                this.initial = initial;
                initialErrorFlag = false;
            }
            else
            {
                this.initial = "None recorded";
                initialErrorFlag = true;
            }

            if(final != null)
            {
                this.final = final;
                finalErrorFlag = false;
            }
            else
            {
                this.final = "None recorded";
                finalErrorFlag = false;
            }

            if(info == null)
            {
                infoErrorFlag = true;       
            }
            else
            {
                if(info.Count == 0)
                {
                    infoErrorFlag = true;
                }
                else
                {
                    infoErrorFlag = false;
                }
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
            XHTMLBuilder = new StringBuilder("<div class='testOutcome'><span class='title'>" + title + "</span>");
            
            if(infoErrorFlag != true && !testFailedFlag)
            {
                XHTMLBuilder.Append("<p><span class='info important'>" + info[0] + "!</span></p>");
            }
            else if(infoErrorFlag == true)
            {   
                XHTMLBuilder.Append("<p><span class='error important'>Error: Results came in as null.</span></p>");
            }

            if(infoErrorFlag != true && testFailedFlag)
            {
                XHTMLBuilder.Append("<p><span class='error important'>" + info[0] + "!</span></p>");
            }

            if(initialErrorFlag == false)
            {
                XHTMLBuilder.Append("<p><span class='info important'>Test initiated: " + initial + "</span></p>");
            }
            else
            {
                XHTMLBuilder.Append("<p><span class='error importnat'>Test initiated: " + initial + "</span></p>");
            }
            
            if(finalErrorFlag == false)
            {
                XHTMLBuilder.Append("<p><span class='info important'>Test completed: " + final + "</span></p>");
            }
            else
            {
                XHTMLBuilder.Append("<p><span class='error important'>Test completed: " + final + "</span></p>");
            }
            if(info.Count != 0 && !infoErrorFlag)
            {
                XHTMLBuilder.Append("<table border='0' class='testOutcome'>");
                XHTMLBuilder.Append("<caption>Messages</caption>");
                XHTMLBuilder.Append("<th class='testOutcome'>Message</th>");

                for(int i = 1; i < info.Count; i++)
                {
                    int evenOrOdd = i%2;

                    if(evenOrOdd == 0)
                    {
                        XHTMLBuilder.Append("<tr class='testOutcome'><td class='even'>" + info[i] + "</td></tr>");
                    }
                    else
                    {
                        XHTMLBuilder.Append("<tr class='testOutcome'><td class='odd'>" + info[i] + "</td></tr>");
                    }
                }
                XHTMLBuilder.Append("</table>");
            }
            else
            {
                XHTMLBuilder.Append("<p><span class='warning'>Result list is null</span></p>");
            }
           

            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }


	}
}

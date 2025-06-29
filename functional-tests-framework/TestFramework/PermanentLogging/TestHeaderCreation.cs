using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Presents a summary of the test info at the top of the page
	/// </summary>
	public class TestHeaderCreation : TestLogCreationBase
	{
        
        protected StringCollection times;
        protected string units;
        protected int interval;
        protected int numberOfSignalsSent;
        protected bool timesErrorFlag;

        public TestHeaderCreation(StringCollection info, string title) :base(info, title)
        {
        }

        public override string CreateXML()
        {
            //Call XML serializer
            string results = null;
            return results;
        }
        public override string CreateXHTML()
        {
            XHTMLBuilder = new StringBuilder("<div class='main'><span class='title'>" + title + "</span>");
            XHTMLBuilder.Append("<ul>");
            for(int i = 0; i < info.Count; i++)
            {
                XHTMLBuilder.Append("<li><span>" +info[i] + "</span></li>");
            }
            XHTMLBuilder.Append("</ul>");
            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }


	}
}

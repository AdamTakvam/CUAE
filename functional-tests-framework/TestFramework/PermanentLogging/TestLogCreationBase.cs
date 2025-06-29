using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Abstract class for creating XML or XHMTL components from test results
	/// </summary>
	public abstract class TestLogCreationBase
	{
        protected StringCollection info;
        protected string title;
        protected StringBuilder XHTMLBuilder;
        

		public TestLogCreationBase(StringCollection info, string title)
		{
            this.info = info;
            this.title = title;
		}

        public TestLogCreationBase()
        {

        }

        public abstract string CreateXML();


        public abstract string CreateXHTML();

	}
}

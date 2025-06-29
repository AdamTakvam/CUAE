using System;
using System.Xml;
using System.Xml.Serialization;

namespace QuickDialRequester
{
    [Serializable]
	public class test
	{
		public test(){}

        public pair[] pairs;
	}

    public class pair
    {
        [XmlAttribute()]
        public string to;

        [XmlAttribute()]
        public string confereeTo;
    }
}

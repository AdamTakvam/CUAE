using System;
using System.Xml;
using System.Xml.Serialization;

namespace Metreos.Samoa.FunctionalTestFramework
{
    [Serializable]
    [System.Xml.Serialization.XmlRootAttribute("results", Namespace="", IsNullable=false)]
    public class TestResults
    {
        [XmlAttributeAttribute("ran")]
        public bool ran;

        [XmlAttributeAttribute("errorreason")]
        public string errorReason;

        [System.Xml.Serialization.XmlElementAttribute("testresult", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TestResult[] results;
    }

    public class TestResult
    {  
        [XmlElementAttribute("output")]
        public string output;
       
        [System.Xml.Serialization.XmlElementAttribute("issue", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public Issue[] issues;
        
        [System.Xml.Serialization.XmlElementAttribute("qatest", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public QaTest[] qaTests;
        
        [XmlAttributeAttribute("success")]
        public Success success;

        [XmlAttributeAttribute("errorreason")]
        public string errorReason;

        [XmlAttributeAttribute("testName")]
        public string testname;
    }

    public class Issue
    {
        public Issue() {}
        public Issue(string issue) { this.issue = issue; }
        [System.Xml.Serialization.XmlTextAttribute()]
        public string issue;
    }

    public class QaTest
    {
        public QaTest() {}
        public QaTest(string qaTest) { this.qaTest = qaTest; }
        [System.Xml.Serialization.XmlTextAttribute()]
        public string qaTest;
    }

    public enum Success
    {
        True,
        False,
        NotRun
    }
}

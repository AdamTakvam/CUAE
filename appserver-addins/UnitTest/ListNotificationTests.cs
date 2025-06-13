using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using NUnit.Framework;

using Metreos.LoggingFramework;
using Metreos.Types.Presence;
using Metreos.Types.Presence.Pidf;

namespace UnitTest
{

    /// <summary>
    /// Summary description for Class1
    /// </summary>
    [TestFixture]
    public class ListNotificationTests
    {
        public const string DataInputPath = "NativeTypes\\Presence\\input";

        private LogWriter log;

        public ListNotificationTests()
        {
            this.log = new Metreos.LoggingFramework.LogWriter(TraceLevel.Verbose, "UnitTest");
        }

        [Test]
        public void ParseListNotificationPositive()
        {
            //load the message
            TextReader reader = new StreamReader(Path.Combine(DataInputPath, "multipart2.txt"));
            StringBuilder buffer = new StringBuilder();
            string tmp;
            while((tmp = reader.ReadLine()) != null)
            {
                buffer.Append(tmp).Append("\r\n");

            }
            
            PresenceNotification ln = new PresenceNotification(log);
            Assert.IsNotNull(ln.Parse(buffer.ToString()));
            Assert.IsNotNull(ln.ResourceList);
            Assert.IsNotNull(ln.ResourceList.Resources);
            ResourceList rl = ln.ResourceList;
            Assert.IsTrue(rl.Resources.Count == 4);

            string xml = ((Resource) rl.Resources["sip:metreos@cisco.com"]).Pidf;
            TextReader reader1 = new StringReader(xml);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader1);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("dm", "urn:ietf:params:xml:ns:pidf:data-model");
            nsmgr.AddNamespace("pidf", "urn:ietf:params:xml:ns:pidf");
            nsmgr.AddNamespace("rpid", "urn:ietf:params:xml:ns:pidf:rpid");
            nsmgr.AddNamespace("crpid", "urn:cisco:params:xml:ns:pidf:rpid");

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("presence", nsmgr);
            nodes = doc.DocumentElement.SelectNodes("//pidf:tuple", nsmgr);
            nodes = doc.DocumentElement.SelectNodes("//status", nsmgr);
            nodes = doc.SelectNodes("/presence/dm:person", nsmgr);
            Assert.IsTrue(nodes.Count==1);

            nodes = doc.SelectNodes("/presence/dm:person/rpid:activities", nsmgr);
            Assert.AreEqual(1, nodes.Count);
            nodes = doc.SelectNodes("/presence/dm:person/rpid:activities/crpid:unavailable", nsmgr);
            Assert.AreEqual(1, nodes.Count);
        }

        [Test]
        public void ParseListNotification1()
        {
            PresenceNotification ln = SimpleParse("notify1.txt");
            Assert.IsNotNull(ln.ResourceList.Resources);
            ResourceList rl = ln.ResourceList;
            Assert.IsTrue(rl.Resources.Count == 4);

            Hashtable resources = ln.ResourceList.Resources;
            string xml = ((Resource) rl.Resources["sip:metreos@cisco.com"]).Pidf;

            Metreos.Types.Presence.Resource r = (Metreos.Types.Presence.Resource) rl.Resources["sip:metreos@cisco.com"];
            Assert.AreEqual(r.Presence.tuple[0].status.basic, Metreos.Types.Presence.Pidf.basic.closed);

            r = (Metreos.Types.Presence.Resource) rl.Resources["sip:tom@cisco.com"];
            Assert.AreEqual(r.Presence.tuple[0].status.basic, Metreos.Types.Presence.Pidf.basic.closed);

            r = (Metreos.Types.Presence.Resource) rl.Resources["sip:jan@cisco.com"];
            Assert.AreEqual(r.Presence.tuple[0].status.basic, Metreos.Types.Presence.Pidf.basic.open);

            r = (Metreos.Types.Presence.Resource) rl.Resources["sip:tuser1@cisco.com"];
            Assert.AreEqual(r.Presence.tuple[0].status.basic, Metreos.Types.Presence.Pidf.basic.closed);

        }

        [Test]
        public void ParseListNotification2()
        {
            PresenceNotification ln = SimpleParse("notify4.txt");
        }

        [Test]
        public void SimpleParse()
        {
            string[] files = {"notify2.txt", "notify3.txt", "notify4.txt", "notify5.txt", 
                "periodic_notify1.txt", "periodic_notify2.txt", "periodic_notify3.txt",
                "periodic_notify4.txt"};

            foreach(string f in files)
                SimpleParse(f);
        }

        private PresenceNotification SimpleParse(string file)
        {
            //load the message
            TextReader reader = new StreamReader(DataInputPath+"/" + file);
            StringBuilder buffer = new StringBuilder();
            string tmp;
            while((tmp = reader.ReadLine()) != null)
            {
                buffer.Append(tmp).Append("\r\n");

            }

            PresenceNotification ln = new PresenceNotification(log);
            Assert.IsNotNull(ln.Parse(buffer.ToString()));

            return ln;
        }

        [Test]
        public void ParseListNotificationNegative()
        {
            //load the message
            TextReader reader = new StreamReader(Path.Combine(DataInputPath, "missinguri.txt"));
            StringBuilder buffer = new StringBuilder();
            string tmp;
            while((tmp = reader.ReadLine()) != null)
            {
                buffer.Append(tmp).Append("\r\n");

            }

            PresenceNotification ln = new PresenceNotification(log);
            ln.Parse(buffer.ToString());
            Assert.IsNull(ln.ResourceList);
        }

        [Test]
        public void ParseListNotificationInvalidXml()
        {
            //load the message
            TextReader reader = new StreamReader(Path.Combine(DataInputPath, "invalidxml.txt"));
            StringBuilder buffer = new StringBuilder();
            string tmp;
            while((tmp = reader.ReadLine()) != null)
            {
                buffer.Append(tmp).Append("\r\n");

            }

            PresenceNotification ln = new PresenceNotification(log);
            ln.Parse(buffer.ToString());
            Assert.IsNull(ln.ResourceList);
        }

        [Test]
        public void ParsePidf()
        {
            TextReader reader = new StreamReader(Path.Combine(DataInputPath, "pidf.xml"));
            
            XmlSerializer xs = new XmlSerializer(typeof(presence));
            presence p = (presence) xs.Deserialize(reader);
            Assert.IsNotNull(p);
            Assert.IsNotNull(p.tuple);
            Assert.IsTrue(p.tuple.Length==3);
            Assert.IsTrue(p.tuple[0].status.basic==basic.open);

            Assert.IsNotNull(p.person);
            Assert.IsNotNull(p.device);
            Assert.IsTrue(p.device.Length==2);
            Assert.IsTrue(p.device[0].deviceID.Equals("mac:1234567890"));
            Assert.IsTrue(p.device[0].id.Equals("d1"));
        }

        [Test]
        public void PresenceNotificationParsePidf()
        {
            //load the message
            TextReader reader = new StreamReader(Path.Combine(DataInputPath, "pidf.xml"));
            StringBuilder buffer = new StringBuilder();
            string tmp;
            while((tmp = reader.ReadLine()) != null)
            {
                buffer.Append(tmp).Append("\r\n");

            }
            
            PresenceNotification pn = new PresenceNotification(log);
            pn.Parse(buffer.ToString());

            Assert.IsNotNull(pn.Resource);
            Assert.IsNotNull(pn.Resource.Presence);
            Assert.IsNotNull(pn.Resource.Presence.tuple);
            Assert.IsTrue(pn.Resource.Presence.tuple.Length==3);
            Assert.IsTrue(pn.Resource.Presence.tuple[0].status.basic==basic.open);

        }
    }

}
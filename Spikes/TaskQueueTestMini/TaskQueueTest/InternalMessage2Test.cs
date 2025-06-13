using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace TaskQueueTest
{
    [TestFixture]
    public class InternalMessage2Test
    {
        [Test]
        public void testAddAndGetField()
        {
            InternalMessage msg = new InternalMessage();
            msg.AddField("stringVal", "someString");
            Assert.AreEqual("someString", msg["stringVal"]);
            Assert.AreEqual(1, msg.Count);

            msg = new InternalMessage();
            msg.AddField("intVal", 1234);
            Assert.AreEqual(1234, msg["intVal"]);
            Assert.AreEqual(1, msg.Count);
        }

        [Test]
        public void testAddAndGetMultipleFieldsSameName()
        {
            InternalMessage msg = new InternalMessage();
            msg.AddField("strVal", "first");
            msg.AddField("strVal", "second");
            msg.AddField("strVal", "third");
            msg.AddField("strVal", "fourth");

            object[] vals = msg.GetFields("strVal");
            Assert.IsNotNull(vals);
            Assert.AreEqual(4, vals.Length);
            Assert.AreEqual(4, msg.Count);

            Assert.AreEqual("first", vals[0]);
            Assert.AreEqual("second", vals[1]);
            Assert.AreEqual("third", vals[2]);
            Assert.AreEqual("fourth", vals[3]);
        }

        [Test]
        public void testAddAlotOfFields()
        {
            InternalMessage msg = new InternalMessage();
            for(int i = 0; i < 100; i++)
            {
                msg.AddField(i.ToString(), i);
            }
            Assert.AreEqual(100, msg.Count);

            for(int i = 0; i < 100; i++)
            {
                Assert.AreEqual(i, msg[i.ToString()]);
            }
        }

        [Test]
        public void testAddFieldWithNullValue()
        {
            InternalMessage msg = new InternalMessage();
            msg.AddField("someNullvalue", null);
            Assert.AreEqual(0, msg.Count);
            Assert.IsNull(msg["someNullValue"]);
        }

        [Test]
        public void testFieldsProperty()
        {
            InternalMessage msg = new InternalMessage();
            for(int i = 0; i < 10; i++)
            {
                msg.AddField(i.ToString(), i);
            }
            
            ArrayList fields = msg.Fields;
            Assert.IsNotNull(fields);
            Assert.AreEqual(10, fields.Count);

            for(int i = 0; i < 10; i++)
            {
                Field f = fields[i] as Field;
                Assert.IsNotNull(f);
                Assert.AreEqual(i.ToString(), f.Name);
                Assert.AreEqual(i, f.Value);
            }
        }

        [Test]
        public void testFieldsPropertyNoFields()
        {
            InternalMessage msg = new InternalMessage();
            ArrayList fields = msg.Fields;
            Assert.IsNotNull(fields);
            Assert.AreEqual(0, fields.Count);
        }

        [Test]
        public void testContains()
        {
            InternalMessage msg = new InternalMessage();
            msg.AddField("someField", "someValue");
            msg.AddField("anotherField", 1234);
            msg.AddField(null, "shouldNotGoIn");
            Assert.IsTrue(msg.Contains("someField"));
            Assert.IsTrue(msg.Contains("anotherField"));
            Assert.IsFalse(msg.Contains("bogusField"));
            Assert.IsFalse(msg.Contains(null));
        }

        [Test]
        public void testContainsNoFields()
        {
            InternalMessage msg = new InternalMessage();
            Assert.IsFalse(msg.Contains("someBogusKey"));
            Assert.IsFalse(msg.Contains(null));
        }

        [Test]
        public void testRemove()
        {
            InternalMessage msg = new InternalMessage();
            msg.AddField("field1", true);
            msg.AddField("field2", "blah");
            msg.AddField("field3", 1234);
            Assert.AreEqual(3, msg.Count);

            object retVal = msg.RemoveField(null);
            Assert.IsNull(retVal);
            Assert.AreEqual(3, msg.Count);

            retVal = msg.RemoveField("field2");
            Assert.AreEqual("blah", retVal);
            Assert.AreEqual(2, msg.Count);

            retVal = msg.RemoveField("field1");
            Assert.IsTrue((bool)retVal);
            Assert.AreEqual(1, msg.Count);

            retVal = msg.RemoveField("field3");
            Assert.AreEqual(1234, retVal);
            Assert.AreEqual(0, msg.Count);
        }
        
        [Test]
        public void testRemoveNoFields()
        {
            InternalMessage msg = new InternalMessage();
            object retVal = msg.RemoveField("someBogusField");
            Assert.IsNull(retVal);
            Assert.AreEqual(0, msg.Count);
        }

        [Test]
        public void testToString()
        {
            string expectedString =
                "InternalMessage:" + Environment.NewLine +
                "  Type: bogusType" + Environment.NewLine +
                "  Message ID: unspecified" + Environment.NewLine +
                "  Routing GUID: unspecified" + Environment.NewLine +
                "  Source: unspecified" + Environment.NewLine +
                "  Destination: unspecified" + Environment.NewLine +
                "Fields:" + Environment.NewLine +
                "  field1: someStrValue" + Environment.NewLine +
                "  field2: 1234" + Environment.NewLine + 
                "  field3: True" + Environment.NewLine;

            InternalMessage msg = new InternalMessage();
            msg.AddField("field1", "someStrValue");
            msg.AddField("field2", 1234);
            msg.AddField("field3", true);

            Assert.AreEqual(expectedString, msg.ToString("bogusType", null));
        }

        [Test]
        public void testCountProperty()
        {
            InternalMessage msg = new InternalMessage();
            for(int i = 0; i < 100; i++)
            {
                msg.AddField(i.ToString(), i);
            }
            Assert.AreEqual(100, msg.Count);
            Assert.AreEqual(100, msg.Fields.Count);
        }

        [Test]
        public void testIsCompleteProperty()
        {
            InternalMessage msg = new InternalMessage();
            Assert.IsFalse(msg.IsComplete);

            msg.Source = "someSource";
            Assert.IsFalse(msg.IsComplete);

            msg.MessageId = "someMessageId";
            Assert.IsTrue(msg.IsComplete);
        }

        [Test]
        public void testMessageIdProperty()
        {
            InternalMessage msg = new InternalMessage();
            Assert.IsNotNull(msg.MessageId);
            Assert.AreEqual(String.Empty, msg.MessageId);
            msg.MessageId = "blah";
            Assert.AreEqual("blah", msg.MessageId);
        }
    }
}

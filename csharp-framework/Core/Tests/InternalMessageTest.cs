using System;
using System.Xml.Serialization;

using Metreos.Samoa;

namespace Metreos.Samoa.Core.Tests
{
    /// <summary>
    /// Unit tests for Metreos.Samoa.Core.InternalMessage.
    /// </summary>
    public class InternalMessageTest
    {
        public InternalMessageTest()
        {
        }

        /// <summary>
        /// Test the serialization and deserialization of an InternalMessage.
        /// </summary>
        public void testSerializeDeserialize()
        {
            // Build a test message with some default values.
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "1234567890";
            im.Source = "Metreos.Provider.SIP";
            im.SourceQueue = "sourceQ";

            im.AddField(new Field("aName", "aValue"));
            im.AddField(new Field("bName", "bValue"));
            im.AddField(new Field("cName", "cValue"));

            // Serialize the message into XML. We'll keep it around in a StringBuilder.
            XmlSerializer serializer = new XmlSerializer(typeof(Core.InternalMessage));
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);

            serializer.Serialize(writer, im);
            writer.Close();

            // Deserialize the message from the string so we can verify the values are the
            // same.
            Core.InternalMessage newIm;

            System.IO.TextReader reader = new System.IO.StringReader(sb.ToString());

            newIm = (Core.InternalMessage)serializer.Deserialize(reader);
            reader.Close();

            // Check the values.
            csUnit.Assert.True(newIm.MessageId == im.MessageId);
            csUnit.Assert.True(newIm.Source == im.Source);

            csUnit.Assert.True(newIm.Fields.GetLength(0) == im.Fields.GetLength(0));

            int j = newIm.Fields.GetLength(0);

            for(int i = 0; i < j; i++)
            {
                csUnit.Assert.True(newIm.Fields[i].Name == im.Fields[i].Name);
                csUnit.Assert.True(newIm.Fields[i].Value == im.Fields[i].Value);
            }

            im = null;
            serializer = null;
            sb = null;
            writer = null;
            reader = null;
            newIm = null;
        }

        /// <summary>
        /// Test the field search functionality.
        /// </summary>
        public void testGetFieldByName()
        {
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "id";
            im.Source = "source";
            im.SourceQueue = "sourceQ";

            im.AddField(new Field("nameA", "valueA"));
            im.AddField(new Field("nameB", "valueB"));
            im.AddField(new Field("nameC", "valueC"));

            string retValue;
            bool foundField;
            
            foundField = im.GetFieldByName("nameB", out retValue);

            csUnit.Assert.True(foundField);
            csUnit.Assert.Equals("valueB", retValue);

            foundField = im.GetFieldByName("nameA", out retValue);

            csUnit.Assert.True(foundField);
            csUnit.Assert.Equals("valueA", retValue);

            foundField = im.GetFieldByName("ShouldNotFindThis", out retValue);

            csUnit.Assert.False(foundField);

            im = null;
            retValue = null;
        }

		public void testGetFieldsByName()
		{
			Core.InternalMessage im = new Core.InternalMessage();

			im.MessageId = "id";
			im.Source = "source";
			im.SourceQueue = "sourceQ";

			im.AddField(new Field("nameA", "valueA"));
			im.AddField(new Field("nameB", "valueB"));
			im.AddField(new Field("nameC", "valueC"));
			im.AddField(new Field("nameC", "valueD"));
			im.AddField(new Field("nameC", "valueE"));
			im.AddField(new Field("nameC", "valueF"));

			string[] retValues;
			bool foundField;
	            
			foundField = im.GetFieldsByName("nameC", out retValues);

			csUnit.Assert.True(foundField);
			csUnit.Assert.NotNull(retValues);
			csUnit.Assert.Equals(4, retValues.Length);
			csUnit.Assert.NotEquals("", retValues[0]);

			im = null;
			retValues = null;
		}

        /// <summary>
        /// Test the overload of ToString().
        /// </summary>
        public void testToString()
        {
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "id";
            im.Source = "source";
            im.SourceQueue = "sourceQ";
            im.AddField(new Field("nameA", "valueA"));

            string imString;

            imString = im.ToString();

            csUnit.Assert.True(imString != null);
            csUnit.Assert.True(imString.IndexOf("valueA") != -1);

            imString = null;
            im = null;
        }

        /// <summary>
        /// Test that the serialize/deserialize works with zero fields.
        /// </summary>
        public void testSerializeDeserializeWithZeroFields()
        {
            // Build a test message with some default values.
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "1234567890";
            im.Source = "Metreos.Provider.SIP";
            im.SourceQueue = "sourceQ";

            // Serialize the message into XML. We'll keep it around in a StringBuilder.
            XmlSerializer serializer = new XmlSerializer(typeof(Core.InternalMessage));
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);

            serializer.Serialize(writer, im);
            writer.Close();

            // Deserialize the message from the string so we can verify the values are the
            // same.
            Core.InternalMessage newIm;

            System.IO.TextReader reader = new System.IO.StringReader(sb.ToString());

            newIm = (Core.InternalMessage)serializer.Deserialize(reader);
            reader.Close();

            // Check the values.
            csUnit.Assert.True(newIm.MessageId == im.MessageId);
            csUnit.Assert.True(newIm.Source == im.Source);

            csUnit.Assert.True(newIm.Fields.GetLength(0) == im.Fields.GetLength(0));
            csUnit.Assert.True(newIm.Fields.GetLength(0) == 0);

            im = null;
            serializer = null;
            sb = null;
            writer = null;
            reader = null;
            newIm = null;
        }

        /// <summary>
        /// Test that the field search works with zero fields.
        /// </summary>
        public void testGetFieldByNameWithZeroFields()
        {
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "id";
            im.Source = "source";
            im.SourceQueue = "sourceQ";

            string retValue;
            bool foundField;

            foundField = im.GetFieldByName("ShouldNotFindThis", out retValue);

            csUnit.Assert.False(foundField);

            im = null;
            retValue = null;
        }

        public void testGetNullMessageIdAndSourceAndSourceQueue()
        {
            Core.InternalMessage im = new Core.InternalMessage();

            csUnit.Assert.NotNull(im.MessageId);
            csUnit.Assert.Equals("", im.MessageId);
            csUnit.Assert.NotNull(im.Source);
            csUnit.Assert.Equals("", im.Source);
            csUnit.Assert.NotNull(im.SourceQueue);
            csUnit.Assert.Equals("", im.SourceQueue);
        }

        public void testGetMessageNamespace()
        {
            Core.InternalMessage im = new Core.InternalMessage();
            im.MessageId = "Namespace.Message";

            csUnit.Assert.Equals("Namespace", im.GetNamespace());

            im.MessageId = "Namespace.Namespace2.Message";

            csUnit.Assert.Equals("Namespace.Namespace2", im.GetNamespace());

            im.MessageId = "Namespace.Namespace2.Namespace3.Message";

            csUnit.Assert.Equals("Namespace.Namespace2.Namespace3", im.GetNamespace());

            im.MessageId = "Namespace.Namespace2.Namespace3.Namespace4.Message";

            csUnit.Assert.Equals("Namespace.Namespace2.Namespace3.Namespace4", im.GetNamespace());

            im.MessageId = "Namespace.Namespace2.Namespace3.Namespace4.Namespace5.Message";

            csUnit.Assert.Equals("Namespace.Namespace2.Namespace3.Namespace4.Namespace5", im.GetNamespace());

            im.MessageId = "Namespace.Namespace2.Namespace3.Namespace4.Namespace5.Namespace6.Message";

            csUnit.Assert.Equals("Namespace.Namespace2.Namespace3.Namespace4.Namespace5.Namespace6", im.GetNamespace());

            im = null;
        }

        public void testGetNamespaceBad()
        {
            Core.InternalMessage im = new Core.InternalMessage();

            im.MessageId = "MessageNoNamespace";

            csUnit.Assert.Equals("", im.GetNamespace());

            im.MessageId = "";

            csUnit.Assert.Equals("", im.GetNamespace());

            im = null;
        }
    }
}

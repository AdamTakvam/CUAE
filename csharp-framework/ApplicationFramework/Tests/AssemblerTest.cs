using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.ApplicationFramework.Assembler;

namespace Metreos.ApplicationFramework.Tests
{
	public class AssemblerTest
	{
		public AssemblerTest()
		{
		}

        [csUnit.Test]
        public void TestAssembler()
        {
            #region Setup test output directory: x:\build\test\AppRoot

            DirectoryInfo dir = new DirectoryInfo("x:\\build\\test");
            csUnit.Assert.True(dir.Exists);

            dir = new DirectoryInfo("x:\\build\\test\\AppRoot");
            if(!dir.Exists)
            {
                dir.Create();
            }
            #endregion

            #region Deserialize test app in x:\build\test\TestScript.xml

            FileInfo testFile = new FileInfo("x:\\build\\test\\TestScript.xml");
            FileStream testFileStream = testFile.OpenRead();
            XmlSerializer serializer = new XmlSerializer(typeof(XmlScriptData));
            XmlScriptData xmlScriptData = (XmlScriptData) serializer.Deserialize(testFileStream);
            
            #endregion

            Assembler.Assembler assembler = new Assembler.Assembler("x:\\build\\framework", "x:\\build\\test\\AppRoot");
            
            ScriptData script = assembler.AssembleScript(xmlScriptData, "TestApplication", "1.0", "1.0");

            csUnit.Assert.NotNull(script);
        }
	}
}

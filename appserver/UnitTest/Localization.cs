using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.AppServer.ARE;
using Metreos.Configuration;
using Metreos.Interfaces;

using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class Localization
    {
        private Config config;

        [SetUp]
        public void Setup()
        {
            // Add app in DB
            this.config = Config.Instance;

            ComponentInfo cInfo = new ComponentInfo();
            cInfo.name = AppName;
            cInfo.type = IConfig.ComponentType.Application;
            cInfo.version = AppVersion;
            cInfo.status = IConfig.Status.Enabled_Running;
            config.AddComponent(cInfo, "en-US");

            // Set up app metadata
            AppEnvironment.AppMetaData = new AppMetaData(AppName, AppVersion, "1,0", null, null);

            // Create directories
            Config.ApplicationDir.Create();

            DirectoryInfo dir = new DirectoryInfo(Config.RootPath);
            dir = dir.Parent;
            dir = dir.CreateSubdirectory("Framework");
            dir = dir.CreateSubdirectory("1.0");
        }

        [Test]
        public void PopulateLocaleDataTest()
        {
            // Instantiate SchedulerTask
            LogWriter log = new LogWriter(TraceLevel.Verbose, "UnitTest");
            Repository rep = new Repository(log);
            SchedulerTask st = new SchedulerTask(rep, TraceLevel.Verbose);

            // Put StringTable in DB
            ConfigEntry cEntry = new ConfigEntry(IConfig.Entries.Names.STRING_TABLE, StringTable, null, IConfig.StandardFormat.String, false);
            config.AddEntry(IConfig.ComponentType.Application, AppName, cEntry);

            // Set up media file paths
            DirectoryInfo dir = Config.ApplicationDir;
            dir = dir.CreateSubdirectory(AppName);
            dir = dir.CreateSubdirectory(AppVersion);
            dir = dir.CreateSubdirectory(IConfig.AppDirectoryNames.MEDIA_FILES);
            dir.CreateSubdirectory(MediaLocale);

            // Run target method
            st.PopulateLocaleData();

            // Validate results
            Assert.IsNotNull(AppEnvironment.AppMetaData.Locales);
            Assert.AreEqual(AppEnvironment.AppMetaData.Locales.Count, TotalLocales);
            Assert.IsNotNull(AppEnvironment.AppMetaData.StringTable);
            Assert.AreEqual(AppEnvironment.AppMetaData.StringTable.GetTableValue("Hello", "fr-FR"), "Salut");
            Assert.IsNull(AppEnvironment.AppMetaData.StringTable.GetTableValue("Hello", MediaLocale));
            Assert.AreEqual(AppEnvironment.AppMetaData.StringTable.GetLocales().Count, NumTableLocales);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove app
            config.RemoveComponent(IConfig.ComponentType.Application, AppName);

            // Delete directories
            Config.ApplicationDir.Delete(true);
            config.FrameworkDir.Delete(true);
        }

        private const string AppName        = "UnitTest";
        private const string AppVersion     = "1.0";
        private const string MediaLocale    = "en-UK";

        private const int NumTableLocales   = 3;
        private const int NumMediaLocales   = 1;
        private const int TotalLocales      = NumTableLocales + NumMediaLocales;

        private const string StringTable = @"<?xml version=""1.0""?>
<LocaleTable xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://www.cisco.com/LocaleTable.xsd"">
  <Locales readonly=""false"">
    <Locale name=""fr-FR"" devmode=""false"" width=""100"">
      <PromptInfo ref=""Hello"">
        <Value><![CDATA[Salut]]></Value>
      </PromptInfo>
    </Locale>
    <Locale name=""en-US"" devmode=""false"" width=""100"">
      <PromptInfo ref=""Hello"">
        <Value><![CDATA[Hi]]></Value>
      </PromptInfo>
    </Locale>
    <Locale name=""pl-PL"" devmode=""false"" width=""100"">
      <PromptInfo ref=""Hello"">
        <Value><![CDATA[Cześć]]></Value>
      </PromptInfo>
    </Locale>
  </Locales>
  <Prompts>
    <Prompt name=""Hello"" height=""22"" />
  </Prompts>
</LocaleTable>";
    }
}

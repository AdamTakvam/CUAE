using System;
using System.IO;
using System.Collections.Generic;

using Metreos.AppArchiveCore.Xml;
using Metreos.AppServer.ARE.Collections;

namespace Metreos.AppServer.ARE
{
    [Serializable]
	public class AppMetaData
	{
        public readonly StringTable StringTable;

        public readonly string Name = null;
        public readonly string Version = null;
        public readonly string FwVersion = null;
        
        public readonly List<string> Databases = null;
        public readonly List<FileInfo> ScriptFiles = null;

        public List<string> Locales = null;

        public AppMetaData(string name, string version, string fwVersion, 
            List<string> databases, List<FileInfo> scriptFiles)
        {
            this.Name = name;
            this.Version = version;
            this.FwVersion = fwVersion;
            this.Databases = databases;
            this.ScriptFiles = scriptFiles;

            this.StringTable = new StringTable();
        }
	}
}

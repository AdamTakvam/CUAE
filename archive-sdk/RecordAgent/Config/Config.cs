using System;
using System.IO;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for Config.
	/// </summary>
	public class Config
	{
        static Config instance = null;
        static readonly object configlock = new object();

        private string userRecordPath;
        public string UserRecordPath { get { return userRecordPath; } }

        private string userDataPath;
        public string UserDataPath { get { return userDataPath; } }

        private string userAppPath;
        public string UserAppPath { get { return userAppPath; } }

        public Config()
        {
            userAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Metreos\\RecordAgent");
            if (!Directory.Exists(userAppPath))
                Directory.CreateDirectory(userAppPath);
            userDataPath = Path.Combine(userAppPath, "Data");
            if (!Directory.Exists(userDataPath))
                Directory.CreateDirectory(userDataPath);
            userRecordPath = Path.Combine(userAppPath, "Record");
            if (!Directory.Exists(userRecordPath))
                Directory.CreateDirectory(userRecordPath);
        }

        public static Config Instance
        {
            get
            {
                lock (configlock)
                {
                    if (instance == null)
                    {
                        instance = new Config();
                    }
                    return instance;
                }
            }
        }
    }
}

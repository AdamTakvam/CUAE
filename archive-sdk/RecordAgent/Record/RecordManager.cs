using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for RecordManager.
	/// </summary>
    public class RecordManager
    {
        static RecordManager instance = null;
        static readonly object padlock = new object();

        private Config config;

        public RecordManager()
        {
            config = Config.Instance;
        }

        public static RecordManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RecordManager();
                    }
                    return instance;
                }
            }
        }

        public void WriteRecord(Record r, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Record));
            TextWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, r);
            writer.Close();
        }

        public Record ReadRecord(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Record));
            FileStream fs = new FileStream(filePath, FileMode.Open);
            Record r;
            r = (Record)serializer.Deserialize(fs);
            fs.Close();

            if (r.callDateTime == null)
                r.callDateTime = "";

            if (r.key == null)
                r.key = "";

            if (r.name == null)
                r.name = "";

            if (r.number == null)
                r.number = "";

            if (r.topic == null)
                r.topic = "";

            return r;
        }

        public int RemoveRecord(string key)
        {
            int duration = 0;
            string recordFilePath = Path.Combine(config.UserRecordPath, key + ".xml");
            string noteFilePath = Path.Combine(config.UserDataPath, key + ".rtf");

            Record r = ReadRecord(recordFilePath);
            if (r != null)
            {
                int numFiles = r.AtomicAudioFiles.Length;

                for (int i=0; i<numFiles; i++)
                {   
                    string sFilePath = r.AtomicAudioFiles[i].fileName;
                    duration += r.AtomicAudioFiles[i].duration;
                    File.Delete(sFilePath);
                }
            }
            File.Delete(recordFilePath);
            File.Delete(noteFilePath);

            return duration;
        }

        public void SaveRecord(string key, string targetFolder)
        {
            string recordFilePath = Path.Combine(config.UserRecordPath, key + ".xml");
            string noteFilePath = Path.Combine(config.UserDataPath, key + ".rtf");

            string sFileName, sSourceFolder;

            Record r = ReadRecord(recordFilePath);
            if (r != null)
            {
                int numFiles = r.AtomicAudioFiles.Length;

                for (int i=0; i<numFiles; i++)
                {   
                    sFileName = Path.GetFileName(r.AtomicAudioFiles[i].fileName);
                    sSourceFolder = Path.GetDirectoryName(r.AtomicAudioFiles[i].fileName);
                    try
                    {
                        File.Copy(Path.Combine(sSourceFolder, sFileName), Path.Combine(targetFolder, sFileName), true);
                    }
                    catch
                    {
                    }
                }
            }
            sFileName = Path.GetFileName(recordFilePath);
            sSourceFolder = Path.GetDirectoryName(recordFilePath);
            try
            {
                File.Copy(Path.Combine(sSourceFolder, sFileName), Path.Combine(targetFolder, sFileName), true);
            }
            catch
            {
            }

            sFileName = Path.GetFileName(noteFilePath);
            sSourceFolder = Path.GetDirectoryName(noteFilePath);
            try
            {
                File.Copy(Path.Combine(sSourceFolder, sFileName), Path.Combine(targetFolder, sFileName), true);
            }
            catch
            {
            }
        }

        public void ToggleStar(string key, bool flag)
        {
            string recordFilePath = Path.Combine(config.UserRecordPath, key + ".xml");
            Record r = ReadRecord(recordFilePath);
            if (r != null)
            {
                r.flag = flag;
                WriteRecord(r, recordFilePath);
            }
        }

    }
}

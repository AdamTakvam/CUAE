using System;
using System.IO;
using System.Text;

namespace Metreos.RecordAgent
{
    public delegate void SearchDelegate(string filename);
    public delegate void SearchCompleted();

	/// <summary>
	/// Summary description for SearchRecord.
	/// </summary>
	public class SearchRecord
	{
        public string PathName;
        public string DataPathName;
        public string FindText;
        public string FileExt;

        public static bool bStopIt = false;
        private DirectoryInfo dir = null;
        private FileInfo[] files = null;
        private RecordManager recordManager;

        public event SearchDelegate OnRecordFound;
        public event SearchCompleted OnSearchCompleted;

        public SearchRecord()
        {
            recordManager = RecordManager.Instance;
        }

        private  bool CheckValidDir()
        {
            bool bExist = false;
            if (bExist = Directory.Exists(PathName))
                dir = new DirectoryInfo(PathName);

            return bExist;
        }

        public void StopSearch()
        {
            bStopIt = true;
        }

        public void StartSearch()
        {
            bStopIt = false;

            if (!CheckValidDir())
            {
                if (OnSearchCompleted != null)
                    OnSearchCompleted();
                return;
            }

            files = dir.GetFiles(FileExt);
            int i = 0;
            int nFile = files.Length;
            while (i < nFile && (!bStopIt))
            {
                SearchFileContent(files[i].FullName);
                i++ ;
            }

            if (OnSearchCompleted != null)
                OnSearchCompleted();
        }

        private void SearchFileContent(string FileName)
        {
            FileStream file = null;
            long nLen = 0;
            bool bFound = false;

            Record r = recordManager.ReadRecord(FileName);
            if (r == null)
                return;

            try
            {
                string str = r.GetDataString().ToLower();
                string lowerFindText = FindText.ToLower();
                if (str.IndexOf(lowerFindText) > -1)
                {
                    if (OnRecordFound != null)
                        OnRecordFound(FileName);

                    bFound = true;
                }
            }
            catch
            {
            }

            if (bFound)
                return;

            // Read meta data to find it if searching text is not in record.                    
            string metaDataFile = Path.Combine(DataPathName, r.key + ".rtf");
            try
            {
                file = new FileStream(metaDataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                nLen = file.Length;
                byte[] buff = new byte[file.Length];
                file.Position = 0;
                file.Read(buff, 0, (int)nLen);
                string str = Encoding.ASCII.GetString(buff).ToLower();
                string lowerFindText = FindText.ToLower();
                if (str.IndexOf(lowerFindText) > -1)
                {
                    if (OnRecordFound != null)
                        OnRecordFound(FileName);
                }
                buff = null;
            }
            catch
            {                
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
    }
}

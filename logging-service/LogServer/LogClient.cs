using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.Interfaces;
using Metreos.Core.IPC.Flatmaps.LogServer;


namespace Metreos.LogServer
{
    public delegate void OnFileErrorDelegate(string name, string msg);

	/// <summary>
	/// Log client
	/// </summary>
	/// 
    public class LogClient
    {
        #region Container class for queued message
        public class LogMessage
        {
            public TraceLevel LogLevel { get { return logLevel; } }
            public string Message { get { return message; } }
            public string TimeStamp { get { return timeStamp; } }

            private TraceLevel logLevel;                // log level
            private string message;                     // log message
            private string timeStamp;                   // time stamp

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="logLevel"></param>
            /// <param name="message"></param>
            /// <param name="timeStamp"></param>
            public LogMessage(TraceLevel logLevel, string message, string timeStamp)
            {
                this.logLevel = logLevel;
                this.message  = message;
                this.timeStamp = timeStamp;
            }
        }
        #endregion

        #region File Date Comparer Helper Class
        private class FilesDateComparer: IComparer  
        { 
            /// <summary>
            /// Compare two FileInfo objects bt last modified time.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare (object x, object y)  
            { 
                int iResult = 0; 

                FileInfo oFileX = x as FileInfo; 
                FileInfo oFileY = y as FileInfo;
 
                if (oFileX == null || oFileY == null)
                    return iResult;

                if(oFileX.LastWriteTime == oFileY.LastWriteTime) 
                { 
                    iResult = 0; 
                } 
                else if(oFileX.LastWriteTime > oFileY.LastWriteTime) 
                { 
                    iResult = 1; 
                } 
                else 
                { 
                    iResult = -1; 
                } 
             
                return iResult; 
            } 
        }
        #endregion 

        public uint MaximumLinesToWrite  { get { return maximumLinesToWrite; } set { maximumLinesToWrite = value; } }

        private const string LOG_TIMESTAMP_FORMAT   = "yyyy:MM:dd::HH:mm:ss(ff)";
        private const string FILE_TIMESTAMP_FORMAT  = "yyyyMMdd-HHmmssff";
        private const string FILE_LOG_EXTENSION     = ".log";

        public string Name { get { return name; } set { name = value; } }
        public string Path { get { return path; } set { path = value; } } 

        private string name;                // file name
        private string path;                // file path

        private StreamWriter writer;        // stream writer
        private int numLinesWritten;        // number of written lines
        private object writerLock;          // writer object lock
        private object numLinesWrittenLock; // counter lock
        private uint maximumLinesToWrite;   // max lines to write
        private uint maximumFiles;          // max number of files to keep

        public event OnFileErrorDelegate onFileError;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="maximumLinesToWrite"></param>
        /// <param name="maximumFiles"></param>
        public LogClient(string path, string name, uint maximumLinesToWrite, uint maximumFiles)
        { 
            this.path                   = path;
            this.name                   = name;
            this.numLinesWritten        = 0;
            this.maximumLinesToWrite    = maximumLinesToWrite;
            this.maximumFiles			= maximumFiles;
            this.writerLock             = new object();
            this.numLinesWrittenLock    = new object();
        }

        /// <summary>
        /// Create a log file based on the pass in time stamp
        /// </summary>
        /// <param name="ftStamp"></param>
        public void CreateLogFile(string ftStamp)
        {
            string logFolder = System.IO.Path.Combine(path, name);

            if(Directory.Exists(logFolder) == false)
            {
                // The log directory doesn't currently exist, so create it.
                try
                {
                    Directory.CreateDirectory(logFolder);
                }
                catch
                {
                    // Can't create the log directory, therefore we can't 
                    // have a valid file log writer.

                    if (this.onFileError != null)
                        onFileError(this.name, "CreateDirectory failed");
                }
            }

            // Remove any old files to maintain file count to a pre-defined level
            DirectoryInfo dir = new DirectoryInfo(logFolder);
            string findWhat = "*" + FILE_LOG_EXTENSION;
            FileInfo[] files = dir.GetFiles(findWhat);
            if (files.Length >= maximumFiles)
            {
                FilesDateComparer fileDateComparer = new FilesDateComparer();
                Array.Sort(files, fileDateComparer);

                int numFileToRemove = (int)(files.Length-maximumFiles);
                for (int i=0; i<=numFileToRemove; i++)
                {
                    try
                    {
                        FileInfo fi = (FileInfo)files[i];
                        if (File.Exists(fi.FullName))
                        {
                            File.Delete(fi.FullName);
                        }
                    }
                    catch
                    {
                        if (this.onFileError != null)
                            onFileError(this.name, "Delete file failed.");
                    }
                }
            }


            string fileTimeStamp = DateTime.Now.ToString(FILE_TIMESTAMP_FORMAT);
            if (ftStamp != null)
            {
                // Parse file time stamp to create a file time stamp
                string ft = ftStamp.Substring(0, ftStamp.Length - 4);   // trim (xx)
                try
                {
                    ft = ft.Replace("::", ":");
                    string ca = ":";
                    string[] fts = ft.Split(ca.ToCharArray());                    
                    ft = String.Format("{0}/{1}/{2}T{3}:{4}:{5}", fts[0], fts[1], fts[2], fts[3], fts[4], fts[5]);
                    DateTime dt = DateTime.Parse(ft);
                    fileTimeStamp = dt.ToString(FILE_TIMESTAMP_FORMAT);
                }
                catch (Exception e)
                {
                    if (this.onFileError != null)
                        onFileError(this.name, "Parsing error for " + ft + " " + e.ToString());
                }
            }

            string fileName = System.IO.Path.Combine(logFolder, System.IO.Path.ChangeExtension(fileTimeStamp, FILE_LOG_EXTENSION));

            try
            {
                writer = new StreamWriter(fileName, true, System.Text.Encoding.ASCII, 8192);
                writer.AutoFlush = true;

                lock(numLinesWrittenLock)
                {
                    numLinesWritten = 0;
                }
            }
            catch
            {
                // Can't create this particular log file, therefore we can't
                // have a valid file log writer.

                // REFACTOR Eventually, we need to add some appropriate error
                // output here. Why did it fail? Not enough free disk space?
                // Permissions error? Etc, etc.
                if (this.onFileError != null)
                    onFileError(this.name, "CreateText failed");
            }
        }

        /// <summary>
        /// Write a log statement into file
        /// </summary>
        /// <param name="level">log level</param>
        /// <param name="message">log message</param>
        /// <param name="timeStamp">time stamp</param>
        public void WriteLog(TraceLevel level, string message, string timeStamp)
        {
            lock(writerLock)
            {
                if (writer == null)
                {
                    if (this.onFileError != null)
                        onFileError(this.name, "WriteLog failed, writer is null");
                    return;
                }

                string logTimeStamp;
                if (timeStamp == null)
                    logTimeStamp = System.DateTime.Now.ToString(LOG_TIMESTAMP_FORMAT);
                else
                    logTimeStamp = timeStamp;

                try
                {
                    lock(numLinesWrittenLock)
                    {
                        this.numLinesWritten++;

                        if(this.numLinesWritten > this.maximumLinesToWrite)
                        {
                            NewLog(logTimeStamp);
                        }
                    }

                    writer.WriteLine("{0}: {1}: {2}", logTimeStamp, level, message);
                }
                catch (Exception e)
                {
                    if (this.onFileError != null)
                        onFileError(this.name, "WriteLine failed " + e.ToString());
                }
            }
        }

        /// <summary>
        /// Create a new log file
        /// </summary>
        /// <param name="ftStamp">Base on the timestamp to create log file, 
        /// if null then use current time stamp
        /// </param>
        public void NewLog(string ftStamp)
        {
            lock(writerLock)
            {
                try
                {
                    writer.Flush();
                }
                catch
                {}

                try
                {
                    writer.Close();
                }
                catch
                {}

                writer = null;

                CreateLogFile(ftStamp);
            }
        }

        /// <summary>
        /// Disposer
        /// </summary>
        public void Dispose()
        {
            lock(writerLock)
            {
                if(writer != null)
                {
                    try
                    {
                        writer.Flush();
                    }
                    catch
                    {}

                    try
                    {
                        writer.Close();
                    }
                    catch
                    {}

                    writer = null;
                }
            }
        }
    }
}

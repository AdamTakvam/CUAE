using System;
using System.Diagnostics;
using System.IO;

using Metreos.LoggingFramework;

namespace Metreos.Samoa.Core
{
    /// <summary>
    /// Implementation of a logger sink that writes log messages
    /// to a file.
    /// </summary>
    public sealed class TestFileLoggerSink : LoggerSinkBase
    {
        private StreamWriter writer;

        private const string LOG_TIMESTAMP_FORMAT   = "HH:mm:ss(ff)";
        private const string FILE_TIMESTAMP_FORMAT  = "yyyyMMdd-HHmmssff";
        private const string FILE_LOG_DIRECTORY     = "Logs";
        private const string FILE_LOG_EXTENSION     = ".log";

        private int numLinesWritten = 0;
        private int maximumLinesToWrite = 1000000;

        private string logOpenTime;
        private string logClosedTime = "<currently Still Open>";
        private volatile int numTimesLogWrapped = 0;

        public TestFileLoggerSink() : base()
        {
            this.logOpenTime = System.DateTime.Now.ToString();

            this.CreateLogFile();

            this.WriteHeader();

            RefreshConfiguration();
        }

        public override void RefreshConfiguration()
        {
            this.SubscribeToLoggerEvents(TraceLevel.Verbose);
        }


        public void Cleanup()
        {
            this.logClosedTime = System.DateTime.Now.ToString();

            this.WriteLastLogEntryMarker();

            if(this.PlaceStreamAtBeginning() == true)
            {
                this.WriteHeader();
            }

            this.PlaceStreamAtEnd();

            if(writer != null)
            {
                writer.Close();
            }
        }


        /// <summary>
        /// Invoked by Logger to write a log message to this sink.
        /// </summary>
        /// <param name="errorLevel">The error level of the message.</param>
        /// <param name="message">The message to write.</param>
        public override void LoggerWriteCallback(TraceLevel errorLevel, string message)
        {
            Debug.Assert(writer != null, "writer is null");
			
            string logTimeStamp = System.DateTime.Now.ToString(LOG_TIMESTAMP_FORMAT);

            try
            {
                lock(writer)
                {
                    writer.WriteLine("{0}: {1}: {2}", logTimeStamp, errorLevel, message);
                }

                this.numLinesWritten++;

                if(this.numLinesWritten >= this.maximumLinesToWrite)
                {
                    this.WrapLogFile();
                }
            }
            catch
            {
                // REFACTOR We need a better way to report this condition.
                Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
            }
        }


        /// <summary>
        /// Write header information to the file log.
        /// </summary>
        private void WriteHeader()
        {
            Debug.Assert(writer != null, "writer is null");

            try
            {
                lock(writer)
                {
                    writer.WriteLine("Log Opened        : {0}", this.logOpenTime);
                    writer.WriteLine("Log Closed        : {0}", this.logClosedTime);
                    writer.WriteLine("Times Log Wrapped : {0}", this.numTimesLogWrapped);
                    writer.WriteLine("-----");
                    writer.WriteLine();
                }
            }
            catch
            {
                // REFACTOR We need a better way to report this condition.
                Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
            }
        }


        /// <summary>
        /// Write footer information to the file log.
        /// </summary>
        private void WriteLastLogEntryMarker()
        {
            Debug.Assert(writer != null, "writer is null");

            try
            {
                lock(writer)
                {
                    writer.WriteLine(">>>> LAST LOG ENTRY <<<<");
                    writer.WriteLine();
                }
            }
            catch
            {
                // REFACTOR We need a better way to report this condition.
                Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
            }
        }


        /// <summary>
        /// Create the log file. If the log directory doesn't exist, create one.
        /// </summary>
        private void CreateLogFile()
        {
            if(Directory.Exists(FILE_LOG_DIRECTORY) == false)
            {
                // The log directory doesn't currently exist, so create it.
                try
                {
                    Directory.CreateDirectory(FILE_LOG_DIRECTORY);
                }
                catch
                {
                    // Can't create the log directory, therefore we can't 
                    // have a valid file log writer.

                    // REFACTOR We need a better way to report this condition.
                    Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
                }
            }

            string fileTimeStamp = System.DateTime.Now.ToString(FILE_TIMESTAMP_FORMAT);
            string fileName = FILE_LOG_DIRECTORY + "/" + fileTimeStamp + FILE_LOG_EXTENSION;

            try
            {
                writer = File.CreateText(fileName);
                writer.AutoFlush = true;
            }
            catch
            {
                // Can't create this particular log file, therefore we can't
                // have a valid file log writer.

                // REFACTOR Eventually, we need to add some appropriate error
                // output here. Why did it fail? Not enough free disk space?
                // Permissions error? Etc, etc.
                Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
            }
        }


        /// <summary>
        /// Reset the current stream position to the beginning of
        /// the file.
        /// </summary>
        private void WrapLogFile()
        {
            if(this.PlaceStreamAtBeginning() == true)
            {
                this.numTimesLogWrapped++;
                this.WriteHeader();
            }

            this.numLinesWritten = 0;
        }


        private bool PlaceStreamAtBeginning()
        {
            if(writer.BaseStream.CanSeek == true)
            {
                try
                {
                    lock(writer)
                    {
                        writer.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                    }
                    return true;
                }
                catch
                {
                    Console.Write("Log Error: Can not place the log stream at the beginning of the file.");
                    return false;
                }
            }

            return false;
        }


        private bool PlaceStreamAtEnd()
        {
            if(writer.BaseStream.CanSeek == true)
            {
                try
                {
                    lock(writer)
                    {
                        writer.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                    }
                    return true;
                }
                catch
                {
                    Console.Write("Log Error: Can not place the log stream at the end of the file.");
                    return false;
                }
            }

            return false;
        }
    }
}

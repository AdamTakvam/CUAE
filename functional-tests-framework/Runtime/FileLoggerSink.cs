using System;
using System.Diagnostics;
using System.IO;

using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
    /// <summary>
    /// Implementation of a logger sink that writes log messages
    /// to a file.
    /// </summary>
    public sealed class FileLoggerSink : LoggerSinkBase
    {
        private const string LOG_TIMESTAMP_FORMAT   = "HH:mm:ss.fff";
        private const string FILE_TIMESTAMP_FORMAT  = "yyyyMMdd-HHmmss";
        private const string FILE_LOG_DIRECTORY     = "Logs";
        private const string FILE_LOG_EXTENSION     = ".log";

        private object writerLock = new object();
        private StreamWriter writer;

        private object numLinesWrittenLock = new object();
        private int numLinesWritten = 0;

        private uint maximumLinesToWrite = 10000;

        public FileLoggerSink()
            : base(TraceLevel.Verbose)
        {
            CreateLogFile();
            RefreshConfiguration();
        }


        public override void RefreshConfiguration()
        {
            this.SubscribeToLoggerEvents(TraceLevel.Verbose);
        }


        public override void Dispose()
        {
            lock(writerLock)
            {
                if(writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }


        /// <summary>
        /// Invoked by Logger to write a log message to this sink.
        /// </summary>
        /// <param name="timeStamp">The time that the event that this entry represents occurred.</param>
        /// <param name="errorLevel">The error level of the message.</param>
        /// <param name="message">The message to write.</param>
        public override void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            Debug.Assert(writer != null, "writer is null");
			
            string logTimeStamp = timeStamp.ToString(LOG_TIMESTAMP_FORMAT);

            try
            {
                lock(writerLock)
                {
                    writer.WriteLine("{0} {1} {2}", logTimeStamp, errorLevel.ToString()[0], message);
                }
                
                lock(numLinesWrittenLock)
                {
                    this.numLinesWritten++;

                    if(this.numLinesWritten >= this.maximumLinesToWrite)
                    {
                        lock(writerLock)
                        {
                            writer.Flush();
                            writer.Close();
                            writer = null;

                            CreateLogFile();
                        }
                    }
                }
            }
            catch
            {
                // REFACTOR: We need a better way to report this condition.
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
                Console.WriteLine("Log Error: Could not write to log file. Is the disk full?");
            }
        }
    }
}

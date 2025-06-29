using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MigrationCore
{

    public abstract class LogLevels
    {
        public const int NONE       = 0;
        public const int INFO       = 1;
        public const int VERBOSE    = 2;
    }

    /// <summary>
    /// Ghetto logger for outputting to both a text log and the console
    /// </summary>
    public class LogWriter : IDisposable
    {

        public int LogLevel;
        private StreamWriter logfile; 

        public LogWriter(string logname)
        {
            string filename = String.Format("dbtool-{0}.{1}", logname, Configuration.FILE_LOG_EXTENSION);
            this.logfile = new StreamWriter(filename, false);
            this.LogLevel = LogLevels.INFO;
        }

        public void Write(string formatted, params object[] args)
        {
            this.Write(LogLevels.INFO, formatted, args);
        }

        public void Write(int level, string formatted, params object[] args)
        {
            string message = String.Format(formatted, args);
            this.logfile.Write(message);
            if (level <= this.LogLevel)
                Console.Write(message);
        }

        public void WriteLine()
        {
            this.WriteLine("");
        }

        public void WriteLine(string formatted, params object[] args)
        {
            this.WriteLine(LogLevels.INFO, formatted, args);
        }

        public void WriteLine(int level, string formatted, params object[] args)
        {
            string message = String.Format(formatted, args);
            this.logfile.WriteLine(message);
            if (level <= this.LogLevel)
                Console.WriteLine(message);
        }

        public void Dispose()
        {
            this.logfile.Close();
        }
    }

}

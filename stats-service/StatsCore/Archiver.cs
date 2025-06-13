using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Stats
{
    public delegate StatTable GetStatsDelegate();

    public sealed class Archiver : Loggable, IDisposable
    {
        #region Constants

        public abstract class Consts
        {
            public const string Name = "StatsArchiver";
            public const string DefaultRRDOutputString = "<No Output>";

            public readonly static TimeSpan PollInterval        = new TimeSpan(0, 0, 30);
            public readonly static TimeSpan RRDToolExecTimeout  = new TimeSpan(0, 0, 1);
            public readonly static TimeSpan ThreadWaitTimeout   = new TimeSpan(0, 0, 2);
            public readonly static DateTime Epoch               = new DateTime(1970, 1, 1);

            public const string RRDToolPath     = "rrdtool.exe";
            public const string RRDPath         = "RRD";
            public const string ImageFormat     = "PNG";
            public const string HLName          = "Licenses";

            public readonly static string DbName    = Path.Combine("RRD", "cuae.rrd");
            public readonly static string ImagePath = Path.Combine("RRD", "images");

            public const int ImageHeight        = 200;
            public const int ImageWidth         = 600;
            public const int ImageSegments      = 30;  // Number of squares along the X-axis

            public const int InitThreadCount    = 1;
            public const int MaxThreadCount     = 1;   // Only want one at a time

            public const string GraphMaxColorArea   = "66CC66"; // green
            public const string GraphMaxColorLine   = "000000"; // black
            public const string GraphHLColor        = "ff0000"; // red

            /// <summary>List of stats we care about (by OID)</summary>
            public readonly static string[][] Stats = 
            {               /* OID     Name           MinValue  MaxValue */
                new string[] { "2010", "AppSessions", "0",      "100000" }, 
                new string[] { "2011", "Calls",       "0",      "100000" }, 
                new string[] { "2012", "Ext1",        "0",      "100000" },
                new string[] { "2013", "Ext2",        "0",      "100000" },
                new string[] { "2014", "Ext3",        "0",      "100000" },
                new string[] { "2015", "Ext4",        "0",      "100000" },
                new string[] { "2016", "Ext5",        "0",      "100000" },
                new string[] { "2100", "Voice",       "0",      "1000" }, 
                new string[] { "2101", "RTP",         "0",      "1000" }, 
                new string[] { "2102", "ERTP",        "0",      "1000" }, 
                new string[] { "2103", "ConfRes",     "0",      "1000" }, 
                new string[] { "2104", "Speech",      "0",      "1000" }, 
                new string[] { "2105", "TTS",         "0",      "1000" },
                new string[] { "2106", "ConfSlots",   "0",      "1000" }, 
                new string[] { "2107", "Conf",        "0",      "1000" }, 
                new string[] { "2108", "MSExt1",      "0",      "1000" }, 
                new string[] { "2109", "MSExt2",      "0",      "1000" }, 
                new string[] { "2110", "MSExt3",      "0",      "1000" },
                new string[] { "2111", "MSExt4",      "0",      "1000" },
                new string[] { "2112", "MSExt5",      "0",      "1000" }
            };

            public abstract class CommandVerbs
            {
                public const string Quit    = "quit";
                public const string Update  = "update";
                public const string Create  = "create";
                public const string Graph   = "graph";
            }

            public abstract class LowResDb
            {
                // See the RRD Tool docs for definitions of these values:
                // http://oss.oetiker.ch/rrdtool/doc/rrdcreate.en.html
                public const decimal XFF            = 0.5M;
                public const int Steps              = 120;

                // Number of data points to store
                // 60 / update interval seconds = updates per minute
                // updates per minute / steps = rows per minute
                // rows per minute * 525,600 = rows per year
                // rows per year * number of years = total rows required
                private const int ArchiveYears      = 3;
                public readonly static double UPM   = 60 / (double) PollInterval.TotalSeconds;
                public readonly static int Rows     = Convert.ToInt32((UPM / Steps) * 525600 * ArchiveYears);
            }

            public abstract class HighResDb
            {
                public const decimal XFF            = 0.5M;
                public const int Steps              = 2;

                public const int ArchiveDays        = 7;
                public readonly static double UPM   = 60 / (double) PollInterval.TotalSeconds;
                public readonly static int Rows     = Convert.ToInt32((UPM / Steps) * 1440 * ArchiveDays);
            }

            public abstract class AvgDb
            {
                public const decimal XFF            = 0.5M;
                public const int Steps              = 20;

                public const int ArchiveDays        = 7;
                public readonly static double UPM   = 60 / (double) PollInterval.TotalSeconds;
                public readonly static int Rows     = Convert.ToInt32((UPM / Steps) * 1440 * ArchiveDays);
            }
        }
        #endregion

        public GetStatsDelegate GetStats;

        private readonly ImageNameRepository imageNames;
        private readonly TimerManager timers;
        private readonly object archiveLock;
        private readonly object commandExecuteLock;
        private Process rrdProcess = null;
        private string lastOutput = null;

        #region Construct/Dispose

        public Archiver()
            : base(TraceLevel.Verbose, Consts.Name)
        {
            this.archiveLock = new object();
            this.commandExecuteLock = new object();
            this.imageNames = new ImageNameRepository(Consts.Stats.Length * 5);

            this.timers = new TimerManager(Consts.Name, new WakeupDelegate(ArchiveStats), 
                null, Consts.InitThreadCount, Consts.MaxThreadCount);
            timers.threadPool.MessageLogged += new LogDelegate(log.Write);

            // Set current working dir to AppDomain base dir 
            //  so service runtime doesn't get confused
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public override void Dispose()
        {
            // need to connect this to stop
            this.timers.Shutdown();
            base.Dispose();
        }
        #endregion

        #region Start/Stop

        public bool Start()
        {
            // Create data directories, if not exist
            if(!Directory.Exists(Consts.RRDPath))
            {
                try { Directory.CreateDirectory(Consts.RRDPath); }
                catch(Exception e) 
                {
                    log.Write(TraceLevel.Error, "Failed to create output directory: " + e.Message);
                    return false;
                }
            }

            if(!Directory.Exists(Consts.ImagePath))
            {
                try { Directory.CreateDirectory(Consts.ImagePath); }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Failed to create output directory: " + e.Message);
                    return false;
                }
            }

            rrdProcess = CreateRRDToolProcess("-");

            if(!CreateDatabase())
                return false;

            timers.Add(Convert.ToInt64(Consts.PollInterval.TotalMilliseconds));
            return true;
        }

        public void Stop()
        {
            timers.RemoveAll();
            TerminateRRDToolProcess();
        }

        private bool CreateDatabase()
        {
            if(File.Exists(Consts.DbName))
                return true;

            StringBuilder args = new StringBuilder(String.Format("{0} {1} ", Consts.CommandVerbs.Create, Consts.DbName));

            for(int i=0; i<Consts.Stats.Length; i++)
            {
                string statId = Consts.Stats[i][0];
                string minValue = Consts.Stats[i][2];
                string maxValue = Consts.Stats[i][3];

                args.AppendFormat("DS:{0}:GAUGE:{1}:{2}:{3} ", statId,
                    Consts.PollInterval.TotalSeconds * 2, minValue, maxValue);
            }

            args.AppendFormat("RRA:MAX:{0}:{1}:{2} ", Consts.HighResDb.XFF, Consts.HighResDb.Steps, Consts.HighResDb.Rows);
            args.AppendFormat("RRA:MAX:{0}:{1}:{2} ", Consts.LowResDb.XFF, Consts.LowResDb.Steps, Consts.LowResDb.Rows);
            args.AppendFormat("RRA:AVERAGE:{0}:{1}:{2} ", Consts.AvgDb.XFF, Consts.AvgDb.Steps, Consts.AvgDb.Rows);
            args.AppendFormat("--step {0}", Consts.PollInterval.TotalSeconds);

            if (!ExecuteRRDToolCommand(args.ToString()))
            {
                log.Write(TraceLevel.Error, "Failed to create RRDTool database.");
                log.Write(TraceLevel.Error, "RRDTool output: " + GetLastRRDOutput());
                log.Write(TraceLevel.Error, "RRDTool errors: " + GetRRDErrorOutput());
                return false;
            }

            return true;
        }
        
        #endregion

        #region Stats archiving thread

        private long ArchiveStats(TimerHandle handle, object state)
        {
            Assertion.Check(GetStats != null, "GetStats delegate is not hooked");

            lock(archiveLock)
            {
                StatTable stats = GetStats();

                if(!ExecuteRRDToolCommand(CreateUpdateArgs(Consts.DbName, stats)))
                {
                    log.Write(TraceLevel.Error, "Failed to archive high-res stats.");
                    log.Write(TraceLevel.Error, "RRDTool output: " + GetLastRRDOutput());
                    log.Write(TraceLevel.Error, "RRDTool errors: " + GetRRDErrorOutput());
                }
            }

            return Convert.ToInt64(Consts.PollInterval.TotalMilliseconds);    // Keep recurring
        }

        private string CreateUpdateArgs(string dbName, StatTable stats)
        {
            StringBuilder sb = new StringBuilder(String.Format("update {0} N", dbName));

            for(int i=0; i<Consts.Stats.Length; i++)
            {
                string statId = Consts.Stats[i][0];
                long val = stats.GetMaxValue(Convert.ToInt32(statId), true);

                sb.AppendFormat(":{0}", val.ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region Report generation

        public void GenerateGraph(string oid, IStats.MgmtListener.Commands.Interval interval, long lineVal, out string imgPath)
        {
            string imgFilename = String.Format("{0}_{1}.png", oid.ToString(), interval.ToString());
            string relImgPath = Path.Combine(Consts.ImagePath, imgFilename);
            imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relImgPath);
            log.Write(TraceLevel.Verbose, "Image file: " + imgPath);

            FileInfo imgFile = new FileInfo(imgPath);
            if(imgFile.Exists)
            {
                if (interval == IStats.MgmtListener.Commands.Interval.Month ||
                    interval == IStats.MgmtListener.Commands.Interval.Year)
                {
                    // Only bother updating once per hour from low-res DB
                    DateTime hourAgo = DateTime.UtcNow - new TimeSpan(1, 0, 0);
                    if(imgFile.CreationTimeUtc > hourAgo)
                        return;
                }
                else
                { 
                    // Only update once per minute from high-res DB
                    DateTime minuteAgo = DateTime.UtcNow - new TimeSpan(0, 1, 0);
                    if(imgFile.CreationTimeUtc > minuteAgo)
                        return;
                }
            }

            DateTime startTime;
            DateTime endTime = DateTime.UtcNow;

            switch(interval)
            {
                case IStats.MgmtListener.Commands.Interval.Hour:
                    startTime = endTime - new TimeSpan(1, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.SixHour:
                    startTime = endTime - new TimeSpan(6, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.TwelveHour:
                    startTime = endTime - new TimeSpan(12, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.Day:
                    startTime = endTime - new TimeSpan(1, 0, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.Week:
                    startTime = endTime - new TimeSpan(7, 0, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.Month:
                    startTime = endTime - new TimeSpan(30, 0, 0, 0);
                    break;
                case IStats.MgmtListener.Commands.Interval.Year:
                    startTime = endTime - new TimeSpan(365, 0, 0, 0);
                    break;
                default:
                    startTime = endTime - new TimeSpan(1, 0, 0, 0); // 1 day
                    break;
            }

            GenerateGraph(oid, relImgPath, startTime, endTime, lineVal);
        }

        public void GenerateGraph(string oid, DateTime startTime, DateTime endTime, long lineVal, out string imgPath)
        {
            string imgFile = Path.Combine(Consts.ImagePath, imageNames.GetName());
            imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imgFile);
            log.Write(TraceLevel.Verbose, "Image file: " + imgPath);

            GenerateGraph(oid, imgFile, startTime, endTime, lineVal);
        }

        private void GenerateGraph(string oid, string imgFile, DateTime startTime, DateTime endTime, long lineVal)
        {
            DateTime now = DateTime.UtcNow;
            if(endTime > now)
                endTime = now;

            // Convert start and end times to unix time format
            long startSecs = SecondsSinceEpoch(startTime);
            long endSecs = SecondsSinceEpoch(endTime);

            if(startSecs > endSecs)
            {
                // If they are backwards, switch 'em
                long _endSecs = endSecs;
                endSecs = startSecs;
                startSecs = _endSecs;
            }

            StringBuilder args = new StringBuilder("graph ");
            args.Append(imgFile);

            int i = GetStatIndex(oid);
            if(i == -1)
                throw new ApplicationException("Invalid OID: " + oid);

            string id = Consts.Stats[i][0];
            string name = Consts.Stats[i][1];
            string maxName = name + "_max";
            string avgName = name + "_avg";

            // Define a data source to graph
            args.AppendFormat(" DEF:{0}={1}:{2}:MAX",   maxName, Consts.DbName, id);

            // We want an area graph, in other words the area under the line will be filled in
            args.AppendFormat(" AREA:{0}#{1}:\"{2}\"",  maxName, Consts.GraphMaxColorArea, maxName);

            // Define an outline line that will run across the top of our area graph
            args.AppendFormat(" LINE0.5:{0}#{1}",       maxName, Consts.GraphMaxColorLine, maxName);

            if(lineVal > 0)
                args.AppendFormat(" HRULE:{0}#{1}:{2}", lineVal, Consts.GraphHLColor, Consts.HLName);

            // Add the following for a pretty legend: --title \"Concurrent Calls\" --vertical-label \"# of Calls\""
            args.AppendFormat(" --start {0} --end {1} --imgformat {2} --width {3} --height {4}",
                startSecs, endSecs, Consts.ImageFormat, Consts.ImageWidth, Consts.ImageHeight);

            if(!ExecuteRRDToolGraphCommand(args.ToString()))
            {
                throw new ApplicationException("Failed to generate graph: " + GetLastRRDOutput());
            }
        }

        private int GetStatIndex(string oid)
        {
            for(int i=0; i<Consts.Stats.Length; i++)
            {
                string currOid = Consts.Stats[i][0];
                if(oid == currOid)
                    return i;
            }
            return -1;
        }
        #endregion

        #region Helpers

        private Process CreateRRDToolProcess(string args)
        {
            log.Write(TraceLevel.Verbose, "Executing: rrdtool.exe {0}", args);

            Process currentRRDToolProcess = new Process();
            currentRRDToolProcess.StartInfo.FileName = Consts.RRDToolPath;
            currentRRDToolProcess.StartInfo.Arguments = args;
            currentRRDToolProcess.StartInfo.ErrorDialog = false;

            if(ShouldUseUTCTime())
                currentRRDToolProcess.StartInfo.EnvironmentVariables["TZ"] = "UTC";

            currentRRDToolProcess.StartInfo.CreateNoWindow = true;
            currentRRDToolProcess.StartInfo.UseShellExecute = false;
            currentRRDToolProcess.StartInfo.RedirectStandardError = true;
            currentRRDToolProcess.StartInfo.RedirectStandardOutput = true;
            currentRRDToolProcess.StartInfo.RedirectStandardInput = true;
            currentRRDToolProcess.Start();

            return currentRRDToolProcess;
        }

        private void WriteToRRDInputStream(string rrdCommand)
        {
            log.Write(TraceLevel.Verbose, "Executing rrdtool command: " + rrdCommand);

            StreamWriter rrdInputWriter = rrdProcess.StandardInput;
            try
            {
                rrdInputWriter.WriteLine(rrdCommand);
                rrdInputWriter.Flush();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "RRDTool error on input: " + Exceptions.FormatException(e));
            }
        }

        private bool ExecuteRRDToolCommand(string rrdCommand)
        {
            lock(commandExecuteLock)
            {
                //rrdProcess.StandardOutput.ReadLine();
                WriteToRRDInputStream(rrdCommand);
                if(GetRRDOutput().Equals("OK"))
                    return true;
                else
                    return false;
            }
        }

        private bool ExecuteRRDToolGraphCommand(string rrdCommand)
        {
            Regex sizePattern = new Regex("[0-9]+x[0-9]+");

            lock (commandExecuteLock)
            {
                WriteToRRDInputStream(rrdCommand);
                // The successful output of a graphing command is the display of the image 
                // dimensions followed by an "OK" message
                if (sizePattern.IsMatch(GetRRDOutput()))           
                    if (GetRRDOutput().Equals("OK"))
                        return true;

                return false;
            }
        }

        private string GetRRDOutput()
        {
            string outputString = null;
            if(rrdProcess != null)
               outputString = rrdProcess.StandardOutput.ReadLine();

            if(outputString == null || outputString == String.Empty)
               outputString = Consts.DefaultRRDOutputString;
            
            lastOutput = outputString;
            return outputString;
        }

        private string GetLastRRDOutput()
        {
            return lastOutput;
        }

        private string GetRRDErrorOutput()
        {
            string outputString = null;
            if(rrdProcess != null)
                outputString = rrdProcess.StandardError.ReadLine();

            if(outputString == null || outputString == String.Empty)
                outputString = Consts.DefaultRRDOutputString;

            return outputString;
        }

        private bool TerminateRRDToolProcess()
        {
            if(rrdProcess == null || rrdProcess.HasExited)
                return false;

            WriteToRRDInputStream(Consts.CommandVerbs.Quit);

            // need to read output prior to WaitForExit or risk deadlock
            string stdErrorOutput = GetRRDErrorOutput();
            string stdOutput = GetRRDOutput();

            if(!rrdProcess.WaitForExit(Convert.ToInt32(Consts.RRDToolExecTimeout.TotalMilliseconds)))
            {
                log.Write(TraceLevel.Error, "RRDTool execution timed out. Most recent statistics may not have been saved.");
                log.Write(TraceLevel.Error, "RRDTool output: " + stdOutput);
                log.Write(TraceLevel.Error, "RRDTool errors: " + stdErrorOutput);
                rrdProcess.Kill();
                return false;
            }
            else
            {
                log.Write(TraceLevel.Verbose, "RRDTool exited successfully.");
                log.Write(TraceLevel.Verbose, "RRDTool output: " + stdOutput);
                log.Write(TraceLevel.Verbose, "RRDTool errors: " + stdErrorOutput);
            }
            
            return true;
        }

        private long SecondsSinceEpoch(DateTime time)
        {
            TimeSpan t = time - Consts.Epoch;
            return Convert.ToInt64(t.TotalSeconds);
        }

        private bool ShouldUseUTCTime()
        {
            string utcTimeSetStr = Metreos.Configuration.AppConfig.GetEntry("UseUTCTimeForGraphs");
            bool utcTimeSet = false;
            try { utcTimeSet = System.Convert.ToBoolean(utcTimeSetStr); }
            catch { }
            return utcTimeSet;
        }

        #endregion
    }
}

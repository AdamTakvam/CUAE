using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

using Metreos.LogSinks;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.LoggingFramework;

namespace LocalizedDbTest
{
    class Program
    {
        private const string Filename       = "test.html";
        
        private const string PlEntryName    = "TestPl";
        private const string PlEntryValue   = "Cześć";
        
        private const string HebEntryName   = "TestHeb";
        private const string HebEntryValue  = "יחוח";

        static void Main(string[] args)
        {
            using(ConsoleLoggerSink cls = new ConsoleLoggerSink(TraceLevel.Verbose))
            {
                string plValue = PutThroughDb(PlEntryName, PlEntryValue);
                string hebValue = PutThroughDb(HebEntryName, HebEntryValue);

                StringBuilder sb = new StringBuilder("<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"><title>Google</title></head><body>");
                sb.AppendLine();
                sb.AppendLine(PlEntryValue + " = " + plValue);
                sb.Append("<br><br>");
                sb.AppendLine(HebEntryValue + " = " + hebValue);
                sb.Append("</body></html>");

                Trace.WriteLine(sb.ToString(), TraceLevel.Info.ToString());

                FileInfo fInfo = new FileInfo(Filename);
                if(fInfo.Exists)
                    fInfo.Delete();

                FileStream fStream = fInfo.Create();
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                fStream.Write(buffer, 0, buffer.Length);
                fStream.Flush();
                fStream.Close();

                Console.WriteLine();
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }

        private static string PutThroughDb(string name, string Value)
        {
            ConfigEntry ce = new ConfigEntry(name, Value, null, IConfig.StandardFormat.String, false);
            Config.Instance.AddEntry(IConfig.ComponentType.Core, IConfig.CoreComponentNames.APP_MANAGER, ce);

            return Config.Instance.GetEntryValue(IConfig.ComponentType.Core,
                IConfig.CoreComponentNames.APP_MANAGER, name) as string;
        }
    }
}

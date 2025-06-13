using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.LogSinks;
using Metreos.LoggingFramework;

namespace Metreos.AppServer.ProviderManager
{
    class Program : Loggable
    {
        static void Main(string[] args)
        {
            using(ConsoleLoggerSink cls = new ConsoleLoggerSink(TraceLevel.Verbose))
            {
                CommandLineArguments clargs = new CommandLineArguments(args);
                StringCollection saps = clargs.GetStandAloneParameters();
                if(saps == null || saps.Count == 0)
                {
                    Console.WriteLine("No service name specified");
                    return;
                }

                Program p = new Program(clargs.GetSingleParam("f"), saps[0]);

                if(clargs.IsParamPresent("u"))
                    p.Uninstall();
                else if(p.Install())
                    p.Start(clargs["a"]);
            }
            Console.ResetColor();
            System.Threading.Thread.Sleep(500);
        }

        private string serviceFilename;
        private string serviceName;
        private ServiceManager sm;

        public Program(string serviceFilename, string serviceName)
            : base(TraceLevel.Verbose, "ServiceManager")
        {
            this.serviceFilename = serviceFilename;
            this.serviceName = serviceName;

            this.sm = new ServiceManager(log);
        }

        public bool Install()
        {
            string failReason;
            if(!sm.InstallService(new FileInfo(serviceFilename), serviceName, serviceName, null, null, out failReason))
            {
                log.Write(TraceLevel.Error, "Install failed: " + failReason);
                return false;
            }
            return true;
        }

        public void Start(string[] args)
        {
            if(args != null)
            {
                string argsStr = "";
                foreach(string a in args)
                {
                    argsStr += a + " ";
                }
                log.Write(TraceLevel.Info, "Startup args: " + argsStr);
            }

            try { sm.StartService(serviceName, args); }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, Exceptions.FormatException(e));
            }
        }

        public void Uninstall()
        {
            string failReason;
            if(!sm.UninstallService(serviceName, out failReason))
                log.Write(TraceLevel.Error, "Uninstall failed: " + failReason);
        }
    }
}

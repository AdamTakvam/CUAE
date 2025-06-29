using System;
using System.Collections;
using System.Collections.Specialized;
using Metreos.Utilities;
using Metreos.Samoa.FunctionalTestFramework;
namespace MaxTestGen
{
	class MaxTestGen
	{
        public static string TestName = "n";
        public static string TestGroup = "g";
        public static string ScriptName ="s";
        public static string SignalName = "sig";
        public static string EventName = "e";
        public static string Triggers = "trigger";
        public static string OutputDirectory = "d";
        public static string LaunchDebugger = "l";

        public static string DefaultOutputDir = @"x:\samoa-functional-tests\TestBank";
		[STAThread]
		static void Main(string[] args)
		{
            CommandLineArguments parser = new CommandLineArguments(args);
        
            if(parser.IsParamPresent(LaunchDebugger))
            {
                System.Diagnostics.Debugger.Launch();
            }

            bool error = false;
            if(!parser.IsParamPresent(TestName))
            {
                error = true;
                Console.WriteLine("No testname defined");
            }
            if(!parser.IsParamPresent(ScriptName))
            {
                error = true;
                Console.WriteLine("One script must be defined");
            }
            if(!parser.IsParamPresent(TestGroup))
            {
                error = true;
                Console.Write("No test group defined");
            }

            if(!error)
            {
                string testName = parser.GetSingleParam(TestName);
                string testGroup = parser.GetSingleParam(TestGroup);
                string outputDirectory = parser.GetSingleParam(OutputDirectory);
                if(outputDirectory == null || outputDirectory == String.Empty)
                {
                    outputDirectory = DefaultOutputDir;
                }

                if(!System.IO.Directory.Exists(outputDirectory))
                {
                    Console.WriteLine("Output directory doesn't exist: " + outputDirectory);
                    Console.WriteLine(Usage);
                    return;
                }

                string[] scripts = parser[ScriptName];
                string[] signals = parser[SignalName];
                string[] events = parser[EventName];
                string[] triggers = parser[Triggers];

                TestInfo info = new TestInfo(testName, testGroup, outputDirectory);

                foreach(string script in scripts)
                {
                    string fullTriggerEvent = DesignerTestMaker.FunctionalTestTrigger;
                    if(triggers != null)
                    {
                        foreach(string trigger in triggers)
                        {
                            if(trigger.StartsWith(script))
                            {
                                int index = trigger.IndexOf(".", 0);
                                if(index > 0 && index < trigger.Length - 2)
                                {
                                    string pureTriggerEventName = trigger.Substring(index + 1);
                                    if(pureTriggerEventName != null && pureTriggerEventName != String.Empty)
                                    {
                                        fullTriggerEvent = pureTriggerEventName;
                                    }
                                }
                            }
                        }
                    }

                    ScriptInfo scriptInfo = info.AddScript(script, fullTriggerEvent);

                    if(signals != null)
                    {
                        foreach(string signal in signals)
                        {
                            if(signal.StartsWith(script))
                            {
                                int index = signal.IndexOf(".", 0);
                                if(index > 0 && index < signal.Length - 2)
                                {
                                    string pureSignalName = signal.Substring(index + 1);
                                    if(pureSignalName != null && pureSignalName != String.Empty)
                                    {
                                       scriptInfo.AddSignal(pureSignalName); 
                                    }
                                }
                            }
                        }
                    }

                    if(events != null)
                    {
                        foreach(string @event in events)
                        {
                            if(@event.StartsWith(script))
                            {
                                int index = @event.IndexOf(".", 0);
                                if(index > 0 && index < @event.Length - 2)
                                {
                                    string pureEventName = @event.Substring(index + 1);
                                    if(pureEventName != null && pureEventName != String.Empty)
                                    {
                                        scriptInfo.AddEvent(pureEventName); 
                                    }
                                }
                            }
                        }
                    }
                }

                DesignerTestMaker maker = new DesignerTestMaker();
                maker.Info += new FTLogDelegate(WriteInfo);
                maker.Error += new FTLogDelegate(WriteError);
                maker.Save(info);
            }
            else
            {
                Console.WriteLine(Usage);
            }
        }

        public const string Usage = @"

MAX Test Generation Tool
------------------------
-n:<testname>   required  name of MAX project file
-g:<testgroup>  required  name of test group
                         ex: ARE
                         ex: IVT
                         ex: Provider
                         refer to the testbank for all valid groups
-s:<scriptname> required at least one
-trigger:<scripttrigger> optional for each script.  If not specified for a script,
                         the trigger event will be assumed 
                         'Metreos.Providers.FunctionalTest.TriggerApp'
                         
                         Must be of form <scriptname>.<eventname>
                         ex: script1.Metreos.Providers.Http.GotRequest
                         ex: script1.Metreos.CallControl.IncomingCall
-sig:<signalname> must be of form <scriptname>.<signalname>
                         ex: script1.MadeCall
-e:<eventname> must be of form <scriptname>.<eventname>
                         ex: script1.MakeCall
-d:<outputdirectory>   optional, defaults to x:\samoa-functional-tests\TestBank

Note: the 'Templates' folder must be found in the directory immediately below this executable
";

        private static void WriteInfo(string message)
        {
            Console.WriteLine("Info: " + message);
        }

        private static void WriteError(string message)
        {
            Console.WriteLine("Error: " + message);
        }
    }

  
}

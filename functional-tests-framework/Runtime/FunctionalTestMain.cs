using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Activation;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using FTF=Metreos.Samoa.FunctionalTestFramework;

using Metreos.Core.ConfigData;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestRuntime
{
    /// <summary> The underlying framework for tests:  responsible for starting/stopping  </summary>
    public class FunctionalTestMain : IFunctionalTestConduit, IDisposable
    { 
        #region Conduit Events
        
        public event FTF.CommonTypes.AddTest AddNewTest; 
        public event FTF.CommonTypes.AdvanceProgressMeter AdvanceProgress;
        public event FTF.CommonTypes.LoadDone FrameworkLoaded;
        public event FTF.CommonTypes.InstructionLine InstructionLine;
        public event FTF.CommonTypes.OutputLine Output;
        public event FTF.CommonTypes.ResetProgressMeter ResetProgress;
        public event FTF.CommonTypes.StatusUpdate StatusUpdate;
        public event FTF.CommonTypes.TestEndedDelegate TestEnded;
        public event FTF.CommonTypes.TestAbortable TestNowAbortable;
        public event FTF.CommonTypes.ServerConnectionStatus UpdateConnectedStatus;

        #endregion

        #region Conduit External Commands

        public string TestSettingsFolder{ get{ return testSettingsFolder.FullName; }}
        public FTF.Settings Settings{ get{ return settings; } set{ settings = value; }}
        public FTF.TestSettings TestSettings{ get{ return testSettings; } set{ testSettings = value; }}
        public FTF.CommonTypes.StartTestDelegate StartTest{ get{ return startTest; } set{ startTest = value; }}
        public FTF.CommonTypes.ConnectServer ConnectServer{ get{ return connectServer; } set{ connectServer = value; }}
        public FTF.CommonTypes.AddUser AddUser{ get{ return addUser; } set{ addUser = value; }}
        public FTF.CommonTypes.AsyncUserInputCallback AsyncUserInput{ get{ return asyncUserInput; } set{ asyncUserInput = value; }}
        public FTF.CommonTypes.ReconnectToServer ReconnectToServer{ get{ return reconnect; } set{ reconnect = value; }}
        public FTF.CommonTypes.PrepareServerBeforeTest PrepareServerBeforeTest { get { return prepareServerBeforeTest; } set { prepareServerBeforeTest = value; } }
        public FTF.CommonTypes.InitializeGlobalSettings InitializeGlobalSettings{ get{ return initializeGlobalSettings; } set{ initializeGlobalSettings = value; }}
        public FTF.CommonTypes.StopTestDelegate AbortTest { get { return stopTest; } set { stopTest = value; } }
        
        private FTF.CommonTypes.StartTestDelegate startTest;
        private FTF.CommonTypes.ConnectServer connectServer;
        private FTF.CommonTypes.AddUser addUser;
        private FTF.CommonTypes.AsyncUserInputCallback asyncUserInput;    
        private FTF.CommonTypes.ReconnectToServer reconnect;
        private FTF.CommonTypes.InitializeGlobalSettings initializeGlobalSettings;
        private FTF.CommonTypes.StopTestDelegate stopTest;

        #endregion
 
        public bool Windowed { set { windowed = value; } }

        private const int pollRate              = 1000; 
        private static int numCycles            = 1;
        private static string defaultOutputfilename  = "results.xml";
        private static string outputResultsFilename  = Path.Combine(Environment.CurrentDirectory, defaultOutputfilename);

        private int errorCount                  = 0;
        private LogWriter log;
        private bool stop;
        private bool windowed;
        private ArrayList errors;
        private FTF.Settings settings;
        private AutoResetEvent testEnd;
        private ArrayList functionalTests;
        private FTF.TestSettings testSettings;
        private DirectoryInfo testSettingsFolder;
        private FTF.CommonTypes.UpdateConfigValue updateAppConfig;
        private FTF.CommonTypes.UpdateScriptParameter updateScriptParameter;
        private FTF.CommonTypes.UpdateCallRouteGroup updateCallRouteGroup;
        private FTF.CommonTypes.UpdateMediaRouteGroup updateMediaRouteGroup;
        private FTF.CommonTypes.PrepareServerBeforeTest prepareServerBeforeTest;
        private bool dontUninstall;
        private FileLoggerSink fileLogger;
        private StringBuilder logCatcher;
        private LoggerWriteDelegate verboseLog;
        private LoggerWriteDelegate infoLog;
        private LoggerWriteDelegate warningLog;
        private LoggerWriteDelegate errorLog;
        private FunctionalTestInfo currentTest;

        public FunctionalTestMain(CommandLineArguments parser)
        {
            windowed = true;

            if(parser.IsParamPresent("count"))
            {
                SetCycleCount(parser.GetSingleParam("count"));
            }
            
            if(parser.IsParamPresent("results"))
            {
                SetResultFilename(parser.GetSingleParam("results"));
            }

            this.stop                       = true;
            this.dontUninstall              = false;
            this.currentTest                = null;
            this.verboseLog                 = new LoggerWriteDelegate(WriteToLogCatcher);
            this.infoLog                    = new LoggerWriteDelegate(WriteToLogCatcher);
            this.warningLog                 = new LoggerWriteDelegate(WriteToLogCatcher);
            this.errorLog                   = new LoggerWriteDelegate(WriteToLogCatcher);
            this.logCatcher                 = new StringBuilder(100); 
            this.functionalTests            = new ArrayList();
            this.errors                     = new ArrayList();
            this.testEnd                    = new AutoResetEvent(false);
            this.startTest                  = new FTF.CommonTypes.StartTestDelegate(ExecuteTest);
            this.connectServer              = new FTF.CommonTypes.ConnectServer(EstablishRemoteConnection);
            this.addUser                    = new FTF.CommonTypes.AddUser(AddUserCallback);
            this.reconnect                  = new FTF.CommonTypes.ReconnectToServer(ReconnectServer);
            this.prepareServerBeforeTest    = new FTF.CommonTypes.PrepareServerBeforeTest(PrepareServerBeforeTestHandler);
            this.initializeGlobalSettings   = new FTF.CommonTypes.InitializeGlobalSettings(InitializeSettings);
            this.stopTest                   = new FTF.CommonTypes.StopTestDelegate(AbortTestRequested);
            this.testSettingsFolder         = new DirectoryInfo(Environment.CurrentDirectory);
            this.updateAppConfig            = new FTF.CommonTypes.UpdateConfigValue(UpdateConfigValue);
            this.updateScriptParameter      = new FTF.CommonTypes.UpdateScriptParameter(UpdateScriptParameter);
            this.updateCallRouteGroup       = new FTF.CommonTypes.UpdateCallRouteGroup(UpdateCallRouteGroup);
            this.updateMediaRouteGroup      = new FTF.CommonTypes.UpdateMediaRouteGroup(UpdateMediaRouteGroup);
            this.log                        = new LogWriter(TraceLevel.Verbose, FTF.Constants.FTProvider);
            this.fileLogger                 = new FileLoggerSink();
            
            LoadSettings(parser);
        }
        
        public void Initialize()
        {            
            FTF.ManagementCommunicator.Instance.Initialize(log, settings);
            FTF.TestCommunicator.Instance.Initialize(log, settings);  

            LoadFunctionalTestsFromDirectory();

            InitializeIndividualTestSettings();

            InitializeTests();
        }

        #region Initialization

        private void InitializeIndividualTestSettings()
        {
            FileInfo testSettingFile;

            testSettingFile = new FileInfo(testSettingsFolder.FullName 
                + Path.DirectorySeparatorChar + settings.TestSettings);
  
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(FTF.TestSettings));
			
            if(testSettingFile.Exists)
            {
                FileStream fileStream = null;

                try
                {
                    fileStream = testSettingFile.OpenRead();
                }
                catch
                {
                    OutputLine("Could not open the Test Settings file.  Make sure it is not open");
                    return;
                }

                try
                {
                    TestSettings = (FTF.TestSettings) serializer.Deserialize(fileStream);
                }
                catch
                {
                    OutputLine("Could not deserialize the Test Settings file.  Recreating it.");
                    TestSettings = new FTF.TestSettings();
                }
                finally
                {
                    if(fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
            else
            {
                TestSettings = new FTF.TestSettings();	
            }

            if(testSettings.tests != null)
            {
                foreach(FTF.testType test in testSettings.tests)
                {
                    foreach(FunctionalTestInfo testInfo in functionalTests)
                    {
                        if(test.name == testInfo.FullName)
                        {
                            testInfo.previousSuccess = test.success;
                            testInfo.firstTime = false;

                            if( test.variables != null)
                            {
                                foreach(FTF.variableType variable in test.variables)
                                {
                                    testInfo.configValues[variable.name] = variable.Value;
                                }
                            }

                            break;
                        }
                    }
                }
            }			
        }

        public void InitializeTests()
        {
            for(int i = 0; i < functionalTests.Count; i++)
            {
                FunctionalTestInfo temp = (FunctionalTestInfo)functionalTests[i];

                AddNewTest(temp.numBaseSpacesInNameSpace, temp.test.Name, temp.test.GetRequiredUserInput(), 
                    temp.configValues, temp.firstTime, temp.previousSuccess, 
                    temp.test.Description, temp.test.Instructions);
            }

            FrameworkLoaded();
        }

        private void InitializeSettings()
        {
            settings = new FTF.Settings();
            settings.Password = FTF.Constants.DefaultPassword;
            settings.SamoaPort = FTF.Constants.DefaultSamoaPort;
            settings.TestPort = FTF.Constants.DefaultTestClientPort;
            settings.Username = FTF.Constants.DefaultUsername;
            settings.FrameworkDir = FTF.Constants.FrameworkDir;
            settings.DllFolder = FTF.Constants.CompiledTestsFolder;
            settings.TestSettings = FTF.Constants.DefaultTestSettingPath;
            settings.CompiledMaxTestsDir = FTF.Constants.CompiledMaxTestsFolder;
            settings.PollTimes = FTF.Constants.DefaultPollTimes;
            settings.PhoneStartRange = FTF.Constants.DefaultStartRange;
            settings.PhoneEndRange = FTF.Constants.DefaultEndRange;
            settings.CallManagerIp = FTF.Constants.CallManagerIp;
            settings.NeverUninstallExistingApps = FTF.Constants.DefaultNeverUninstall;
            SamoaSettings samoaSettings = new SamoaSettings(settings, this.testSettingsFolder.FullName);

            MessageBox.Show("This seems to be the first time you have run the Functional Tests.  Please establish some basic settings.");
	
            samoaSettings.ShowDialog();
			
            if(!samoaSettings.IsDisposed)
            {
                samoaSettings.Dispose();
            }		
        }

        private void LoadFunctionalTestsFromDirectory()
        {
            DebugLog.MethodEnter();

            this.StatusUpdate("Loading Functional Tests");

            string[] assemblies = null;

            try
            {
                assemblies = Directory.GetFiles(settings.DllFolder, "*.dll");
            }
            catch(System.Security.SecurityException e)
            {
                log.Write(TraceLevel.Error, "Could not retrieve functional test assemblies from directory. " +
                    "Exception caught is: " + e.ToString());
            }
            catch(ArgumentNullException e)
            {
                log.Write(TraceLevel.Error, "Could not retrieve functional test assemblies from directory. " +
                    "Exception caught is: " + e.ToString());
            }
            catch(PathTooLongException e)
            {
                log.Write(TraceLevel.Error, "Could not retrieve functional test assemblies from directory. " +
                    "Exception caught is: " + e.ToString());
            }
            catch(DirectoryNotFoundException e)
            {
                log.Write(TraceLevel.Error, "Could not retrieve functional test assemblies from directory. " +
                    "Exception caught is: " + e.ToString());
            }

            StringCollection corruptAssemblies = new StringCollection();

            if(assemblies != null)
            {   
                this.ResetProgress(assemblies.Length);

                foreach(string assembly in assemblies)
                {
                    try
                    {
                        LoadFunctionalTestFromAssembly(assembly);
                    }
                    catch
                    {
                        corruptAssemblies.Add(assembly);
                    }
                    this.AdvanceProgress(1);
                }
            }

            if(corruptAssemblies.Count > 0)
            {
                StringBuilder corruptMessage = new StringBuilder();
                corruptMessage.Append(corruptAssemblies.Count + FTF.Constants.SPACE + 
                    FTF.Constants.ASSEMBLY_LOAD_ERROR + FTF.Constants.NEW_LINE);
                    
                for(int i = 0; i < corruptAssemblies.Count; i++)
                {
                    corruptMessage.Append(corruptAssemblies[i] + ", " + FTF.Constants.NEW_LINE);
                }

                corruptMessage.Remove(corruptMessage.Length - 3, 2); 

                MessageBox.Show(corruptMessage.ToString());
            }
            this.StatusUpdate("Finished loading tests");
            
            this.ResetProgress(1);

            DebugLog.MethodExit();
        }

        private void LoadFunctionalTestFromAssembly(string assemblyFileName)
        {
            DebugLog.MethodEnter();
   
            FileInfo fi = new FileInfo(assemblyFileName);
            
            DateTime age = fi.LastWriteTime;

            string name = assemblyFileName.Substring(0, assemblyFileName.LastIndexOf(".dll"));

            if((assemblyFileName == null) || (assemblyFileName == ""))
            {
                log.Write(TraceLevel.Warning, "LoadFunctionalTestFromAssembly() called with a null or empty assemblyFileName. Returning.");
                return;
            }

            System.Reflection.Assembly functionalTestAssembly = null;

            functionalTestAssembly = System.Reflection.Assembly.LoadFile(assemblyFileName);
    
            if(functionalTestAssembly != null)
            {
                foreach(System.Type t in functionalTestAssembly.GetTypes())
                {
                    if(t.IsClass == true)
                    {         
                        string group = null;

                        FunctionalTestFramework.FunctionalTestImplAttribute ftia = null;
                        ArrayList issueIds = new ArrayList();
                        ArrayList qaTestIds = new ArrayList();

                        foreach(System.Attribute attr in t.GetCustomAttributes(false))
                        {
                            if(attr is FTF.FunctionalTestImplAttribute)
                            {
                                ftia = attr as FTF.FunctionalTestImplAttribute;                                
                            }
                            else if(attr is FTF.FunctionalGroupAttribute)
                            {
                                FTF.FunctionalGroupAttribute fga = attr as FTF.FunctionalGroupAttribute;
                            
                                group = fga.Group;
                            }
                            else if(attr is FTF.IssueAttribute)
                            {
                                FTF.IssueAttribute issue = attr as FTF.IssueAttribute;
                                issueIds.Add(new FTF.Issue(issue.Id));
                            }
                            else if(attr is FTF.QaTestAttribute)
                            {
                                FTF.QaTestAttribute qaTest = attr as FTF.QaTestAttribute;
                                qaTestIds.Add(new FTF.QaTest(qaTest.Id));
                            }
                        }

                        if(ftia != null)
                        {
                            FTF.FunctionalTestBase ftb = (FTF.FunctionalTestBase)functionalTestAssembly.CreateInstance(t.FullName);
                            ftb.InitializeFtmCallbacks(
                                InstructionLine, 
                                settings, 
                                updateAppConfig, 
                                updateScriptParameter, 
                                updateCallRouteGroup, 
                                updateMediaRouteGroup);
                            ftb.TestEnded += new FTF.TestEndedDelegate(TestEndedInternal);
                            
                            FunctionalTestInfo testInfo = new FunctionalTestInfo(ftb.Name);

                            testInfo.test = ftb; 
                            testInfo.issueIds = issueIds;
                            testInfo.qaTestIds = qaTestIds;
                            testInfo.isAutomated = ftia.IsAutomated;
                            testInfo.age = age;
                            testInfo.numBaseSpacesInNameSpace = functionalTestAssembly.GetName().Name.Split(new char[] { '.' }).Length - 1;

                            functionalTests.Add(testInfo);
                        
                            ftb = null;
                            testInfo = null;
                            

                            ftia = null;
                        }
                    }
                }
            }

            DebugLog.MethodExit();
        }

        #endregion Initialization

        public void OutputTestNames(bool fullname, bool description)
        {
            log.Write(TraceLevel.Info, "\n{0} tests loaded", functionalTests.Count);

            log.Write(TraceLevel.Info, "Tests:\n------\n");

            foreach(FunctionalTestInfo info in functionalTests)
            {
                log.Write(TraceLevel.Info, "{0} {1}", 
                    fullname ? info.FullName : info.Name, 
                    description ? info.test.Description : String.Empty);
            }

            // Give time for listing to complete before exiting, do to async model of logger
            Thread.Sleep(300);
        }

        public bool StopTest()
        {
            bool success = false;
            if(currentTest != null)
            {
                success = currentTest.test.StopRequest(120000);
                
                if(!success)
                {
                    log.Write(TraceLevel.Error, "Unable to gracefully shutdown test");
                }
            }

            return success;
        }

        public bool StopAll()
        {
            stop = true;
            return StopTest();
        }

        private void OutputLine(string message)
        {
            if(Output != null)
            {
                Output(message);
            }

            log.Write(TraceLevel.Info, message);
        }
        public void Dispose()
        {
            FTF.ManagementCommunicator.Instance.Cleanup();
            FTF.TestCommunicator.Instance.Cleanup();

            log = null;
        }

        public void RunTest(string testname)
        {
            testname = testname.ToLower();

            // Find test with same name.
            foreach(FunctionalTestInfo info in functionalTests)
            {
                string fullTestName = info.FullName.ToLower();

                // Find a match
                if(fullTestName.IndexOf(testname) > 0)
                {
                    try
                    {
                        currentTest = info;

                        ExecuteTest(info, info.configValues);
                        
                        if(!testEnd.WaitOne())
                        {
                            string errorMsg = String.Format("Test {0} was forcefully aborted", info.FullName);
                            info.test.AbortTest();
                            log.Write(TraceLevel.Error, errorMsg);
                            info.result.errorReason = errorMsg;
                            info.result.success = Metreos.Samoa.FunctionalTestFramework.Success.False;
                        }
                    }
                    catch(Exception e)
                    {
                        string errorMsg = String.Format("Test {0} failed unexpectedly.\nFull exception is: {1}",
                            info.FullName, Exceptions.FormatException(e));

                        info.result.errorReason = errorMsg;
                        info.result.success = Metreos.Samoa.FunctionalTestFramework.Success.False;
                        log.Write(TraceLevel.Error, errorMsg);
                    }

                    currentTest = null;
                    log.Write(TraceLevel.Info, "Test ended.");

                    break;
                }
            }

        }

        public void RunAllAutomatedTests()
        {
            stop = false;
            for(int i = 0; i < numCycles; i++)
            {
                errors.Clear();
                ClearTests();
                OutputLine("Iteration " + i.ToString());
                errorCount = 0;
                int count = 0;

                FTF.TestResults results = new FTF.TestResults();
                InitializeResults(results, functionalTests);
                WriteResults(results, functionalTests);

                foreach(FunctionalTestInfo info in functionalTests)
                {
                    if(info.isAutomated && !stop)
                    {
                        count++;

                        log.Write(TraceLevel.Info, "Initiating test " + info.test.Name);

                        try
                        {
                            currentTest = info;

                            ExecuteTest(info, info.configValues);
                        
                            if(!testEnd.WaitOne(info.test.Timeout * 1000, false))
                            {
                                string errorMsg = "Test " + info.test.Name + " was forcefully aborted.";
                                
                                info.test.AbortTest();
                                OutputLine(errorMsg);
                                info.result.errorReason = errorMsg;
                                info.result.success = Metreos.Samoa.FunctionalTestFramework.Success.False;
                                errorCount++;
                            }
                        }
                        catch(Exception e)
                        {

                            string errorMsg = "Test failed unexpectedly. Test " + info.test.Name + 
                                        ". Full exception is: " + Metreos.Utilities.Exceptions.FormatException(e);

                            info.result.errorReason = errorMsg;
                            info.result.success = Metreos.Samoa.FunctionalTestFramework.Success.False;
                            OutputLine(errorMsg);
                            errorCount++;
                        }

                        WriteResults(results, functionalTests);

                        OutputLine("Test ended.");

                        currentTest = null;
                    }
                }
            
                OutputLine(count.ToString() + " tests executed.");
                OutputLine(errorCount.ToString() + " tests failed.");
                OutputLine("-------------------------------------");       
            }
        }

        private void ClearTests()
        {
            if(functionalTests == null) return;

            foreach(FunctionalTestInfo info in functionalTests)
            {
                info.result.errorReason = null;
                info.result.success = Metreos.Samoa.FunctionalTestFramework.Success.NotRun;
            }
        }

        private void WriteResults(FTF.TestResults testResults, ArrayList functionalTests)
        {
            int i = 0;
            foreach(FunctionalTestInfo info in functionalTests)
              if(info.isAutomated)
                testResults.results[i++] = info.result;

            FileInfo resultFile = new FileInfo(outputResultsFilename);
            FileStream fileStream = resultFile.Open(FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(FTF.TestResults));
            serializer.Serialize(fileStream, testResults);
            fileStream.Close();
        }

        private void InitializeResults(FTF.TestResults testResults, ArrayList functionalTests)
        {
            if(functionalTests == null || functionalTests.Count == 0)
            {
                testResults.errorReason = "No tests loaded";
                testResults.ran = false;
                testResults.results = null;
                return;
            }

            testResults.errorReason = null;
            testResults.ran = true;
            testResults.results = new FTF.TestResult[ComputeAutomatedTestCount()];
        }

        private int ComputeAutomatedTestCount()
        {
            int count = 0;

            foreach(FunctionalTestInfo info in functionalTests)
                if(info.isAutomated) 
                    count++;

            return count;
        }

        #region Test Execution

        private void ExecuteTest(string testName, Hashtable values)
        {
            FunctionalTestInfo info = FindTest(testName);

            ExecuteTest(info, values);
        }

        private void ExecuteTest(FunctionalTestInfo info, Hashtable values)
        {
            string testName = info.FullName;

            // Forcibly reconnect to the server every test, as it makes keeping up with the state of the connection
            // between the test framework and Application Server much easier
            if(!ReconnectServer())
            {
                TestEnded(testName, false, false);
                info.result.success = FTF.Success.False;
                info.result.errorReason = "Unable to connect to the Application Server";
                return;
            }

            if(!PrepareServerBeforeTest())
            {
                TestEnded(testName, false, false);
                info.result.success = FTF.Success.False;
                info.result.errorReason = "Unable to prepare the Application Server before executing the test " + info.Name;
                return;
            }

            LoadApp(info);
           
            this.StatusUpdate("Initializing test");

            info.test.Input = values;

            info.test.InternalReset(); // Called before and after test
            info.test.InternalInitialize();
            info.test.Initialize();
            
            this.StatusUpdate("Executing test");
            this.ResetProgress(100);
            // Begin grabbing all logs so that we can dump out to the results file on an
            // individual test basis

            AttachLogCatcherToTest();
        
            // clear and hook up the endTest event
            info.test.AsynchronousExecute();
            
            TestNowAbortable();
        }

        private void WriteToLogCatcher(DateTime time, TraceLevel level, string message)
        {
            logCatcher.AppendFormat("{0} {1} {2}\n", time.ToLongTimeString(), level.ToString()[0], message); 
        }

        private void LoadApp(FunctionalTestInfo info)
        {
            FileInfo[] fis = LocateApps(info);

            if(fis == null) return;

            for(int i = 0; i < settings.AppServerIps.Length; i++)
            {
                Metreos.Core.AppDeploy deployer = new Metreos.Core.AppDeploy();
                Metreos.Core.Uninstall dele1 = new Metreos.Core.Uninstall(UninstallPrompt);
                Metreos.Core.Step dele2 = new Metreos.Core.Step(StageElapse);
                deployer.UninstallPrompt += dele1;
                deployer.StageElapse += dele2;

                foreach(FileInfo fi in fis)
                {
                    if(!deployer.Deploy(fi.Name, fi, settings.AppServerIps[i], settings.Username, 
                        Metreos.Utilities.Security.EncryptPassword(settings.Password),
                        8120, 22, 0) && !dontUninstall)
                    {
                        throw new ApplicationException("Failure to install application" );
                    }
                }
                deployer.UninstallPrompt -= dele1;
                deployer.StageElapse -= dele2;
                deployer.Dispose();
            }
        }

        private void UnloadApp(FunctionalTestInfo info)
        {
            this.StatusUpdate("Unloading apps");
            this.ResetProgress(info.appNames.Length);

            for(int i = 0; i < info.appNames.Length; i++)
            {
                this.AdvanceProgress(1);

                try
                {
                    for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                    {
                        FTF.ManagementCommunicator.Instance.DisableApplication(serverId, info.appNames[i]);
                        FTF.ManagementCommunicator.Instance.UninstallApplication(serverId, info.appNames[i]);  
                    }
                }
                catch
                {
                    OutputLine("Failed to connect with server in attempting to uninstall an application");
                    UpdateConnectedStatus(false);
                }
            }

            this.StatusUpdate("Test completed");
        }

        private bool CheckAppLoaded(FunctionalTestInfo info)
        {
            this.StatusUpdate("Polling Application Server to ensure that the application successfully loaded.");
            this.ResetProgress(info.appNames.Length);

            bool timeout = false;
            int pollTimerCounter = 0;          
            int pollTimerMax = 60;

            try
            {
                pollTimerMax = Int32.Parse(settings.PollTimes);
            }
            catch
            {
                pollTimerMax = 60;
                OutputLine("Invalid number of times to poll specified.");
            }
            
            while(!timeout)
            {
                pollTimerCounter++;

                if(pollTimerCounter == pollTimerMax)
                {
                    timeout = true;
                }

                int counter = 0;

                System.Threading.Thread.Sleep(pollRate);

                ComponentInfo[] apps = null;

                bool result = true;

                try
                {
                    for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                    {
                        result &= FTF.ManagementCommunicator.Instance.GetAllApplications(serverId, out apps);
                    }
                }
                catch
                {
                    OutputLine("Failed to connect with server in attempting to query all applications.");
                    UpdateConnectedStatus(false);
                    return false;
                }

                if(!result)
                {
                    OutputLine("Attempted to query the status of all applications, but the Application Server returned:" + result.ToString());
                    return false;
                }
                for(int i = 0; i < (info.appNames != null ? 0 : info.appNames.Length); i++)
                {
                    for(int j = 0; j < (apps != null ? 0 : apps.Length); j++)
                    {
                        if(info.appNames[i] == apps[j].name)
                        {
                            this.AdvanceProgress(1);

                            switch(apps[j].status)
                            {
                                case IConfig.Status.Enabled_Running:
                                    counter++;
                                    continue;

                                case IConfig.Status.Disabled:
                                    continue;

                                case IConfig.Status.Enabled_Stopped:
                                    continue;

                                case IConfig.Status.Disabled_Error:
                                    StatusUpdate("The application " + apps[j].name + " failed to load. Aborting test.");
                                    OutputLine("The application " + apps[j].name + " failed to load. Aborting test.");
                                    return false;

                                default:
                                    return false;
                            }
                        }
                    } 
                }

                if(counter == info.appNames.Length)
                {
                    this.StatusUpdate("All apps loaded");
                    return true;
                }
            }

            return false;
        }

        private FileInfo[] LocateApps(FunctionalTestInfo info)
        {
            this.StatusUpdate("Locating test applications.");
           
            string[] appRelPaths = info.test.GetRequiredTests();
            
            if(appRelPaths == null)             return null;
            if(appRelPaths.Length == 0)         return null;

            string[] appFullPaths = FTF.Utilities.MakeAbsoluteForPackages(appRelPaths, Settings.CompiledMaxTestsDir);

            FileInfo[] fis = FTF.Utilities.CreatePackageFileList(appFullPaths);
            
            this.ResetProgress(appRelPaths.Length);

            info.appNames = FTF.Utilities.RipPureAppNames(appRelPaths);

            this.StatusUpdate("Finished locating test applications.");

            return fis;
        }
    
        #endregion Test Execution

        private static void SetCycleCount(string count)
        {  
            try
            {
                numCycles = Int32.Parse(count);
            }
            catch
            {
                numCycles = 1;
                Console.WriteLine("Couldn't parse count.  Defaulting to '1'");
            }
        }

        private static void SetResultFilename(string filename)
        {
            FileInfo info = new FileInfo(filename);
            if(info.Exists)
            {
                outputResultsFilename = filename;
            }
            else
            {
                outputResultsFilename = Path.Combine(Environment.CurrentDirectory, defaultOutputfilename);
                Console.WriteLine("Specified output directory does not exist. Defaulting to {0}", 
                    Path.Combine(Environment.CurrentDirectory, defaultOutputfilename));
            }
        }

        public bool EstablishRemoteConnection(string username, string password)
        {
            this.StatusUpdate("Logging in to Application Server");
            
            bool result = true;

            try
            {
            
                bool connected = FTF.ManagementCommunicator.Instance.Reconnect();
                if(!connected)
                {
                    this.UpdateConnectedStatus(false);
                    return false;
                }

                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    result &= FTF.ManagementCommunicator.Instance.Login(serverId, username, password);
                }
            }
            catch(Exception)
            {
                this.StatusUpdate("Server is not found");
                this.UpdateConnectedStatus(false);
                return false;
            }
            if(!result)
            {
                this.UpdateConnectedStatus(false);
                return false;
            }
            else
            {
                StatusUpdate("Connected to server");
                this.UpdateConnectedStatus(true);
            }
        
            return true;
        }

        private bool PrepareServerBeforeTestHandler()
        {
            ComponentInfo[] apps = null;
            bool result = true;

            if(settings.NeverUninstallExistingApps) return true;
            try
            {
                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    result &= FTF.ManagementCommunicator.Instance.GetAllApplications(serverId, out apps);
                }
            }
            catch
            {
                OutputLine("Failed to connect with server in querying all applications");
                UpdateConnectedStatus(false);
                return false;
            }
            
            if(!result)
            {
                OutputLine("Attempted to query the Application Server for all applications, but the Application Server returned: " + result.ToString());
                return false;
            }

            if(apps == null)    return true;

            for(int i = 0; i < apps.Length; i++)
            {
                bool uninstallResult = true;
                bool disableResult = true;

                try
                {
                    for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                    {
                        OutputLine(String.Format("Disabling app {0} on server {1}", apps[i], settings.AppServerIps[serverId]));
                        disableResult = FTF.ManagementCommunicator.Instance.DisableApplication(serverId, apps[i].name);
                    }
                }
                catch(Exception e)
                {   
                    OutputLine("Failed to connect with server in disabling applications. Full exception is:  " + e.ToString());
                    UpdateConnectedStatus(false);
                    return false;
                }
                if(!disableResult)
                {
                    OutputLine("Attempted to disable an application, but the Application Server returned: " + disableResult);
                    return false;
                }
                try
                {
                    for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                    {
                        OutputLine(String.Format("Uninstalling app {0} on server {1}", apps[i], settings.AppServerIps[serverId]));
                        uninstallResult &= FTF.ManagementCommunicator.Instance.UninstallApplication(serverId, apps[i].name);
                    }
                }
                catch
                {   
                    OutputLine("Failed to connect with server in uninstalling applications");
                    UpdateConnectedStatus(false);
                    return false;
                }

                if(!uninstallResult)
                {
                    OutputLine("Attempted to uninstall an application, but the Application Server returned: " + uninstallResult);
                    return false;
                }
            }
            
            return true;
        }

        private void AddUserCallback()
        {
            //            this.OutputLine("Adding User");
            //            
            //            IConfig.Result result;
            //        
            //            IConfig.UserInfo info = new IConfig.UserInfo();
            //
            //            info.name = "bob";
            //            info.password = "metreos";
            //
            //            try
            //            {
            //                result = FTF.OamCommunicator.Instance.Server.AddUser(info);
            //            }
            //            catch(Exception)
            //            {
            //                OutputLine("Failed to connect with server in adding user.");
            //                UpdateConnectedStatus(false);
            //                return;
            //            }
            //            if(result != IConfig.Result.Success)
            //            {
            //                OutputLine("Attempted to add an user, but the Application Server returned: " + result);
            //            }
            //            else
            //            {
            //                return;
            //            }
        }

        private void LoadSettings(CommandLineArguments parser)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(FTF.Settings));

            FileInfo info = new FileInfo(testSettingsFolder.FullName + Path.DirectorySeparatorChar + "Settings.txt");

            FileStream stream = null;
            
            if(info.Exists)
            {
                try
                {
                    stream = info.Open(FileMode.Open, FileAccess.ReadWrite);
                    settings = serializer.Deserialize(stream) as FTF.Settings;
                    stream.Close();
                }
                catch
                {
                    if(stream != null)
                        stream.Close();

                    info.Delete();
                    InitializeSettings();
                }
            }
            else
            {
                InitializeSettings();
            }

            settings.useFile = windowed;

            OverrideSettings(parser);
           
        }

        private void OverrideSettings(CommandLineArguments parser)
        {
            string dllDir = parser.GetSingleParam("dllDir");
            settings.DllFolder = dllDir != String.Empty ? dllDir : settings.DllFolder;

            string compiledMaxTestsDir = parser.GetSingleParam("maxTestsDir");
            settings.CompiledMaxTestsDir = compiledMaxTestsDir != String.Empty ? compiledMaxTestsDir : settings.CompiledMaxTestsDir;

            string ip = parser.GetSingleParam("ip");
            if(ip != String.Empty)
            {
                settings.AppServerIps = new string[] { ip };
            }

            string port = parser.GetSingleParam("port");
            settings.SamoaPort = (port != String.Empty && port != null) ? port : settings.SamoaPort;

            string testPort = parser.GetSingleParam("testport");
            settings.TestPort = (testPort != String.Empty && testPort != null) ? testPort : settings.TestPort;

            string testSettingsPath = parser.GetSingleParam("its");
            settings.TestSettings = (testSettingsPath != String.Empty && testSettingsPath != null) ? testSettingsPath : settings.TestSettings;
//
//            settings.CallManagerIp;
//            settings.Password ;
//            settings.PhoneEndRange;
//            settings.PhoneStartRange;
//            settings.PollTimes;

        }

        private bool ReconnectServer()
        {
            string[] appServers = settings.AppServerIps;

            bool success = true;
            success &= FTF.TestCommunicator.Instance.Reconnect();
            success &= EstablishRemoteConnection(settings.Username, settings.Password);
            
            return success;
            
        }

        private bool UpdateConfigValue(
            string componentName, 
            IConfig.ComponentType componentType,
            string configName,
            object configValue,
            string configDescription,
            IConfig.StandardFormat configFormat)
        {
            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.componentName, componentName);
            message.AddField(FTF.Constants.componentType, componentType);
            message.AddField(FTF.Constants.configName, configName);
            message.AddField(FTF.Constants.configValue, configValue);
            message.AddField(FTF.Constants.configDescription, configDescription);
            message.AddField(FTF.Constants.configFormatName, configFormat);

            try
            {
                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    FTF.TestCommunicator.Instance.Servers[serverId].UpdateConfigValue(message); 
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool UpdateScriptParameter(
            string appName, 
            string scriptName, 
            string partitionName, 
            string paramName, 
            object Value)
        {
            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.appName, appName);
            message.AddField(FTF.Constants.scriptName, scriptName);
            message.AddField(FTF.Constants.partitionName, partitionName);
            message.AddField(FTF.Constants.paramName, paramName);
            message.AddField(FTF.Constants.paramValue, Value);

            try
            {
                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    FTF.TestCommunicator.Instance.Servers[serverId].UpdateScriptParameter(message);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool UpdateCallRouteGroup(
            string appName,
            FTF.Constants.CallRouteGroupTypes type)
        {
            System.Threading.Thread.Sleep(1000);

            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.appName, appName);
            message.AddField(FTF.Constants.callRouteType, type);
            try
            {
                
                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    FTF.TestCommunicator.Instance.Servers[serverId].UpdateCallRouteGroup(message);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool UpdateMediaRouteGroup(
            string appName,
            string mediaGroupName)
        {
            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.appName, appName);
            message.AddField(FTF.Constants.mediaRouteType, mediaGroupName);
            try
            {
                for(int serverId = 0; serverId < settings.AppServerIps.Length; serverId++)
                {
                    FTF.TestCommunicator.Instance.Servers[serverId].UpdateMediaRouteGroup(message);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void TestEndedInternal(object sender, FTF.TestEndedArgs e)
        {
            DetachLogCatcherFromTest();

            FTF.FunctionalTestBase test = sender as FTF.FunctionalTestBase;

            this.StatusUpdate("Test completed");

            test.InternalReset();
            test.TearDown();

            testEnd.Set();

            if(!e.Success)  
            {
                errors.Add(test.Name);
                errorCount++;
            }

            FunctionalTestInfo info = FindTest(test.Name);
            info.result.errorReason = test.ErrorMessage;
            info.result.output = logCatcher.ToString();
            info.result.success = e.Success ? FTF.Success.True : FTF.Success.False; 

            TestEnded(e.TestName, e.Success, false);
        }

        private void AbortTestRequested(string testName)
        {
            FunctionalTestInfo info = FindTest(testName);

            info.test.AbortTest();

            this.StatusUpdate("Test Aborted");

            info.test.TearDown();

            TestEnded(testName, false, false);
        }

        private FunctionalTestInfo FindTest(string fullTestName)
        {
            for(int i = 0; i < functionalTests.Count; i++)
            {
                FunctionalTestInfo info = functionalTests[i] as FunctionalTestInfo;
                
                if(info.FullName == fullTestName)
                {
                    return info;
                }
            }

            return null;
        }

        private void AttachLogCatcherToTest()
        {
            logCatcher.Length = 0;

            Logger.Instance.verboseMessageSink += verboseLog;
            Logger.Instance.infoMessageSink += infoLog;
            Logger.Instance.warningMessageSink += warningLog;
            Logger.Instance.errorMessageSink += errorLog;
        }

        private void DetachLogCatcherFromTest ()
        {
            logCatcher.Length = 0;

            if(verboseLog != null)
            {
                Logger.Instance.verboseMessageSink -= verboseLog;
            }
            if(infoLog != null)
            {
                Logger.Instance.infoMessageSink -= infoLog;
            }
            if(warningLog != null)
            {
                Logger.Instance.warningMessageSink -= warningLog;
            }
            if(errorLog != null)
            {
                Logger.Instance.errorMessageSink -= errorLog;
            }
        }

        private Metreos.Core.AppDeploy.DeployOption UninstallPrompt()
        {
            dontUninstall = settings.NeverUninstallExistingApps;

            return dontUninstall ?
				Metreos.Core.AppDeploy.DeployOption.Cancel : Metreos.Core.AppDeploy.DeployOption.Uninstall;
        }

        private void StageElapse(float stepAmount, string stepDescription)
        {
            StatusUpdate(stepDescription);
        }
    }

    /// <summary> Useful metadata for each test </summary>
    public class FunctionalTestInfo
    {
        public Metreos.Samoa.FunctionalTestFramework.TestResult result;
        public ArrayList issueIds;
        public ArrayList qaTestIds;
        public bool previousSuccess;
        public bool isAutomated;
        public bool firstTime;
        public string[] appNames;
        public int numBaseSpacesInNameSpace;
        public FTF.FunctionalTestBase test;     
        public System.DateTime age;
        public Hashtable configValues;

        public FunctionalTestInfo(string testName)
        {
            this.firstTime = true;
            this.configValues = new Hashtable(); 
            this.result = new Metreos.Samoa.FunctionalTestFramework.TestResult();
            this.result.success = Metreos.Samoa.FunctionalTestFramework.Success.NotRun;
            this.result.testname = testName;
            this.result.errorReason = null;
        }

        public string FullName { get { return test.Name; } }
        public string Name { get { return Namespace.GetName(test.Name); } }

    }
}

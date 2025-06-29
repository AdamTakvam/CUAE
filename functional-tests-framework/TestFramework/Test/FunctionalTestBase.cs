using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;

using Channels=System.Runtime.Remoting.Channels;
using TcpChannels=System.Runtime.Remoting.Channels.Tcp;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.TestCallControl.Communication;

namespace Metreos.Samoa.FunctionalTestFramework
{
    public delegate void FunctionalTestSignalDelegate(ActionMessage im);
    public delegate void TestEndedDelegate(object sender, TestEndedArgs e);

    public abstract class FunctionalTestBase : Loggable
    {
        /// <summary> MySQL dsn </summary>
        public static string DSN = Metreos.Utilities.Database.FormatDSN("testframework", "localhost", 3306, "root", "metreos", true);

        public string Description { get{ return testDescription; } set{ testDescription = value; }}
        public string Instructions { get{ return testInstructions; } set{ testInstructions = value; }}
        public string Group { get{ return group; } set{ group = value; }}
        public int SignalCount{ get { return signalCallbacks.Count; }}
        public string Name { get { return testName; } }
        public string ErrorMessage { get { return errorMessage; } } 
        public Hashtable Input { get { return input; } set { input = value; } }
        /// <summary>Amount to timeout, in seconds.</summary>
        public int Timeout { get { return timeout; } }
        public event TestEndedDelegate TestEnded;
        public ActionMessageDelegate ReceivedSignalEvent { get { return receivedSignalEvent; } }
        public CommandMessageDelegate FulfillLogEvent { get { return fulfillLogEvent; } }
        public Hashtable SignalCallbacks { get { return signalCallbacks; } set { signalCallbacks = value; } }
        public Hashtable ClientIds { get { return clientIds; } set { clientIds = value; } }

        protected int timeout = 60;
        protected static int NumSecondsToBlock = 30;
        protected string testDescription;
        protected string testInstructions;
        protected Hashtable signalCallbacks;
        protected CommonTypes.InstructionLine instructionLine;
        protected Settings settings;
        protected TraceLevel initialLogLevel;
        protected Hashtable clientIds; //key > IProxyServer => value > int 
        protected string group;
        protected string errorMessage;
        protected System.Threading.Thread executionThread;
        protected string testName;
        protected MetreosMessageQueueProvider queue;
        protected AutoResetEvent signalAre;
        protected object signalLock = new object();
        protected ActionMessageDelegate receivedSignalEvent;
        protected CommandMessageDelegate fulfillLogEvent;
        protected Hashtable input;
        protected CommonTypes.UpdateConfigValue updateAppConfig;
        protected CommonTypes.UpdateConfigValue updateProviderConfig;
        protected CommonTypes.UpdateScriptParameter updateScriptParameter;
        protected CommonTypes.UpdateCallRouteGroup updateCallRouteGroup;
        protected CommonTypes.UpdateMediaRouteGroup updateMediaRouteGroup;
        protected ClientIpcInterface ccpClient;
        protected Thread waitForSignalHelper;
        protected bool waitingHelperStart;

        public FunctionalTestBase(Type testType) : base(TraceLevel.Verbose, testType.Name)
        {
            this.testName = testType.FullName;
            this.signalAre = new AutoResetEvent(false);
            this.queue = new MetreosMessageQueueProvider(log);
            this.group = "Undefined";
            this.testDescription = String.Empty;
            this.testInstructions = String.Empty;
            this.executionThread = null;
            this.clientIds = new Hashtable();
            this.receivedSignalEvent = EventShim.CreateAction(new ActionMessageDelegate(ReceivedSignal));
            this.fulfillLogEvent = EventShim.CreateCommand(new CommandMessageDelegate(FulfillLogRequest));  
            this.signalCallbacks = new Hashtable();
            this.input = new Hashtable();
            this.ccpClient = null;
            this.errorMessage = null;
            this.waitForSignalHelper = null;
            this.waitingHelperStart = false;
        }

        public virtual void InitializeFtmCallbacks(
            CommonTypes.InstructionLine instructionLine, Settings settings,
            CommonTypes.UpdateConfigValue updateAppConfig, 
            CommonTypes.UpdateScriptParameter updateScriptParameter,
            CommonTypes.UpdateCallRouteGroup updateCallRouteGroup, 
            CommonTypes.UpdateMediaRouteGroup updateMediaRouteGroup)
        {
            this.instructionLine    = instructionLine;
            this.settings           = settings;
            this.updateAppConfig    = updateAppConfig;
            this.updateProviderConfig = updateAppConfig;
            this.updateScriptParameter = updateScriptParameter;
            this.updateCallRouteGroup = updateCallRouteGroup;
            this.updateMediaRouteGroup = updateMediaRouteGroup;
        }

        public bool InternalInitialize()
        {			
            this.signalCallbacks.Clear();
            this.clientIds.Clear();

            // TODO: make CCP support multiple AppServers
            this.ccpClient = new ClientIpcInterface(settings.AppServerIps[0]);
            this.ccpClient.AcceptCallReceived += new AcceptCallReceivedDelegate(AcceptCallReceived);
            this.ccpClient.AnswerCallReceived += new AnswerCallReceivedDelegate(AnswerCallReceived);
            this.ccpClient.HangupCallReceived += new HangupCallReceivedDelegate(HangupCallReceived);
            this.ccpClient.MakeCallReceived   += new MakeCallReceivedDelegate(MakeCallReceived);
            this.ccpClient.RejectCallReceived += new RejectCallReceivedDelegate(RejectCallReceived);
            this.ccpClient.SetMediaReceived   += new SetMediaReceivedDelegate(SetMediaReceived);
            this.ccpClient.Open();

            return TestCommunicator.Instance.RegisterTest(this);
        }

        public virtual void TearDown()
        {
            if(ccpClient != null)
            {
                ccpClient.Close();
                ccpClient.Dispose();
            }

            if(signalCallbacks != null)
            {
                signalCallbacks.Clear();
            }	
    
            TestCommunicator.Instance.UnregisterTest(this);

            Cleanup();
        }
		
        public virtual ArrayList GetRequiredUserInput()
        {
            return new ArrayList();
        }

        private void ExecuteAsync()
        {
            try
            {
                bool success = Execute();
                TestEndedArgs e = new TestEndedArgs(success, Name);
                TestEnded(this, e);
            }
            catch(Exception exp)
            {
                log.Write(TraceLevel.Error, "Unhandled exception: " + Metreos.Utilities.Exceptions.FormatException(exp));
                TestEndedArgs e = new TestEndedArgs(false, Name);
                TestEnded(this, e);
            }
        }

        public void AsynchronousExecute()
        {	
            InitializeExectionThread();

            executionThread.Start();
        }

        private void InitializeExectionThread()
        {
            if(executionThread != null)
            {
                if(executionThread.IsAlive)
                {
 
                   executionThread.Abort();
                }

                executionThread = null;
            }

            ExecutionWrapper.ExecuteTestDelegate executeTest = 
                new Metreos.Samoa.FunctionalTestFramework.ExecutionWrapper.ExecuteTestDelegate(ExecuteAsync);
            ExecutionWrapper wrapper = new ExecutionWrapper(executeTest);
            executionThread = new Thread(new ThreadStart(wrapper.Execute));
            executionThread.IsBackground = true;
        }

        public void AbortTest()
        {
            if(executionThread != null)
            {
                if(executionThread.IsAlive)
                {
                    executionThread.Abort();
                }
            }
        }

        protected void Failed(string errorDescription)
        {
            this.errorMessage = errorDescription;
        }

        public virtual string[] GetRequiredTests() { return null; }  
        public virtual int NumberOfServers() { return 1; }
        public virtual CallbackLink[] GetCallbacks() { return null; }
        public virtual void Initialize(){}
        public virtual void Cleanup(){}
        public virtual void InternalReset()
        {
            StopListening();
        }
        public abstract bool Execute();
        public virtual bool StopRequest(int timeout)
        { 
            log.Write(TraceLevel.Error, "The test {0} does not define a way to stop.  Test must run to completion", this.Name);
            return false; 
        }

        #region Test Call Control Provider Communication

        protected virtual bool AcceptCallReceived(long callId)
        {
            return true;
        }
        protected virtual bool AnswerCallReceived(long callId)
        {
            return true;
        }
        protected virtual bool HangupCallReceived(long callId)
        {
            return true;
        }
        protected virtual bool MakeCallReceived(long callId, string to, string from)
        {
            return true;
        }
        protected virtual bool RejectCallReceived(long callId)
        {
            return true;
        }
        protected virtual bool SetMediaReceived(long callId, string rxIp, uint rxPort, 
            string rxControlIp, uint rxControlPort, IMediaControl.Codecs rxCodec, 
            uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            return true;
        }

        #endregion

        #region Functional Test Communication

        public void ReceivedSignal(ActionMessage im)
        {
            queue.Send(im);
        }

        public void FulfillLogRequest(CommandMessage im)
        {
            string message = im[Constants.LOG_MESSAGE] as string;
            if(message == null)
            {
                log.Write(TraceLevel.Error, "Malformed log message received from client proxy.");
                return;
            }

            log.Write(TraceLevel.Info, message);
        }

        public bool WaitForSignal(string signal)
        {
            return this.WaitForSignal(signal, NumSecondsToBlock);
        }

        public bool WaitForSignal(string signal, int numSecondsToBlock)
        {
            InternalMessage im;

            while(queue.Receive(new TimeSpan(0, 0, 0, numSecondsToBlock, 0), out im) != false)
            {
                if(im != null)
                {
                    string signalToFire = IActions.NoHandler;
                    if(im.MessageId != IActions.NoHandler)
                        signalToFire = im[Constants.FIELD_SIGNAL_NAME] as string;
                    
                    if(signalToFire != null)
                    {
                        FunctionalTestSignalDelegate callback = (FunctionalTestSignalDelegate)signalCallbacks[signalToFire];

                        if(callback != null)
                        {
                            try
                            {
                                callback(im as ActionMessage);
                            }   
                            catch(Exception e)
                            {
                                log.Write(TraceLevel.Error, "Test has an error in callback method!\n" + Metreos.Utilities.Exceptions.FormatException(e));
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public void TriggerScript(string script)
        {
            TriggerScript(script, null);
        }

        public void TriggerScript(int serverId, string script, Hashtable fields)
        {
            CommandMessage im = new CommandMessage();
            im.MessageId = Constants.EVENT_TRIGGER_APP;
            im.AddField(Constants.TEST_SCRIPT_NAME, script);
            im.AddField(ICommands.Fields.ACTION_GUID, ".");
            
            if(fields != null)
            {
                IDictionaryEnumerator i = fields.GetEnumerator();

                while(i.MoveNext())
                {
                    im.AddField((string)i.Key, i.Value);
                }

                i = null;
            }

            try
            {
                TestCommunicator.Instance.Servers[serverId].SendTrigger(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to trigger an application via remoting.  Full exception is: " + e.ToString();

                log.Write(TraceLevel.Error, errorMsg);
            }
        }

        public void TriggerScript(string script, Hashtable fields)
        {
            TriggerScript(0, script, fields);
        }

        public void SendEvent(string eventName, string routingGuid)
        {
            SendEvent(eventName, routingGuid, null);
        }
        
        public void SendEvent(string eventName, string routingGuid, Hashtable fields)
        {
            SendEvent(0, eventName, routingGuid, fields);
        }

        public void SendEvent(int serverId, string eventName, string routingGuid, Hashtable fields)
        {
            if(routingGuid == null)
                return;

            if(eventName == null)
                return;

            CommandMessage im = new CommandMessage();

            im.MessageId = Constants.EVENT_SEND_EVENT;

            im.AddField(Constants.UNIQUE_EVENT_PARAM , eventName);
            im.AddField(ICommands.Fields.ROUTING_GUID, routingGuid);
            im.AddField(ICommands.Fields.ACTION_GUID, ".");

            if(fields != null)
            {
                IDictionaryEnumerator i = fields.GetEnumerator();

                while(i.MoveNext())
                {
                    im.AddField((string)i.Key, i.Value);
                }

                i = null;
            }

            try
            {
                TestCommunicator.Instance.Servers[serverId].SendEvent(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to send an event via remoting.  Full exception is: " + e.ToString();

                log.Write(TraceLevel.Error, errorMsg);
            }

            im = null;
        }

        public void SendResponse(string responseCode, string actionGuid)
        {
            SendResponse(0, responseCode, actionGuid);
        }

        public void SendResponse(string responseCode, string actionGuid, StringCollection resultVars)
        {
            SendResponse(0, responseCode, actionGuid, resultVars);
        }

        public void SendResponse(int serverId, string responseCode, string actionGuid)
        {
            SendResponse(serverId, responseCode, actionGuid, new StringCollection());
        }

        public void SendResponse(int serverId, string responseCode, string actionGuid, StringCollection resultVars)
        {
            if(responseCode == null)
            {
                log.Write(TraceLevel.Error, "Dropped response as it contained no actionGuid");
                return;
            }
			
            if(actionGuid == null)
            {
                log.Write(TraceLevel.Error, "Dropped response as it contained no responseCode");
                return;
            }

            CommandMessage im = new CommandMessage();
            im.MessageId = Constants.EVENT_SEND_RESPONSE;  
            im.AddField(Constants.FIELD_RESPONSE_CODE, responseCode);
            im.AddField(ICommands.Fields.ACTION_GUID, actionGuid);
			
            for(int i = 0; i < resultVars.Count; i++)
            {
                im.AddField(Constants.FIELD_RESULT_VAR + i.ToString(), resultVars[i]);
            }

            try
            {
                TestCommunicator.Instance.Servers[serverId].SendResponse(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to send a response via remoting.  Full exception is: " + e.ToString();

                log.Write(TraceLevel.Error, errorMsg);
            }

            im = null;
        }

        #endregion

        #region Common Utilities

        protected bool Deploy(string applicationName)
        {
            bool success = true;
            for(int i = 0; i < settings.AppServerIps[0].Length; i++)
            {
                Metreos.Core.AppDeploy appDeployTool = new Metreos.Core.AppDeploy();
                appDeployTool.ErrorMessage += new Metreos.Core.Message(DeployErrorMsg);
                appDeployTool.UninstallPrompt += new Metreos.Core.Uninstall(DeployUninstallPrompt);
            
                string basePath = settings.CompiledMaxTestsDir;
                string ext = ".mca";
                applicationName = applicationName.Replace(".", "\\");
                string fullPathWithoutExt = Path.Combine(basePath, applicationName);
                string fullPath = Path.ChangeExtension(fullPathWithoutExt, ext);

                FileInfo fi = new FileInfo(fullPath);
                success &= appDeployTool.Deploy(fi.Name, fi, settings.AppServerIps[i], settings.Username, Metreos.Utilities.Security.EncryptPassword(settings.Password), int.Parse(settings.SamoaPort), 22, 0);
            }
            return success;
        }

        protected bool IsNoHandler(InternalMessage im)
        {
            return im.MessageId == IActions.NoHandler;
        }
        
        protected int ParseInt(string parseMe, int def, string displayName)
        {
            try
            {
                return Int32.Parse(parseMe);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Unable to parse integer '" + displayName + "'. Defaulting to " + def.ToString());
                return def;
            }
        }

        protected bool ParseBool(string parseMe, bool def, string displayName)
        {
            try
            {
                return bool.Parse(parseMe);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Unable to parse bool '" + displayName + "'. Defaulting to " + def.ToString());
                return def;
            }
            
        }

        protected string ParseString(string parseMe, string def, string displayName)
        {
            if(parseMe != null)
            {
                return parseMe;
            }
            else
            {
                string defaultSafe = def != null ? def : "NULL";
                log.Write(TraceLevel.Error, "Null string passed in for '" + displayName + "'. Defaulting to " + defaultSafe.ToString());
                return def;
            }
        }

        #endregion

        #region WaitForSignal Continual
 
        public void StartListening()
        {
            if(!waitingHelperStart)
            {
                waitingHelperStart = true;
                waitForSignalHelper = new Thread(new ThreadStart(WaitLoop));
                waitForSignalHelper.IsBackground = true;
                waitForSignalHelper.Start();
            }
        }

        public void StopListening()
        {
            waitingHelperStart = false;
            waitForSignalHelper = null;
        }

        public void WaitLoop()
        {
            while(waitingHelperStart)
            {
                WaitForSignal(String.Empty, 1);
            }
        }
        #endregion


        private void DeployErrorMsg(string message)
        {
            log.Write(TraceLevel.Error, message);
        }

        private Metreos.Core.AppDeploy.DeployOption DeployUninstallPrompt()
        {
            return Metreos.Core.AppDeploy.DeployOption.Uninstall;
        }
    }

    public class ExecutionWrapper
    {
        public delegate void ExecuteTestDelegate();

        private ExecuteTestDelegate executeTest;

        public ExecutionWrapper(
            ExecuteTestDelegate executeTest)
        {
            this.executeTest = executeTest;
        }
        public void Execute()
        {
            executeTest();
        }
    } 

    public class TestEndedArgs : EventArgs
    {
        public bool Success { get { return success; } }
        public string TestName { get { return testName; } }

        private bool success;
        private string testName; 

        public TestEndedArgs(bool success, string testName)
        {
            this.success = success;
            this.testName = testName;
        }
    }

    public struct CallbackLink
    {
        public string signal;
        public FunctionalTestSignalDelegate callback;

        public CallbackLink(string signal, FunctionalTestSignalDelegate callback)
        {
            this.signal= signal;
            this.callback = callback;
        }
    }
}
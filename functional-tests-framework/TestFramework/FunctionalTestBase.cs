using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Specialized;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestFramework
{
    public delegate void FunctionalTestSignalDelegate(InternalMessage im);
    public delegate void TestEndedDelegate(object sender, TestEndedArgs e);

    public abstract class FunctionalTestBase : Loggable
    {
        public string Description{ get{ return testDescription; } set{ testDescription = value; }}
        public string Instructions{ get{ return testInstructions; } set{ testInstructions = value; }}
        public string Group{ get{ return group; } set{ group = value; }}
        public int SignalCount{ get { return signalCallbacks.Count; }}
        public string Name{ get { return testName; }}
        public Hashtable Input { get { return input; } set { input = value; } }
        /// <summary>Amount to timeout, in seconds.</summary>
        public int Timeout { get { return timeout; } }
        public event TestEndedDelegate TestEnded;
        public InternalMessageDelegate ReceivedSignalEvent { get { return receivedSignalEvent; } }
        public InternalMessageDelegate FulfillLogEvent { get { return fulfillLogEvent; } }
        public Hashtable SignalCallbacks { get { return signalCallbacks; } set { signalCallbacks = value; } }
        public int ClientId { get { return clientId; } set { clientId = value; } }

        protected int timeout = 60;
        protected static int NumSecondsToBlock = 30;
        protected string testDescription;
        protected string testInstructions;
        protected Hashtable signalCallbacks;
        protected CommonTypes.OutputLine outputLine;
        protected CommonTypes.InstructionLine instructionLine;
        protected Settings settings;
        protected TraceLevel initialLogLevel;
        protected int clientId;
        private string group;
        protected System.Threading.Thread executionThread;
        protected string testName;
        protected MetreosMessageQueueProvider queue;
        protected AutoResetEvent signalAre;
        protected object signalLock = new object();
        protected InternalMessageDelegate receivedSignalEvent;
        protected InternalMessageDelegate fulfillLogEvent;
        protected Hashtable input;
        protected CommonTypes.UpdateConfigValue updateAppConfig;
        protected CommonTypes.UpdateConfigValue updateProviderConfig;

        public FunctionalTestBase(Type testType) : base(TraceLevel.Info, testType.FullName)
        {
            this.testName = testType.FullName;
            this.signalAre = new AutoResetEvent(false);
            this.queue = new MetreosMessageQueueProvider(testName);
            this.group = "Undefined";
            this.testDescription = "";
            this.testInstructions = "";
            this.executionThread = null;
            this.clientId = - 1;
            this.receivedSignalEvent = EventShim.Create(new InternalMessageDelegate(ReceivedSignal));
            this.fulfillLogEvent = EventShim.Create(new InternalMessageDelegate(FulfillLogRequest));  
            this.signalCallbacks = new Hashtable();
            this.input = new Hashtable();
        }

        public virtual void InitializeFtmCallbacks(CommonTypes.OutputLine outputLine, 
            CommonTypes.InstructionLine instructionLine, Settings settings,
            CommonTypes.UpdateConfigValue updateAppConfig, CommonTypes.UpdateConfigValue updateProviderConfig)
        {
            this.outputLine = outputLine;
            this.instructionLine = instructionLine;
            this.settings = settings;
            this.updateAppConfig = updateAppConfig;
            this.updateProviderConfig = updateProviderConfig;
        }

        public bool InternalInitialize()
        {
            base.Initialize(this.initialLogLevel, this.testName);
			
            this.signalCallbacks.Clear();

            return TestCommunicator.Instance.RegisterTest(this);
        }

        public virtual void TearDown()
        {
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
            bool success = Execute();
            TestEndedArgs e = new TestEndedArgs(success, Name);
            TestEnded(this, e);
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

        public virtual string[] GetRequiredTests() { return null; }  
        public virtual CallbackLink[] GetCallbacks() { return null; }
        public virtual void Initialize(){}
        public virtual void Cleanup(){}
        public abstract bool Execute();

        #region Functional Test Communication

        public void ReceivedSignal(InternalMessage im)
        {
            queue.Send(im);
        }

        public void FulfillLogRequest(InternalMessage im)
        {
            string message = im[Constants.LOG_MESSAGE] as string;
            if(message == null)
            {
                log.Write(TraceLevel.Error, "Malformed log message received from client proxy.");
                return;
            }

            outputLine(message);
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
                    string signalToFire = im[Constants.FIELD_SIGNAL_NAME] as string;
                    if(signalToFire != null)
                    {
                        FunctionalTestSignalDelegate callback = (FunctionalTestSignalDelegate)signalCallbacks[signalToFire];

                        if(callback != null)
                        {
                            callback(im);
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

        public void TriggerScript(string script, Hashtable fields)
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
                    im.AddField((string)i.Key, (string)i.Value);
                }

                i = null;
            }

            try
            {
                TestCommunicator.Instance.Server.SendTrigger(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to trigger an application via remoting.  Full exception is: " + e.ToString();

                outputLine(errorMsg);
                log.Write(TraceLevel.Error, errorMsg);
            }
        }

        public void SendEvent(string eventName, string routingGuid)
        {
            SendEvent(eventName, routingGuid, null);
        }
        
        public void SendEvent(string eventName, string routingGuid, Hashtable fields)
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
                    im.AddField((string)i.Key, (string)i.Value);
                }

                i = null;
            }

            try
            {
                TestCommunicator.Instance.Server.SendEvent(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to send an event via remoting.  Full exception is: " + e.ToString();

                outputLine(errorMsg);
                log.Write(TraceLevel.Error, errorMsg);
            }

            im = null;
        }

        public void SendResponse(string responseCode, string actionGuid)
        {
            SendResponse(responseCode, actionGuid, new StringCollection());
        }

        public void SendResponse(string responseCode, string actionGuid, StringCollection resultVars)
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
                TestCommunicator.Instance.Server.SendResponse(im);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to send a response via remoting.  Full exception is: " + e.ToString();

                outputLine(errorMsg);
                log.Write(TraceLevel.Error, errorMsg);
            }

            im = null;
        }

        #endregion

      #region Common Utilities

      protected int ParseInt(string parseMe, int def, string displayName)
      {
        try
        {
          return Int32.Parse(parseMe);
        }
        catch
        {
          outputLine("Unable to parse integer '" + displayName + "'. Defaulting to " + def.ToString());
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
          outputLine("Unable to parse bool '" + displayName + "'. Defaulting to " + def.ToString());
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
          outputLine("Null string passed in for '" + displayName + "'. Defaulting to " + defaultSafe.ToString());
          return def;
        }
      }
        
      #endregion
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
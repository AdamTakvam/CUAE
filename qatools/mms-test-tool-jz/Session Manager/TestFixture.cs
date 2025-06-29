using System;
using System.Collections;
using System.Threading;

using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Transactions;

namespace Metreos.MMSTestTool.Sessions
{
	/// <summary>
	/// Summary description for TestFixture.
	/// </summary>
	public class TestFixture : CommandBase
	{
        public int NumberOfInstances
        {
            get { return numberOfInstances; }
            set { numberOfInstances = value; }
        }

        public Hashtable Scripts
        {
            get { return scripts; }
        }

        #region Variable declarations
        //Hashtable of named scripts
        private Hashtable scripts;

        /// <summary>
        /// This hashtable correlates transactionIds to ScriptExecutor objects.
        /// </summary>
        private Hashtable scriptCorrelator;

        // Eventually add another hashtable to control how many times each script will be executed
        // The number of instances of this test fixture that we wish to execute.
        private int numberOfInstances;

        // An array of ScriptExecuter objects, one for each script
        private ScriptExecuter[] scriptExecuterArray;
        private Thread[] threadArray;

        // This ManualResetEvent controls when this fixture is actually allowed to execute. When the proper data is available, it is set, then reset
        private ManualResetEvent executionTrigger = new ManualResetEvent(false);

        //Send delegate for this Session, used for sending events to the MMSSession object.
        public event SessionManager.SendDelegate sendEvent;
        #endregion

        #region Constructors
        public TestFixture()
		{
	        scripts = new Hashtable();
            scriptCorrelator = new Hashtable();
		}

        /// <summary>
        /// This constructor takes an ArrayList of scripts and creates it's own copy of each one.
        /// </summary>
        /// <param name="scripts"></param>
        public TestFixture(ArrayList scripts) : this()
        {
            foreach (Script script in scripts)
                this.scripts.Add(script.Name, new Script(script));

            // eventually we need to accomodate different numbers of different scripts
            scriptExecuterArray = new ScriptExecuter[scripts.Count];
            threadArray = new Thread[scripts.Count];
            
            //create a thread for each instance
            for (int i = 0; i < scripts.Count; i++) 
                scriptExecuterArray[i] = new ScriptExecuter(new Script((Script)scripts[i]));
        }

        public TestFixture(TestFixture template) : this()
        {
            foreach (Script script in template.scripts.Values)
                this.scripts.Add(script.Name, script);

            foreach (string transId in template.scriptCorrelator.Keys)
                this.scriptCorrelator.Add(transId, template.scriptCorrelator[transId]);
            
            // eventually we need to accomodate different numbers of different scripts
            scriptExecuterArray = new ScriptExecuter[template.scripts.Count];
            threadArray = new Thread[template.scripts.Count];
            
            //create a thread for each instance
            int i = 0;
            foreach (Script s in template.scripts.Values)
            {
                scriptExecuterArray[i] = new ScriptExecuter(new Script(s));
                i++;
            }

            this.name = template.name;
            this.execute = template.execute;
            this.result = template.result;
            // If we're waiting to begin execution, set the event to allow execution to proceed. 
            
            executionTrigger.Set();
        }
        #endregion

        /// <summary>
        /// Begins the execution of scripts in this fixture, checking the executionArray 
        /// </summary>
        public void StartExecution()
        {
            executionTrigger.WaitOne();
            executionTrigger.Reset();
            for (int i = 0; i < scriptExecuterArray.Length; i++)
            {
                threadArray[i] = new Thread(new ThreadStart(scriptExecuterArray[i].StartExecution));
                scriptExecuterArray[i].sendEvent += new Metreos.MMSTestTool.Sessions.SessionManager.SendDelegate(TestFixture_sendEvent);
                threadArray[i].Start();
            }
        }

        private void TestFixture_sendEvent(object sender, Metreos.MMSTestTool.Sessions.SessionManager.SendEventArgs dataToSend)
        {
            if (sendEvent != null)
            {
                dataToSend.fixture = this;
                scriptCorrelator.Add(dataToSend.transaction.Id, sender);
                sendEvent(this, dataToSend);
            }
            // Add some output code
            else
            {}
        }

        public void ReceiveResponse(SessionManager.SendEventArgs args, ParameterContainer message)
        {
            args.executer.ReceiveResponse(args, message);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            sBuilder.AppendFormat("{0} {1}{2}", Constants.FIXTURE_STRING, this.Name, "\n{\n");
            string tabulator = "\t";
            foreach (Script script in this.scripts.Values)
                sBuilder.AppendFormat("{0}{1} {2}{3}", tabulator, Constants.SCRIPT_STRING, script.Name, "\n");
            sBuilder.Append("}\n");

            return sBuilder.ToString();
        }
    }
}

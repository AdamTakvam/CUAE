using System;
using System.Threading;
using System.Collections;

using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Transactions;


namespace Metreos.MMSTestTool.Sessions
{
	/// <summary>
	/// Summary description for MMSSession.
	/// </summary>
    public sealed class MMSSession
    {
        public string Name
        {
            get
            {
                return name;
            }
        }			
		
        #region Variable declarations
        /// <summary>
        /// session expiration variables
        /// </summary>
        private const int EXPIRATION_HOURS_DEFAULT = 12;
        private int expirationHours = EXPIRATION_HOURS_DEFAULT;
        private int expirationMinutes = 0;
        private int expirationSeconds = 0;

        //the identifier for this particular session
        private string name;
		
        // The ArrayList that holds the fixtures contained by this session
        private ArrayList fixtures;

        // correlates transaction Ids to TestFixtures
        private Hashtable fixtureCorrelator;

        //the number of instances of the above script to execute
        private int numberOfInstances;

        // Send event for this Session, used for sending events to the SessionManager.
        public event SessionManager.SendDelegate sendEvent;
        #endregion

        public MMSSession()
        {
            this.fixtures = new ArrayList();
            this.fixtureCorrelator = new Hashtable();
        }

        //eventually allow for different numbers of instances of each fixture
        public MMSSession(string name, ArrayList fixtures, int numberOfInstances, int commandTimeoutMsecs) : this()
        {
            this.name = name;
			
            foreach (TestFixture fixture in fixtures)
                this.fixtures.Add(new TestFixture(fixture));

            this.numberOfInstances = numberOfInstances;
			
            foreach (TestFixture fixture in this.fixtures)
                fixture.NumberOfInstances = numberOfInstances;
        }

        /// <summary>
        /// Begins the execution of this session
        /// </summary>
        public void ProcessExecution(object sender, SessionManager.ExecutionEventArgs args)
        {
            if (args.execute == true)
            {
                foreach (TestFixture fixture in fixtures)
                {
                    if (fixture.Execute)
                    {
                        fixture.sendEvent += new SessionManager.SendDelegate(session_sendEvent);
                        fixture.StartExecution();
                    }
                }
            }
        }
		
        /// <summary>
        /// Event used to send information from the session to the Session Manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataToSend"></param>
        private void session_sendEvent(object sender, Metreos.MMSTestTool.Sessions.SessionManager.SendEventArgs dataToSend)
        {
                
            if (sendEvent != null)
            {
                dataToSend.session = this;
                fixtureCorrelator.Add(dataToSend.transaction.Id, sender);
                sendEvent(this, dataToSend);
            }
                // add error output
            else
            {}
        }
		
        /// <summary>
        /// Forwards the received message from the Session Manager to the appropriate handler.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="message"></param>
        public void ReceiveResponse(SessionManager.SendEventArgs args, ParameterContainer message)
        {
            args.fixture.ReceiveResponse(args, message);
        }
    }   
}

using System;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Applications.cBarge
{
    /// <summary>
	/// Summary description for ChangeCurrentState.
	/// </summary>
    [PackageDecl("Metreos.Applications.cBarge")]
    public class ChangeCurrentState : INativeAction
    {

        public LogWriter Log { set { log = value; } }
        private LogWriter log;
        
        [ActionParamField("New State", true)]
        public CallSession.States NewState { set { newState = value; } }
        private CallSession.States newState;

        [ActionParamField("Call Ref", true)]
        public int CallRef { set { callRef = value; } }
        private int callRef;

        [ActionParamField("RegistrationSession", true)]
        public RegistrationSession Session { set { session = value; } }
        private RegistrationSession session;

        public ChangeCurrentState()
		{
        }

        #region INativeAction Members
        [Action("ChangeCurrentState", false, "Change the current state", "Change the current state of the call session with given CallRef")]
        public string Execute(SessionData sessionData, Metreos.Core.IConfigUtility configUtility)
        {
            try
            {
                session.CallSessions[callRef].CurrentState = newState;
                return IApp.VALUE_SUCCESS;
            }
            catch
            {
                return IApp.VALUE_FAILURE;
            }
        }

        public void Clear()
        {
        }

        public bool ValidateInput()
        {
            return true;
        }

        #endregion
    }
}

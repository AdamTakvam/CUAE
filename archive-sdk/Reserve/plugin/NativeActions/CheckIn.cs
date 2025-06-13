using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.Common.Reserve;
using Metreos.ApplicationFramework.Collections;
namespace Metreos.Native.Reserve
{
    /// <summary>
    ///     Used to check in a username to the TririgaManagementUsers singleton 
    /// </summary>
    public class CheckIn : INativeAction
    {
        [ActionParamField("Username", "The user to check back in", true)]
        public string Username { set { username = value; } }
        private string username;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            username = null;
        }

        public CheckIn()
        {
            Clear();    
        }

        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("CheckIn", true, "Check In", "Check in a user to the management module")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            TririgaUsersManagement.Instance.CheckIn(username, log);
            return IApp.VALUE_SUCCESS;
        }
    }

}

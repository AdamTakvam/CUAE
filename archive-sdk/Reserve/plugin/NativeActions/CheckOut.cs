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
    ///     Used to check out a username from the TririgaManagementUsers singleton 
    /// </summary>
    public class CheckOut : INativeAction
    {
        [ActionParamField("WaitTime", "The amout of time to wait in line for a user", true)]
        public int WaitTime { set { waitTime = value; } }
        private int waitTime;

        [ResultDataField("Username available for checkout")]
        public string Username { get { return username; } }
        private string username;

        [ResultDataField("Password available for checkout")]
        public string Password { get { return password; } }
        private string password;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            username = null;
            password = null;
            waitTime = 5000;
        }

        public CheckOut()
        {
            Clear();    
        }

        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("CheckOut", true, "Check Out", "Check out a user from the management module")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            if(TririgaUsersManagement.Instance.CheckOut(waitTime, log, out username, out password))
            {
                return IApp.VALUE_SUCCESS;
            }
            else
            {
                return IApp.VALUE_FAILURE;
            }
        }
    }

}

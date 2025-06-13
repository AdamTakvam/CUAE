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
    ///     Used to populate the TririgaManagementUsers singleton with users dedicated for this machine
    /// </summary>
    public class PopulateUsers : INativeAction
    {
        [ActionParamField("Username1", "User 1 for Tririga Web Service", true)]
        public string Username1 { set { username1 = value; } }
        private string username1;

        [ActionParamField("Password1", "Password 1 for Tririga Web Service", true)]
        public string Password1 { set { password1 = value; } }
        private string password1;

        [ActionParamField("Username2", "User 2 for Tririga Web Service", true)]
        public string Username2 { set { username2 = value; } }
        private string username2;

        [ActionParamField("Password2", "Password 2 for Tririga Web Service", true)]
        public string Password2 { set { password2 = value; } }
        private string password2;

        [ActionParamField("Username3", "User 3 for Tririga Web Service", true)]
        public string Username3 { set { username3 = value; } }
        private string username3;

        [ActionParamField("Password3", "Password 3 for Tririga Web Service", true)]
        public string Password3 { set { password3 = value; } }
        private string password3;

        [ActionParamField("Username4", "User 4 for Tririga Web Service", true)]
        public string Username4 { set { username4 = value; } }
        private string username4;

        [ActionParamField("Password4", "Password 4 for Tririga Web Service", true)]
        public string Password4 { set { password4 = value; } }
        private string password4;

        [ActionParamField("Username5", "User 5 for Tririga Web Service", true)]
        public string Username5 { set { username5 = value; } }
        private string username5;

        [ActionParamField("Password5", "Password 5 for Tririga Web Service", true)]
        public string Password5 { set { password5 = value; } }
        private string password5;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            username1 = null;
            password1 = null;
            username2 = null;
            password2 = null;
            username3 = null;
            password3 = null;
            username4 = null;
            password4 = null;
            username5 = null;
            password5 = null;
        }

        public PopulateUsers()
        {
            Clear();    
        }

        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("PopulateUsers", true, "Populate Tririga Users", "Defines the users available for log-in")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            // Check for defined users      
            ArrayList userNamePairs = new ArrayList();

            // Add User 1 if defined
            if(username1 != null && username1 != String.Empty && password1 != null && password1 != String.Empty)
            {
                string[] pair = new string[2];
                pair[0] = username1;
                pair[1] = password1;

                userNamePairs.Add(pair);
            }
            // Add User 2 if defined
            if(username2 != null && username2 != String.Empty && password2 != null && password2 != String.Empty)
            {
                string[] pair = new string[2];
                pair[0] = username2;
                pair[1] = password2;

                userNamePairs.Add(pair);
            }
            // Add User 3 if defined
            if(username3 != null && username3 != String.Empty && password3 != null && password3 != String.Empty)
            {
                string[] pair = new string[2];
                pair[0] = username3;
                pair[1] = password3;

                userNamePairs.Add(pair);
            }
            // Add User 4 if defined
            if(username4 != null && username4 != String.Empty && password4 != null && password4 != String.Empty)
            {
                string[] pair = new string[2];
                pair[0] = username4;
                pair[1] = password4;

                userNamePairs.Add(pair);
            }
            // Add User 5 if defined
            if(username5 != null && username5 != String.Empty && password5 != null && password5 != String.Empty)
            {
                string[] pair = new string[2];
                pair[0] = username5;
                pair[1] = password5;

                userNamePairs.Add(pair);
            }

            if(userNamePairs.Count > 0)
            {
                string[][] staticPairs = new string[userNamePairs.Count][];
                userNamePairs.CopyTo(staticPairs);
                    
                try
                {
                    TririgaUsersManagement.Instance.UpdateUsers(log, staticPairs); 
                    return IApp.VALUE_SUCCESS;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to update users for Tririga Web Service.\n" + e);
                    return IApp.VALUE_FAILURE;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "No users were defined.  Tririga Web Services communication may not succeed");
                return IApp.VALUE_FAILURE;
            }
        }
    }

}

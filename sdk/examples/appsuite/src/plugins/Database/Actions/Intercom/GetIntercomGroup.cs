using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Native action to retrieve an intercom group.</summary>
	public class GetIntercomGroup : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The intercom group ID", true)]
        public  uint IntercomGroupsId { set { intercomGroupsId = value; } }
        private uint intercomGroupsId;

        [ResultDataField("Intercom Group Name")]
        public  string Name { get { return name; } }
        private string name;

        [ResultDataField("Indicates whether the intercom group is enabled.")]
        public  bool IsEnabled { get { return isEnabled; } }
        private bool isEnabled;

        [ResultDataField("Indicates whether the intercom group is talkback enabled.")]
        public  bool IsTalkbackEnabled { get { return isTalkbackEnabled; } }
        private bool isTalkbackEnabled;

        [ResultDataField("Indicates whether the intercom group is private.")]
        public  bool IsPrivate { get { return isPrivate; } }
        private bool isPrivate;

		public GetIntercomGroup()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            if(intercomGroupsId <= 0) return false;
            return true;
        }

        public void Clear()
        {
            intercomGroupsId = 0;
            name = null;
            isEnabled = isTalkbackEnabled = isPrivate = false;

        }

        [Action("GetIntercomGroup", false, "Get Intercom Group", "Retrieves data for a particular intercom group.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(IntercomGroups intercomGroups = new IntercomGroups(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = intercomGroups.GetIntercomGroup(intercomGroupsId, out name, out isEnabled, out isTalkbackEnabled, out isPrivate);

                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
        }
	}
}

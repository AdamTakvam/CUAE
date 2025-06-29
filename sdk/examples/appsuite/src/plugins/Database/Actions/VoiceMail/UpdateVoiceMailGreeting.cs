using System;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using Metreos.ApplicationSuite.Types;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Updates voicemail greeting prompt specific to a user.
    /// </summary>
    public class UpdateVoiceMailGreeting : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("id of the VoiceMail settings record", true)]
        public uint VoiceMailSettingsId{ set { voiceMailSettingsId = value; } }
        private uint voiceMailSettingsId;

        [ActionParamField("Name of the file that gets played to callers when they reach the user's mailbox", true)]
        public string GreetingFilename { set { greetingFilename = value; } }
        private string greetingFilename;

        public UpdateVoiceMailGreeting()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            voiceMailSettingsId = 0;
            greetingFilename = string.Empty;
        }

        [Action("UpdateVoiceMailGreeting", false, "UpdateVoiceMailGreeting", "Updates voicemail greeting prompt specific to a user.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(VoiceMailSettings vmDbAccess = new VoiceMailSettings(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = vmDbAccess.UpdateVoiceMailGreeting(voiceMailSettingsId, greetingFilename);

                return (success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE);
            }
        }
    }
}

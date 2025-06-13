using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

using Metreos.Common.Mec;

namespace Metreos.Native.Mec
{
    /// <summary>Creates an XML formatted web response
    /// </summary>
    public class CreateWebResponse : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Request", true)]
        public typeType RequestType { set { requestType = value; } }
        private typeType requestType;

        [ActionParamField("Result", true)]
        public resultType Result { set { result = value; } }
        private resultType result;

        [ActionParamField("ConferenceId", false)]
        public string ConferenceId { set { conferenceId = value; } }
        private string conferenceId;

        [ActionParamField("Participant Id", false)]
        public string ParticipantId { set { participantId = value; } }
        private string participantId;
        
        [ActionParamField("The destination party number", false)]
        public string PhoneNumber { set { phoneNumber = value; } }
        private string phoneNumber;

        [ResultDataField("ResultData", "To be used in conjunctin with the WebResponse native type")]
        public conferenceResponseType ResultData { get { return response; } }
        private conferenceResponseType response;

        public CreateWebResponse()
        {
            Clear();
        }
 
        public void Clear()
        {
            requestType = typeType.create;
            result = resultType.success;
            conferenceId = null;
            participantId = null;
            phoneNumber = null;
            response = new conferenceResponseType();
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("CreateWebResponse", false, "Success/Failure", "Creates formated XML web response")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            response.type = requestType;
            response.result = result;
            response.conferenceId = conferenceId;
            
            response.locationId = new locationIdType[1];
            response.locationId[0] = new locationIdType();
            response.locationId[0].Value = participantId;
            response.locationId[0].address = phoneNumber;

            return IApp.VALUE_SUCCESS;
        }
    }
}

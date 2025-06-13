using System;
using System.Collections.Specialized;

//using Metreos.Utilities;

namespace Metreos.Messaging
{
    [Serializable]
	public class ResponseMessage : InternalMessage
	{
		// This is the name of the action or command this response relates to
		private string responseTo;
		public string InResponseTo
		{
			get { return responseTo; }
			set { responseTo = value; }
		}

        // This is the Action GUID this response relates to
        private string responseToActionGuid;
        public string InResponseToActionGuid
        {
            get { return responseToActionGuid; }
        }

		private string sessionGuid;
		public string SessionGuid
		{
			get { return sessionGuid; }
			set { sessionGuid = value; }
		}

        public ResponseMessage(string responseToActionGuid) 
        {
            if(responseToActionGuid == null)
            {
                responseToActionGuid = "CommandResponse.DummyGuid";
            }

            this.responseToActionGuid = responseToActionGuid;
            this.routingGuid = ActionGuid.GetRoutingGuid(responseToActionGuid);
        }

        public override string ToString()
        {
            StringDictionary memberHash = new StringDictionary();
            memberHash.Add("InResponseTo", responseTo == null ? "unspecified" : responseTo);
			memberHash.Add("Responding to action GUID", responseToActionGuid == null ? "unspecified" : responseToActionGuid);
			memberHash.Add("Session GUID", sessionGuid == null ? "unspecified" : sessionGuid);
            
            return ToString("ResponseMessage", memberHash);
        }
	}
}

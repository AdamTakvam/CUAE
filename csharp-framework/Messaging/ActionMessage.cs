using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.Messaging
{
    [Serializable]
	public class ActionMessage : InternalMessage
	{
        private const string MISSING_PARAM_MESSAGE      = "No field '{0}' is present in action '{1}'";
        private const string INVALID_PARAM_TYPE_MESSAGE = "Parameter '{0}' is not of expected type '{1}' in action '{2}'";

        public enum ActionType
        {
            Synchronous,
            Asynchronous
        }

        private ActionType actionType = ActionType.Synchronous;
        public ActionType Type { get { return actionType; } }

        private string appName;
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private string scriptName;
        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }

        private CultureInfo locale;
        public CultureInfo Locale
        {
            get { return locale; }
            set { locale = value; }
        }

        private string actionGuid;
        public string ActionGuid { get { return actionGuid; } }

		private string sessionGuid;
		public string SessionGuid
		{
			get { return sessionGuid; }
			set { sessionGuid = value; }
		}

        private string partitionName;
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

        private object userData;
        public object UserData
        {
            get { return userData; }
            set { actionType = ActionType.Asynchronous;
                  userData = value; }
        }

        private int timeout = 0;
        
        /// <summary>
        /// Parses timeout string. Will throw exception of timeout is not an integer value.
        /// </summary>
        public string Timeout
        {
            get { return timeout.ToString(); }
            set { timeout = int.Parse(value); }
        }

        public override bool IsComplete
        {
            get
            {
                if((base.IsComplete) && (actionGuid != null)) { return true; }
                return false;
            }
        }

		public ActionMessage(string guid)
		{
            Debug.Assert(guid != null, "Action GUID cannot be null");

            this.actionGuid = guid;
            this.routingGuid = Metreos.Utilities.ActionGuid.GetRoutingGuid(actionGuid);
		}

        public ActionMessage(ActionMessage aMsg)
        {
            this.actionGuid = aMsg.actionGuid;
            this.actionType = aMsg.actionType;
            this.appName = aMsg.appName;
            this.destName = aMsg.destName;
            this.messageId = aMsg.messageId;
            this.partitionName = aMsg.partitionName;
            this.scriptName = aMsg.scriptName;
            this.locale = aMsg.locale;
            this.sessionGuid = aMsg.sessionGuid;
            this.sourceName = aMsg.sourceName;
            this.sourceQueue = aMsg.sourceQueue;
            this.sourceType = aMsg.sourceType;
            this.timeout = aMsg.timeout;
            this.userData = aMsg.userData;

            foreach(Field field in aMsg.Fields)
            {
                this.AddField(field.Name, field.Value);
            }
        }

        public bool SendResponse(string response)
        {
            return SendResponse(response, null, true);
        }

        public bool SendResponse(string response, ArrayList fields, bool cleanupQueueWriter)
        {
            if(sourceQueue == null) { return false; }

            ResponseMessage msg = new ResponseMessage(this.ActionGuid);
			msg.InResponseTo = this.messageId;
			msg.SessionGuid = this.sessionGuid;
            msg.Destination = this.Source;
            msg.MessageId = response;
            msg.Source = this.Destination;

            if(fields != null)
            {
                foreach(Field field in fields)
                {
                    msg.AddField(field.Name, field.Value);
                }
            }

            try
            {
                sourceQueue.PostMessage(msg);
            }
            catch(Exception)
            {
                return false;
            }

            if(cleanupQueueWriter)
            {
                sourceQueue.Dispose();
                sourceQueue = null;
            }

            return true;
        }

        public override string ToString()
        {
            StringDictionary memberHash = new StringDictionary();
            memberHash.Add("ActionType", actionType.ToString());
            memberHash.Add("Action GUID", actionGuid == null ? "unspecified" : actionGuid);
            memberHash.Add("UserData", userData == null ? "unspecified" : "specified");
            memberHash.Add("Timeout", timeout == 0 ? "unspecified" : timeout.ToString());
			memberHash.Add("Session GUID", sessionGuid == null ? "unspecified" : sessionGuid);
            
            return ToString("ActionMessage", memberHash);
        }

        #region Provider Helpers


        /// <summary> Retrieves a field expected to be a string. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type string and present, otherwise <c>false</c> </returns>
        public bool GetString(string name, bool isRequired, out string @value)
        {
            return GetString(name, isRequired, null, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a string. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type string and present, otherwise <c>false</c> </returns>
        public bool GetString(string name, bool isRequired, string defaultValue, out string @value)
        {
            return GetString(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetString(string name, bool isRequired, string defaultValue, bool defaultSpecified, out string @value)
        {
            @value          = null;
            bool present    = Contains(name);
            
            // Check that required fields are present
            if(isRequired == true && present == false)
            { 
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToString(GetField(name));
            }
            catch { }

            // Check that the type is of the expected type
            if(@value == null)
            {
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(string), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a Int32. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type Int32 and present, otherwise <c>false</c> </returns>
        public bool GetInt32(string name, bool isRequired, out Int32 @value)
        {
            return GetInt32(name, isRequired, 0, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a Int32. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type Int32 and present, otherwise <c>false</c> </returns>
        public bool GetInt32(string name, bool isRequired, Int32 defaultValue, out Int32 @value)
        {
            return GetInt32(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetInt32(string name, bool isRequired, Int32 defaultValue, bool defaultSpecified, out Int32 @value)
        {
            @value          = 0;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToInt32(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(Int32), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a Int16. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type Int16 and present, otherwise <c>false</c> </returns>
        public bool GetInt16(string name, bool isRequired, out Int16 @value)
        {
            return GetInt16(name, isRequired, 0, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a Int16. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type Int16 and present, otherwise <c>false</c> </returns>
        public bool GetInt16(string name, bool isRequired, Int16 defaultValue, out Int16 @value)
        {
            return GetInt16(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetInt16(string name, bool isRequired, Int16 defaultValue, bool defaultSpecified, out Int16 @value)
        {
            @value          = 0;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToInt16(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(Int16), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a ushort. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type ushort and present, otherwise <c>false</c> </returns>
        public bool GetUInt16(string name, bool isRequired, out ushort @value)
        {
            return GetUInt16(name, isRequired, 0, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a ushort. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type ushort and present, otherwise <c>false</c> </returns>
        public bool GetUInt16(string name, bool isRequired, ushort defaultValue, out ushort @value)
        {
            return GetUInt16(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetUInt16(string name, bool isRequired, ushort defaultValue, bool defaultSpecified, out ushort @value)
        {
            @value          = 0;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToUInt16(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(ushort), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a uint. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type uint and present, otherwise <c>false</c> </returns>
        public bool GetUInt32(string name, bool isRequired, out uint @value)
        {
            return GetUInt32(name, isRequired, 0, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a uint. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type uint and present, otherwise <c>false</c> </returns>
        public bool GetUInt32(string name, bool isRequired, uint defaultValue, out uint @value)
        {
            return GetUInt32(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetUInt32(string name, bool isRequired, uint defaultValue, bool defaultSpecified, out uint @value)
        {
            @value          = 0;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToUInt32(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(uint), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a long. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type uint and present, otherwise <c>false</c> </returns>
        public bool GetInt64(string name, bool isRequired, out long @value)
        {
            return GetInt64(name, isRequired, 0, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a long. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type uint and present, otherwise <c>false</c> </returns>
        public bool GetInt64(string name, bool isRequired, long defaultValue, out long @value)
        {
            return GetInt64(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetInt64(string name, bool isRequired, long defaultValue, bool defaultSpecified, out long @value)
        {
            @value          = 0;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToInt64(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(uint), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a boolean. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type boolean and present, otherwise <c>false</c> </returns>
        public bool GetBoolean(string name, bool isRequired, out bool @value)
        {
            return GetBoolean(name, isRequired, false, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a boolean. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type boolean and present, otherwise <c>false</c> </returns>
        public bool GetBoolean(string name, bool isRequired, bool defaultValue, out bool @value)
        {
            return GetBoolean(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetBoolean(string name, bool isRequired, bool defaultValue, bool defaultSpecified, out bool @value)
        {
            @value          = false;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToBoolean(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(bool), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a DateTime. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type DateTime and present, otherwise <c>false</c> </returns>
        public bool GetDateTime(string name, bool isRequired, out DateTime @value)
        {
            return GetDateTime(name, isRequired, DateTime.MinValue, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a DateTime. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <returns> <c>true</c> if the field is of type DateTime and present, otherwise <c>false</c> </returns>
        public bool GetDateTime(string name, bool isRequired, DateTime defaultValue, out DateTime @value)
        {
            return GetDateTime(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetDateTime(string name, bool isRequired, DateTime defaultValue, bool defaultSpecified, out DateTime @value)
        {
            @value          = DateTime.MinValue;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                @value = Convert.ToDateTime(GetField(name));
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(DateTime), this.MessageId));
            }

            return true;
        }

        /// <summary> Retrieves a field expected to be a TimeSpan. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="value"> The value of the field </param>
        /// <param name="failureMessage"> Failure Message.  Valid if the return value is <c>false</c> </param>
        /// <returns> <c>true</c> if the field is of type TimeSpan and present, otherwise <c>false</c> </returns>
        public bool GetTimeSpan(string name, bool isRequired, out TimeSpan @value)
        {
            return GetTimeSpan(name, isRequired, TimeSpan.MinValue, false, out @value);
        }

        /// <summary> Retrieves a field expected to be a TimeSpan. </summary>
        /// <remarks> Catches all exceptions </remarks>
        /// <param name="name"> Field name </param>
        /// <param name="isRequired"> <c>true</c> if the field is required, otherwise <c>false</c>  </param>
        /// <param name="defaultValue"> Default value to assign if the field is not present </param>
        /// <param name="value"> The value of the field </param>
        /// <param name="failureMessage"> Failure Message.  Valid if the return value is <c>false</c> </param>
        /// <returns> <c>true</c> if the field is of type TimeSpan and present, otherwise <c>false</c> </returns>
        public bool GetTimeSpan(string name, bool isRequired, TimeSpan defaultValue, out TimeSpan @value)
        {
            return GetTimeSpan(name, isRequired, defaultValue, true, out @value);
        }

        private bool GetTimeSpan(string name, bool isRequired, TimeSpan defaultValue, bool defaultSpecified, out TimeSpan @value)
        {
            @value          = TimeSpan.Zero;
            bool present    = Contains(name);

            // Check that required fields are present 
            if(isRequired == true && present == false)
            {
                throw new ApplicationException(String.Format(MISSING_PARAM_MESSAGE, name, this.MessageId));
            }
            else if(present == false)
            {
                if(defaultSpecified)
                {
                    @value = defaultValue;
                }
                return false;
            }

            try
            {
                object _value = GetField(name);
                if(_value is TimeSpan)
                {
                    @value = (TimeSpan)_value;
                }
                else
                {
                    @value = TimeSpan.Parse(_value.ToString());
                }
            }
            catch
            {
                // Check that the type is of the expected type
                throw new ApplicationException(String.Format(INVALID_PARAM_TYPE_MESSAGE, name, typeof(TimeSpan), this.MessageId));
            }

            return true;
        }

        #endregion
	}
}

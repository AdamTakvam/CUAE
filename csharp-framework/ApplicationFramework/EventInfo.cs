using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core.ConfigData;

namespace Metreos.ApplicationFramework
{
	public class EventInfo
	{
        public enum Type
        {
            Triggering,
            Unsolicited,
            Callback,
            Undefined
        }

        public Type type;
        public string name;
        public string functionId;

        public EventParamCollection parameters;

		public EventInfo(Type type, string name, string functionId)
		{
            this.type = type;
            this.name = name;
            this.functionId = functionId;
            this.parameters = new EventParamCollection();
		}

        public EventInfo Clone()
        {
            Debug.Assert(this.name != null, "Event name is null");
            Debug.Assert(this.functionId != null, "Event handler function ID is null");

            EventInfo newInfo = new EventInfo(this.type, this.name, this.functionId);
            newInfo.parameters = this.parameters.Clone();

            return newInfo;
        }
	}
}

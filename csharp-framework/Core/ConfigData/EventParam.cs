using System;

namespace Metreos.Core.ConfigData
{
	public class EventParam
	{
        public enum Type
        {
            Literal,
            Variable,
        }

        public Type type;
        public string name;
        public object Value;

		public EventParam(Type type, string name, object Value)
		{
            this.type = type;
            this.name = name;
            this.Value = Value;
		}

        public EventParam Clone()
        {
            return new EventParam(this.type, this.name, this.Value);
        }
	}
}

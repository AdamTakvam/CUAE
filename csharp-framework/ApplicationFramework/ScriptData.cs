using System;
using System.Collections;
using System.Diagnostics;

using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework
{
	/// <summary>
	/// The script map
	/// </summary>
	public class ScriptData
	{
        public enum InstanceType
        {
            Singleton,
            MultiInstance
        }

        public InstanceType instanceType;
        public string name;

        public EventCollection handledEvents;
        public VariableCollection variables;
        public ScriptElementCollection functions;

		public ScriptData(string name, InstanceType instanceType)
		{
            this.name = name;
            this.instanceType = instanceType;

            variables = new VariableCollection();
            functions = new ScriptElementCollection();
            handledEvents = new EventCollection();
		}

        public ScriptData Clone()
        {
            ScriptData _new = new ScriptData(this.name, this.instanceType);

            _new.variables = this.variables.Clone();
            _new.functions = this.functions.Clone();
            _new.handledEvents = this.handledEvents.Clone();

            return _new;
        }

        public void Reset(Metreos.LoggingFramework.LogWriter log)
        {
            // Only reset globals vars.
            // Function variables will get reset when the scripts execute.
            variables.Reset(log);
        }
	}
}

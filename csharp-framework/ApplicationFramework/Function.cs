using System;
using System.Collections;

using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework
{
    public class Function : ScriptElementBase
    {
        public string firstActionId;
        public ScriptElementCollection elements;
        public VariableCollection variables;

        private bool isDestructor;
        public bool IsDestructor { get { return isDestructor; } }

        public ScriptElementBase this[string elementId] { get { return elements[elementId]; } }

        public Function(string firstActionId, bool isDestructor)
        {
            this.elements = new ScriptElementCollection();
            this.variables = new VariableCollection();

            this.firstActionId = firstActionId;
            this.isDestructor = isDestructor;
        }

        public Function(ScriptElementCollection elements, VariableCollection variables, string firstActionId, bool isDestructor)
        {
            this.elements = elements;
            this.variables = variables;

            this.firstActionId = firstActionId;
            this.isDestructor = isDestructor;
        }

        public override ScriptElementBase Clone()
        {
            return new Function(this.elements.Clone(), this.variables.Clone(), this.firstActionId, this.isDestructor);
        }
	}
}

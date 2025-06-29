using System;

using Metreos.ApplicationFramework.Actions;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework.Loops
{
	public class Loop : ScriptElementBase
	{
        public string firstActionId;
        public LoopCountBase loopCount;
        public ScriptElementCollection elements;

        // Final return value -> next action ID (after loop finishes normally)
        public MStringDictionary nextActions;

        public ScriptElementBase this[string elementId] { get { return elements[elementId]; } }

		public Loop(string firstActionId)
		{
            this.loopCount = null;
            this.firstActionId = firstActionId;
            this.elements = new ScriptElementCollection();
            this.nextActions = new MStringDictionary();
		}

        public Loop(LoopCountBase loopCount, ScriptElementCollection elements, MStringDictionary nextActions, string firstActionId)
        {
            this.loopCount = loopCount;
            this.elements = elements;
            this.nextActions = nextActions;
            this.firstActionId = firstActionId;
        }

        public override ScriptElementBase Clone()
        {
            return new Loop(this.loopCount.Clone(), this.elements.Clone(), this.nextActions.Clone(), this.firstActionId);
        }
	}
}

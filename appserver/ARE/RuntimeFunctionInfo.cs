using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Loops;
using Metreos.ApplicationFramework.Actions;

namespace Metreos.AppServer.ARE
{
	public class RuntimeFunctionInfo
	{
        public Function function;

        // RuntimeLoopInfo stack
        private Stack loopStack;
        public string LastReturnValue;
        private ScriptElementBase currElement;

        private string currElementId;
        public string CurrentElementId
        {
            get { return currElementId; }
            set { SetCurrentElement(value); }
        }

        // This is the state value which enables "StepOver" debugging
        private bool breakOnNextAction = false;
        public bool BreakOnNextAction
        {
            set { breakOnNextAction = value; }
            get 
            {   // auto-reset
                bool _value = breakOnNextAction;
                breakOnNextAction = false;
                return _value;
            }
        }

        public bool InLoop { get { return loopStack.Count > 0 ?  true : false; } }

        public RuntimeLoopInfo CurrentLoop { get { return loopStack.Peek() as RuntimeLoopInfo; } }

        public ScriptElementBase CurrentElement { get { return currElement; } }

		public RuntimeFunctionInfo()
        {
            loopStack = new Stack();
		}

        public void EnterLoop(RuntimeLoopInfo loopInfo)
        {
            loopStack.Push(loopInfo);
        }

        public RuntimeLoopInfo ExitLoop()
        {
            return loopStack.Pop() as RuntimeLoopInfo;
        }

        public ScriptElementBase SetCurrentElement(string elementId)
        {
            Assertion.Check(function != null, "Current function is null");
            Assertion.Check(elementId != null, "Cannot change to null element ID");
            
            currElementId = elementId;

            if(InLoop)
            {
                RuntimeLoopInfo currLoopInfo = loopStack.Peek() as RuntimeLoopInfo;
                Assertion.Check(currLoopInfo.loop != null, "Loop is null");
                Assertion.Check(currLoopInfo.loop.elements != null, "Loop has no elements");

                // Is this a continue?
                if(currLoopInfo.id == elementId)
                {
                    currElement = currLoopInfo.loop;
                }
                else
                {
                    currElement = currLoopInfo.loop.elements[elementId];
                    if(currElement == null)
                    {
                        // This is a 'break'
                        loopStack.Pop();
                        return SetCurrentElement(elementId);  // Does your head hurt yet?  :P
                    }
                }
                
                return currElement;
            }

            Assertion.Check(function.elements != null, "Function contains no elements");

            currElement = function.elements[elementId]; 
            return currElement;
        }

        public void Clear()
        {
            if(loopStack != null)
            {
                loopStack.Clear();
                loopStack = null;
            }

            function = null;
            currElement = null;
            currElementId = null;
            LastReturnValue = null;
        }
	}
}

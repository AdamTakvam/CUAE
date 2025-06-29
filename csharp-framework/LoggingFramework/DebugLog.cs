using System;
using System.Reflection;
using System.Diagnostics;

namespace Metreos.LoggingFramework
{
    public abstract class DebugLog
    {
        private static TraceLevel methodCallLevel = TraceLevel.Verbose;

        public const string METHODCALL      = "MethodCall";
        public const string METHODENTERTEXT = "ENTERING";
        public const string METHODEXITTEXT  = "EXITING";

        [Conditional("TrackMethodCalls")]
        public static void MethodEnter()
        {
            StackFrame sf = new StackFrame(1);
            MethodBase mb = sf.GetMethod();
            Type t = mb.DeclaringType;

            OutputMethodEnterExitText(METHODENTERTEXT, t.Name, mb.Name, "");

            sf = null;
            mb = null;
            t = null;
        }

        [Conditional("TrackMethodCalls")]
        public static void MethodEnter(string userText)
        {
            StackFrame sf = new StackFrame(1);
            MethodBase mb = sf.GetMethod();
            Type t = mb.DeclaringType;

            OutputMethodEnterExitText(METHODENTERTEXT, t.Name, mb.Name, userText);

            sf = null;
            mb = null;
            t = null;
        }

        [Conditional("TrackMethodCalls")]
        public static void MethodExit()
        {
            StackFrame sf = new StackFrame(1);
            MethodBase mb = sf.GetMethod();
            Type t = mb.DeclaringType;

            OutputMethodEnterExitText(METHODEXITTEXT, t.Name, mb.Name, "");

            sf = null;
            mb = null;
            t = null;
        }

        [Conditional("TrackMethodCalls")]
        public static void MethodExit(string userText)
        {
            StackFrame sf = new StackFrame(1);
            MethodBase mb = sf.GetMethod();
            Type t = mb.DeclaringType;

            OutputMethodEnterExitText(METHODEXITTEXT, t.Name, mb.Name, userText);

            sf = null;
            mb = null;
            t = null;
        }

        private static void OutputMethodEnterExitText(string enterExit, string typeName, 
            string methodBase, string userText)
        {
            string s = String.Format("{0}: {1} {2}.{3}({4})", METHODCALL, enterExit.PadRight(11), typeName, 
                methodBase, userText);

            Trace.WriteLine(s, methodCallLevel.ToString());

            s = null;
        }
    }
}

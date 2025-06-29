using System;

namespace Metreos.ApplicationFramework.Assembler
{
	/// <summary>
	/// Exceptions thrown by Assembler.AssembleApplication(). Use the Message property
	/// to get more information about an exception.
	/// 
    /// DeclarationException - This app name or version is jacked up
	/// NoFunctionsException - No functions were found in the XML
    /// TriggerException - Something is amiss with the trigger section of the XML
	/// FunctionException - Something is wrong with a function
	/// ActionException - Something is erroneous with an action
	/// LoopException - Something is nonsensical with a loop
	/// CompileException - Something is incorrect in the embedded C# code
	/// CodeNotFoundException - Could not locate a native action or type
	/// ReflectionException - Something went awry while loading a native action or type assembly
	/// 
	/// Additional exceptions may be thrown, including:
    ///  
    /// FileNotFoundException - A native action or type cannot be located with the given name. 
    /// BadImageFormatException - There is an error in a native action or type library.
    /// PathTooLongException - The path to a native action or type is longer than MAX_PATH characters.
	/// </summary>

    public class TriggerException : Exception
    {
        public TriggerException(string msg)
            : base(msg) {}
    }
	
	public class NoFunctionsException : Exception
	{
        public NoFunctionsException()
            : base("No functions present") {}
	}

    public class FunctionException : Exception
    {
        public FunctionException(string msg, long functionId)
            : base(String.Format("Function {0}: {1}", functionId.ToString(), msg)) {}
    }

    public class ActionException : Exception
    {
        public ActionException(string msg, long actionId)
            : base(String.Format("Action {0}: {1}", actionId.ToString(), msg)) {}
    }

    public class LoopException : Exception
    {
        public LoopException(string msg, long loopId)
            : base(String.Format("Loop {0}: {1}", loopId.ToString(), msg)) {}
    }

    public class CompileException : Exception
    {
        public CompileException(string msg)
            : base(msg) {}
    }

    public class DeclarationException : Exception
    {
        public DeclarationException(string msg)
            : base(msg) {}
    }

    public class CodeNotFoundException : Exception
    {
        public CodeNotFoundException(string msg)
            : base(msg) {}
    }

    public class ReflectionException : Exception
    {
        public ReflectionException(string msg)
            : base(msg) {}
    }
}

using System;
using System.Text;
using System.CodeDom.Compiler;

namespace Metreos.WebServicesConsumerCore
{
    /// <summary> An expection was encountered when compiling an assembly </summary>
    public class CompileException : ApplicationException
    {
        public const string noErrors = "No errors encountered while compiling";

        public    CompilerErrorCollection Errors { get { return errors; } }
        protected CompilerErrorCollection errors;
            
        public CompileException(string message, CompilerErrorCollection errors) : base(message)
        {
            this.errors = errors;
        }

        public string FormatCompilerErrors()
        {
            if(errors == null || errors.Count == 0)
            {
                return noErrors;
            }

            StringBuilder sb = new StringBuilder();
            foreach(CompilerError error in errors)
            {
                FormatError(error, sb);
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }

        protected void FormatError(CompilerError error, StringBuilder sb)
        {
            sb.Append(String.Format("{0}. {1} {2} Line: {3}", new object[] { error.ErrorNumber,
                                                                               error.IsWarning ? "W" : "E", error.ErrorText, error.Line }));
        }
    }

    public class DuplicateNameException : ApplicationException
    {
        public DuplicateNameException(string name) : base(FormatError(name))
        {
        }

        public static string FormatError(string name)
        {
            return String.Format("There is already a web service named '{0}'", name);
        }
    }

    /// <summary> The wsdl service name could not be read or re-formatted </summary>
    public class InvalidWsdlNameException : ApplicationException
    {
        public InvalidWsdlNameException(string message) : base(message)
        {
        }
    }

    /// <summary> Unable to convert wsdl file to an assembly </summary>
    public class WsdlConvertException : WsdlGenerateAssemblyException
    {
        public const string expMessage = "Unable to convert the WSDL file to an assembly.";
        public WsdlConvertException(string errorPath)   : base(expMessage, errorPath ) { }
        public WsdlConvertException(string errorPath, CompileException e) : base(expMessage, errorPath, e)   { }
    }

    public class WsdlGenerateAssemblyException : ApplicationException
    {
        protected static string ExceptionWithFilePath(string msg, string errorPath) 
        { 
            return String.Format("{0} See {1} for more details.", msg, errorPath);
        }

        public WsdlGenerateAssemblyException (string msg, string errorPath) : 
            base(ExceptionWithFilePath(msg, errorPath)) { }

        public WsdlGenerateAssemblyException (string msg, string errorPath, CompileException e) : 
            base(ExceptionWithFilePath(msg, errorPath), e) { }
    }

    public class WsdlParseException : WsdlGenerateAssemblyException
    {
        public const string expMessage = "Unable to parse the WSDL assembly.";
        public WsdlParseException(string errorPath) : base(expMessage, errorPath) { }
    }

    public class NativeTypeAssembleException : WsdlGenerateAssemblyException
    {
        public const string expMessage = "Unable to assemble the native type assembly.";
        public NativeTypeAssembleException(string errorPath)                     : base(expMessage, errorPath)   { }
        public NativeTypeAssembleException(string errorPath, CompileException e) : base(expMessage, errorPath, e) { }
    }

    public class NativeActionAssembleException : WsdlGenerateAssemblyException
    {
        public const string expMessage = "Unable to assemble the native action assembly.";
        public NativeActionAssembleException(string errorPath)                     : base(expMessage, errorPath)     { }
        public NativeActionAssembleException(string errorPath, CompileException e) : base(expMessage, errorPath, e)  { }
    }
}

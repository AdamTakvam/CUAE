using System;
using System.Text;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;

namespace Metreos.Utilities
{
	/// <summary>
	/// Exceptions thrown by core components
	/// </summary>
	[Serializable]
	public class ConfigurationException : Exception
	{
        private const string DEFAULT_ERROR = "A configuration-related error has ocurred";

        public ConfigurationException() 
            : base(DEFAULT_ERROR) {}

        public ConfigurationException(string message) 
            : base(message) {}

        public ConfigurationException(string message, Exception inner) 
            : base(message, inner) {}

        public ConfigurationException(SerializationInfo info, StreamingContext context) 
            : base(info, context) {}
	}

    [Serializable]
    public class CallControlException : Exception
    {
        private const string DEFAULT_ERROR = "A call control related error has ocurred";

        public CallControlException() 
            : base(DEFAULT_ERROR) {}

        public CallControlException(string message) 
            : base(message) {}

        public CallControlException(string message, Exception inner) 
            : base(message, inner) {}

        public CallControlException(SerializationInfo info, StreamingContext context) 
            : base(info, context) {}
    }

    public abstract class Exceptions
    {       
        public static string FormatException(Exception exception)
        {
            if(exception == null)   return "N/A";

            StringBuilder errorMessage = new StringBuilder(System.Environment.NewLine);

            if(exception is MySqlException)
            {
                MySqlException mysqlException = exception as MySqlException;
                errorMessage.Append("A MySQL-specific error has occurred.");
                errorMessage.Append(System.Environment.NewLine);
                errorMessage.AppendFormat("MySQL error number '{0}', Is Fatal '{1}'",
                    mysqlException.Number, mysqlException.IsFatal);
            }
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append("Full information as follows:");
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append("Source:  "); 
            errorMessage.Append(exception.Source);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append("Message:  ");
            errorMessage.Append(exception.Message);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append("Stack Trace:  ");
            if(exception.TargetSite != null)
                errorMessage.Append(exception.TargetSite.Name);
            errorMessage.Append(System.Environment.NewLine);
            errorMessage.Append(exception.StackTrace);

            if(exception.InnerException != null)
            {
                errorMessage.Append(System.Environment.NewLine);
                errorMessage.Append(System.Environment.NewLine);
                errorMessage.Append(FormatException(exception.InnerException));
            }
         
            return errorMessage.ToString();
        }

        /// <summary>
        /// Compares two exceptions for effective equality
        /// </summary>
        /// <param name="e1">An exception to compare</param>
        /// <param name="e2">An exception to compare</param>
        /// <returns>True if exceptions are the same type and from the same source</returns>
        public static bool Compare(Exception e1, Exception e2)
        {
            if(e1 == null)
                return e2 == null;
            if(e2 == null)
                return e1 == null;

            if( e1.GetType() == e2.GetType() &&
                e1.Source == e2.Source &&
                e1.Message == e2.Message)
                return true;
 
            return false;
        }
    }
}

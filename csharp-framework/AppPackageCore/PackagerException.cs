using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using Metreos.Utilities;

namespace Metreos.AppArchiveCore
{
    [Serializable]
    public enum PackagerErrorType
    {
        InvalidCreatePackageOptions,
        InvalidExtractPackageOptions,
        MandatoryFileValidationFailed,
        ChecksumValidationFailed,
        ApplicationSourceFileError,
        ArchiveExtractionFailed,
        BuildApplicationFailed,
        UnableToResolveNativeDependencies
    }

    [Serializable]
    public class PackagerException :  ApplicationException, ISerializable
    {
        private PackagerErrorType errorType;
        private StringCollection errorMessages;

        public PackagerErrorType ErrorType
        {
            get { return errorType; }
        }

        public StringCollection ErrorMessages
        {
            get { return errorMessages; }
        }

        public PackagerException()
        {}

        public PackagerException(string message) : base(message)
        {}

        public PackagerException(string message, Exception inner) : base(message, inner)
        {}

        public PackagerException(string message, PackagerErrorType errorType, StringCollection errorMessages) : base(message)
        {
            this.errorType = errorType;
            this.errorMessages = errorMessages;
        }

        public PackagerException(PackagerErrorType errorType, StringCollection errorMessages) : base()
        {
            this.errorType = errorType;
            this.errorMessages = errorMessages;
        }

        public PackagerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            errorType = (PackagerErrorType)info.GetValue("ErrorType", typeof(PackagerErrorType));
            errorMessages = (StringCollection)info.GetValue("ErrorMessages", typeof(StringCollection));
        }

        #region ISerializable

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData (info, context);
            info.AddValue("ErrorType", errorType);
            info.AddValue("ErrorMessages", errorMessages);
        }

        #endregion
    }
}

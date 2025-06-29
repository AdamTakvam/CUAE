using System;
using System.Runtime.Serialization;

namespace Metreos.ProviderFramework
{
    /// <summary>
    /// Indicates an error condition with the CreateProvider method on the factory.
    /// </summary>
    
    [Serializable]
    public class CreateProviderException : System.ApplicationException
    {
        public const string DEFAULT_ERROR               = "CreateProvider failed: Unknown error";
        public const string NO_AVAILABLE_PROVIDERS      = "The assembly does not contain any providers.";
        public const string PROVIDER_TYPE_NOT_FOUND     = "Provider assembly loaded but specified provider type not found";
        public const string INITIALIZATION_FAILED       = "Could not initialize the provider";
        public const string PROVIDER_FILE_NOT_FOUND     = "Provider could not be created because the file was not found";
        public const string PROVIDER_FILE_LOAD_FAILED   = "Provider could not be created because the file could not be loaded";
        public const string PROVIDER_ASSEMBLY_BAD       = "Provider could not be created because the file contained a bad assembly image";
        
        public CreateProviderException() : base(DEFAULT_ERROR)
        {}

        public CreateProviderException(string message) : base(message)
        {}

        public CreateProviderException(string message, Exception inner) : base(message, inner)
        {}

        public CreateProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}

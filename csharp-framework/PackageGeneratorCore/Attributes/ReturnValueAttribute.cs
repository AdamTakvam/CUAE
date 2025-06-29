using System;

using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.PackageGeneratorCore.Attributes
{
    /// <summary>
    /// Attribute to be used to declare a result value (ie, "success").
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=true)]
    public class ReturnValueAttribute : System.Attribute
    {
        public enum SuccessFailure
        {
            Success,
            Failure
        }

        public enum TrueFalse
        {
            True,
            False
        }

        public enum YesNo
        {
            Yes,
            No
        }

		private Type enumType;
        private string description;

        public ReturnValueAttribute()
            : this(typeof(SuccessFailure), null) {}

        public ReturnValueAttribute(bool openSet, string description)
            : this(openSet ? null : typeof(SuccessFailure), description) {}

		public ReturnValueAttribute(Type enumType, string description)
		{
            if(enumType != null && enumType.IsEnum == false)
                throw new ArgumentException("Return value type is not an enumeration: " + enumType.Name);

            this.enumType = enumType;
            this.description = description;
		}

		public string[] Values { get { return enumType != null ? Enum.GetNames(enumType) : null; } }

        public string Description { get { return description; } }
    }
}

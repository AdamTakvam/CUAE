using System;

using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;

namespace Metreos.PackageGeneratorCore.Attributes
{
    public abstract class Defaults
    {
        public const useType ACTION_PARAM_USE = useType.required;
        public const bool ACTION_ALLOW_CUSTOM_PARAMS = false;
        public const bool ACTION_FINAL = false;
        public static Type ACTION_PARAM_TYPE = typeof(string);
        public static Type RESULT_DATA_TYPE = typeof(string);
        public const string RESULT_DATA_NAME = IApp.FIELD_RETURN_VALUE;
        public const bool EVENT_PARAM_GUARANTEED = true;
        public static Type EVENT_PARAM_TYPE = typeof(string);
    }
}

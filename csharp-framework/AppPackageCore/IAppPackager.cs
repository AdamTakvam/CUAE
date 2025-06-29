using System;

namespace Metreos.AppArchiveCore
{
    public abstract class IAppPackager
    {
        public const string INSTALLER_FILE_EXTENSION            = ".installer";
        public const string LOCALES_FILE_EXTENSION              = ".locale";
        public const string DEFAULT_APP_PACKAGE_EXTENSION       = ".mca";
        public const string DEFAULT_INSTALLER_FILENAME          = "INSTALLER.xml";
        public const string DEFAULT_LOCALES_FILENAME            = "LOCALES.xml";
        public const string DEFAULT_MANIFEST_FILENAME           = "MANIFEST.xml";
    }
}

using System;

using Metreos.Core;

namespace Metreos.AppArchiveGenerator
{
    public abstract class Parameters
    {
        public const string CREATE_PACKAGE                      = "c";
        public const string EXTRACT_PACKAGE                     = "x";
        public const string APP_XML_FILE                        = "app";
        public const string DB_CREATE_SCRIPT                    = "dbscript";
        public const string MEDIA_FILE							= "media";
        public const string INSTALLER_XML_FILE                  = "installer";
        public const string LOCALES_XML_FILE                    = "locale";
        public const string NATIVE_TYPE_SEARCH_DIR              = "ntdir";
        public const string NATIVE_ACTION_SEARCH_DIR            = "nadir";
        public const string APP_VERSION                         = "version";
        public const string PRINT_USAGE                         = "help";
        public const string RECURSIVE_DIRECTORY_SEARCH          = "r";
        public const string VERBOSE                             = "v";
        public const string FRAMEWORK_DIR                       = "fwdir";
        public const string OUTPUT_DIRECTORY                    = "outdir";
        public const string APP_DISPLAY_NAME                    = "name";
        public const string APP_DESCRIPTION                     = "desc";
        public const string APP_COMPANY                         = "company";
        public const string APP_AUTHOR                          = "author";
        public const string APP_COPYRIGHT                       = "copyright";
        public const string APP_TRADEMARK                       = "trademark";
        public const string DEBUG_MODE                          = "debug";

        public const string CREATE_PACKAGE_HELP                 = "-" + CREATE_PACKAGE;
        public const string EXTRACT_PACKAGE_HELP                = "-" + EXTRACT_PACKAGE;
        public const string APP_XML_FILE_HELP                   = "-" + APP_XML_FILE                + ":<file>";
        public const string DB_CREATE_SCRIPT_HELP               = "-" + DB_CREATE_SCRIPT            + ":<file>";
        public const string MEDIA_FILE_HELP			            = "-" + MEDIA_FILE			        + ":<file>";
        public const string INSTALLER_XML_FILE_HELP             = "-" + INSTALLER_XML_FILE          + ":<file>";
        public const string LOCALES_XML_FILE_HELP               = "-" + LOCALES_XML_FILE            + ":<file>";
        public const string NATIVE_TYPE_SEARCH_DIR_HELP         = "-" + NATIVE_TYPE_SEARCH_DIR      + ":<directory>";
        public const string NATIVE_ACTION_SEARCH_DIR_HELP       = "-" + NATIVE_ACTION_SEARCH_DIR    + ":<directory>";
        public const string APP_VERSION_HELP                    = "-" + APP_VERSION                 + ":<version>";
        public const string PRINT_USAGE_HELP                    = "-" + PRINT_USAGE;
        public const string RECURSIVE_DIRECTORY_SEARCH_HELP     = "-" + RECURSIVE_DIRECTORY_SEARCH;
        public const string VERBOSE_HELP                        = "-" + VERBOSE;
        public const string FRAMEWORK_DIR_HELP                  = "-" + FRAMEWORK_DIR               + ":<directory>";
        public const string OUTPUT_DIRECTORY_HELP               = "-" + OUTPUT_DIRECTORY            + ":<directory>";
        public const string APP_DISPLAY_NAME_HELP               = "-" + APP_DISPLAY_NAME            + ":<displayNameText>";
        public const string APP_DESCRIPTION_HELP                = "-" + APP_DESCRIPTION             + ":<descriptionText>";
        public const string APP_COMPANY_HELP                    = "-" + APP_COMPANY                 + ":<companyText>";
        public const string APP_AUTHOR_HELP                     = "-" + APP_AUTHOR                  + ":<authorText>";
        public const string APP_COPYRIGHT_HELP                  = "-" + APP_COPYRIGHT               + ":<copyrightText>";
        public const string APP_TRADEMARK_HELP                  = "-" + APP_TRADEMARK               + ":<trademarkText>";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MigrationCore
{
    public abstract class Configuration
    {
        public const string MIGRATION_DEFINITION_ASSEMBLY  = "migration-defs.dll";
        public const string DISPLAY_NO_EXCEPTION_KEY       = "DoNotDisplayException";

        // Database config names
        public const string DATABASE_HOST_CONFIG    = "DatabaseHostname";
        public const string DATABASE_PORT_CONFIG    = "DatabasePort";
        public const string DATABASE_NAME_CONFIG    = "DatabaseName";
        public const string DATABASE_USER_CONFIG    = "DatabaseUsername";
        public const string DATABASE_PASSWD_CONFIG  = "DatabasePassword";

        // Log file values
        public const string FILE_TIMESTAMP_FORMAT  = "yyyyMMdd-HHmmss";
        public const string FILE_LOG_EXTENSION     = "log";
    }
}

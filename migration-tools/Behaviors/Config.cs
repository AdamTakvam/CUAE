using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace BehaviorCore
{
    abstract public class Config
    {
        static public string BACKUP_VERSION         = "2.4.0";

        static public string PARENT_PATH            = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
        static public string MYSQL_BIN_PATH         = "C:\\Program Files\\MySQL\\MySQL Server 4.1\\bin";
        static public string MYSQLDUMP_PATH         = String.Concat(MYSQL_BIN_PATH, "\\mysqldump.exe");
        static public string MYSQL_EXE_PATH         = String.Concat(MYSQL_BIN_PATH, "\\mysql.exe");

        static public string METADATA_FILE_NAME     = "info.txt";

        static public string DEFAULT_MCE_PATH       = "C:\\Metreos";
        static public string DEFAULT_CUAE23_PATH    = "C:\\Program Files\\Metreos";
        static public string DEFAULT_CUAE_PATH      = Directory.GetParent(Directory.GetParent(PARENT_PATH).ToString()).ToString();

        static public string DEFAULT_DB_HOST        = "localhost";
        static public string DEFAULT_DB_PORT        = "3306";
        static public string DEFAULT_DB_USER        = "root";
        static public string DEFAULT_DB_PASSWORD    = "metreos";
        static public string DEFAULT_DB_NAME        = "mce";
    }
}

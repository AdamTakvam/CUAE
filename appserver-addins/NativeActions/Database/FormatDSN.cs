using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.Database.Actions.FormatDSN;

namespace Metreos.Native.Database
{
	/// <summary>
	/// Establishes a connection to the specified external database
	/// and adds it to the session data with the specified name
	/// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Database.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Database.Globals.PACKAGE_DESCRIPTION)]
    public class FormatDSN : INativeAction
	{
        private const ushort MYSQL_DEFAULT_PORT = 3306;

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.Type.DISPLAY, Package.Params.Type.DESCRIPTION, true, Package.Params.Type.DEFAULT)]
        public Metreos.Utilities.Database.DbType Type { set { type = value; } }
        private Metreos.Utilities.Database.DbType type;

		[ActionParamField(Package.Params.DatabaseName.DISPLAY, Package.Params.DatabaseName.DESCRIPTION, true, Package.Params.DatabaseName.DEFAULT)]
		public string DatabaseName { set { dbName = value; } }
		private string dbName;

		[ActionParamField(Package.Params.Server.DISPLAY, Package.Params.Server.DESCRIPTION, true, Package.Params.Server.DEFAULT)]
		public string Server { set { server = value; } }
		private string server;

        [ActionParamField(Package.Params.Port.DISPLAY, Package.Params.Port.DESCRIPTION, false, Package.Params.Port.DEFAULT)]
        public ushort Port { set { port = value; } }
        private ushort port;

        [ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, true, Package.Params.Username.DEFAULT)]
        public string Username { set { username = value; } }
        private string username;

        [ActionParamField(Package.Params.Password.DISPLAY, Package.Params.Password.DESCRIPTION, true, Package.Params.Password.DEFAULT)]
        public string Password { set { password = value; } }
        private string password;

        [ActionParamField(Package.Params.Pooling.DISPLAY, Package.Params.Pooling.DESCRIPTION, false, Package.Params.Pooling.DEFAULT)]
        public bool Pooling { set { pooling = value; } }
        private bool pooling;

        [ActionParamField(Package.Params.ConnectionTimeout.DISPLAY, Package.Params.ConnectionTimeout.DESCRIPTION, false, Package.Params.ConnectionTimeout.DEFAULT)]
        public uint ConnectionTimeout { set { connectionTimeout = value; } }
        private uint connectionTimeout;

        [ResultDataField(Package.Results.DSN.DISPLAY, Package.Results.DSN.DESCRIPTION)]
        public string DSN { get { return dsn; } }
        private string dsn;

		public FormatDSN() { Clear(); }

        public bool ValidateInput()
        {
            if((dbName == null) || (dbName.Length == 0)) { return false; }
            if((server == null) || (server.Length == 0)) { return false; }
            // Username and password isn't required for all databases, or configurations of databases (MSC)
//            if((username == null) || (username.Length == 0)) { return false; }
//            if((password == null) || (password.Length == 0)) { return false; }
            if(port == 0) { port = MYSQL_DEFAULT_PORT; }
            return true;
        }

        public void Clear()
        {
            connectionTimeout = 0;
            pooling = true;
            dbName = null;
            server = null;
            port = 0;
            username = null;
            password = null;
            dsn = String.Empty;
        }

		[Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            // This action only supports MySQL for the time being
//			switch(type)
//			{
//				case DbType.oracle:
//					dsn = "";
//					break;
//				case DbType.sqlserver:
//					dsn = "";
//					break;
//                case DbType.mysql:
                    dsn = Metreos.Utilities.Database.FormatDSN(dbName, server, port, username, password, pooling, connectionTimeout);
//                    break;
//				default:
//					dsn = "";
//					break;
//			}

			return IApp.VALUE_SUCCESS;
		}
	}
}

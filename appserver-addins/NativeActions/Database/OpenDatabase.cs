using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using DbType = Metreos.Utilities.Database.DbType;

using Package = Metreos.Interfaces.PackageDefinitions.Database.Actions.OpenDatabase;

namespace Metreos.Native.Database
{
	/// <summary>
	/// Establishes a connection to the specified external database
	/// and adds it to the session data with the specified name
	/// </summary>
	[PackageDecl(Metreos.Interfaces.PackageDefinitions.Database.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Database.Globals.PACKAGE_DESCRIPTION)]
	public class OpenDatabase : INativeAction
	{
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.DSN.DISPLAY, Package.Params.DSN.DESCRIPTION, true, Package.Params.DSN.DEFAULT)]
		public string DSN { set { dsn = value; } }
		private string dsn;

		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, true, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }
		private string name;

		[ActionParamField(Package.Params.Type.DISPLAY, Package.Params.Type.DESCRIPTION, true, Package.Params.Type.DEFAULT)]
		public DbType Type { set { type = value; } }
		private DbType type;

		public OpenDatabase() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
        }

		[Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            // First, check if the database already exists
            IDbConnection conn = sessionData.DbConnections[name];
            if(conn != null)
                return IApp.VALUE_SUCCESS;

            try
            {
                conn = Metreos.Utilities.Database.CreateConnection(type, dsn);
                // Toggle connection to make this action more useful up-front, rather than loading knowledge 
                conn.Open();
                conn.Close();
                sessionData.DbConnections.Add(name, conn);
                return IApp.VALUE_SUCCESS;            
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to create a connection to the database.  Exception is: " 
                    + Exceptions.FormatException(e));
                return IApp.VALUE_FAILURE;
            }
		}
	}
}

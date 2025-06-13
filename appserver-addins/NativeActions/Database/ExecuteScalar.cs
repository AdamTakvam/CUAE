using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.Database.Actions.ExecuteScalar;

namespace Metreos.Native.Database
{
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Database.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Database.Globals.PACKAGE_DESCRIPTION)]
	public class ExecuteScalar : INativeAction
	{
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ActionParamField(Package.Params.Query.DISPLAY, Package.Params.Query.DESCRIPTION, true, Package.Params.Query.DEFAULT)]
		public string Query { set { query = value; } }
		private string query;

		[ActionParamField(Package.Params.Name.DISPLAY, Package.Params.Name.DESCRIPTION, true, Package.Params.Name.DEFAULT)]
		public string Name { set { name = value; } }
		private string name;

		[ResultDataField(Package.Results.Scalar.DISPLAY, Package.Results.Scalar.DESCRIPTION)]
		public object Scalar { get { return scalar; } }
		private object scalar;

		public ExecuteScalar() {}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            name = null;
            query = null;
            scalar = new object();
        }

		[Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
			IDbConnection conn = sessionData.DbConnections[name];
            if(conn == null) 
            {
                log.Write(TraceLevel.Warning, "No such database: " + name);
                return IApp.VALUE_FAILURE;
            }

            try
            {
                Assertion.Check(
                    conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken, 
                    "The connection has already been opened");
                conn.Open();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to open DB connection: " + e.Message);
                return IApp.VALUE_FAILURE;
            }

            bool success = false;

            try
            {
                using(IDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    scalar = command.ExecuteScalar();
                }

                success = true;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Warning, "Query '{0}' on database '{1}' failed. Error: {2}",
                    query, name, e.Message);
            }
            finally
            {
                try
                {
                    conn.Close(); 
                }
                catch(Exception closeException)
                {
                    // Success is left alone, because the sql functionality could have at least finished
                    log.Write(TraceLevel.Error, "Failed to close DB connection: " + closeException.Message);
                }
            }

            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
		}
	}
}

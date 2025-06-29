using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Summary description for IServerLog.
	/// </summary>
	public abstract class IServerLog
	{
		public const  int Message_WriteRequest          = 1001;
		public const  int Message_DisposeRequest        = 1002;
		public const  int Message_RefreshRequest        = 1003;
		public const  int Message_IntroductionRequest   = 1004;
		public const  int Message_IntroductionResponse  = 1005;
		public const  int Message_WriteResponse         = 1006;
		public const  int Message_FlushRequest			= 1007;

		public const  ushort Default_Port               = 8400;
		public const  uint   Default_NumberLines        = 4000;
		public const  uint   Default_NumFiles			= 50;

		public const string CONFIG_DATABASENAME			= "DatabaseName";
		public const string CONFIG_DATABASEHOSTNAME		= "DatabaseHostname";
		public const string CONFIG_DATABASEPORT			= "DatabasePort";
		public const string CONFIG_DATABASEUSERNAME		= "DatabaseUsername";
		public const string CONFIG_DATABASEPASSWORD		= "DatabasePassword";

		public const string CONFIG_LOGROOTFOLDER		= "LogRootFolder";
		public const string CONFIG_PORTNUMBER			= "PortNumber";
		public const string CONFIG_MAXFILESPERCLIENT	= "MaxFilesPerClient";
		public const string CONFIG_MAXLINESPERFILE		= "MaxLinesPerFile";

		public abstract class DefaultValues
		{
			public const string DEFAULT_DATABASENAME         = "MCE";
			public const string DEFAULT_DATABASEHOSTNAME     = "localhost";
			public const ushort DEFAULT_DATABASEPORT         = 3306;
			public const string DEFAULT_DATABASEUSERNAME     = "root";
			public const string DEFAULT_DATABASEPASSWORD     = "metreos";
		}
	}
}

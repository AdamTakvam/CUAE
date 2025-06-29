using System;

namespace Metreos.Samoa.FunctionalTestRuntime
{
	/// <summary>
	/// Summary description for CommonTypes.
	/// </summary>
	public abstract class CommonTypes
	{
		public delegate bool StartTestDelegate ( string testName );

        public delegate void StopTestDelegate ();

        public delegate void OutputLine( string line );

        public delegate void StatusUpdate( string status );

        public delegate void AddTest( string testName, string testGroup );

        public delegate bool ConnectServer();

        public delegate void AddUser();
	}
}

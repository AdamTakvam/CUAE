using System;
using System.Collections;

using Metreos.Interfaces;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for CommonTypes.
	/// </summary>
	public abstract class CommonTypes
	{
		public delegate void StartTestDelegate ( string testName, Hashtable values );
		public delegate void TestEndedDelegate(
            string testName, 
            bool success, bool 
            ignoreSuccess);
		public delegate void StopTestDelegate( string testName );
        public delegate void TestAbortable();
		public delegate void OutputLine( string line );
		public delegate void InstructionLine( string line );
		public delegate void StatusUpdate( string status );
		public delegate void AddTest(
            int baseNameSpaceLength, 
            string fullTestName, 
            ArrayList inputData, 
            Hashtable configValues, 
            bool firstTime, 
            bool previousSuccess, 
            string description, 
            string instructions);                                   
		public delegate void LoadDone();
        public delegate bool ConnectServer(string username, string password);
        public delegate void ServerConnectionStatus( bool status );
        public delegate void AddUser();
        public delegate bool AsyncUserInputCallback( string key, string value_ );
        public delegate void ResetProgressMeter( int maximumTicks );
        public delegate void AdvanceProgressMeter( int advanceAmount );
        public delegate bool ReconnectToServer();
        public delegate bool PrepareServerBeforeTest();
        public delegate void InitializeGlobalSettings();
		public delegate void UpdateTestSettings(
            string testName, 
            string variableName, 
            string variableValue ); 
        public delegate bool UpdateConfigValue(
            string componentName, 
            IConfig.ComponentType componentType, 
            string configName, 
            object configValue, 
            string description, 
            IConfig.StandardFormat format);
        public delegate bool UpdateScriptParameter(
            string appName, 
            string scriptName, 
            string partitionName, 
            string paramName, 
            object Value);
        public delegate bool UpdateCallRouteGroup(
            string appName,
            Constants.CallRouteGroupTypes type);
        public delegate bool UpdateMediaRouteGroup(
            string appName,
            string mediaGroupName);
        public delegate void UnloadFramework();
	}
}

using System;
using System.Threading;
using Metreos.Samoa.FunctionalTestFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
	/// <summary>
	/// Summary description for IFunctionalTestConduit.
	/// </summary>
    public interface IFunctionalTestConduit
    {
        string TestSettingsFolder{ get; }

        Settings Settings{ get; set; }
	
        TestSettings TestSettings{ get; set; }

        CommonTypes.StartTestDelegate StartTest                         { get; set; }
        CommonTypes.ConnectServer ConnectServer                         { get; set; }
        CommonTypes.AddUser AddUser                                     { get; set; }
        CommonTypes.ReconnectToServer ReconnectToServer                 { get; set; }
        CommonTypes.PrepareServerBeforeTest PrepareServerBeforeTest     { get; set; }
        CommonTypes.InitializeGlobalSettings InitializeGlobalSettings   { get; set; }
        CommonTypes.StopTestDelegate AbortTest                          { get; set; }

        event CommonTypes.TestEndedDelegate TestEnded;
        event CommonTypes.TestAbortable TestNowAbortable;
        event CommonTypes.StatusUpdate StatusUpdate;
        event CommonTypes.OutputLine Output;
        event CommonTypes.InstructionLine InstructionLine;
        event CommonTypes.AddTest AddNewTest;
        event CommonTypes.LoadDone FrameworkLoaded;
        event CommonTypes.ServerConnectionStatus UpdateConnectedStatus;
        event CommonTypes.ResetProgressMeter ResetProgress;
        event CommonTypes.AdvanceProgressMeter AdvanceProgress;
    }
}

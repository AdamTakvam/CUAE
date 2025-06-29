using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MinutelyTimerTest = Metreos.TestBank.Provider.Provider.MinutelyTimer;

namespace Metreos.FunctionalTests.SingleProvider.Timer
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MinutelyTimer : FunctionalTestBase
    {
        public MinutelyTimer() : base(typeof( MinutelyTimer ))
        {

        }

        public override bool Execute()
        {
            updateProviderConfig(
                "TimerFacility", 
                IConfig.ComponentType.Provider,
                "TimerEventsEveryMinute",
                true, 
                "If true, minute by minute timer events will be generated", 
                IConfig.StandardFormat.Bool);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration("TimerFacility");

            for(int i = 0; i < 5; i++)
            {
                if( !WaitForSignal( MinutelyTimerTest.script1.S_Fired.FullName, 90 ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from within the timer callback.");
                    updateProviderConfig(
                        "TimerFacility", 
                        IConfig.ComponentType.Provider,
                        "TimerEventsEveryMinute", 
                        false, 
                        "If true, minute by minute timer events will be generated", 
                        IConfig.StandardFormat.Bool);

                    return false;
                }
            }

            TriggerScript( MinutelyTimerTest.script2.FullName );

            if( WaitForSignal( MinutelyTimerTest.script1.S_Fired.FullName, 90) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received a minutely timer event after the timer was removed.");
                updateProviderConfig(
                    "TimerFacility", 
                    IConfig.ComponentType.Provider,
                    "TimerEventsEveryMinute", 
                    false, 
                    "If true, minute by minute timer events will be generated", 
                    IConfig.StandardFormat.Bool);

                return false;
            }

            updateProviderConfig(
                "TimerFacility", 
                IConfig.ComponentType.Provider,
                "TimerEventsEveryMinute",
                false, 
                "If true, minute by minute timer events will be generated", 
                IConfig.StandardFormat.Bool);

            return true;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( MinutelyTimerTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( MinutelyTimerTest.script1.S_Fired.FullName , null)

            };
        }
    } 
}

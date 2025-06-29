using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using HighLoadSignallingTest = Metreos.TestBank.Core.Core.HighLoadSignalling;

namespace Metreos.FunctionalTests.Standard.Core.Signalling.Stress
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
	public class HighLoadSignalling : FunctionalTestBase
	{
        private const int loopCount = 100;
        private const int rigourousTimeout = 5;

		    public HighLoadSignalling() : base(typeof( HighLoadSignalling ))
        {
            this.timeout = 3000;
        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();

            fields["signalAmount"] = loopCount.ToString();

            TriggerScript( HighLoadSignallingTest.script1.FullName, fields);
            
            for(int i = 0; i < loopCount; i++)
            {
                if(!WaitForSignal( HighLoadSignallingTest.script1.S_Simple.FullName, rigourousTimeout ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal in the rigourous timeout period of " + rigourousTimeout);
                    log.Write(System.Diagnostics.TraceLevel.Info, "Received " + i.ToString() + " out of " + loopCount.ToString() + " signals.");
                    return false;
                }
            }

            return true;
        }

        public override void Initialize()
        {
        }

        public override void Cleanup()
        {
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( HighLoadSignallingTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( HighLoadSignallingTest.script1.S_Simple.FullName , null )};
        }
	} 
}

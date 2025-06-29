using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MediaFileProvisioningMultipleTest = Metreos.TestBank.ARE.ARE.MediaFileProvisioningMultiple;


namespace Metreos.FunctionalTests.Standard.ARE.ApplicationMobility
{
	/// <summary>  Checks that a media files are sent to media server share directories </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MediaFileProvisioningMultiple : FunctionalTestBase
	{
		public MediaFileProvisioningMultiple() : base(typeof( MediaFileProvisioningMultiple ))
        {
            this.Instructions = "The test will install an application with 3 media file references:  beep.wav, call_end.wav, champions.wav.  Watch that the Media Server audio directory has these files after installation.";
        }

        public override bool Execute()
        {
            TriggerScript( MediaFileProvisioningMultipleTest.script1.FullName );
            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( MediaFileProvisioningMultipleTest.FullName ) };
        }
	} 
}

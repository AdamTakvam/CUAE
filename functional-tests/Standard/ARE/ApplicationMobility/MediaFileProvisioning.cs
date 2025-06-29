using System;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MediaFileProvisioningTest = Metreos.TestBank.ARE.ARE.MediaFileProvisioning;


namespace Metreos.FunctionalTests.Standard.ARE.ApplicationMobility
{
	/// <summary>  Checks that a media files are sent to media server share directories </summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MediaFileProvisioning : FunctionalTestBase
	{
		public MediaFileProvisioning() : base(typeof( MediaFileProvisioning ))
        {
            this.Instructions = "The test will install an application with a media file reference, beep.wav.  Watch that the Media Server audio directory has this file after installation.";
        }

        public override bool Execute()
        {
            TriggerScript( MediaFileProvisioningTest.script1.FullName );
            return true;
        }
    
        public override string[] GetRequiredTests()
        {
            return new string[] { ( MediaFileProvisioningTest.FullName ) };
        }
	} 
}

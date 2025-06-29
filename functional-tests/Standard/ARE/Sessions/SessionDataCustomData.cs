using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SessionDataCustomDataTest = Metreos.TestBank.ARE.ARE.SessionDataCustomData;


namespace Metreos.FunctionalTests.Standard.ARE.Sessions
{
	/// <summary>Ensures that a session can support the addition and usage of custom data</summary>
	[Exclusive(IsExclusive=true)]
	[FunctionalTestImpl(IsAutomated=true)]
	public class SessionDataCustomData : FunctionalTestBase
	{
        private bool success;
        private int state;

		public SessionDataCustomData() : base(typeof( SessionDataCustomData ))
        {

        }

        public override void Initialize()
        {
            success = true;
            state = 0;
        }

        public override bool Execute()
        {
            TriggerScript( SessionDataCustomDataTest.master1.FullName );
        
            if(!WaitForSignal( SessionDataCustomDataTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the int value signal.");
                return false;
            }

            if(!WaitForSignal( SessionDataCustomDataTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the literal string value signal.");
                return false;
            }

            if(!WaitForSignal( SessionDataCustomDataTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the metreos string value signal.");
                return false;
            }

            if(!WaitForSignal( SessionDataCustomDataTest.master1.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a signal from after the hashtable value signal.");
                return false;
            }
            return success;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SessionDataCustomDataTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( SessionDataCustomDataTest.master1.S_Simple.FullName, new FunctionalTestSignalDelegate(CheckValue)) };   
        }

        public void CheckValue(ActionMessage im)
        {
            state++;
            switch(state)
            {
                case 1:
                    int intValue = (int) im["intValue"];
                    
                    log.Write(System.Diagnostics.TraceLevel.Info, "Expected 5.  Received " + intValue);

                    if(intValue != 5)
                    {
                        success = false;
                    }

                    break;

                case 2:

                    string literalValue = im["stringValue"] as string;

                    log.Write(System.Diagnostics.TraceLevel.Info, "Expected \"literalStringValue\".  Received \"" + literalValue + "\".");

                    if(literalValue != "literalStringValue")
                    {
                        success = false;
                    }

                    break;

                case 3:

                    string metreosStringValue = im["metreosStringValue"] as string;

                    log.Write(System.Diagnostics.TraceLevel.Info, "Expected \"specificValue\".  Received \"" + metreosStringValue + "\".");

                    if(metreosStringValue != "specificValue")
                    {
                        success = false;
                    }

                    break;

                case 4:

                    Hashtable hashtableValue = im["hashtableValue"] as Hashtable;

                    if(hashtableValue.Count == 1)
                    {
                        if(hashtableValue.Contains("testKey"))
                        {
                            string testValue = hashtableValue["testKey"] as string;

                            if(testValue == "testValue")
                            {
                                log.Write(System.Diagnostics.TraceLevel.Info, "Hashtable was of the right size, and had the right key-value pair.");
                            }
                            else
                            {
                                log.Write(System.Diagnostics.TraceLevel.Info, "Hashtable was of the right size, but did not have the right value for the key.  Value was: " + testValue);
                                success = false;
                            }
                        }
                        else
                        {
                            log.Write(System.Diagnostics.TraceLevel.Info, "Hashtable did not contain \"testKey\" as a key");
                            success = false;
                        }
                    }
                    else
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "Hashtable had an incorrect count. Count was: " + hashtableValue.Count);
                        success = false;
                    }
                    break;
            }
        }
	} 
}

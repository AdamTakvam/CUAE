using System;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using CompConfigTypesTest = Metreos.TestBank.ARE.ARE.CompConfigTypes;

namespace Metreos.FunctionalTests.Standard.ARE.Config
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0901")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0902")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0903")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0904")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0905")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0906")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0907")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0908")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0909")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0910")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0911")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0912")]
    [QaTest(Id="TESTCASE-WEBCONSOLE-0913")]
    public class CompConfigTypes : FunctionalTestBase
    {
        public CompConfigTypes() : base(typeof( CompConfigTypes ))
        {

        }

        public override bool Execute()
        {
            log.Write(TraceLevel.Info, "App should be installed. Go to mceadmin and verify the various config types.");
            return true;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( CompConfigTypesTest.FullName ) };
        }
    }
}

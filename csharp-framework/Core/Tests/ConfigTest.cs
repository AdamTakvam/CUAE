using System;

using Metreos.Samoa;
using Metreos.Samoa.Core;
using Metreos.Samoa.RemotableInterfaces;

namespace Metreos.Samoa.Core.Tests
{
    public class ConfigTest
    {
        public ConfigTest()
        {
        }

        public void testGetQueueId()
        {
            string appServerName = IConfig.CoreComponentNames.APP_SERVER;
            csUnit.Assert.NotNull(appServerName);
        }
    }
}

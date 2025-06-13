using System;

namespace Metreos.Types.CiscoIpPhone.Tests
{
    [csUnit.TestFixture]
    public class ExecuteTest
    {
        public ExecuteTest()
        {}

        [csUnit.Test]
        public void testToString()
        {
            string expected="XML=%3CCiscoIPPhoneExecute%3E%0D%0A%20%20%3CExecuteItem%20URL%3D%22http:%2F%2Fwww.metreos.com%2FApplication%22%20%2F%3E%0D%0A%3C%2FCiscoIPPhoneExecute%3E";

            Execute e = new Execute();

            e.type = ICiscoPhone.EXECUTE;
            
            CiscoIPPhoneExecuteItemType item = new CiscoIPPhoneExecuteItemType();
            item.URL = "http://www.metreos.com/Application";

            e.menu.ExecuteItem = new CiscoIPPhoneExecuteItemType[1];
            e.menu.ExecuteItem[0] = item;

            csUnit.Assert.Equals(expected, e.ToString());
        }
    }
}

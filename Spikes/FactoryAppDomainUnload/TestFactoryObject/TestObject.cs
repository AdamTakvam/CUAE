using System;

namespace Spike.TestFactoryObject
{
	public class TestObject : MarshalByRefObject, Spike.AppDomainFactory.IFactoryObject
	{
		public TestObject()
		{
		}

        public void DoSomething()
        {
            Console.WriteLine("TestFactoryObject inside of domain: {0}", AppDomain.CurrentDomain.FriendlyName);
        }
	}
}

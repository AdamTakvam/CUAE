using System;

namespace Spike.AppDomainFactory
{   
    public interface IFactoryObject
    {
        void DoSomething();
    }

	public class Factory : MarshalByRefObject
	{
		public Factory()
		{
		}

        public IFactoryObject LoadFactoryObject(string assembly, string className)
        {
            IFactoryObject io = (IFactoryObject)Activator.CreateInstanceFrom(assembly, className).Unwrap();

            return io;
        }                                         
	}
}

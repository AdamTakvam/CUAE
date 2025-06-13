using System;

namespace Spike.Runtime
{
	class MainRuntime
	{
		[STAThread]
		static void Main(string[] args)
		{

            AppDomainSetup domainSetup = new AppDomainSetup();
            domainSetup.ShadowCopyFiles = "true";

            try
            {
                System.IO.File.Copy("TestFactoryObject.dll", "temp.dll");
            }
            catch(System.UnauthorizedAccessException)
            {
                Console.WriteLine("Couldn't copy the file, something is wrong");
            }

            AppDomain domain = AppDomain.CreateDomain("TestDomain", null, domainSetup);

            AppDomainFactory.Factory factory = (AppDomainFactory.Factory)domain.CreateInstanceAndUnwrap(
                                                                                    "AppDomainFactory",
                                                                                    "Spike.AppDomainFactory.Factory");

            AppDomainFactory.IFactoryObject factoryObject = factory.LoadFactoryObject(
                                                                "temp.dll",
                                                                "Spike.TestFactoryObject.TestObject");

            factoryObject.DoSomething();

            try
            {
                System.IO.File.Delete("temp.dll");
                System.IO.File.Copy("TestFactoryObject2.dll", "temp.dll");
            }
            catch(System.UnauthorizedAccessException)
            {
                Console.WriteLine("Couldn't delete/copy the file, something is wrong");
            }

            factoryObject = null;
            factory = null;

            AppDomain.Unload(domain);

            domain = AppDomain.CreateDomain("TestDomain");

            factory = (AppDomainFactory.Factory)domain.CreateInstanceAndUnwrap(
                                                        "AppDomainFactory",
                                                        "Spike.AppDomainFactory.Factory");

            AppDomainFactory.IFactoryObject factoryObject2 = factory.LoadFactoryObject(
                                                                "temp.dll",
                                                                "Spike.TestFactoryObject.TestObject");

            factoryObject2.DoSomething();

            factoryObject2 = null;
            factory = null;

            AppDomain.Unload(domain);

            try
            {
                System.IO.File.Delete("temp.dll");
            }
            catch(System.UnauthorizedAccessException)
            {
                Console.WriteLine("Couldn't delete the file, something is wrong");
            }
		}
	}
}

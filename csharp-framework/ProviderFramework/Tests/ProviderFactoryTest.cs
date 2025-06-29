using System;
using System.Diagnostics;

using Metreos.Samoa.ProviderFramework;

namespace Metreos.Samoa.ProviderFramework.Tests
{
    #region Mock Objects

    [ProviderDecl("Mock implementation")]
    public class ProviderImplMock : ProviderBase
    {
        public static bool intializeReturn = false;

        public ProviderImplMock() : base("ProviderImplMock", "SomeNamespace", TraceLevel.Info)
        {}
        
        public override bool Initialize()
        {
            return ProviderImplMock.intializeReturn;
        }

		protected override void RefreshConfiguration()
		{}
    }

    #endregion 

    public class ProviderFactoryTest
    {
        private IProvider provider;
        private ProviderFactory factory;

        public ProviderFactoryTest()
        {}

        [csUnit.FixtureSetUp]
        public void FixtureSetUp()
        {
            factory = new ProviderFactory();
            factory.PalQName = "ProviderFactoryTest";
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            if(provider != null)
            {
                provider.Cleanup();
            }
            
            factory = null;
        }

        [csUnit.Test]
        public void testCreateProviderGood()
        {
            bool caughtException = false;

            ProviderImplMock.intializeReturn = true;

            try
            {
                provider = factory.CreateProvider(System.Reflection.Assembly.GetExecutingAssembly().Location,
                                                    "Metreos.Samoa.ProviderFramework.Tests.ProviderImplMock");
            }
            catch(ProviderFramework.CreateProviderException)
            {
                caughtException = true;
            }

            csUnit.Assert.False(caughtException);
            csUnit.Assert.NotNull(provider);

            provider.Cleanup();
            provider = null;
        }

        [csUnit.Test]
        public void testCreateProviderBad()
        {
            bool caughtException = false;

            ProviderImplMock.intializeReturn = false;

            try
            {
                provider = factory.CreateProvider(System.Reflection.Assembly.GetExecutingAssembly().Location,
                                                    "Metreos.Samoa.ProviderFramework.Tests.ProviderImplMock");
            }
            catch(ProviderFramework.CreateProviderException)
            {
                caughtException = true;
            }

            csUnit.Assert.True(caughtException);
            csUnit.Assert.Null(provider);
        }

        [csUnit.Test]
        public void testCreateProviderBadAssemblyName()
        {
            bool caughtException = false;

            try
            {
                provider = factory.CreateProvider("ShouldNotWork");
            }
            catch(ProviderFramework.CreateProviderException)
            {
                caughtException = true;
            }

            csUnit.Assert.True(caughtException);
            csUnit.Assert.Null(provider);
        }

        [csUnit.Test]
        public void testCreateProviderNoProviderImpl()
        {
            string location = "";

            bool caughtException = false;

            foreach(System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if(a.FullName != System.Reflection.Assembly.GetExecutingAssembly().FullName)
                {
                    location = a.Location;
                    break;
                }
            }

            try
            {
                provider = factory.CreateProvider(location, "Metreos.Samoa.ProviderFramework.Tests.ProviderImplMock");
            }
            catch(ProviderFramework.CreateProviderException)
            {
                caughtException = true;
            }

            csUnit.Assert.NotEquals(location, "");
            csUnit.Assert.Null(provider);
            csUnit.Assert.True(caughtException);

            location = null;
        }

        [csUnit.Test]
        public void testCreateProviderNoTypeName()
        {
            bool caughtException = false;

            ProviderImplMock.intializeReturn = true;

            try
            {
                provider = factory.CreateProvider(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch(ProviderFramework.CreateProviderException)
            {
                caughtException = true;
            }

            csUnit.Assert.False(caughtException);
            csUnit.Assert.NotNull(provider);

            provider.Cleanup();
            provider = null;
        }
    }
}

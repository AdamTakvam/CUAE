using System;
using System.Diagnostics;

namespace Metreos.Samoa.ProviderFramework.Tests
{
    #region Mock Objects

    [ProviderDecl("Mock implementation")]
    public class MockProviderForProviderBase : ProviderBase
    {
        public static System.Threading.ManualResetEvent handleMessageCallbackCalled;

        public static MockProviderForProviderBase instance;

        public MockProviderForProviderBase() 
			: base("MockProviderForProviderBase", "Test.MockProviderForProviderBase", TraceLevel.Info)
        {
            MockProviderForProviderBase.handleMessageCallbackCalled = new System.Threading.ManualResetEvent(false);

            instance = this;
        }
        
        public override bool Initialize()
        {
            this.messageCallbacks.Add("Test.MockProviderForProviderBase.SomeMessage", 
                                        new ProviderFramework.HandleMessageDelegate(this.HandleMessageCallback));

            return true;
        }

		protected override void RefreshConfiguration()
		{}

        public void HandleMessageCallback(Core.InternalMessage im)
        {
            MockProviderForProviderBase.handleMessageCallbackCalled.Set();
        }
    }

    #endregion

    [csUnit.TestFixture]
    public class ProviderBaseTest
    {
        private IProvider provider;
        private ProviderFactory factory;

        public ProviderBaseTest()
        {}

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            if(provider != null)
            {
                provider.Cleanup();
            }

            provider = null;
            factory = null;
        }

        [csUnit.Test]
        public void testProviderHandleMessage()
        {
            Core.MessageQueueWriter writer = new Core.MessageQueueWriter(new Core.MsmqMessageQueueProvider("MockProviderForProviderBase"));

            factory = new ProviderFactory();
            factory.PalQName = "testProviderHandleMessage";

            provider = factory.CreateProvider(System.Reflection.Assembly.GetExecutingAssembly().Location, 
                "Metreos.Samoa.ProviderFramework.Tests.MockProviderForProviderBase");

            bool callbackFired = false;

            Core.InternalMessage im = new Core.InternalMessage();
            im.MessageId = "Test.MockProviderForProviderBase.SomeMessage";

            MockProviderForProviderBase.instance.PostMessage(im);

            callbackFired = MockProviderForProviderBase.handleMessageCallbackCalled.WaitOne(500, false);

            csUnit.Assert.True(callbackFired);

            writer.Cleanup();

            writer = null;

            factory = null;
        }

        [csUnit.Test]
        public void testProviderNameStartsWithProviderNameTag()
        {
            csUnit.Assert.True(MockProviderForProviderBase.instance.Name.StartsWith(ProviderBase.PROVIDER_NAME_TAG));
        }

        [csUnit.Test]
        public void testGetName()
        {
            csUnit.Assert.Equals(MockProviderForProviderBase.instance.Name, provider.GetName());
        }

        [csUnit.Test]
        public void testGetTaskStatus()
        {
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN, provider.GetTaskStatus());
        }
    }
}
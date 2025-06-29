using System;

using Metreos.Samoa.Core;
using Metreos.Samoa.CommonRuntime;

namespace Metreos.Samoa.CommonRuntime.Tests
{
    public class ApplicationServerTest
    {
        private CommonRuntime.ApplicationServer appServer;

        private System.Threading.ManualResetEvent startupCompleteCallbackFired;
        private System.Threading.ManualResetEvent shutdownCompleteCallbackFired;

        public System.Collections.Specialized.StringDictionary startupProgressMessages;
        public System.Collections.Specialized.StringDictionary shutdownProgressMessages;

        public ApplicationServerTest()
        {
            appServer = CommonRuntime.ApplicationServer.Instance;

            startupCompleteCallbackFired = new System.Threading.ManualResetEvent(false);
            shutdownCompleteCallbackFired = new System.Threading.ManualResetEvent(false);

            appServer.startupComplete += new CommonRuntime.StartupCompleteDelegate(this.StartupCompleteCallback);
            appServer.shutdownComplete += new CommonRuntime.ShutdownCompleteDelegate(this.ShutdownCompleteCallback);

            appServer.startupProgress += new CommonRuntime.StartupProgressDelegate(this.StartupProgressCallback);
            appServer.shutdownProgress += new CommonRuntime.ShutdownProgressDelegate(this.ShutdownProgressCallback);

            startupProgressMessages = new System.Collections.Specialized.StringDictionary();
            shutdownProgressMessages = new System.Collections.Specialized.StringDictionary();
        }

        public void StartupCompleteCallback()
        {
            startupCompleteCallbackFired.Set();
        }

        public void ShutdownCompleteCallback()
        {
            shutdownCompleteCallbackFired.Set();
        }

        public void StartupProgressCallback(string progressMessage)
        {
            startupProgressMessages.Add(progressMessage, progressMessage);
        }

        public void ShutdownProgressCallback(string progressMessage)
        {
            shutdownProgressMessages.Add(progressMessage, progressMessage);
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            if(appServer.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(appServer.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    appServer.BeginShutdown();
                
                    System.Threading.Thread.Sleep(10000);
                }
            }

            appServer.Cleanup();

            startupProgressMessages.Clear();
            shutdownProgressMessages.Clear();

            startupProgressMessages = null;
            shutdownProgressMessages = null;

            appServer = null;
            startupCompleteCallbackFired = null;
            shutdownCompleteCallbackFired = null;
        }

        public void testStartup()
        {
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN, appServer.TaskStatus);

            bool callbackFired = false;
            
            appServer.BeginStartup();

            callbackFired = startupCompleteCallbackFired.WaitOne(10000, false);

            csUnit.Assert.Equals(3, this.startupProgressMessages.Count);

            csUnit.Assert.True(callbackFired);            
        }

        public void testShutdown()
        {
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.STARTED, appServer.TaskStatus);

            bool callbackFired = false;

            appServer.BeginShutdown();

            callbackFired = shutdownCompleteCallbackFired.WaitOne(10000, false);

            csUnit.Assert.Equals(3, this.shutdownProgressMessages.Count);

            csUnit.Assert.True(callbackFired);
        }
    }
}

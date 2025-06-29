using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

namespace Metreos.FunctionalTests.Standard.ARE.Deployment
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class SerialDeployment : FunctionalTestBase
    {
        private const string AppNameName = "defaultTestName";
        private const string DefaultAppName = "Max\\Label1";

        private const string TestEndButton = "testEndButton";

        private const string DefaultTestTimeName = "defaultTestTime";
        private const int DefaultTestTime = 60 * 60 * 1000; // 1 hour in milliseconds

        public int successCount;
        public int failureCount;

        private static uint randomCounter = 0;

        private Timer timer;
        private string appName;

        private volatile bool testOver;

        public override void Initialize()
        {
            testOver = true;
            successCount = 0;
            failureCount = 0;
        }

        public override void Cleanup()
        {
            testOver = true;
            successCount = 0;
            failureCount = 0;

            if(timer != null)
                timer.Dispose();
        }

        public SerialDeployment() : base(typeof( SerialDeployment ))
        {
            testOver = true;
            successCount = 0;
            failureCount = 0; 
        }

        public override bool Execute()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);

            this.appName = Convert.ToString(base.Input[AppNameName]);
            string appPath = CreateTestPackagePath(appName);
            FileInfo fi = new FileInfo(appPath);

            if(!fi.Exists)
            {
                log.Write(TraceLevel.Error, "Cannot find specified application");
                return false;
            }

            base.timeout = Convert.ToInt32(base.Input[DefaultTestTimeName]);
            timer = new Timer(new TimerCallback(TimeUp), null, timeout, System.Threading.Timeout.Infinite);
            testOver = false;

            while(!testOver)
            {
                if(deploy.Deploy(
                    fi.Name,
                    fi,
                    settings.AppServerIps[0],
                    settings.Username,
                    Metreos.Utilities.Security.EncryptPassword(settings.Password),
                    Convert.ToInt32(settings.SamoaPort),
                    22,
                    0))
                {
                    successCount++;
                    log.Write(TraceLevel.Info, String.Format("{0} deployed. Successes {1}", appName, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", appName, failureCount));
                }
            }

            log.Write(TraceLevel.Info, String.Format("Successes: {0}  Failures: {1}", successCount, failureCount));

            return failureCount == 0;
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            
            TestTextInputData testLength = new TestTextInputData(
                "Test length (ms)", 
                "Amount of time for this test to run, in milliseconds", 
                DefaultTestTimeName, 
                DefaultTestTime.ToString(), 
                40);

            TestTextInputData appNameInput = new TestTextInputData(
                "Application Name", 
                "Name of application to deploy", 
                AppNameName, 
                DefaultAppName,
                150);

            TestUserEvent stop = new TestUserEvent(
                "End test", 
                "Push to end test", 
                TestEndButton, 
                "Stop", 
                new CommonTypes.AsyncUserInputCallback(UserEnd));

            inputs.Add(testLength);
            inputs.Add(appNameInput);
            inputs.Add(stop);

            return inputs;        
        }

        private bool UserEnd(string name, string @value)
        {
            testOver = true;
            return true;
        }

        private string CreateTestPackagePath(string relTestPath)
        {
            string basePath = settings.CompiledMaxTestsDir;
            string ext = ".mca";
            relTestPath = relTestPath.Replace(".", "\\");
            string fullPathWithoutExt = Path.Combine(basePath, relTestPath);
            return Path.ChangeExtension(fullPathWithoutExt, ext);
        }

        private void TimeUp(object state)
        {
            testOver = true;
        }

        private void deploy_ErrorMessage(string message)
        {
            log.Write(TraceLevel.Info, message);
        }

        private void deploy_LogOutput(string message)
        {
            log.Write(TraceLevel.Info, message);
        }

        private Metreos.Core.AppDeploy.DeployOption deploy_UninstallPrompt()
        {
            if(randomCounter % 2 == 0)
            {
                return Metreos.Core.AppDeploy.DeployOption.Uninstall;
            }
            else
            {
                return Metreos.Core.AppDeploy.DeployOption.Update;
            }
        }
    } 
}

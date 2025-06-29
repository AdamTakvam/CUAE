using System;
using System.IO;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using Test1 = Metreos.TestBank.Max.Max.Label1;
using Test2 = Metreos.TestBank.Max.Max.Label2;
using Test3 = Metreos.TestBank.Max.Max.Label3;
using Test4 = Metreos.TestBank.Max.Max.Label4;
using Test5 = Metreos.TestBank.Max.Max.Label5;
using Test6 = Metreos.TestBank.Max.Max.Label6;
using Test7 = Metreos.TestBank.Max.Max.Label7;

namespace Metreos.FunctionalTests.Standard.ARE.Deployment
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=true)]
    public class ConcurrentDeployment : FunctionalTestBase
    {
        public int successCount;
        public int failureCount;

        public const string testEndButton = "testEndButton";
        public const string defaultTestTimeName = "defaultTestTime";
        public const int defaultTestTime = 60 * 60 * 1000; // 1 hour in milliseconds

        private static uint randomCounter = 0;
        private AutoResetEvent are1;
        private AutoResetEvent are2;
        private AutoResetEvent are3;
        private AutoResetEvent are4;
        private AutoResetEvent are5;
        private AutoResetEvent are6;
        private AutoResetEvent are7;

        private System.Timers.Timer timer;

        private bool testOver;

        public override void Initialize()
        {
            testOver = true;;
            successCount = 0;
            failureCount = 0;
        }

        public override void Cleanup()
        {
            testOver = true;
            successCount = 0;
            failureCount = 0;
        }

        public ConcurrentDeployment() : base(typeof( ConcurrentDeployment ))
        {
            testOver = true;
            successCount = 0;
            failureCount = 0;

            are1 = new AutoResetEvent(false);
            are2 = new AutoResetEvent(false);
            are3 = new AutoResetEvent(false);
            are4 = new AutoResetEvent(false);
            are5 = new AutoResetEvent(false);
            are6 = new AutoResetEvent(false);
            are7 = new AutoResetEvent(false);

            this.timeout = defaultTestTime;
            timer = new System.Timers.Timer(defaultTestTime);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimeUp);
        }

        public override bool Execute()
        {
            string testTime = input[defaultTestTimeName] as String;
            int testTimeInt = int.Parse(testTime);
            timer.Interval = testTimeInt;
            
            Thread thread1 = new Thread(new ThreadStart(Run1));
            Thread thread2 = new Thread(new ThreadStart(Run2));
            Thread thread3 = new Thread(new ThreadStart(Run3));
            Thread thread4 = new Thread(new ThreadStart(Run4));
            Thread thread5 = new Thread(new ThreadStart(Run5));
            Thread thread6 = new Thread(new ThreadStart(Run6));
            Thread thread7 = new Thread(new ThreadStart(Run7));
            thread1.IsBackground = true;
            thread2.IsBackground = true;
            thread3.IsBackground = true;
            thread4.IsBackground = true;
            thread5.IsBackground = true;
            thread6.IsBackground = true;
            thread7.IsBackground = true;

            testOver = false;
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
            thread6.Start();
            thread7.Start();

            timer.Start();

            are1.WaitOne();
            are2.WaitOne();
            are3.WaitOne();
            are4.WaitOne();
            are5.WaitOne();
            are6.WaitOne();
            are7.WaitOne();

            log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Successes: {0}  Failures: {1}", successCount, failureCount));

            return failureCount == 0;
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            
            TestTextInputData testLength = new TestTextInputData(
                "Test length (ms)", 
                "Amount of time for this test to run, in milliseconds", 
                defaultTestTimeName, 
                defaultTestTime.ToString(), 
                40);

            TestUserEvent stop = new TestUserEvent(
                "End test", 
                "Push to end test", 
                testEndButton, 
                "Stop", 
                new CommonTypes.AsyncUserInputCallback(UserEnd));

            inputs.Add(testLength);
            inputs.Add(stop);

            return inputs;        
        }

        private bool UserEnd(string name, string @value)
        {
            testOver = true;
            return true;
        }

        private void Run1()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test1.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test1.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test1.Name, failureCount));
                }
            }

            are1.Set();
        }

        private void Run2()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test2.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test2.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test2.Name, failureCount));
                }
            }

            are2.Set();
        }

        private void Run3()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test3.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test3.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test3.Name, failureCount));
                }
            }

            are3.Set();
        }

        private void Run4()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test4.FullName);

                FileInfo fi = new FileInfo(testPath);
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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test4.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test4.Name, failureCount));
                }
            }

            are4.Set();
        }

        private void Run5()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test5.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test5.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test5.Name, failureCount));
                }
            }

            are5.Set();
        }

        private void Run6()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test6.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test6.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test6.Name, failureCount));
                }
            }

            are6.Set();
        }

        private void Run7()
        {
            AppDeploy deploy = new AppDeploy();
            deploy.UninstallPrompt += new Uninstall(deploy_UninstallPrompt);
            deploy.ErrorMessage += new Message(deploy_ErrorMessage);
            deploy.LogOutput += new Message(deploy_LogOutput);
            
            while(!testOver)
            {
                string testPath = CreateTestPackagePath(Test7.FullName);

                FileInfo fi = new FileInfo(testPath);

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
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} deployed. Successes {1}", Test7.Name, successCount));
                }
                else
                {
                    failureCount++;
                    log.Write(System.Diagnostics.TraceLevel.Info, String.Format("{0} failed to deploy. Failures {1}", Test7.Name, failureCount));
                }
            }

            are7.Set();
        }

        private string CreateTestPackagePath(string relTestPath)
        {
            string basePath = settings.CompiledMaxTestsDir;
            string ext = ".mca";
            relTestPath = relTestPath.Replace(".", "\\");
            string fullPathWithoutExt = Path.Combine(basePath, relTestPath);
            return Path.ChangeExtension(fullPathWithoutExt, ext);
        }

        private void TimeUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            testOver = true;
            Thread.Sleep(10000); // Deploying apps need to finish, so their threads can exit
        }

        private void deploy_ErrorMessage(string message)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, message);
        }

        private void deploy_LogOutput(string message)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, message);
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

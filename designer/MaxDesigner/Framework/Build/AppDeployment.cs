using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.AppArchiveCore;
using Metreos.Interfaces;
using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Xml;
using Metreos.Core.IPC.Sftp;
using Metreos.Core.ConfigData;
using DeployOption = Metreos.Core.AppDeploy.DeployOption;
using Metreos.Max.Core;
using Metreos.Max.GlobalEvents;

namespace Metreos.Max.Framework
{
    public delegate void MessageWriterCallback(string message);

    /// <summary> 
    ///     Handles the asynchronous deployment of an application to an application server.
    ///     Uses the SFTP deployment code found in the metreos framework, and wraps it such
    ///     that the output of the utility deployment method makes its way to the output window 
    ///     of the Designer.
    /// </summary>
    public class AppDeployment : IDisposable
    {
        public  static AppDeployment Instance { get { return instance; } }
        private static AppDeployment instance = new AppDeployment();
        public  static bool Initialized { get { return initialized; } }
        private static bool initialized = false;
        public bool Deploying { get { return deploying; } }
        

        /// <summary>
        ///     Matches the S.W.Form.Invoke method, so that we can write to the Designer windowing
        ///     from within a spawned thread seperate from the windows thread.
        /// </summary>
        public  delegate object FormInvoke(System.Delegate @delegate, object[] parameters);
        private delegate void AppDeploymentDelegate(string packageFilePath, int numMediaFiles);
        private delegate void ShimDelegate();
        private MaxDeployingDlg shim;           
        private FormInvoke formInvoke;
        private AppDeploymentDelegate asyncDeploy;
        private MessageWriterCallback writer;
        private MessageWriterCallback status;
        private Metreos.Core.AppDeploy appDeploy;
        private volatile bool deploying;
        private object deploySync;
        private IAsyncResult currentDeployment;
        private DeployOption deployOption;
        private AutoResetEvent are;
        public AppDeployment() { Initialize(); }


        public void Initialize()
        {
            initialized     = true;
            asyncDeploy     = null;
            writer          = null;
            status          = null;
            formInvoke      = null;
            deploySync      = new object();
            deploying       = false;
            are             = new AutoResetEvent(false);
            shim            = new MaxDeployingDlg();
            appDeploy       = new Metreos.Core.AppDeploy();
            appDeploy.StageElapse           += new Step(IndicateStep);
            appDeploy.LogOutput             += new Metreos.Core.Message(WriteOutput);
            appDeploy.ErrorMessage          += new Metreos.Core.Message(WriteOutput);
            appDeploy.UninstallPrompt       += new Uninstall(PromptUserForUninstall);
            appDeploy.LoginFailed           += new LoginUserRetry(PromptUserLoginFailed);
        }

        /// <summary>
        ///      Uses the .NET async threading abilities (the .NET thread pool, in other words) to launch a thread 
        ///      which will deploy an application, while writing to the status bar and output window of the Designer   
        /// </summary>
        /// <param name="packageFilePath">
        ///     The full path to the project file (.mca)
        /// </param>
        /// <param name="writer">
        ///     Output window writer
        /// </param>
        /// <param name="status">
        ///     Status bar writer
        /// </param>
        /// <param name="formInvoke">
        ///     Handle to the Designer Form.Invoke method
        /// </param>
        /// <returns><c>true</c> if successful, <c>false</c> if not</returns>
        public bool DeployAsync(string packageFilePath, int numMediaFiles, MessageWriterCallback writer, 
            MessageWriterCallback status, FormInvoke formInvoke)
        {
            this.writer         = writer;
            this.status         = status;
            this.formInvoke     = formInvoke;

            // The placement of ShowShim here guarantees that the deployment dialog will appear everytime
            // the user attempts to deploy an application
            ShowShim();

            bool success = false;
            lock(deploySync)
            {
                // If we are already deploying then report to the user of that in the output window.
                // Otherwise, reset the deployment progress dialog and deploy
                if(!deploying)
                {
                    success   = true;
                    deploying = true;
                    shim.ResetBars();
                    asyncDeploy = new AppDeploymentDelegate(Deploy);
                    currentDeployment = asyncDeploy.BeginInvoke(packageFilePath, numMediaFiles, new AsyncCallback(EndDeploy), null);
                }
                else
                {
                    success = false;
                    WriteOutput(Const.AlreadyDeployingMsg);
                }
            }
            
            return success;
        }


        private void IndicateStep(float amount, string nextStageDescription)
        {
            Metreos.Core.Step stepDelegate = new Step(shim.StepProgressBar);
            formInvoke(stepDelegate, new object[] {amount * 100f, nextStageDescription}); 
        }


        private void Deploy(string packageFilePath, int numMediaFiles)
        {
            appDeploy.Deploy(
                MaxMain.ProjectName,
                new FileInfo(packageFilePath),
                Config.AppServerIP, Config.AppServerAdminUser, Config.AppServerAdminPass, 
                Convert.ToInt32(Config.AppServerPort), Convert.ToInt32(Config.SshPort), numMediaFiles, Convert.ToInt32(Config.SshTimeOut));
        }

     
        private void ShowShim()
        {
            // Center the deploy dialog to the parent.  This can not be done with the StartPosition attribute 
            // 'CenterParent', at least not by MSC
            if(MaxMain.DockMgr != null && MaxMain.DockMgr.Container is MaxMain)
            {
                ContainerControl parent = MaxMain.DockMgr.Container;
                shim.Left = parent.Left +  parent.Bounds.Width  / 2 - shim.Width  / 2;
                shim.Top  = parent.Top  +  parent.Bounds.Height / 2 - shim.Height / 2;
            }

            shim.Show();

            // Through only testing, I found this to be the only combination of values to ensure that the 
            // deployment dialog always shows up above the Designer.  If this were not this way,
            // I often experienced that eventualy the dialog window would show up behind.  
            // If someone changes this, please check that the deployment window *always* shows up in front
            // of the designer when deploying. -- MSC
            shim.TopMost = true;
            shim.BringToFront();
        }

        private void HideShim()
        {
            shim.Hide();
        }

        private void EndDeploy(IAsyncResult result)
        {

            ShimDelegate hideShim = new ShimDelegate(HideShim);
            formInvoke(hideShim, null);

            lock(deploySync)
            {
                try
                {
                    asyncDeploy.EndInvoke(currentDeployment);
                }
                catch(MissingMemberException)
                {
                    MaxMain.MessageWriter.WriteLine(Const.deployFailedMsg);
                    Utl.OutOfSyncMsg();
                }
                catch(Exception)
                {
                    System.Diagnostics.Debugger.Launch();
                    MaxMain.MessageWriter.WriteLine(Const.deployFailedMsg);
                    Utl.DeploymentFailed();
                }
            }

            deploying = false;
            
        }


        /// <summary>
        ///     Don't attempt to use Designer callbacks directly from a thread other than the main windowing thread.
        ///     Instead, use the Form.Invoke delegate to allow the window to process the delegate, since it 
        ///     affects window drawing
        /// </summary>
        /// <param name="message">Output window message</param>
        private void WriteOutput(string message)
        {
            if (writer != null)
                formInvoke(writer, new object[] { message} );
        }

        /// <summary>
        ///     Don't attempt to use Designer callbacks directly from a thread other than the main windowing thread.
        ///     Instead, use the Form.Invoke delegate to allow the window to process the delegate, since it 
        ///     affects window drawing
        /// </summary>
        /// <param name="message">Status bar message</param>
        private void UpdateStatus(string message)
        {
            if(status != null)
                formInvoke(status, new object[] { message } );
        }

        private void ShowDialogShim()
        {
            MaxAppUninstallDlg uninstallDlg = new MaxAppUninstallDlg();

            uninstallDlg.ShowDialog(shim);

            if (uninstallDlg.DeployOption == Metreos.Core.AppDeploy.DeployOption.Cancel)
            {
                WriteOutput(Const.DeployCanceledMsg);
                UpdateStatus(String.Empty);
            }

            deployOption = uninstallDlg.DeployOption;

            are.Set();
        }

        /// <summary>
        ///     When the user attempts to deploy an application, it must be uninstalled first if its installed.
        ///     So, we provide a dialog to user to ensure that they really want to do that
        /// </summary>
        /// <returns></returns>
        private DeployOption PromptUserForUninstall()
        {
            ShimDelegate shomShim = new ShimDelegate(ShowDialogShim);
            formInvoke(shomShim, null);

            are.WaitOne();

            return deployOption;
        }


        /// <summary>
        ///     Return true to indicate to the Deploy process 
        ///     that a retry should be attempted.
        ///     
        ///     Return false to indicate to give up on the 
        ///     Deploy process
        /// </summary>
        /// <param name="username">
        ///     User-enterned username.  
        ///     Must be set on return of true
        /// </param>
        /// <param name="password">
        ///     MD5 hashed version of the user-entered password.  
        ///     Must be set on return of true
        /// </param>
        /// <returns></returns>
        private bool PromptUserLoginFailed(out string username, out string password)
        {
            username = null;
            password = null; // Metreos.Utilities.Security.Encrypt() (MD5) must be 
            // performed on password this before relinquishing control 
            // of this method if returning true
            // NOTE: the MAX registry helper performs MD5 already.


            return false;
        }
    

        #region IDisposable Members

        public void Dispose()
        {
            deploying = false;
            if(appDeploy != null)
            {
                appDeploy.Dispose();
            }
            if(shim != null)
            {
                shim.Dispose();
            }
        }
        
        #endregion
    }

    /// <summary>
    ///     Merely wraps the metreos framework application packaging utility, specifying the options
    ///     that best suit the Designer
    /// </summary>
    public class PackageBuild
    {
        static readonly PackageBuild instance = new PackageBuild();

        static PackageBuild(){}
        public PackageBuild(){}
        public static string FrameWorkDir { get { return Config.FrameworkDirectory; } }
      
        static bool recursiveDirSearch = false;
        static bool verbose = true;
        static bool printUsage = false;

        public static void Build(AppPackagerOptions appOptions)
        {  
            // Setting some rather boring 
            appOptions.recursiveDirSearch  = recursiveDirSearch;
            appOptions.verbose = verbose;
            appOptions.printUsage = printUsage;

            if(!appOptions.ValidateCreate())
                return; // TODO : return a failed event response to framework

            try
            {
                AppPackager.BuildPackage(appOptions);
            }
            catch(Exception e)
            {
                throw e; // TODO: return a build failed message to max, and what caused the build to fail.
            }
            // TODO: return a successful event response to framework
        }
    }
}

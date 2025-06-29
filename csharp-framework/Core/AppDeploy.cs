using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.Interfaces;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Xml;
using Metreos.Core.IPC.Sftp;
using Metreos.Core.ConfigData;

namespace Metreos.Core
{
    public delegate void Message(string message);
    public delegate AppDeploy.DeployOption Uninstall();
    public delegate void Step(float stepAmount, string stepDescription);                 
    public delegate bool LoginUserRetry(out string username, out string password);

    //public delegate void MediaStep(MediaStepMessage[] mmsStepInfo);

    public class MediaStepMessage
    {
        public const string UndefinedError = "Undefined Error";
        public bool Success;
        public float StepAmount;
        public string ErrorMessage;

        public MediaStepMessage(bool success, float stepAmount, string errorMessage)
        {
            this.Success = success;
            this.StepAmount = stepAmount;
            this.ErrorMessage = (errorMessage == null || errorMessage == String.Empty) ? UndefinedError : errorMessage;
        }
    }
	public class AppDeploy : IDisposable
	{
        public enum DeployOption
        {
            Uninstall,
            Update,
            Cancel
        }

        public enum LoginAction
        {
            Failure, // Login Failure
            Success, // Login Success
            UserRetry // User has indicated they would like to retry
        }

        public const DeployOption DefaultOption = DeployOption.Cancel;

        public event Message LogOutput;
        public event Message ErrorMessage;
        public event Uninstall UninstallPrompt;
        public event Step StageElapse;
        public event LoginUserRetry LoginFailed;
        //public event MediaStep MediaStageElapse;

        public const int commandTimeoutMs     = 5000;
        public const int commandTimeoutS      = 20;

        private IpcXmlClient client;
        private static XmlSerializer serializer   = new XmlSerializer(typeof(commandType));
        private static XmlSerializer deserializer = new XmlSerializer(typeof(responseType));
        private static XmlDocument cdataMaker     = new XmlDocument();
        private responseType lastResponse;
        
        private StringBuilder cmdBuilder;
        private StringWriter cmdWriter;
        private SftpClient sftpClient;

        // Temporary locals valid only for duration of deploy invoke
        private string appName;
        private string appServerUsername;
        private string appServerPassword;
        private int appServerPort;
        private int sshPort;
        private int sshTimeOut;
        private string appServerIp;
        private int numMediaFiles;
        private volatile bool stop;

		public AppDeploy()
		{
            Initialize();
		}

        public void Dispose()
        {
            stop = true;

            Cleanup();

            if(sftpClient != null)
            {
                sftpClient.Close();
                sftpClient = null;
            }
            if(cmdWriter != null)
            {
                cmdWriter.Close();
            }
        }

        public bool Deploy(
            string appName,
            FileInfo package,
            string appServerIp,
            string appServerUsername, string appServerPassword, int appServerPort,
            int sshPort, int numMediaFiles
            )
        {
            return Deploy(appName, package, appServerIp, appServerUsername, appServerPassword, appServerPort, sshPort, numMediaFiles, sshTimeOut);
        }

        /// <summary>
        ///     Returns true if the application was deployed, false if the application didn't.
        ///     Keep that in mind when the 'UinstallPrompt' event is raised.  If you return false
        ///     to that event, then this method will also return false.  In other words,
        ///     false does not necessarily mean error, in that case.
        /// </summary>
        /// <param name="appServerPassword">Must be MD5'ed (Metreos.Utilities.Security)</param>
        public bool Deploy(
            string appName,
            FileInfo package,
            string appServerIp,
            string appServerUsername, string appServerPassword, int appServerPort, 
            int sshPort, int numMediaFiles, int sshTimeOut
            )
        {  
            stop = false;

            this.appName                    = appName;
            this.appServerIp                = appServerIp;
            this.sshPort                    = sshPort;
            this.appServerUsername          = appServerUsername;
            this.appServerPassword          = appServerPassword;
            this.appServerPort              = appServerPort;
            this.numMediaFiles              = numMediaFiles;
            this.sshTimeOut                 = sshTimeOut;

            string packageName = Path.GetFileNameWithoutExtension(package.Name);

            bool update = false;
            bool success = false;
            if (CheckPackageExistence(package)                  &&
                Reconnect()                                     &&
                Login()                                         &&
                IsAppLoaded(packageName, out update)            &&
                TransferApplication(package.FullName))
            {
                success = true;

                if(update)
                {
                    if(appName == null)
                    {
                        Output(IOutput.noAppNameOnUpdate);
                        success = false; 
                    }
                    else
                    {
                        success &= UpdateApplication(package);
                        success &= MediaProvisioned(packageName);
                    }
                }
                else
                {
                    success &= InstallApplication(package);
                    success &= MediaProvisioned(packageName);
                }
                
                if(success)
                    Output(IOutput.deploySucceeded);
            }
            
            Cleanup();
            return success;
        }

        
        protected void Initialize()
        {
            stop                = false;
            sftpClient          = null;
            cmdBuilder          = new StringBuilder();
            cmdWriter           = new StringWriter(cmdBuilder);
            appServerIp         = null;
            appServerUsername   = null;
            appServerPassword   = null;
            sshPort             = 0;
            sshTimeOut          = 10;
        }

        protected void Cleanup()
        {
            if(client != null)
            {
                try { client.Close(); client.Dispose(); }
                catch { }
            }

            client = null;
        }

        protected bool CheckPackageExistence(FileInfo packageInfo)
        {
            Step(IOutput.locatingApp);

            if(!packageInfo.Exists)
            {
                Error(IErrors.appPackageNotFound);

                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool Reconnect()
        {
            Step(IOutput.connectingToAppServer[appServerIp]);

            Cleanup();

            client = new IpcXmlClient(new IPEndPoint(IPAddress.Parse(appServerIp), appServerPort));
            client.onXmlMessageReceived += new OnXmlMessageReceivedDelegate(OnResponse);
            bool success = client.Open();

            if(success == false)
            {
                Error(IErrors.inaccessible); 
            }
            return success;
        }
       
        protected bool Login()
        {    
            Step(IOutput.loggingIn[appServerUsername]);

            LoginAction result;

            do // Keep attempting to login as long as the event returns a UserRetry
            {
                result = AttemptLogin(appServerUsername, appServerPassword);
            }
            while(result == LoginAction.UserRetry);

            return result == LoginAction.Success;
        }

        protected LoginAction AttemptLogin(string username, string password)
        {
            LoginAction result; 
 
            commandType login = new Metreos.Core.IPC.Xml.commandType();
            login.name = IManagement.Commands.LogIn.ToString();
            login.param = new paramType[2];

            paramType usernameParam = new paramType();
            usernameParam.name = IManagement.ParameterNames.USERNAME;
            usernameParam.Value = appServerUsername;

            paramType passwordParam = new paramType();
            passwordParam.name = IManagement.ParameterNames.PASSWORD;
            passwordParam.Value = appServerPassword;

            login.param[0] = usernameParam;
            login.param[1] = passwordParam;

            if(!SendCommand(login, IErrors.loginFailure))
            {
                // Response was not success.   Check for loginFailure
                if(lastResponse != null && lastResponse.type == IConfig.Result.NotAuthorized)
                {
                    if(RequestUserLoginRetry(out appServerUsername, out appServerPassword))
                    {
                        result = LoginAction.UserRetry;
                    }
                    else
                    {
                        result = LoginAction.Failure;
                    }
                }
                else
                {
                    result = LoginAction.Failure;
                }
            }
            else
            {
                result = LoginAction.Success;
            }

            return result;
        }

        private bool RequestUserLoginRetry(out string username, out string password)
        {
            bool retry = false;
            username = null;
            password = null;

            if(LoginFailed != null)
            {
                retry = LoginFailed(out username, out password);
            }
    
            return retry;
        }

        protected bool IsAppLoaded(string appName, out bool update)
        {
            update = false;

            Step(IOutput.alreadyInstalledCheck);

            IConfig.Status status = IConfig.Status.Disabled;
            
            ComponentInfo[] apps;
            IConfig.Result result;
            GetApps(appName, out apps, out result);

            if(result != IConfig.Result.Success)
            {
                Error(IErrors.getApps);
                return false;
            }

            if(apps == null || apps.Length == 0) return true;

            foreach(ComponentInfo app in apps)
            {
                if(appName == app.name)  // This should never fail. But better safe, eh?
                {
                    status = app.status;

                    // Raise event for permission of install
                    DeployOption userSelection = Uninstall();

                    if(userSelection == DeployOption.Uninstall)
                    {
                        if(!DisableApplication(appName))    return false;
                        if(!UninstallApplication(appName))  return false;
                    }
                    else if(userSelection == DeployOption.Update)
                    {
                        if(!DisableApplication(appName))    return false;
                        update = true;
                    }
                    else
                    {
                        return false;
                    }
                    
                    break;
                }
            }

            return true;
        }

        protected bool TransferApplication(string packagePath)
        {
            Step(IOutput.transferringApp);

            sftpClient = new SftpClient();
            string errorReason;

            if(!sftpClient.Open(appServerIp, this.sshPort, appServerUsername, appServerPassword, sshTimeOut, out errorReason))
            {
                Error(IErrors.sftpConnectionFailure[errorReason]); 
                sftpClient.Close();
                sftpClient = null;
                return false;   
            }

            if(!sftpClient.Upload(new FileInfo(packagePath), null, out errorReason))
            {
                Error(IErrors.sftpUploadFailure[errorReason]); 
                sftpClient.Close();
                sftpClient = null;
                return false;
            }

            sftpClient.Close();
            sftpClient = null;
            return true;
        }
        /// <summary>
        ///     Polls the Application Server for media provisioning status.
        ///     1.  It succeeds in the case that there are no media servers configured,
        ///     3.  A failure can occur if unexpected messages are returned, (aborts)
        ///     4.  if the response type is error, (aborts)
        ///     5.  or if the message response is success, but the internal data is error. (continues until done)
        ///     7.  communication error
        ///     Success:
        ///     AllComplete
        ///     Failure:
        ///     BadMessage
        ///     FailedMessage
        ///     MmsError
        ///     CommError
        /// </summary>
        protected bool MediaProvisioned(string packageName)
        {
            Step(IOutput.provisioningMedia);
            
            // GetProvisioningStatus doesn't work for no media files,
            // so don't try!
            if(numMediaFiles == 0)  return true;

            bool done = false;
            bool success = true;

            bool allComplete = false;
            bool badMessage = false;
            bool failedMessage = false;
            bool commError = false;

            while(!stop && !done)
            {
                Thread.Sleep(1000);

                commError = !GetProvisioningStatus(packageName);
                if(!commError)
                {
                    ArrayList responseMessages = new ArrayList();

                    string[] responses = lastResponse.resultList;
                    IConfig.Result result = lastResponse.type;

                    if(result == IConfig.Result.Success)
                    {
                        // Case for no media servers configured
                        if(responses != null && responses.Length > 0)
                        {
                            for(int i = 0; i < responses.Length; i++)
                            {
                                float progress = 0;
                                string errorMessage = null;

                                string response = responses[i];
                                if(response != null)
                                {
                                    int index = response.IndexOf(':');

                                    if(index > 0 && index < response.Length - 1)
                                    {
                                        string type = response.Substring(0, index);
                                        string message = response.Substring(index + 1);
    
                                        if(type == IMediaManager.Fields.Progress)
                                        {
                                            try
                                            {
                                                progress = float.Parse(message);
                                                allComplete = progress == 1.0f;
                                                responseMessages.Add(new MediaStepMessage(true, progress, errorMessage));
                                            }
                                            catch
                                            { 
                                                badMessage = true;
                                            } 
                                        }
                                        else if(type == IMediaManager.Fields.Error)
                                        {
                                            success = false;
                                            errorMessage = message;
                                            responseMessages.Add(new MediaStepMessage(false, progress, errorMessage));
                                        }
                                        else if(type == IMediaManager.Fields.AppName)
                                        {
                                            // Name is returned.  Don't need it
                                            continue;
                                        }
                                        else if(type == ICommands.Fields.TRANS_ID)
                                        {
                                            //Trans ID is returned.  Don't need it
                                            continue;
                                        }
                                        else
                                        {
                                            badMessage = true;
                                            // not a mms field
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        badMessage = true;
                                        // not a mms field
                                        continue;
                                    }
                                }
                                else
                                {
                                    badMessage = true;
                                    // not a mms field
                                    continue;
                                }
                            }
                        }
                        else // no progress servers
                        {
                            badMessage = true;
                        }
                    }
                    else
                    {
                        failedMessage = true;
                    }
                    
                    MediaStepMessage[] allMessages = new MediaStepMessage[responseMessages.Count];
                    responseMessages.CopyTo(allMessages);

                    // Determine outcome of poll
                    // -------------------------
                    // Outright failures tests
                    if(badMessage)
                    {
                        done = true;
                        success = false;
                        Error(IOutput.badMessagePM);
                    }
                    if(failedMessage)
                    {
                        done = true;
                        success = false;
                        Error(IOutput.failedMessage);
                    }
                    if(allComplete)
                    {
                        done = true;
                        foreach(MediaStepMessage message in allMessages)
                        {
                            if(!message.Success && message.ErrorMessage != null)
                            {
                                Error(message.ErrorMessage.Trim());
                            }
                        }
                    }
                }
                else
                {
                    done = true;
                    success = false;
                    Error(IOutput.commError);
                }
            }
            
            if(!success)
            {
                Output(IOutput.provisioningError);
            }

            return success;
        }

        protected bool UpdateApplication(FileInfo package)
        {
            Step(IOutput.installApp);

            bool success = false;

            try
            {
                commandType installAppCommand = new commandType();
                installAppCommand.name = IManagement.Commands.UpdateApplication.ToString();
                installAppCommand.param = new paramType[2];

                paramType fileName = new paramType();
                fileName.name = IManagement.ParameterNames.NAME;
                fileName.Value = package.Name;

                paramType appNameParam = new paramType();
                appNameParam.name = IManagement.ParameterNames.APP_NAME;
                appNameParam.Value = appName;

                installAppCommand.param[0] = fileName;
                installAppCommand.param[1] = appNameParam;
        
                success = SendCommand(
                    installAppCommand, 
                    IErrors.appWillNotInstall["Error in updating application"], -1);
            }
            catch
            {
                Error(IErrors.appWillNotInstall["Error in updating application"]);
                success = false;
            }
         
            if(success && !IsResponseSuccess())
            {
                Error(IErrors.appWillNotInstall[lastResponse != null ? lastResponse.Value : String.Empty]);
                success = false;
            }

            return success;
        }
        
        protected bool InstallApplication(FileInfo package)
        {
            Step(IOutput.installApp);

            bool success = false;

            try
            {
                commandType installAppCommand = new commandType();
                installAppCommand.name = IManagement.Commands.InstallApplication.ToString();
                installAppCommand.param = new paramType[1];

                paramType appName = new paramType();
                appName.name = IManagement.ParameterNames.NAME;
                appName.Value = package.Name;

                installAppCommand.param[0] = appName;
        
                success = SendCommand(
                    installAppCommand, 
                    IErrors.appWillNotInstall["Error in installing application"], -1);
            }
            catch
            {
                Error(IErrors.appWillNotInstall["Error in installing application"]);
                success = false;
            }

            if(success && !IsResponseSuccess())
            {
                Error(IErrors.appWillNotInstall[lastResponse != null ? lastResponse.Value : String.Empty]);
                success = false;
            }

            return success;
        }


        protected bool DisableApplication(string packageName)
        {
            // Disable the application
            commandType disableCommand = new commandType();
            disableCommand.name = IManagement.Commands.DisableApplication.ToString();
            disableCommand.param = new paramType[1];

            paramType packageNameParam = new paramType();
            packageNameParam.name = IManagement.ParameterNames.NAME;
            packageNameParam.Value = packageName;

            disableCommand.param[0] = packageNameParam;

            bool success = SendCommand(disableCommand, IErrors.disableApp);
            if(!success || !IsResponseSuccess())
            {
                return false;
            }

            return true;
        }
     
        protected bool UninstallApplication(string packageName)
        {
            // Uninstall the application
            commandType uninstallCommand = new commandType();
            uninstallCommand.name = IManagement.Commands.UninstallApplication.ToString();
            uninstallCommand.param = new paramType[1];

            paramType packageNameParam = new paramType();
            packageNameParam.name = IManagement.ParameterNames.NAME;
            packageNameParam.Value = packageName;

            uninstallCommand.param[0] = packageNameParam;

            bool success = SendCommand(uninstallCommand, 
                IErrors.appWillNotUninstall[lastResponse!= null ? 
                lastResponse.Value : String.Empty]);

            if(!success || !IsResponseSuccess())
            {
                return false;
            }

            return true;
        }

        protected bool GetProvisioningStatus(string appName)
        {
            //Step(IOutput.provisioningMedia);

            commandType provisioningStatus = new Metreos.Core.IPC.Xml.commandType();
            provisioningStatus.name = IManagement.Commands.GetProvisioningStatus.ToString();
            provisioningStatus.param = new paramType[1];

            paramType appParam = new paramType();
            appParam.name = IManagement.ParameterNames.APP_NAME;
            appParam.Value = appName;

            provisioningStatus.param[0] = appParam;

            return SendCommand(provisioningStatus, IErrors.provisioningFailure);
        }

        protected bool SendCommand(object cmd, string failMsg)
        {
            return SendCommand(cmd, failMsg, commandTimeoutS);
        }

        protected bool SendCommand(object cmd, string failMsg, int commandTimeoutS)
        {
            lock(this)
            {
                try
                {
                    lastResponse = null;
                    cmdBuilder.Length = 0;

                    serializer.Serialize(cmdWriter, cmd);
                    client.Write(cmdBuilder.ToString());

                    if(commandTimeoutS < 0) Monitor.Wait(this);
                    else                    Monitor.Wait(this, commandTimeoutS * 1000);
                }
                catch
                {
                    Error(IErrors.inaccessible);
                    return false;
                }

                if(!IsResponseSuccess())
                {
                    Error(failMsg);
                    return false;
                }
            }

            return true;
        }

        protected void OnResponse(IpcXmlClient ipcClient, string responseStr)
        {
            lock(this)
            {
                try
                {
                    StringReader sr = new StringReader(responseStr);
                    lastResponse = (responseType) deserializer.Deserialize(sr);
                    string msg = sr.ReadToEnd();
                    Console.WriteLine(msg);
                }
                catch
                {
                    lastResponse = null;
                }

                Monitor.Pulse(this);
            }
        }

        protected void GetApps(string appName, out ComponentInfo[] apps, out IConfig.Result result)
        {
            apps = null;
            result = IConfig.Result.NotFound;

            commandType getApps = new commandType();
            getApps.name = IManagement.Commands.GetApps.ToString(); 

            bool success = SendCommand(getApps, IErrors.getApps);
            if(!success)
            {
                result = IConfig.Result.ServerError;
                return;
            }
            apps = lastResponse.componentInfo;
            result = lastResponse.type;
        }
    
        protected bool CheckServerForCondition(string appName, IConfig.Status status, out bool notFound)
        {
            notFound = true;
            IConfig.Status failedStatus = IConfig.Status.Disabled;
            ComponentInfo[] apps;
            notFound = true;
            IConfig.Result result;
            GetApps(appName, out apps, out result);
    
            if(IConfig.Result.Success != result)
            {
                return false;
            }

            if(apps != null && apps.Length > 0)
            {
                foreach(ComponentInfo app in apps)
                {
                    if(app.name == appName)
                    {
                        notFound = false;
                        if(app.status == status)  return true;
                        else
                        {
                            failedStatus = app.status;

                            if(failedStatus == IConfig.Status.Disabled_Error) return false;
                        }
                    }
                }
            }

            // App not found
            return false;
        }

        protected bool IsResponseSuccess()
        {
            if(lastResponse == null)  return false;
            return lastResponse.type == IConfig.Result.Success;
        }


        protected void Output(string message)
        {
            if(LogOutput != null)
            {
                LogOutput(message);
            }
        }

        protected void Error(string message)
        {
            if(ErrorMessage != null)
            {
                ErrorMessage(message);
            }
        }

        protected DeployOption Uninstall()
        {
            if(UninstallPrompt != null)
            {
                System.Delegate[] suscribers = UninstallPrompt.GetInvocationList();
                foreach(Uninstall prompt in suscribers)
                {
                    // It doesn't make sense to have multiple consumers for this event
                    // Just go with the first non-null one
                    if(prompt != null)
                    {
                        return prompt();
                    }
                }
            }

            return DefaultOption;
        }

        protected void Step(string description)
        {
            if(StageElapse != null)
            {
                StageElapse(1.0f/7.0f, description);
            }
        }

//        protected void MediaStep(MediaStepMessage[] messages)
//        {
//            if(MediaStageElapse != null)
//            {
//                MediaStageElapse(messages);
//            }
//        }
    }
}

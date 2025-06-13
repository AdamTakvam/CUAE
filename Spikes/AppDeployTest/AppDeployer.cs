using System;
using System.IO;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Threading;

using Metreos.AppArchiveCore;
using Metreos.Interfaces;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Xml;
using Metreos.Core;
using Metreos.Core.ConfigData;

namespace AppDeployTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AppDeployer
	{
    private Thread run;
    private IpcXmlClient client;
    private static XmlSerializer serializer   = new XmlSerializer(typeof(commandType));
    private static XmlSerializer deserializer = new XmlSerializer(typeof(responseType));
    private static XmlDocument cdataMaker     = new XmlDocument();
    private responseType lastResponse;

    
    public const int commandTimeoutMs     = 15000;   
    private StringBuilder cmdBuilder;
    private StringWriter cmdWriter;
    private string appName;
    private string ipAddress;
    private bool shutdown;
    private int count;
    private int errorCount;

		public AppDeployer(string appName, string ipAddress)
		{
      this.count = 0;
      this.errorCount = 0;
      this.appName = appName;
      this.ipAddress = ipAddress;
		  shutdown = false;
	    run = new Thread(new ThreadStart(Run));
      cmdBuilder = new StringBuilder();
      cmdWriter = new StringWriter(cmdBuilder);
		}

    public void Start()
    {
      run.Start();
    }

    public void Stop()
    {
      shutdown = true;
    }

    public void Run()
    {
      while(!shutdown)
      {
        IConfig.Result result;

        bool connect = Reconnect();

        if(!connect)
        {
          errorCount++;
          Console.WriteLine("Could not reconnect");
          Console.WriteLine(errorCount + " errors");
          continue;
        }

        bool login = Login("Administrator", "metreos");

        if(!login)
        {
          errorCount++;
          Console.WriteLine("Could not login");
          Console.WriteLine(errorCount + " errors");
          continue;
        }

        bool disable = DisableApp(appName);

        if(!disable)
        {
          errorCount++;
          Console.WriteLine("Could disable the application " + appName );
          Console.WriteLine(errorCount + " errors");
          continue;
        }

        bool deploy = DeployApplication(new FileInfo(@"..\..\..\TestApp\VoiceTunnel.mca"), out result);

        if(!deploy)
        {
          errorCount++;
          Console.WriteLine("Could not deploy the application " + appName);
          Console.WriteLine(errorCount + " errors");
          continue;
        }

        count++;
        Console.WriteLine("Successfully deployed " + count + " times");
      }
    }

    public void Cleanup()
    {
      if(client != null)
      {
        try { client.Close(); client.Dispose(); }
        catch { }
      }

      client = null;
    }

    public bool Reconnect()
    {
      Cleanup();

      client = new IpcXmlClient();
      client.OnMessageReceived += new IpcXmlClient.OnMessageReceivedDelegate(OnResponse);
      bool success = client.Open(ipAddress, 8120, 5);

      if(success == false)
      {
      }
      return success;
    }

    private bool Login(string username, string password)
    {    
        commandType login = new Metreos.Core.IPC.Xml.commandType();
        login.name = IManagement.Commands.LogIn.ToString();
        login.param = new paramType[2];

        paramType usernameParam = new paramType();
        usernameParam.name = IManagement.ParameterNames.USERNAME;
        usernameParam.Value = username;

        paramType passwordParam = new paramType();
        passwordParam.name = IManagement.ParameterNames.PASSWORD;
        passwordParam.Value = password;

        login.param[0] = usernameParam;
        login.param[1] = passwordParam;

        return SendCommand(login, IErrors.loginFailure);
    }

    private void GetApps(string appName, out ComponentInfo[] apps, out IConfig.Result result)
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


    private bool DisableApp(string appName)
    {
      ComponentInfo[] apps;
      IConfig.Status status = IConfig.Status.Disabled;
      IConfig.Result result;

      GetApps(appName, out apps, out result);

      if(result != IConfig.Result.Success)
      {
        return false;
      }

      if(apps == null || apps.Length == 0) return true;

      foreach(ComponentInfo app in apps)
      {
        if(appName == app.name)
        {
          status = app.status;
          if(!QueryUserForUninstall(appName, status))                 return false;
          if(!QueryForAppServerUninstallSuccess(appName, 1)) return false;
          break;
        }
      }

      return true;
    }

    private bool QueryUserForUninstall(string packageName, IConfig.Status status)
    {
      

      if(status == IConfig.Status.Enabled_Running || status == IConfig.Status.Enabled_Stopped)
      {
        // Disable the application
          commandType disableCommand = new commandType();
          disableCommand.name = IManagement.Commands.DisableApplication.ToString();
          disableCommand.param = new paramType[1];

          paramType appName = new paramType();
          appName.name = IManagement.ParameterNames.NAME;
          appName.Value = packageName;

        disableCommand.param[0] = appName;

        bool success = SendCommand(disableCommand, IErrors.disableApp);
        if(!success || lastResponse.type != IConfig.Result.Success)
        {
          return false;
        }
      }

      return true;
    }


    public bool QueryForAppServerUninstallSuccess(string packageName, int numScripts)
    {
      // Uninstall the application
        commandType uninstallCommand = new commandType();
        uninstallCommand.name = IManagement.Commands.UninstallApplication.ToString();
        uninstallCommand.param = new paramType[1];

        paramType appName = new paramType();
        appName.name = IManagement.ParameterNames.NAME;
        appName.Value = packageName;


      uninstallCommand.param[0] = appName;

      bool success = SendCommand(uninstallCommand, IErrors.appWillNotUninstall[lastResponse.Value]);

      if(!success || lastResponse.type != IConfig.Result.Success)
      {
        return false;
      }

      return true;
    }

    public bool DeployApplication(FileInfo package, out IConfig.Result result)
    {
      result = IConfig.Result.Failure;
      FileStream stream;
      bool success;

      try
      {
        stream = package.Open(FileMode.Open);
      }
      catch
      {
        return false;
      }

      byte[] bytes = new byte[stream.Length];

      try
      {
        stream.Read(bytes, 0, (int) stream.Length);
      }
      catch
      {
        return false;
      }
      finally
      {
        stream.Close();
      }         

      try
      {
          commandType installAppCommand = new commandType();
          installAppCommand.name = IManagement.Commands.InstallApplication.ToString();
          installAppCommand.data = cdataMaker.CreateCDataSection(System.Convert.ToBase64String(bytes));
          installAppCommand.param = new paramType[1];

          paramType appName = new paramType();
          appName.name = IManagement.ParameterNames.NAME;
          appName.Value = package.Name;

        installAppCommand.param[0] = appName;
        
        success = SendCommand(installAppCommand, IErrors.appWillNotInstall["Error in installing application"]);
      }
      catch
      {
        return false;
      }
         
      if(!success)
      {
        return false;
      }

      if(lastResponse.type != IConfig.Result.Success)
      {
        return false;
      }

      return true;
    }

    private bool SendCommand(object cmd, string failMsg)
    {
      lock(this)
      {
        try
        {
          lastResponse = null;
          cmdBuilder.Length = 0;

          serializer.Serialize(cmdWriter, cmd);
          client.Write(cmdBuilder.ToString());

          Monitor.Wait(this, commandTimeoutMs);
        }
        catch
        {
          return false;
        }
        if(lastResponse != null && lastResponse.type != IConfig.Result.Success)
        {    
          return false;
        }
      }

      return true;
    }

    private void OnResponse(string responseStr)
    {
      lock(this)
      {
        try
        {
          StringReader sr = new StringReader(responseStr);
          lastResponse = (responseType) deserializer.Deserialize(sr);
        }
        catch
        {
          lastResponse = null;
        }

        Monitor.Pulse(this);
      }
    }
	}
}

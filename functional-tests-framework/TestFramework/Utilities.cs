using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary>
    /// Summary description for Utilities.
    /// </summary>
    public class Utilities
    {   
        public static string GetServerUri(string ip, string port)
        {
            return "tcp://" + ip + ':' + port + '/' + Constants.serverProxyUri;
        }   
        
        public static string GetServerUri(string ip)
        {
            return GetServerUri(ip, Constants.serverPort.ToString());
        }

        public static string GetServerUri()
        {
            return GetServerUri("localhost", Constants.serverPort.ToString());
        }

        public static string GetClientUri(string port)
        {
            return "tcp://localhost" + ':' + port + '/' + Constants.clientProxyUri;
        }

        public static string GetClientUri()
        {
            return GetClientUri(Constants.clientPort.ToString()); 
        }

        public static string[] MakeAbsoluteForPackages(string[] appRelPaths, string baseDir)
        {
            string[] appFullPaths = new string[appRelPaths.Length];
            
            int i = 0;

            foreach(string appRelPath in appRelPaths)
            {
                string[] spaces = appRelPath.Split(new char[] {'.'});

                if(spaces.Length < 2)
                {
                    throw new MalformedTestBank("The application type " + appRelPath + "is malformed");
                }

                appFullPaths[i++] = Path.Combine(baseDir, spaces[spaces.Length - 2] + Path.DirectorySeparatorChar
                    + spaces[spaces.Length - 1] + Constants.packageExtension);
            }

            return appFullPaths;
        }

        public static FileInfo[] CreatePackageFileList(string[] appFullPaths)
        {
            ArrayList fileInfoList = new ArrayList();

            foreach(string appFullPath in appFullPaths)
            {
                FileInfo packagedApplication = new FileInfo(appFullPath);

                if(!packagedApplication.Exists)
                {
                    throw new InstallException("Unable to find a test application at " + appFullPath + ".  Unable to execute test.");
                }
                else
                {
                    fileInfoList.Add(packagedApplication);
                }
            }

            FileInfo[] list = new FileInfo[fileInfoList.Count];
            fileInfoList.CopyTo(list);
            return list;
        }

        public static string[] RipPureAppNames(string[] appRelPaths)
        {
            string[] pureAppNames = new string[appRelPaths.Length];

            // Should contain one '\\'

            int i = 0;
            foreach(string appRelPath in appRelPaths)
            {
                string[] spaces = appRelPath.Split(new char[] {'.'});

                if(spaces.Length < 2)
                {
                    throw new MalformedTestBank("The application type " + appRelPath + "is malformed");
                }
               
                pureAppNames[i++] = spaces[spaces.Length - 1];
            }

            return pureAppNames;
        }

        public static CommandMessage CreateLog(string message, TraceLevel level)
        {
            CommandMessage im = new CommandMessage();
            im.AddField(Constants.LOG_MESSAGE, message);
            im.AddField(Constants.LOG_LEVEL, level.ToString());
            return im;
        }

        public static string GetOamRemotingUri(string samoaIp, string samoaPort, string samoaOamUrl)
        {
            return String.Format("http://{0}:{1}/{2}", samoaIp, samoaPort, samoaOamUrl);
        }

        public static DirectoryInfo CreateUserSettingsFolders(string folderPath)
        {
            DirectoryInfo userSettingsFolder = new DirectoryInfo(folderPath);

            try
            {
                if(!userSettingsFolder.Exists)
                {
                    userSettingsFolder.Create();
                }

                DirectoryInfo testSettingsFolder = new DirectoryInfo(userSettingsFolder.FullName + Path.DirectorySeparatorChar +
                                "Metreos" + Path.DirectorySeparatorChar + "FunctionalTests");
			
                if(!testSettingsFolder.Exists)
                {
                    testSettingsFolder.Create();
                }

                return testSettingsFolder;
            }
            catch
            {
                return new DirectoryInfo(Directory.GetCurrentDirectory());
            }      
        }

        public static TraceLevel ParseForLogLevel(string logLevel)
        {
            TraceLevel level;

            try
            {
                level = (TraceLevel)
                    Enum.Parse(typeof(TraceLevel), logLevel);
            }
            catch
            {
                level = TraceLevel.Verbose;
            }

            return level;
        }

        public static void RequestForLogIterator(CommandMessage im, CommandMessageDelegate RequestForLog)
        {
            if(RequestForLog == null) return;

            CommandMessageDelegate[] clientLogs = RequestForLog.GetInvocationList() as CommandMessageDelegate[];

            if(clientLogs != null)
            {
                foreach(CommandMessageDelegate testLogRequestor in clientLogs)
                {
                    try
                    {
                        testLogRequestor(im);
                    }
                    catch
                    {
                        RequestForLog -= testLogRequestor;
                    }
                }
            }
            else
            {
                Debug.Assert(false, "Empty delegate in RequestForLogIterator!");;
            }
        }

        public static string GetFullAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }
    }
}

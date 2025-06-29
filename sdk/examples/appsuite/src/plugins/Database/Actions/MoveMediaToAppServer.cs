using System;
using System.IO;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Moves a media file from the MediaServer to the Applicaiton Server </summary>
	public class MoveMediaToAppSever : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("The filename of the media file", true)]
        public string MediaFilename { set { mediaFilename = value ; } }
        private string mediaFilename;

        [ActionParamField("The media server IP address", true)]
        public string MediaServerIp { set { mediaServerIp = value; } }
        private string mediaServerIp;

        [ResultDataField("The relative path of the URL where the moved file can be found")]
        public string MediafileUrl { get { return mediafileUrl; } }
        private string mediafileUrl;

		public MoveMediaToAppSever()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            userId          = 0;
            mediaServerIp   = null;
            mediaFilename   = null;
            mediafileUrl    = String.Empty;
        }

        [Action("MoveMediaToAppSever", false, "Move Media To Application Server", "Moves a media file from the MediaServer to the Applicaiton Server")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            if(mediaFilename == null)
            {
                return IApp.VALUE_FAILURE;
            }

            using(Config configDbAccess = new Config(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                using(Users usersDbAccess = new Users(
                          sessionData.DbConnections[SqlConstants.DbConnectionName], 
                          log,
                          sessionData.AppName,
                          sessionData.PartitionName,
                          DbTable.DetermineAllowWrite(sessionData.CustomData)))
                {
                    // First attempt to download the dll
                    string mediapath = configDbAccess.GetConfigValue(ConfigurationName.MediaServerRelPath);
                    if(mediapath == null)
                    {
                        log.Write(TraceLevel.Error, 
                            "Unable to retrieve the relative path for the web accessable media folder");
                        return IApp.VALUE_FAILURE;
                    }

                    string mediaServerMediaUrl = String.Format("http://{0}/{1}/{2}", 
                        mediaServerIp, 
                        mediapath,
                        mediaFilename); 
                    string localpath = "N/A";
                    FileStream fileStream = null;
                    MemoryStream data = new MemoryStream();
                    UrlStatus webDlStatus = Web.Download(mediaServerMediaUrl, out data);

                    try
                    {
                        if(webDlStatus != UrlStatus.Success)
                        {
                            log.Write(TraceLevel.Error, 
                                "Unable to download the media file at '{0}'", mediaServerMediaUrl);
                            return IApp.VALUE_FAILURE;
                        }
            
                        string webserverFilepath = configDbAccess.GetConfigValue(ConfigurationName.WebServerFilepath);
                        if(webserverFilepath == null)
                        {
                            log.Write(TraceLevel.Error, 
                                "Unable to retrieve the filepath for the web server on the Application Server");
                            return IApp.VALUE_FAILURE;
                        }
                        string webserverRelpath = configDbAccess.GetConfigValue(ConfigurationName.RecordingRelPath);
                        if(webserverRelpath == null)
                        {
                            log.Write(TraceLevel.Error, 
                                "Unable to retrieve the relative path for the web server media files on the Application Server");
                            return IApp.VALUE_FAILURE;
                        }
   
                        mediafileUrl = String.Format("{0}/{1}", userId, mediaFilename);

                        localpath = Path.Combine(webserverFilepath, webserverRelpath);
                        localpath = Path.Combine(localpath, userId != 0 ? userId.ToString() : "NO_USER");
                        if(Directory.Exists(localpath) == false)
                        {
                            Directory.CreateDirectory(localpath);
                        }

                        localpath = Path.Combine(localpath, mediaFilename);
                        fileStream = new FileStream(localpath, FileMode.Create);
                        data.WriteTo(fileStream);
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, 
                            "Unable to save the media '{0}' to the local Application Server at '{1}'\n" +
                            "Exception message {2}", 
                            mediaServerMediaUrl,  localpath, e.Message);
                        return IApp.VALUE_FAILURE;
                    }
                    finally
                    {
                        if(data != null)
                        {
                            data.Close();
                        }
                        if(fileStream != null)
                        {
                            fileStream.Close();
                        }
                    }

                    return IApp.VALUE_SUCCESS;
                }
            }
        }
	}
}

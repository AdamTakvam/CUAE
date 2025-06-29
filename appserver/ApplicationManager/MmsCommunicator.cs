using System;
using System.IO;
using System.Diagnostics;

using Metreos.Core.IPC.Sftp;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.AppServer.ApplicationManager
{
    /// <summary>
    /// Just a wrapper to keep the AppManager unaware of the mechanism used to transfer media files
    /// </summary>
	public class MmsCommunicator : IDisposable
	{
        private readonly SftpClient client;

		public MmsCommunicator()
		{
            this.client = new SftpClient();
		}

        public void Dispose()
        {
            Disconnect();
        }

        public bool Connect(string mmsIp, uint mmsPort, string username, string password, 
            string appName, string locale, out string failReason)
        {
            bool success = false;            
            try
            {
                success = client.Open(mmsIp, (int)mmsPort, username, password, out failReason);
            }
            catch(Exception e)
            {
                failReason = e.Message;
            }

            // Change dir to: %BasePath%\AppName\Locale
            if(success)
            {
                if(!client.SubDirectories.Contains(appName))
                    client.MakeDir(appName);
                client.ChangeDir(appName);

                if(!client.SubDirectories.Contains(locale))
                    client.MakeDir(locale);
                client.ChangeDir(locale);
            }

            return success;
        }

        public void Disconnect()
        {
            try { client.Close(); }
            catch {}
        }

        public bool ProvisionMedia(FileInfo mediaFile, out string failReason)
        {
            return client.Upload(mediaFile, null, out failReason);
        }
	}
}

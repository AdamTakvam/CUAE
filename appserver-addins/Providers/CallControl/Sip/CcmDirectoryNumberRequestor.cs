using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Xml;

using nsoftware.IPWorks;

using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Interfaces;
using Metreos.Core;

namespace Metreos.CallControl.Sip
{
	/// <summary>
	/// Summary description for CcmDirectoryNumberRequestor.
	/// </summary>
	public class CcmDirectoryNumberRequestor
	{
		private Thread requestor;
		private AutoResetEvent requestEvent = new AutoResetEvent(false);
	
		private Queue requests = new Queue();

		private volatile bool shutdown = false;

		private const int TFTP_TIMEOUT = 20;		//overall tft timeout in seconds
		private int TFTP_MAX_RETRANSMITS = 5;	//max number of times to retry sending a failed packet before giving up
		private const int TFTP_PORT = 69;

		private Tftp tftp = new Tftp();
		private StringBuilder buffer = new StringBuilder(9000);	//average config file size is about 8k

		private const string DEVICE_TAG = "device";
		private const string PROFILE_TAG = "sipProfile";
		private const string LINES_TAG = "sipLines";
		private const string LINE_TAG = "line";
		private const string DIRECTORY_NUM_TAG = "name";

		//private const string DIRECTORY_NUM_TAG = "device::sipProfile::sipLines::line.name";
		private const string XSI_ATTR = "xsi:type=\"axl:XIPPhone\"";
		LogWriter log;
		IConfigUtility configUtility;
		SipProxy proxy;

		public CcmDirectoryNumberRequestor(IConfigUtility configUtility, LogWriter log, SipProxy p)
		{
			this.configUtility = configUtility;
			this.log = log;
			proxy = p;

			requestor = new Thread(new ThreadStart(this.StartProc));
			tftp.TFTPPort = TFTP_PORT;
			tftp.Timeout = TFTP_TIMEOUT;
			tftp.MaxRetransmits = TFTP_MAX_RETRANSMITS;
			tftp.OnTransfer += new Tftp.OnTransferHandler(OnTransfer);
			tftp.OnStartTransfer += new Tftp.OnStartTransferHandler(OnStartTransfer);
			tftp.OnEndTransfer += new Tftp.OnEndTransferHandler(OnEndTransfer);
		}

		public void AddRequest(SipDeviceInfo di)
		{
			lock(requests.SyncRoot)
			{
				requests.Enqueue(di);
			}

			requestEvent.Set();
		}

		private SipDeviceInfo NextRequest()
		{
			SipDeviceInfo di = null;

			lock(requests.SyncRoot)
			{
				try
				{
					di = (SipDeviceInfo) requests.Dequeue();
				}
				catch(InvalidOperationException)
				{
					//empty queue, just ignore it
				}
			}

			return di;
		}

		public void StartProc()
		{
			SipDeviceInfo di = null;

			while(!shutdown)
			{
				requestEvent.WaitOne();
				while ( !shutdown && (di = NextRequest()) != null)
					RequestDeviceConfig(di);
			}
		}

		public void Start()
		{
			requestor.Start();
		}

		public void Shutdown()
		{
			shutdown = true;
			if (requestor.Join(2000) == false)
				requestor.Abort();
		}

		private void RequestDeviceConfig(SipDeviceInfo di)
		{
			tftp.TFTPServer = di.ServerAddrs[0].ToString();
			StringBuilder sb = new StringBuilder(64);
			sb.Append("SEP").Append(di.Name).Append(".cnf.xml");
			tftp.RemoteFile = sb.ToString();
			
			XmlTextReader reader = null;

			try
			{
				tftp.GetFile();

				//remove xsi from the file so xml parser wont whine
				buffer.Replace(XSI_ATTR, "");
				//now parse the string to get the directory number
				reader = new  XmlTextReader(new StringReader(buffer.ToString()));
				reader.MoveToContent();
				//first look for device tag

                // WTF is this supposed to be?   -- APC
                //while (DEVICE_TAG != DEVICE_TAG && reader.Read())
                //{
                //}

				//then look for profile tag within device
				if (DEVICE_TAG == reader.Name)
				{
					while (reader.Read() && PROFILE_TAG != reader.Name)
					{
					}
				}

				//then lines tag within profile
				if (PROFILE_TAG == reader.Name)
				{
					while (reader.Read() && LINES_TAG != reader.Name)
					{
					}
				}

				//then line tag within lines
				if (LINES_TAG == reader.Name)
				{
					while (reader.Read() && LINE_TAG != reader.Name)
					{
					}
				}

				//finally the directory number tag within line
				if (LINE_TAG == reader.Name)
				{
					while (reader.Read() && DIRECTORY_NUM_TAG != reader.Name)
					{
					}
				}

				string num = null;
				if (DIRECTORY_NUM_TAG == reader.Name)
					num = reader.ReadString();

				if (num != null && num.Length > 0) //got the number
				{
					int dn = Convert.ToInt32(num.ToString());
					log.Write(TraceLevel.Info, "Retrieved directory number: {0} for device: {1}",
						dn, di.Name);

					//need to request registration
					di.DirectoryNumber = num;
					proxy.SendRegister(di);
				}
				else //signal the error
				{
					log.Write(TraceLevel.Error, "Failed to retrieve directory number for device: {0}. Missing name contents.",
						di.Name);
					di.Status = IConfig.Status.Enabled_Stopped;
					configUtility.UpdateDeviceStatus(di.Name, di.Type, di.Status);
				}
			}
			catch (IPWorksException ie)
			{
				log.Write(TraceLevel.Error, "Failed to retrieve configuration file {0} for device {1}. Exception: {2}",
					tftp.RemoteFile, di.Name, ie);

				di.Status = IConfig.Status.Enabled_Stopped;
				configUtility.UpdateDeviceStatus(di.Name, di.Type, di.Status);
			}
			catch (XmlException xe)
			{
				log.Write(TraceLevel.Error, "Failed to parse the configuration file for device {0}. Exception: {1}", di.Name, xe);
				di.Status = IConfig.Status.Enabled_Stopped;
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Failed to parse the configuration file for device {0}. Exception: {1}", di.Name, e);
				di.Status = IConfig.Status.Enabled_Stopped;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		}

		public void OnStartTransfer(object sender, TftpStartTransferEventArgs e)
		{
			buffer.Length = 0;
		}

		public void OnTransfer(object sender, TftpTransferEventArgs e)
		{
			buffer.Append(e.Text);
		}

		public void OnEndTransfer(object sender, TftpEndTransferEventArgs e)
		{
			//got the full file
			log.Write(TraceLevel.Verbose, "End of Tftp Transfer. Length={0}", buffer.Length);
			//dump out the contents
			log.Write(TraceLevel.Verbose, buffer.ToString());
		}


	}
}

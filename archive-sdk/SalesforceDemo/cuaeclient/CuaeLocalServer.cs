using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

using System.Security.Permissions;

using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;


// to remove
// "SEP00166F6DB336"
// "10.89.31.64"

namespace CuaeLocalServer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
	public class TrayForm : System.Windows.Forms.Form
	{
        #region Cuae Client Messages
        // cuae  client
        public abstract class CuaeClient
        {
            public const int MESSAGE_LOGIN_REQUEST = 20000;
            public const int MESSAGE_LOGIN_RESPONSE = 20000;

            public const int MESSAGE_CALL_REQUEST = 20001;
            public const int MESSAGE_CALL_PROV_RESPONSE = 20001;
            public const int MESSAGE_CALL_FINAL_RESPONSE = 20003;

            public const int MESSAGE_INCOMING_CALL = 20002;

            public abstract class LoginRequestFields
            {
                public const int PARAM_DEVICENAME = 1000;
            }


            public abstract class LoginResponseFields
            {
                public const int PARAM_STATUS = 1000;

                public enum Status
                {
                    OK = 0,
                    ALREADY_LOGGED_IN = 1,

                }
            }

            public abstract class CallRequestFields
            {
                public const int PARAM_DEVICENAME = 1000;
                public const int PARAM_TO = 1001;
                public const int PARAM_CALLID = 1002;

                // todo: add incoming key from client
            }

            public abstract class CallResponseFields
            {
                public const int PARAM_STATUS = 1000;
                public const int PARAM_CALLID = 1001;
                public enum Status
                {
                    OK = 0,
                    FAILED = 1,

                }
            }

            public abstract class IncomingCallFields
            {
                public const int PARAM_PHONE_NUMBER = 1000;
                public const int PARAM_STREET = 1001;
                public const int PARAM_CITY = 1002;
                public const int PARAM_STATE = 1003;
                public const int PARAM_ZIP = 1004;
                public const int PARAM_COUNTRY = 1005;
                public const int PARAM_FIRSTNAME = 1006;
                public const int PARAM_LASTNAME = 1007;
                public const int PARAM_ACCOUNTNAME = 1008;
                public const int PARAM_CALLID = 1009;
                public const int PARAM_CONTACT_SF_ID = 1010;
                public const int PARAM_LATITUDE = 1011;
                public const int PARAM_LONGITUDE = 1012;
                public const int PARAM_ACCOUNT_SF_ID = 1013;
                public const int PARAM_ACCOUNT_TYPE = 1014;
                public const int PARAM_RESELLERS_COUNT = 1015;

                public const int PARAM_RESELLER_BASE = 3000;
            }
        }
        #endregion

        #region Data Structs

        public class CurrentCallContext
        {
            public bool isCaller;
            public string firstName;
            public string lastName;
            public string accountName;
            public string street;
            public string city;
            public string state;
            public string zip;
            public string country;
            public string latitude;
            public string longitude;
            public string callId;
            public string phoneNumber;
            public string accountType;
            public string accountSFId;
            public string contactSFId;


            public CurrentCallContext() { }
        }

        #endregion

        private System.ComponentModel.Container components = null;

        private delegate void NotifierDelegate(int width, int height, string label, object context, NotifyManager.ClickHandler click);
        private delegate void InvokeScriptDelegate(string functionName, string[] arguments);

        private NotifyIcon trayIcon;
        private MenuItem showLast;
        private MenuItem exit;
        private MenuItem configure;
        private Configure configureDlg;
        private GoogleMapHolder gMapDlg;
        private IpcFlatmapClient client;
        private AutoResetEvent are;
        private bool geoCodeSuccess;
        private string lat;
        private string lng;
        private Thread worker;
        private List<CurrentCallContext> itemsToDisplay;

        private OnConnectDelegate onConnectDelegate;
		private OnFlatmapMessageReceivedDelegate onMessageDelegate;
		private OnCloseDelegate onCloseDelegate;

        private NotifierDelegate notifyDelegate;
        private InvokeScriptDelegate invokeScriptDelegate;

		public TrayForm()
		{
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            onConnectDelegate = new OnConnectDelegate(OnConnect);
		    onMessageDelegate = new OnFlatmapMessageReceivedDelegate(OnMessageReceieved);
		    onCloseDelegate = new OnCloseDelegate(OnClose);
            notifyDelegate = new NotifierDelegate(Notifier);
            invokeScriptDelegate = new InvokeScriptDelegate(InvokeScript);

			InitializeComponent();

            itemsToDisplay = new List<CurrentCallContext>();
            configureDlg = new Configure();
            gMapDlg = new GoogleMapHolder();
            gMapDlg.Browser.ObjectForScripting = this;
            gMapDlg.Browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
            this.showLast = new MenuItem("Show Last", new EventHandler(HandleShowLast));
            this.exit = new MenuItem("Exit", new EventHandler(HandleExit));
            this.configure = new MenuItem("Configure", new EventHandler(HandleConfigure));
			
			trayIcon = new NotifyIcon();
			trayIcon.Text = "CUAE"; 
			trayIcon.Visible = true; 
			trayIcon.Icon = new Icon("TrayIcon.ico");
            trayIcon.ContextMenu = new ContextMenu( new MenuItem[] { showLast, configure, exit } );

            are = new AutoResetEvent(false);

            worker = new Thread(new ThreadStart(Worker));
            worker.IsBackground = true;
            worker.Start();

            CreateClient();
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }

        private void OnConnect(IpcClient c, bool reconnect)
        {
            // on every connect, must relogin

            bool sent = SendLogin();
        }


        private bool SendLogin()
        {
            bool sent = false;
            if (client != null)
            {
                string deviceName = Configuration.Default.DeviceName == String.Empty ? "SEP00166F6DB336" : Configuration.Default.DeviceName;
                if (deviceName != null && deviceName != String.Empty)
                {
                    FlatmapList message = new FlatmapList();
                    message.Add(CuaeClient.LoginRequestFields.PARAM_DEVICENAME, deviceName);

                    sent = client.Write(CuaeClient.MESSAGE_LOGIN_REQUEST, message);
                }
            }
            return sent;
        }

        private void CreateClient()
        {
            if (client != null)
            {
                client.Close();
                client.Dispose();
                client.onFlatmapMessageReceived -= onMessageDelegate;
                client.onClose -= onCloseDelegate;
                client.onConnect -= onConnectDelegate;
                client = null;
            }

            string cuaeIP = Configuration.Default.CUAEIP == String.Empty ? "10.89.31.64" : Configuration.Default.CUAEIP;
            int port = 10000; // todo, make configurable
            IPAddress cuaeIPAddress = null;
            try
            {
                cuaeIPAddress = IPAddress.Parse(cuaeIP);
            }
            catch{}

            if(cuaeIPAddress != null)
            {
                
                IPEndPoint endpoint = new IPEndPoint(cuaeIPAddress, port);
                client = new IpcFlatmapClient(endpoint);
                client.onConnect += onConnectDelegate;
                client.onClose += onCloseDelegate;
                client.onFlatmapMessageReceived += onMessageDelegate;

                client.Start();
            }
        }

      

        private void OnClose(IpcClient client, Exception e)
        {
            client.Close();
            client.Dispose();
            client = null;

            // update UI

            //TODO
        }

        private void OnMessageReceieved(IpcFlatmapClient ipcClient, int messageType, FlatmapList flatmap)
        {
            switch (messageType)
            {
                case CuaeClient.MESSAGE_LOGIN_RESPONSE:
                    // update UI
                    break;

                case CuaeClient.MESSAGE_CALL_FINAL_RESPONSE:
                    // update UI
                    break;

                case CuaeClient.MESSAGE_INCOMING_CALL:
                    // popup toast
                    string phoneNumber = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_PHONE_NUMBER, 1).dataValue as string;
                    string street = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_STREET, 1).dataValue as string;
                    string city = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_CITY, 1).dataValue as string;
                    string state = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_STATE, 1).dataValue as string;
                    string zip = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_ZIP, 1).dataValue as string;
                    string country = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_COUNTRY, 1).dataValue as string;
                    string firstName = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_FIRSTNAME, 1).dataValue as string;
                    string lastName = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_LASTNAME, 1).dataValue as string;
                    string accountName = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNTNAME, 1).dataValue as string;
                    string callId = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_CALLID, 1).dataValue as string;
                    string accountType = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_TYPE, 1).dataValue as string;
                    string accountSFId = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_SF_ID, 1).dataValue as string;
                    string contactSFId = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_CONTACT_SF_ID, 1).dataValue as string;
                    string latitude = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_LATITUDE, 1).dataValue as string;
                    string longitude = flatmap.Find(CuaeClient.IncomingCallFields.PARAM_LONGITUDE, 1).dataValue as string;
                    uint resellersCount = (uint) flatmap.Find(CuaeClient.IncomingCallFields.PARAM_RESELLERS_COUNT, 1).dataValue;
                    
                    // extra caller
                    CurrentCallContext currentContext = new CurrentCallContext();
                    currentContext.isCaller = true;
                    currentContext.phoneNumber = phoneNumber;
                    currentContext.street = street;
                    currentContext.city = city;
                    currentContext.state = state;
                    currentContext.zip = zip;
                    currentContext.country = country;
                    currentContext.firstName = firstName;
                    currentContext.lastName = lastName;
                    currentContext.latitude = lat;
                    currentContext.longitude = lng;
                    currentContext.accountName = accountName;
                    currentContext.callId = callId;
                    currentContext.accountType = accountType;
                    currentContext.accountSFId = accountSFId;
                    currentContext.contactSFId = contactSFId;
                    currentContext.latitude = latitude;
                    currentContext.longitude = longitude;

                    lock (this)
                    {
                        itemsToDisplay.Clear();

                        itemsToDisplay.Add(currentContext);

                    }

                    for (int i = 0; i < resellersCount; i++)
                    {
                        FlatmapList resellerInfo = new FlatmapList(flatmap.Find((uint)(CuaeClient.IncomingCallFields.PARAM_RESELLER_BASE + i), 1).dataValue as byte[]);

                        phoneNumber = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_PHONE_NUMBER, 1).dataValue as string;
                        street = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_STREET, 1).dataValue as string;
                        city = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_CITY, 1).dataValue as string;
                        state = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_STATE, 1).dataValue as string;
                        zip = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_ZIP, 1).dataValue as string;
                        country = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_COUNTRY, 1).dataValue as string;
                        firstName = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_FIRSTNAME, 1).dataValue as string;
                        lastName = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_LASTNAME, 1).dataValue as string;
                        accountName = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNTNAME, 1).dataValue as string;
                        callId = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_CALLID, 1).dataValue as string;
                        accountType = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_TYPE, 1).dataValue as string;
                        accountSFId = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_SF_ID, 1).dataValue as string;
                        contactSFId = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_CONTACT_SF_ID, 1).dataValue as string;
                        latitude = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_LATITUDE, 1).dataValue as string;
                        longitude = resellerInfo.Find(CuaeClient.IncomingCallFields.PARAM_LONGITUDE, 1).dataValue as string;

                        CurrentCallContext resellerContext = new CurrentCallContext();
                        resellerContext.isCaller = false;
                        resellerContext.phoneNumber = phoneNumber;
                        resellerContext.street = street;
                        resellerContext.city = city;
                        resellerContext.state = state;
                        resellerContext.zip = zip;
                        resellerContext.country = country;
                        resellerContext.firstName = firstName;
                        resellerContext.lastName = lastName;
                        resellerContext.latitude = lat;
                        resellerContext.longitude = lng;
                        resellerContext.accountName = accountName;
                        resellerContext.callId = callId;
                        resellerContext.accountType = accountType;
                        resellerContext.accountSFId = accountSFId;
                        resellerContext.contactSFId = contactSFId;
                        resellerContext.latitude = latitude;
                        resellerContext.longitude = longitude;

                        lock (this)
                        {
                            itemsToDisplay.Add(resellerContext);
                        }

                    }
                   
                    PopupCallNotifyToast(currentContext);
                    
                    break;
            }
        }

        public void OnClick(object sender, object state)
        {
            CurrentCallContext context = state as CurrentCallContext;

            // Popup web browser
            gMapDlg.Show();
            gMapDlg.Focus();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath = basePath.Replace('\\', '/');
            try
            {
                gMapDlg.Browser.Url = new Uri("file:///" + basePath + "mapcontrol.html");
            }
            catch (Exception e)
            {
                // just ignore request at the moment--better to retry
            }
        }

        public void PopupCallNotifyToast(CurrentCallContext context)
        {
            string formattedLabel = String.Format("{0} {1} ({2}) from {3}", context.firstName, context.lastName, context.accountName, context.phoneNumber);

            this.Invoke(notifyDelegate, -1, -1, formattedLabel, context, new NotifyManager.ClickHandler(OnClick));
        }

        private void Notifier(int width, int height, string label, object state, NotifyManager.ClickHandler click)
        {
            NotifyManager.Instance.AddNotifier(width , height , label, state, new NotifyManager.ClickHandler(click));
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Monitor.TryEnter(this, 4000))
            {
                Monitor.Pulse(this);

                Monitor.Exit(this);
            }
        }


        private void InvokeScript(string functionName, string[] parameters)
        {
            gMapDlg.Browser.Document.InvokeScript(functionName, parameters);
        }


        private void Worker()
        {
            while (true)
            {
                lock (this)
                {
                    Monitor.Wait(this); // wait for worker request

                    if (!this.Disposing)
                    {
                        foreach(CurrentCallContext currentContext in itemsToDisplay)
                        {
                            string firstName = currentContext.firstName;
                            string lastName = currentContext.lastName;
                            string contactAccountName = currentContext.accountName;
                            string street = currentContext.street;
                            string city = currentContext.city;
                            string state = currentContext.state;
                            string zip = currentContext.zip;
                            string country = currentContext.country;
                            string lat = currentContext.latitude;
                            string lng = currentContext.longitude;
                            string phone = currentContext.phoneNumber;
                            string accountType = currentContext.accountType;

                            if(lat == null || lat == String.Empty || lat == "0" || lng == null || lng == String.Empty || lng == "0")
                            {
                                string address = String.Format(@"{0}, {1} {2}, {3} {4}", street, city, state, zip, country);

                                // Must do geocode request
                                gMapDlg.Invoke(invokeScriptDelegate, new object[] { "doGeocode", new string[] {address } } );
                                geoCodeSuccess = false;

                                // wait for done
                                Monitor.Wait(this);

                                if(geoCodeSuccess)
                                {
                                    lat = this.lat;
                                    lng = this.lng;

                                    // Let server know of lat/lng of customer TODO
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if(currentContext.isCaller)
                            {
                                string formattedText = String.Format(@"<img class='contact' src='user.gif' /> <b>{0} {1}</b>({2})<br /><br /><img class='contact' src='mail.gif' /> {3}<br />{4}, {5} {6}", 
                                    firstName, lastName, contactAccountName, street, city, state, zip);

                                Console.WriteLine("Enter");
                                gMapDlg.Invoke(invokeScriptDelegate, new object[] 
                                    { "setLocation",  
                                        new string[] { lng, lat, formattedText } });
                                Console.WriteLine("Exit");
                            }
                            else
                            {
                                string formattedText = String.Format(@"{0}(Reseller)<br /><br /><img class='contact' src='mail.gif' /> {1}<br />{2}, {3} {4}", 
                                    contactAccountName, street, city, state, zip);

                                string address = String.Format(@"{0}, {1} {2}, {3} {4}", 
                                    street, city, state, zip, country);

                                gMapDlg.Invoke(invokeScriptDelegate, new object[] 
                                    { "setCallableEntity",  
                                       new string[] { lng, lat, formattedText, phone, currentContext.callId }});
                            }


                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
 
        public void GeoCodeDone(string lat, string lng)
        {
            geoCodeSuccess = true;
            this.lat = lat;
            this.lng = lng;
            lock(this)
            {
                Monitor.Pulse(this);
            }
        }

        public void GeoCodeFailed()
        {
            geoCodeSuccess = false;
            lock (this)
            {
                Monitor.Pulse(this);
            }
        }

          
       

            //            <!-- 
//100 Wild Basin Rd S, Austin, TX 78746, USA // central
//4401 Westgate Blvd Suite 308 Austin, Texas 78745  // south
//12515 Research Blvd, Building 4, Austin TX, 78759, USA // north

//-->
        
        public void ConferenceRequest(string phone, string callId)
        {
            FlatmapList message = new FlatmapList();
            message.Add(CuaeClient.CallRequestFields.PARAM_CALLID, callId);
            message.Add(CuaeClient.CallRequestFields.PARAM_DEVICENAME, Configuration.Default.DeviceName == String.Empty ? "SEP00166F6DB336" : Configuration.Default.DeviceName);
            message.Add(CuaeClient.CallRequestFields.PARAM_TO, phone);

            if (client != null)
            {
                client.Write(CuaeClient.MESSAGE_CALL_REQUEST, message);
            }
        }


        #region Dispose
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                lock (this)
                {
                    Monitor.Pulse(this);
                }

                if (trayIcon != null)
                {
                    trayIcon.Dispose();
                }
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion


        protected override void OnShown(EventArgs e)
        {
            this.Visible = false;
            base.OnShown(e);
        }

		[STAThread]
		static void Main() 
		{
			Application.Run(new TrayForm());
		}

		protected void HandleExit(object sender, EventArgs args)
		{
			Application.Exit();
		}

		protected void HandleConfigure(object sender, EventArgs args)
		{
            configureDlg.InitializeFields();

            if (configureDlg.ShowDialog() == DialogResult.OK)
            {
                CreateClient();
            }
		}


        protected void HandleShowLast(object sender, EventArgs args)
        {
            // Popup web browser
            gMapDlg.Show();
            string basePath =AppDomain.CurrentDomain.BaseDirectory;
            basePath = basePath.Replace('\\', '/');
            gMapDlg.Browser.Url = new Uri("file:///" + basePath + "mapcontrol.html");

            //CurrentCallContext currentContext = new CurrentCallContext();
            //currentContext.phoneNumber = "2432";
            //currentContext.street = "110 Wild Basin Road S";
            //currentContext.city = "Austin";
            //currentContext.state = "Texas";
            //currentContext.zip = "78746";
            //currentContext.country = "USA";
            //currentContext.firstName = "Louis";
            //currentContext.lastName = "Marascio";
            //currentContext.accountName = "Metreos";
            //currentContext.callId = "1000";
            //currentContext.isCaller = true;

            //CurrentCallContext marker1 = new CurrentCallContext();
            //marker1.street = "4401 Westgate Blvd Suite 308";
            //marker1.city = "Austin";
            //marker1.state = "TX";
            //marker1.zip = "78745";
            //marker1.country = "USA";
            //marker1.phoneNumber = "915126892237";
            //marker1.isCaller = false;
            //marker1.lastName = "Jonie";
            //marker1.firstName = "Bonie";
            //marker1.accountName = "Resellers R US";
            //lock (this)
            //{
            //    itemsToDisplay.Clear();
p
            //    itemsToDisplay.Add(currentContext);
            //    itemsToDisplay.Add(marker1);
            //}

            if (itemsToDisplay.Count > 0)
            {
                PopupCallNotifyToast(itemsToDisplay[0] as CurrentCallContext);
            }   
        }
	}
}

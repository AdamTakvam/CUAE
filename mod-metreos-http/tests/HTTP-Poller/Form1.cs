using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;

namespace HTTP_Poller
{
	/// <summary>
	/// Summary description for HTTPPoller.
	/// </summary>
    public class HTTPPoller : System.Windows.Forms.Form
    {
		public class RequestState
		{
			// This class stores the State of the request.
			const int BUFFER_SIZE = 1024;
			public StringBuilder requestData;
			public byte[] BufferRead;
			public HttpWebRequest request;
			public HttpWebResponse response;
			public Stream streamResponse;
			public bool gotResponse;
			public bool aborted;
			public RequestState()
			{
				BufferRead = new byte[BUFFER_SIZE];
				requestData = new StringBuilder("");
				request = null;
				streamResponse = null;
				gotResponse = false;
				aborted = false;
			}
		}

		public static ManualResetEvent allDone= new ManualResetEvent(false);
		const int BUFFER_SIZE = 1024;
		const int DefaultTimeout = 5 * 1000; // 5 sec timeout

        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Button startButton;

        private int intervalAmount = 0;
        private int intervalMultiplier = 1;
        private int numberOfThreads = 10;
        private UInt64 numRequests = 0;
        private UInt64 numResponses = 0;
		private UInt64 numAborted = 0;
		int interval = 1;
        private object numRequestsLock = new object();
        private object numResponsesLock = new object();
		private object numAbortedLock = new object();
        private System.Threading.Thread[] threadPool;
        private bool running = false;
        private string urlString = string.Empty;
        private System.Windows.Forms.Label runStatusLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label sentCountLabel;
        private System.Windows.Forms.Label receiveCountLabel;
        private System.Windows.Forms.RichTextBox outputTextBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public HTTPPoller()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.urlLabel = new System.Windows.Forms.Label();
			this.urlTextBox = new System.Windows.Forms.TextBox();
			this.startButton = new System.Windows.Forms.Button();
			this.runStatusLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.sentCountLabel = new System.Windows.Forms.Label();
			this.receiveCountLabel = new System.Windows.Forms.Label();
			this.outputTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// urlLabel
			// 
			this.urlLabel.Location = new System.Drawing.Point(16, 16);
			this.urlLabel.Name = "urlLabel";
			this.urlLabel.Size = new System.Drawing.Size(40, 16);
			this.urlLabel.TabIndex = 0;
			this.urlLabel.Text = "URL:";
			// 
			// urlTextBox
			// 
			this.urlTextBox.Location = new System.Drawing.Point(72, 16);
			this.urlTextBox.Name = "urlTextBox";
			this.urlTextBox.Size = new System.Drawing.Size(432, 20);
			this.urlTextBox.TabIndex = 1;
			this.urlTextBox.Text = "http://";
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(424, 48);
			this.startButton.Name = "startButton";
			this.startButton.TabIndex = 5;
			this.startButton.Text = "Start";
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// runStatusLabel
			// 
			this.runStatusLabel.Location = new System.Drawing.Point(16, 80);
			this.runStatusLabel.Name = "runStatusLabel";
			this.runStatusLabel.Size = new System.Drawing.Size(344, 23);
			this.runStatusLabel.TabIndex = 6;
			this.runStatusLabel.Text = "Stopped";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 24);
			this.label2.TabIndex = 7;
			this.label2.Text = "Requests sent:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 24);
			this.label3.TabIndex = 8;
			this.label3.Text = "Requests received:";
			// 
			// sentCountLabel
			// 
			this.sentCountLabel.Location = new System.Drawing.Point(120, 104);
			this.sentCountLabel.Name = "sentCountLabel";
			this.sentCountLabel.Size = new System.Drawing.Size(120, 24);
			this.sentCountLabel.TabIndex = 9;
			this.sentCountLabel.Text = "0";
			// 
			// receiveCountLabel
			// 
			this.receiveCountLabel.Location = new System.Drawing.Point(120, 128);
			this.receiveCountLabel.Name = "receiveCountLabel";
			this.receiveCountLabel.Size = new System.Drawing.Size(120, 24);
			this.receiveCountLabel.TabIndex = 10;
			this.receiveCountLabel.Text = "0";
			// 
			// outputTextBox
			// 
			this.outputTextBox.Location = new System.Drawing.Point(16, 160);
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.Size = new System.Drawing.Size(488, 240);
			this.outputTextBox.TabIndex = 11;
			this.outputTextBox.Text = "";
			// 
			// HTTPPoller
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 414);
			this.Controls.Add(this.urlTextBox);
			this.Controls.Add(this.outputTextBox);
			this.Controls.Add(this.receiveCountLabel);
			this.Controls.Add(this.sentCountLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.runStatusLabel);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.urlLabel);
			this.Name = "HTTPPoller";
			this.Text = "HTTP Poller";
			this.ResumeLayout(false);

		}
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new HTTPPoller());
        }

		// Abort the request if the timer fires.
		private static void TimeoutCallback(object state, bool timedOut) 
		{ 
			if (timedOut) 
			{
				RequestState request = state as RequestState;
				if (request.request != null)
				{
					request.request.Abort();
					request.aborted = true;
				}
				/*
				HttpWebRequest request = state as HttpWebRequest;
				if (request != null) 
				{
					request.Abort();
				}
				*/
			}
		}

		private static void RespCallback(IAsyncResult asynchronousResult)
		{  
			try
			{
				// State of request is asynchronous.
				RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
				HttpWebRequest  myHttpWebRequest=myRequestState.request;
				myRequestState.response = (HttpWebResponse) myHttpWebRequest.EndGetResponse(asynchronousResult);      
			}
			catch(WebException e)
			{
			}
			allDone.Set();
		}

        private void startButton_Click(object sender, System.EventArgs e)
        {
            
            if (running)
            {
                running = false;
                runStatusLabel.Text = "Stopping polling thread....";
                runStatusLabel.Refresh();
                foreach (System.Threading.Thread thread in threadPool)
                {
                    thread.Abort();
                }
                startButton.Text = "Start";
                runStatusLabel.Text = "Stopped";
            }
            else
            {
                running = true;
                numRequests = numResponses = numAborted = 0;
                sentCountLabel.Text = receiveCountLabel.Text = "0"; 
                outputTextBox.Clear();
                startButton.Text = "Stop";
                interval = intervalAmount * intervalMultiplier;
                urlString = urlTextBox.Text;
                runStatusLabel.Text = "Polling specified URL at an interval of " + interval + " milliseconds.";
                threadPool = new Thread[numberOfThreads];
                for (int i = 0; i < numberOfThreads; i++)
                {
                    threadPool[i] = new Thread(new ThreadStart(PollURL));
                    threadPool[i].Start();
                }
            }
        }

        public void PollURL()
        {
			while(true)
			{
				try
				{
					HttpWebRequest myHttpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
					RequestState myRequestState = new RequestState();  
					myRequestState.request = myHttpWebRequest;

					// Start the asynchronous request.
					IAsyncResult result=
						(IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

					lock (numRequestsLock)
					{
						numRequests++;	
						sentCountLabel.Text = numRequests.ToString();
					}

					// this line impliments the timeout, if there is a timeout, the callback fires and the request becomes aborted
					ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myRequestState, DefaultTimeout, true);

					/*
					allDone.WaitOne();

					// The response came in the allowed time. The work processing will happen in the 
					// callback function.
					if (myRequestState.gotResponse)
					{
						lock (numResponsesLock)
						{
							numResponses++;						
							receiveCountLabel.Text = numResponses.ToString();
						}
					}

					if (myRequestState.aborted)
					{
						lock (numAbortedLock)
						{
							numAborted++;						
						}
					}
      
					// Release the HttpWebResponse resource.
					myRequestState.response.Close();
					*/
				}
				catch 
				{ 
					return; 
				}

				Thread.Sleep(200);
			}
        }
    }
}

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsolePoller
{
	public class RequestState
	{
		const int BUFFER_SIZE = 1024;
		public StringBuilder requestData;
		public byte[] BufferRead;
		public HttpWebRequest request;
		public HttpWebResponse response;
		public Stream streamResponse;
		public RequestState()
		{
			BufferRead = new byte[BUFFER_SIZE];
			requestData = new StringBuilder("");
			request = null;
			streamResponse = null;
		}
	}

	class HttpWebRequest_BeginGetResponse
	{
		public static ManualResetEvent allDone= new ManualResetEvent(false);
		const int BUFFER_SIZE = 1024;
		const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout
		static int num_responses = 0;
		static int num_response_exception = 0;
		static int num_read_exception = 0;
		static string startTime = DateTime.Now.ToLongTimeString();
 
		// Abort the request if the timer fires.
		private static void TimeoutCallback(object state, bool timedOut) 
		{ 
			if (timedOut) 
			{
				HttpWebRequest request = state as HttpWebRequest;
				if (request != null) 
				{
					request.Abort();
				}
			}
		}

		static void Main()
		{    
			try
			{
				for (int i=0; i<10000; i++)
				{
					// Create a HttpWebrequest object to the desired URL. 
                    string s = "http://localhost:8000/mytest?hello=" + i.ToString();
					HttpWebRequest myHttpWebRequest= (HttpWebRequest)WebRequest.Create(s);
					/*
					myHttpWebRequest.Headers.Add("Metreos-SessionID", "1234544");
					System.Net.Cookie ci = new Cookie("metreosSessionId", "2222222");
					ci.Domain = "localhost";
					myHttpWebRequest.CookieContainer = new CookieContainer(); 
					myHttpWebRequest.CookieContainer.Add(ci);
					*/
					myHttpWebRequest.KeepAlive = false;
					// Create an instance of the RequestState and assign the previous myHttpWebRequest
					// object to it's request field.  
					RequestState myRequestState = new RequestState();  
					myRequestState.request = myHttpWebRequest;

					// Start the asynchronous request.
					IAsyncResult result = (IAsyncResult) myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);

					// this line impliments the timeout, if there is a timeout, the callback fires and the request becomes aborted
					ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

					// The response came in the allowed time. The work processing will happen in the 
					// callback function.
					allDone.WaitOne();
	      
					// Release the HttpWebResponse resource.
					if (myRequestState!=null && myRequestState.response!=null)
						myRequestState.response.Close();

					Thread.Sleep(50);
				}
			}
			catch(WebException e)
			{
				Console.WriteLine("\nMain Exception raised!");
				Console.WriteLine("\nMessage:{0}",e.Message);
				Console.WriteLine("\nStatus:{0}",e.Status);
				Console.WriteLine("Press any key to continue..........");
				Console.Read();
			}
			catch(Exception e)
			{
				Console.WriteLine("\nMain Exception raised!");
				Console.WriteLine("Source :{0} " , e.Source);
				Console.WriteLine("Message :{0} " , e.Message);
				Console.WriteLine("Press any key to continue..........");
				Console.Read();
			}

			Console.Read();
		}

		private static void RespCallback(IAsyncResult asynchronousResult)
		{  
			try
			{
				// State of request is asynchronous.
				RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
				HttpWebRequest  myHttpWebRequest=myRequestState.request;
				myRequestState.response = (HttpWebResponse) myHttpWebRequest.EndGetResponse(asynchronousResult);
      
				// Read the response into a Stream object.
				Stream responseStream = myRequestState.response.GetResponseStream();
				myRequestState.streamResponse=responseStream;
      
				// Begin the Reading of the contents of the HTML page and print it to the console.
				IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

				return;
			}
			catch(WebException e)
			{
				num_response_exception++;
				Console.WriteLine("\nRespCallback Exception raised!");
				Console.WriteLine("\nMessage:{0}",e.Message);
				Console.WriteLine("\nStatus:{0}",e.Status);
				Console.WriteLine("\nNumber of response exception:{0}",num_response_exception.ToString());
			}
			allDone.Set();
		}

		private static  void ReadCallBack(IAsyncResult asyncResult)
		{
			try
			{
				RequestState myRequestState = (RequestState)asyncResult.AsyncState;
				Stream responseStream = myRequestState.streamResponse;
				int read = responseStream.EndRead( asyncResult );
				// Read the HTML page and then print it to the console.
				if (read > 0)
				{
					myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.BufferRead, 0, read));
					IAsyncResult asynchronousResult = responseStream.BeginRead( myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);

					return;
				}
				else
				{
					Console.WriteLine("\nThe contents of the Html page are : ");
					if(myRequestState.requestData.Length>1)
					{
						string stringContent;
						stringContent = myRequestState.requestData.ToString();
						Console.WriteLine(stringContent);
					}

					num_responses++;
					Console.WriteLine("Responses Counter = " + num_responses.ToString() + ", Time = " + startTime + " - " + DateTime.Now.ToLongTimeString());

					responseStream.Close();
				}
			}
			catch(WebException e)
			{
				num_read_exception++;
				Console.WriteLine("\nReadCallBack Exception raised!");
				Console.WriteLine("\nMessage:{0}",e.Message);
				Console.WriteLine("\nStatus:{0}",e.Status);
				Console.WriteLine("\nNumber of read exception:{0}",num_read_exception.ToString());
			}
			allDone.Set();
		}
	}
}
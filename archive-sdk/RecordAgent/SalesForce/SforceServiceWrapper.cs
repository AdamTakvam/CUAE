using System;
using System.Net;
using System.IO;

namespace Metreos.RecordAgent 
{
	#region SforceServiceWrapper Class implementation
	/// <summary>
	/// Summary description for SforceServiceWrapper.
	/// </summary>
	/// 
	public class SforceServiceWrapper: sforce.SforceService
	{
		private bool _acceptZippedResponse;
		private bool _sendZippedRequest;
		private string un;
		private string pw;
		private sforce.LoginResult lr;

		/// <summary>
		/// Property to toggle compression on and off, set to true to enable sending compressed requests, false to disable.
		/// </summary>
		public bool SendZippedRequest
		{
			set { _sendZippedRequest = value;}
			get { return _sendZippedRequest;}
		}

		/// <summary>
		/// Property to toggle compression on and off, set to true to enable receiving compressed responses, false to disable.
		/// </summary>
		public bool AcceptZippedResponse
		{
			set { _acceptZippedResponse = value;}
			get { return _acceptZippedResponse;}
		}

		/// <summary>
		/// This constructor will create a request that has has it's initial compression
		/// attributes set to the parameter values.
		/// </summary>
		/// <param name="acceptZippedResponse">
		/// Set to true to accept a zipped response, false to accept plain response.
		/// </param>
		/// <param name="sendZippedRequest">
		/// Set to true to send a zipped request, false to send plain response.
		/// </param>
		public SforceServiceWrapper(bool acceptZippedResponse, bool sendZippedRequest) 
		{
			_acceptZippedResponse = acceptZippedResponse;
			_sendZippedRequest = sendZippedRequest;
		}

		/// <summary>This constructor will create a request that has compression off by default (can be enabled later).</summary>
		/// <remarks>
		/// You can later set compression instructions using the SendZippedRequest
		/// property and the <para>AcceptZippedResponse property to turn compression on and off.</para></remarks>
		public SforceServiceWrapper() 
		{
			_acceptZippedResponse = false;
			_sendZippedRequest = false;
		}

		public sforce.LoginResult loginWrapped(string username, string password)
		{
			un = username;
			pw = password;
			lr = base.login (username, password);
			this.SessionHeaderValue = new sforce.SessionHeader();
			this.SessionHeaderValue.sessionId = lr.sessionId;
			this.Url = lr.serverUrl;
			return lr;
		}

		public sforce.SaveResult[] updateWrapped(sforce.sObject[] sObjects)
		{
			try 
			{
				return base.update (sObjects);
			}
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID"))
				{
					loginWrapped(this.un, this.pw);
					return base.update (sObjects);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		public sforce.SearchResult searchWrapped(string searchString)
		{
			try 
			{
				return base.search (searchString);
			}
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID") )
				{
					loginWrapped(this.un, this.pw);
					return base.search (searchString);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		public sforce.sObject[] retrieveWrapped(string fieldList, string sObjectType, string[] ids)
		{
			try 
			{
				return base.retrieve (fieldList, sObjectType, ids);
			}
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID") )
				{
					loginWrapped(this.un, this.pw);
					return base.retrieve (fieldList, sObjectType, ids);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		public sforce.QueryResult queryMoreWrapped(string queryLocator)
		{
			try 
			{
				return base.queryMore (queryLocator);
			}
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID") )
				{
					loginWrapped(this.un, this.pw);
					return base.queryMore (queryLocator);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		public sforce.QueryResult queryWrapped(string queryString)
		{

			try 
			{
				return base.query (queryString);
			} 
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID") )
				{
					loginWrapped(this.un, this.pw);
					return base.query (queryString);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		public sforce.SaveResult[] createWrapped(sforce.sObject[] sObjects)
		{
			try 
			{
				return base.create (sObjects);
			} 
			catch (System.Web.Services.Protocols.SoapException e)
			{
				string errorCode = ((System.Xml.XmlElement)((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Xml.XmlNode)(((System.Web.Services.Protocols.SoapException)(e)).Detail)).FirstChild)).NextSibling)).FirstChild)).NextSibling).InnerText;
				if (errorCode.Equals("INVALID_SESSION_ID") )
				{
					loginWrapped(this.un, this.pw);
					return base.create(sObjects);
				} 
				else 
				{
					throw new Exception(e.Message);
				}
			}
		}

		protected override System.Net.WebRequest GetWebRequest(Uri uri)
		{
			// sforce support compression in both directions.
			return new WebRequestWrapper(base.GetWebRequest(uri), SendZippedRequest, AcceptZippedResponse);
		}
	}
	#endregion

	#region WebRequestWrapper Class implementation
	/// <summary>
	/// Summary description for WebRequestWrapper.
	/// </summary>
	public class WebRequestWrapper : WebRequest
	{
		internal const string GZIP = "gzip";
		private bool gzipRequest;
		private WebRequest wr;


		/// <summary>
		/// This constructor will send an uncompressed request, and indicate that we can accept a compressed response.
		/// You should be able to use this anywhere to get automatic support for handling compressed responses
		/// </summary>
		/// <param name="wrappedRequest"></param>
		public WebRequestWrapper(WebRequest wrappedRequest) : this(wrappedRequest, false, true)
		{
		}

		/// <summary>
		/// This constructor allows to indicate if you want to compress the request, and if you want to indicate that you can handled a compressed response
		/// </summary>
		/// <param name="wrappedRequest">The WebRequest we're wrapping.</param>
		/// <param name="compressRequest">if true, we will gzip the request message.</param>
		/// <param name="acceptCompressedResponse">if true, we will indicate that we can handle a gzip'd response, and decode it if we get a gziped response.</param>
		public WebRequestWrapper(WebRequest wrappedRequest, bool compressRequest, bool acceptCompressedResponse)
		{
			this.wr = wrappedRequest;
			this.gzipRequest = compressRequest;
			if(this.gzipRequest)
				wr.Headers["Content-Encoding"] = GZIP;
			if(acceptCompressedResponse)
				wr.Headers["Accept-Encoding"] = GZIP;
		}

		#region These are straight pass-through delegates to the contained webrequest
		// most of these just delegate to the contained WebRequest
		public override string Method
		{
			get { return wr.Method; }
			set { wr.Method = value; }
		}
	
		public override Uri RequestUri
		{
			get { return wr.RequestUri; }
		}
	
		public override WebHeaderCollection Headers
		{
			get { return wr.Headers; }
			set { wr.Headers = value; }
		}
	
		public override long ContentLength
		{
			get { return wr.ContentLength; }
			set { wr.ContentLength = value; }
		}
	
		public override string ContentType
		{
			get { return wr.ContentType; }
			set { wr.ContentType = value; }
		}
	
		public override ICredentials Credentials
		{
			get { return wr.Credentials; }
			set { wr.Credentials = value; }
		}
	
		public override bool PreAuthenticate
		{
			get { return wr.PreAuthenticate; }
			set { wr.PreAuthenticate = value; }
		}
	
		#endregion

		private Stream request_stream = null;

		public override System.IO.Stream GetRequestStream()
		{
			return WrappedRequestStream(wr.GetRequestStream());
		}
	
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return wr.BeginGetRequestStream (callback, state);
		}
	
		public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return WrappedRequestStream(wr.EndGetRequestStream (asyncResult));
		}
	
		/// <summary>
		/// helper function that wraps the request stream in a GzipOutputStream, 
		/// if we're going to be compressing the request
		/// </summary>
		/// <param name="requestStream"></param>
		/// <returns></returns>
		private Stream WrappedRequestStream(Stream requestStream)
		{
			if ( request_stream == null )
			{
				request_stream = requestStream;
				if(this.gzipRequest)
					request_stream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(request_stream);
			}
			return request_stream;
		}

		public override WebResponse GetResponse()
		{
			return new WebResponseWrapper(wr.GetResponse ());
		}
	
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return wr.BeginGetResponse (callback, state);
		}
	
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			return new WebResponseWrapper(wr.EndGetResponse (asyncResult));
		}
	}

	#endregion

	#region WebResponseWrapper Class implementation
	/// <summary>
	/// Summary description for WebResponseWrapper.
	/// </summary>
	public class WebResponseWrapper : WebResponse	
	{
		private WebResponse wr;
		private Stream response_stream = null;

		internal WebResponseWrapper(WebResponse wrapped) 
		{
			this.wr = wrapped;
		}

		/// <summary>
		/// Wrap the returned stream in a gzip uncompressor if needed
		/// </summary>
		/// <returns></returns>
		public override Stream GetResponseStream()
		{
			if ( response_stream == null )
			{
				response_stream = wr.GetResponseStream();
				if ( string.Compare(Headers["Content-Encoding"], "gzip", true) == 0 )
					response_stream = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(response_stream);
			}
			return response_stream;
		}

		// these all delegate to the contained WebResponse
		public override long ContentLength
		{
			get { return wr.ContentLength; }
			set { wr.ContentLength = value; }
		}
	
		public override string ContentType
		{
			get { return wr.ContentType; }
			set { wr.ContentType = value; }
		}
	
		public override Uri ResponseUri
		{
			get { return wr.ResponseUri; }
		}
	
		public override WebHeaderCollection Headers
		{
			get { return wr.Headers; }
		}
	}

	#endregion


}
// NAnt - A .NET build tool
// Copyright (C) 2001-2002 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

// Jay Turpin (jayturpin@hotmail.com)
// Gerry Shaw (gerry_shaw@yahoo.com)

using System;
using System.Globalization;
using System.IO;
using System.Net;

using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.Core.Util;

namespace NAnt.Core.Tasks {
    /// <summary>
    /// Gets a particular file from a URL source.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   Options include verbose reporting and timestamp based fetches.
    ///   </para>
    ///   <para>
    ///   Currently, only HTTP and UNC protocols are supported. FTP support may 
    ///   be added when more pluggable protocols are added to the System.Net 
    ///   assembly.
    ///   </para>
    ///   <para>
    ///   The <see cref="UseTimeStamp" /> option enables you to control downloads 
    ///   so that the remote file is only fetched if newer than the local copy. 
    ///   If there is no local copy, the download always takes place. When a file 
    ///   is downloaded, the timestamp of the downloaded file is set to the remote 
    ///   timestamp.
    ///   </para>
    ///   <note>
    ///   This timestamp facility only works on downloads using the HTTP protocol.
    ///   </note>
    /// </remarks>
    /// <example>
    ///   <para>
    ///   Gets the index page of the NAnt home page, and stores it in the file 
    ///   <c>help/index.html</c> relative to the project base directory.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <get src="http://nant.sourceforge.org/" dest="help/index.html" />
    ///     ]]>
    ///   </code>
    /// </example>
    /// <example>
    ///   <para>
    ///   Gets the index page of a secured web site using the given credentials, 
    ///   while connecting using the specified password-protected proxy server.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <get src="http://password.protected.site/index.html" dest="secure/index.html">
    ///     <credentials username="user" password="guess" domain="mydomain" />
    ///     <proxy host="proxy.company.com" port="8080">
    ///         <credentials username="proxyuser" password="dunno" />
    ///     </proxy>
    ///     ]]>
    ///   </code>
    /// </example>
    [TaskName("get")]
    public class GetTask : Task {
        #region Private Instance Fields

        private string _src;
        private string _dest;
        private string _httpProxy;
        private Proxy _proxy;
        private int _timeout = 100000;
        private bool _useTimeStamp;
        private Credential _credentials;

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// The URL from which to retrieve a file.
        /// </summary>
        [TaskAttribute("src", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Source {
            get { return _src; }
            set { _src = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// The file where to store the retrieved file.
        /// </summary>
        [TaskAttribute("dest", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Destination {
            get { return Project.GetFullPath(_dest); }
            set { _dest = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// If inside a firewall, proxy server/port information
        /// Format: {proxy server name}:{port number}
        /// Example: proxy.mycompany.com:8080 
        /// </summary>
        [TaskAttribute("httpproxy")]
        [Obsolete("Use the <proxy> child element instead.", false)]
        public string HttpProxy {
            get { return _httpProxy; }
            set { _httpProxy = value; }
        }

        /// <summary>
        /// The network proxy to use to access the Internet resource.
        /// </summary>
        [BuildElement("proxy")]
        public Proxy Proxy {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>
        /// The network credentials used for authenticating the request with 
        /// the Internet resource.
        /// </summary>
        [BuildElement("credentials")]
        public Credential Credentials {
            get { return _credentials; }
            set { _credentials = value; }
        }

        /// <summary>
        /// Log errors but don't treat as fatal. The default is <see langword="false" />.
        /// </summary>
        [TaskAttribute("ignoreerrors")]
        [Obsolete("Use the 'failonerror' attribute instead.")]
        [BooleanValidator()]
        public bool IgnoreErrors {
            get { return FailOnError; }
            set { FailOnError = value; }
        }

        /// <summary>
        /// Conditionally download a file based on the timestamp of the local 
        /// copy. HTTP only. The default is <see langword="false" />.
        /// </summary>
        [TaskAttribute("usetimestamp")]
        [BooleanValidator()]
        public bool UseTimeStamp {
            get { return _useTimeStamp; }
            set { _useTimeStamp = value; }
        }

        /// <summary>
        /// The length of time, in milliseconds, until the request times out.
        /// The default is <c>100000</c> milliseconds.
        /// </summary>
        [TaskAttribute("timeout")]
        [Int32Validator()]
        public int Timeout {
            get { return _timeout; }
            set { _timeout = value; }
        }

        #endregion Public Instance Properties

        #region Override implementation of Task

        /// <summary>
        /// Initializes task and ensures the supplied attributes are valid.
        /// </summary>
        /// <param name="taskNode">Xml node used to define this task instance.</param>
        protected override void InitializeTask(System.Xml.XmlNode taskNode) {
            if (Source == null) {
                throw new BuildException("src attribute is required.", Location);
            }

            if (Destination == null) {
                throw new BuildException("dest attribute is required.", Location);
            }

            if (Directory.Exists(Destination)) {
                throw new BuildException("Specified destination is a directory.", Location);
            }

            if (File.Exists(Destination) && (FileAttributes.ReadOnly == (File.GetAttributes(Destination) & FileAttributes.ReadOnly))) {
                throw new BuildException("Cannot write to " + Destination, Location);
            }

            if (Proxy != null && HttpProxy != null) {
                throw new BuildException("The <proxy> child element and the 'httpproxy' attribute are mutually exclusive.", Location);
            }
        }

        /// <summary>
        /// This is where the work is done 
        /// </summary>
        protected override void ExecuteTask() {
            try {
                //set the timestamp to the file date.
                DateTime fileTimeStamp = new DateTime();

                if (UseTimeStamp && File.Exists(Destination)) {
                    fileTimeStamp = File.GetLastWriteTime(Destination);
                    Log(Level.Verbose, LogPrefix + "Local file time stamp: {0}.", fileTimeStamp.ToString(CultureInfo.InvariantCulture));
                }

                //set up the URL connection
                WebRequest webRequest = GetWebRequest(Source, fileTimeStamp);
                WebResponse webResponse = webRequest.GetResponse();

                Stream responseStream = null;

                // Get stream
                // try three times, then error out
                int tryCount = 1;

                while (true) {
                    try {
                        responseStream = webResponse.GetResponseStream();
                        break;
                    } catch (IOException ex) {
                        if (tryCount > 3) {
                            throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                                "Unable to download '{0}' to '{1}'.", Source, Destination), Location);
                        } else {
                            Log(Level.Warning, LogPrefix + "Unable to open connection to '{0}' (try {1} of 3): " + ex.Message, Source, tryCount);
                        }
                    }
                
                    // increment try count
                    tryCount++;
                }

                // open file for writing
                BinaryWriter destWriter = new BinaryWriter(new FileStream(Destination, FileMode.Create));
                
                Log(Level.Info, LogPrefix + "Retrieving '{0}' to '{1}'.", Source, Destination);

                // Read in stream from URL and write data in chunks
                // to the dest file.
                int bufferSize = 100 * 1024;
                byte[] buffer = new byte[bufferSize];
                int totalReadCount = 0;
                int totalBytesReadFromStream = 0;
                int totalBytesReadSinceLastDot = 0;

                do {
                    totalReadCount = responseStream.Read(buffer, 0, bufferSize);
                    if ( totalReadCount != 0 ) { // zero means EOF
                        // write buffer into file
                        destWriter.Write(buffer, 0, totalReadCount);
                        // increment byte counters
                        totalBytesReadFromStream += totalReadCount;
                        totalBytesReadSinceLastDot += totalReadCount;
                        // display progress
                        if (Verbose && totalBytesReadSinceLastDot > bufferSize) {
                            if (totalBytesReadSinceLastDot == totalBytesReadFromStream) {
                                // TO-DO !!!!
                                //Log.Write(LogPrefix);
                            }
                            // TO-DO !!!
                            //Log.Write(".");
                            totalBytesReadSinceLastDot = 0;
                        }
                    }
                } while (totalReadCount != 0);

                if (totalBytesReadFromStream > bufferSize) {
                    Log(Level.Verbose, "");
                }
                Log(Level.Verbose, LogPrefix + "Number of bytes read: {0}.", totalBytesReadFromStream.ToString(CultureInfo.InvariantCulture));

                // clean up response streams
                destWriter.Close();
                responseStream.Close();

                //check to see if we actually have a file...
                if(!File.Exists(Destination)) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Unable to download '{0}' to '{1}'.", Source, Destination), Location);
                }

                //if (and only if) the use file time option is set, then the
                //saved file now has its timestamp set to that of the downloaded file
                if (UseTimeStamp)  {
                    // HTTP only
                    if (webRequest is HttpWebRequest) {
                        HttpWebResponse httpResponse = (HttpWebResponse) webResponse;

                        // get timestamp of remote file
                        DateTime remoteTimestamp = httpResponse.LastModified;

                        Log(Level.Verbose, LogPrefix + "{0} last modified on {1}.", Destination, remoteTimestamp.ToString(CultureInfo.InvariantCulture));
                        TouchFile(Destination, remoteTimestamp);
                    }
                }
            } catch (BuildException ex) {
                // rethrow exception
                throw ex;
            } catch (WebException ex) {
                // If status is WebExceptionStatus.ProtocolError,
                //   there has been a protocol error and a WebResponse
                //   should exist. Display the protocol error.
                if (ex.Status == WebExceptionStatus.ProtocolError) {
                    // test for a 304 result (HTTP only)
                    // Get HttpWebResponse so we can check the HTTP status code
                    HttpWebResponse httpResponse = (HttpWebResponse) ex.Response;
                    if (httpResponse.StatusCode == HttpStatusCode.NotModified) {
                        //not modified so no file download. just return instead
                        //and trace out something so the user doesn't think that the
                        //download happened when it didn't

                        Log(Level.Verbose, LogPrefix + "{0} not downloaded.  Not modified since {1}.", Destination, httpResponse.LastModified.ToString(CultureInfo.InvariantCulture));
                        return;
                    } else {
                        throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                            "Unable to download '{0}' to '{1}'.", Source, 
                            Destination), Location, ex);
                    }
                } else {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "Unable to download '{0}' to '{1}'.", Source, Destination), 
                        Location, ex);
                }
            } catch (Exception ex) {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                    "Unable to download '{0}' to '{1}'.", Source, Destination), Location, ex);
            }
        }

        #endregion Override implementation of Task

        #region Protected Instance Methods

        /// <summary>
        /// Sets the timestamp of a given file to a specified time.
        /// </summary>
        protected void TouchFile(string fileName, DateTime touchDateTime) {
            try {
                if (File.Exists(fileName)) {
                    Log(Level.Verbose, LogPrefix + "Touching file {0} with {1}.", fileName, touchDateTime.ToString(CultureInfo.InvariantCulture));
                    File.SetLastWriteTime(fileName, touchDateTime);
                } else {
                    throw new FileNotFoundException();
                }
            } catch (Exception e) {
                // swallow any errors and move on
                Log(Level.Verbose, LogPrefix + "Error: {0}.", e.ToString());
            }
        }

        #endregion Protected Instance Methods

        #region Private Instance Methods

        private WebRequest GetWebRequest(string url, DateTime fileLastModified) {
            WebRequest webRequest = null;
            Uri uri = new Uri(url);

            // conditionally determine type of connection
            // if HTTP, cast to an HttpWebRequest so that IfModifiedSince can be set
            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) {
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(uri);

                //modify the headers
                if (!fileLastModified.Equals(new DateTime())) {
                    // When IfModifiedSince is set, it internally converts the local time
                    // to UTC (or, for us old farts, GMT). For all locations behind UTC
                    // (US and Canada), this causes the IfModifiedSince time to always be
                    // set to a time earlier than the file timestamp and force the file
                    // to be fetched, even if it hasn't changed. The UtcOffset is used to
                    // counter this behavior and a second is added for good measure.

                    TimeSpan timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                    DateTime gmtTime = fileLastModified.AddSeconds(1).Subtract(timeSpan);
                    httpRequest.IfModifiedSince = gmtTime;

                    //REVISIT: at this point even non HTTP connections may support the if-modified-since
                    //behaviour -we just check the date of the content and skip the write if it is not
                    //newer. Some protocols (FTP) dont include dates, of course.
                }

                webRequest = httpRequest;
            } else {
                webRequest = WebRequest.Create(uri);
            }

            // set the number of milliseconds that the request will wait 
            // for a response
            webRequest.Timeout = Timeout;

            // configure proxy settings
            if (Proxy != null) {
                webRequest.Proxy = Proxy.GetWebProxy();
            } else if (HttpProxy != null) {
                webRequest.Proxy = new WebProxy(HttpProxy);
            }

            // set authentication information
            if (Credentials != null) {
                webRequest.Credentials = Credentials.GetCredential();
            }

            return webRequest;
        }

        #endregion Private Instance Methods
    }
}


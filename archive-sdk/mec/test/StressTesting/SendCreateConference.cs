using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Samoa.Core;
using Metreos.Mec.WebMessage;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for SendCreateConference.
	/// </summary>
    public class SendCreateConference 
    {
        HttpWebRequest request;
        string content;
        public const string CREATE_COMMAND = "/conference/create";

        // This Send Create Conference makes random suffixes for address of the remote party
        public SendCreateConference(string appServerIp, string remotePartyNumber)
        {
            request = (HttpWebRequest) HttpWebRequest.Create("http://" + appServerIp + ":8000" + CREATE_COMMAND);

            conferenceRequestType createMsg = new conferenceRequestType();
            locationType location = new locationType();

            location.address = remotePartyNumber;
            location.description = "MecStressTest:Create";
            createMsg.location = new locationType[1];
            createMsg.location[0] = location;
            createMsg.type = conferenceRequestTypeType.create;
            
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, createMsg);
            writer.Close();

            serializer = null;
            writer = null;

            content = sb.ToString();

            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = content.Length;
            request.TransferEncoding = null;
            request.KeepAlive = false;     
        }
        

        public HttpWebResponse Send()
        {
            Stream writeStream;
            try
            {
                writeStream = request.GetRequestStream();
            }
            catch(Exception)
            {
                return null;
            }
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
            writeStream.Write(buffer, 0, buffer.Length);
            writeStream.Close();
            writeStream = null;

            WebResponse response = null;

            try
            {
                response = request.GetResponse();
            }
            catch(Exception)
            {
                request = null;
                return null;
            }

            request = null;

            return (HttpWebResponse) response;
        }
    }
}

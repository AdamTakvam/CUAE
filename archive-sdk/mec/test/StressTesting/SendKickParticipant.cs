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
	/// Summary description for SendKickParticipant.
	/// </summary>
    public class SendKickParticipant 
    {
        HttpWebRequest request;
        string content;
        public const string KICK_COMMAND = "/conference/kick";

        public SendKickParticipant(string appServerIp, string locationId, string sessionId)
        {
            request = (HttpWebRequest) HttpWebRequest.Create("http://" + appServerIp + ":8000" + KICK_COMMAND);

            conferenceRequestType kickMsg = new conferenceRequestType();
            locationType location = new locationType();

            location.Value = locationId;
            kickMsg.location = new locationType[1];
            kickMsg.location[0] = location;
            kickMsg.type = conferenceRequestTypeType.kick;
            
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, kickMsg);
            writer.Close();

            serializer = null;
            writer = null;

            content = sb.ToString();

            request.Headers.Add("Metreos-SessionID", sessionId);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = content.Length;
            request.TransferEncoding = null;
            request.KeepAlive = false;
        }

        public HttpWebResponse Send()
        {
            Stream writeStream = request.GetRequestStream();
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

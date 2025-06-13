using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using System.Web;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Common.Mec;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for AsynchronousHttp.
	/// </summary>
	public class AsynchronousHttp
	{
        public const string HEADER_SESSION_ID = "metreosSessionId";
        public const string CREATE_COMMAND = "/conference/create";
        public const string JOIN_COMMAND = "/conference/join";
        public const string KICK_COMMAND = "/conference/kick";
        public const string CHECK_CONFERENCE = "/conference/isConferenceActive";
        public const string CHECK_JOIN = "/conference/isLocationOnline";
        public const string MUTE_COMMAND ="/conference/mute";

        public IHttpCallbacks httpInterface;
        public IMecTester testInterface;
        public StressTesting.Settings settings;
		public AsynchronousHttp(IHttpCallbacks httpInterface, IMecTester testInterface, StressTesting.Settings settings)
		{
			this.httpInterface = httpInterface;
            this.testInterface = testInterface;
            this.settings = settings;
		}
        #region Send Routines


        public bool Initialize(string locationGuid, string phoneNumber, string remotePartyNumber)
        {
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(phoneNumber != null, "Phone Number is null");

            State state = new State();
            state.locationGuid = locationGuid;
    
            testInterface.UpdateVerboseInfo("Create conference with phone number " + phoneNumber);

            try
            {
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + CREATE_COMMAND);
            }
            catch(Exception e)
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                testInterface.UpdateErrorInfo("Error reported was: " + e.ToString());
                return false;
            }
            conferenceRequestType createMsg = new conferenceRequestType();
            locationType location = new locationType();

            // this is just the cisco-style number
            state.remotePartyNumber = phoneNumber;

            // this is the full number, with the callGen Ip appended or CallManager Ip appended
            location.address = remotePartyNumber;
            location.description = "MecStressTest:Create";
            createMsg.location = new locationType[1];
            createMsg.location[0] = location;
            createMsg.type = conferenceRequestTypeType.create;

            StringBuilder sb;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
                sb = new System.Text.StringBuilder();
                System.IO.TextWriter writer = new System.IO.StringWriter(sb);
                serializer.Serialize(writer, createMsg);
                writer.Close();

                serializer = null;
                writer = null;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            

            state.content = sb.ToString();

            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;     
         
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleCreateResponse), state);
            
            return true;
        }

        public bool AddConnection(string sessionId, string locationGuid, string remotePartyNumber, string phoneNumber, string conferenceId)
        {
            Debug.Assert(phoneNumber != null, "Phone Number is null");
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(conferenceId != null, "conferenceId is null");

            State state = new State();
            state.locationGuid = locationGuid;

            try
            {
                testInterface.UpdateVerboseInfo("Location joining in conference " + conferenceId);
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + JOIN_COMMAND);
            }
            catch
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                return false;
            }
                
            conferenceRequestType joinMsg = new conferenceRequestType();
            locationType location = new locationType();

            state.remotePartyNumber = phoneNumber;

            location.address = remotePartyNumber;
            location.description = "MecStressTest:Join";
            joinMsg.location = new locationType[1];
            joinMsg.location[0] = location;
            joinMsg.type = conferenceRequestTypeType.join;

            System.Text.StringBuilder sb;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
                sb = new System.Text.StringBuilder();
                System.IO.TextWriter writer = new System.IO.StringWriter(sb);
                serializer.Serialize(writer, joinMsg);
                writer.Close();

                serializer = null;
                writer = null;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            

            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleJoinResponse), state);
            
            return true;
        }

        // REFACTOR: make it so random connections dont hit on pending ones


        public bool RemoveConnection(string sessionId, string locationGuid, string locationId, string conferenceId)
        { 
            Debug.Assert(locationGuid != null, "locationGuid is null");
            Debug.Assert(locationId != null, "locationId is null");
            Debug.Assert(sessionId != null, "sessionId is null");
            Debug.Assert(conferenceId != null, "conferenceId is null");

            State state = new State();
            state.locationGuid = locationGuid;
            state.locationId = locationId;

            try
            {
                testInterface.UpdateVerboseInfo("Kicking location " + locationId + " in conference " + conferenceId);
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + KICK_COMMAND);
            }
            catch
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                return false;
            }
                
            conferenceRequestType kickMsg = new conferenceRequestType();
            locationType location = new locationType();

            location.Value = locationId;
            kickMsg.location = new locationType[1];
            kickMsg.location[0] = location;
            kickMsg.type = conferenceRequestTypeType.kick;
            
            StringBuilder sb;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
                sb = new System.Text.StringBuilder();
                System.IO.TextWriter writer = new System.IO.StringWriter(sb);
                serializer.Serialize(writer, kickMsg);
                writer.Close();

                serializer = null;
                writer = null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }


            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                testInterface.UpdateVerboseInfo("!!SENDING KICK REQUEST!! " + locationId + " " + DateTime.Now.ToLongTimeString());
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            // ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleKickResponse), state);
            
            return true;
        }

        public bool MuteConnection(string sessionId, string locationGuid, string locationId, string conferenceId)
        { 
            Debug.Assert(conferenceId != null, "conferenceId is null");
            State state = new State();
            state.locationGuid = locationGuid;
            state.locationId = locationId;


            try
            {
                testInterface.UpdateVerboseInfo("Muting location " + locationId + " in conference" + conferenceId);
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + MUTE_COMMAND);
            }
            catch
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                return false;
            }
                
            conferenceRequestType muteMsg = new conferenceRequestType();
            locationType location = new locationType();

            location.Value = locationId;
            muteMsg.location = new locationType[1];
            muteMsg.location[0] = location;
            muteMsg.type = conferenceRequestTypeType.mute;

            StringBuilder sb;
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
                sb = new System.Text.StringBuilder();
                System.IO.TextWriter writer = new System.IO.StringWriter(sb);
                serializer.Serialize(writer, muteMsg);
                writer.Close();
                serializer = null;
                writer = null;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            // ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleMuteResponse), state);
            
            return true;
        }

        public bool CheckConference(string sessionId, string locationGuid, string locationId, string remotePartyNumber, int count)
        {
            State state = new State();
            state.count = count;
            state.locationId = locationId;
            state.locationGuid = locationGuid;
            state.remotePartyNumber = remotePartyNumber;

            try
            {
                //testInterface.UpdateVerboseInfo("CHECK CREATE CONFERENCE: send");
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + CHECK_CONFERENCE);
            }
            catch
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                return false;
            }
                
            conferenceRequestType checkConferenceMsg = new conferenceRequestType();

            checkConferenceMsg.type = conferenceRequestTypeType.isConferenceActive;
            
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, checkConferenceMsg);
            writer.Close();

            serializer = null;
            writer = null;

            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleCheckConferenceResponse), state);
            
            return true;
        }

        public bool CheckJoined(string sessionId, string locationGuid, string locationId, string remotePartyNumber, int count)
        {
            State state = new State();
            state.count = count++;
            state.locationGuid = locationGuid;
            state.locationId = locationId;
            state.remotePartyNumber = remotePartyNumber;

            try
            {
                //testInterface.UpdateVerboseInfo("CHECK JOIN CONFERENCE: send");
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + CHECK_JOIN);
            }
            catch
            {
                testInterface.UpdateErrorInfo("Check IP of Application Server or start Application Server");
                return false;
            }
                

            conferenceRequestType checkJoinMsg = new conferenceRequestType();
            checkJoinMsg.type = conferenceRequestTypeType.isLocationOnline;

            locationType location = new locationType();
            location.Value = locationId;
            location.address = remotePartyNumber.ToString();
            location.description = "MecStressTest:isLocationActive";
            checkJoinMsg.location = new locationType[1];
            checkJoinMsg.location[0] = location;
            
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, checkJoinMsg);
            writer.Close();

            serializer = null;
            writer = null;

            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch
            {
                return false;
            }

            //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleCheckJoinResponse), state);
            
            return true;
        }

        public bool CheckKicked(string sessionId, string locationGuid, string locationId, int count)
        {
            State state = new State();
            state.count = count++;
                
            state.locationId = locationId;
            state.locationGuid = locationGuid;

            try
            {
                //testInterface.UpdateVerboseInfo("CHECK KICK: send");
                state.request = (HttpWebRequest) HttpWebRequest.Create("http://" + settings.appServerIp + ":8000" + CHECK_JOIN);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
                
            conferenceRequestType checkKickMsg = new conferenceRequestType();
            checkKickMsg.type = conferenceRequestTypeType.isLocationOnline;

            locationType location = new locationType();
            location.Value = locationId;
            location.description = "MecStressTest:isLocationActive";
            checkKickMsg.location = new locationType[1];
            checkKickMsg.location[0] = location;
            
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, checkKickMsg);
            writer.Close();

            serializer = null;
            writer = null;

            state.content = sb.ToString();

            state.request.Headers.Add("Metreos-SessionID", sessionId);
            state.request.Method = "POST";
            state.request.ContentType = "text/xml";
            state.request.ContentLength = state.content.Length;
            state.request.TransferEncoding = null;
            state.request.KeepAlive = false;
                           
            System.IAsyncResult result;

            try
            {
                result = state.request.BeginGetRequestStream(new AsyncCallback( GrabAndSendToWriteStream ), state);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), state.request, TIMEOUT, true);
            IAsyncResult resultFromResponse = (IAsyncResult) state.request.BeginGetResponse( new AsyncCallback(HandleCheckKickResponse), state);
            
            return true;
        }

        #endregion Send Routines

        #region More Async Http Stuff
        public void GrabAndSendToWriteStream(IAsyncResult asynchronousResult)
        {
            try
            {
                State state = (State) asynchronousResult.AsyncState;
                state.writeStream = state.request.EndGetRequestStream(asynchronousResult);
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(state.content.ToString());
                state.writeStream.Write(buffer, 0, buffer.Length);
                state.writeStream.Close();
                state.writeStream = null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion More Async Http Stuff

        #region ReadCallbacks

        private void ReadCallBackCreate(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCreate), state);
                    return;
                }
                else
                {
                        
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        conferenceResponseType rData;

                        stringContent = state.requestData.ToString();

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            body.Close(); 
                            state.responseStream.Close();
                        }
                        catch
                        {
                            return;
                        }

                        WebMessage.typeType typeOfConference = rData.type;
                        WebMessage.resultType result = rData.result;
                        WebMessage.locationIdType[] locationId = rData.locationId;

                        httpInterface.AssignConferenceId(rData.conferenceId);
                  
                        // Sanity check.  Asked for a create, should receive a create
                        if(typeOfConference != typeType.create)
                        {
                            return;
                        }          
            
                        // Make sure that the connection was able to be established
                        if(result == resultType.success)
                        {       
                            httpInterface.AssignSessionId(state.response.Headers[HEADER_SESSION_ID]);  
                            testInterface.UpdateVerboseInfo("Conference " + rData.conferenceId + " has been successfully created");
                        }
                        else
                        {
                            httpInterface.CreateConferenceFailure(state.locationGuid);
                            testInterface.UpdateErrorInfo("Failed to create conference with phone number " + state.remotePartyNumber);
                            return;
                        }

                        // Make sure database is up to snuff
                        if(locationId[0] == null)
                        {
                            return;
                        }

                        // Last minute check that the create conference really succeeded

                        if(settings.errorChecking)
                        {
                            CheckForConferenceExistence(state.response.Headers[HEADER_SESSION_ID],  state.locationGuid, locationId[0].Value, state.remotePartyNumber, 0);
                        }
                        else
                        {
                            httpInterface.CreateConferenceSuccess(state.locationGuid, locationId[0].Value, state.remotePartyNumber);
                        }
                    }                    
                        
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackCheckJoin\n" + e.ToString());
            }
        }

        private void ReadCallBackJoin(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackJoin), state);
                    return;
                }
                else
                {
                    //Console.WriteLine("\nThe contents of the Html page are : ");
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = state.requestData.ToString();
                        conferenceResponseType rData;

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            body.Close(); 
                            state.responseStream.Close();
                        }
                        catch
                        {
                            return;
                        }
            
                        WebMessage.typeType typeOfConference = rData.type;
                        WebMessage.resultType result = rData.result;
                        WebMessage.locationIdType[] locationId = rData.locationId;
            
                        //Sanity check.  Asked for a join, should receive a join message
                        if(typeOfConference != typeType.join)
                        {
                            return;
                        }

                        // Make sure that the connection was able to be established
                        if(result != resultType.success)
                        { 
                            httpInterface.JoinConferenceFailure(state.locationGuid); 
                            testInterface.UpdateErrorInfo("Failed to join a location to conference");
                            return;
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("Joined connection to conference");
                        }

                        // Make sure database is up to snuff
                        if(locationId[0] == null)
                        {
                            return;
                        }

                        // Last minute check that the join really succeeded
                        if(settings.errorChecking)
                        {
                            CheckForJoin(state.response.Headers[HEADER_SESSION_ID], state.locationGuid, locationId[0].Value, state.remotePartyNumber, 0);          
                        }
                        else
                        {
                            httpInterface.JoinConferenceSuccess(state.locationGuid, locationId[0].Value, state.remotePartyNumber);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackJoin\n" + e.ToString());
            }
        }

        private void ReadCallBackKick(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackKick), state);
                    return;
                }
                else
                {
                    //Console.WriteLine("\nThe contents of the Html page are : ");
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = state.requestData.ToString();
                        conferenceResponseType rData;

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            state.responseStream.Close();
                            body.Close();
                        }
                        catch
                        {
                            return;
                        }
            
                        WebMessage.typeType typeOfConference = rData.type;
                        WebMessage.resultType result = rData.result;
                        WebMessage.locationIdType[] locationIdReturned = rData.locationId;
  
                        //Sanity check.  Asked for a kick, should receive a kick message
                        if(typeOfConference != typeType.kick)
                        {
                            return;
                        }

                        // Last minute check that the kick really succeeded
                        if(result != resultType.success)
                        {
                            testInterface.UpdateErrorInfo("KICK LOCATION: failure");
                            httpInterface.KickConferenceFailure(state.locationGuid, state.locationId);
                            return;
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("KICK LOCATION: success");
                        }

                        if(settings.errorChecking)
                        {
                            CheckForKick(state.response.Headers[HEADER_SESSION_ID], state.locationGuid, state.locationId, 0);
                        }
                        else
                        {
                            httpInterface.KickConferenceSuccess(state.locationGuid, state.locationId);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackKick\n" + e.ToString());
            }
        }

        private void ReadCallBackMute(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackMute), state);
                    return;
                }
                else
                {
                    //Console.WriteLine("\nThe contents of the Html page are : ");
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = state.requestData.ToString();
                        conferenceResponseType rData;

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            state.responseStream.Close();
                            body.Close();
                        }
                        catch
                        {
                            return;
                        }
            
                        WebMessage.typeType typeOfConference = rData.type;
                        WebMessage.resultType result = rData.result;
                        WebMessage.locationIdType[] locationIdReturned = rData.locationId;
  
                        //Sanity check.  Asked for a kick, should receive a kick message
                        if(typeOfConference != typeType.mute)
                        {
                            return;
                        }

                        // Last minute check that the kick really succeeded
                        if(result != resultType.success)
                        {
                            testInterface.UpdateErrorInfo("MUTE PARTICIPANT: failure");
                            httpInterface.MuteConnectionFailure(state.locationGuid, state.locationId);
                            return;
                        }
                        else
                        {
                            testInterface.UpdateVerboseInfo("MUTE PARTICIPANT: success");
                            httpInterface.MuteConnectionSuccess(state.locationGuid, state.locationId);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackMute\n" + e.ToString());
            }
        }

        private void ReadCallBackCheckConference(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckConference), state);
                    return;
                }
                else
                {
                        
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        conferenceResponseType rData;

                        stringContent = state.requestData.ToString();

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            body.Close(); 
                            state.responseStream.Close();

                        }
                        catch
                        {
                            return;
                        }
                  
                        WebMessage.resultType result = rData.result;

                        // Make sure that the connection was able to be established
                        if(result == resultType.success)
                        {  
                            double ratio = (System.Double.Parse(settings.createTimeout))/(System.Double.Parse(settings.createPoll));
                            
                            double lastTime = Math.Ceiling(ratio);

                            if(state.count <= (int)lastTime)
                            {
                                this.CheckForConferenceExistence(state.response.Headers[HEADER_SESSION_ID], state.locationGuid, state.locationId, state.remotePartyNumber, state.count);
                            }
                            else
                            {
                                httpInterface.CreateConferenceSuccess(state.locationGuid, state.locationId, state.remotePartyNumber);
                               
                                testInterface.UpdateVerboseInfo("CHECK CREATE CONFERENCE: success");
                            }

                            
                        }
                        else
                        {
                            httpInterface.CreateConferenceFailure(state.locationGuid);

                            testInterface.UpdateErrorInfo("CHECK CREATE CONFERENCE: failure");
                        }                          
                    }                    

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackCheckConference\n" + e.ToString());
            }
        }

        private void ReadCallBackCheckJoin(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckJoin), state);
                    return;
                }
                else
                {
                        
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        conferenceResponseType rData;

                        stringContent = state.requestData.ToString();

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            body.Close(); 
                            state.responseStream.Close();
                        }
                        catch
                        {
                            return;
                        }

                        WebMessage.resultType result = rData.result;
 
                        // Make sure that the connection was able to be established
                        if(result == resultType.success)
                        {
                            double ratio = (System.Double.Parse(settings.joinTimeout))/(System.Double.Parse(settings.joinPoll));
                            
                            double lastTime = Math.Ceiling(ratio);

                            if(state.count <= (int)lastTime)
                            {
                                this.CheckForJoin(state.response.Headers[HEADER_SESSION_ID], state.locationGuid, state.locationId, state.remotePartyNumber, state.count);
                            }
                            else
                            {
                                httpInterface.JoinConferenceSuccess(state.locationGuid, state.locationId, state.remotePartyNumber);
                                
                                testInterface.UpdateVerboseInfo("CHECK JOIN CONFERENCE: success");

                            }

                            
                        }
                        else
                        {
                            httpInterface.JoinConferenceFailure(state.locationGuid);

                            testInterface.UpdateErrorInfo("CHECK JOIN CONFERENCE: failure");
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackCheckJoin\n" + e.ToString());
            }
        }

        private void ReadCallBackCheckKick(IAsyncResult asyncResult)
        {
            State state = (State) asyncResult.AsyncState;

            try
            {
                int read = state.responseStream.EndRead( asyncResult );
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckKick), state);
                    return;
                }
                else
                {
                        
                    if(state.requestData.Length > 1)
                    {
                        string stringContent;
                        conferenceResponseType rData;

                        stringContent = state.requestData.ToString();

                        try
                        {
                            StringReader body = new StringReader(stringContent);
                            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                            rData = (conferenceResponseType) serializer.Deserialize(body);
                            body.Close(); 
                            state.responseStream.Close();
                        }
                        catch
                        {
                            return;
                        }

                        WebMessage.resultType result = rData.result;
            
                        // Make sure that the connection was able to be established
                        if(result != resultType.success)
                        {       
                            httpInterface.KickConferenceSuccess(state.locationGuid, state.locationId);

                            testInterface.UpdateVerboseInfo("CHECK KICK CONFERENCE: success");
                        }
                        else
                        {
                            double ratio = (System.Double.Parse(settings.createTimeout))/(System.Double.Parse(settings.createPoll));
                            
                            double lastTime = Math.Ceiling(ratio);

                            if(state.count <= (int)lastTime)
                            {
                                this.CheckForKick(state.response.Headers[HEADER_SESSION_ID], state.locationGuid, state.locationId, state.count);
                            }
                            else
                            {
                                httpInterface.KickConferenceFailure(state.locationGuid, state.locationId);
                                testInterface.UpdateErrorInfo("CHECK KICK CONFERENCE: failure");
                            }
                        }
                    }                          
                }
            }
             catch(Exception e)
            {
                Console.WriteLine("Exception in ReadCallBackCheckKick\n" + e.ToString());
            }
        }

        #endregion ReadCallbacks

        #region HandleResponses

        private void HandleCreateResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCreate), state);
                return;
            }
            catch(WebException e)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}",e.Message);
                Console.WriteLine("\nStatus:{0}",e.Status);
            }
            catch(System.IO.IOException e)
            {
                Console.WriteLine("\nHandleResponse Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nFullString:{0}", e.ToString());
            }
        }

        private void HandleJoinResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackJoin), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.Join404(state.locationGuid);
            }
        }

        private void HandleKickResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackKick), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.Kick404(state.locationGuid);
            }
        }

        private void HandleMuteResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackMute), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.Mute404(state.locationGuid);
            }
        }

        public void HandleCheckConferenceResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckConference), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.CheckCreate404();
            }
        }

        public void HandleCheckJoinResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckJoin), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.CheckJoin404();
            }
        }

        public void HandleCheckKickResponse(System.IAsyncResult result)
        {
            State state = (State) result.AsyncState;

            try
            {
                state.response = (HttpWebResponse) state.request.EndGetResponse(result);
      
                state.responseStream = state.response.GetResponseStream();
      
                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = state.responseStream.BeginRead(state.bufferRead, 0, state.BUFFER_SIZE, new AsyncCallback(ReadCallBackCheckKick), state);
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                httpInterface.CheckKick404();
            }
        }

        #endregion HandleResponse

        #region Mec Error Checking

        public void CheckForConferenceExistence(string sessionId, string locationGuid, string locationId, string remotePartyNumber, int count)
        {
            httpInterface.CheckConferenceDatabase(sessionId, locationGuid, locationId, remotePartyNumber);

//            try
//            {
//                Thread.Sleep(Int32.Parse(settings.createPoll));
//
//            }
//            catch
//            {
//                return false;
//            }
//
//            count++;
//            if(!CheckConference(sessionId, locationGuid, locationId, remotePartyNumber, count))
//                return false;
//
//            return true;
        }

        public void CheckForJoin(string sessionId, string locationGuid, string locationId, string remotePartyNumber, int count)
        {
            httpInterface.CheckJoinDatabase(sessionId, locationGuid, locationId, remotePartyNumber);
//            try
//            {
//                Thread.Sleep(Int32.Parse(settings.joinPoll));
//            }
//            catch
//            {
//                return false;
//            }
//
//            count++;
//            if(!CheckJoined(sessionId, locationGuid, locationId, remotePartyNumber, count))
//                return false;
//
//            return true;
        }

        public void CheckForKick(string sessionId, string locationGuid, string locationId, int count)
        {
            httpInterface.CheckKickDatabase(sessionId, locationGuid, locationId);
//            try
//            {
//                Thread.Sleep(Int32.Parse(settings.kickPoll));
//            }
//            catch
//            {
//                return false;
//            }
//
//            count++;
//            if(!CheckKicked(sessionId, locationGuid, locationId, count))
//                return false;       
//      
//            return true;
        }

        #endregion Mec Error Checking

        public class State
        { 
            // This class stores the State of the request.
            public int BUFFER_SIZE = 1024;
            public StringBuilder requestData;
            public byte[] bufferRead;
            public string content;
            public HttpWebRequest request;
            public HttpWebResponse response;
            public Stream responseStream;
            public Stream writeStream;
            public string locationId;
            public string locationGuid;
            public string remotePartyNumber;
            public int count;
            //public conferenceRequestType serializableRequestMsg;

            public State()
            {
                bufferRead = new byte[BUFFER_SIZE];
                requestData = new StringBuilder("");
                request = null;
                responseStream = null;
            }
        }

	}
}

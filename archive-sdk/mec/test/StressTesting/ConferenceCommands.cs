using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;
using System.Net.Sockets;

using Metreos.Samoa.Core;
using Metreos.Mec.WebMessage;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for SendCreateConference.
	/// </summary>
    public class ConferenceCommand
    {
        public string APP_SERVER_IP;
        public string CALL_GEN_IP;
        public bool useSimClient;
        public bool errorChecking;
        public string callManagerIp;
        public int initialPause;

        public ConferenceCommand(string appServerIp, string callGenIp,  bool useSimClient, string callManagerIp, bool errorChecking, string initialPause)
        {
            this.APP_SERVER_IP = appServerIp;
            this.CALL_GEN_IP = callGenIp;
            this.callManagerIp = callManagerIp;
            this.useSimClient = useSimClient;
            this.errorChecking = errorChecking;
            try
            {
                this.initialPause = Int32.Parse(initialPause);
            }
            catch
            {
                this.initialPause = 5000;
            }  
        }

        public bool CreateConference(ArrayList sessionsListSync, ArrayList locationIdListListSync, MecStressTest mecStressTest, int phoneNumber, ref string locationIdReturn, ref string conferenceIdReturn)
        {
            conferenceResponseType rData;

            SendCreateConference scc;

            string sessionId; 

            if(useSimClient)
            {
                scc = new SendCreateConference(APP_SERVER_IP, phoneNumber.ToString() + "@" + callManagerIp);
            }
            else
            {
                scc = new SendCreateConference(APP_SERVER_IP, DateTime.Now.Ticks.ToString() + '@' + CALL_GEN_IP);
            }

            HttpWebResponse createResponse = scc.Send();
  
            if(createResponse == null)
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.  Not adding conference";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(createResponse.GetResponseStream());

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                createResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationId = rData.locationId;
            string conferenceId = rData.conferenceId;
                  
            // Sanity check.  Asked for a create, should receive a create
            if(typeOfConference != typeType.create)
            {
                mecStressTest.ErrorOutputText = "\nA non-create message received";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }          
            
            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {       
                //Add Location id retrieved for first location to the list of locationIds
                // Parse out sessionId, and put it into an ArrayList of sessions
                ArrayList tempLocationIdList = new ArrayList();

                sessionId = createResponse.Headers[HttpMessage.HEADER_SESSION_ID];

                sessionsListSync.Add(sessionId);
                tempLocationIdList.Add(locationId[0].Value);
                locationIdListListSync.Add(tempLocationIdList);
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nThe create command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure database is up to snuff
            if(locationId[0] != null)
            {
                locationIdReturn = locationId[0].Value;
                DatabaseComparisions.CheckLocationId(locationId[0].Value);  
            }
            else
            {
                locationIdReturn = "[None Returned]";
                mecStressTest.ErrorOutputText = "\nLocation id is null";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            if(conferenceId != null)
            {
                conferenceIdReturn = conferenceId;
                DatabaseComparisions.CheckConferenceId(conferenceId);
            }
            else
            {
                conferenceIdReturn = "[None Returned]";
                mecStressTest.ErrorOutputText = "\nConference id is null";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Last minute check that the create conference really succeeded

            if(errorChecking)
            {
                return CheckForConferenceExistence(sessionId, mecStressTest);
            }
            else
            {
                return true;
            }

        }

        public bool CreateJoin(ArrayList sessionsListSync, ArrayList locationIdListListSync, MecStressTest mecStressTest, int phoneNumber, int sessionNumber, ref string locationIdReturn, ref string conferenceIdReturn)
        {
            conferenceResponseType rData;

            SendJoinConference sjc;

            if(useSimClient)
            {
                sjc = new SendJoinConference(APP_SERVER_IP, phoneNumber.ToString() + "@" + callManagerIp, (string) sessionsListSync[sessionNumber]);
            }
            else
            {
                sjc = new SendJoinConference(APP_SERVER_IP, DateTime.Now.Ticks.ToString() + '@' + CALL_GEN_IP, (string) sessionsListSync[sessionNumber]);
            }

            HttpWebResponse joinResponse = sjc.Send();

            // Parse out sessionId
            if(joinResponse != null)
            {
                if(( (string) sessionsListSync[sessionNumber]) != joinResponse.Headers[HttpMessage.HEADER_SESSION_ID])
                {
                    mecStressTest.ErrorOutputText = "\nSession id doesn't match up. sessionId from create comand: " + (string) sessionsListSync[sessionNumber] + ". Session id from join command is: " + joinResponse.Headers[HttpMessage.HEADER_SESSION_ID];
                    mecStressTest.errorOutput.ScrollToCaret();
                    return false;
                }
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.";
                return false;
            }
            

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(joinResponse.GetResponseStream());
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                joinResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationId = rData.locationId;
            string conferenceId = rData.conferenceId;
  
            //Add Location id retrieved for first location to the list of locationIds
            ArrayList tempLocationIdList = (ArrayList) locationIdListListSync[sessionNumber];
            

            //Sanity check.  Asked for a join, should receive a join message
            if(typeOfConference != typeType.join)
            {
                mecStressTest.ErrorOutputText = "\nReceived a non-join message";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {
                // Update app-side lists of which locations are present
                tempLocationIdList.Add(locationId[0].Value);
            }
            else
            {   
                mecStressTest.ErrorOutputText = "\nThe join command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure database is up to snuff
            if(locationId[0] != null)
            {
                locationIdReturn = locationId[0].Value;
                DatabaseComparisions.CheckLocationId(locationId[0].Value);
            }
            else
            {
                locationIdReturn = "[None returned]";
                mecStressTest.ErrorOutputText = "\nLocation id is null";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            if(conferenceId != null)
            {
                conferenceIdReturn = conferenceId;
                DatabaseComparisions.CheckConferenceId(conferenceId);
            }
            else
            {
                conferenceIdReturn = "[None returned]";
                mecStressTest.ErrorOutputText = "\nConference id is null";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Last minute check that the join really succeeded
            if(errorChecking)
            {
                return CheckForJoin((string) sessionsListSync[sessionNumber], locationId[0].Value, mecStressTest);
            }
            else
            {
                return true;
            }
           
        }

        public bool  KickParticipant(ArrayList sessionsListSync, ArrayList locationIdListListSync, MecStressTest mecStressTest, string locationId, int conferenceIteration)
        {
            conferenceResponseType rData;

            SendKickParticipant sjc = new SendKickParticipant(APP_SERVER_IP, locationId, (string) sessionsListSync[conferenceIteration]);
            HttpWebResponse kickResponse = sjc.Send();

            // Parse out sessionId
            if(kickResponse != null)
            {
                if(((string) sessionsListSync[conferenceIteration] ) != kickResponse.Headers[HttpMessage.HEADER_SESSION_ID])
                {
                    mecStressTest.ErrorOutputText = "\nSession id doesn't match up. sessionId from create comand: " + (string) sessionsListSync[conferenceIteration] + ". Session id from kick command is: " + kickResponse.Headers[HttpMessage.HEADER_SESSION_ID];
                    mecStressTest.errorOutput.ScrollToCaret();
                    return false;
                }
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(kickResponse.GetResponseStream());
            

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                kickResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationIdReturned = rData.locationId;
            string conferenceId = rData.conferenceId;
  
            //Sanity check.  Asked for a kick, should receive a kick message
            if(typeOfConference != typeType.kick)
            {
                mecStressTest.ErrorOutputText = "\nReceived a non-kick message";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {
            }
            else
            {   
                mecStressTest.ErrorOutputText = "\nThe kick command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            // Last minute check that the join really succeeded
            if(errorChecking)
            {
                return CheckForKick((string) sessionsListSync[conferenceIteration], locationId, mecStressTest);
            }
            else
            {
                return true;
            }
            
           
        }  

        public bool CheckConference(string sessionId, MecStressTest mecStressTest)
        {
            conferenceResponseType rData;

            SendCheckConference scc = new SendCheckConference(APP_SERVER_IP, sessionId);
            HttpWebResponse checkConferenceResponse = scc.Send();

            // Parse out sessionId
            if(checkConferenceResponse != null)
            {
                if((sessionId ) != checkConferenceResponse.Headers[HttpMessage.HEADER_SESSION_ID])
                {
                    mecStressTest.ErrorOutputText = "\nSession id doesn't match up for the conference check";
                    mecStressTest.errorOutput.ScrollToCaret();
                    return false;
                }
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(checkConferenceResponse.GetResponseStream());
            

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                checkConferenceResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationIdReturned = rData.locationId;
            string conferenceId = rData.conferenceId;
  

            if(typeOfConference != typeType.isConferenceActive)
            {
                mecStressTest.ErrorOutputText = "\nReceived a non-kick message";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {
                return true;
            }
            else
            {   
                mecStressTest.ErrorOutputText = "\nThe check conference command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

        } 
 
        public bool CheckJoined(string sessionId, string locationId, MecStressTest mecStressTest)
        {
            conferenceResponseType rData;

            SendCheckJoin scc = new SendCheckJoin(APP_SERVER_IP, locationId, sessionId);
            HttpWebResponse checkJoinResponse = scc.Send();

            // Parse out sessionId
            if(checkJoinResponse != null)
            {
                if((sessionId ) != checkJoinResponse.Headers[HttpMessage.HEADER_SESSION_ID])
                {
                    mecStressTest.ErrorOutputText = "\nSession id doesn't match up for the join check";
                    mecStressTest.errorOutput.ScrollToCaret();
                    return false;
                }
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(checkJoinResponse.GetResponseStream());
            

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                checkJoinResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationIdReturned = rData.locationId;
            string conferenceId = rData.conferenceId;
  
            if(typeOfConference != typeType.isLocationOnline)
            {
                mecStressTest.ErrorOutputText = "\nReceived a non-kick message";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {
                return true;
            }
            else
            {   
                mecStressTest.ErrorOutputText = "\nThe check join command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
        }  

        public bool CheckKicked(string sessionId, string locationId, MecStressTest mecStressTest)
        {
            conferenceResponseType rData;

            SendCheckJoin scc = new SendCheckJoin(APP_SERVER_IP, locationId, sessionId);
            HttpWebResponse checkJoinResponse = scc.Send();

            // Parse out sessionId
            if(checkJoinResponse != null)
            {
                if((sessionId ) != checkJoinResponse.Headers[HttpMessage.HEADER_SESSION_ID])
                {
                    mecStressTest.ErrorOutputText = "\nSession id doesn't match up for the kick check";
                    mecStressTest.errorOutput.ScrollToCaret();
                    return false;
                }
            }
            else
            {
                mecStressTest.ErrorOutputText = "\nNo response from the application via HTTP.";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            

            // Parse out body, then parse out Xml from body
            StreamReader body = new StreamReader(checkJoinResponse.GetResponseStream());
            

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
                rData = (conferenceResponseType) serializer.Deserialize(body);
                body.Close();
                checkJoinResponse.Close();
            }
            catch(Exception e)
            {
                mecStressTest.ErrorOutputText = "\n" + e.ToString();
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }
            
            Metreos.Mec.WebMessage.typeType typeOfConference = rData.type;
            Metreos.Mec.WebMessage.resultType result = rData.result;
            Metreos.Mec.WebMessage.locationIdType[] locationIdReturned = rData.locationId;
            string conferenceId = rData.conferenceId;
  
            //Sanity check. 
            if(typeOfConference != typeType.isLocationOnline)
            {
                mecStressTest.ErrorOutputText = "\nReceived a non-kick message";
                mecStressTest.errorOutput.ScrollToCaret();
                return false;
            }

            // Make sure that the connection was able to be established
            if(result == resultType.success)
            {
                return false;
            }
            else
            {   
                mecStressTest.ErrorOutputText = "\nThe check kick command returned a failure";
                mecStressTest.errorOutput.ScrollToCaret();
                return true;
            }
        }  

        public bool CheckForConferenceExistence(string sessionId, MecStressTest mecStressTest)
        {
            Thread.Sleep(initialPause);

            if(!CheckConference(sessionId, mecStressTest))
                return false;

            return true;
        }

        public bool CheckForJoin(string sessionId, string locationId, MecStressTest mecStressTest )
        {
            Thread.Sleep(initialPause);

            if(!CheckJoined(sessionId, locationId, mecStressTest))
                return false;

            return true;
        }

        public bool CheckForKick(string sessionId, string locationId, MecStressTest mecStressTest)
        {
            Thread.Sleep(initialPause);

            if(!CheckKicked(sessionId, locationId, mecStressTest))
                return false;       
      
            return true;
        }
    }
}

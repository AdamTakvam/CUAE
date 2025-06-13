using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.LoggingFramework;
using Metreos.Messaging;

namespace Metreos.Providers.SalesforceDemo
{
	public class IpcListener : Loggable
	{
		public delegate string MakeCallRequestDelegate(string to, string from, string deviceName); // return the routingGuid
		public delegate bool HangupRequestDelegate(string routingGuid, string callId);
		public delegate bool AnswerRequestDelegate(string routingGuid);
		public delegate bool ConferenceRequestDelegate(string routingGuid, string callId, string to);
		public delegate bool LoginDelegate(string deviceName);

		public event MakeCallRequestDelegate MakeCallRequest;
		public event HangupRequestDelegate HangupRequest;
		public event AnswerRequestDelegate AnswerRequest;
		public event ConferenceRequestDelegate ConferenceRequest;
		public event LoginDelegate LoginRequest;

		private IpcFlatmapServer flatmapServer;
		private int ipcPort;
		private SortedList salesforceClients; // ID socketId, devicename
		private SortedList cuaeClients;

		private SortedList currentCalls;

		#region SF Client Messages
		// client messages
		private const int MESSAGE_LOGIN				= 1000;
		private const int MESSAGE_LOGOUT			= 1001;
		private const int MESSAGE_MAKECALL			= 1002;
		private const int MESSAGE_HANGUP			= 1003;
		private const int MESSAGE_ANSWER			= 1004;

		// server messages
		private const int MESSAGE_INCOMING			= 1050;
		private const int MESSAGE_CALLACTIVE		= 1051;
		private const int MESSAGE_CALLINACTIVE      = 1052;
		private const int MESSAGE_INITIATE			= 1053;
		private const int MESSAGE_LOGIN_ACK			= 1054;

		//params
		private const int PARAM_DEVICENAME			= 2000;
		private const int PARAM_TO					= 2001;
		private const int PARAM_FROM				= 2002;
		private const int PARAM_ORIGINALTO			= 2003;
		private const int PARAM_CALLID				= 2004;
		private const int PARAM_INUSE				= 2005;
		private const int PARAM_CAUSE				= 2006;
		private const int PARAM_LINEDN_COUNT		= 2007;

		private const int PARAM_LINEDN_START		= 3000;
		#endregion

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
	

		public IpcListener(int ipcPort, TraceLevel logLevel) : base(logLevel, "IpcListener")
		{
			this.ipcPort = ipcPort;
			this.salesforceClients = new SortedList();
			this.cuaeClients = new SortedList();
			this.currentCalls = new SortedList();
		}

		public void Start()
		{
			flatmapServer = new IpcFlatmapServer("IpcListener", (ushort)ipcPort, false, base.log.LogLevel);
			flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
			flatmapServer.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapServer.OnMessageReceivedDelegate(this.OnMessageReceieved);
			flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
			flatmapServer.Start();

			log.Write(TraceLevel.Info, "IPCLister started, waiting for Salesforce agent to connect.  IPC port is " + ipcPort.ToString());
		}

		public void Stop()
		{
			flatmapServer.Stop();
			log.Write(TraceLevel.Info, "IpcListener stopped, Salesforce requests will not be accepted.");
		}
        
		private void OnNewConnection(int socketId, string remoteHost)
		{ 
			log.Write(TraceLevel.Info, "Salesforce client connected from {0}.", remoteHost);
		}

		private void OnCloseConnection(int socketId)
		{
			if(salesforceClients.Contains(socketId))
			{
				salesforceClients.Remove(socketId);
				log.Write(TraceLevel.Info, "Salesforce client disconnected.");
			}
			if(cuaeClients.Contains(socketId))
			{
				cuaeClients.Remove(socketId);
				log.Write(TraceLevel.Info, "CUAE client disconnected.");
			}

		}

		private void OnMessageReceieved(int socketId, string receiveInterface, int messageType, FlatmapList message)
		{

			if(messageType >= 20000 && messageType <= 29999)
			{
				ProcessTrayIconRequest(socketId, receiveInterface, messageType, message);
			}
			else 
			{
				log.Write(TraceLevel.Verbose, "Received message from Salesforce client");

				if(messageType == MESSAGE_LOGIN)
				{
					if(salesforceClients.Contains(socketId))
					{
						log.Write(TraceLevel.Error, "Double-login from the same socketId");
					}
					else
					{
						string deviceName = message.Find(PARAM_DEVICENAME, 1).dataValue as String;
						salesforceClients[socketId] = deviceName;

						if(LoginRequest != null)
						{
							LoginRequest(deviceName);
						}
						else
						{
							log.Write(TraceLevel.Error, "No subscriber to take action on Login request");
						}

						log.Write(TraceLevel.Info, "SocketId {0} mapped to Device {1}", socketId, deviceName);

					}
				}
				else if(messageType == MESSAGE_LOGOUT)
				{
					string deviceName = message.Find(PARAM_DEVICENAME, 1).dataValue as String;
					
					if(salesforceClients.Contains(socketId))
					{
						salesforceClients.Remove(socketId);

						log.Write(TraceLevel.Verbose, "Removed client with device name {0} from client list", deviceName);
					}
					else
					{
						log.Write(TraceLevel.Warning, "Unable to remove a client with socketId {0} and device name {1} from client list", socketId, deviceName);
					}
				}
				else if(messageType == MESSAGE_MAKECALL)
				{
					string deviceName = message.Find(PARAM_DEVICENAME, 1).dataValue as String;
					string to = message.Find(PARAM_TO, 1).dataValue as String;
					string from = message.Find(PARAM_FROM, 1).dataValue as String;

					if(salesforceClients.Contains(socketId))
					{
						if(MakeCallRequest != null)
						{
							// Null is indication of failure to makecall
							MakeCallRequest(to, from, deviceName);
						}
						else
						{
							// No subscribers to actually make the call for us.
						
							// Send error indication to client TODO
						}
					
					}
					else
					{
						// Unknown client.  Ignore
						log.Write(TraceLevel.Warning, "Received MakeCall request from unknown client {0}", deviceName);
					}
				}
				else if(messageType == MESSAGE_HANGUP)
				{
					string deviceName = message.Find(PARAM_DEVICENAME, 1).dataValue as String;
					//Temp log
					log.Write(TraceLevel.Info, "Device: {0}", deviceName);
					string uniqueId = message.Find(PARAM_CALLID, 1).dataValue as String;

					log.Write(TraceLevel.Info, "Received a hangup command with deviceName {0} and callId {1}", deviceName, uniqueId);

					if(salesforceClients.Contains(socketId))
					{
						if(currentCalls.Contains(uniqueId))
						{
							if(HangupRequest != null)
							{
								bool result = HangupRequest(currentCalls[uniqueId] as string, uniqueId);

								if(result)
								{
									log.Write(TraceLevel.Verbose, "Hangup request successful");

									// Send success indication to client TODO
								}
								else
								{
									log.Write(TraceLevel.Error, "Unable to initiate hangup on behalf of client");

									// Send error indication to client TODO
								}
							}
							else
							{
								// No subscribers to actually hangup the call for use
							
								// Send error indication to client TODO
							}
						}
						else
						{
							// Unable to hangup a unknown call!
							log.Write(TraceLevel.Error, "Unable to hangup a call for no known ID {0}", uniqueId);

							// Send error indication to client TODO
						}
					}
					else
					{
						// Unknown client.  Ignore
						log.Write(TraceLevel.Warning, "Received Hangup request from unknown client {0}", deviceName);
					}
				}
				else if(messageType == MESSAGE_ANSWER)
				{
					string deviceName = message.Find(PARAM_DEVICENAME, 1).dataValue as String;
					string uniqueId = message.Find(PARAM_CALLID, 1).dataValue as String;

					if(salesforceClients.Contains(socketId))
					{
						if(currentCalls.ContainsKey(uniqueId))
						{
							if(AnswerRequest != null)
							{
								bool result = AnswerRequest(currentCalls[uniqueId] as string);

								if(result)
								{
									log.Write(TraceLevel.Verbose, "Answer request successful");

									// Send success indication to client TODO
								}
								else
								{
									log.Write(TraceLevel.Error, "Unable to initiate answer on behalf of client");

									// Send error indication to client TODO
								}
							}
							else
							{
								// No subscribers to actually hangup the call for use
							
								// Send error indication to client TODO
							}
						}
						else
						{
							// Unable to hangup a unknown call!
							log.Write(TraceLevel.Error, "Unable to answer a call for no known device {0}", deviceName);

							// Send error indication to client TODO
						}
					}
					else
					{
						// Unknown client.  Ignore
						log.Write(TraceLevel.Warning, "Received Hangup request from unknown client {0}", deviceName);
					}
				}
			}
		}

		#region Cuae Client Action/Events
		private void ProcessTrayIconRequest(int socketId, string receiveInterface, int messageType, FlatmapList message)
		{
			log.Write(TraceLevel.Verbose, "Received message from CUAE client");

			bool sent = false;
			switch(messageType)
			{
				case CuaeClient.MESSAGE_LOGIN_REQUEST:

					if(cuaeClients.Contains(socketId))
					{
						log.Write(TraceLevel.Error, "Double-login from the same socketId for CUAE client");

						message = new FlatmapList();
						message.Add(CuaeClient.LoginResponseFields.PARAM_STATUS, CuaeClient.LoginResponseFields.Status.ALREADY_LOGGED_IN);
			
						sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_LOGIN_RESPONSE, message);

					}
					else
					{
						string deviceName = message.Find(CuaeClient.LoginRequestFields.PARAM_DEVICENAME, 1).dataValue as String;
						cuaeClients[socketId] = deviceName;

						message = new FlatmapList();
						message.Add(CuaeClient.LoginResponseFields.PARAM_STATUS, CuaeClient.LoginResponseFields.Status.OK);

						sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_LOGIN_RESPONSE, message);

						log.Write(TraceLevel.Info, "SocketId {0} mapped to Device {1} for CUAE client", socketId, deviceName);
					}
					break;

				case CuaeClient.MESSAGE_CALL_REQUEST:
					
					string theDeviceName = message.Find(CuaeClient.CallRequestFields.PARAM_DEVICENAME, 1).dataValue as String;
					string to = message.Find(CuaeClient.CallRequestFields.PARAM_TO, 1).dataValue as String;
					string uniqueId = message.Find(CuaeClient.CallRequestFields.PARAM_CALLID, 1).dataValue as String;

					//Temp log
					log.Write(TraceLevel.Info, "Device: {0}", theDeviceName);
					
					log.Write(TraceLevel.Info, "Received a conference command with deviceName {0} and callId {1}", theDeviceName, uniqueId);

					if(cuaeClients.Contains(socketId))
					{
						if(currentCalls.Contains(uniqueId))
						{
							if(ConferenceRequest != null)
							{
								bool result = ConferenceRequest(currentCalls[uniqueId] as string, uniqueId, to);

								if(result)
								{
									log.Write(TraceLevel.Verbose, "Conference request initiated successfully");

									// need to have hooks into NoHandler to make the prov work
									//									FlatmapList message = new FlatmapList();
//									message.Add(CuaeClient.LoginResponseFields.PARAM_STATUS, CuaeClient.CallResponseFields.Status.OK);
//									message.Add(CuaeClient.LoginResponseFields.PARAM_CALLID, callId);
//
//									sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_CALL_PROV_RESPONSE, message);
								}
								else
								{
									// need to have hooks into NoHandler to make the prov work

									log.Write(TraceLevel.Error, "Unable to initiate conference on behalf of client");
//
//									FlatmapList message = new FlatmapList();
//									message.Add(CuaeClient.LoginResponseFields.PARAM_STATUS, CuaeClient.CallResponseFields.Status.FAILED);
//
//									sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_CALL_PROV_RESPONSE, message);
								}
							}
							else
							{
								// No subscribers to actually hangup the call for use
							
								// Send error indication to client TODO
							}
						}
						else
						{
							// Unable to hangup a unknown call!
							log.Write(TraceLevel.Error, "Unable to conference a call for no known ID {0}", uniqueId);

							// Send error indication to client TODO
						}
					}
					else
					{
						// Unknown client.  Ignore
						log.Write(TraceLevel.Warning, "Received Conference request from unknown client {0}", theDeviceName);
					}
					break;
			}
		}

		public bool SendConferenceInitiated(string routingGuid, string deviceName, string callId, bool success)
		{
			bool sent;
			if(cuaeClients.ContainsValue(deviceName))
			{
				int socketId = (int)  cuaeClients.GetKey(cuaeClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(CuaeClient.CallResponseFields.PARAM_CALLID, callId);
				message.Add(CuaeClient.CallResponseFields.PARAM_STATUS, success ? CuaeClient.CallResponseFields.Status.OK : CuaeClient.CallResponseFields.Status.FAILED);		
				sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_CALL_FINAL_RESPONSE, message);
				if(sent)
				{
					// todo: add new collection to store concurrent conference requests

					log.Write(TraceLevel.Verbose, "ConferenceInitiated message has been sent to CUAE client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "CUAE client is no longer connected with clientKey {0}", deviceName);
					cuaeClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no CUAE client with clientKey {0}", deviceName);
			}

			return sent;
		}

		/*
		 * 
            //100 Wild Basin Rd S, Austin, TX 78746, USA
            contactName = "Louis Marascio";
            contactAccountName = "Metreos";
            street = "100 Wild Basin Rd S";
            city = "Austin";
            state = "TX";
            zip = "78746";
            country = "USA";
			*/

		public bool SendIncomingCalltoCuaeClient(string routingGuid, string deviceName, string from, string callId, SalesforceProvider.ContactInfo caller, ArrayList resellers)
		{
			// fake content for now on account

			bool sent;
			if(cuaeClients.ContainsValue(deviceName))
			{
				int socketId = (int)  cuaeClients.GetKey(cuaeClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(CuaeClient.IncomingCallFields.PARAM_PHONE_NUMBER, from);
				message.Add(CuaeClient.IncomingCallFields.PARAM_CALLID, callId);
				message.Add(CuaeClient.IncomingCallFields.PARAM_FIRSTNAME, caller.FirstName);
				message.Add(CuaeClient.IncomingCallFields.PARAM_LASTNAME, caller.LastName);
				message.Add(CuaeClient.IncomingCallFields.PARAM_STREET, caller.Street);
				message.Add(CuaeClient.IncomingCallFields.PARAM_CITY, caller.City);
				message.Add(CuaeClient.IncomingCallFields.PARAM_STATE, caller.State);
				message.Add(CuaeClient.IncomingCallFields.PARAM_ZIP, caller.PostalCode);
				message.Add(CuaeClient.IncomingCallFields.PARAM_COUNTRY, caller.Country);
				message.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNTNAME, caller.AccountName);
				message.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_TYPE, caller.AccountType);
				message.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_SF_ID, caller.AccountSFId);
				message.Add(CuaeClient.IncomingCallFields.PARAM_LATITUDE, caller.Latitude);
				message.Add(CuaeClient.IncomingCallFields.PARAM_LONGITUDE, caller.Longitude);
				message.Add(CuaeClient.IncomingCallFields.PARAM_CONTACT_SF_ID, caller.ContactSFId);

				message.Add(CuaeClient.IncomingCallFields.PARAM_RESELLERS_COUNT, (uint)resellers.Count);

				for(int i = 0; i < resellers.Count; i++)
				{
					SalesforceProvider.ContactInfo resellerInfo = resellers[i] as SalesforceProvider.ContactInfo;

					FlatmapList reseller = new FlatmapList();
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_PHONE_NUMBER, resellerInfo.PhoneNumber);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_CALLID, callId);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_FIRSTNAME, resellerInfo.FirstName);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_LASTNAME, resellerInfo.LastName);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_STREET, resellerInfo.Street);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_CITY, resellerInfo.City);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_STATE, resellerInfo.State);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_ZIP, resellerInfo.PostalCode);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_COUNTRY, resellerInfo.Country);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNTNAME, resellerInfo.AccountName);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_TYPE, resellerInfo.AccountType);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_ACCOUNT_SF_ID, resellerInfo.AccountSFId);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_LATITUDE, resellerInfo.Latitude);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_LONGITUDE, resellerInfo.Longitude);
					reseller.Add(CuaeClient.IncomingCallFields.PARAM_CONTACT_SF_ID, resellerInfo.ContactSFId);
					message.Add((uint)(CuaeClient.IncomingCallFields.PARAM_RESELLER_BASE + i), reseller.ToFlatmap());
				}

				sent = flatmapServer.Write(socketId, CuaeClient.MESSAGE_INCOMING_CALL, message);
				if(sent)
				{
					// todo: add new collection to store concurrent conference requests

					log.Write(TraceLevel.Verbose, "IncomingCall message has been sent to CUAE client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "CUAE client is no longer connected with clientKey {0}", deviceName);
					cuaeClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no CUAE client with clientKey {0}", deviceName);
			}

			return sent;
		}


		#endregion

		public bool SendIncomingCall(string routingGuid, string deviceName, string to, string from, string originalTo, string callId)
		{
			bool sent;

			// associate this devicename with the routing guid to have a track of running calls
			currentCalls[callId] = routingGuid;

			if(salesforceClients.ContainsValue(deviceName))
			{
				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(PARAM_TO, to);
				message.Add(PARAM_FROM, from);
				message.Add(PARAM_ORIGINALTO, originalTo);
				message.Add(PARAM_CALLID, callId);
				message.Add(PARAM_DEVICENAME, deviceName);
			
				sent = flatmapServer.Write(socketId, MESSAGE_INCOMING, message);
				if(sent)
				{
					log.Write(TraceLevel.Verbose, "IncomingCall message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no client with clientKey {0}", deviceName);
			}

			return sent;
		}

		public bool SendCallActive(string deviceName, string to, string callId)
		{
			bool sent;
			if(salesforceClients.ContainsValue(deviceName))
			{
				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(PARAM_TO, to);
				message.Add(PARAM_CALLID, callId);
				message.Add(PARAM_DEVICENAME, deviceName);
			
				sent = flatmapServer.Write(socketId, MESSAGE_CALLACTIVE, message);

				if(sent)
				{
					log.Write(TraceLevel.Verbose, "CallActive message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no client with clientKey {0}", deviceName);
			}

			return sent;
		}

		public bool SendCallInactive(string deviceName, bool inUse, string callId)
		{
			bool sent;
			if(salesforceClients.ContainsValue(deviceName))
			{
				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(PARAM_INUSE, inUse ? 1 : 0);
				message.Add(PARAM_CALLID, callId);
			
				sent = flatmapServer.Write(socketId, MESSAGE_CALLINACTIVE, message);

				if(sent)
				{
					log.Write(TraceLevel.Verbose, "CallInactive message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no client with clientKey {0}", deviceName);
			}

			return sent;
		}

		public bool SendHangup(string deviceName, string cause, string callId)
		{
			bool sent;
			if(currentCalls.ContainsKey(callId))
			{
				currentCalls.Remove(callId);
			}

			if(salesforceClients.ContainsValue(deviceName))
			{
				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(PARAM_CAUSE, cause);
				message.Add(PARAM_CALLID, callId);
				message.Add(PARAM_DEVICENAME, deviceName);
			
				sent = flatmapServer.Write(socketId, MESSAGE_HANGUP, message);

				if(sent)
				{
					log.Write(TraceLevel.Verbose, "Hangup message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				sent = false;
				log.Write(TraceLevel.Warning, "Their is no client with clientKey {0}", deviceName);
			}

			return sent;
		}

		public bool SendInitiateCall(string routingGuid, string callId, string deviceName, string to, string from)
		{
			bool sent = false;

			currentCalls[callId] = routingGuid;

			if(salesforceClients.ContainsValue(deviceName))
			{
				// associate this devicename with the routing guid to have a track of running calls

				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();
				message.Add(PARAM_CALLID, callId);
				message.Add(PARAM_TO, to);
				message.Add(PARAM_FROM, from);
				message.Add(PARAM_DEVICENAME, deviceName);
			
				sent = flatmapServer.Write(socketId, MESSAGE_INITIATE, message);

				if(sent)
				{
					log.Write(TraceLevel.Verbose, "InitiateCall message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				log.Write(TraceLevel.Warning, "Unable to send message to non-connected client {0}", deviceName);
			}

			return sent;
		}

		public bool SendLoginResponse(string routingGuid, string deviceName, ArrayList lines)
		{
			bool sent = false;
			if(salesforceClients.ContainsValue(deviceName))
			{
				int socketId = (int)  salesforceClients.GetKey(salesforceClients.IndexOfValue(deviceName));
				FlatmapList message = new FlatmapList();

				if(lines != null)
				{
					message.Add(PARAM_LINEDN_COUNT, lines.Count);

					log.Write(TraceLevel.Verbose, "Line Count: {0}", lines.Count);

					for(int i = 0; i< lines.Count; i++)
					{
						message.Add((uint)(PARAM_LINEDN_START + i), lines[i] as string);
						log.Write(TraceLevel.Verbose, "Adding line {0} to login message", lines[i] as string);
					}
				}
				else
				{
					message.Add(PARAM_LINEDN_COUNT, 0);
				}

				message.Add(PARAM_DEVICENAME, deviceName);
			
				sent = flatmapServer.Write(socketId, MESSAGE_LOGIN_ACK, message);

				if(sent)
				{
					log.Write(TraceLevel.Verbose, "Login response message has been sent to salesforce client");
				}
				else
				{
					log.Write(TraceLevel.Warning, "Salesforce client is no longer connected with clientKey {0}", deviceName);
					salesforceClients.Remove(deviceName);
				}
			}
			else
			{
				log.Write(TraceLevel.Warning, "Unable to send message to non-connected client {0}", deviceName);
			}

			return sent;
		}
	}
}

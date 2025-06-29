using System;
using System.Diagnostics;
using System.Data; 
using System.Timers;
using System.Text.RegularExpressions;

using MySql.Data.MySqlClient;

using Metreos.Core;            
using Metreos.ProviderFramework;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;

using jabber.client;
using jabber.protocol.client;

namespace Metreos.Providers.JabberProvider
{
    [ProviderDecl("Shell Provider")] 
    [PackageDecl("Metreos.Providers.JabberProvider", "Description of your provider")]
    public class JabberProviderProvider : ProviderBase 
    {
		protected class Configs
		{
			public const string JabberServer			= "JabberServer";
			public const string BotUserName				= "BotUserName";
			public const string BotPassword				= "BotPassword";
			public const string BotAlias				= "BotAlias";
			public const string ConferenceRoomHost      = "ConferenceRoomHost";
			public const string ConferenceRoom			= "ConferenceRoom";
		}

		protected JabberClient client;
		protected string jabberServer;
		protected string botUsername;
		protected string botPassword;
		protected string botAlias;
		protected string conferenceRoomHost;
		protected string conferenceRoom;
		protected Regex commandStart;
		protected volatile bool started;


		// Replace this with your name please before installing when in the training!
		protected const string ProviderNamespace = "Metreos.Providers.JabberProvider"; 

		// Event
		protected const string BotCommand = "Metreos.Providers.JabberProvider.BotCommand";

		// Action
		protected const string Chat =  "Metreos.Providers.JabberProvider.SendChat";

        public JabberProviderProvider(IConfigUtility configUtility) 
            : base(typeof(JabberProviderProvider), "Jabber Provider", configUtility)
        {
			client = new JabberClient();
			client.OnConnect += new bedrock.net.AsyncSocketHandler(OnConnect);
			client.OnAuthenticate += new bedrock.ObjectHandler(OnAuthenticate);
			client.OnAuthError += new IQHandler(OnAuthError);
			client.OnMessage += new MessageHandler(OnMessage);
			client.OnError += new bedrock.ExceptionHandler(OnError);
			client.AutoStartTLS = false;
			client.AutoLogin = true;
			client.AutoAgents = false;
			client.AutoPresence = false;
			client.AutoRoster = false;
			client.AutoReconnect = 0;
			started = false;
			
        }

         #region ProviderBase Implementation
        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
			configItems = null;  // these two lines are here only so that the provider compiles.
			extensions = null;   // you will most likely remove these when implementing your own

            ///     TODO:
            ///     1.  Defining which methods handle which actions that originate from apps.
            ///     This is done by creating a relationship between an action name and a 
            ///     HandleMessageDelegate class... and then adding that to the  messageCallbacks
            ///     collection
            ///     Example:
                this.messageCallbacks.Add(Chat, 
            			new HandleMessageDelegate(this.SendChat));
					

            ///     2.  Define configuration items
            ///     A provider declares what is configurable in the management console in the 
            ///     Initialize method.  Do so by defining how many configItems you have as
            ///     an array, and then go about defining each configItem!
            ///     
			///      
					configItems = new ConfigEntry[6];
					configItems[0] = new ConfigEntry(Configs.JabberServer, "Jabber Server Host/IP", "", 
						"Hostname or IP address of the Jabber Server", IConfig.StandardFormat.String, true);
					configItems[1] = new ConfigEntry(Configs.BotUserName, "Jabber Bot Username", "cuaebot", 
						"Username of registered bot", IConfig.StandardFormat.String, true);
					configItems[2] = new ConfigEntry(Configs.BotPassword, "Jabber Bot Password", "", 
						"Password of registered bot", IConfig.StandardFormat.Password, true);
					configItems[3] = new ConfigEntry(Configs.BotAlias, "Jabber Bot Alias", "cuaebot", 
						"Alias of registered bot", IConfig.StandardFormat.String, true);
					configItems[4] = new ConfigEntry(Configs.ConferenceRoomHost, "Conference Room Host", "", 
						"The host name for this Jabber server's conference rooms", IConfig.StandardFormat.String, true);
					configItems[5] = new ConfigEntry(Configs.ConferenceRoom, "Conference Room", "cuae", 
						"The conference room name", IConfig.StandardFormat.String, true);
		 
            return true;
        }


        protected override void RefreshConfiguration()
        {
			jabberServer		= GetConfigValue(Configs.JabberServer) as string;
			botUsername			= GetConfigValue(Configs.BotUserName) as string;
			botPassword			= GetConfigValue(Configs.BotPassword) as string;
			botAlias			= GetConfigValue(Configs.BotAlias) as string;
			conferenceRoomHost	= GetConfigValue(Configs.ConferenceRoomHost) as string;
			conferenceRoom		= GetConfigValue(Configs.ConferenceRoom) as string;
			commandStart		= new Regex(String.Format("^\\s*{0}\\s*/", botAlias), RegexOptions.Compiled);

			ClientStart();
		}

        /// <summary>
        ///     If this method fires, then an event we fired was handled by no application.
        /// </summary>
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {      

        }

        /// <summary>
        ///     * Runs in "your" thread – not the application manager – 
        ///           so that it doesn't slow down the startup of other providers.
        ///     * You must call RegisterNamespace() here or applications can not use the actions of this provider.
        ///     * Perform possibly time-consuming actions, e.g., initializing stack.
        ///     * Note: Your provider should not send any events 
        ///           (and will not receive any actions) until this method completes.
        /// </summary>
        protected override void OnStartup()
        {
			started = true;

            RegisterNamespace();

			ClientStart();

			base.OnStartup();
        }

		private void ClientStart()
		{
			if(started)
			{
				if(jabberServer != null && jabberServer != String.Empty &&
					botUsername != null && botUsername != String.Empty &&
					botPassword != null && botPassword != String.Empty &&
					botAlias != null && botAlias != String.Empty &&
					conferenceRoomHost != null && conferenceRoomHost != String.Empty &&
					conferenceRoom != null && conferenceRoom != String.Empty)
				{

					client.User = botUsername;
					client.Server = jabberServer;
					client.Password = botPassword;

					client.Connect();
				}
			}
		}

        /// <summary>
        ///     Guaranteed to be called on a graceful shutdown of the Application Server
        /// </summary>
        protected override void OnShutdown()
        {
			started = false;
			client.Close();
        }

        #endregion

        #region Provider Events

		[Event(BotCommand, true, null, "Bot Command", "A command has been issued to the bot")]
		[EventParam("Nick", typeof(System.String), false, "Nick of user issuing command")]
		[EventParam("FullNick", typeof(System.String), false, "Fully qualified nick of user issuing command")]
		[EventParam("Type", typeof(System.String), true, "Type of message")]
		[EventParam("Command", typeof(System.String), false, "Body of message")]
		[EventParam("FullXMLBody", typeof(System.String), true, "Full body of message")]
		[EventParam("Delay", typeof(System.Boolean), true, "True if the message is a catch-up message, false if real-time message")]
		protected void BotCommandTrigger(string nick, string fullNick, string type, string command, string fullBody, bool delay)
		{
			string scriptId = System.Guid.NewGuid().ToString();
			
			EventMessage msg = CreateEventMessage(
				BotCommand, 
				EventMessage.EventType.Triggering, 
				scriptId);
                            
			msg.AddField("Nick", nick);
			msg.AddField("FullNick", fullNick);
			msg.AddField("Type", type);
			msg.AddField("Command", command);
			msg.AddField("Full XML Body", fullBody);
			msg.AddField("Delay", delay);

			palWriter.PostMessage(msg);
		}

        #endregion

        #region Actions
        
		[Action(Chat, false, "Action1", "Performs my action", false)]
		[ActionParam("FullNick", typeof(string), false, false, "Fully qualified message to send the message to, if not to the whole room")]
		[ActionParam("Body", typeof(string), true, false, "The body of the message")]
		[ActionParam("Direct", typeof(bool), false, false, "True to send to a single person, false to send to the whole room")]
		protected void SendChat(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string to = null;
			string body = null;
			bool direct = false;
			action.InnerMessage.GetString("To", false, String.Empty, out to);
			action.InnerMessage.GetString("Body", true, String.Empty, out body);
			action.InnerMessage.GetBoolean("Direct", false, false, out direct);

			bool success = false;

			if(direct)
			{
				if(to == null)
				{
					success = false;
				}
				else
				{
					log.Write(TraceLevel.Verbose, "Sending chat to {0}", to);
					client.Message(to, body);
					success = true;
				}
			}
			else
			{
				string fullQualTo = String.Format("{0}@{1}", conferenceRoom, conferenceRoomHost);
				log.Write(TraceLevel.Verbose, "Sending group chat to {0}", fullQualTo);
				client.Message(MessageType.groupchat, fullQualTo, body);
				success = true;
			}

			action.SendResponse(success);
		}
	
        #endregion

		#region JabberEvents
		private void OnConnect(object sender, bedrock.net.BaseSocket sock)
		{

		}

		private void OnAuthenticate(object sender)
		{
			Presence p = new Presence(client.Document);
			string to = String.Format("{0}@{1}/{2}", conferenceRoom, conferenceRoomHost, botAlias);
			string from = String.Format("{0}@{1}/{2}", botUsername, jabberServer, "Jabber.Net");
			log.Write(TraceLevel.Verbose, "Logging into chat room to: {0}, from: {1}", to, from);
			p.To = to;
			p.From = from;
			p.Type = PresenceType.available;
			client.Write(p);

			string roomFullQual = String.Format("{0}@{1}", conferenceRoom, conferenceRoomHost);
			log.Write(TraceLevel.Verbose, "Sending presence available message to chat room {0}", roomFullQual);
			client.Message(MessageType.groupchat, roomFullQual ,String.Format("The {0} bot is online", botAlias));
		}

		private void OnAuthError(object sender, jabber.protocol.client.IQ iq)
		{
			log.Write(TraceLevel.Error, "Unable to log in to the Jabber Server");
		}

		private void OnMessage(object sender, jabber.protocol.client.Message msg)
		{
			JabberClient client = sender as JabberClient;
			log.Write(TraceLevel.Verbose, "Message received from the Jabber Server:\n" + msg.OuterXml);

			// Get body
			string body = msg.Body;

			if(body != null && commandStart.IsMatch(body))
			{
				// Determine nick
				string nick = null;
				if(msg.From != null && msg.From.Resource != null)
				{
					nick = msg.From.Resource;
				}

				string fullNick = null;
				if(msg.From != null && msg.From.Bare != null)
				{
					fullNick = msg.From.Bare;
				}

				// Get message type
				MessageType type = msg.Type;

				// Rip off post command start
				int firstForwardSlash = body.IndexOf("/", 0);

				if(firstForwardSlash >= body.Length)
				{
					// unable to parse command
					body = String.Empty;
				}
				else
				{
					body = body.Substring(firstForwardSlash + 1);
				}

				
				// Is in past?
				bool isInPast = msg.X is jabber.protocol.x.Delay;
	
				log.Write(TraceLevel.Verbose, "Nick: {0}, Type: {1}, Body: {2}, Delay: {3}", nick, type, body, isInPast);

				BotCommandTrigger(nick, fullNick, type.ToString(), body, msg.OuterXml, isInPast);
			}
		}

		private void OnError(object sender, Exception ex)
		{
			log.Write(TraceLevel.Error, "Error occurred: " + ex);
		}

		#endregion JabberEvents
	}
}
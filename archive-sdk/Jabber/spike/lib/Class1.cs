using System;

using jabber.client;
using bedrock;
using bedrock.net;
using jabber.protocol.client;

namespace JabberExample
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class JabberCommunication
	{
		public JabberCommunication()
		{
			JabberClient client = new JabberClient();

			client.OnAgents += new IQHandler(OnAgents);
			client.OnConnect += new AsyncSocketHandler(OnConnect);
			client.OnAuthenticate += new ObjectHandler(OnAuthenticate);
			client.OnAuthError += new IQHandler(OnAuthError);

			client.OnReadText += new TextHandler(OnReadText);
			client.OnWriteText += new TextHandler(OnWriteText);
			client.OnMessage += new MessageHandler(OnMessage);
			client.OnAgents += new IQHandler(OnAgents);
			client.OnError += new ExceptionHandler(OnError);
			client.AutoStartTLS = false;
			client.AutoLogin = true;
			client.AutoAgents = false;
			client.AutoPresence = false;
			client.AutoRoster = false;
            client.User = ""; //registered user
			client.Server = ""; // jabber server
            client.Password = "";// password
			client.Connect();
		}

		public bool Connect()
		{
			return true;
		}

		private void OnConnect(object sender, BaseSocket sock)
		{
		}

		private void OnAuthenticate(object sender)
		{
			JabberClient client = sender as JabberClient;

			Presence p = new Presence(client.Document);
			p.To = // room @ conference host / alias
			p.From = // my name @ host / resource
			p.Type = PresenceType.available;
			client.Write(p);

			client.Message(MessageType.groupchat, ""/* my name @ host */ , "The CUAE bot is online");
		}


		private void OnAuthError(object sender, jabber.protocol.client.IQ iq)
		{

		}

		private void OnAgents(object sender, jabber.protocol.client.IQ iq)
		{

		}

		private void OnReadText(object sender, string txt)
		{

			Console.WriteLine(txt);
		}

		private void OnWriteText(object sender, string txt)
		{

		}

		private void OnError(object sender, Exception ex)
		{
			Console.WriteLine(ex);
		}

		private void OnMessage(object sender, Message msg)
		{
			Console.WriteLine(msg.OuterXml);
		}
	}

}

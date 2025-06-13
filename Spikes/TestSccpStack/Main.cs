using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using Metreos.SccpStack;

using Metreos.LoggingFramework;

namespace TestSccpStack
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("Press ENTER to start and then to exit");
			Console.ReadLine();
			
//			new TestMessageCodec();
//			new TestStack().Start("10.1.10.25");
			TestClient testClient = new TestClient();
			testClient.Start();

			// Entering 'q' and Enter terminates the session;
			// just Enter does nothing;
			// anything else places a call to what is assumed to be an extension.
			while (true)
			{
				string text = Console.ReadLine();
				if (text == "q")
				{
					testClient.Shutdown();
					break;
				}
				if (text.Length > 0)
				{
					if (text == "hold")
					{
						testClient.Hold();
					}
					else if (text == "resume")
					{
						testClient.Resume();
					}
					else if (text == "hangup")
					{
						testClient.Hangup();
					}
					else if (text == "redial")
					{
						testClient.Redial();
					}
					else if (text == "speeddial")
					{
						testClient.Speeddial();
					}
					else
					{
						testClient.Digits(text);
					}
				}
			}
		}
	}
}
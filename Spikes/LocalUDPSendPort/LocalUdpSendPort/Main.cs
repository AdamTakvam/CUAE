using System;
using System.Threading;

namespace LocalUdpSendPort
{
	/// <summary>
	/// This spike was for investigating how to send and receive on the same
	/// UDP port using C#.
	/// </summary>
	/// <remarks>
	/// I found out that you can't bind two sockets (a receive socket and a
	/// separate send socket) on the same port. The solution is to bind a
	/// socket to the port and use that socket to both send to and receive
	/// from a given entity.
	/// 
	/// The technique is shown in region, "Receive and SendTo on same port," in
	/// Wall.cs. It didn't work when I did a Connect to the destination
	/// address, so I replaced the Connect/Send scheme with just SendTo.
	/// </remarks>
	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
			wall = new Wall(Consts.ShoutToPort, Consts.EchoBackToPort);
			ear = new Ear(Consts.EchoBackToPort);
			mouth = new Mouth(Consts.ShoutToPort);

			while (!wall.IsReady || !ear.IsReady)
			{
				Console.WriteLine(".");
			}

			mouth.Shout("Four");
			mouth.Shout("score");
			mouth.Shout("and");
			mouth.Shout("seven");
			mouth.Shout("years");
			mouth.Shout("ago");
			mouth.Shout("our");
			mouth.Shout("fathers");
			mouth.Shout("...");

			Thread.Sleep(1000);

			wall.Stop();
			ear.Stop();

			Console.Write("press Enter to exit");
			Console.ReadLine();
		}

		private static Wall wall;
		private static Ear ear;
		private static Mouth mouth;

		private abstract class Consts
		{
			public const int ShoutToPort = 3000;
			public const int EchoBackToPort = 4000;
		}
	}
}

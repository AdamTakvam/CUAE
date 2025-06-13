using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

using ServerInterface;

namespace Client
{
	class ClientMain
	{
        private const int SERVER_PORT = 8020;
        private static string url;
        private static string path;
        private static byte[] data;

		[STAThread]
		static void Main(string[] args)
		{
			if(args.Length > 0)
			{
				url = args[0];
                path = args[1];
			}
			else
			{
				url = "http://192.168.1.100:" + SERVER_PORT.ToString() + "/RemotingServer";
			}

            try
            {
                HttpClientChannel c = new HttpClientChannel();
                ChannelServices.RegisterChannel(c);
            }
            catch(Exception e)
            {
                Console.WriteLine("Client cannot start, exception is:\n" + e.ToString());
                return;
            }

            IRemotingServer server = (IRemotingServer) Activator.GetObject(typeof(IRemotingServer), url); 
            Console.WriteLine("The server's name is: " + server.GetName());

            if(path != null)
            {
                FillData();
                Console.WriteLine("Uploading data of length {0}", data.Length);
                server.UploadData(data);
            }

            server.Cleanup();
		}

        static void FillData()
        {
            try
            {
                System.IO.FileInfo info = new System.IO.FileInfo(path);
                System.IO.FileStream stream = info.OpenRead();
                System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
               
                data = new byte[info.Length];
                reader.Read(data, 0, data.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }
	}
}

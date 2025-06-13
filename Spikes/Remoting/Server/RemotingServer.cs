using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

using ServerInterface;

namespace Server
{
	/// <summary>
	/// Summary description for RemotingServer.
	/// </summary>
	public class RemotingServer : MarshalByRefObject, IRemotingServer
	{
        private string Name = "MyRemotingServer";

        public string GetName()
        {
            return this.Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void UploadData(byte[] data)
        {
            Console.WriteLine("Received data of length {0}", data.Length);
        }

        public void Cleanup()
        {
            System.GC.Collect();
        }
	}
}

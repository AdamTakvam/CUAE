using System;
using System.Net;
using System.Net.Sockets;

namespace HttpListener
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Listener
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
        {
            string response =
                "HTTP/1.0 200 OK\rContent-Type: text/xml\rContent-Length: 128\rSet-Cookie: Blah=Blahism\r\r<CiscoIPPhoneText><Title>Cookie Tester</Title><Prompt>Press Respond</Prompt><Text>Testing cookies.</Text></CiscoIPPhoneText>\r\r";

            int port = 8002;
            if(args.Length > 0)
            {
                port = int.Parse(args[0]);
            }

            TcpListener listener = new TcpListener(port);

            listener.Start();

            Console.WriteLine("TCP listener started on port " + 
                port + ". Waiting for connection...");

            Socket socket = listener.AcceptSocket();
            
            Console.WriteLine("Connected to: " + socket.RemoteEndPoint.ToString());

            byte[] buffer = new Byte[4096];
            int bytesReceived = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None); 
            string httpMsg = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesReceived);

            Console.WriteLine("Received:\n" + httpMsg);

            Console.WriteLine();

            Console.WriteLine("Sending response.");
            buffer = System.Text.Encoding.UTF8.GetBytes(response);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
            socket.Send(buffer);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
	}
}

using System;

namespace CiscoIPPhoneExecute
{
    class CIPExecute
    {
        [STAThread]
        static void Main(string[] args)
        {
            string s = @"<HTML>
                        <HEAD></HEAD>
                        <BODY>
                        <FORM action=""http://192.168.1.200/CGI/Execute"" Method=""POST"">
                        <TEXTAREA NAME=""XML"" Rows=""5"" Cols=""60"">
                        &lt;CiscoIPPhoneExecute&gt;&lt;ExecuteItem URL=&quot;http://209.163.143.103/test.xml&quot; /&gt;&lt;/CiscoIPPhoneExecute&gt;
                        </TEXTAREA>
                        <BR>
                        <input type=submit value=POST>
                        </FORM>
                        </BODY>
                        </HTML>";

            
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(CIPExecute.Blah));
            
//            string cipExecuteXml = @"<CiscoIPPhoneExecute><ExecuteItem URL='http://209.163.143.103/test.xml' /></CiscoIPPhoneExecute>";
//            string s = "XML=" + cipExecuteXml;

            System.Net.WebRequest wr = System.Net.HttpWebRequest.Create("http://192.168.1.200/CGI/Execute");

            wr.Credentials = new System.Net.NetworkCredential("marascio", "lafnog22");

            wr.ContentType = "application/x-www-form-urlencoded";

            wr.Method = "POST";

            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(s);

            wr.ContentLength = b.Length;

            System.IO.Stream sw = wr.GetRequestStream();

            sw.Write(b, 0, b.Length);

            sw.Close();

            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)wr.GetResponse();

            sw = null;

            sw = response.GetResponseStream();

            System.IO.StreamReader reader = new System.IO.StreamReader(sw);

            s = reader.ReadToEnd();

            Console.WriteLine(s);

            reader.Close();
            sw.Close();

            response.Close();
        }

        public static void Blah(object state)
        {
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(8080);
            listener.Start();

            System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
            System.Net.Sockets.NetworkStream stream = client.GetStream();

            while(stream.CanRead)
            {
                byte[] data = new byte[client.ReceiveBufferSize];
                int count = stream.Read(data, 0, client.ReceiveBufferSize);
                Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(data, 0, count));
            }

        }
    }
}

using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace HashtableSerialization
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
            Hashtable hash = new Hashtable();
            Hashtable newHash;

            for(int i = 0; i < 25; i++)
            {
                hash.Add("Key" + i.ToString(), "324234324324324234234234234234234234234234234234234234" + i.ToString());
            }

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            bf.Serialize(stream, hash);

            byte[] buf = stream.GetBuffer();

            Console.WriteLine("Serialized {0} bytes", buf.Length);

            string safeBuf = System.Convert.ToBase64String(buf);
            
            Console.WriteLine("hash count: {0}", hash.Count);

            byte[] newBuf = System.Convert.FromBase64String(safeBuf);
            newHash = (Hashtable)bf.Deserialize(new MemoryStream(newBuf));

            Console.WriteLine("newHash count: {0}", newHash.Count);
            Console.WriteLine(safeBuf.Length);

            Console.WriteLine("hash[\"Key5\"] = {0}", (string)hash["Key5"]);
            Console.WriteLine("newHash[\"Key5\"] = {0}", (string)newHash["Key5"]);
    }
}
}

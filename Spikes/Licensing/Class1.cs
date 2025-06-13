using System;
using System.Security.Cryptography;

namespace Licensing
{
    class Class1
    {
        const string dataToSign = "this is my key";

        [STAThread]
        static void Main(string[] args)
        {Console.WriteLine("{0}", System.DateTime.Now.ToString());
            if(args.Length > 0)
            {
                if(args[0] == "-generateKeys")
                {
                    GeneratePrivateKey();
                }
            }
            else
            {
                CreateLicense();
            }
        }

        static void CreateLicense()
        {
            string privateKey = LoadKey("..\\..\\PrivateKey.xml");

            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            dsa.FromXmlString(privateKey);

            byte[] data = null;
            byte[] signature = null;

            data = System.Text.Encoding.Default.GetBytes(dataToSign);

            signature = dsa.SignData(data);

            Console.WriteLine(System.Convert.ToBase64String(signature));
            Console.WriteLine();

            if(VerifyLicense(data, signature))
            {
                Console.WriteLine("Verified");
            }
            else
            {
                Console.WriteLine("Not verified");
            }
        }

        static bool VerifyLicense(byte[] data, byte[] signature)
        {
            string publicKey = LoadKey("..\\..\\PublicKey.xml");

            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            dsa.FromXmlString(publicKey);

            return dsa.VerifyData(data, signature);
        }

        static string LoadKey(string keyFileName)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(keyFileName);
            
            string keyData = sr.ReadToEnd();

            sr.Close();

            return keyData;
        }

        static void GeneratePrivateKey()
        {
            System.IO.StreamWriter privateKey = new System.IO.StreamWriter("..\\..\\PrivateKey.xml");
            System.IO.StreamWriter publicKey = new System.IO.StreamWriter("..\\..\\PublicKey.xml");

            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(1024);

            string key;
            
            key = dsa.ToXmlString(true);
            privateKey.Write(key);
            
            key = dsa.ToXmlString(false);
            publicKey.Write(key);

            privateKey.Close();
            publicKey.Close();
        }
    }
}

using System;
using System.Configuration;

namespace ConfigSettings
{
    class Configurator
    {
        [STAThread]
        static void Main(string[] args)
        {
            string testKey1 = ConfigurationSettings.AppSettings["testKey1"];
            string testKey2 = ConfigurationSettings.AppSettings["testKey2"];

            Console.WriteLine("testKey1 is {0}", testKey1);
            Console.WriteLine("testKey2 is {0}", testKey2);
        }
    }
}

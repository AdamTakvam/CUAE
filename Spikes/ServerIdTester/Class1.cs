using System;

namespace ServerIdTester
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            uint serverId = 0;
            uint conxId = 0;

            do
            {
                Console.Write("Enter Server ID: ");
                try { serverId = uint.Parse(Console.ReadLine()); } 
                catch(Exception) {}
            } while(serverId <= 0 || serverId > 255);

            do
            {
                Console.Write("Enter Conx ID: ");
                try { conxId = uint.Parse(Console.ReadLine()); } 
                catch(Exception) {}
            } while(conxId <= 0 || conxId > 16777214);

            uint idValue = ((serverId & 0x000000ff) << 24) | (conxId & 0x00ffffff);

            Console.WriteLine("Combined ID: {0}", idValue);
            
            serverId = 0;
            conxId = 0;

            serverId = (idValue & 0xff000000) >> 24;
            conxId = idValue & 0x00ffffff;

            Console.WriteLine("Server ID: {0}", serverId);
            Console.WriteLine("Conx ID: {0}", conxId);
        }
    }
}

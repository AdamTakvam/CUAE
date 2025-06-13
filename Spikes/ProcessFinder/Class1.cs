using System;
using System.Diagnostics;

namespace ProcessFinder
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            Process[] p = Process.GetProcessesByName("mmsserver");

            if( (p != null) &&
                (p.Length >= 1))
            {
                p[0].EnableRaisingEvents = true;
                p[0].Exited += new EventHandler(Class1_Exited);
                p[0].WaitForExit();
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void Class1_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("Process exited");
            Console.ReadLine();
        }
    }
}

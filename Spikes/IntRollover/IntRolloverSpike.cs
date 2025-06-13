using System;

namespace IntRollover
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            uint i = System.UInt32.MaxValue - 5;

            Console.WriteLine("i is: {0}", i);

            i++;

            Console.WriteLine("i is: {0}", i);

            i++;

            Console.WriteLine("i is: {0}", i);
        }
    }
}

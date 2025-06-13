using System;

using Metreos.Messaging;
using Metreos.Utilities;

namespace InternalMessageTest
{
    public sealed class Tester
    {
        static void Main(string[] args)
        {
            Tester t = new Tester();

            if(t.Run())
                Console.WriteLine("Test Succeeded!");
            else
                Console.WriteLine("Test failed.");

            Console.WriteLine();
            Console.WriteLine("Press <enter> to quit");
            Console.ReadLine();
        }

        public Tester()
        { 
        }

        public bool Run()
        {
            // Insert data
            InternalMessage msg = CreateMessage();

            long startTime = HPTimer.Now();

            // Read data
            if(!ReadSingles(msg))
                return false;

            Console.WriteLine("Single field read time: {0}ms", HPTimer.MillisSince(startTime));

            startTime = HPTimer.Now();

            if(!ReadMultiples(msg))
                return false;

            Console.WriteLine("Multiple field read time: {0}ms", HPTimer.MillisSince(startTime));

            // See how long it takes to make a bunch of 'em
            startTime = HPTimer.Now();

            for(int i=0; i<1000; i++)
            {
                msg = CreateMessage();
            }

            Console.WriteLine("Batch create time (large): {0}ms", HPTimer.MillisSince(startTime));

            startTime = HPTimer.Now();

            for(int i=0; i<1000; i++)
            {
                msg = new InternalMessage();

                for(int x=0; x<14; x++)
                {
                    msg.AddField(x.ToString(), "sldkfhlshklsdfjklsj");
                }
            }

            Console.WriteLine("Batch create time (typical): {0}ms", HPTimer.MillisSince(startTime));

            return true;
        }

        private InternalMessage CreateMessage()
        {
            InternalMessage msg = new InternalMessage();
            for(int i=0; i<100; i++)
            {
                msg.AddField("single" + i, i);
            }

            for(int i=0; i<100; i++)
            {
                msg.AddFields("double" + i, i, i*100);
            }
            return msg;
        }

        private bool ReadSingles(InternalMessage msg)
        {
            for(int i=0; i<100; i++)
            {
                int val = Convert.ToInt32(msg["single" + i]);
                if(val != i)
                    return false;
            }
            return true;
        }

        private bool ReadMultiples(InternalMessage msg)
        {
            for(int i=0; i<100; i++)
            {
                bool gotVal1 = false;
                int val1 = 0, val2 = 0;

                object[] vals = msg.GetFields("double" + i);
                if(vals.Length != 2)
                    return false;

                foreach(object valObj in vals)
                {
                    if(!gotVal1)
                    {
                        val1 = Convert.ToInt32(valObj);
                        gotVal1 = true;
                    }
                    else
                    {
                        val2 = Convert.ToInt32(valObj);
                    }
                }

                if(val1 == (val2 / 100))
                    continue;
                if(val2 == (val1 / 100))
                    continue;

                return false;
            }
            return true;
        }
    }
}

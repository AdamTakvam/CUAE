//
// Cmdline.cs
//
using System;


// Command Line Parameters
// =========================================================
// msip(a.b.c.d)     media server IP address
// samples(n)        number of wave samples in the test
// hairpin(1)        hairpin conferences or not (1/0)
// myip(a.b.c.d)     ip address of this test client
// myport(n)         arbitrary port of this test client
// msglvl(n)         verbosity 0 = debug, 1 = normal



namespace mmslat
{
    public class Cmdline
    {
        /// <summary>Get any command line parameters</summary>
        public static int Parse(string[] args)
        {
            int errors = 0, nargval = 0;
            string[] parts;

            for(int i=0; i < args.Length; i++)
            {
                string arg = args[i];

                string[] argparts = ParseArgParts(arg);

                if (argparts == null)
                {
                    Console.WriteLine("malformed command line entry '{0}'", arg);
                    errors++;
                    break;
                }

                string argname = argparts[0];
                string argval  = argparts[1];

                switch(argname)
                {
                   case Const.argMsip:

                        parts = argval.Split(Const.cSplitBy(Const.dotc));

                        if (parts.Length != 4)
                        {    Console.WriteLine("value {0} for cmdline entry '{1}' is malformed", 
                                     argval, Const.argMsip);
                             errors++;
                        }

                        mediaServerIP = argval;
                        isRequiredArgsPresent = true;
                        break;


                   case Const.argSamples:

                        nargval = atoi(argval);

                        if (nargval < Const.minSamples || nargval > Const.maxSamples)
                        {
                            string s = (nargval < Const.minSamples)? "small": "large";
                            nargval  = (nargval < Const.minSamples)? Const.minSamples: Const.maxSamples;

                            Console.WriteLine("value {0} for cmdline entry '{1}' is too {2} - using {3}", 
                                               argval, Const.argSamples, s, nargval);
                        }

                        samplesToTest = nargval;
                        break;


                   case Const.argHairpin:

                        nargval = atoi(argval);

                        if ((nargval == 0 && argval != Const.szero) || (nargval != 0 && nargval != 1)) 
                        {
                             Console.WriteLine("value {0} for cmdline entry '{1}' incorrect - must be 0 or 1", 
                                      argval, Const.argHairpin);
                             errors++;
                        } 

                        isHairpinning = (nargval == 1);
                        break;
   

                   case Const.argMyip:

                        parts = argval.Split(Const.cSplitBy(Const.dotc));

                        if (parts.Length != 4)
                        {
                            Console.WriteLine("value {0} for cmdline entry '{1}' is malformed", 
                                    argval, Const.argMyip);
                            errors++;
                        }

                        testClientIP = argval;
                        break;


                   case Const.argMyport:

                        nargval = atoi(argval);

                        if (nargval == 0) 
                        {
                            Console.WriteLine("value {0} for cmdline entry '{1}' incorrect", 
                                    argval, Const.argMyport);
                            errors++;
                        } 

                        testClientPort = nargval;
                        break;


                   case Const.argMsglvl:

                        nargval = atoi(argval);
                    
                        if ((nargval == 0 && argval != Const.szero) || (nargval != 0 && nargval != 1)) 
                        {
                            Console.WriteLine("value {0} for cmdline entry '{1}' incorrect - must be 0 or 1", 
                                    argval, Const.argMsglvl);
                            errors++;
                        } 

                        msglvl = nargval;
                        break;

                   default:
                        Console.WriteLine("unrecognized command line entry '{0}'", argname);
                        errors++;
                        break;
                }    
            }

            return errors;  
        } 


        private static string[] ParseArgParts(string arg)
        {
           string[] s = arg.Split(new char[] { Const.lparenc, Const.rparenc } );           
           return (s.Length < 3)? null: s;
        } 


        /// <summary>Convert alpha to int with behavior as c library atoi</summary>
        public static int atoi(string s)
        {
            int    n = 0;
            try  { n = System.Convert.ToInt32(s); } catch { }
            return n;
        }


        public static void ShowCurrent()
        {
            Console.WriteLine("samples . . . . . . . . . " + samplesToTest);
            Console.WriteLine("media server IP . . . . . " + mediaServerIP);
            Console.WriteLine("hairpinning . . . . . . . " + (isHairpinning? "1 - on": "0 - off"));
            Console.WriteLine("client address. . . . . . " + testClientIP + Const.slash + testClientPort);
            Console.WriteLine("message verbosity . . . . " + (msglvl == 1? "1 - normal": "0 - debug"));
            Console.WriteLine();
        }


        public static void ShowSyntax()
        {
            Console.WriteLine("\nsyntax is: \nmmslat msip(i.j.k.l) samples(n) hairpin(1|0) myip(i.j.k.l) myport(n) msglvl(1|0)");
            Console.WriteLine("example: mmslat msip(127.0.0.1) samples(32) hairpin(1) myip(127.0.0.1)\n");

            Console.WriteLine("msip . . . . . IP address of media server (required)");
            Console.WriteLine("samples  . . . number of measurements to take, default 32");
            Console.WriteLine("hairin . . . . whether to hairpin conference, default 1");
            Console.WriteLine("myip   . . . . IP address of this test, default 127.0.0.1");
            Console.WriteLine("myport . . . . arbitrary port for this test, default 8311");
            Console.WriteLine("msglvl . . . . verbosity, 1 or zero, default 1");
            Console.WriteLine();
        }

        public static int    samplesToTest = Const.DefaultSamplesToTest;
        public static int    msglvl = Const.MSGLVL_NORMAL;     
        public static string mediaServerIP = Const.LocalHostIP;    
        public static bool   isHairpinning = true;                 
        public static long   packetIntervalMs = Const.DefaultPacketIntervalsMs;  

        public static string testClientIP     = Const.LocalHostIP;            
        public static int    testClientPort   = Const.TestClientDefaultPort; // ususally IP/port of test box

        public static int    packetsOfMedia   = Const.DefaultPacketsOfMedia;
        public static int    packetsOfSilence = Const.DefaultPacketsOfSilence;

        private static bool  isRequiredArgsPresent = false;
        public  static bool  IsRequiredArgsPresent { get { return isRequiredArgsPresent; } }

    } // class Cmdline

}   // namespace


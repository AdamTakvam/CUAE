using System;
using System.IO;
using Metreos.Max.Core;


namespace Metreos.Max.Framework
{
    public class MaxCommandLine
    {
        /// <summary>Get any command line parameters</summary>
        public static int Parse(string[] args)
        {
            int errors = 0;

            if (args.Length > 1)
                MaxMain.cmdlinePath = new string[args.Length - 1];
            else
                MaxMain.cmdlinePath = new string[1];

            for(int i=0; i < args.Length; i++)
            {
                string x = args[i];

//                if (i == 0 && x[0] != Const.cslash)
                if (x[0] != Const.cslash)
                {
                    if (File.Exists(x))
                        MaxMain.cmdlinePath.SetValue(x, i) ;
                    else
                    {
                        MaxMain.MessageWriter.WriteLine(Const.CmdlineErrMsg(i, Const.CmdlinePathErrMsg));
                        errors++;
                    }   
                    continue;
                }
/*
                if (x[0] != Const.cslash)
                {
                    MaxMain.MessageWriter.WriteLine(Const.CmdlineErrMsg(i, Const.CmdlineSlashErrMsg));
                    errors++;
                    continue;
                }
*/
                string arg = x.Substring(1).ToLower();

                if (arg == Const.cmdlineParamBuild)
                {
                    if (MaxMain.cmdlinePath == null)
                    {
                        MaxMain.MessageWriter.WriteLine(Const.CmdlineErrMsg(i, Const.CmdlineNoPathMsg));
                        errors++;                  
                    }
                    else MaxMain.autobuild = true;
                }
            }

            return errors;  
        }  
    } // class MaxCommandLine
}   // namespace

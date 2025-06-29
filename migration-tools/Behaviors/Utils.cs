using System;
using System.Collections.Specialized;
using System.Text;
using System.IO;

namespace BehaviorCore
{
    abstract public class Utils
    {

        public static void DirectoryCopy(string srcdir, string destdir, bool recursive)
        {
            DirectoryInfo dir;
            FileInfo[] files;
            DirectoryInfo[] dirs;
            string tmppath;

            //determine if the destination directory exists, if not create it
            if (!Directory.Exists(destdir))
            {
                Directory.CreateDirectory(destdir);
            }

            dir = new DirectoryInfo(srcdir);

            //if the source dir doesn't exist, throw
            if (!dir.Exists)
            {
                throw new ArgumentException("source dir doesn't exist -> " + srcdir);
            }

            //get all files in the current dir
            files = dir.GetFiles();

            //loop through each file
            foreach (FileInfo file in files)
            {
                //create the path to where this file should be in destdir
                tmppath=Path.Combine(destdir, file.Name);

                //copy file to dest dir
                file.CopyTo(tmppath, false);
            }

            //cleanup
            files = null;

            //if not recursive, all work is done
            if (!recursive)
            {
                return;
            }

            //otherwise, get dirs
            dirs = dir.GetDirectories();

            //loop through each sub directory in the current dir
            foreach (DirectoryInfo subdir in dirs)
            {
                //create the path to the directory in destdir
                tmppath = Path.Combine(destdir, subdir.Name);

                //recursively call this function over and over again
                //with each new dir.
                DirectoryCopy(subdir.FullName, tmppath, recursive);
            }

            //cleanup
            dirs = null;

            dir = null;
        }

        public static string GetDbVersionForSoftware(string softwareVersion)
        {
            StringDictionary versions = new StringDictionary();
            versions["2.1.0"] = "1";
            versions["2.1.1"] = "2";
            versions["2.1.2"] = "3";
            versions["2.1.3"] = "4";
            versions["2.1.4"] = "5";
            versions["2.2.0"] = "6";
            versions["2.2.1"] = "7";
            versions["2.3.0"] = "8";
            versions["2.3.1"] = "9";

            if (versions.ContainsKey(softwareVersion))
                return versions[softwareVersion];
            else
                return String.Empty;
        }

        public static string GetMainVersion(string softwareVersion)
        {
            string[] vbits = softwareVersion.Split('.',' ');
            if (vbits.Length < 3)
                return softwareVersion;

            return String.Format("{0}.{1}.{2}", vbits[0], vbits[1], vbits[2]);
        }

    }
}

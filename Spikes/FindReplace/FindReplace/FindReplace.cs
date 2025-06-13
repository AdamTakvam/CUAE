using System;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Metreos.Utilities;

namespace FindReplace
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class FindReplaceTool
	{
        public const string findParam = "f";
        public const string replaceParam = "r";
        public const string recursiveParam = "R";
        public const string extensionParam = "e";


		/// <summary>
		///     FindReplace tool
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
		    CommandLineArguments parser = new CommandLineArguments(args);
 
            string findParamValue       = parser.IsParamPresent(findParam) ? parser.GetSingleParam(findParam) : null;
            string replaceParamValue    = parser.IsParamPresent(replaceParam) ? parser.GetSingleParam(replaceParam) : null;
            string extensionParamValue  = parser.IsParamPresent(extensionParam) ? parser.GetSingleParam(extensionParam) : null;
            bool recursiveParamValue    = parser.IsParamPresent(recursiveParam); 
            StringCollection extras     = parser.GetStandAloneParameters();
            string fileParamValue       = extras == null ? null : (extras.Count == 0 ? null : extras[0]) ;
            string startDirectory       = System.Environment.CurrentDirectory;

            string formattedStartInfo = String.Format(StartInfo, 
                findParamValue == null      ? "[Not specified]" : '\"' + findParamValue + '\"',
                replaceParamValue == null   ? "[Not specified]" : '\"' + replaceParamValue + '\"',
                extensionParamValue == null ? "[Not specified]" : '\"' + extensionParamValue + '\"',
                recursiveParamValue         ? "recursive" : "not recursive",
                fileParamValue == null      ? "[Not specified]" : '\"' + fileParamValue + '\"',
                startDirectory);

            Console.WriteLine(formattedStartInfo);

            Console.WriteLine("Would you like to continue with these parameters? y for yes, all else for no");
            char someChar = (char)Console.Read();
            if(someChar.ToString().ToLower() != "y")
            {
                Console.WriteLine("Thank you for nothing.");
                return;
            }

            if(!ValidateParams(findParamValue, replaceParamValue, extensionParamValue, recursiveParamValue, fileParamValue))
            {
                Console.WriteLine(Usage);
                return;
            }

            Execute(findParamValue, replaceParamValue, extensionParamValue, recursiveParamValue, fileParamValue);
		}

        private static bool ValidateParams(
            string findParamValue, 
            string replaceParamValue, 
            string extensionParamValue, 
            bool recursiveParamValue,
            string fileParamValue)
        {
            if(extensionParam == null && fileParamValue == null)
            {
                Console.WriteLine("You must specify at least a filename or extension.");
                return false;
            }

            if(findParamValue == null)
            {
                Console.WriteLine("You must specify a 'find' parameter (-f)");
                return false;
            }

            if(replaceParamValue == null)
            {
                Console.WriteLine("You must specify a 'replace' parameter (-r)");
                return false;
            }

            return true;
        }

        public static void Execute(string findParamValue, 
            string replaceParamValue, 
            string extensionParamValue, 
            bool recursiveParamValue,
            string fileParamValue)
        {
            DirectoryInfo[] directories = new DirectoryInfo[1] { new DirectoryInfo(System.Environment.CurrentDirectory) };

            ReplaceCheck(directories, findParamValue, replaceParamValue, extensionParamValue, recursiveParamValue, fileParamValue);
        }

        public static void ReplaceCheck(
            DirectoryInfo[] directories, 
            string find, 
            string replace, 
            string ext, 
            bool recursive, 
            string file)
        {
            if(directories == null || directories.Length == 0)
            {
                return;
            }

            foreach(DirectoryInfo directory in directories)
            {
                FileInfo[] files;
                if(ext != null)
                {
                    files = directory.GetFiles("*." + ext);
                }
                else
                {
                    files = directory.GetFiles(file);
                }

                Replace(files, find, replace);

                if(recursive)
                {
                    ReplaceCheck(directory.GetDirectories(), find, replace, ext, recursive, file);
                }
            }
        }

        public static void Replace(FileInfo[] files, string find, string replace)
        {
            if(files == null || files.Length == 0)
            {
                return;
            }

            foreach(FileInfo file in files)
            {
                FileStream stream = null;
                StreamReader reader = null;
                StringWriter writer = null;
                StreamWriter streamWriter = null;
                Console.WriteLine("Replacing for file {0}", file.FullName);

                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite);
                    reader = new StreamReader(stream);
                    writer = new StringWriter();
                    bool read = true;
                    while(read)
                    {
                        string line = reader.ReadLine();

                        if(line == null) 
                        {
                            read = false;
                            continue;
                        }

                        line = Regex.Replace(line, find, replace);

                        writer.WriteLine(line);
                    }

                    reader.Close();
                    reader = null;

                    stream.Close();
                    stream = null;

                    stream = file.Open(FileMode.Create);
                    streamWriter = new StreamWriter(stream);

                    streamWriter.Write(writer.ToString());                     
                }
                catch(Exception e)
                {
                    Console.WriteLine("Couldn't replace text in file {0} due to exception: ", file.FullName, Exceptions.FormatException(e));
                }
                finally
                {
                    if(streamWriter != null)
                    {
                        streamWriter.Close();
                    }

                    if(reader != null)
                    {
                        reader.Close();
                    }

                    if(writer != null)
                    {
                        writer.Close();
                    }

                    if(stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }

        #region Usage
        public const string Usage =@"

fr.exe -f:[] -r:[] -e:[] -R filename.fil

-f:[]    The phrase to look for. Required.
-r:[]    The phrase to replace with. Required.
-e:[]    The file extension to search files for.  Do not include the '.' . Optional
-R       Recursively search files of the given name or extension. Optional.

filename.fil   The file to replace text in.  Optional. Has priority over extension.
";

        public const string StartInfo = @"

Find/Replace Starting Information
---------------------------------
-f:       {0}
-r:       {1}
-e:       {2}
-R:       {3}
filename: {4}
startdir: {5}
";
        #endregion
	}
}

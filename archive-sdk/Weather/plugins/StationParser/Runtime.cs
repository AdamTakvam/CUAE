using System;

namespace StationParser
{
    /// <summary> A utility to generate the StationInfo.cs, which in turn gives us a cached listing of valid
    /// states and station friendly names/url/ids </summary>
	public class Runtime
	{
        #region Usage

        private static string usage = @"
        Input as the only parameter the full path and filename of the validstation.xml file.
";
        #endregion

		[STAThread]
		static void Main(string[] args)
		{
            if(args.Length == 0 || args[0] == null)
            {
                Console.WriteLine(usage);
            }

            if(!System.IO.File.Exists(args[0]))
            {
                Console.WriteLine(usage);
            }

			StationChooser chooser = new StationChooser(args[0]);
            chooser.WriteCsFile();
		}
	}
}

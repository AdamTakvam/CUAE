using System;

namespace Metreos.Interfaces
{
    public abstract class IConsoleApps
    {
        /// <summary>Format string for standard console application header text.</summary>
        public const string CONSOLE_HEADER_TEXT = "CUAE {0} v" + IBuild.BuildIDFQ;

        /// <summary>The standard copyright text.</summary>
        public const string COPYRIGHT_TEXT = "Copyright (c) 2003-2007 Cisco Systems, Inc. All Rights Reserved.";


        /// <summary>
        /// Convenience method to generate the standard console header text.
        /// </summary>
        /// <param name="appName">The name of the application: e.g., 'Application Server' or 'SFTP Server'</param>
        /// <returns>A string with the header text.</returns>
        public static string GetConsoleHeaderText(string appName)
        {
            return String.Format(CONSOLE_HEADER_TEXT, appName);
        }


        /// <summary>
        /// Convenience method to print a header for a console application.
        /// </summary>
        /// <param name="appName">The name of the application: e.g., 'Application Server' or 'SFTP Server'</param>
        public static void PrintHeaderText(string appName)
        {
            Console.WriteLine();
            Console.WriteLine(GetConsoleHeaderText(appName));
            Console.WriteLine(COPYRIGHT_TEXT);
            Console.WriteLine();
        }
    }
}

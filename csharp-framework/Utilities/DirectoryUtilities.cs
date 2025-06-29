using System;

namespace Metreos.Utilities
{
    public abstract class DirectoryUtilities
    {
        /// <summary>
        /// Creates a temporary directory name based on a GUID.
        /// </summary>
        /// <returns>A string containing the temporary directory name.</returns>
        public static string CreateTemporaryDirectoryName()
        {
            string scratchDir = System.Guid.NewGuid().ToString();

            // Make sure the scratch directory name doesn't include any
            // bogus characterse that aren't allowed in directory names.
            scratchDir = scratchDir.Replace("==", "");
            scratchDir = scratchDir.Replace("/", "");
            scratchDir = scratchDir.Replace("\\", "");
            scratchDir = scratchDir.Replace("?", "");
            scratchDir = scratchDir.Replace(":", "");
            scratchDir = scratchDir.Replace(";", "");
            scratchDir = scratchDir.Replace("+", "");
            scratchDir = scratchDir.Replace("*", "");
            scratchDir = scratchDir.Replace("<", "");
            scratchDir = scratchDir.Replace(">", "");
            scratchDir = scratchDir.Replace("|", "");
            scratchDir = scratchDir.Replace("\"", "");
            scratchDir = scratchDir.Replace("-", "");
            scratchDir = scratchDir.Replace("{", "");
            scratchDir = scratchDir.Replace("}", "");

            return scratchDir.ToLower();
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;

namespace Metreos.AppServer.ApplicationManager.Collections
{
    public class MediaFileCollection
    {
        private Dictionary<string, FileInfo[]> files;

        public int Count { get { return fileCount; } }
        private int fileCount = 0;

        public MediaFileCollection(DirectoryInfo mediaDir)
        {
            this.files = new Dictionary<string, FileInfo[]>();

            if(mediaDir != null)
            {
                MoveLegacyMedia(mediaDir);

                foreach(DirectoryInfo localeDir in mediaDir.GetDirectories())
                {
                    FileInfo[] mediaFiles = localeDir.GetFiles();
                    if(mediaFiles != null && mediaFiles.Length > 0)
                    {
                        files.Add(localeDir.Name, mediaFiles);
                        fileCount += mediaFiles.Length;
                    }
                }
            }
        }

        public string[] GetLocales()
        {
            string[] locales = new string[files.Keys.Count];
            files.Keys.CopyTo(locales, 0);
            return locales;
        }


        public FileInfo[] GetFiles(string locale)
        {
            return files[locale];
        }

        private static void MoveLegacyMedia(DirectoryInfo mediaDir)
        {
            // If there are any files in the main media dir, 
            //   this is a legacy app.
            FileInfo[] legacyMedia = mediaDir.GetFiles();
            if(legacyMedia != null && legacyMedia.Length > 0)
            {
                // Move files to default locale directory
                DirectoryInfo localeDir = null;
                DirectoryInfo[] localeDirs = mediaDir.GetDirectories(MediaManager.Consts.Defaults.Locale);
                if(localeDirs == null || localeDirs.Length == 0)
                    localeDir = mediaDir.CreateSubdirectory(MediaManager.Consts.Defaults.Locale);
                else
                    localeDir = localeDirs[0];

                foreach(FileInfo legacyFile in legacyMedia)
                {
                    string destPath = Path.Combine(localeDir.FullName, legacyFile.Name);
                    try { legacyFile.MoveTo(destPath); }
                    catch { /* Chalk it up to "best effort" */ }
                }
            }
        }
    }
}

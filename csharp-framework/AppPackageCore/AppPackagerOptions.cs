using System;
using System.Globalization;

namespace Metreos.AppArchiveCore
{
    /// <summary>
    /// Contains a series of options that can be specified
    /// when performing actions using the application packager.
    /// </summary>
    [Serializable]
    public sealed class AppPackagerOptions
    {
        /// <summary>The filename to operate on.</summary>
        /// <remarks>Required for extraction and creation</remarks>
        public string filename;

        /// <summary>Application script XML files that are going
        /// to be packaged for the application.</summary>
        /// <remarks>Required for package creation.</remarks>
        public string[] appXmlFiles;

        /// <summary>Database creation scripts that are going
        /// to be packaged for the application.</summary>
        /// <remarks>Always optional.</remarks>
        public string[] dbCreateScripts;

		/// <summary>Media files that are going to be 
		/// packaged for the application</summary>
		/// <remarks>Always optional</remarks>
		public string[] mediaFiles;

        /// <summary>The locales associated 1:1 with the
        /// mediaFiles array </summary>
        /// <remarks>Must have same count as mediaFiles</remarks>
        public CultureInfo[] mediaLocales;

        /// <summary>Voice Recognition Resource files used
        /// by text-to-speech engines</summary>
        /// <remarks>Always optional</remarks>
        public string[] voicerecFiles;

        /// <summary>The application's installer file used
        /// when creating an application package.</summary>
        /// <remarks>Always optional.</remarks>
        public string installerXmlFile;

        /// <summary>The application's locales file used
        /// when creating an application package.</summary>
        /// <remarks>Always optional.</remarks>
        public string localesXmlFile;

        /// <summary>The framework directory to use when creating
        /// an application package.</summary>
        /// <remarks>Required for package creation.</remarks>
        public string frameworkDirName;

        /// <summary>A series of directories that should be
        /// searched when resolving native type dependencies.</summary>
        /// <remarks>Always optional.</remarks>
        public string[] nativeTypeSearchDirs;

        /// <summary>A series of directories that should be
        /// searched when resolving native action dependencies.</summary>
        /// <remarks>Always optional.</remarks>
        public string[] nativeActionSearchDirs;

        /// <summary>Native Type dlls explicitly added as references to the project</summary>
        /// <remarks>Always optional.</remarks>
        public string[] explicitNativeTypeDlls;

        /// <summary>Native Action dlls explicitly added as references to the project</summary>
        /// <remarks>Always optional.</remarks>
        public string[] explicitNativeActionDlls;

        /// <summary>Other dlls added as references to the project</summary>
        /// <remarks>Always optional.</remarks>
        public string[] explicitOtherDlls;

        /// <summary>Indicates whether directories should be
        /// searched recursively when resolving native action
        /// and type dependencies.</summary>
        /// <remarks>Always optional.</remarks>
        public bool recursiveDirSearch;

        /// <summary>The directory into which to place the 
        /// output of the packager operation.</summary>
        /// <remarks>Always optional.</remarks>
        public string outputDirectory;

        /// <summary>Application display name.</summary>
        /// <remarks>Always optional.</remarks>
        public string appDisplayName;

        /// <summary>Application description.</summary>
        /// <remarks>Always optional.</remarks>
        public string appDescription;

        /// <summary>Application company name.</summary>
        /// <remarks>Always optional.</remarks>
        public string appCompany;

        /// <summary>Application author name.</summary>
        /// <remarks>Always optional.</remarks>
        public string appAuthor;

        /// <summary>Application copyright statement.</summary>
        /// <remarks>Always optional.</remarks>
        public string appCopyright;     

        /// <summary>Application trademark statement.</summary>
        /// <remarks>Always optional.</remarks>
        public string appTrademark;

        /// <summary>Application version.</summary>
        /// <remarks>Always optional.</remarks>
        public string appVersion;

        /// <summary>Indicates whether to print the help text
        /// describing how to use the packager.</summary>
        /// <remarks>Always optional.</remarks>
        public bool printUsage;

        /// <summary>Indicates whether to output verbose messages
        /// while the packager is running.</summary>
        /// <remarks>Always optional.</remarks>
        public bool verbose;


        /// <summary>Default constructor.</summary>
        public AppPackagerOptions() 
        {}


        /// <summary>Verifies that the options specified are valid for
        /// an create operation.</summary>
        /// <returns>Returns true if the options are valid,
        /// false otherwise.</returns>
        public bool ValidateCreate()
        {
            if(printUsage == true) return false;

            if( (appXmlFiles == null) || 
                (frameworkDirName == null) ||
                (filename == null))
            {
                return false;
            }

            return true;
        }


        /// <summary>Verifies that the options specified are valid for
        /// an extract operation.</summary>
        /// <returns>Returns true if the options are valid,
        /// false otherwise.</returns>
        public bool ValidateExtract()
        {
            if(printUsage == true) return false;
            if(filename == null) return false;

            return true;
        }
    }
}

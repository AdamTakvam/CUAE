using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Metreos.UnitTest
{
    /// <summary>
    /// Base class for all CUAE C# unit tests. This class will setup a few
    /// commonly used variables such as the location of a temporary directory
    /// and provide useful methods for doing common tasks such as cleaning
    /// that directory.
    /// </summary>
    public abstract class UnitTestBase
    {        
        /// <summary>
        /// The location of the CUAE workspace. This is derived from the environment
        /// variable CUAEWORKSPACE.
        /// </summary>
        public string Workspace { get { return workspace; } }
        private string workspace;
       
        /// <summary>
        /// The directory to which a unit test can store temporary files. 
        /// This is unique for each test in this form: $CUAEWORKSPACE\temp\[testClassFullName]\
        /// </summary>
        public string TempDir { get { return tempDir; } }
        private string tempDir;

        public UnitTestBase()
        {
            workspace   = System.Environment.GetEnvironmentVariable("CUAEWORKSPACE");

            System.Diagnostics.Debug.Assert(workspace != null);

            // Temporary directory should be $CUAEWORKSPACE\temp\<assemblyFullName>\
            tempDir     = Path.Combine(workspace, "temp");
            tempDir     = Path.Combine(tempDir, GetType().FullName);
        }

        /// <summary>
        /// Create a temporary directory for a unit test.
        /// </summary>
        public void CreateTempDirectory()
        {
            if(!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
        }

        /// <summary>
        /// Clean up the temporary directory if we created one.
        /// </summary>
        public void DeleteTempDirectory()
        {
            if(Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
    }
}

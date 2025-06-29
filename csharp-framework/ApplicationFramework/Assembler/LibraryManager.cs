using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.ApplicationFramework.Assembler
{
	/// <summary>
	/// Locates libraries for actions and types
	/// </summary>
	internal class LibraryManager
	{
        public DirectoryInfo CompilerOutputDir
        {
            get { return appCodeDir; }
        }

        public string frameworkRootDir;
        public string applicationRootDir;

        internal static DirectoryInfo fwCoreDir;
        internal static DirectoryInfo fwActionDir;
        internal static DirectoryInfo fwTypeDir;
        internal static DirectoryInfo appActionDir;
        internal static DirectoryInfo appTypeDir;
        internal static DirectoryInfo appLibDir;
        internal static DirectoryInfo appCodeDir;

		public LibraryManager(string frameworkRootDir, string applicationRootDir)
		{
            this.frameworkRootDir = frameworkRootDir;
            this.applicationRootDir = applicationRootDir;
		}

        public bool Reset(string fwVersion, string appName, string appVersion)
        {
            try
            {
                string libDirStr = Path.Combine(frameworkRootDir, fwVersion);
                fwCoreDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.FwDirectoryNames.CORE));
                fwActionDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.FwDirectoryNames.ACTIONS));
                fwTypeDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.FwDirectoryNames.TYPES));

                libDirStr = Path.Combine(applicationRootDir, appName);
                libDirStr = Path.Combine(libDirStr, appVersion);
                appActionDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.AppDirectoryNames.ACTIONS));
                appTypeDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.AppDirectoryNames.TYPES));
                appLibDir  = new DirectoryInfo(Path.Combine(libDirStr, IConfig.AppDirectoryNames.LIBS));
                appCodeDir = new DirectoryInfo(Path.Combine(libDirStr, IConfig.AppDirectoryNames.EMBEDDED_CODE));

                if(appCodeDir.Exists == false)
                {
                    appCodeDir.Create();
                }
            }
            catch(Exception) { return false; }

            return true;
        }

        public static object GetObjFromAssembly(string name)
        {
            string assemblyName;
            return GetObjFromAssembly(name, out assemblyName);
        }

        public static object GetObjFromAssembly(string name, out string assemblyFileName)
        {
            // Construct the expected filename of the native action/type dll
            assemblyFileName = FindLibrary(Namespace.GetNamespace(name) + ".dll");
            if(assemblyFileName == null) { return null; }

            // Strip off namespace from action/type name
            name = Namespace.GetName(name);

            // Try to open the native action dll
            Assembly actionAssembly = null;

            try
            {
                actionAssembly = Assembly.LoadFrom(assemblyFileName);
            }
            catch(Exception e)
            {
                throw(new ReflectionException("Error loading " + assemblyFileName + ": " + e.Message));
            }

            Type[] types;
            try
            {
                types = actionAssembly.GetTypes();
            }
            catch(Exception)
            {
                throw(new ReflectionException("Error loading types from " + assemblyFileName));
            }

            foreach(System.Type t in types)
            {
                if(t.IsClass == true)
                {                    
                    if(t.Name == name)
                    {
                        if(t.GetInterface(typeof(IVariable).FullName) != null)
                        {
                            LogWriter log = new LogWriter(TraceLevel.Warning, "MasterScript");
                            return CreateNativeType(t, log);
                        }
                        else
                        {
                            return actionAssembly.CreateInstance(t.FullName, false);
                        }
                    }
                }
            }

            return null;
        }

        public static IVariable CreateNativeType(Type varType, LogWriter log)
        {
            // First try to call a constructor which takes a LogWriter
            ConstructorInfo cInfo = varType.GetConstructor(new Type[] { typeof(LogWriter) });
            if(cInfo != null)
            {
                return cInfo.Invoke(new object[] { log }) as IVariable;
            }
            else
            {
                // Use default
                cInfo = varType.GetConstructor(System.Type.EmptyTypes);
                if(cInfo != null)
                {
                    return cInfo.Invoke(null) as IVariable;
                }
            }
            return null;
        }

        public static string FindLibrary(string filename)
        {
            Debug.Assert(fwActionDir != null, "Library Manager not initialized");
            Debug.Assert(appActionDir != null, "Library Manager not initialized");
            Debug.Assert(appTypeDir != null, "Library Manager not initialized");
            Debug.Assert(appLibDir != null, "Library Manager not initialized");
            
            if ((fwActionDir.Exists == false) ||
                (appActionDir.Exists == false) ||
                (appTypeDir.Exists == false) ||
                (appLibDir.Exists == false))
            {
                // Application archive is corrupt or obsolete
                return null;
            }

            // Try to find it in the framework
            FileInfo[] files = fwActionDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            files = fwTypeDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            // Try to get it from the action packages
            files = appActionDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            files = appTypeDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            files = appLibDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            // Search in the application's custom code directory
            files = appCodeDir.GetFiles(filename);
            if(files.Length > 0)
            {
                return files[0].FullName;
            }

            return null;
        }

	}
}

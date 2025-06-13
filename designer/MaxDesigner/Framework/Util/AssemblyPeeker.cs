using System;
using System.IO;
using System.Reflection;
using Metreos.Interfaces;
using Metreos.Max.Core;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore;
using Metreos.ProviderFramework;

namespace Metreos.Max.Framework
{
    /// <summary> Loads an assembly and determines its type </summary>
    public class AssemblyPeeker
    {
        public bool Success { get { return success; } }
        public bool MultipleTypesFound { get { return multi; } } 
        public AssemblyType Type { get { return type; } }

        private AssemblyType type;
        private bool success;
        private bool multi;
    
        public AssemblyPeeker(string assemblyPath)
        {
            try
            {
                Assembly assembly = null;
                using(FileStream stream = File.Open(assemblyPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    assembly = Assembly.Load(buffer);
                }
                Type[] types = assembly.GetExportedTypes();

                CheckForMetreosTypes(types);
            }
            catch(FileNotFoundException fnfe)
            {
                throw fnfe;
            }
            catch(BadImageFormatException)
            {
                type |= AssemblyType.Other; 
            }
            catch(Exception) { success = false; }
        }


        private void CheckForMetreosTypes(Type[] types)
        {
            if(types == null || types.Length == 0)
            {
                type |= AssemblyType.Other;
                return;
            }

            bool found = false;

            if(HasType(types, typeof(INativeAction)))
            {
                type |= AssemblyType.NativeAction;
                found = true;
            }

            if(HasType(types, typeof(IVariable)))
            {
                type |= AssemblyType.NativeType;
                if(found) multi = true;
                found = true;
            }

            if(HasType(types, typeof(Metreos.ProviderFramework.IProvider)))
            {
                type |= AssemblyType.Provider;
                if(found) multi = true;
                found = true;
            }

            if(!found)  type |= AssemblyType.Other;
        }


        private bool HasType(Type[] types, Type typeToFind)
        {
            if(types == null || types.Length == 0) return false; 

            foreach(Type type in types)
                if(typeToFind.IsAssignableFrom(type))
                    return true;
       
            return false;
        }
    }

    [Flags()]
    public enum AssemblyType
    {
        NativeAction = 1,
        NativeType = 2,
        Provider = 4,
        Other = 8,
        None = 16
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Metreos.Utilities
{
	/// <summary>
	///     Provides common logic needed when dealing with .NET assemblies
	/// </summary>
	public class AssemblyUtl
	{
        public static string[] SortByDependencies(string[] references)
        {
            Hashtable referencedAssemblies = null;
            return SortByDependencies(references, out referencedAssemblies);
        }

        /// <summary>Given a list of full paths to dlls, returns the same list ordered by dependency
        ///          assemblyReferencesByPath -- paths of the referenced assemblies, given key path of assembly
        ///          For the hashtable, use the reference paths returned by this method to key!</summary>
        public static string[] SortByDependencies(string[] references, out Hashtable assemblyReferencesByPath)
        {
            assemblyReferencesByPath = new Hashtable();
            ArrayList skipDependenciesList = new ArrayList();
            SortedList list = new SortedList(DependencyComparer.Create());

            foreach(string reference in references)
            {
                try
                {
                    Assembly assembly = null;
                    using(FileStream stream = File.Open(reference, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        assembly = Assembly.Load(buffer);
                    }
                    
                    list[assembly] = reference;
                }
                catch
                {
                    skipDependenciesList.Add(reference);
                }
            }

            IList orderedList = list.GetValueList();
            ArrayList orderedReferences = new ArrayList();
            foreach(string reference in orderedList)
            {
                orderedReferences.Add(reference);
            }
      
            string[] orderedDependencies = (string[]) orderedReferences.ToArray(typeof(string));

            string[] skippedDependencies = (string[]) skipDependenciesList.ToArray(typeof(string));

            string[] allDependencies = new string[(orderedDependencies != null ? orderedDependencies.Length : 0) + 
                (skippedDependencies != null ? skippedDependencies.Length : 0)];

            if (allDependencies.Length == 0) return null;

            skippedDependencies.CopyTo(allDependencies, 0);
            orderedDependencies.CopyTo(allDependencies, skippedDependencies.Length);
     

            return allDependencies;
        }

        #region DependencyComparer

        /// <summary>Used to sort assemblies by dependency chain</summary>
        protected class DependencyComparer : IComparer
        {
            private DependencyComparer() { }

            public static DependencyComparer Create()
            {
                return new DependencyComparer();
            }

            public int Compare(object x, object y)
            {
                Assembly firstAssembly = x as Assembly;
                Assembly secondAssembly = y as Assembly;

                if (firstAssembly  == null) return 1;
                if (secondAssembly == null) return -1;

                bool firstReferencesSecond = CheckForReference(firstAssembly, secondAssembly);

                if (firstReferencesSecond) return 1;

                bool secondReferencesFirst = CheckForReference(secondAssembly, firstAssembly);

                if (secondReferencesFirst) return -1;

                return -1;
            }


            /// <summary>Checks if the first assembly references the second assembly</summary>
            protected bool CheckForReference(Assembly firstAssembly, Assembly secondAssembly)
            {
                AssemblyName[] references = firstAssembly.GetReferencedAssemblies();
                if(references == null)   return false;

                foreach(AssemblyName assembly in references)
                    if(assembly.FullName == secondAssembly.FullName)
                        return true;

                return false;
            }
        }
        #endregion
	}
}

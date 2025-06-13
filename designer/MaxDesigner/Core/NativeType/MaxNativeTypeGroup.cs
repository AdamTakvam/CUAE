using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;

namespace Metreos.Max.Core.NativeType
{
    /// <summary> A collection of MaxNativeType's </summary>
    /// <remarks>
    ///  Shares a 1:1 ratio logically with the Metreos Application Server
    ///  nativeTypePackageType. It was originally designed to keep
    ///  a reference to the original nativeTypePackageType, solely
    ///  to fulfull the requirements of a MaxPropertiesManager callback,
    ///  as MPM and MaxDesigner can both reference Metreos.Samoa, it makes
    ///  life much easier in terms of passing data between the two
    ///  assemblies.  The "RawPackage" could be dumped, in the case
    ///  MPM gets shifted to the MaxDesigner project.
    /// </remarks>
    public class MaxNativeTypeGroup : CollectionBase, ICollection
    {
        private static XmlSerializer typesDeserializer   
                 = new XmlSerializer(typeof(nativeTypePackageType));

        private string path;
        private string name;
        private nativeTypePackageType rawPackage;
        private bool isFrameworkPackage;
        private bool dllImported;

        /// <summary> The location of the file that this collecton uses to load itself from </summary>
        public  string FilePath     { get { return path; } }
    
        /// <summary> The name of the native type package. Analogous to a namespace </summary>
        public  string Name         { get { return name; } }

        /// <summary> The Metreos Samoa version of this package</summary>
        /// <remarks></remarks>
        public nativeTypePackageType RawPackage { get { return rawPackage; } }

        /// <summary> Is a package defined in the framework </summary>
        public bool IsFrameworkPackage { get { return isFrameworkPackage; } }
		
        /// <summary> Creates a collection of nativeTypes, bound by the common namespace they represent </summary>
        /// <param name="xmlFilePath"> The path of the file containing the native types package </param>
        public MaxNativeTypeGroup(string xmlFilePath) : base()
        {
            this.dllImported = false;
            this.isFrameworkPackage = Utl.IsSameDirectory(xmlFilePath, Config.PackagesFolder + Path.DirectorySeparatorChar);
            this.path = xmlFilePath;
        }

        public MaxNativeTypeGroup(string dllPath, nativeTypePackageType package) : base()
        {
            this.dllImported = true;
            this.isFrameworkPackage = Utl.IsSameDirectory(dllPath, Config.PackagesFolder + Path.DirectorySeparatorChar);
            this.path = dllPath;
            this.rawPackage = package;
        }

        /// <summary> Populates a MaxNativeTypeCollection with the types 
        /// found in a packaged native type file </summary>
        public bool Load()
        {
            if(!dllImported)
            {
                // Retrieve all types from the package
                rawPackage = DeserializeNativeType(path);
            }

            // Something has gone awry in deserializing, so load failed.
            if(rawPackage == null) return false; 

            name = rawPackage.name;  

            // Empty package, but does not indicate failure.
            if(rawPackage.type == null)  return true;
      
            foreach(typeType nativeType in rawPackage.type)
            {
                MaxNativeType wrappedType = new MaxNativeType(this, nativeType);

                this.List.Add(wrappedType);
            }

            return true;
        }


        /// <summary> Provides a native type package given the path of the file </summary>
        /// <returns> Null if failed to load package </returns>
        public static nativeTypePackageType DeserializeNativeType(string filepath)
        {
            FileStream file = null;
            XmlReader xmlReader = null;

            try
            {
                file = new FileStream(filepath, FileMode.Open);

                xmlReader = new XmlTextReader(file);
      
                if(typesDeserializer.CanDeserialize(xmlReader))
                {
                    return (nativeTypePackageType) typesDeserializer.Deserialize(xmlReader);
                }
            }
            catch(Exception e)
            {
                if(! MaxMain.autobuild)
                    MaxManager.Instance.SignalFrameworkTextMessage(e.Message, true, false); 
            }
            finally
            {
                if (xmlReader != null) xmlReader.Close();                
                if (file      != null) file.Close();   
            }

            return null;
        }

    } // MaxNativeTypeGroup
} // namespace Metreos.Max.Core.NativeType

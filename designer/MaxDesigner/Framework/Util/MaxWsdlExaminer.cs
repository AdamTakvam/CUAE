using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.XPath;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Utilities;

namespace Metreos.Max.Framework
{
    /// <summary>Identifies content of a web services definition file.  
    /// Will attempt to download/load additional files if the WSDL file references 
    /// external definition files (such as .xsd external files) </summary>
    public class MaxWsdlExaminer
    {
        public int ServiceCount { get { return services == null? 0: Services.Length; } }
        public int MethodCount  { get { return services == null? 0: Methods.Length;  } }

        /// <summary> The wsdl services as determined from within the wsdl file </summary>
        public Service[] Services { get { return services; } }

        /// <summary> Accesses all the methods described by this web service </summary>
        public Method[] Methods   { get { return methods; } }
    
        public string[] MethodStrings 
        {
            get
            {   int i = 0;
                string[] s = new string[MethodCount];
                if (Methods != null)  
                    foreach(Method method in Methods) s[i++] = method.Name;
                return s;
            }
        }

        /// <summary> Any additional file references that this WSDL file needs 
        ///   (it will explicitly reference these files) </summary>
        public string[] ExternalReferences { get { return externalReferences; } }

        protected const string definitionName = "definitions";
        protected const string portTypeName   = "portType";
        protected const string operationName  = "operation";
        protected const string serviceName    = "service";
        protected const string xmlAttrName    = "name";

        protected Method[]  methods;
        protected Service[] services;
        protected string path;
        protected string name;
        protected string originalPath;
		protected string errorLogPath;
        protected string[] externalReferences;


        /// <summary> Creates a simple parser to examine a web service </summary>
        /// <param name="filepath"> The path to the wsdl file </param>
        /// <param name="originallyLocal"> Original path to aid in potential external, relative paths referenced 
        ///  in the WSDL file </param>
        public MaxWsdlExaminer(string filepath, string originalPath, string errorLogPath)
        {
            this.path = filepath;
            this.name = null;
            this.methods = null;
            this.services = null;
            this.originalPath = originalPath;
			this.errorLogPath = errorLogPath;
        }


        /// <summary> Loads and examines the wsdl file</summary>
        public bool Examine()
        {
            XmlDocument doc  = new XmlDocument();

            bool result = false;

            try
            {
                doc.Load(path); 
                result = true;  
            }
            catch(Exception e)
			{ 
				AppendToLog(Const.WsdlXmlLoadError, e);
			}
     
            if (!result) return false; 

            try
            {
                ExamineExternalFiles(doc);
            }
            catch { result = false; } 

            if (!result) return false;

            XmlNode definitionNode = MoveToNode(doc, definitionName);

            if (definitionNode == null) return true;

            try
            {
                ExamineServices(definitionNode);
                ExamineMethods (definitionNode);
            }
            catch { result = false; }

            return result;
        }

        
        /// <summary> Finds all methods and stores their info </summary>
        /// <param name="definitionNode"> The 'definition' node from the wsdl file </param>
        protected void ExamineMethods(XmlNode definitionNode)
        {
            ArrayList methods = new ArrayList();

            XmlNode[] portTypeNodes = GetAllChildElementsOfName(definitionNode, portTypeName);
            if (portTypeNodes == null) return;

            foreach(XmlNode portTypeNode in portTypeNodes)
            {
                XmlNode[] operationNodes = GetAllChildElementsOfName(portTypeNode, operationName);
                if (operationNodes == null) return;

                foreach(XmlNode operationNode in operationNodes)
                {
                    string name = GetAttribute(operationNode, xmlAttrName);
                    if(name != null) methods.Add(new Method(name));
                }
            }

            this.methods = methods.ToArray(typeof(Method)) as Method[];
        }


        /// <summary> Searches for any import references, and if found, continues to search for 
        ///           additional external references </summary>
        /// <param name="document"> The loaded WSDL document </param>
        protected void ExamineExternalFiles(XmlDocument document)
        {
            XmlNodeList importNodes = GetWsdlImportNodes(document);

            // If there is no import directive, there are no important external files, 
            // so stop the search process
            if (importNodes == null || importNodes.Count == 0) return;

            Uri originalUri = null;
            try
            {
                originalUri = new Uri(originalPath);
            } 
            catch { return; }   

            ArrayList allExternalRefs = new ArrayList();

            foreach(XmlNode importNode in importNodes)
            {
                XmlAttribute locationAttr = importNode.Attributes[Const.xmlAttrWsdlLocation];

                if (locationAttr == null)
                {
                    locationAttr = importNode.Attributes[Const.xmlAttrXsdLocation];
                }
                if (locationAttr == null) continue;

                string xsdFile = ReserveExternalFile(locationAttr.Value, originalPath);

                if(xsdFile == null) continue;

                allExternalRefs.Add(xsdFile);

                // Determine if this XSD file refers to any other external XSD files
                string[] additionalRefsFromXsd = ParseXsdForExternalFiles(xsdFile, originalPath);
        
                if(additionalRefsFromXsd == null) continue;

                allExternalRefs.AddRange(additionalRefsFromXsd);
            }

            if (allExternalRefs.Count == 0) this.externalReferences = null;
      
            this.externalReferences = allExternalRefs.ToArray(typeof(string)) as string[];
        }


		protected void AppendToLog(string title, Exception e)
		{       
			FileInfo file = new FileInfo(errorLogPath);
			FileStream stream = null;
			StreamWriter writer = null;
			try
			{
				stream = file.Open(FileMode.Append, FileAccess.Write);
				writer = new StreamWriter(stream);
				writer.Write(String.Format("{0} \r\n\r\n {1}\r\n\r\n", title, Metreos.Utilities.Exceptions.FormatException(e)));
			}
			catch { }
			finally
			{
				if(writer != null)
				{
					writer.Close();
				}
			}
		}

        /// <summary>
        /// Reads an XSD file for any other XSD file that it might reference, 
        /// and continues the search for external XSD files in that file
        /// </summary>
        protected string[] ParseXsdForExternalFiles(string xsdFile, string originalPath)
        {
            XmlDocument doc  = new XmlDocument();

            bool result = false;

            try
            {
                doc.Load(xsdFile); 
                result = true;  
            }
            catch(Exception e)
			{ 
				AppendToLog(Const.XsdXmlLoadError, e);
			}
     
            if (!result) return null; 

            XmlNodeList importNodes = GetXsdImportNodes(doc);

            // If there is no import directive, there are no important external files, so stop the search process
            if (importNodes == null || importNodes.Count == 0) return null;

            Uri originalUri = null;
            try
            {
                originalUri = new Uri(originalPath);
            } 
            catch { return null; }   

            ArrayList allExternalRefs = new ArrayList();

            foreach(XmlNode importNode in importNodes)
            {
                XmlAttribute locationAttr = importNode.Attributes[Const.xmlAttrXsdLocation];

                if(locationAttr == null)  continue;

                string anotherXsdFile = ReserveExternalFile(locationAttr.Value, originalPath);

                if (anotherXsdFile == null) continue;

                allExternalRefs.Add(anotherXsdFile);

                // Determine if this XSD file refers to any other external XSD files
                string[] additionalRefsFromXsd = ParseXsdForExternalFiles(anotherXsdFile, originalPath);
        
                if (additionalRefsFromXsd == null) continue;

                allExternalRefs.AddRange(additionalRefsFromXsd);
            }

            if(allExternalRefs.Count == 0)  return null;

            string[] externalRefs = allExternalRefs.ToArray(typeof(string)) as string[];

            return externalRefs;
        }


        /// <summary>
        ///   Returns the local file path containing the external file reference
        /// </summary>
        /// <param name="relAbsLocation"> A relative or absolute path to an external file reference, and can be
        ///  located on the web via HTTP </param>
        /// <param name="originalPath"> The path of the original WSDL file </param>
        protected string ReserveExternalFile(string relAbsLocation, string originalPath)
        {
            string filepath       = null;
            Uri originalUri       = null;
            Uri relAbsLocationUri = null;
            bool isRelative;
            try
            {
                originalUri = new Uri(originalPath);
            } 
            catch { return null; }
            try
            {
                relAbsLocationUri = new Uri(relAbsLocation);
                isRelative = false;
            }
            catch 
            { 
                isRelative = true;
            }
  
            if (originalUri.IsFile)
            {
                if  (isRelative)  
                     filepath = Path.Combine(Path.GetDirectoryName(originalPath), relAbsLocation);
                else filepath = relAbsLocationUri.AbsolutePath.Replace('/', '\\');
            }
            else
            {
                string url;
                if (isRelative)
                {
                    string directoryPath = String.Join(String.Empty, originalUri.Segments, 0, originalUri.Segments.Length - 1);
                    url = directoryPath + "/" + relAbsLocation;
                }
                else url = relAbsLocationUri.ToString();

				filepath          = Path.GetTempFileName(); 
                filepath          = Path.ChangeExtension(filepath, Const.XsdFileExtension);
                UrlStatus status  = Metreos.Utilities.Web.Download(url, filepath);

                if (status != UrlStatus.Success) return null;
            }

            return filepath;
        }


        /// <summary> Finds all services and stores their info </summary>
        /// <param name="definitionNode"> The 'definition' node from the wsdl file </param>
        protected void ExamineServices(XmlNode definitionNode)
        {
            ArrayList services = new ArrayList();

            XmlNode[] serviceNodes = GetAllChildElementsOfName(definitionNode, serviceName);
            if (serviceNodes == null) return;

            foreach(XmlNode serviceNode in serviceNodes)
            {
                string name = GetAttribute(serviceNode, xmlAttrName);
                if (name != null)    
                    services.Add(new Service(name));
            }

            this.services = services.ToArray(typeof(Service)) as Service[];
        }


        /// <summary> Moves to a child node of the given name </summary>
        /// <param name="node"> Node to use for child node collection </param>
        /// <param name="name"> Node name </param>
        /// <returns> The first child node encountered </returns>
        protected XmlNode MoveToNode(XmlNode node, string name)
        {
            foreach(XmlNode childNode in node.ChildNodes)
                if(childNode.LocalName == name) return childNode;

            return null;
        }


        protected XmlNodeList GetWsdlImportNodes(XmlNode node)
        {
            XmlNode defNode = MoveToNode(node, Const.xmlEltWsdlDef);

            if(defNode == null) return null;

            XmlNodeList list = defNode.SelectNodes("//*[local-name()='import']");

            if (list == null || list.Count == 0) return null;

            return list;
        }


        protected XmlNodeList GetXsdImportNodes(XmlNode node)
        {
            XmlNode schemaNode = MoveToNode(node, Const.xmlEltXsdSchema);

            if (schemaNode == null) return null;

            XmlNodeList list = schemaNode.SelectNodes("//*[local-name()='import' or local-name()='include']");

            if (list == null || list.Count == 0) return null;

            return list;
        }


        /// <summary> Returns a collection of child nodes based on the element name</summary>
        protected XmlNode[] GetAllChildElementsOfName(XmlNode node, string name)
        {
            ArrayList childNodes = new ArrayList();

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == name) childNodes.Add(childNode as XmlNode);
            }

            return childNodes.Count == 0? null: childNodes.ToArray(typeof(XmlNode)) as XmlNode[];
        }


        /// <summary> Cleaner interface than the XmlNode.Attributes accessor</summary>
        protected string GetAttribute(XmlNode node, string attrName)
        {
            XmlAttribute nameAttr = node.Attributes[attrName];

            return(nameAttr == null || nameAttr.Value == null || nameAttr.Value == String.Empty)?
                   null: nameAttr.Value;
        }


        /// <summary> Storage class for methods </summary>
        public class Method
        {
            public  string Name { get { return name; } }
            private string name; 

            public Method(string name)
            {
                this.name = name;
            }
        }


        /// <summary> Storage class for services </summary>
        public class Service
        {
            public  string Name { get { return name; } }
            private string name; 

            public Service(string name)
            {
                this.name = name;
            }
        }
    }  
}    

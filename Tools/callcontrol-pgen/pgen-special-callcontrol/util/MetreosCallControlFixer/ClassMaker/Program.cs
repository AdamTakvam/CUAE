using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

using Metreos.PackageGeneratorCore.PackageXml;

namespace ClassMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PackageInfo> packages = CollectPackages(@"x:\build\framework\1.0\Packages\");

            CodeNamespace ns = new CodeNamespace("Metreos.Interfaces.PackageDefinitions");
            ns.Imports.Add(new CodeNamespaceImport("System"));

            foreach (PackageInfo packageInfo in packages)
            {
                packageType package = packageInfo.package;

                string[] packageBits = package.name.Split(new char[] {'.'});
                string prunedPackageName = packageBits[packageBits.Length - 1];

                CodeTypeDeclaration topLevelClass = new CodeTypeDeclaration(prunedPackageName);
                topLevelClass.Attributes = MemberAttributes.Abstract & MemberAttributes.Public;

                CodeTypeDeclaration globalsClass = new CodeTypeDeclaration("Globals");
                globalsClass.Attributes = MemberAttributes.Abstract & MemberAttributes.Public;

                CodeMemberField namespaceField = new CodeMemberField(typeof(string), "NAMESPACE");
                namespaceField.Attributes = MemberAttributes.Public | MemberAttributes.Const;
                namespaceField.InitExpression = new CodePrimitiveExpression(package.name);

                CodeMemberField packageNameField = new CodeMemberField(typeof(string), "PACKAGE_NAME");
                providerNameField.Attributes = MemberAttributes.Public | MemberAttributes.Const;
                providerNameField.InitExpression = new CodePrimitiveExpression(package.name);

                CodeMemberField descriptionField = new CodeMemberField(typeof(string), "PACKAGE_DESCRIPTION");
                providerNameField.Attributes = MemberAttributes.Public | MemberAttributes.Const;
                providerNameField.InitExpression = new CodePrimitiveExpression(package.name);

                globalsClass.Members.Add(namespaceField);
                globalsClass.Members.Add(packageNameField);
                globalsClass.Members.Add(descriptionField);

                topLevelClass.Members.Add(globalsClass);


                if (package.actionList != null && package.actionList.Length > 0)
                {
                    CodeTypeDeclaration actionClass = new CodeTypeDeclaration("Actions");
                    actionClass.Attributes = MemberAttributes.Abstract & MemberAttributes.Public;

                    foreach (actionType action in package.actionList)
                    {
                        
                    }
                }


            }

            // Create top level container class.
            CodeTypeDeclaration topLevelClass = new CodeTypeDeclaration(opts.topLevelName);

            
            ns.Types.Add(topLevelClass);

            // Create class for max solution
            if(maxApps != null)
            {
                foreach(MaxApp maxApp in maxApps)
                {
                    CodeTypeDeclaration maxSolutionClass = new CodeTypeDeclaration(maxApp.Name);
                    
                    maxSolutionClass.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
                    
                    AppendNameProperty(maxSolutionClass, maxApp.Name);
                    AppendFullNamePropertyToSolution(maxSolutionClass, maxApp.Name);
                    
                    CreateScripts(maxSolutionClass, maxApp);

                    topLevelClass.Members.Add(maxSolutionClass);
                }
            }
        }

        private static List<PackageInfo> CollectPackages(string pathtoPackages)
        {
            List<PackageInfo> packages = new List<PackageInfo>();
            
            if (Directory.Exists(pathtoPackages))
            {
                string[] files = Directory.GetFiles(pathtoPackages);

                foreach (string pathToPackageXml in files)
                {
                    FileStream stream = null;

                    try
                    {
                        stream = File.OpenRead(pathToPackageXml);
                    }
                    catch (Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Exception in opening file {0} for reading. {1}", pathToPackageXml, e);
                    }

                    packageType package = null;

                    try
                    {
                        if (stream != null)
                        {
                            XmlSerializer seri = new XmlSerializer(typeof(packageType));
                            package = seri.Deserialize(stream) as packageType;
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLog(TraceLevel.Error, "Exception in deserializing package XML in file {0}.  {1}", pathToPackageXml, e);
                    }

                    if(package != null)
                    {
                        packages.Add(new PackageInfo(package, pathToPackageXml));
                    }
                }
            }
            else
            {
                WriteLog(TraceLevel.Error, "Unable to find the packages directory at location: {0}", pathtoPackages);
            }
                
            return packages;
        }
    }

    public class PackageInfo
    {
        public PackageInfo(packageType package, string packageFilepath)
        {
            this.package = package;
            this.packageFilepath = packageFilepath;
        }

        public packageType package;
        public string packageFilepath;
    }
}

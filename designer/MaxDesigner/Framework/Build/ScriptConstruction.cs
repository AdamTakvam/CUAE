using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Core;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using PropertyGrid.Core;

namespace Metreos.Max.Framework
{
    public delegate void UpdateErrorStatus(bool status);

    public class ScriptConstruction
    {
        /// <summary>Max-specific: everything works in default mode</summary>
        public static XmlScriptData[] ConstructScripts(string[] scriptPaths,
            string[] usings, string[] references, out ErrorInformation[] errors, 
            out ErrorInformation[] warnings)
        {
            ConstructionErrorInformation constructionErrors = new ConstructionErrorInformation();
            ConstructionWarningInformation constructionWarnings = new ConstructionWarningInformation();

            XmlScriptData[] scripts = new XmlScriptData[scriptPaths.Length];

            for(int i = 0; i < scriptPaths.Length; i++)
            {
                MaxMainUtil.PeekAppFile(scriptPaths[i]);
        
                ScriptParser parser = new ScriptParser(scriptPaths[i]);

                MaxMain.MessageWriter.WriteLine(ConstructionConst.PreparingScript(parser.Name));

                parser.Parse();

                // If there are any errors, display the errors here, as that makes it 
                // easiest to know which script the error occurred in

                int lineCount = 0;

                if(parser.Warning)
                {
                    constructionWarnings.AddWarnings(parser.Warnings);

                    // Iterate through all warnings, logging them to the Max Output Window
                    if (parser.Warnings.Length > 0)
                    {
                        foreach(ErrorInformation e in parser.Warnings)
                        {
                            string errormsg = GenerateErrorMessage(e, ref lineCount);     
                            MaxMain.MessageWriter.WriteLine(errormsg);           
                        }     
                    }
                } 

                if(parser.Error)
                {
                    constructionErrors.AddErrors(parser.Errors);

                    // Iterate through all errors, logging them to the Max Output Window
                    if (parser.Errors.Length > 0)
                    {
                        foreach(ErrorInformation e in parser.Errors)
                        {
                            string errormsg = GenerateErrorMessage(e, ref lineCount);   
                            MaxMain.MessageWriter.WriteLine(errormsg);           
                        }     
                    }
                }
        
                scripts[i] = null;
          
                if(parser.Error) // Can't compile if the parsing fails.
                {
                    // Output a 'build failed' for script message.
                    MaxMain.MessageWriter.WriteLine(ConstructionConst.BuildEndedForScript(parser.Name, false)); 

                    continue;
                }

                // Output that we are beginning the construction of this script
                MaxMain.MessageWriter.WriteLine(ConstructionConst.ConstructingScript(parser.Name));

                // Need packages at this point.
                if(MaxMain.autobuild)
                    MaxPackages.Instance.Load();

                ScriptCompiler compiler = new ScriptCompiler(parser.Map, NumberingScheme.RetainMaxNodes, usings, 
                    references);
        
                compiler.Compile();

                lineCount = 0;

                if(compiler.Error)
                {
                    constructionErrors.AddErrors(compiler.Errors);

                    // Iterate through all warnings, logging them to the Max Output Window
                    if (compiler.Errors.Length > 0)
                    {
                        foreach(ErrorInformation e in compiler.Errors)
                        {
                            string errormsg = GenerateErrorMessage(e, ref lineCount);        
                            MaxMain.MessageWriter.WriteLine(errormsg);           
                        }     
                    }
                }

                if(compiler.Warning)
                {
                    constructionWarnings.AddWarnings(compiler.Warnings);

                    // Iterate through all warnings, logging them to the Max Output Window
                    if (compiler.Warnings.Length > 0)
                    {
                        foreach(ErrorInformation e in compiler.Warnings)
                        {
                            string errormsg = GenerateErrorMessage(e, ref lineCount);        
                            MaxMain.MessageWriter.WriteLine(errormsg);           
                        }     
                    }
                }

                scripts[i] = compiler.Script; 
 
                // Output a 'build succeeded' or 'build failed' for script message.
                MaxMain.MessageWriter.WriteLine(ConstructionConst.BuildEndedForScript(parser.Name, !compiler.Error)); 
            }

            errors = constructionErrors.Errors;
            warnings = constructionWarnings.Warnings;

            return scripts;
        }


        /// <summary> Creates a formatted errormessage </summary>
        private static string GenerateErrorMessage(ErrorInformation e, ref int lineCount)
        {
            string errormsg;
            if(e.ContainingFunction != null && e.ContainingFunction != String.Empty)
            {
                errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage + Const.blank + 
                    ConstructionConst.IN + Const.blank + e.ContainingFunction;
            }
            else
            {
                errormsg = Const.blank + (++lineCount) + Const.dotb + e.ErrorMessage;
            }
        
            if(e.NodesInError != null)
            {
                errormsg += Const.colon + Const.blank;

                for(int i = 0; i < e.NodesInError.Length; i++)
                {
                    errormsg += e.NodesInError[i].NodeName + Const.blank + ConstructionConst.openParen + 
                        e.NodesInError[i].NodeId + ConstructionConst.closeParen;

                    if(i < e.NodesInError.Length - 1)
                        errormsg += ConstructionConst.comma + Const.blank;
                }
            }

            return errormsg;   
        }


        /// <summary>Generic construct scripts option</summary>
        public static void ConstructScripts(ConstructionOptions options)
        {
        }


        public static void ConstructPackage()
        {
        }
    }


    public class ConstructionOptions
    {
        /// <summary>
        /// True:  An outputDir\obj and outputDir\obj\bin dir will be made. Scripts go into obj, package goes into bin
        /// False:  Flat: scripts and package go into outputDir.
        /// </summary>
        public bool DefaultFileMode { get { return defaultFileStructure; } set { defaultFileStructure = value; } }
        private bool defaultFileStructure;
        public ConstructionOptions()
        {
            defaultFileStructure = true;
        }
    }

    #region Error and Warning Classes

    public class ConstructionWarningInformation
    {
        /// <summary>Returns accumulated errors for Application Construction</summary>
        public ErrorInformation[] Warnings 
        {
            get 
            {
                ErrorInformation[] warnings = new ErrorInformation[warningsGrowable.Count];
                warningsGrowable.CopyTo(warnings);
                return warnings.Length != 0 ? warnings : null; 
            }
        }
        private ArrayList warningsGrowable;
        private UpdateErrorStatus warningOccured;

        public ConstructionWarningInformation()
        {
            warningsGrowable = new ArrayList();
            this.warningOccured = null;
        }

        /// <param name="warningOccured">Will get called if a warning is recorded</param>
        public ConstructionWarningInformation(UpdateErrorStatus warningOccured)
        {
            warningsGrowable = new ArrayList();
            this.warningOccured = warningOccured;
        }

        public void AddWarning(string warningMessage)
        {
            if(warningOccured != null) { warningOccured(true); }
            ErrorInformation warningInformation = new ErrorInformation(warningMessage, null, null);
            warningsGrowable.Add(warningInformation);
        }

        public void AddWarning(string warningMessage, string functionName)
        {
            if(warningOccured != null) { warningOccured(true); }
            ErrorInformation warningInformation = new ErrorInformation(warningMessage, functionName, null);
            warningsGrowable.Add(warningInformation);
        }

        public void AddWarning(string warningMessage, string functionName, NodeInfo nodeId)
        {
            if(warningOccured != null) { warningOccured(true); }
            ErrorInformation warningInformation = new ErrorInformation(warningMessage, functionName, new NodeInfo[] { nodeId });
            warningsGrowable.Add(warningInformation);
        }
        public void AddWarning(string warningMessage, string functionName, NodeInfo[] nodeIds)
        {
            if(warningOccured != null) { warningOccured(true); }
            ErrorInformation warningInformation = new ErrorInformation(warningMessage, functionName, nodeIds);
            warningsGrowable.Add(warningInformation);
        }

        public void AddWarnings(ErrorInformation[] warnings)
        {
            if(warnings == null) return;

            if(warningOccured != null) { warningOccured(true); }

            foreach(ErrorInformation warning in warnings)
            {
                warningsGrowable.Add(warning);
            }
        }
    }

    public class ConstructionErrorInformation
    {
        /// <summary>
        /// Returns accumulated errors for Application Construction
        /// </summary>
        public ErrorInformation[] Errors 
        {
            get 
            {
                ErrorInformation[] errors = new ErrorInformation[errorsGrowable.Count];
                errorsGrowable.CopyTo(errors);
                return errors.Length != 0 ? errors : null; 
            }
        }
        private ArrayList errorsGrowable;
        private UpdateErrorStatus errorOccured;

        public ConstructionErrorInformation()
        {
            errorsGrowable = new ArrayList();
            this.errorOccured = null;
        }

        public ConstructionErrorInformation( UpdateErrorStatus errorOccured)
        {
            errorsGrowable = new ArrayList();
            this.errorOccured = errorOccured;
        }

        public void AddError(string errorMessage)
        {
            if(errorOccured != null) { errorOccured(true); }
            ErrorInformation errorInformation = new ErrorInformation(errorMessage, null, null);
            errorsGrowable.Add(errorInformation);
        }

        public void AddError(string errorMessage, string functionName)
        {
            if(errorOccured != null) { errorOccured(true); }
            ErrorInformation errorInformation = new ErrorInformation(errorMessage, functionName, null);
            errorsGrowable.Add(errorInformation);
        }

        public void AddError(string errorMessage, string functionName, NodeInfo nodeInError)
        {
            if(errorOccured != null) { errorOccured(true); }
            ErrorInformation errorInformation = new ErrorInformation(errorMessage, functionName, new NodeInfo[] { nodeInError } );
            errorsGrowable.Add(errorInformation);
        }
        public void AddError(string errorMessage, string functionName, ArrayList nodesInError)
        {
            if(errorOccured != null) { errorOccured(true); }
            NodeInfo[] nodesInErrorStatic = new NodeInfo[nodesInError.Count];
            nodesInError.CopyTo(nodesInErrorStatic);
            ErrorInformation errorInformation = new ErrorInformation(errorMessage, functionName, nodesInErrorStatic);
            errorsGrowable.Add(errorInformation);
        }

        public void AddError(string errorMessage, string functionName, NodeInfo[] nodesInError)
        {
            if(errorOccured != null) { errorOccured(true); }
            ErrorInformation errorInformation = new ErrorInformation(errorMessage, functionName, nodesInError);
            errorsGrowable.Add(errorInformation);
        }

        public void AddErrors(ErrorInformation[] errors)
        {
            if(errors == null) return;

            if(errorOccured != null) { errorOccured(true); }

            foreach(ErrorInformation error in errors)
            {
                errorsGrowable.Add(error);
            }
        }
    }

    public class ErrorInformation
    {
        public NodeInfo[] NodesInError{ get { return nodes; } }
        public string ErrorMessage { get { return errorMessage; } }
        public string ContainingFunction { get { return functionName; } }

        protected NodeInfo[] nodes;
        protected string errorMessage;  
        protected string functionName;

        public ErrorInformation(string errorMessage, string functionName, NodeInfo[] nodeIdsInError)
        {
            this.errorMessage = errorMessage;
            this.functionName = functionName;
            this.nodes = nodeIdsInError;
        }
    }

    public class NodeInfo
    {
        public long NodeId { get { return nodeId; } }
        public string NodeName { get { return nodeName; } }

        private long nodeId;
        private string nodeName;

        public NodeInfo(long nodeId, string fullNodeName)
        {
            this.nodeId = nodeId;

            int startPosition = fullNodeName.LastIndexOf(".");

            if(startPosition < 0)
                this.nodeName = fullNodeName;
            else
                this.nodeName = fullNodeName.Substring(startPosition + 1, fullNodeName.Length - (startPosition + 1));
        }
    }

    #endregion
}
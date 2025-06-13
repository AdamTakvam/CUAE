using System;
using System.Diagnostics;
using Metreos.Interfaces;

namespace Metreos.Max.Framework
{
  /// <summary> Error codes for the build process in Max </summary>
  public sealed class ErrorCodes
  {
    // Critical Generic                                  0100
    public static CodedMessage internalError                 
      = new CodedMessage         
                                                        (0100, "Internal error", TraceLevel.Error);
    public static CodedMessage internalErrorExpected
      = new CodedMessage                                (0101, "Internal error. ({0})", TraceLevel.Error);
    public static CodedMessage xmlDataCorrupt                
      = new CodedMessage                                (0101, "Script XML data is corrupt", TraceLevel.Error);

    // Map                                               0200
    public static CodedMessage duplicateLinkBranch           
      = new CodedMessage                                (0200, "Duplicate branch conditions", TraceLevel.Error);
    public static CodedMessage invalidStartNodeType          
      = new CodedMessage                                (0201, "Start node not Action, Label or Loop", TraceLevel.Error);
    public static CodedMessage noStartNode                   
      = new CodedMessage                                (0202, "No start node", TraceLevel.Error);
    public static CodedMessage danglingActionNode            
      = new CodedMessage                                (0203, "Dangling action node", TraceLevel.Error);
    public static CodedMessage danglingLabelNode             
      = new CodedMessage                                (0204, "Dangling label node", TraceLevel.Error);
    public static CodedMessage nonFinalWithNoOutLink         
      = new CodedMessage                                (0205, "Non-final action must have at least one outbound link", TraceLevel.Error);
    public static CodedMessage noMatchingEndLabel            
      = new CodedMessage                                (0206, "Label has no matching terminal label", TraceLevel.Error);
    public static CodedMessage noMatchingStartLabel          
      = new CodedMessage                                (0207, "Label has no matching start label", TraceLevel.Error);
    public static CodedMessage outLabelMultipleLinks         
      = new CodedMessage                                (0208, "Label has multiple outgoing links", TraceLevel.Error);
    public static CodedMessage undefinedLabelText            
      = new CodedMessage                                (0208, "Label text undefined", TraceLevel.Error);
    public static CodedMessage intersectingLoops             
      = new CodedMessage                                (0209, "Intersecting loops not allowed", TraceLevel.Error);
    public static CodedMessage undefinedTrigger              
      = new CodedMessage                                (0210, "Undefined trigger", TraceLevel.Error);
    public static CodedMessage undefinedEventHandler         
      = new CodedMessage                                (0211, "Undefined event handler", TraceLevel.Error);

    // Action                                            0300
    public static CodedMessage missingRequiredParam 
      = new CodedMessage                                (0300, "Missing required parameter '{0}'", TraceLevel.Error); 
    public static CodedMessage unknownVariableParam      
      = new CodedMessage                                (0301, "Unknown variable '{0}' in parameter '{1}'", TraceLevel.Error);
    public static CodedMessage unknownVariableResult     
      = new CodedMessage                                (0302, "Unknown variable '{0}' in result data '{1}'", TraceLevel.Error);

    // Loops                                             0400
    public static CodedMessage noStartNodeLoop      
      = new CodedMessage                                (0400, "No start node in loop '{0}'", TraceLevel.Error);

    // Variable                                          0500
    public static CodedMessage unusedLocalVariable  
      = new CodedMessage                                (0500, "Unreferenced local variable '{0}'", TraceLevel.Warning);
    public static CodedMessage unusedGlobalVariable 
      = new CodedMessage                                (0501, "Unreferenced global variable '{0}'", TraceLevel.Warning);
    
    // Assembler                                         0600
    public static CodedMessage failedCompile        
      = new CodedMessage                                (0600, "Compile error: {0}", TraceLevel.Error);
    public static CodedMessage failedAssemble       
      = new CodedMessage                                (0601, "Could not assemble application", TraceLevel.Error);

    // Misc                                              0700
    public static CodedMessage noPackages                    
      = new CodedMessage                                (0700, "No packages associated with project", TraceLevel.Warning);
    public static CodedMessage generatedLogMultiLink         
      = new CodedMessage                                (0701, "Generated log message with multiple outgoing links", TraceLevel.Error);
    public static CodedMessage appPackageNotFound            
      = new CodedMessage                                (0702, "Application package not found", TraceLevel.Error);
    public static CodedMessage appPackageLock                
      = new CodedMessage                                (0703, "Could not access application package", TraceLevel.Error);
    public static CodedMessage appPackageRead                
      = new CodedMessage                                (0704, "Could not parse application package", TraceLevel.Error);
    public static CodedMessage unknownPackage       
      = new CodedMessage                                (0705, "No package found for action '{0}'", TraceLevel.Error);
    public static CodedMessage unknownActionInPackage 
      = new CodedMessage                                (0706, "Action '{0}' not found in package '{1}'", TraceLevel.Warning);

    // Max Project File                                  0800
    public static CodedMessage multipleInstallers            
      = new CodedMessage                                (0800, "Multiple installers not permitted", TraceLevel.Error);

    // Deployment                                        0900
    public static CodedMessage inaccessible                  
      = new CodedMessage                                (0900, "Application Server inaccessible", TraceLevel.Error);
    public static CodedMessage loginFailure                  
      = new CodedMessage                                (0901, "Could not log into Application Server", TraceLevel.Error);
    public static CodedMessage appWillNotInstall 
      = new CodedMessage                                (0902, "Package could not be installed", TraceLevel.Error);
    public static CodedMessage appWillNotUninstall
      = new CodedMessage                                (0903, "Package could not be uninstalled", TraceLevel.Error);
    public static CodedMessage getApps                       
      = new CodedMessage                                (0904, "Could not enumerate running applications", TraceLevel.Error);
    public static CodedMessage disableApp                    
      = new CodedMessage                                (0905, "Could not disable existing application", TraceLevel.Error);
    public static CodedMessage uninstallApp                  
      = new CodedMessage                                (0906, "Could not uninstall existing application", TraceLevel.Error);

  }
}

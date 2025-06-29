using System;
using System.Diagnostics;

namespace Metreos.Interfaces
{
    public abstract class IErrors
    {
        #region Max Errors
        // Critical Generic                                    0100
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
        public static CodedMessage missingUserData      
            = new CodedMessage                                (0303, "Missing " + ICommands.Fields.USER_DATA + " parameter in an asychronous action", TraceLevel.Error);

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
            = new CodedMessage                                (0705, "No package found for action '{0}'", TraceLevel.Warning);
        public static CodedMessage unknownActionInPackage 
            = new CodedMessage                                (0706, "Action '{0}' not found in package '{1}'", TraceLevel.Warning);

        // Max Project File                                  0800
        public static CodedMessage multipleInstallers            
            = new CodedMessage                                (0800, "Multiple installers not permitted", TraceLevel.Error);
        public static CodedMessage invalidInstaller            
            = new CodedMessage                                (0801, "The XML of the installer file is invalid.", TraceLevel.Error);
        public static CodedMessage undefinedConfigName            
                  = new CodedMessage                          (0802, "The configuration item at position {0} in the installer file has an undefined name", TraceLevel.Error);
        public static CodedMessage multipleLocaleDefinitions
                  = new CodedMessage                          (0803, "Multiple locale definitions not permitted", TraceLevel.Error);
        public static CodedMessage undefinedLocalesName
                  = new CodedMessage                          (0804, "The locale definition at position {0} in the locales file has an undefined name", TraceLevel.Error);
        public static CodedMessage invalidLocalesName
                  = new CodedMessage                          (0805, "The locale definition at position {0} in the locales file has an unsupported name", TraceLevel.Error);
        public static CodedMessage undefinedPromptName
                  = new CodedMessage                          (0806, "The prompt definition at position {0} in the locales file has an unsupported name", TraceLevel.Error);
        public static CodedMessage mediaFilesLocalesMismatch
                  = new CodedMessage                          (0807, "The number of media files specified do not match the number of accompanying locales", TraceLevel.Error);
      

        // Deployment                                        0900
        public static CodedMessage inaccessible                  
            = new CodedMessage                                (0900, "Application Server inaccessible", TraceLevel.Error);
        public static CodedMessage loginFailure                  
            = new CodedMessage                                (0901, "Could not log into Application Server", TraceLevel.Error);
        public static CodedMessage appWillNotInstall 
            = new CodedMessage                                (0902, "Package could not be installed. {0}", TraceLevel.Error);
        public static CodedMessage appWillNotUninstall
            = new CodedMessage                                (0903, "Package could not be uninstalled. {0}", TraceLevel.Error);
        public static CodedMessage getApps                       
            = new CodedMessage                                (0904, "Could not enumerate running applications", TraceLevel.Error);
        public static CodedMessage disableApp                    
            = new CodedMessage                                (0905, "Could not disable existing application", TraceLevel.Error);
        public static CodedMessage uninstallApp                  
            = new CodedMessage                                (0906, "Could not uninstall existing application", TraceLevel.Error);
        public static CodedMessage refreshFailure
            = new CodedMessage                                (0907, "Could not send RefreshConfiguration application", TraceLevel.Error);
        public static CodedMessage sftpConnectionFailure      
            = new CodedMessage                                (0908, "SFTP server inaccessible. '{0}'", TraceLevel.Error);
        public static CodedMessage sftpUploadFailure
            = new CodedMessage                                (0909, "Unable to upload package to the SFTP server. '{0}'", TraceLevel.Error);
        public static CodedMessage provisioningFailure
            = new CodedMessage                                (9010, "Could not get provisioning status", TraceLevel.Error);

        #endregion

        #region Application Packager Errors

        public static CodedMessage genericFailureToPackage
            = new CodedMessage                              (3000, "Could not package the application", TraceLevel.Error);
        public static CodedMessage extractFailure 
            = new CodedMessage                              (3001, "Could not extract application package '{0}'", TraceLevel.Error);
        public static CodedMessage noScripts
            = new CodedMessage                              (3002, "No application scripts specified", TraceLevel.Error);
        public static CodedMessage noInstaller
            = new CodedMessage                              (3003, "No installer specified", TraceLevel.Warning);
        public static CodedMessage noFrameworkDirSpecified
            = new CodedMessage                              (3004, "No Metreos Framework directory specified.", TraceLevel.Error);
        public static CodedMessage noFrameworkVersionSpecified
            = new CodedMessage                              (3005, "No application version specified, assuming '1.0'.", TraceLevel.Warning);
        public static CodedMessage unableCreateOutputDir
            = new CodedMessage                              (3006, "Could not create output directory '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage noAppPackageSpecified   
            = new CodedMessage                              (3007, "No application package specified for extraction.", TraceLevel.Error);
        public static CodedMessage unableLocateScript       
            = new CodedMessage                              (3008, "Could not locate application script '{0}'", TraceLevel.Error);
        public static CodedMessage unableLocateInstaller      
            = new CodedMessage                              (3009, "Could not locate installer '{0}'", TraceLevel.Error);
        public static CodedMessage unableLocateDbScripts
            = new CodedMessage                              (3010, "Could not locate database creation script '{0}'", TraceLevel.Error);
        public static CodedMessage unableLocateMediaFile
            = new CodedMessage                              (3011, "Could not locate media file '{0}'", TraceLevel.Error);
        public static CodedMessage unableLocateOtherDll   
            = new CodedMessage                              (3012, "Could not locate additional library '{0}'", TraceLevel.Error);
        public static CodedMessage unableLocateFrameworkDir
            = new CodedMessage                              (3012, "Metreos framework directory does not exist.", TraceLevel.Error);
        public static CodedMessage unableOpenFrameworkDir
            = new CodedMessage                              (3013, "Could not open Metreos framework directory. {0}", TraceLevel.Error);
        public static CodedMessage unableLoadInstaller
            = new CodedMessage                              (3014, "Could not load installer '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableLoadScript
            = new CodedMessage                              (3015, "Could not load application script '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableLoadScriptNoExc
            = new CodedMessage                              (3016, "Could not load application script '{0}'.", TraceLevel.Error);
        public static CodedMessage noManifest           
            = new CodedMessage                              (3017, "No manifest file found", TraceLevel.Error);
        public static CodedMessage invalidExtractDirStruct
            = new CodedMessage                              (3018, "Extracted archive directory structure appears invalid.", TraceLevel.Error);
        public static CodedMessage invalidChecksum
            = new CodedMessage                              (3019, "Checksum for '{0}' does not match computed checksum {1}", TraceLevel.Error);
        public static CodedMessage unableOpenChecksum   
            = new CodedMessage                              (3020, "Could not open '{0}' for checksum processing. {1}", TraceLevel.Error);
        public static CodedMessage unableCalculateChecksum
            = new CodedMessage                              (3021, "Could not compute MD5 checksum for '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unreachableManifest      
            = new CodedMessage                              (3022, "Could not get directories and/or files while searching for manifest. {0}", TraceLevel.Error);
        public static CodedMessage unableLoadManifest
            = new CodedMessage                              (3023, "Could not load application manifest file from disk.", TraceLevel.Error);
        public static CodedMessage unableCopyScript
            = new CodedMessage                              (3024, "Could not copy application script '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyNativeAction
            = new CodedMessage                              (3025, "Could not copy native action '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyNativeType
            = new CodedMessage                              (3026, "Could not copy native type '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyOtherDlls
            = new CodedMessage                              (3027, "Could not copy an additional library '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyDbScripts
            = new CodedMessage                              (3028, "Could not copy database creation script '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyMediaFile      
            = new CodedMessage                              (3029, "Could not copy media file '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyRef            
            = new CodedMessage                              (3030, "Could not copy referenced assembly '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyInstaller      
            = new CodedMessage                              (3031, "Could not copy installer XML file '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableWriteManifest      
            = new CodedMessage                              (3032, "Could not write application manifest file to disk. {0}", TraceLevel.Error);
        public static CodedMessage unableResolveNativeDep
            = new CodedMessage                              (3033, "Could not resolve native dependency '{0}'", TraceLevel.Error);
        public static CodedMessage invalidDependencyDir
            = new CodedMessage                              (3034, "Invalid directory for dependency. Try removing trailing '/' from directory name.", TraceLevel.Error);
        public static CodedMessage unableLocateVoiceRec 
            = new CodedMessage                              (3035, "Could not locate voice recognition resource file '{0}'", TraceLevel.Error);
        public static CodedMessage unableCopyVoicerec 
            = new CodedMessage                              (3036, "Could not copy voice recognition resource file '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCopyLocales
            = new CodedMessage                              (3037, "Could not copy locales XML file '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableLoadLocales
            = new CodedMessage                              (3038, "Could not load locales '{0}'. {1}", TraceLevel.Error);
        public static CodedMessage unableCreateLocaleFolder
            = new CodedMessage                              (3039, "Could not create locale folder '{0}'. {1}", TraceLevel.Error);
 

        
        #endregion
    }

    /// <summary>
    /// Just output messages with no corresponding
    /// </summary>
    public abstract class IOutput
    {
        #region Deployment Constants

        public static CodedMessage installApp
            = new CodedMessage                              (90000, "Installing application", TraceLevel.Off);
        public static CodedMessage locatingApp         
            = new CodedMessage                              (90001, "Locating application", TraceLevel.Off);     
        public static CodedMessage connectingToAppServer         
            = new CodedMessage                              (90002, "Connecting to {0}", TraceLevel.Off);
        public static CodedMessage loggingIn         
            = new CodedMessage                              (90003, "Logging in as {0}", TraceLevel.Off);
        public static CodedMessage alreadyInstalledCheck         
            = new CodedMessage                              (90004, "Checking for old application", TraceLevel.Off);
        public static CodedMessage transferringApp         
            = new CodedMessage                              (90005, "Transferring the application", TraceLevel.Off);
        public static CodedMessage validatingInstallation   
            = new CodedMessage                              (90006, "Validating installation", TraceLevel.Off);
        public static CodedMessage deploySucceeded 
            = new CodedMessage                              (90007, "Package deployed", TraceLevel.Off);
        public static CodedMessage tempDirCreateFailed
            = new CodedMessage                              (90008, "Unable to create a temporary directory for extraction", TraceLevel.Off);
        public static CodedMessage extractFailed
            = new CodedMessage                              (90009, "Can not extract application package {0}. Error: {1}", TraceLevel.Off);
        public static CodedMessage noAppNameOnUpdate
            = new CodedMessage                              (90010, "An application name was not provided--can not update", TraceLevel.Off);
        public static CodedMessage provisioningMedia    
            = new CodedMessage                              (90011, "Provisioning media", TraceLevel.Off);
        public static CodedMessage badMessagePM
            = new CodedMessage                              (90012, "Malformed message during media check", TraceLevel.Off);
        public static CodedMessage mmsCountChanged
            = new CodedMessage                              (90013, "The number of media servers changed during deploy", TraceLevel.Off);
        public static CodedMessage failedMessage
            = new CodedMessage                              (90014, "Remote system error during media check", TraceLevel.Off);
        public static CodedMessage commError            
            = new CodedMessage                              (90015, "No response during media check", TraceLevel.Off);
        public static CodedMessage individualMmsFail
            = new CodedMessage                              (90016, "Individual Media Server failed provision: {0}", TraceLevel.Off);
        public static CodedMessage provisioningError  
            = new CodedMessage                              (90017, "Provisioning media error", TraceLevel.Off);
        #endregion
    }
    
    #region Coded Message Helper 

    /// <summary> An error class for errors potentially requiring arguments, along with error codes </summary>
    public class CodedMessage
    {
        protected const string undefinedArguments = " (some arguments not specified)";
        public    int Id              { get { return id; } }
        public	  TraceLevel ErrorLevel { set { errorLevel = value; } get { return errorLevel; } }

        protected int id;
        protected int argCount;
        protected string description;
        protected TraceLevel errorLevel;
    
        /// <summary> Using the String.Format style of specifying arguments ('{0}', etc...), ErrorCode
        ///           will allow you to only worry about a method call and any arguments you may need, 
        ///           in order to format an error string </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public CodedMessage(int id, string description, TraceLevel errorLevel)
        {    
            this.id             = id;
            this.errorLevel     = errorLevel;
            this.description    = description;   
            this.argCount       = ParseFormatString(description);
        }

        /// <summary> Use when you know that the error requires no parameters </summary>
        public static implicit operator string(CodedMessage message)
        {
            return message.description;
        }

        /// <summary> Use when you know that the error requires no parameters </summary>
        public string Message { get { return description; } }

        /// <summary> This description could possibly require formatting </summary>
        /// <param name="args"> The required arguments </param>
        /// <returns> A formatted description </returns>
        public string this[params object[] args]
        {
            get { return FormatDescriptionString(args); }
        }

        /// <summary> This description could possibly require formatting </summary>
        /// <param name="args"> The required arguments </param>
        /// <returns> A formatted description </returns>
        public string FormatDescription(params object[] args)
        {
            return FormatDescriptionString(args);
        }

        protected string FormatDescriptionString(params object[] args)
        {

            string formattedDescription;
            try
            {
                formattedDescription = String.Format(description, args);
            }
            catch
            {
                formattedDescription = description + undefinedArguments;
            }
            
            string message;
            if(errorLevel != TraceLevel.Off)
            {
                message = String.Format("{0}{1}: {2}", GetAbbrErrorType(), id.ToString("g4"), formattedDescription);
            }
            else
            {
                // Don't print code or tracelevel for non-errors
                message = formattedDescription;
            }

            return message;
        }

        /// <summary> Determines the number of arguments of the form '{[num]}'</summary>
        /// <param name="descry iption"> The description </param>
        /// <returns> Number of arguments found </returns>
        protected int ParseFormatString(string description)
        {
            int numArgs = 0;
            int index = 0;
            foreach(char character in description)
            {
                if(character == '{')
                {
                    if(IsBeginningOfArg(description.Substring(index)))
                    {
                        numArgs++;
                    }
                }

                index++;
            }

            return numArgs;
        }

        protected string GetAbbrErrorType()
        {
            switch(errorLevel)
            {
                case TraceLevel.Verbose:
                    return "V";

                case TraceLevel.Info:
                    return "I";

                case TraceLevel.Warning:
                    return "W";

                case TraceLevel.Error:
                    return "E";

                case TraceLevel.Off:
                    return String.Empty;

                default:
                    return "E";
            }
        }

        /// <summary> Hunts for the '}', and takes the middle of '{' - '}' and tries to parse it as int </summary>
        /// <param name="restOfDescription"> The description string including and after '{' </param>
        /// <returns> If the middle of '{' - '}' int parseable (an arg of form {[num]}), return <c>true</c>,
        ///           otherwise <c>false</c> </returns>
        protected bool IsBeginningOfArg(string restOfDescription)
        {
            if(restOfDescription.Length == 1) return false;

            int indexOfEnd = 0;
            int index = 0;
            string afterOpen = restOfDescription.Substring(1);
            foreach(char character in afterOpen)
            {
                if(character == '}')
                {
                    indexOfEnd = index;
                    break;
                }

                index ++;
            }

            string middle = afterOpen.Substring(0, index);

            if(middle != null && middle != String.Empty)
            {
                bool isNumericArg;
                try
                {
                    int.Parse(middle);
                    isNumericArg = true;
                }
                catch
                {
                    isNumericArg = false;
                }
          
                return isNumericArg;
            }

            return false;
        }
    }


    #endregion
}
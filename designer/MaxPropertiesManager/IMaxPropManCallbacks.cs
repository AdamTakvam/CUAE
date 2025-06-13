using System;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    ///           As the number of delegates have grown that the MPM  as need of,
    ///           this interface became necessary mearly as a means of straightening 
    ///           up the constructor of MPM
    /// </summary>
    public interface IMaxPropManCallbacks
    {
        event MaxPropertiesManager.VoidDelegate                 ForcePropertyUpdate;
        MaxPropertiesManager.GetFunctionVars                    GetFunctionVars { get; }
        MaxPropertiesManager.GetGlobalVars                      GetGlobalVars { get; }
        MaxPropertiesManager.GetChangedProperties               GetChangedProperties { get; }
        MaxPropertiesManager.GetAllNativeTypes                  GetAllNativeTypes { get; }
        MaxPropertiesManager.GetNativeTypesInfo                 GetNativeTypesInfo { get; }
        MaxPropertiesManager.GetValidInitWithValues             GetValidInitWithValues { get; }
        MaxPropertiesManager.GetInstallerConfigParameters       GetConfigParameters { get; }
        MaxPropertiesManager.GetLocalizableStrings              GetLocaleStrings { get; } 
        MaxPropertiesManager.GetUsingStatements                 GetUsings { get; } 
        MaxPropertiesManager.GetPackagedAction                  GetAction { get; }
        MaxPropertiesManager.GetMediaFilesDelegate              GetMediaFiles { get; }
        MaxPropertiesManager.GetGrammarFilesDelegate            GetGrammarFiles { get; }
        MaxPropertiesManager.GetVoiceRecFilesDelegate           GetVoiceRecFiles { get; }
        MaxPropertiesManager.UpdateUsingStatements              UpdateUsings { get; }
        MaxPropertiesManager.GetDefaultUserType                 GetDefaultUserType {get; } 
        MaxPropertiesManager.GetEventParameter                  GetEventParamByDisplay { get; }
        MaxPropertiesManager.GetEventParameter                  GetEventParamByName { get; }
    }
}

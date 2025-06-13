using System;
using Metreos.Max.Framework.Satellite.Property;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>An interface used to abstract the MaxPropertyManager down to something a little more elegant.</summary>
    public interface IMpmDelegates
    {
        MaxPropertyGrid                                     PropertyGrid  { get; }
        MaxPropertiesManager.GetFunctionVars                GetFunctionVarsDelegate { get; set; }
        MaxPropertiesManager.GetGlobalVars                  GetGlobalVarsDelegate { get; set; }
        MaxPropertiesManager.IndividualValueChangedDelegate ValueChanged { get; set; }
        MaxPropertiesManager.GetAllNativeTypes              GetAllNativeTypesDelegate { get; set; }
        MaxPropertiesManager.GetNativeTypesInfo             GetNativeTypesInfoDelegate { get; set; }
        MaxPropertiesManager.GetValidInitWithValues         GetInitWithValues { get; set; }
        MaxPropertiesManager.GetInstallerConfigParameters   GetConfigParameters { get; set; }
        MaxPropertiesManager.GetLocalizableStrings          GetLocaleStrings { get; set; }
        MaxPropertiesManager.GetUsingStatements             GetUsings { get; set; }
        MaxPropertiesManager.UpdateUsingStatements          UpdateUsings { get; set; }
        MaxPropertiesManager.GetMediaFilesDelegate          GetMediaFiles { get; set; }
        MaxPropertiesManager.GetGrammarFilesDelegate        GetGrammarFiles { get; set; }
        MaxPropertiesManager.GetVoiceRecFilesDelegate       GetVoiceRecFiles { get; set; }
        MaxPropertiesManager.RemovePropertyFromGrid         RemovePropertyDelegate { get; set; }
        MaxPropertiesManager.GetDefaultUserType             GetDefaultUserTypeDelegate { get; set; }
        MaxPropertiesManager.VoidDelegate                   FocusPropertyGrid { get; }
        MaxPropertiesManager.GetEventParameter              GetEventParameterByDisplayName { get; set; }
        MaxPropertiesManager.GetEventParameter              GetEventParameterByName { get; set; }
        }
}

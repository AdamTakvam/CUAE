using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using Metreos.Interfaces;
using Metreos.ApplicationFramework.ScriptXml;
using PropertyGrid.Core;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    ///   Common utility functions 
    ///   In general, the static methods stored within this utility fall under two categories:
    ///   1.)  Translation methods that convert Designer enumerations to and from Metreos Framework enumerations       
    ///   2.)  Methods used by the Designer Property Manager to determine the occurrence of special case action node types
    /// </summary>
    public class Util
    {
        #region Designer enumerations <> Metreos Framework enumerations

        public static paramType FromMaxToMetreosActPar(DataTypes.UserVariableType type)
        {
            if(type == DataTypes.UserVariableType.literal)
                return paramType.literal;

            else if(type == DataTypes.UserVariableType.csharp)
                return paramType.csharp;

            else if(type == DataTypes.UserVariableType.variable)
                return paramType.variable;

            return paramType.literal;
        }

        public static DataTypes.UserVariableType FromMetreosToMaxActPar(paramType type)
        {
            if(type == paramType.literal)
                return DataTypes.UserVariableType.literal;

            else if(type == paramType.variable)
                return DataTypes.UserVariableType.variable;

            else if(type == paramType.csharp)
                return DataTypes.UserVariableType.csharp;

            return DataTypes.UserVariableType.literal;
        }

        public static DataTypes.AllowableLanguages FromMetreosToMaxLanguageType(languageType language)
        {
            if(language == languageType.csharp)
                return DataTypes.AllowableLanguages.csharp;

            return DataTypes.AllowableLanguages.csharp;
        }

        public static paramType FromMaxToMetreosLoopType(DataTypes.LoopType type)
        {
            if(type == DataTypes.LoopType.literal)
                return paramType.literal;
      
            else if(type == DataTypes.LoopType.variable)
                return paramType.variable;

            else if(type == DataTypes.LoopType.csharp)
                return paramType.csharp;

            return paramType.literal;
        } 

        public static loopCountEnumType FromMaxToMetreosLoopCountType(DataTypes.LoopIterateType type)
        {
            if(type == DataTypes.LoopIterateType.@int)
                return loopCountEnumType.@int;

            else if(type == DataTypes.LoopIterateType.@enum)
                return loopCountEnumType.@enum;

            else if(type == DataTypes.LoopIterateType.dictEnum)
                return loopCountEnumType.dictEnum;

            return loopCountEnumType.@int;
        }

        public static DataTypes.LoopIterateType FromMetreosToMaxLoopCountType(loopCountEnumType type)
        {
            if(type == loopCountEnumType.@int)
                return DataTypes.LoopIterateType.@int;

            else if(type == loopCountEnumType.@enum)
                return DataTypes.LoopIterateType.@enum;

            else if(type == loopCountEnumType.dictEnum)
                return DataTypes.LoopIterateType.dictEnum;

            return DataTypes.LoopIterateType.@int;
        }

        public static DataTypes.LoopType FromMetreosToMaxLoopType(paramType type)
        {
            if(type == paramType.literal)
                return DataTypes.LoopType.literal;

            else if(type == paramType.variable)
                return DataTypes.LoopType.variable;

            else if(type == paramType.csharp)
                return DataTypes.LoopType.csharp;

            return DataTypes.LoopType.literal;
        }

        public static eventParamTypeType FromMaxToMetreosEventParamType(DataTypes.EventParamType type)
        {
            if(type == DataTypes.EventParamType.literal)
                return eventParamTypeType.literal;

            else if(type == DataTypes.EventParamType.variable)
                return eventParamTypeType.variable;

            return eventParamTypeType.literal;
        }

        public static DataTypes.EventParamType FromMetreosToMaxEventParamType(eventParamTypeType type)
        {
            if(type == eventParamTypeType.literal) 
                return DataTypes.EventParamType.literal;

            else if(type == eventParamTypeType.variable)
                return DataTypes.EventParamType.variable;

            return DataTypes.EventParamType.literal;
        }

        public static languageType FromMaxToMetreosLanguageType(DataTypes.AllowableLanguages type)
        {
            if(type == DataTypes.AllowableLanguages.csharp)
                return languageType.csharp;

            return languageType.csharp;
        }

        public static Metreos.PackageGeneratorCore.PackageXml.actionTypeType ExtractActionTypeType( string _value )
        {
            try
            {
                return (Metreos.PackageGeneratorCore.PackageXml.actionTypeType) 
                    Enum.Parse( typeof(Metreos.PackageGeneratorCore.PackageXml.actionTypeType), _value, true);
            }
            catch
            {
                return Metreos.PackageGeneratorCore.PackageXml.actionTypeType.native;
            }
        }

        public static paramType ExtractActionParamTypeType( string _value )
        {
            try
            {
                return (paramType) Enum.Parse(typeof(paramType), _value, true);
            }
            catch
            {
                return paramType.literal;
            }
        }

        public static DataTypes.EventParamType ExtractEventParamType( string _value )
        {
            try   { return (DataTypes.EventParamType) Enum.Parse(typeof(DataTypes.EventParamType), _value, true); }
            catch { return DataTypes.EventParamType.literal; }
        }

        public static eventParamTypeType ExtractEventParamTypeMetreos (string _value )
        {
            try   { return (eventParamTypeType) Enum.Parse(typeof(eventParamTypeType), _value, true); }
            catch { return eventParamTypeType.literal; }
        }

        public static paramType ExtractLoopType( string _value)
        {
            try
            {
                return (paramType) Enum.Parse(typeof(paramType), _value, true);
            }
            catch
            {
                return paramType.literal;
            }
        }

        public static loopCountEnumType ExtractLoopCountType( string _value )
        {
            try
            {
                return (loopCountEnumType) Enum.Parse( typeof(loopCountEnumType), _value, true);
            }
            catch
            {
                return loopCountEnumType.@int;
            }
        }

        public static TraceLevel ExtractLogLevel( string _value )
        {
            try
            {
                return (TraceLevel) Enum.Parse(typeof(TraceLevel), _value, true);
            }
            catch
            {
                return TraceLevel.Info;
            }
        }

        public static DataTypes.OnOff ExtractOnOff( string _value )
        {
            try
            {
                return (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), _value, true);
            }
            catch
            {
                return DataTypes.OnOff.Off;
            }
        }

        public static DataTypes.ReferenceType ExtractReferenceType( string _value )
        {
            try
            {
                return (DataTypes.ReferenceType) Enum.Parse(typeof(DataTypes.ReferenceType), _value, true);
            }
            catch
            {
                return Defaults.REFERENCE_TYPE;
            }
        }

        public static parameterTypeType ExtractMetreosReferenceType( string _value )
        {
            try
            {
                return (parameterTypeType) Enum.Parse(typeof(parameterTypeType), _value, true);
            }
            catch
            {
                return Defaults.REFERENCE_TYPE_METREOS;
            }
        }
    
        public static DataTypes.AllowableLanguages ExtractLanguageType( string _value )
        {
            try   { return (DataTypes.AllowableLanguages) Enum.Parse(typeof(DataTypes.AllowableLanguages), _value, true); }
            catch { return Defaults.USER_CODE_LANGUAGE; }
        }

        public static languageType ExtractLanguageTypeMetreos( string _value )
        {
            try   { return (languageType) Enum.Parse(typeof(languageType), _value, true); }
            catch { return Defaults.USER_CODE_LANGUAGE_METREOS; }
        }

        #endregion

        #region Property Access

        /// <param name="nillable">set nillable to true to return a <c>null</c> || <c>""</c> as <c>null</c>
        /// Otherwise, a return <c>null</c> || <c>""</c> as <c>""</c></param>
        public static string GetPropertyValue(MaxProperty property, bool nillable)
        {
            if (property == null) return null;  
            string propval = property.Value == null? String.Empty: property.Value.ToString();

            if  (propval.Length == 0)
                return nillable? null: String.Empty;
            else return propval;
        }

        #endregion

        #region Special-Case Determination Logic

        public static bool IsMediaFileName(string propertyName)
        {
            bool firstHalfCorrect = false, secondHalfCorrect = false;

            if  (propertyName != null)  
                firstHalfCorrect = propertyName.StartsWith(Defaults.PLAY_ANN_FILE_CHOOSE);
            if (!firstHalfCorrect) return false;
      
            string lastHalf = propertyName.Substring(Defaults.PLAY_ANN_FILE_CHOOSE.Length);

            if(lastHalf == String.Empty)
            {
                secondHalfCorrect = true;
            }
            else
            {
                try
                {
                    int fileNum = int.Parse(lastHalf);
                    secondHalfCorrect = true;
                }
                catch { }     
            }
      
            return secondHalfCorrect;
        }


        public static string MakeFullyQualified(string @namespace, string className)
        {
            return @namespace + '.' + className;
        }

        public static bool IsPlayAnnAction(MaxPmAction action)
        {
            return 0 == String.Compare(MakeFullyQualified(action.PackageName, action.Name),
                MakeFullyQualified(Defaults.MediaServerProviderNs, Defaults.PlayAnnouncement));
        }

        /// <summary> Determines if the parameter is 'FunctionName' and the action is
        ///           'Metreos.ApplicationControl.CallFunction'</summary>
        public static bool IsCallFunctionFuncName(MaxPmAction action, string paramName)
        {
            bool isCallFunction = 0 == String.Compare(IActions.CallFunction,
                MakeFullyQualified(action.PackageName, action.Name));

            bool foundFunctionName = 0 == String.Compare(paramName, IActions.Fields.FunctionName);

            return isCallFunction && foundFunctionName;
        }

        #endregion

        #region XML-Parsing

        public static string ResolveNullToEmptyString(string strang)
        {
            if(strang == null)  return String.Empty;

            else                return strang;
        }

        /// <summary>Returns the value of an attribute, allowing <c>null</c> to be passed in for the attribute</summary>
        /// <returns>The value of the attribute, or null if the attribute is null</returns>
        public static string ResolveAttributeValue(XmlAttribute attribute)
        {
            if(attribute == null)   return null;
            else return attribute.Value;
        }

        #endregion
    }
}

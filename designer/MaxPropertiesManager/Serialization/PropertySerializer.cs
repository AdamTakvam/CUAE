using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Interfaces;
using Metreos.WebServicesConsumerCore;
using PropertyGrid.Core;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for PropertySerializer.
    /// </summary>
    public class PropertySerializer
    {
        MaxPropertiesManager mpm;
        XmlDocument doc = new XmlDocument();
        private static XmlSerializer actionDeserializer = new XmlSerializer(typeof(MaxActionType));
        private static XmlSerializer eventDeserializer = new XmlSerializer(typeof(MaxEventType));
        private static XmlSerializer functionDeserializer = new XmlSerializer(typeof(MaxFunctionType));
        private static XmlSerializer loopDeserializer = new XmlSerializer(typeof(MaxLoopType));
        private static XmlSerializer localVariableDeserializer = new XmlSerializer(typeof(MaxLocalVariableType));
        private static XmlSerializer globalVariableDeserializer = new XmlSerializer(typeof(MaxGlobalVariableType));
    
        private static ArrayList actions = new ArrayList();
        private static ArrayList results = new ArrayList();
        private static ArrayList logs = new ArrayList();
        private static ArrayList eventParams = new ArrayList();

        public PropertySerializer(MaxPropertiesManager maxPropMan)
        {
            this.mpm = maxPropMan;	
        }

        public void SerializeSave(DataTypes.Type type,
            PropertyDescriptorCollection maxProperties, XmlTextWriter writer)
        {
            switch(type)
            {
                case DataTypes.Type.ActionInstance:
                    SerializeActionInstance(maxProperties, writer);
                    break;

                case DataTypes.Type.EventInstance:
                    SerializeEventInstance(maxProperties, writer);
                    break;

                case DataTypes.Type.Function:
                    SerializeFunction(maxProperties, writer);
                    break;

                case DataTypes.Type.Link:
                    SerializeLink(maxProperties, writer);
                    break;

                case DataTypes.Type.Loop:
                    SerializeLoop(maxProperties, writer);
                    break;

                case DataTypes.Type.LocalVariable:
                    SerializeLocalVariable(maxProperties, writer);
                    break;

                case DataTypes.Type.GlobalVariable:
                    SerializeGlobalVariable(maxProperties, writer);
                    break;

                case DataTypes.Type.StartNode:
                    break;

                case DataTypes.Type.Script:
                    SerializeScript(maxProperties, writer);
                    break;

                case DataTypes.Type.Project:
                    SerializeProject(maxProperties, writer);
                    break;

                case DataTypes.Type.Code:
                    SerializeCode(maxProperties, writer);
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "Missed a type to serialize. " +
                        "Type is: " + type.ToString());
                    break;
            }
        }

        public void Deserialize(object selectableObject, DataTypes.Type type,
            PropertyDescriptorCollection properties, XmlNode node, bool packageExists)
        {
            switch(type)
            {
                case DataTypes.Type.ActionInstance:
                    if(packageExists)
                        DeserializeActionInstance(selectableObject, properties, node);
                    else
                        DeserializePackagelessActionInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.EventInstance:
                    if(packageExists)
                        DeserializeEventInstance(selectableObject, properties, node);
                    else
                        DeserializePackagelessEventInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Function:
                    DeserializeFunctionInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Link:
                    DeserializeLinkInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Loop:
                    DeserializeLoopInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.LocalVariable:
                    DeserializeLocalVariableInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.GlobalVariable:
                    DeserializeGlobalVariableInstance(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Script:
                    DeserializeScript(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Project:
                    DeserializeProject(selectableObject, properties, node);
                    break;

                case DataTypes.Type.Code:
                    DeserializeCode(selectableObject, properties, node);
                    break;
            }
        }

        #region Custom Serializations
    
        private void SerializeActionInstance(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        { 
            writer.WriteStartElement(Defaults.xmlEltProperties);

            // Attributes
            WriteFinal(maxProperties, writer);
            WriteActionType(maxProperties, writer);
            WriteMasterLog(maxProperties, writer);

            // Elements
            WriteActionParameters(maxProperties, writer);
            WriteResultData(maxProperties, writer);
            WriteLogging(maxProperties, writer);

            writer.WriteFullEndElement();

            #region XmlSerialization Impl
            //      MaxActionType actionXml = new MaxActionType();
            //      ArrayList actionParams = new ArrayList();
            //      ArrayList resultVars = new ArrayList();
            //      ArrayList logging = new ArrayList();
            //
            //      foreach(MaxProperty property in maxProperties)
            //      {
            //        if(property is ActionParameterProperty)
            //          
            //          ConstructActionParameter(property as ActionParameterProperty, ref actionParams); 
            //
            //        else if(property is ActionParameterPropertyGrowable)
            //          
            //          foreach(ActionParameterProperty customAction in property.ChildrenProperties)
            //            ConstructActionParameter(customAction, ref actionParams);
            //
            //        else if(property is ResultDataProperty)
            //          
            //          ConstructResultVar(property as ResultDataProperty, ref resultVars);
            //
            //        else if(property is ResultDataPropertyGrowable)
            //          
            //          foreach(ResultDataProperty customVar in property.ChildrenProperties)
            //            ConstructResultVar(customVar, ref resultVars);
            //
            //        else if(property is LoggingProperty)
            //        {
            //          foreach(MaxProperty innerProperty in property.ChildrenProperties)
            //          {
            //            if(innerProperty is OnLogEventProperty)
            //              ConstructLogging(innerProperty as OnLogEventProperty, ref logging);
            //            else if(innerProperty is OnLogEventPropertyGrowable)
            //              foreach(OnLogEventProperty log in innerProperty.ChildrenProperties)
            //                ConstructLogging(log, ref logging);
            //          }
            //        }
            //        else if(property is GenericProperty || property is HiddenGenericProperty)
            //        {
            //          string propertyValue = Util.GetPropertyValue(property, false);
            //
            //          if(property.Name == DataTypes.ACTION_TYPE)
            //            
            //            actionXml.type = propertyValue != "" ? (Metreos.PackageGeneratorCore.actionTypeType)
            //              Enum.Parse( typeof(Metreos.PackageGeneratorCore.actionTypeType), 
            //              propertyValue , true ) : Metreos.PackageGeneratorCore.actionTypeType.native;
            //          
            //          else if(property.Name == DataTypes.FINAL)
            //            actionXml.final = bool.Parse(propertyValue != "" ? propertyValue : false.ToString());
            //        }  
            //      }
            //
            //      if(actionParams.Count != 0)
            //      {
            //        actionXml.@params = new actionParamType[actionParams.Count];
            //        actionParams.CopyTo(actionXml.@params);
            //      }
            //
            //      if(resultVars.Count != 0)
            //      {
            //        actionXml.results = new resultDataVariableType[resultVars.Count];
            //        resultVars.CopyTo(actionXml.results);
            //      }
            //
            //      if(logging.Count != 0)
            //      {
            //        actionXml.logging = new LogType[logging.Count];
            //        logging.CopyTo(actionXml.logging);
            //      }
            //
            //      return actionXml;
            //
            #endregion
        }

        private void WriteFinal(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            GenericProperty property = maxProperties[DataTypes.FINAL] as GenericProperty; 
            string propertyValue = Util.GetPropertyValue(property, true);

            if(propertyValue == null) return;

            writer.WriteAttributeString(Defaults.xmlAttrFinal, null, propertyValue.ToLower());
        }

        private void WriteActionType(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            foreach(MaxProperty property in maxProperties)
            {
                if(!(property is GenericProperty))  continue;
        
                if(!(property.Name == DataTypes.ACTION_TYPE)) continue;

                string propertyValue = Util.GetPropertyValue(property, true);
      
                if(propertyValue == null) return;

                writer.WriteAttributeString(Defaults.xmlAttrType, null, propertyValue);

                break;
            }
        }

      private void WriteMasterLog(PropertyDescriptorCollection maxProperties,
        XmlTextWriter writer)
      {
        foreach(MaxProperty property in maxProperties)
        {
            if(property is LoggingProperty)
            {
              string propertyValue = Util.GetPropertyValue(property, true);
              if(propertyValue == null) return;
              writer.WriteAttributeString(Defaults.xmlAttrLog, null, propertyValue);
            }
        }
      }

        private void WriteActionParameters(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            foreach(MaxProperty property in maxProperties)
            {
                if(property is ActionParameterProperty)
                    WriteActionParameter(property as ActionParameterProperty, writer);

                else if(property is ActionParameterPropertyGrowable)
                    WriteActionParameters(property.ChildrenProperties, writer);

                else if(property is ComplexTypeProperty)
                    WriteActionParametersSpecial(property as ComplexTypeProperty, writer);
                // todo add hidden else if(    // 20060907 also deserialize
            }
        }

        private void WriteActionParameter(ActionParameterProperty actionProperty,
            XmlTextWriter writer)
        {
            string actionValue = Util.GetPropertyValue(actionProperty, true);
            if(actionValue == null) return;

            UserTypeProperty userType
                = actionProperty.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;

            writer.WriteStartElement(Defaults.xmlEltActionParameter);

            writer.WriteAttributeString(Defaults.xmlAttrName, null, actionProperty.Name);  
            writer.WriteAttributeString(Defaults.xmlAttrType, null,
                Util.FromMaxToMetreosActPar(userType.Value).ToString());
            writer.WriteString(actionValue);

            writer.WriteFullEndElement();
        }
    
        private void WriteActionParametersSpecial(ComplexTypeProperty parent, XmlTextWriter writer)
        {
            string complexParentName = parent.Name;

            foreach(ActionParameterProperty actionProperty in parent.ChildrenProperties)
            {
                string actionValue = Util.GetPropertyValue(actionProperty, true);
                if(actionValue == null)   continue;

                UserTypeProperty userType
                    = actionProperty.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;

                writer.WriteStartElement(Defaults.xmlEltActionParameter);

                writer.WriteAttributeString(Defaults.xmlAttrName, null, 
                    complexParentName + NativeActionAssembler.heirarchySeperator + actionProperty.Name);  
                writer.WriteAttributeString(Defaults.xmlAttrType, null,
                    Util.FromMaxToMetreosActPar(userType.Value).ToString());
                writer.WriteString(actionValue);

                writer.WriteFullEndElement();
            }
        }

        private void WriteResultData(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            foreach(MaxProperty property in maxProperties)
            {
                if(property is ResultDataProperty)
                    WriteResultDatum(property as ResultDataProperty, writer);

                else if(property is ResultDataPropertyGrowable)
                    WriteResultData(property.ChildrenProperties, writer);
            }
        }

        private void WriteResultDatum(ResultDataProperty resultProperty,
            XmlTextWriter writer)
        {
            string resultPropertyValue = Util.GetPropertyValue(resultProperty, true);     
            if(resultPropertyValue == null) return;

            writer.WriteStartElement(Defaults.xmlEltResultData);

            writer.WriteAttributeString(Defaults.xmlAttrField, null, resultProperty.Name);
            writer.WriteString(resultPropertyValue);

            writer.WriteFullEndElement();
        }

        private void WriteLogging(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            foreach(MaxProperty property in maxProperties)
              if(property is LoggingProperty)
                foreach(MaxProperty innerProperty in property.ChildrenProperties)
                {
                  if(innerProperty is OnLogEventProperty)
                    WriteLog(innerProperty as OnLogEventProperty, writer);

                  else if(innerProperty is OnLogEventPropertyGrowable)
                    foreach(OnLogEventProperty logProperty in innerProperty.ChildrenProperties)
                      WriteLog(logProperty, writer);
                }
        }

        private void WriteLog(OnLogEventProperty logProperty, XmlTextWriter writer)
        {
            string message = Util.GetPropertyValue(logProperty, true);
            if(message == null) return;

            writer.WriteStartElement(Defaults.xmlEltLogging, null);

            OnOffProperty onProperty 
                = logProperty.ChildrenProperties[DataTypes.LOGGING] as OnOffProperty;
            TraceLevelProperty levelProperty
                = logProperty.ChildrenProperties[DataTypes.TRACELEVEL] as TraceLevelProperty;
            UserTypeProperty userTypeProperty
                = logProperty.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;

            writer.WriteAttributeString(Defaults.xmlAttrCondition, null, logProperty.Name);
            writer.WriteAttributeString(Defaults.xmlAttrOn, null, 
                onProperty.Value == DataTypes.OnOff.On ? true.ToString().ToLower() : 
                false.ToString().ToLower());

            writer.WriteAttributeString(Defaults.xmlAttrLevel, null,
                levelProperty.Value.ToString());
            writer.WriteAttributeString(Defaults.xmlAttrType, null,
                Util.FromMaxToMetreosActPar(userTypeProperty.Value).ToString());
            writer.WriteString(message);

            writer.WriteFullEndElement();
        }

        private void SerializeEventInstance(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        { 
            writer.WriteStartElement(Defaults.xmlEltProperties);

            foreach(MaxProperty property in maxProperties)
                if(property is GenericProperty)
                    if(property.Name == DataTypes.EVENT_TYPE)
                    {
                        string eventType = Util.GetPropertyValue(property, true);
                        writer.WriteAttributeString(Defaults.xmlAttrType, null, eventType);
                        break;
                    }


            foreach(MaxProperty property in maxProperties)
                if(property is EventParameterProperty)
                {
                    string eventParamValue = Util.GetPropertyValue(property, true);
                    if(eventParamValue == null) continue;

                    EventParamTypeProperty typeProperty
                        = property.ChildrenProperties[DataTypes.EVENT_PARAM_TYPE] as EventParamTypeProperty;
         
                    writer.WriteStartElement(Defaults.xmlEltEventParameter);
         
                    writer.WriteAttributeString(Defaults.xmlAttrName, null, property.Name);

                    writer.WriteAttributeString(Defaults.xmlAttrType, null,
                        Util.FromMaxToMetreosEventParamType(typeProperty.Value).ToString());
                    writer.WriteString(eventParamValue);

                    writer.WriteFullEndElement();
                }

            writer.WriteFullEndElement();

            #region XmlSerialization Impl
            //      MaxEventType eventXml = new MaxEventType();
            //      ArrayList eventParams = new ArrayList();
            //
            //      foreach(MaxProperty property in maxProperties)
            //      {
            //        if(property is EventParameterProperty)
            //          ConstructEventParameter(property as EventParameterProperty, ref eventParams); 
            //      }
            //
            //      if(eventParams.Count != 0)
            //      {
            //        eventXml.@params = new triggerParamType[eventParams.Count];
            //        eventParams.CopyTo(eventXml.@params);
            //      }
            //
            //      return eventXml;
            #endregion
        }

        private void ConstructActionParameter(ActionParameterProperty action,
            ref ArrayList actionParams)
        {
            actionParamType param = new actionParamType();
            param.name = action.Name;
            UserTypeProperty type
                = action.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;
            param.type = Util.FromMaxToMetreosActPar(type.Value);
            param.Value = Util.GetPropertyValue(action, true);

            if(param.Value != null)
                actionParams.Add(param);
        }

        private void ConstructResultVar(ResultDataProperty resultVarProperty,
            ref ArrayList resultVars)
        {
            resultDataType resultVar = new resultDataType();
            resultVar.field = resultVarProperty.Name;
            resultVar.Value = Util.GetPropertyValue(resultVarProperty, true);
            resultVar.type = resultDataTypeType.variable;
            if(resultVar.Value != null)
                resultVars.Add(resultVar);
        }

        private void ConstructLogging(OnLogEventProperty logProperty, ref ArrayList logging)
        {
            LogType log = new LogType();
            log.condition = logProperty.Name;
            TraceLevelProperty levelProperty
                = logProperty.ChildrenProperties[DataTypes.TRACELEVEL] as TraceLevelProperty;
            log.level = levelProperty.Value;
            OnOffProperty onOffProperty
                = logProperty.ChildrenProperties[DataTypes.LOGGING] as OnOffProperty;
            log.on = onOffProperty.Value == DataTypes.OnOff.On ? true : false;
            log.Value = Util.GetPropertyValue(logProperty, true);

            if(log.Value != null)
                logging.Add(log);
        }

        private void ConstructEventParameter(EventParameterProperty eventProperty,
            ref ArrayList eventParams)
        {
            eventParamType eventParam = new eventParamType();
            eventParam.name = eventProperty.Name;
            EventParamTypeProperty typeProperty = eventProperty.ChildrenProperties[DataTypes.EVENT_PARAM_TYPE] as EventParamTypeProperty;
            eventParam.type = Util.FromMaxToMetreosEventParamType(typeProperty.Value);
            eventParam.Value = Util.GetPropertyValue(eventProperty, true);
      
            if(eventParam.Value != null)
                eventParams.Add(eventParam);
        }

        private void SerializeFunction(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties);

            //      GenericProperty functionNameProperty = maxProperties[DataTypes.FUNCTION_NAME] as GenericProperty;      
            //      string functionName = Util.GetPropertyValue(functionNameProperty, true);
            //
            //      if(functionName != null)
            //        writer.WriteAttributeString(Defaults.xmlAttrName, null, functionName);

            writer.WriteFullEndElement();

            #region XmlSerialzation Impl
            //      MaxFunctionType functionXml = new MaxFunctionType();
            //
            //      GenericProperty functionNameProperty = maxProperties[DataTypes.FUNCTION_NAME] as GenericProperty;
            //      functionXml.name = Util.GetPropertyValue(functionNameProperty, true);
            //      
            //      if(functionXml.name == "")
            //        functionXml.name = null;
            //
            //      return functionXml;
            #endregion
        }

        // TODO
        private void SerializeLink(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            writer.WriteFullEndElement();

            #region XmlSerialization Impl
            //      MaxLinkType linkXml = new MaxLinkType();
            //      linkXml.logging = null;
            //      StyleProperty style = maxProperties[DataTypes.LINKSTYLE] as StyleProperty;
            //      linkXml.style = null; // TODO:
            //      linkXml.text = null;
            #endregion
        }

        private void SerializeLoop(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            LoopCountProperty loopCount = maxProperties[DataTypes.COUNT] as LoopCountProperty;
            string loopValue = Util.GetPropertyValue(loopCount, true);
      
            if(loopValue == null)
            { 
                writer.WriteFullEndElement();
                return;
            }

            LoopTypeProperty loopType
                = loopCount.ChildrenProperties[DataTypes.LOOP_TYPE_NAME] as LoopTypeProperty;
     
            LoopControllerTypeProperty loopIterator
                = loopCount.ChildrenProperties[DataTypes.LOOP_ITERATE_TYPE_NAME] as LoopControllerTypeProperty;
         
            writer.WriteAttributeString(Defaults.xmlAttrLoopIteratorType, null, Util.FromMaxToMetreosLoopCountType(loopIterator.Value).ToString());
            writer.WriteAttributeString(Defaults.xmlAttrType, null, Util.FromMaxToMetreosLoopType(loopType.Value).ToString());
            writer.WriteString(loopValue);

            writer.WriteFullEndElement();
            #region XmlSerialiazation Impl
            //      MaxLoopType loopXml = new MaxLoopType();
            //      GenericProperty loopCount = maxProperties[DataTypes.COUNT] as GenericProperty;
            //      loopXml.Value = Util.GetPropertyValue(loopCount, true);
            //      
            //      if(loopXml.Value == null)  
            //        return loopXml;
            //        
            //      LoopTypeProperty loopType = loopCount.ChildrenProperties[DataTypes.LOOP_TYPE_NAME] as LoopTypeProperty;
            //      loopXml.type = Util.FromMaxToMetreosLoopType(loopType.Value);
            //      GenericProperty renameIndex = loopCount.ChildrenProperties[DataTypes.INDEX_NAME]   as GenericProperty;
            //      loopXml.index = Util.GetPropertyValue(renameIndex, true);
            //
            //      return loopXml;
            #endregion
        }

        private void SerializeLocalVariable(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            GenericProperty valueProperty
                = maxProperties[DataTypes.VARIABLE] as GenericProperty;
            string variableValue = Util.GetPropertyValue(valueProperty, true);
      
            if(variableValue == null)
            {
                writer.WriteFullEndElement();
                return;
            }

            VariableProperty varProperty
                = maxProperties[DataTypes.VARIABLE_TYPE] as VariableProperty;
            string variableType = varProperty.VariableName; // Here we serialize out the true name 
                                                            // of the variable type, as opposed to display
            if(variableType == null)  variableType = Defaults.TYPE;

            LocalVarInitWithProperty initWithProperty
                = maxProperties[DataTypes.LOCAL_VAR_INIT_WITH] as LocalVarInitWithProperty;
            string initWithValue = initWithProperty.InnerName;
            string eventName = initWithProperty.InnerEventName;
            

            MaxProperty defaultInitWith 
                = maxProperties[DataTypes.DEFAULT_INIT_WITH] as MaxProperty;
            string defaultInitWithValue = Util.GetPropertyValue(defaultInitWith, true);

            ReferenceProperty referenceProperty
                = maxProperties[DataTypes.REFERENCE_TYPE_NAME] as ReferenceProperty;
            string referenceValue = Util.GetPropertyValue(referenceProperty, true);

            writer.WriteAttributeString(Defaults.xmlAttrType, null, variableType);

            if(initWithValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrInitWith, null, initWithValue);
            if(defaultInitWithValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrDefaultInitWith, null, defaultInitWithValue);
            if(referenceValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrRefType, null, referenceValue);
            if(eventName != null)
                writer.WriteAttributeString(Defaults.xmlAttrName, null, eventName);

            writer.WriteString(variableValue);
            writer.WriteFullEndElement();

            #region XmlSerialization Impl
            //      MaxVariableType variableXml = new MaxVariableType();
            //      
            //      GenericProperty valueProperty = maxProperties[DataTypes.VARIABLE] as GenericProperty;
            //      variableXml.Value = Util.GetPropertyValue(valueProperty, true);
            //      
            //      if(variableXml.Value == null)
            //        return variableXml;
            //
            //      VariableProperty varProperty = maxProperties[DataTypes.VARIABLE_TYPE] as VariableProperty;
            //      variableXml.varType = Util.GetPropertyValue(varProperty, true);
            //
            //      if(variableXml.varType == null)
            //        variableXml.varType = Defaults.TYPE;
            //
            //      GenericProperty initWithProperty = maxProperties[DataTypes.INIT_WITH] as GenericProperty;
            //      variableXml.initWith = Util.GetPropertyValue(initWithProperty, true);
            //
            //      return variableXml;      
            #endregion
        }

        private void SerializeGlobalVariable(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            GenericProperty valueProperty 
                = maxProperties[DataTypes.VARIABLE] as GenericProperty;
            string variableValue = Util.GetPropertyValue(valueProperty, true);
      
            if(variableValue == null)
            {
                writer.WriteFullEndElement();
                return;
            }

            VariableProperty varProperty
                = maxProperties[DataTypes.VARIABLE_TYPE] as VariableProperty;
            string variableType = varProperty.VariableName;    // Here we serialize out the true name 
                                                               // of the variable type, as opposed to display
            if(variableType == null)  variableType = Defaults.TYPE;

            GlobalVarInitWithProperty initWithProperty 
                = maxProperties[DataTypes.CONFIG_INIT_WITH] as GlobalVarInitWithProperty;
            string initWithValue = Util.GetPropertyValue(initWithProperty, true);

            MaxProperty defaultInitWith 
                = maxProperties[DataTypes.DEFAULT_INIT_WITH] as MaxProperty;
            string defaultInitWithValue = Util.GetPropertyValue(defaultInitWith, true);
 
            writer.WriteAttributeString(Defaults.xmlAttrType, null, variableType);

            if(defaultInitWithValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrDefaultInitWith, null, defaultInitWithValue);
            if(initWithValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrInitWith, null, initWithValue);

            writer.WriteString(variableValue);
            writer.WriteFullEndElement();

            #region XmlSerialization Impl
            //      MaxVariableType variableXml = new MaxVariableType();
            //      
            //      GenericProperty valueProperty = maxProperties[DataTypes.VARIABLE] as GenericProperty;
            //      variableXml.Value = Util.GetPropertyValue(valueProperty, true);
            //      
            //      if(variableXml.Value == null)
            //        return variableXml;
            //
            //      VariableProperty varProperty = maxProperties[DataTypes.VARIABLE_TYPE] as VariableProperty;
            //      variableXml.varType = Util.GetPropertyValue(varProperty, true);
            //
            //      if(variableXml.varType == null)
            //        variableXml.varType = Defaults.TYPE;
            //
            //      GenericProperty initWithProperty = maxProperties[DataTypes.INIT_WITH] as GenericProperty;
            //      variableXml.initWith = Util.GetPropertyValue(initWithProperty, true);
            //
            //      return variableXml;      
            #endregion
        }

        /// <summary>Writes out script type, script instance type</summary>
        public void SerializeScript(PropertyDescriptorCollection maxProperties, 
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            GenericProperty scriptDesc = 
                maxProperties[DataTypes.SCRIPT_DESCRIPTION] as GenericProperty;
            string scriptDescription = Util.ResolveNullToEmptyString(scriptDesc.Value.ToString());

            writer.WriteAttributeString(Defaults.xmlAttrDescription, null, scriptDescription);

            writer.WriteFullEndElement();
        }

        /// <summary>Writes out project metadata and using statements</summary>
        public void SerializeProject(PropertyDescriptorCollection maxProperties,
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            GenericProperty appDisplayName =
                maxProperties[DataTypes.APP_DISPLAY_NAME_META] as GenericProperty;
            string appDisplayNameValue = Util.GetPropertyValue(appDisplayName, true);
            if(appDisplayNameValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrName, null, appDisplayNameValue);

            GenericProperty appDescription =
                maxProperties[DataTypes.APP_DESCRIPTION_META] as GenericProperty;
            string appDescriptionValue = Util.GetPropertyValue(appDescription, true);
            if(appDescriptionValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrDescription, null, appDescriptionValue);

            GenericProperty appCompany =
                maxProperties[DataTypes.APP_COMPANY_META] as GenericProperty;
            string appCompanyValue = Util.GetPropertyValue(appCompany, true);
            if(appCompanyValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrCompany, null, appCompanyValue);

            GenericProperty appAuthor =
                maxProperties[DataTypes.APP_AUTHOR_META] as GenericProperty;
            string appAuthorValue = Util.GetPropertyValue(appAuthor, true);
            if(appAuthorValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrAuthor, null, appAuthorValue);

            GenericProperty appCopyright =
                maxProperties[DataTypes.APP_COPYRIGHT_META] as GenericProperty;
            string appCopyrightValue = Util.GetPropertyValue(appCopyright, true);
            if(appCopyrightValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrCopyright, null, appCopyrightValue);

            GenericProperty appTrademark =
                maxProperties[DataTypes.APP_TRADEMARK_META] as GenericProperty;
            string appTrademarkValue = Util.GetPropertyValue(appTrademark, true);
            if(appTrademarkValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrTrademark, null, appTrademarkValue);

            GenericProperty appVersion =
                maxProperties[DataTypes.APP_VERSION_META] as GenericProperty;
            string appVersionValue = Util.GetPropertyValue(appVersion, true);
            if(appVersionValue != null)
                writer.WriteAttributeString(Defaults.xmlAttrVersion, null, appVersionValue);

            GenericProperty parentUsing = 
                maxProperties[DataTypes.USINGS] as GenericProperty;
      
            if(parentUsing != null)
                if(parentUsing.ChildrenProperties != null)
                    if(parentUsing.ChildrenProperties.Count != 0)
                    {
                        string[] usings = new string[parentUsing.ChildrenProperties.Count];
                        for(int i = 0; i < parentUsing.ChildrenProperties.Count; i++)
                        {   
                            writer.WriteStartElement(Defaults.xmlEltUsing);
                            writer.WriteString(Util.ResolveNullToEmptyString(
                                (parentUsing.ChildrenProperties[i] as MaxProperty).Value.ToString()));
                            writer.WriteEndElement();
                        }
                    }

            writer.WriteFullEndElement();
        }

        public void SerializeCode(PropertyDescriptorCollection maxProperties, 
            XmlTextWriter writer)
        {
            writer.WriteStartElement(Defaults.xmlEltProperties, null);

            CodeProperty Code = maxProperties[DataTypes.USER_CODE] as CodeProperty;
      
            LanguageProperty language = maxProperties[DataTypes.LANGUAGE] as LanguageProperty;

            if(language != null)
                writer.WriteAttributeString(Defaults.xmlAttrLanguage, null, language.Value.ToString());
      
            WriteLogging(maxProperties, writer);

            if(Code != null)
                writer.WriteString(Util.ResolveNullToEmptyString(Code.Value.ToString()));

            writer.WriteFullEndElement();
        }
        #endregion

        #region Deserialization Methods

        private void DeserializeActionInstance(object selectableObject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxActionType actionXml = DeserializeActionTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_ACTION_NODE);

            PopulateActionGrid(actionXml, properties, selectableObject); 
        } 

        private void DeserializeEventInstance(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxEventType eventXml = DeserializeEventTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_EVENT_NODE);

            PopulateEventNode(eventXml, properties, subject);
        }

        private void DeserializeFunctionInstance(object subject, 
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxFunctionType functionXml = DeserializeFunctionTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_FUNCTION_NODE);

            PopulateFunctionNode(functionXml, properties, subject);
        }
 

        private void DeserializeLinkInstance(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            //      MaxLinkType linkXml = DeserializeLinkType(node, Defaults.ERROR_DESERIALIZE_LINK_NODE);
            //
            //      PopulateLinkNode(linkXml, properties, subject);
        }

        private void DeserializeLoopInstance(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxLoopType loopXml = DeserializeLoopTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_LOOP_NODE);

            PopulateLoopNode(loopXml, properties, subject);
        }
    

        private void DeserializeLocalVariableInstance(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxLocalVariableType variableXml = DeserializeLocalVariableTypeCustom(node, 
                Defaults.ERROR_DESERIALIZE_VARIABLE_NODE);

            PopulateLocalVariableNode(variableXml, properties, subject);
        }

        private void DeserializeGlobalVariableInstance(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxGlobalVariableType variableXml = DeserializeGlobalVariableTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_VARIABLE_NODE);

            PopulateGlobalVariableNode(variableXml, properties, subject);
        }

        public void DeserializeScript(object subject, 
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxScriptType scriptXml = DeserializeScriptTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_SCRIPT);

            PopulateScript(scriptXml, properties, subject);
        }

        public void DeserializeProject(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxProjectType projectXml = DeserializeProjectTypeCustom(node,
                Defaults.ERROR_DESERIALIZE_PROJECT);

            PopulateProject(projectXml, properties, subject);
        }

        public void DeserializeCode(object subject,
            PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxCodeType CodeXml = DeserializeCodeCustom(node,
                Defaults.ERROR_DESERIALIZE_USER_CODE);

            PopulateCode(CodeXml, properties, subject);
        }

        #endregion

        #region Populating

        private void PopulateActionGrid(MaxActionType actionXml, 
            PropertyDescriptorCollection properties, object selectableObject)
        {
            if(actionXml.@params != null)
                PopulateActionParameters(actionXml.@params, properties, selectableObject);

            if(actionXml.results != null)
                PopulateResultData(actionXml.results, properties, selectableObject);

            if(actionXml.logging != null)
                PopulateLogging(actionXml.logging, properties, selectableObject);

            if(actionXml.log != null)
                PopulateMasterLoging(actionXml.log, properties);
        }

        private ComplexTypeProperty[] GetComplexTypes(PropertyDescriptorCollection properties)
        {
            if(properties == null || properties.Count == 0)   return null;

            ArrayList complexTypes = new ArrayList();
            foreach(MaxProperty property in properties)
                if(property is ComplexTypeProperty)
                    complexTypes.Add(property);

            if(complexTypes.Count == 0) return null;

            return complexTypes.ToArray(typeof(ComplexTypeProperty)) as ComplexTypeProperty[];
        }

        public class ReplicatedParamHolder
        {
            public MaxProperty property;
            public actionParamType param;

            public ReplicatedParamHolder(MaxProperty property, actionParamType param)
            {
                this.property = property;
                this.param = param;
            }
        }

        private void PopulateActionParameters(actionParamType[] actionParams, 
            PropertyDescriptorCollection properties, object subject)
        {
            StringCollection replicatedActionParams = new StringCollection();
            ArrayList extraReplicatedActionParams = new ArrayList();

            foreach(actionParamType actionParam in actionParams)
            {
                bool isRegularActionParam = false;
                bool isExtra = false;

                #region Web Services Code
                ComplexTypeProperty[] complexParents = GetComplexTypes(properties);
                ComplexTypeProperty parent = ComplexTypeProperty.FindComplexParent(actionParam.name, complexParents);
                #endregion
  
                foreach(MaxProperty property in properties)
                {
                    MaxProperty temporaryProperty;
                    if(!(property is ActionParameterProperty) && !(property is ComplexTypeProperty))  continue;

                    #region Web Services Code
                    if(parent != null && property == parent)
                    {
                        string name = MaxPropertiesManager.OverrideIfWebServicesName(actionParam.name);
                        temporaryProperty = property.ChildrenProperties[name] as ActionParameterProperty;
                        if(property == null)  continue;
                    }
                    else if(parent == null)
                    {
                        temporaryProperty = property;
                    }
                    else continue;

                    #endregion

                    if(parent != null || String.Compare(temporaryProperty.Name, actionParam.name, true) == 0)
                    {
                        if(replicatedActionParams.Contains(actionParam.name))
                        {
                            extraReplicatedActionParams.Add(new ReplicatedParamHolder(temporaryProperty, actionParam));
                            isExtra = true;
                            break;
                        }
                        isRegularActionParam = true;

                        temporaryProperty.Value = Util.ResolveNullToEmptyString(actionParam.Value);

                        UserTypeProperty typeProp 
                            = temporaryProperty.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;
                        typeProp.Value = Util.FromMetreosToMaxActPar(actionParam.type);
                        replicatedActionParams.Add(actionParam.name);
                        break;
                    }
                }
                if(isExtra) continue;
                if(isRegularActionParam) continue;

                bool foundGrowable = false;

                foreach(MaxProperty property in properties)
                {
                    if(!(property is ActionParameterPropertyGrowable))   continue;

                    foundGrowable = true;

                    ActionParameterProperty actionProperty = 
                        new ActionParameterProperty(
                        actionParam.name,
                        actionParam.name,
                        Util.ResolveNullToEmptyString(actionParam.Value), 
                        false, 
                        false,
                        actionParam.name,
                        null, 
                        mpm,
                        subject, 
                        properties);

                    UserTypeProperty typeProperty = 
                        new UserTypeProperty(Util.FromMetreosToMaxActPar(actionParam.type),
                        mpm, subject, properties);

                    RenamingProperty renamingProperty = 
                        new RenamingProperty(actionParam.name, actionProperty, mpm,
                        subject, properties);

                    actionProperty.ChildrenProperties.Add(typeProperty);
                    actionProperty.ChildrenProperties.Add(renamingProperty);
                    property.ChildrenProperties.Add(actionProperty);

                    break;
                }
    
                if(!foundGrowable) continue; // TODO: pass up deserialization error/warning        
            }

            // Found extra params. Will need to create
            foreach(ReplicatedParamHolder actionParam in extraReplicatedActionParams)
            {
                ActionParameterProperty actionProperty =
                    new ActionParameterProperty(
                    actionParam.param.name,
                    actionParam.property.DisplayName,
                    Util.ResolveNullToEmptyString(actionParam.param.Value),
                    false,
                    true,
                    actionParam.property.Description,
                    (actionParam.property as ActionParameterProperty).EnumValues,
                    mpm,
                    subject, 
                    properties);

                UserTypeProperty typeProperty = 
                    new UserTypeProperty(Util.FromMetreosToMaxActPar(actionParam.param.type),
                    mpm, subject, properties);

                actionProperty.ChildrenProperties.Add(typeProperty);
                properties.Add(actionProperty);
            }
        }

        private void PopulateResultData(resultDataType[] resultData, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(resultDataType resultDatum in resultData)
            {
                bool isRegularResultDatum = false;

                foreach(MaxProperty property in properties)
                {
                    if(!(property is ResultDataProperty))   continue;

                    if(property.Name == resultDatum.field)
                    {
                        property.Value = Util.ResolveNullToEmptyString(resultDatum.Value);
                        isRegularResultDatum = true;
                        break;
                    }
                }

                if(isRegularResultDatum) continue;

                bool foundGrowable = false;

                foreach(MaxProperty property in properties)
                {
                    if(!(property is ResultDataPropertyGrowable))   continue;

                    foundGrowable = true;

                    ResultDataProperty resultDatumProperty = new ResultDataProperty(resultDatum.field, resultDatum.field,
                        Util.ResolveNullToEmptyString(resultDatum.Value), mpm, subject, properties);

                    property.ChildrenProperties.Add(resultDatumProperty);
                }

                if(!foundGrowable)  continue; // TODO: pass up deserialization error/warning      
            }
        }

        private void PopulateLogging(LogType[] logs, 
            PropertyDescriptorCollection properties, object subject)
        {
            LoggingProperty parentLog = null;
            foreach(MaxProperty property in properties)
            {
                if(!(property is LoggingProperty)) continue;

                parentLog = property as LoggingProperty;
            }

            if(parentLog == null) return; // TODO: mark error


            foreach(LogType log in logs)
            {
                bool isRegularLog = false;

                foreach(MaxProperty property in parentLog.ChildrenProperties)
                {
                    if(!(property is OnLogEventProperty))   continue;

                    if(property.Name == log.condition)
                    {
                        isRegularLog = true;

                        property.Value = Util.ResolveNullToEmptyString(log.Value);
            
                        foreach(MaxProperty childProperty in property.ChildrenProperties)
                        {
                            if(childProperty is TraceLevelProperty)
                                childProperty.Value = log.level;

                            else if(childProperty is OnOffProperty)
                                childProperty.Value = log.on ? DataTypes.OnOff.On : DataTypes.OnOff.Off;

                            else if(childProperty is UserTypeProperty)
                                childProperty.Value = log.type;
                        }
                    }
                }

                if(isRegularLog)  continue;

                bool foundGrowable = false;

                foreach(MaxProperty property in parentLog.ChildrenProperties)
                {
                    if(!(property is OnLogEventPropertyGrowable))   continue;

                    foundGrowable = true;

                    OnLogEventProperty newLog = new OnLogEventProperty(log.condition, 
                        Util.ResolveNullToEmptyString(log.Value), mpm, subject, properties);

                    TraceLevelProperty newTrace 
                        = new TraceLevelProperty(log.level, mpm, subject, properties);

                    OnOffProperty onOffProperty 
                        = new OnOffProperty(log.on ? DataTypes.OnOff.On : DataTypes.OnOff.Off, 
                        mpm, subject, properties);

                    UserTypeProperty userType = new UserTypeProperty(
                        Util.FromMetreosToMaxActPar(log.type), mpm, subject, properties);

                    newLog.ChildrenProperties.Add(newTrace);
                    newLog.ChildrenProperties.Add(onOffProperty);
                    newLog.ChildrenProperties.Add(userType);

                    property.ChildrenProperties.Add(newLog);
                }

                if(!foundGrowable)  continue; // TODO: Log error
            }
        }

        private void PopulateMasterLoging(string log, PropertyDescriptorCollection properties)
        {
            DataTypes.OnOff masterLog = DataTypes.OnOff.On;

            try
            {
                masterLog = (DataTypes.OnOff) Enum.Parse(typeof(DataTypes.OnOff), log, true);
            }
            catch{}

            foreach(MaxProperty property in properties)
            {
                if(property is LoggingProperty)
                {
                    LoggingProperty logProperty = property as LoggingProperty;

                    logProperty.Value = masterLog;

                    break;
                }
            }
        }

        private void PopulateEventNode(MaxEventType eventXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
                if(property is HiddenGenericProperty)
                    if(property.Name == DataTypes.EVENT_TYPE)
                    {
                        if(property.Value as string == String.Empty || property.Value == null)
                            property.Value = eventXml.eventType.ToString();

                        break;
                    }

            if(eventXml.@params != null)
                PopulateEventParams(eventXml.@params, properties);     
        }

        private void PopulateEventParams(eventParamType[] eventParams, PropertyDescriptorCollection properties)
        {
            foreach(eventParamType eventParam in eventParams)
            {
                bool foundParam = false;

                foreach(MaxProperty property in properties)
                {
                    if(property.Name == eventParam.name)
                    {
                        foundParam = true;

                        property.Value = Util.ResolveNullToEmptyString(eventParam.Value);

                        foreach(MaxProperty childProperty in property.ChildrenProperties)
                        {
                            if(childProperty is EventParameterInitWithProperty)
                            {
                                if(childProperty.Name == DataTypes.CONFIG_INIT_WITH) 
                                    childProperty.Value = Util.ResolveNullToEmptyString(eventParam.Value);   
                            }

                            else if(childProperty is EventParamTypeProperty)
                                childProperty.Value = Util.FromMetreosToMaxEventParamType(eventParam.type);
                        }
                        break;
                    }
                }

                if(!foundParam) continue; // Log error
            }
        }

        private void PopulateFunctionNode(MaxFunctionType functionXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            //      foreach(MaxProperty property in properties)
            //        if(property is GenericProperty)
            //          if(property.Name == DataTypes.FUNCTION_NAME) // TODO: not good to key off visual name
            //            property.Value = Util.ResolveNullToEmptyString(functionXml.name);
        }

        private void PopulateLinkNode(MaxLinkType linkXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            // TODO : Work this out, must run up delegates with Jim
            return;
        }

        private string ResolveRealToDisplay(string realVarType)
        { 
            if(realVarType == null) return Defaults.TYPE;


            Metreos.PackageGeneratorCore.PackageXml.nativeTypePackageType[] allNativeTypes = mpm.GetNativeTypesInfoDelegate();
            if(allNativeTypes == null || allNativeTypes.Length == 0) return realVarType;

            foreach(Metreos.PackageGeneratorCore.PackageXml.nativeTypePackageType nativeType in allNativeTypes)
            {
                if(nativeType.type == null || nativeType.type.Length == 0) continue;

                foreach(Metreos.PackageGeneratorCore.PackageXml.typeType type in nativeType.type)
                {
                    if(Util.MakeFullyQualified(nativeType.name, type.name) == realVarType) return (type.displayName != null && type.displayName != String.Empty) 
                                                     ? type.displayName : type.name;
                }
            }

            return realVarType;
        }

        private void PopulateLocalVariableNode(MaxLocalVariableType variableXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
            {
                if(property is VariableProperty)
                {
                    VariableProperty varProperty = property as VariableProperty;

                    string resolvedName = ResolveRealToDisplay(variableXml.varType);
                    varProperty.VariableDisplayName = resolvedName;
                    varProperty.Value = resolvedName;
                    varProperty.VariableName = variableXml.varType != null ? variableXml.varType : Defaults.TYPE;       
                }

                else if(property is GenericProperty)
                {
                    if(property.Name == DataTypes.VARIABLE) 
                        property.Value = Util.ResolveNullToEmptyString(variableXml.Value);
                    else if(property.Name == DataTypes.DEFAULT_INIT_WITH)
                        property.Value = Util.ResolveNullToEmptyString(variableXml.defaultInitWith);
                }
                else if(property is LocalVarInitWithProperty)
                {
                    LocalVarInitWithProperty initWithProp = property as LocalVarInitWithProperty;
                    if(property.Name == DataTypes.LOCAL_VAR_INIT_WITH)
                    {
                        EventParameter eParam = mpm.GetEventParameterByName(variableXml.eventName, variableXml.initWith);
                        if(eParam == null || eParam.DisplayName == null)
                        {
                            initWithProp.Value = Util.ResolveNullToEmptyString(variableXml.initWith);
                            initWithProp.InnerName = variableXml.initWith;  
                        }
                        else
                        {
                            initWithProp.Value = Util.ResolveNullToEmptyString(eParam.DisplayName);
                            initWithProp.InnerName = variableXml.initWith;
                            initWithProp.InnerEventName = variableXml.eventName;
                        }
                    }
                }
                else if(property is ReferenceProperty)
                {
                    if(property.Name == DataTypes.REFERENCE_TYPE_NAME)
                        property.Value = variableXml.referenceType;
                }
            }
        }

        private void PopulateGlobalVariableNode(MaxGlobalVariableType variableXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
            {
                if(property is VariableProperty)
                {
                    VariableProperty varProperty = property as VariableProperty;
                    string resolvedName = ResolveRealToDisplay(variableXml.varType);
                    varProperty.VariableDisplayName = resolvedName;
                    varProperty.Value = resolvedName;
                    varProperty.VariableName = variableXml.varType != null ? variableXml.varType : Defaults.TYPE;       
                }
                else if(property is GenericProperty)
                {
                    if(property.Name == DataTypes.VARIABLE) 
                        property.Value = Util.ResolveNullToEmptyString(variableXml.Value);
                    else if(property.Name == DataTypes.DEFAULT_INIT_WITH)
                        property.Value = Util.ResolveNullToEmptyString(variableXml.defaultInitWith);
                }
                else if(property is GlobalVarInitWithProperty)
                {
                    if(property.Name == DataTypes.CONFIG_INIT_WITH)
                        property.Value = Util.ResolveNullToEmptyString(variableXml.initWith);
                }
            }
        }

        private void PopulateLoopNode(MaxLoopType loopXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
            {
                if(property is LoopCountProperty)
                {
                    if(property.Name == DataTypes.COUNT)
                        property.Value = Util.ResolveNullToEmptyString(loopXml.Value);

                    foreach(MaxProperty innerProperty in property.ChildrenProperties)
                    {
                        if(innerProperty is LoopTypeProperty && innerProperty.Name == DataTypes.LOOP_TYPE_NAME)
                            innerProperty.Value = Util.FromMetreosToMaxLoopType(loopXml.type);

                        else if(innerProperty is LoopControllerTypeProperty && innerProperty.Name == DataTypes.LOOP_ITERATE_TYPE_NAME)
                            innerProperty.Value = Util.FromMetreosToMaxLoopCountType(loopXml.loopIteratorType);
                    }
                }
            }
        }

        private void PopulateScript(MaxScriptType scriptXml, 
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
            {
                if(property.Name == DataTypes.SCRIPT_DESCRIPTION)
                    property.Value = scriptXml.description;
            }
        }

        private void PopulateProject(MaxProjectType projectXml,
            PropertyDescriptorCollection properties, object subject)
        {
            foreach(MaxProperty property in properties)
            {
                if(property.Name == DataTypes.APP_DISPLAY_NAME_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.name);

                else if(property.Name == DataTypes.APP_DESCRIPTION_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.description);

                else if(property.Name == DataTypes.APP_COMPANY_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.company);

                else if(property.Name == DataTypes.APP_AUTHOR_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.author);

                else if(property.Name == DataTypes.APP_COPYRIGHT_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.copyright);

                else if(property.Name == DataTypes.APP_TRADEMARK_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.trademark);

                else if(property.Name == DataTypes.APP_VERSION_META)
                    property.Value = Util.ResolveNullToEmptyString(projectXml.version);

                else if(property.Name == DataTypes.USINGS && property is HiddenGenericProperty)
                {
                    if(projectXml.usings == null || projectXml.usings.Length == 0) continue;

                    foreach(@using usingContainer in projectXml.usings)
                    {
                        if(Array.IndexOf(Metreos.ApplicationFramework.Assembler.Assembler.usings, usingContainer.Value) < 0)
                        {
                            GenericProperty newUsingStatement = 
                                new GenericProperty(DataTypes.USING, usingContainer.Value, false, mpm, null, properties); 

                            property.ChildrenProperties.Add(newUsingStatement);
                        }
                    }
                }
            }
        }

        private void PopulateCode(MaxCodeType codeXml,
            PropertyDescriptorCollection properties, object subject)
        {
            CodeProperty code = properties[DataTypes.USER_CODE] as CodeProperty;

            if(codeXml != null)
                code.Value = Util.ResolveNullToEmptyString(codeXml.Value);

            if(codeXml.logging != null)
                PopulateLogging(codeXml.logging, properties, subject);

            LanguageProperty language = properties[DataTypes.LANGUAGE] as LanguageProperty;

            language.Value = codeXml.language;
        }

        #endregion    

        #region Deserialization without XmlSerializer
    
        private MaxActionType DeserializeActionTypeCustom(XmlNode node, string errorMessage)
        {
            MaxActionType actionXml = new MaxActionType();
            actions.Clear();
            results.Clear();
            logs.Clear();
      
            foreach(XmlNode childNode in node.ChildNodes)
            {
                switch(childNode.Name)
                {
                    case Defaults.xmlEltActionParameter:
          
                        actionParamType actionParam = new actionParamType();
                        actionParam.name = childNode.Attributes["name"].Value;
                        actionParam.type = Util.ExtractActionParamTypeType(childNode.Attributes["type"].Value);
                        actionParam.Value = childNode.InnerText;

                        actions.Add(actionParam);

                        break;

                    case Defaults.xmlEltResultData:

                        resultDataType resultVar = new resultDataType();
                        resultVar.field = childNode.Attributes[Defaults.xmlAttrField].Value;
                        resultVar.Value = childNode.InnerText;
                        resultVar.type = resultDataTypeType.variable;

                        results.Add(resultVar);

                        break;

                    case Defaults.xmlEltLogging:

                        LogType log = new LogType();
                        log.condition = childNode.Attributes["condition"].Value;
                        log.on = bool.Parse( childNode.Attributes["on"].Value );
                        log.level = Util.ExtractLogLevel(childNode.Attributes["level"].Value);
                        log.type = Util.ExtractActionParamTypeType(childNode.Attributes["type"].Value);
                        log.Value = childNode.InnerText;

                        logs.Add(log);

                        break;
                }
            }

            if(actions.Count != 0)
            {
                actionXml.@params = new actionParamType[actions.Count];
                actions.CopyTo(actionXml.@params);
            }

            if(results.Count != 0)
            {
                actionXml.results = new resultDataType[results.Count];
                results.CopyTo(actionXml.results);
            }

            if(logs.Count != 0)
            {
                actionXml.logging = new LogType[logs.Count];
                logs.CopyTo(actionXml.logging);
            }

            try
            {
                actionXml.final = bool.Parse(node.Attributes[Defaults.xmlAttrFinal].Value);
            } 
            catch
            {
                actionXml.final = false;
            }
            actionXml.type = Util.ExtractActionTypeType(node.Attributes[Defaults.xmlAttrType].Value);

            string logValue = GetAttributeValue(node, Defaults.xmlAttrLog, false);
            if(logValue !=null)
            {
                actionXml.log = logValue;
            }
            return actionXml;
        }  

        private MaxEventType DeserializeEventTypeCustom(XmlNode node, string errorMessage)
        {
            MaxEventType eventXml = new MaxEventType();
      
            eventParams.Clear();

            XmlAttribute typeAttr = node.Attributes[Defaults.xmlAttrType];

            if(typeAttr != null)
                eventXml.eventType = (EventType) Enum.Parse(typeof(EventType), typeAttr.Value, true);
            else
                eventXml.eventType = EventType.Unknown;

            foreach(XmlNode childNode in node.ChildNodes)
            {
                eventParamType eventParam = new eventParamType();
        
                eventParam.name = childNode.Attributes[Defaults.xmlAttrName].Value;
                eventParam.type = Util.ExtractEventParamTypeMetreos(childNode.Attributes[Defaults.xmlAttrType].Value);
                eventParam.Value = childNode.InnerText; 

                eventParams.Add(eventParam);
            }

            if(eventParams.Count != 0)
            {
                eventXml.@params = new eventParamType[eventParams.Count];
                eventParams.CopyTo(eventXml.@params);
            }

            return eventXml;
        }

        private MaxFunctionType DeserializeFunctionTypeCustom(XmlNode node, string errorMessage)
        {
            MaxFunctionType functionXml = new MaxFunctionType();
      
            //functionXml.name = node.Attributes[Defaults.xmlAttrName].Value;

            return functionXml;
        }

        private MaxLoopType DeserializeLoopTypeCustom(XmlNode node, string errorMessage)
        {
            MaxLoopType loopXml = new MaxLoopType();
            XmlAttribute loopTypeAttr = node.Attributes[Defaults.xmlAttrType];
            XmlAttribute loopIteratorTypeAttr = node.Attributes[Defaults.xmlAttrLoopIteratorType];

            loopXml.type = loopTypeAttr != null ?
                Util.ExtractLoopType(loopTypeAttr.Value) : Defaults.LOOP_TYPE;
            loopXml.loopIteratorType = loopIteratorTypeAttr != null ?
                Util.ExtractLoopCountType(loopIteratorTypeAttr.Value) : Defaults.LOOP_ITERATE_TYPE_METREOS;

            loopXml.Value = node.InnerText;

            return loopXml;
        }

        private MaxLocalVariableType DeserializeLocalVariableTypeCustom(XmlNode node, string errorMessage)
        {
            MaxLocalVariableType variableXml = new MaxLocalVariableType();

            XmlAttribute initWithAttr = node.Attributes[Defaults.xmlAttrInitWith];
            XmlAttribute eventNameAttr = node.Attributes[Defaults.xmlAttrName];
            XmlAttribute defaultInitWithAttr = node.Attributes[Defaults.xmlAttrDefaultInitWith];
            XmlAttribute referenceTypeAttr = node.Attributes[Defaults.xmlAttrRefType];

            variableXml.initWith = initWithAttr != null ?
                Util.ResolveNullToEmptyString(initWithAttr.Value) : null;
            variableXml.eventName = eventNameAttr != null ?
                Util.ResolveNullToEmptyString(eventNameAttr.Value) : null;
            variableXml.defaultInitWith = defaultInitWithAttr != null ?
                Util.ResolveNullToEmptyString(defaultInitWithAttr.Value) : null;
            variableXml.referenceType = referenceTypeAttr != null ?
                Util.ExtractReferenceType(referenceTypeAttr.Value) : Defaults.REFERENCE_TYPE;
            
            variableXml.varType = node.Attributes[Defaults.xmlAttrType].Value;

            // TEMPORARY 1.0 to 1.1 helper
            // START
            if(variableXml.varType == "Metreos.Types.Integer")
                variableXml.varType = "Metreos.Types.Int";
            else if(variableXml.varType == "Metreos.Types.Boolean")
                variableXml.varType = "Metreos.Types.Bool";
            // END

            variableXml.Value = node.InnerText;

            return variableXml;
        }

        private MaxGlobalVariableType DeserializeGlobalVariableTypeCustom(XmlNode node, string errorMessage)
        {
            MaxGlobalVariableType variableXml = new MaxGlobalVariableType();

            XmlAttribute initWithAttr = node.Attributes[Defaults.xmlAttrInitWith];
            XmlAttribute defaultInitWithAttr = node.Attributes[Defaults.xmlAttrDefaultInitWith];

            variableXml.initWith = initWithAttr != null ?
                Util.ResolveNullToEmptyString(initWithAttr.Value) : null;
            variableXml.defaultInitWith = defaultInitWithAttr != null ?
                Util.ResolveNullToEmptyString(defaultInitWithAttr.Value) : null;

            variableXml.varType = node.Attributes[Defaults.xmlAttrType].Value;

            // TEMPORARY 1.0 to 1.1 helper
            // START
            if(variableXml.varType == "Metreos.Types.Integer")
                variableXml.varType = "Metreos.Types.Int";
            else if(variableXml.varType == "Metreos.Types.Boolean")
                variableXml.varType = "Metreos.Types.Bool";
            // END

            variableXml.Value = node.InnerText;

            return variableXml;
        }

        private MaxScriptType DeserializeScriptTypeCustom(XmlNode node, string errorMessage)
        {
            MaxScriptType scriptXml = new MaxScriptType();

            XmlAttribute scriptDescriptionAttr = node.Attributes[Defaults.xmlAttrDescription];

            scriptXml.description   = scriptDescriptionAttr != null ?
                Util.ResolveNullToEmptyString(scriptDescriptionAttr.Value) : String.Empty;

            return scriptXml;
        }

        private MaxProjectType DeserializeProjectTypeCustom(XmlNode node, string errorMessage)
        {
            MaxProjectType projectXml = new MaxProjectType();

            XmlAttribute appDisplayNameAttr = node.Attributes[Defaults.xmlAttrName];
            XmlAttribute appDescriptionAttr = node.Attributes[Defaults.xmlAttrDescription];
            XmlAttribute appCompanyAttr     = node.Attributes[Defaults.xmlAttrCompany];
            XmlAttribute appAuthorAttr      = node.Attributes[Defaults.xmlAttrAuthor];
            XmlAttribute appCopyrightAttr   = node.Attributes[Defaults.xmlAttrCopyright];
            XmlAttribute appTrademarkAttr   = node.Attributes[Defaults.xmlAttrTrademark];
            XmlAttribute appVersionAttr     = node.Attributes[Defaults.xmlAttrVersion];

            projectXml.name                 = Util.ResolveAttributeValue(appDisplayNameAttr);
            projectXml.description          = Util.ResolveAttributeValue(appDescriptionAttr);
            projectXml.company              = Util.ResolveAttributeValue(appCompanyAttr);
            projectXml.author               = Util.ResolveAttributeValue(appAuthorAttr);
            projectXml.copyright            = Util.ResolveAttributeValue(appCopyrightAttr);
            projectXml.trademark            = Util.ResolveAttributeValue(appTrademarkAttr);
            projectXml.version              = Util.ResolveAttributeValue(appVersionAttr);

            // Determine usings
            ArrayList usings = new ArrayList();

            foreach(XmlNode childNode in node.ChildNodes)
                if(childNode.Name == Defaults.xmlEltUsing)
                    if(childNode.InnerXml != null)
                        usings.Add(childNode.InnerXml);
       
            if(usings.Count != 0)
            {
                projectXml.usings = new @using[usings.Count];
                for(int i = 0; i < usings.Count; i++)
                {
                    string usingText = usings[i] as string;
                    @using usingContainer = new @using();
                    usingContainer.Value = usingText;
                    projectXml.usings[i] = usingContainer;
                }
            }

            return projectXml;
        }

        public MaxCodeType DeserializeCodeCustom(XmlNode node, string errorMessage)
        {
            MaxCodeType codeXml = new MaxCodeType();

            foreach(XmlNode childnode in node.ChildNodes)
                // The only node of this type for a custom code action node is the code itself
                if(childnode.NodeType == XmlNodeType.Text)
                {
                    codeXml.Value = childnode.InnerText;
                    break;
                }

            XmlAttribute languageAttr = node.Attributes[Defaults.xmlAttrLanguage];

            if(languageAttr != null)
                codeXml.language = Util.ExtractLanguageType(languageAttr.Value);
            else
                codeXml.language = Defaults.USER_CODE_LANGUAGE;

            ArrayList logs = new ArrayList();

            foreach(XmlNode childNode in node.ChildNodes)
            {
                switch(childNode.Name)
                {
                    case Defaults.xmlEltLogging:

                        LogType log = new LogType();
                        log.condition = childNode.Attributes["condition"].Value;
                        log.on = bool.Parse( childNode.Attributes["on"].Value );
                        log.level = Util.ExtractLogLevel(childNode.Attributes["level"].Value);
                        log.type = Util.ExtractActionParamTypeType(childNode.Attributes["type"].Value);
                        log.Value = childNode.InnerText;

                        logs.Add(log);

                        break;
                }
            }

            if(logs.Count != 0)
            {
                codeXml.logging = new LogType[logs.Count];
                logs.CopyTo(codeXml.logging);
            }
            return codeXml;
        }

        /// <summary>
        /// Gets the value of a specified attribute from a node.
        /// </summary>
        public static string GetAttributeValue(XmlNode node, string xmlAttrName, bool requiredPresent)
        {
            if(node is XmlComment)
                return null;

            XmlAttribute attr = node.Attributes[xmlAttrName];
            if(attr != null)
            {
                return attr.Value;
            }
            else
            {
                if(requiredPresent)
                    throw new Exception(IErrors.xmlDataCorrupt);
                else
                    return null;
            }
        }

        #endregion

        #region Deserialization with XmlSerializer

        private MaxActionType DeserializeActionType(XmlNode node, string errorMessage)
        {
            string actionRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);

            StringReader stringReader = new StringReader(actionRawXml);

            MaxActionType actionXml = null;

            try
            {
                actionXml = actionDeserializer.Deserialize(stringReader) as MaxActionType;                      
            }
            catch(Exception e)
            {
                throw new Exception(errorMessage, e);
            }
            finally
            {
                stringReader.Close();
            }    

            return actionXml;    
        }    


        private MaxEventType DeserializeEventType(XmlNode node, string errorMessage)
        {
            string eventRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);

            StringReader stringReader = new StringReader(eventRawXml);

            MaxEventType eventXml = null;

            try
            {
                eventXml = eventDeserializer.Deserialize(stringReader) as MaxEventType;                      
            }
            catch(Exception e)
            {
                throw new Exception(errorMessage, e);
            }
            finally
            {
                stringReader.Close();
            }    

            return eventXml;    
        }

        private MaxFunctionType DeserializeFunctionType(XmlNode node, string errorMessage)
        {
            string functionRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);

            StringReader stringReader = new StringReader(functionRawXml);

            MaxFunctionType functionXml = null;

            try
            {
                functionXml = functionDeserializer.Deserialize(stringReader) as MaxFunctionType;                      
            }
            catch(Exception e)
            {
                throw new Exception(errorMessage, e);
            }
            finally
            {
                stringReader.Close();
            }    

            return functionXml;    
        }

        private MaxLinkType DeserializeLinkType(XmlNode node, string errorMessage)
        {
            return null;
            //      string linkRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);
            //
            //      StringReader stringReader = new StringReader(linkRawXml);
            //
            //      MaxLinkType linkXml = null;
            //
            //      try
            //      {
            //        linkXml = linkDeserializer.Deserialize(stringReader) as MaxLinkType;                      
            //      }
            //      catch(Exception e)
            //      {
            //        throw new Exception(errorMessage, e);
            //      }
            //      finally
            //      {
            //        stringReader.Close();
            //      }    
            //
            //      return linkXml;    
        }

        private MaxLoopType DeserializeLoopType(XmlNode node, string errorMessage)
        {
            string loopRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);

            StringReader stringReader = new StringReader(loopRawXml);

            MaxLoopType loopXml = null;

            try
            {
                loopXml = loopDeserializer.Deserialize(stringReader) as MaxLoopType;                      
            }
            catch(Exception e)
            {
                throw new Exception(errorMessage, e);
            }
            finally
            {
                stringReader.Close();
            }    

            return loopXml;    
        }

        private MaxLocalVariableType DeserializeVariableType(XmlNode node, string errorMessage)
        {
            string variableRawXml = string.Concat(Defaults.xmlDocPrepend, node.OuterXml);

            StringReader stringReader = new StringReader(variableRawXml);

            MaxLocalVariableType variableXml = null;

            try
            {
                variableXml = localVariableDeserializer.Deserialize(stringReader) as MaxLocalVariableType;                      
            }
            catch(Exception e)
            {
                throw new Exception(errorMessage, e);
            }
            finally
            {
                stringReader.Close();
            }    

            return variableXml;    
        }
        #endregion

        #region Deserialization Of Non-packaged Action/Event instances

        private void DeserializePackagelessActionInstance(object selectableObject, PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxActionType actionXml = DeserializeActionTypeCustom(node, Defaults.ERROR_DESERIALIZE_ACTION_NODE);

            PopulatePackagelessActionGrid(selectableObject, properties, actionXml);
        }

        private void DeserializePackagelessEventInstance(object selectableObject, PropertyDescriptorCollection properties, XmlNode node)
        {
            MaxEventType eventXml = DeserializeEventTypeCustom(node, Defaults.ERROR_DESERIALIZE_EVENT_NODE);

            PopulatePackagelessEventGrid(selectableObject, properties, eventXml);
        }

        #endregion

        #region Population of Non-packaged Action/Event instances

        #region Editable Version
        //    private void PopulatePackagelessActionGrid(object subject, PropertyDescriptorCollection properties, MaxActionType actionXml)
        //    {
        //      if(actionXml.@params != null)
        //      {
        //        foreach(actionParamType actionParam in actionXml.@params)
        //        {
        //          ActionParameterProperty actionProp = new ActionParameterProperty(actionParam.name, 
        //            Util.ResolveNullToEmptyString(actionParam.Value), true, 
        //            mpm.GetGlobalVarsDelegate, mpm.GetFunctionVarsDelegate, 
        //            mpm.GetNativeTypesInfoDelegate, mpm.ValueChanged, subject);
        //    
        //          
        //          UserTypeProperty typeProp = new UserTypeProperty(Util.FromMetreosToMaxActPar(actionParam.type), mpm.ValueChanged, subject);
        //
        //          actionProp.ChildrenProperties.Add(typeProp);
        //          properties.Add(actionProp);
        //        }
        //      }
        //
        //      if(actionXml.results != null)
        //      {
        //        foreach(resultDataVariableType resultVar in actionXml.results)
        //        {
        //          ResultDataProperty resultProp = new ResultDataProperty(resultVar.field, Util.ResolveNullToEmptyString(resultVar.Value),
        //            mpm.GetGlobalVarsDelegate, mpm.GetFunctionVarsDelegate, mpm.ValueChanged, subject);
        //
        //          properties.Add(resultProp);
        //        }
        //      }
        //
        //      if(actionXml.logging != null)
        //      {
        //        LoggingProperty overallLogging = new LoggingProperty(DataTypes.OnOff.On, mpm.ValueChanged, subject);
        //
        //        foreach(LogType log in actionXml.logging)
        //        {
        //          OnLogEventProperty logProperty = new OnLogEventProperty(log.condition, Util.ResolveNullToEmptyString(log.Value),
        //            mpm.GetGlobalVarsDelegate, mpm.GetFunctionVarsDelegate, mpm.GetNativeTypesInfoDelegate, mpm.ValueChanged, subject);
        //          UserTypeProperty logTypeProperty = new UserTypeProperty(Util.FromMetreosToMaxActPar(log.type), mpm.ValueChanged, subject);
        //          OnOffProperty onOffProperty = new OnOffProperty(log.on ? DataTypes.OnOff.On : DataTypes.OnOff.Off, mpm.ValueChanged, subject);
        //          TraceLevelProperty levelProperty = new TraceLevelProperty(log.level, mpm.ValueChanged, subject);
        //          
        //          logProperty.ChildrenProperties.Add(logTypeProperty);
        //          logProperty.ChildrenProperties.Add(onOffProperty);
        //          logProperty.ChildrenProperties.Add(levelProperty);
        //
        //          overallLogging.ChildrenProperties.Add(logProperty);
        //        }
        //
        //        properties.Add(overallLogging);
        //      }
        //
        //      HiddenGenericProperty final 
        //        = new HiddenGenericProperty(DataTypes.FINAL, actionXml.final.ToString(), true, mpm.ValueChanged, subject);
        //
        //      HiddenGenericProperty type
        //        = new HiddenGenericProperty(DataTypes.ACTION_TYPE, actionXml.type.ToString(), true, mpm.ValueChanged, subject);
        //
        //      properties.Add(final);
        //      properties.Add(type);
        //    }
        //
        //    private void PopulatePackagelessEventGrid(object subject, PropertyDescriptorCollection properties, MaxEventType eventXml)
        //    {
        //      if(eventXml.@params != null)
        //      {
        //        foreach(triggerParamType eventParam in eventXml.@params)
        //        {
        //          EventParameterProperty eventParamProp = new EventParameterProperty(eventParam.name, Util.ResolveNullToEmptyString(eventParam.Value), false,
        //            mpm.ValueChanged, subject);
        //
        //          RegexTypeProperty matchProp = new RegexTypeProperty(Util.FromMetreosToMaxRegex(eventParam.type), mpm.ValueChanged, subject);
        //
        //          GenericProperty initWithProperty = new GenericProperty(DataTypes.INIT_WITH, Util.ResolveNullToEmptyString(eventParam.initWith),
        //            false, mpm.ValueChanged, subject);
        //
        //          eventParamProp.ChildrenProperties.Add(matchProp);
        //          eventParamProp.ChildrenProperties.Add(initWithProperty);
        //
        //          properties.Add(eventParamProp);
        //        }
        //      }
        //    }

        #endregion

        private void PopulatePackagelessActionGrid(object subject, PropertyDescriptorCollection properties, MaxActionType actionXml)
        {
            if(actionXml.@params != null)
            {
                foreach(actionParamType actionParam in actionXml.@params)
                {
                    ActionParameterProperty actionProp = new ActionParameterProperty(actionParam.name, actionParam.name, 
                        Util.ResolveNullToEmptyString(actionParam.Value), false, true, actionParam.name, null,
                        mpm, subject, properties);
          
                    actionProp.InnerRead = true;

                    UserTypeProperty typeProp = new UserTypeProperty(
                        Util.FromMetreosToMaxActPar(actionParam.type), mpm, subject, properties);

                    typeProp.InnerRead = true;

                    actionProp.ChildrenProperties.Add(typeProp);
                    properties.Add(actionProp);
                }
            }

            if(actionXml.results != null)
            {
                foreach(resultDataType resultVar in actionXml.results)
                {
                    ResultDataProperty resultProp = new ResultDataProperty(
                        resultVar.field, resultVar.field, Util.ResolveNullToEmptyString(resultVar.Value),
                        this.mpm, subject, properties);

                    resultProp.InnerRead = true;

                    properties.Add(resultProp);
                }
            }

            if(actionXml.logging != null)
            {
                LoggingProperty overallLogging = new LoggingProperty(DataTypes.OnOff.On, mpm, subject, properties);

                overallLogging.InnerRead = true;

                foreach(LogType log in actionXml.logging)
                {
                    OnLogEventProperty logProperty 
                        = new OnLogEventProperty(log.condition, Util.ResolveNullToEmptyString(log.Value),
                        mpm, subject, properties);

                    logProperty.InnerRead = true;

                    UserTypeProperty logTypeProperty = new UserTypeProperty(
                        Util.FromMetreosToMaxActPar(log.type), mpm, subject, properties);

                    logTypeProperty.InnerRead = true;

                    OnOffProperty onOffProperty = new OnOffProperty(log.on ? DataTypes.OnOff.On : DataTypes.OnOff.Off,
                        mpm, subject, properties);

                    onOffProperty.InnerRead = true;

                    TraceLevelProperty levelProperty = new TraceLevelProperty(log.level, mpm, subject, properties);
          
                    levelProperty.InnerRead = true;

                    logProperty.ChildrenProperties.Add(logTypeProperty);
                    logProperty.ChildrenProperties.Add(onOffProperty);
                    logProperty.ChildrenProperties.Add(levelProperty);

                    overallLogging.ChildrenProperties.Add(logProperty);
                }

                properties.Add(overallLogging);
            }

            HiddenGenericProperty final 
                = new HiddenGenericProperty(DataTypes.FINAL, actionXml.final.ToString(), true, mpm, subject, properties);

            HiddenGenericProperty type
                = new HiddenGenericProperty(DataTypes.ACTION_TYPE, actionXml.type.ToString(), true, mpm, subject, properties);
            
            if(actionXml.log != null)
                PopulateMasterLoging(actionXml.log, properties);

            properties.Add(final);
            properties.Add(type);
        }

        private void PopulatePackagelessEventGrid(object subject, PropertyDescriptorCollection properties, MaxEventType eventXml)
        {
            if(eventXml.@params != null)
            {
                foreach(eventParamType eventParam in eventXml.@params)
                {
                    EventParameterProperty eventParamProp = new EventParameterProperty(eventParam.name, eventParam.name, true,
                        mpm, subject, properties);
                    eventParamProp.Value = eventParam.Value;
                    eventParamProp.InnerRead = true;

                    EventParamTypeProperty typeProp = new EventParamTypeProperty(mpm, subject, properties);
                    typeProp.Value = Util.FromMetreosToMaxEventParamType(eventParam.type);
                    typeProp.InnerRead = true;

                    eventParamProp.ChildrenProperties.Add(typeProp);

                    properties.Add(eventParamProp);
                }
            }

            foreach(MaxProperty property in properties)
                if(property is HiddenGenericProperty)
                    if(property.Name == DataTypes.EVENT_TYPE)
                    {
                        property.Value = eventXml.eventType.ToString();
                        break;
                    }
        }
        #endregion
    }
}

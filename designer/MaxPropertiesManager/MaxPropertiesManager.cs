using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Specialized;
using PropertyGrid.Core;
using Microsoft.Win32;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;
using Metreos.WebServicesConsumerCore;



namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    ///           Provides the conduit with which to communicate between MaxMain and 
    ///           Property Grid Formatting and related classes.
    ///           
    ///           Handles the creation of properties for the various Visual Designer entities
    /// </summary>
    public class MaxPropertiesManager : IMpmDelegates
    {
        public delegate object[] GetFunctionVars(object subject);
        public delegate object[] GetGlobalVars();
        public delegate string[] GetAllNativeTypes();
        public delegate string[] GetUsingStatements();
        public delegate string[] GetMediaFilesDelegate();
        public delegate string[] GetGrammarFilesDelegate();
        public delegate string[] GetVoiceRecFilesDelegate();
        public delegate string[] GetValidInitWithValues(out string[] events);
        public delegate string[] GetInstallerConfigParameters();
        public delegate string[] GetLocalizableStrings();
        public delegate void RemovePropertyFromGrid(PropertyDescriptor parentProperty, MaxProperty propertyToRemove);
        public delegate void PropertiesChangedDelegate(MaxProperty[] MaxProperty);
        public delegate void IndividualValueChangedDelegate(bool alertFramework);
        public delegate void UpdateUsingStatements(string[] usings);
        public delegate void VoidDelegate();
        public delegate void GetChangedProperties(MaxProperty[] changedProperties);
        public delegate EventParameter GetEventParameter(string fullyQualifiedEventName, string eventParameterDisplayName);
        public delegate void IterateThroughProperties(SimplePropertyType[] properties, 
            object selectableObject, MaxProperty parent, PropertyDescriptorCollection propertiesCollection);
        public delegate nativeTypePackageType[] GetNativeTypesInfo();
        public delegate MaxPmAction GetPackagedAction(string packageName, string actionName);
        public delegate DataTypes.UserVariableType GetDefaultUserType();

        public static bool Deserializing { get { return deserializing; } set { deserializing = value; } }

        public MaxPropertyGrid                PropertyGrid { get { return propertyGrid; } }
        public GetChangedProperties           GetChangedPropertiesDelegate { get { return getChangedProperties; } set { getChangedProperties = value; } }
        public GetFunctionVars                GetFunctionVarsDelegate { get { return getFunctionVars; } set { getFunctionVars = value; } }
        public GetGlobalVars                  GetGlobalVarsDelegate { get { return getGlobalVars; } set { getGlobalVars = value; } }
        public IndividualValueChangedDelegate ValueChanged { get { return this.valueChanged; } set { this.valueChanged = value; } }
        public GetAllNativeTypes              GetAllNativeTypesDelegate { get { return getAllNativeTypes; } set { getAllNativeTypes = value; } }
        public GetNativeTypesInfo             GetNativeTypesInfoDelegate { get { return getNativeTypesInfo; } set { getNativeTypesInfo = value; } }
        public GetValidInitWithValues         GetInitWithValues { get { return getInitWithValues; } set { getInitWithValues = value; } }
        public GetInstallerConfigParameters   GetConfigParameters { get { return getConfigParameters; } set { getConfigParameters = value; } }
        public GetLocalizableStrings          GetLocaleStrings { get { return getLocaleStrings; } set { getLocaleStrings = value; } }
        public GetUsingStatements             GetUsings { get { return getUsingStatements; } set { getUsingStatements = value; } }
        public GetPackagedAction              GetAction { get { return getPackagedAction; } set { getPackagedAction = value; } }
        public UpdateUsingStatements          UpdateUsings { get { return updateUsingStatements; } set { updateUsingStatements = value; } }
        public GetMediaFilesDelegate          GetMediaFiles { get { return getMediaFiles; } set { getMediaFiles = value; } }
        public RemovePropertyFromGrid         RemovePropertyDelegate { get { return removeProperty; } set { removeProperty = value; } } 
        public GetGrammarFilesDelegate        GetGrammarFiles  { get { return getGrammarFiles; } set { getGrammarFiles  = value; } }
        public GetVoiceRecFilesDelegate       GetVoiceRecFiles { get { return getVoiceRecFiles;} set { getVoiceRecFiles = value; } }
        public GetDefaultUserType             GetDefaultUserTypeDelegate { get { return getDefaultUserType; } set { getDefaultUserType = value; } }
        public VoidDelegate                   FocusPropertyGrid { get { return focusPropertyGrid; } set { focusPropertyGrid = value; } }
        public GetEventParameter              GetEventParameterByDisplayName { get { return getEventParamByDisplay; } set { getEventParamByDisplay = value; } }
        public GetEventParameter              GetEventParameterByName { get { return getEventParamByName; } set { getEventParamByName = value; } }

        private static MaxProxyObject proxyMaxObject = new MaxProxyObject();
        private static bool deserializing            = false;
        private bool axlNotLoaded                    = true;
        private IndividualValueChangedDelegate         valueChanged;     
        private MaxPropertyGrid                        propertyGrid;
        private GetChangedProperties                   getChangedProperties;
        private GetFunctionVars                        getFunctionVars;
        private GetGlobalVars                          getGlobalVars;
        private GetAllNativeTypes                      getAllNativeTypes;
        private GetNativeTypesInfo                     getNativeTypesInfo;
        private GetValidInitWithValues                 getInitWithValues;
        private GetInstallerConfigParameters           getConfigParameters;
        private GetLocalizableStrings                  getLocaleStrings;
        private GetUsingStatements                     getUsingStatements;
        private GetPackagedAction                      getPackagedAction;
        private GetMediaFilesDelegate                  getMediaFiles;
        private GetGrammarFilesDelegate                getGrammarFiles;
        private GetVoiceRecFilesDelegate               getVoiceRecFiles;
        private UpdateUsingStatements                  updateUsingStatements;
        private GetDefaultUserType                     getDefaultUserType;
        private PropertySerializer                     propertySerializer;
        private RemovePropertyFromGrid                 removeProperty;
        private VoidDelegate                           focusPropertyGrid;
        private GetEventParameter                      getEventParamByDisplay;
        private GetEventParameter                      getEventParamByName;

        // Enumerates all actions which qualify as a action which requires growable result params
        private string[] growableResultParamActions = new string[] { IActions.CallFunction, Utilities.Namespace.GetName(IActions.CallFunction) };

        /// <summary>
        ///           Provides the conduit with which to communicate 
        ///           between MAXMAN and Property Grid Formatting and related classes.
        /// </summary>
        public MaxPropertiesManager
            (MaxPropertyGrid propertyGrid, IMaxPropManCallbacks callbacks)    
        {	
            this.propertyGrid               = propertyGrid;
            callbacks.ForcePropertyUpdate  += new VoidDelegate(ForcePropertyUpdate);
            this.getChangedProperties       = callbacks.GetChangedProperties;
            this.getFunctionVars            = callbacks.GetFunctionVars;
            this.getGlobalVars              = callbacks.GetGlobalVars;    
            this.getAllNativeTypes          = callbacks.GetAllNativeTypes;
            this.getNativeTypesInfo         = callbacks.GetNativeTypesInfo;
            this.getInitWithValues          = callbacks.GetValidInitWithValues;
            this.getConfigParameters        = callbacks.GetConfigParameters;
            this.getLocaleStrings           = callbacks.GetLocaleStrings;       
            this.getUsingStatements         = callbacks.GetUsings;
            this.getPackagedAction          = callbacks.GetAction;
            this.getMediaFiles              = callbacks.GetMediaFiles;
            this.updateUsingStatements      = callbacks.UpdateUsings;
            this.getDefaultUserType         = callbacks.GetDefaultUserType;
            this.getEventParamByDisplay     = callbacks.GetEventParamByDisplay;
            this.getEventParamByName        = callbacks.GetEventParamByName;
            this.valueChanged               = new MaxPropertiesManager.IndividualValueChangedDelegate(RefreshPropertyGrid);
            this.propertySerializer         = new PropertySerializer(this);
            this.removeProperty             = new RemovePropertyFromGrid(RemoveProperty);
            this.focusPropertyGrid          = new VoidDelegate(FocusGrid);

            this.propertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(propertyGrid_SelectedGridItemChanged);
            this.propertyGrid.PropertyValueChanged    +=new PropertyValueChangedEventHandler(GetChangedPropertiesEvent);

            LoadImages();
        }
                             
        public PropertyDescriptorCollection ConstructProperties(CreatePropertiesArgs args)
        {            
            switch(args.type)
            {
                case DataTypes.Type.Link:               return this.LinkInstance(args);                   
                case DataTypes.Type.ActionInstance:     return this.ActionInstance(args);
                case DataTypes.Type.EventInstance:      return this.EventInstance(args);
                case DataTypes.Type.Function:           return this.FunctionInstance(args);
                case DataTypes.Type.Action:             return this.Action(args);
                case DataTypes.Type.Event:              return this.Event(args);
                case DataTypes.Type.ExitAction:         return this.ExitAction(args);
                case DataTypes.Type.StartNode:          return this.StartNode(args);  
                case DataTypes.Type.Label:              return this.Label(args);
                case DataTypes.Type.Comment:            return this.Comment(args);
                case DataTypes.Type.Loop:               return this.Loop(args);
                case DataTypes.Type.LocalVariable:      return this.LocalVariable(args);
                case DataTypes.Type.GlobalVariable:     return this.GlobalVariable(args);
                case DataTypes.Type.Script:             return this.Script(args);
                case DataTypes.Type.Project:            return this.Project(args);
                case DataTypes.Type.Code:               return this.Code(args);
            }
            return null;
        }

        public void SerializeProperty(DataTypes.Type type, 
            PropertyDescriptorCollection maxProperties, XmlTextWriter writer)
        {
            propertySerializer.SerializeSave(type, maxProperties, writer);
        }

        public void DeserializeProperty(object selectableObject, 
            DataTypes.Type type, PropertyDescriptorCollection properties, XmlNode node, bool packageExists)
        {
            propertySerializer.Deserialize(selectableObject, type, properties, node, packageExists);
        }

        /// <summary>
        ///           Called by the delegate passed in my MAXMAN indicating that a new element 
        ///           in MAX has been selected.
        /// </summary>      
        /// <param name="maxproperties"> The PropertyDescriptorCollection to populate</param>
        /// <param name="nodeType"> The type of element for which the PropDescCollection represents </param>
        public void Show(PropertyDescriptorCollection maxproperties, DataTypes.Type nodeType)
        {
            proxyMaxObject.Reset(nodeType);
            if  (maxproperties == null) return;

            // Resets all values to a new state, so it is possible to see which values have changed  
            // whenever PropertiesChanged event is fired
            for(int i = 0; i < maxproperties.Count; i++)
                ResetValues( (MaxProperty) maxproperties[i]);

            proxyMaxObject.CustomProperties  = maxproperties;

            this.propertyGrid.SetObject(proxyMaxObject);
        }

        #region Property Change Handling
        /// <summary>
        /// Retrieves which properties have changed
        /// </summary>
        private void GetChangedPropertiesEvent(object o, System.Windows.Forms.PropertyValueChangedEventArgs args)
        {
            RetrieveChangedProperties();
        }

        private void RetrieveChangedProperties()
        {
            MaxProperty[] changedProperties = CollectChangedProperties(proxyMaxObject.CustomProperties);

            // Indicates that none of the closely monitored properties has changed.
            // So, let's check to see if any of the properties have changed
            if(changedProperties == null)
            {
                if(IsAnyPropertyChanged(proxyMaxObject.CustomProperties))
                {
                    getChangedProperties(null);
                }

                else return;
            }

            else getChangedProperties(changedProperties);
        }

        private bool IsAnyPropertyChanged(PropertyDescriptorCollection properties)
        {
            foreach(MaxProperty property in properties)
            {
                if(property.IsChanged == true)
                    return true;

                if(IsAnyPropertyChanged(property.ChildrenProperties))
                    return true;
            }

            return false;
        }

        private MaxProperty[] CollectChangedProperties(PropertyDescriptorCollection properties)
        {
            ArrayList changedPropertiesBuilder = new ArrayList();
            MaxProperty[] changedProperties;
            MaxProperty property;

            switch(proxyMaxObject.NodeType)
            {
                case DataTypes.Type.ActionInstance:
                    property = (MaxProperty) properties[DataTypes.ACTION_NAME];
						
                    if(property == null)
                        return null;

                    if(property.Value.ToString() == Utilities.Namespace.GetName(IActions.CallFunction))
                    {
                        MaxProperty property2 = (MaxProperty) properties[IActions.Fields.FunctionName];

                        if(property2 != null)
                            if(property2.IsChanged)
                                changedPropertiesBuilder.Add(property2);
                    }
			
                    break;

                case DataTypes.Type.Function:
                    break;

                case DataTypes.Type.Label:
                    property = (MaxProperty) properties[DataTypes.LABEL_NAME];

                    if(property.IsChanged)
                        changedPropertiesBuilder.Add(property);
                    break;

                case DataTypes.Type.Comment:
                    break;

                case DataTypes.Type.Loop:
                    // If count or looptype is found as changed
                    // then this goes ahead and sends the other along
                    property = properties[DataTypes.COUNT] as MaxProperty;
                    MaxProperty property3 = property.ChildrenProperties[DataTypes.LOOP_TYPE_NAME] as MaxProperty;
                    if(property.IsChanged || property3.IsChanged)
                        changedPropertiesBuilder.Add(property);
                    break;

                case DataTypes.Type.LocalVariable:
                    property = (MaxProperty) properties[DataTypes.VARIABLE];
          
                    if(property.IsChanged)
                        changedPropertiesBuilder.Add(property);
                    break;

                case DataTypes.Type.GlobalVariable:
                    property = (MaxProperty) properties[DataTypes.VARIABLE];

                    if(property.IsChanged)
                        changedPropertiesBuilder.Add(property);
                    break;
            }

            if(changedPropertiesBuilder.Count != 0)
            {
                changedProperties = new MaxProperty[changedPropertiesBuilder.Count];
                changedPropertiesBuilder.CopyTo(changedProperties);
                return changedProperties;
            }

            else return null;
        }

        #endregion

        #region Construct Property Logic

        private ComplexTypeProperty[] ArrangeGroups(ActionParameter[] parameters, object subject, PropertyDescriptorCollection container)
        {
            if(parameters == null || parameters.Length == 0)  return null;

            ArrayList parents = new ArrayList();
            foreach(ActionParameter parameter in parameters)
            {
                int index = parameter.Name.IndexOf(NativeActionAssembler.heirarchySeperator);
                if(index < 0) continue;

                string parentName = parameter.Name.Substring(0, index);

                if(!parents.Contains(parentName))
                    parents.Add(parentName);
            }

            if(parents.Count == 0)  return null;

            ArrayList properties = new ArrayList();
            foreach(string parent in parents)
                properties.Add(new ComplexTypeProperty(parent, this, subject, container));

            return properties.ToArray(typeof(ComplexTypeProperty)) as ComplexTypeProperty[];
        }

        public static string OverrideIfWebServicesName(string name)
        {
            if(name == null || name == String.Empty)  return name;

            int index = name.IndexOf(NativeActionAssembler.heirarchySeperator);
            if(index < 0) return name;

            string overrideName = name.Substring(index + NativeActionAssembler.heirarchySeperator.Length);
            return overrideName;
        }



        private bool FindReplicableParams(ActionParameter[] parameters)
        {
            if(parameters == null || parameters.Length == 0)    return false;
            
            foreach(ActionParameter parameter in parameters)
            {
                if(parameter.AllowMultiple) return true;
            }

            return false;
        }



        private PropertyDescriptorCollection ActionInstance(CreatePropertiesArgs args)
        {
            DataTypes.UserVariableType defaultType = this.getDefaultUserType();

            MaxPmAction action = args.value_ as MaxPmAction;
            if  (action == null) return new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            ComplexTypeProperty[] webServiceHolders = ArrangeGroups(action.Parameters, args.subject, newProperties);

            if(webServiceHolders != null)
                foreach(ComplexTypeProperty complex in webServiceHolders)
                    newProperties.Add(complex);

            int paramCount = action.Parameters == null? 0: action.Parameters.Length;
            bool replicableParamsFound = FindReplicableParams(action.Parameters);

            if(action.AllowCustomParameters || replicableParamsFound)
            {
                ActionParameterPropertyGrowable growableProperty  = new ActionParameterPropertyGrowable
                    (DataTypes.CUSTOM_ACTION_PARAMETERS, action.AllowCustomParameters, 
                    replicableParamsFound, DataTypes.ACTION_PARAMETERS_CATEGORY,
                    false, this, args.subject, newProperties);
               
                newProperties.Add(growableProperty);
            }     
            
           
            for(int i = 0; i < paramCount; i++)
            {
                ActionParameter param = action.Parameters[i];
                string name         = param.Name;
                string description  = param.Description != null? param.Description: name;
                string displayName  = param.DisplayName != null? param.DisplayName: name;
                bool   required     = Use.required == param.Use;
                bool   allowMultiple= param.AllowMultiple; 
                string type         = param.Type != null? param.Type: Defaults.TYPE;
                string value_       = String.Empty;
                string[] enumValues = param.EnumValues;

                // Override name if is a special case for Web Services
                name = OverrideIfWebServicesName(param.Name);

                // Create MediaChooserProperty instead of regular action property if this is a property
                // of name file[num] and the action name == Metreos.Providers.MediaServer.PlayAnnouncement
                ActionParameterProperty actionProperty;
                if(Util.IsMediaFileName(name) && Util.IsPlayAnnAction(action))
                {
                    actionProperty = new MediaFileChooserProperty
                        (MediaFileChooserProperty.MediaFileType.audio,
                         name, displayName, value_, required, allowMultiple, 
                         description, this, args.subject, newProperties
                        );
                }
                else
                {
                    // append subProperties to main property
                    actionProperty = new ActionParameterProperty
                        ( name, displayName, value_, required, allowMultiple, 
                          description, enumValues, this, args.subject, newProperties
                        );
                }

                bool isCallFunctionSpecialCase = Util.IsCallFunctionFuncName(action, name);

                UserTypeProperty.AllowableTypes allow = isCallFunctionSpecialCase ? 
                    UserTypeProperty.AllowableTypes.literal : 
                    UserTypeProperty.AllowableTypes.literal | UserTypeProperty.AllowableTypes.variable |
                    UserTypeProperty.AllowableTypes.csharp;

                DataTypes.UserVariableType userTypeToUse = isCallFunctionSpecialCase ?
                    DataTypes.UserVariableType.literal : defaultType;

                UserTypeProperty userTypeProperty = new UserTypeProperty(allow, userTypeToUse, this, args.subject, newProperties);

                MaxProperty typeVar;
                Type realType = Type.GetType(type, false);
                if(realType == null)
                {
                    realType = LookForTypeInAllAssemblies(type, SwitchToTypePackage(action.PackageName));
                }
        
                // Last ditch effort
                if(realType == null)
                {
                    realType = LookForTypeInAllAssemblies(type);
                }

                if(realType == null)
                    typeVar = new GenericPropertyReadOnly
                        ( DataTypes.VARIABLE_TYPES, type, this, args.subject, newProperties);      
                else
                    typeVar = new ReflectorProperty(
                        DataTypes.VARIABLE_TYPES, realType, null, this, args.subject, newProperties);

                actionProperty.ChildrenProperties = new PropertyDescriptorCollection
                    (new PropertyDescriptor[] { userTypeProperty, typeVar } );
                    
                ComplexTypeProperty parent = ComplexTypeProperty.FindComplexParent(param.Name, webServiceHolders);

                if(parent != null)
                    parent.ChildrenProperties.Add(actionProperty);
                else
                    newProperties.Add(actionProperty);  
            }


            // HiddenGenericProperty ie = new HiddenGenericProperty(); // 20060907 *****
            // newProperties.Add(ie);


            if (action.AsyncCallbacks != null)   // Is this an async action?
            {               
                // Create UserData (Tag) property
                ActionParameterProperty actionProperty 
                    = this.MakeUserDataProperty(args, newProperties);  
                  
                if (actionProperty != null)
                    newProperties.Add(actionProperty);

                // Create hidden handlerID property which will map an async action to its event handler
                ActionParameterProperty handlerIdProperty 
                    = this.MakeEventHandlerIdProperty(args, newProperties);

                if (handlerIdProperty != null)
                    newProperties.Add(handlerIdProperty);
            }
            

            int resultDataCount = action.ResultData == null? 0: action.ResultData.Length;
   
            for(int i = 0; i < resultDataCount; i++)
            {
                ResultDatum resultData = action.ResultData[i];
                string type = resultData.Type != null? resultData.Type.ToString(): Defaults.TYPE;
                string name = resultData.Name != null? resultData.Name: Defaults.DEFAULT;
                string displayName = resultData.DisplayName != null? resultData.DisplayName : name;
                string description = resultData.Description != null? resultData.Description: name;
                string value_ = String.Empty;                                                        

                // Create top level data
                MaxProperty typeVar;
                Type realType = Type.GetType(type, false);
                if(realType == null)
                {
                    realType = LookForTypeInAllAssemblies(type, SwitchToTypePackage(action.PackageName));
                }

                if(realType == null)
                    typeVar = new GenericPropertyReadOnly
                        ( DataTypes.VARIABLE_TYPES, type, this, args.subject, newProperties);      
                else
                    typeVar = new ReflectorProperty(
                        DataTypes.VARIABLE_TYPES, realType, null, this, args.subject, newProperties);

                ResultDataProperty resultDataProperty 
                    = new ResultDataProperty(name, displayName, value_, description,
                    this, args.subject, newProperties);

                // Append child properties to ResultDataVariable
                resultDataProperty.ChildrenProperties 
                    = new PropertyDescriptorCollection( new PropertyDescriptor[] { typeVar } );
                        
                newProperties.Add(resultDataProperty);
            }         

            LoggingProperty loggingProperty = ConstructLogging(action.Final, newProperties, args);

            newProperties.Add(loggingProperty);      
		
            HiddenGenericProperty actionNameProperty = new HiddenGenericProperty
                ( DataTypes.ACTION_NAME, action.Name, true, this, args.subject, newProperties);   

            if(action.Description != null)
                actionNameProperty.SetDescription = action.Description;

            newProperties.Add(actionNameProperty);             
            
            HiddenGenericProperty typeProperty  = new HiddenGenericProperty(DataTypes.ACTION_TYPE, action.Type.ToString(), true, this, args.subject, newProperties);
            HiddenGenericProperty finalProperty = new HiddenGenericProperty(DataTypes.FINAL, action.Final.ToString(), true, this, args.subject, newProperties);

            if(HasGrowableResultParams(action.Name))
            {
                ResultDataPropertyGrowable resultPropGrowable = new ResultDataPropertyGrowable(this, args.subject, newProperties);
                newProperties.Add(resultPropGrowable);
            }

            newProperties.Add(typeProperty);
            newProperties.Add(finalProperty);

            return newProperties;
        }


        /// <summary>Creates the UserData action property</summary> 
        private ActionParameterProperty MakeUserDataProperty(CreatePropertiesArgs args, PropertyDescriptorCollection props)
        {
            // We put 'none' on a dropped action, but if this is a deserialized action
            // we don't put any value in by default, because upon opening, if user hits build
            // with no 'dirty' change, then the 'none' will not propagate to saved .app file.
            // A build-time check will catch the missing value, if this is the case though

            ActionParameterProperty actionProperty = new ActionParameterProperty
             (  ICommands.Fields.USER_DATA,  // name
                ICommands.Fields.USER_DATA,  // displayName
                deserializing ? String.Empty : Defaults.USER_DATA_DEFAULT, // value
                false,          // isRequired
                false,          // allowMultiple
                Defaults.USER_DATA_ACTION_PROPERTY_DESC, 
                null,           // enumValues
                this,           // IMpmDelegates
                args.subject,   // subject
                props);         // PropertyDescriptorCollection container

            UserTypeProperty userTypeProperty = new UserTypeProperty(
                DataTypes.UserVariableType.literal, this, args.subject, props);

            GenericProperty typeVar = new GenericProperty
                ( DataTypes.VARIABLE_TYPES, Defaults.TYPE, true, this, args.subject, props);

            actionProperty.ChildrenProperties = new PropertyDescriptorCollection
                (new PropertyDescriptor[] { userTypeProperty, typeVar } );

            return actionProperty;
        }


        /// <summary>Creates the hidden HandlerId action property 20060907</summary>        
        private ActionParameterProperty MakeEventHandlerIdProperty(CreatePropertiesArgs args, PropertyDescriptorCollection props)
        {        
            ActionParameterProperty actionProperty = new ActionParameterProperty
             (  ICommands.Fields.HANDLER_ID,  // name
                ICommands.Fields.HANDLER_ID,  // displayName
                deserializing ? String.Empty : Defaults.HANDLER_ID_DEFAULT, // value
                false,          // isRequired
                false,          // allowMultiple
                Defaults.HANDLER_ID_PROPERTY_DESC, // description
                null,           // enumValues
                this,           // IMpmDelegates
                args.subject,   // subject
                props);         // PropertyDescriptorCollection container

            UserTypeProperty userTypeProperty = new UserTypeProperty(
                DataTypes.UserVariableType.literal, this, args.subject, props);

            GenericProperty typeVar = new GenericProperty
                ( DataTypes.VARIABLE_TYPES, Defaults.TYPE, true, this, args.subject, props);

            actionProperty.ChildrenProperties = new PropertyDescriptorCollection
                (new PropertyDescriptor[] { userTypeProperty, typeVar } );

            actionProperty.IsHiddenProperty = true;   // Don't show in property grid

            return actionProperty;
        }



        private LoggingProperty ConstructLogging(bool final, PropertyDescriptorCollection container, CreatePropertiesArgs args)
        {
            DataTypes.UserVariableType defaultUserType = this.getDefaultUserType();

            // Logging Construction
            DataTypes.OnOff overallLogging = Defaults.OVERALL_LOGGING;
            System.Diagnostics.TraceLevel traceLevel = Defaults.TRACE_LEVEL;

            LoggingProperty loggingProperty = new LoggingProperty(overallLogging, this, args.subject, container);
              
            // construct all event logging properties 
            string loggingString = String.Empty;

            DataTypes.OnOff defaultLogging = Defaults.INDIVIDUAL_LOGGING;

            OnLogEventProperty logOnEntry = new OnLogEventProperty(DataTypes.LOG_ON_ENTRY,  
                loggingString, this, args.subject, container);
            logOnEntry.SetDescription = Defaults.LOG_ON_ENTRY_DESCRIPTION;
      
            TraceLevelProperty traceEntry   = new TraceLevelProperty(traceLevel, this, args.subject, container);
            OnOffProperty entryOnOff   = new OnOffProperty(defaultLogging, this, args.subject, container);
            UserTypeProperty entryType = new UserTypeProperty(defaultUserType, this, args.subject, container);
            logOnEntry.ChildrenProperties.Add(traceEntry);
            logOnEntry.ChildrenProperties.Add(entryOnOff);
            logOnEntry.ChildrenProperties.Add(entryType);
            loggingProperty.ChildrenProperties.Add(logOnEntry);

            if(!final)
            {
                OnLogEventProperty logOnExit = new OnLogEventProperty(DataTypes.LOG_ON_EXIT, 
                    loggingString, this, args.subject, container);
                logOnExit.SetDescription = Defaults.LOG_ON_EXIT_DESCRIPTION;

                OnLogEventProperty logOnSuccess = new OnLogEventProperty(DataTypes.LOG_ON_SUCCESS, 
                    loggingString, this, args.subject, container);
                logOnSuccess.SetDescription = Defaults.LOG_ON_SUCCESS_DESCRIPTION;

                OnLogEventProperty logOnFailure = new OnLogEventProperty(DataTypes.LOG_ON_FAILURE, 
                    loggingString, this, args.subject, container);
                logOnFailure.SetDescription = Defaults.LOG_ON_FAILURE_DESCRIPTION;

                OnLogEventProperty logOnTimeout = new OnLogEventProperty(DataTypes.LOG_ON_TIMEOUT, 
                    loggingString, this, args.subject, container);
                logOnTimeout.SetDescription = Defaults.LOG_ON_TIMEOUT_DESCRIPTION;
            
                OnLogEventProperty logOnDefault = new OnLogEventProperty(DataTypes.LOG_ON_DEFAULT, 
                    loggingString, this, args.subject, container);
                logOnDefault.SetDescription = Defaults.LOG_ON_DEFAULT_DESCRIPTION;

    
                TraceLevelProperty traceExit    = new TraceLevelProperty(traceLevel, this, args.subject, container);
                TraceLevelProperty traceSuccess = new TraceLevelProperty(traceLevel, this, args.subject, container);
                TraceLevelProperty traceFailure = new TraceLevelProperty(traceLevel, this, args.subject, container);
                TraceLevelProperty traceTimeout = new TraceLevelProperty(traceLevel, this, args.subject, container);
                TraceLevelProperty traceDefault = new TraceLevelProperty(traceLevel, this, args.subject, container);

                OnOffProperty exitOnOff    = new OnOffProperty(defaultLogging, this, args.subject, container);
                OnOffProperty successOnOff = new OnOffProperty(defaultLogging, this, args.subject, container);
                OnOffProperty failureOnOff = new OnOffProperty(defaultLogging, this, args.subject, container);
                OnOffProperty timeoutOnOff = new OnOffProperty(defaultLogging, this, args.subject, container);
                OnOffProperty defaultOnOff = new OnOffProperty(defaultLogging, this, args.subject, container);

                UserTypeProperty exitType = new UserTypeProperty(defaultUserType, this, args.subject, container);
                UserTypeProperty successType = new UserTypeProperty(defaultUserType, this, args.subject, container);
                UserTypeProperty failureType = new UserTypeProperty(defaultUserType, this, args.subject, container);
                UserTypeProperty timeoutType = new UserTypeProperty(defaultUserType, this, args.subject, container);
                UserTypeProperty defaultType = new UserTypeProperty(defaultUserType, this, args.subject, container);
      
                logOnExit.ChildrenProperties.Add(traceExit);
                logOnSuccess.ChildrenProperties.Add(traceSuccess);
                logOnFailure.ChildrenProperties.Add(traceFailure);
                logOnTimeout.ChildrenProperties.Add(traceTimeout);
                logOnDefault.ChildrenProperties.Add(traceDefault);

                logOnExit.ChildrenProperties.Add(exitOnOff);
                logOnSuccess.ChildrenProperties.Add(successOnOff);
                logOnFailure.ChildrenProperties.Add(failureOnOff);
                logOnTimeout.ChildrenProperties.Add(timeoutOnOff);
                logOnDefault.ChildrenProperties.Add(defaultOnOff);

                logOnExit.ChildrenProperties.Add(exitType);
                logOnSuccess.ChildrenProperties.Add(successType);
                logOnFailure.ChildrenProperties.Add(failureType);
                logOnTimeout.ChildrenProperties.Add(timeoutType);
                logOnDefault.ChildrenProperties.Add(defaultType);

                loggingProperty.ChildrenProperties.Add(logOnExit);
                loggingProperty.ChildrenProperties.Add(logOnSuccess);
                loggingProperty.ChildrenProperties.Add(logOnFailure);
                loggingProperty.ChildrenProperties.Add(logOnTimeout);
                loggingProperty.ChildrenProperties.Add(logOnDefault);
      
                OnLogEventPropertyGrowable growableLogging 
                    = new OnLogEventPropertyGrowable(this, args.subject, container);

                loggingProperty.ChildrenProperties.Add(growableLogging);
            }

            return loggingProperty;
        }

        private PropertyDescriptorCollection EventInstance(CreatePropertiesArgs args)
        {
            MaxPmEvent event_ = args.value_ as MaxPmEvent;
            if  (event_ == null) return new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            int paramCount = event_.Parameters == null? 0: event_.Parameters.Length;
                      
            for(int i = 0; i < paramCount; i++)
            {
                EventParameter param = event_.Parameters[i];
                string type = param.Type != null? param.Type: Defaults.TYPE;
                string name = param.Name != null? param.Name: Defaults.DEFAULT;
                string displayName = param.DisplayName != null? param.DisplayName: name;
                                                
                EventParamTypeProperty typeProperty = new EventParamTypeProperty(this, args.subject, newProperties);

                EventParameterProperty eventProperty = new EventParameterProperty
                    ( name, displayName, param.Guaranteed, param.Description, this, args.subject, newProperties);
        
                eventProperty.ChildrenProperties.Add(typeProperty);

                newProperties.Add(eventProperty);
            }

            if(event_.Type == EventType.asyncCallback)
            {
                EventParamTypeProperty typeProperty = new EventParamTypeProperty(this, args.subject, newProperties);

                EventParameterProperty eventProperty = new EventParameterProperty
                    ( ICommands.Fields.USER_DATA, ICommands.Fields.USER_DATA, true, this, args.subject, newProperties);
         
                eventProperty.SetDescription = Defaults.USER_DATA_DESC;

                eventProperty.ChildrenProperties.Add(typeProperty);
                newProperties.Add(eventProperty);
            }

            // Save the type of event in an hidden property, for the sake of the build.
            HiddenGenericProperty eventType = new HiddenGenericProperty(DataTypes.EVENT_TYPE, event_.Type.ToString(), true,
                this, args.subject, newProperties);

            newProperties.Add(eventType);

            return newProperties;
        }


        private PropertyDescriptorCollection LinkInstance(CreatePropertiesArgs args)
        {

            PropertyDescriptorCollection newProperties 
                = new PropertyDescriptorCollection(null);

            DataTypes.LinkStyle initialStyleData = Defaults.LINK_STYLE;

            StyleProperty styleProperty = new StyleProperty( initialStyleData, this, args.subject, newProperties);
            
            newProperties.Add(styleProperty);

            return newProperties;
        }


        private PropertyDescriptorCollection FunctionInstance(CreatePropertiesArgs args)
        {
            return new PropertyDescriptorCollection(null);
            #if(false) 
            //      PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);
            //
            //      string functionName = String.Empty;
            //           
            //      GenericProperty functionNameProperty  = new GenericProperty
            //        ( DataTypes.FUNCTION_NAME, functionName, false, this.valueChanged, args.subject);
            //
            //      newProperties.Add(functionNameProperty);
            //
            //      return newProperties;
            #endif
        }


        private PropertyDescriptorCollection Action(CreatePropertiesArgs args)
        {  
            MaxPmAction action = args.value_ as MaxPmAction;
            if  (action == null) return new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            int paramCount = action.Parameters == null? 0: action.Parameters.Length;
           
            for(int i = 0; i < paramCount; i++)
            {
                ActionParameter param = action.Parameters[i];
                string name     = param.Name;
                string displayName = param.DisplayName != null ? param.DisplayName : name;
                bool   required = Use.required == param.Use;
                string type     = param.Type != null? param.Type: Defaults.TYPE;
                string value_   = String.Empty; 
                                        
                // construct sub properties and sub data
                // Add to the propertyDescriptor collection
                ActionParameterPropertyReadOnly actionPropertyReadOnly 
                    = new ActionParameterPropertyReadOnly( name, displayName, type, required, this, args.subject, newProperties);
                    
                if(param.Description != null)
                    actionPropertyReadOnly.SetDescription = param.Description;
                 
                newProperties.Add(actionPropertyReadOnly);
            }  

            int resultDataCount = action.ResultData == null? 0: action.ResultData.Length;            
          
            for(int i = 0; i < resultDataCount; i++)
            {
                ResultDatum resultData = action.ResultData[i]; 
                string type = resultData.Type != null? resultData.Type: Defaults.TYPE;
                string name = resultData.Name != null? resultData.Name: Defaults.DEFAULT;
                string displayName = resultData.DisplayName != null? resultData.DisplayName: name;
                string value_ = String.Empty;
       
                // Create the property to add to the the custom properties
                ResultDataPropertyReadOnly resultDataPropertyReadOnly 
                    = new ResultDataPropertyReadOnly(name, displayName, type, this, args.subject, newProperties);

                if(resultData.Description != null)
                    resultDataPropertyReadOnly.SetDescription = resultData.Description;
                 
                newProperties.Add(resultDataPropertyReadOnly);
            }
        
            // Package only properties.  These are for ease-of-use from a Max-centric programmatic viewpoint
            GenericPropertyReadOnly actionToolNameProperty = new GenericPropertyReadOnly
                (DataTypes.ACTION_NAME, action.Name, this, args.subject, newProperties);      
            
            if(action.Description != null)
                actionToolNameProperty.SetDescription = action.Description;

            string[] asyncCallbacks = action.AsyncCallbacks;
            
            StringBuilder build = new StringBuilder();

            if(asyncCallbacks != null && asyncCallbacks.Length > 0)
            {
                build.Append(asyncCallbacks[0]);

                for(int i = 1; i < asyncCallbacks.Length; i++)
                {                   
                    build.Append(", " + asyncCallbacks[i]);                   
                }
            }

            GenericPropertyReadOnly final = new GenericPropertyReadOnly
                (DataTypes.FINAL, action.Final.ToString(), this, args.subject, newProperties);
            GenericPropertyReadOnly allowCustomParams = new GenericPropertyReadOnly
                (DataTypes.ALLOW_CUSTOM_PARAMETERS, action.AllowCustomParameters.ToString(), this, args.subject, newProperties);
            
            GenericPropertyReadOnly asyncCallbackProperty = new GenericPropertyReadOnly
                (DataTypes.ASYNC_CALLBACK, build.ToString(), this, args.subject, newProperties);
            GenericPropertyReadOnly description = new GenericPropertyReadOnly
                (DataTypes.DESCRIPTION, action.Description, this, args.subject, newProperties);
            
            newProperties.Add(final);
            newProperties.Add(allowCustomParams);
            newProperties.Add(asyncCallbackProperty);
            newProperties.Add(description);
            newProperties.Add(actionToolNameProperty);              

            return newProperties;
        }


        private PropertyDescriptorCollection Event(CreatePropertiesArgs args)
        {
            MaxPmEvent event_ = args.value_ as MaxPmEvent;
            if  (event_ == null) return new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            GenericPropertyReadOnly eventToolNameProperty = new GenericPropertyReadOnly
                (DataTypes.EVENT_NAME, event_.Name, this, args.subject, newProperties);       

            eventToolNameProperty.category = DataTypes.BASIC_PROPERTIES;

            TriggerProperty triggerProperty = new TriggerProperty(event_.Type, this, args.subject, newProperties);
            
            if(event_.Description != null)
                eventToolNameProperty.SetDescription = event_.Description;

            int paramCount = event_.Parameters == null? 0: event_.Parameters.Length;
            
            for(int i = 0; i < paramCount; i++)
            {
                EventParameter param = event_.Parameters[i];
                string type = param.Type != null? param.Type: Defaults.TYPE;
                string name = param.Name != null? param.Name: Defaults.DEFAULT;
                string displayName = param.DisplayName != null? param.DisplayName: name;

                EventParameterPropertyReadOnly eventProperty 
                    = new EventParameterPropertyReadOnly(name, displayName, type, param.Guaranteed, this, args.subject, newProperties);

                if(param.Description != null)
                    eventProperty.SetDescription = param.Description;

                newProperties.Add(eventProperty);
            }

            newProperties.Add(triggerProperty);
            newProperties.Add(eventToolNameProperty);

            return newProperties;
        }


        private PropertyDescriptorCollection ExitAction(CreatePropertiesArgs args)
        {
            DataTypes.UserVariableType defaultType = this.getDefaultUserType();

            MaxPmAction action = args.value_ as MaxPmAction;
            if  (action == null) return null;
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            ActionParameterPropertyGrowable growableProperty  = new ActionParameterPropertyGrowable
                (DataTypes.RESULT_DATA, true, false, DataTypes.RESULT_DATA_CATEGORY, false, 
                this, args.subject, newProperties);

            growableProperty.category = DataTypes.RESULT_DATA;

            int paramCount = action.Parameters == null? 0: action.Parameters.Length;
        
            for(int i = 0; i < paramCount; i++)
            {
                ActionParameter param = action.Parameters[i];
                bool   required = Use.required == param.Use;
                bool   allowMultiple = param.AllowMultiple;
                string name   = param.Name;
                string type   = type = param.Type != null? param.Type: Defaults.TYPE;
                string value_ = String.Empty;
                  
                if(name == DataTypes.RETURN_VALUE)
                {
                    FunctionReturnValueProperty returnValueProperty = new FunctionReturnValueProperty
                        ( name, value_, this, args.subject, newProperties);

                    returnValueProperty.Category = DataTypes.EXIT_FUNC_RETURN_VALUE_CATEGORY;

                    UserTypeProperty userTypeProperty = new UserTypeProperty(defaultType, this, args.subject, newProperties);

                    GenericProperty typeVar = new GenericProperty
                        ( DataTypes.VARIABLE_TYPES, type, true, this, args.subject, newProperties);

                    returnValueProperty.ChildrenProperties = new PropertyDescriptorCollection
                        (new PropertyDescriptor[] { userTypeProperty, typeVar } );
    
                    newProperties.Add(returnValueProperty);   
                }
                else
                {
                    ActionParameterProperty actionProperty = new ActionParameterProperty
                        ( name, name, value_, required, allowMultiple, name, null, this, args.subject, newProperties);

                    actionProperty.Category = DataTypes.EXIT_FUNC_RETURN_VALUE_CATEGORY;
                    
                    UserTypeProperty userTypeProperty = new UserTypeProperty(defaultType,this, args.subject, newProperties);

                    GenericProperty typeVar = new GenericProperty
                        ( DataTypes.VARIABLE_TYPES, type, true, this, args.subject, newProperties);

                    actionProperty.ChildrenProperties = new PropertyDescriptorCollection
                        (new PropertyDescriptor[] { userTypeProperty, typeVar } );
    
                    growableProperty.ChildrenProperties.Add(actionProperty);   
                }
            }
             

            int resultDataCount = action.ResultData == null? 0: action.ResultData.Length;
       
            for(int i = 0; i < resultDataCount; i++)
            {
                ResultDatum resultData = action.ResultData[i];
                string type = resultData.Type != null? resultData.Type.ToString(): Defaults.TYPE;
                string name = resultData.Name != null? resultData.Name: Defaults.DEFAULT;
                string displayName = resultData.DisplayName != null? resultData.DisplayName : name;
                string value_ = String.Empty;
                
                // Create the propety to add the the custom properties
                ResultDataProperty resultDataProperty = new ResultDataProperty
                    ( name, displayName, value_, this, args.subject, newProperties);

                GenericProperty typeVar = new GenericProperty
                    ( DataTypes.VARIABLE_TYPES, type, true, this, args.subject, newProperties);

                UserTypeProperty userTypeProperty = new UserTypeProperty(defaultType, this, args.subject, newProperties);

                resultDataProperty.ChildrenProperties = new PropertyDescriptorCollection
                    ( new PropertyDescriptor[] { typeVar, userTypeProperty } );

                newProperties.Add(resultDataProperty);
            }
        
              
            GenericProperty actionNameProperty = new GenericProperty
                (DataTypes.ACTION_NAME, action.Name, true, this, args.subject, newProperties);

            newProperties.Add(actionNameProperty);
            newProperties.Add(growableProperty);  

            return newProperties;
        }


        private PropertyDescriptorCollection Comment(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);
            #if(false)
            //      string value_ = String.Empty;		
            //
            //      GenericProperty text = new GenericProperty
            //        (DataTypes.COMMENT_TEXT,  value_, true, valueChanged, args.subject);
            //
            //      newProperties.Add(text);
            #endif
            return newProperties;
        }


        private PropertyDescriptorCollection Loop(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            LoopCountProperty loopProperty = new LoopCountProperty(this, args.subject, newProperties);	
            loopProperty.SetDescription = Defaults.LOOP_COUNT_DESCRIPTION;
            LoopTypeProperty loopTypeProperty = new LoopTypeProperty(this, args.subject, newProperties);
            LoopControllerTypeProperty loopControlTypeProperty = new LoopControllerTypeProperty(this, args.subject, newProperties);

            newProperties.Add(loopProperty);
            loopProperty.ChildrenProperties.Add(loopTypeProperty);
            loopProperty.ChildrenProperties.Add(loopControlTypeProperty);

            return newProperties;
        }
		

        private PropertyDescriptorCollection StartNode(CreatePropertiesArgs args)
        {
            return new PropertyDescriptorCollection(null);
            #if(false)
            //      PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);
            //
            //      string value_ = String.Empty;
            //
            //      GenericProperty startNode = new GenericProperty
            //        (DataTypes.NAME, value_, true, valueChanged, args.subject);
            //
            //      startNode.SetDescription = Defaults.START_NODE_DESCRIPTION;
            //      newProperties.Add(startNode);
            //
            //      return newProperties;
            #endif
        }


        private PropertyDescriptorCollection Label(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            string value_ = String.Empty;

            GenericProperty label = new GenericProperty
                (DataTypes.LABEL_NAME, value_, false, this, args.subject, newProperties);

            label.SetDescription = Defaults.LABEL_DESCRIPTION;

            newProperties.Add(label);
		
            return newProperties;
        }


        /// <summary>Create PropertyDescriptorCollection for a local variable</summary>
        private PropertyDescriptorCollection LocalVariable(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            GenericProperty label = new GenericProperty
                (DataTypes.VARIABLE, String.Empty, false, this, args.subject, newProperties);

            label.SetDescription = Defaults.VARIABLETOOL_DESCRIPTION;

            VariableProperty type = new VariableProperty
                (this, args.subject, newProperties);

            ReferenceProperty refType = new ReferenceProperty(this, args.subject, newProperties);

            LocalVarInitWithProperty initWith = new LocalVarInitWithProperty
                (this, args.subject, newProperties);

            GenericProperty defaultInitWith = new GenericProperty(DataTypes.DEFAULT_INIT_WITH, 
                 String.Empty, false, this, args.subject, newProperties);

            newProperties.Add(label);
            newProperties.Add(type);
            newProperties.Add(refType);
            newProperties.Add(initWith);
            newProperties.Add(defaultInitWith);

            return newProperties;
        }


        /// <summary>Create PropertyDescriptorCollection for a global variable</summary>        
        private PropertyDescriptorCollection GlobalVariable(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            string value_ = String.Empty;

            GenericProperty label = new GenericProperty
                (DataTypes.VARIABLE, value_, false, this, args.subject, newProperties);

            label.SetDescription = Defaults.VARIABLETOOL_DESCRIPTION;

            VariableProperty type = new VariableProperty
                (this, args.subject, newProperties);

            GlobalVarInitWithProperty initWith = new GlobalVarInitWithProperty
                (value_, this, args.subject, newProperties);

            GenericProperty defaultInitWith = new GenericProperty(DataTypes.DEFAULT_INIT_WITH, 
                String.Empty, false, this, args.subject, newProperties);
      
            newProperties.Add(label);
            newProperties.Add(type);
            newProperties.Add(initWith);
            newProperties.Add(defaultInitWith);

            return newProperties;
        }


        private PropertyDescriptorCollection Script(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            GenericProperty scriptDesc = new GenericProperty
                (DataTypes.SCRIPT_DESCRIPTION, String.Empty, false, this, args.subject, newProperties);

            newProperties.Add(scriptDesc);

            return newProperties;
        }


        private PropertyDescriptorCollection Project(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            GenericProperty appDisplayName = new GenericProperty(DataTypes.APP_DISPLAY_NAME_META, String.Empty, false,
                this, args.subject, newProperties);
            appDisplayName.SetDescription = Defaults.APP_DISPLAY_NAME_META_DESC;
            GenericProperty appDescription = new GenericProperty(DataTypes.APP_DESCRIPTION_META, String.Empty, false,
                this, args.subject, newProperties);
            appDescription.SetDescription = Defaults.APP_DESCRIPTION_META_DESC;
            GenericProperty appCompany = new GenericProperty(DataTypes.APP_COMPANY_META, String.Empty, false,
                this, args.subject, newProperties);
            appCompany.SetDescription = Defaults.APP_COMPANY_META_DESC;
            GenericProperty appAuthor = new GenericProperty(DataTypes.APP_AUTHOR_META, String.Empty, false,
                this, args.subject, newProperties);
            appAuthor.SetDescription = Defaults.APP_AUTHOR_META_DESC;
            GenericProperty appCopyright = new GenericProperty(DataTypes.APP_COPYRIGHT_META, String.Empty, false,
                this, args.subject, newProperties);
            appCopyright.SetDescription = Defaults.APP_COPYRIGHT_META_DESC;
            GenericProperty appTrademark = new GenericProperty(DataTypes.APP_TRADEMARK_META, String.Empty, false,
                this, args.subject, newProperties);
            appTrademark.SetDescription = Defaults.APP_TRADEMARK_META_DESC;
            GenericProperty appVersion = new GenericProperty(DataTypes.APP_VERSION_META, String.Empty, false,
                this, args.subject, newProperties);
            appVersion.SetDescription = Defaults.APP_VERSION_META_DESC;

            newProperties.Add(appDisplayName);
            newProperties.Add(appDescription);
            newProperties.Add(appCompany);
            newProperties.Add(appAuthor);
            newProperties.Add(appCopyright);
            newProperties.Add(appTrademark);
            newProperties.Add(appVersion);

            HiddenGenericProperty usingParent = new HiddenGenericProperty(DataTypes.USINGS, String.Empty, false, this, args.subject, newProperties);
      
            foreach(string @using in Metreos.ApplicationFramework.Assembler.Assembler.usings)
            {
                HiddenGenericProperty usingProperty = new HiddenGenericProperty(DataTypes.USING, @using, false, this, args.subject, newProperties);
                usingParent.ChildrenProperties.Add(usingProperty);
            }
     
            newProperties.Add(usingParent);
            return newProperties;
        }


        private PropertyDescriptorCollection Code(CreatePropertiesArgs args)
        {
            PropertyDescriptorCollection newProperties = new PropertyDescriptorCollection(null);

            CodeProperty CodeProperty 
                = new CodeProperty(this, args.subject, newProperties);
            LanguageProperty languageProperty = new LanguageProperty(this, args.subject, newProperties);
            LoggingProperty loggingProperty = ConstructLogging(false, newProperties, args);      
            loggingProperty.Category = DataTypes.BASIC_PROPERTIES;

            newProperties.Add(CodeProperty);
            newProperties.Add(languageProperty);
            newProperties.Add(loggingProperty);

            return newProperties;
        }
    
        #endregion



        #region PropertyGrid Callbacks

        private void RemoveProperty(PropertyDescriptor parentProperty, MaxProperty propertyToRemove)
        {
            GridItem gridItem = this.propertyGrid.SelectedGridItem;
            bool wasExpanded  = gridItem.Expanded;
            gridItem.Expanded = false;

            propertyGrid.Refresh();

            // Its now removeable
            PropertyDescriptorCollection allProps;

            if(parentProperty == null)
            {
                allProps = propertyToRemove.Container;
            }
            else
            {
                allProps = parentProperty.GetChildProperties();
            }

            int propertyIndex = FindProperty(allProps, propertyToRemove);
            if(propertyIndex == -1) return;

            // REFACTOR? OR .NET BUG?  (SETH) 
            // This was implemented because the PropertyDescriptorCollection seems to take 
            // exception to (yuk yuk) the idea of a RemoveAt(0). It is in general quite opposed 
            // to the idea, in fact.  Actually, it's not consistently always 0 that causes the problem,
            // but it *IS* consistently not logical.  
            // Note:  the Property Description collection does some strange things when it becomes completely
            // empty, such as all of a sudden having an internal count of 28, full of ReflectPropertyDescriptor's,
            // and after that, things tend to work. But that's only after the user potentially got hosed.
            // This shim/kludge/hack doesn't make the exception happen, is all I know or care about atm.

            PropertyDescriptor[] kludgeArray = new PropertyDescriptor[allProps.Count];

            allProps.CopyTo(kludgeArray, 0);

            allProps.Clear();

            for(int i = 0; i < kludgeArray.Length; i++)
                if(propertyIndex != i)
                    allProps.Add(kludgeArray[i]);
    
            // REFACTOR: This code does not seem to have any effect. The propGrid item stays collapsed.
            gridItem.Expanded = wasExpanded;
            propertyGrid.Refresh();
        }


        private int FindProperty(PropertyDescriptorCollection properties, MaxProperty property)
        {
            int i = 0;
            foreach(MaxProperty prop in properties)
            {
                if(property == prop) return i;
                i++;
            }
            return -1;
        }


        private void RefreshPropertyGrid(bool alertMaxFramework)
        {
            if(!MaxPropertiesManager.Deserializing)
            {
                this.propertyGrid.Refresh();
            }

            if(alertMaxFramework)
            {
                RetrieveChangedProperties();
            }
        }


        private void FocusGrid()
        {
            if(!MaxPropertiesManager.Deserializing)
            {
                this.propertyGrid.Parent.Focus();
            }
        }


        private void propertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if(e.OldSelection != null)
            {
                MaxProperty oldProperty = e.OldSelection.PropertyDescriptor as MaxProperty;
                if(oldProperty != null) oldProperty.Focused = false;
            }

            if(e.NewSelection != null)
            {
                MaxProperty newProperty = e.NewSelection.PropertyDescriptor as MaxProperty;
                if(newProperty != null) newProperty.Focused = true;
            }
        }


        private void ForcePropertyUpdate()
        {
            propertyGrid.ForcePropertyValueChange();
        }

        #endregion



        #region Utils

        private void ResetValues(MaxProperty property)
        {
            for(int i = 0; i < property.ChildrenProperties.Count; i++)
                ResetValues((MaxProperty) property.ChildrenProperties[i]);

            property.IsDeleted = false;
            property.IsNew = false;
            property.IsChanged = false;
            property.OldValue = property.Value;
        }


        private void LoadImages()
        { 
            System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            PropertiesImageControl.SetImages(thisAssembly);
        }


        private bool HasGrowableResultParams(string actionName)
        {
            foreach(string specialAction in growableResultParamActions) 
                if(actionName == specialAction) 
                    return true;
      
            return false;
        }


        private Type LookForTypeInAllAssemblies(string type, string assemblyName)
        {
            if(type == null || assemblyName == null)    return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if(assemblies == null)  return null;

            foreach(Assembly assembly in assemblies)
            {
                if(assembly.FullName.IndexOf(assemblyName) >= 0)
                {
                    return assembly.GetType(type, false);
                }
            }

            return null; 
        }


        private Type LookForTypeInAllAssemblies(string type)
        {
            if(type == null)    return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if(assemblies == null)  return null;

            foreach(Assembly assembly in assemblies)
            {
                Type foundType =  assembly.GetType(type, false);
                if(foundType != null)    return foundType;
            }

            return null; 
        }


        /// <summary>
        ///   This really needs to be cleaned up.   This method determines assembly names in a
        ///   very hodgepodge manner.  Right now, however, its done this way to add much needed
        ///   ReflectorProperty functionality for as many types as possible
        /// </summary>
        private string SwitchToTypePackage(string actionPackageName)
        {
            if(actionPackageName == null)   return null;
            if(actionPackageName == String.Empty) return null;

            string[] bits = actionPackageName.Split('.');

            if(bits.Length != 3)
            {
                return actionPackageName;
            }
            else if(bits[1] == "NativeActions")
            {
                return String.Format("{0}.NativeTypes.{1}", bits[0], bits[2]);
            }
            else if(bits.Length >= 3 && bits[0] == "Metreos" && bits[1] == "Native" && bits[2].IndexOf("AxlSoap") != -1)
            {
                // Specifically available to support finding AxlSoap types from the Metreos.AxlSoap.dll
                // If this type is found, the assembly will not be loaded yet, making this type unfindable.
                LoadAxlSoap();
                return "Metreos.AxlSoap";
            }
            else
            {
                return String.Format("{0}.Types.{1}", bits[0], bits[2]);
            }
        }


        private void LoadAxlSoap()
        {
            if(axlNotLoaded)
            {
                try
                {
                    // This assembly should tend to load as it is in the GAC
                    Assembly.LoadFrom("Metreos.AxlSoap.dll");
                    axlNotLoaded = false;
                }
                catch { }
            }
        }

        #endregion

    }   // class MaxPropertiesManager
 



    public class MaxPropertyGrid : System.Windows.Forms.PropertyGrid
    {
        public MaxPropertyGrid() : base()
        {
        }

        private PropertyGridMessageFilter current;
        private MouseEventHandler mHandler;
    
        public void ForcePropertyValueChange()
        {
            this.Refresh();
        }


        public void SetObject(object obj)
        {
            // Is tightly bound to focus changing in the Designer
            // Take this opportunity to bind to the inner PropertyGrid
            this.SelectedObject = obj;
            if(!this.Created) return;
            
            // If current is not null, we then re-add the filter in case  
            // the window is auto-destroyed by net, wrecking our old handle
            if(current != null) 
               Application.RemoveMessageFilter(current);

            current  = null; 
            mHandler = null;
 
            try
            {
                Control control = this.GetChildAtPoint(new Point(40, 40));

                if (control != null)
                {
                    mHandler = new MouseEventHandler(MouseEvent);
                    current  = new PropertyGridMessageFilter(control, mHandler);                    
                    Application.AddMessageFilter(current);
                }                        
            }
            catch { }          
        }    


        private void MouseEvent(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Right && 
                this.SelectedGridItem != null && 
                this.SelectedGridItem.PropertyDescriptor is MaxProperty)
            {   // the user right clicked on a property to see the context menu: 
                try
                {
                    MaxProperty property = this.SelectedGridItem.PropertyDescriptor as MaxProperty;
                    property.OnContextMenuRequest(this, PointToClient(MousePosition)); 
                }
                catch
                {
                    // What to do?
                }
            }
        }


        #region PropertyGridMessageFilter

        /// <summary>
        /// Apparently this is the only way to determine if there 
        /// was a mouse up event on a property in the property grid
        /// </summary>
        public class PropertyGridMessageFilter : IMessageFilter
        {
            /// <summary>The control to monitor</summary>
            public Control Control;

            public MouseEventHandler MouseUp;

            public PropertyGridMessageFilter( Control c, MouseEventHandler meh )
            {
                this.Control = c;
                MouseUp = meh;
            }


            #region IMessageFilter Members


            public bool PreFilterMessage(ref Message m)
            {
                if( ! this.Control.IsDisposed && m.HWnd == this.Control.Handle && MouseUp != null)
                {
                    MouseButtons mb = MouseButtons.None;
				
                    switch( m.Msg )
                    {
                       case 0x0202:/*WM_LBUTTONUP, see winuser.h*/
                            mb = MouseButtons.Left;
                            break;
                       case 0x0205:/*WM_RBUTTONUP*/
                            mb = MouseButtons.Right;
                            break;
                    }

                    if( mb != MouseButtons.None )
                    {
                        MouseEventArgs e = new MouseEventArgs( mb, 1, m.LParam.ToInt32() & 0xFFff, m.LParam.ToInt32() >> 16, 0 );

                        // you can visit these pages to understand where the above formulas came from 
                        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/mouseinput/mouseinputreference/mouseinputmessages/wm_lbuttonup.asp
                        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmacros/get_x_lparam.asp
				
                        MouseUp( Control, e );
                    }
                }
                return false;
            }

            #endregion
        }
        #endregion PropertyGridMessageFilter

    }   // class MaxPropertyGrid

}   // namespace

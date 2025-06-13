using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary> 
    ///     All properties used in the Visual Developer Property Grid at least derive from this.
    ///     The base class houses three major concepts: 
    ///     1.) property grid-used methods
    ///     2.) The ValueChanged delegate
    ///     3.) The total delegate interface (IMpmDelegates)
    ///  </summary>
    [TypeConverter(typeof(MaxPropertyConverter))]
    public abstract class MaxProperty : PropertyDescriptor, IMaxProperty
    {
        #region Properties

        public bool InnerRead { set { isReadOnly = value; } }

        #region IMaxProperty Members

        public object Subject { get { return this.subject; } set { this.subject = value; } }

        /// <summary> Invoked by the property whenever the property value changes </summary>
        public MaxPropertiesManager.IndividualValueChangedDelegate ValueChanged
        { get { return this.mpm.ValueChanged; } }

        public override AttributeCollection Attributes
        { get { return new AttributeCollection(this.attributes); } }

        public override string Name
        { get { return name; } }

        public override bool IsReadOnly
        { get { return isReadOnly; } }

        public bool SetReadOnly
        { set { this.isReadOnly = value; } }

        public bool IsRelease
        { get { return this.isRelease; } set { this.isRelease = value; } }

        public override string Description
        { get { return description; } }

        public bool IsValid
        {
            get 
            { 
                if(isRelease == true && value_ .ToString()!= String.Empty) { return true; }
                else { return false; }
            }
        }


        public bool IsChanged
        { get { return this.isChanged; } set { isChanged = value; } }

        public bool IsNew
        { get { return this.isNew; } set { this.isNew = value; } }

        public bool IsDeleted
        { get { return this.isDeleted; } set { this.isDeleted = value; } }
        
        public override string Category
        { get { return this.category; }  }

        public virtual object Value
        { get { return value_; } set { value_ = value; mpm.ValueChanged(false); }}

        public virtual object OldValue
        { get { return oldValue; } set { oldValue = value; } }

        public PropertyDescriptorCollection ChildrenProperties
        { get { return this.childrenProperties; } set { childrenProperties = value; } }

        public PropertyDescriptorCollection Container
        { get { return container; } set { container = value;} }

        /// <summary> All delegates needed by Designer properties are contained in this property </summary>
        public IMpmDelegates Mpm
        { get { return mpm; } }
        #endregion


        public bool Focused { get { return focused; } set { focused = value; } }

        public override string DisplayName
        {
            get
            {
                return this.displayName;
            }
        }

        public string SetName
        {
            set
            {
                this.name = value;
            }
        }

        public string SetDescription
        {
            set
            {
                this.description = value;
            }
        }
        #endregion Properties

        #region Fields

        protected bool isRelease;
        protected bool isReadOnly;
        protected bool isChanged;
        protected bool isDeleted;
        protected bool isNew;
        protected bool focused;

        protected string name;
        protected object value_;
        protected object oldValue;
        protected object subject;
        protected string description;
        public string category;
        protected string groupGuid;
        protected string propertyGridName;
        protected string displayName;
        protected IMpmDelegates mpm;

        protected PropertyDescriptorCollection childrenProperties;
        protected PropertyDescriptorCollection container;
        protected Attribute[] attributes;
  
        #endregion

        #region Constructors

        public MaxProperty(
            string name, 
            object value, 
            bool isReadOnly, 
            IMpmDelegates mpm, 
            object subject,
            PropertyDescriptorCollection container)
            : this(name, name, value, isReadOnly, mpm, subject, container) { }

        public MaxProperty(
            string name,
            string displayName,
            object value,
            bool isReadOnly,
            IMpmDelegates mpm,
            object subject, 
            PropertyDescriptorCollection container)
            : base(name, null)
        {
            this.name = name;
            this.displayName = displayName;
            this.value_ = value;
            this.oldValue = value;
            this.isReadOnly = isReadOnly;
            this.mpm = mpm;
            this.subject = subject;
            this.container = container;
            this.focused = false;

            Default(name);
        }

        public MaxProperty(PropertyDescriptorCollection childrenProperties) : base(String.Empty, null)
        {
            this.childrenProperties = childrenProperties;
        }

        public MaxProperty() : base(String.Empty, null)
        {
        }

        #endregion

        public virtual void Initialize(SimplePropertyType info)
        {
            this.value_ = info.Value;
            this.oldValue = info.Value;
        }

        public void Default(string name)
        {
            this.isDeleted = false;
            this.isNew = true;
            this.isChanged = false;
            this.description = name;
            this.category = DataTypes.BASIC_PROPERTIES;
            this.childrenProperties = new PropertyDescriptorCollection(null);
        }

        public static string[] ParseVarsFromFramework(object[] varsFromFramework)
        {
            if(varsFromFramework == null || varsFromFramework.Length == 0)  return null;
            string[] array = new string[varsFromFramework.Length];

            int i = 0;
            foreach(string[] values in varsFromFramework)
            {
                array[i++] = values[0];
            }

            return array;
        }

        public static ArrayList FormatVariablesList(string[] globalVars, string[] functionVars)
        {
            ArrayList allVars = new ArrayList();
            if(globalVars != null)
                foreach(string globalVar in globalVars)
                    // If this global variable name is not in the function vars
                    if(functionVars != null && 0 > Array.IndexOf(functionVars, globalVar))
                        allVars.Add(globalVar);
                    else if(functionVars == null)
                        allVars.Add(globalVar);

            if(functionVars != null)
            {
                allVars.AddRange(functionVars);
            }
      
            return allVars;
        }

        public virtual void OnContextMenuRequest(Control control, Point point)
        {
            // for derivations to implement
        }

        #region PropertyDescriptor Members

        /// <summary>
        /// Requires correct implementation of a custom TypeConverter, which should convert a string (which is entered into
        /// the PropertyGrid) to a object of type MaxProperty.  
        /// </summary>
        /// <param name="component">The component of this property</param>
        /// <param name="value">Should be of type MaxProperty, and the Value property found in data should be set to what the user entered
        /// into the PropertyGrid</param>
        public override void SetValue(object component, object value)
        {
            // New property created.
            if(! (value is MaxProperty) )
            {
                this.value_ = value;
                this.isChanged = true;
            }

            AfterSetValue(value);
            
            this.mpm.ValueChanged(false);
        }

        /// <summary>
        ///     This SetValue is not called by the .NET Framework--it is meant to be called
        ///     by a programmatic user changing a property value (which is much different than
        ///     if it is changed in the normal way by user focusing on text entry area of 
        ///     prop grid and changing values.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            SetValue(null, value);
            
            // Jog the Max Framework to detect dirty and such
            mpm.ValueChanged(true);
        }

        /// <summary>
        ///     Meant to be implemented by more complex Max Properties.
        ///     Since SetValue is the right way to catch when a user inputs something
        ///     into the grid, and we always want to at least give a derived
        ///     property a change to do something specific on SetValue,
        ///     but keep the baseline implementation of the MaxPropertyBase.SetValue
        ///     this is the method to implement for unique SetValue logic
        /// </summary>
        /// <param name="component"></param>
        /// <param name="value"></param>
        public virtual void AfterSetValue(object value)
        {
         
        }

        public override object GetValue(object component)
        {
            return this;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.GetType();
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.GetType();
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            if(this.childrenProperties.Count != 0)
            {
                return childrenProperties;
            }

            return base.GetChildProperties (instance, filter);
        }


        public override void AddValueChanged(object component, EventHandler handler)
        {
            base.AddValueChanged (component, handler);
        }

        public override void ResetValue(object component)
        {
      
        }

        protected override void OnValueChanged(object component, EventArgs e)
        {
            base.OnValueChanged (component, e);
        }

        #endregion PropertyDescriptor Members
    }

    internal class MaxPropertyConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
        {
            if(value is MaxProperty && destType == typeof(string))
            {
                MaxProperty property = (MaxProperty)value;

                return property.Value;
            }

            return base.ConvertTo(context,culture,value,destType);
        }  
    }
}

using System;
using System.Reflection;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework.Assembler;

namespace Metreos.ApplicationFramework
{
	[Serializable]
	public class Variable
	{
        public const string StandardTypePrefix  = "Metreos.Types.";
        public const char InitWithDelimiter     = '.';

        public enum InitTypes
        {
            Reference,
            Value,
            Undefined
        }

        public enum InitWithTypes
        {
            Config,
            Locale,
            Event,
            Undefined
        }

        public string Type { get { return type; } }
        private string type;

        public string DefaultValue;

        public InitTypes InitType;

        public string InitWith { get { return initWith; } }
        private string initWith;

        public InitWithTypes InitWithType { get { return initWithType; } }
        private InitWithTypes initWithType;
        
        private IVariable _value;
        public object Value
        {
            set 
            {
                if(value is IVariable)
                {
                    _value = value as IVariable; 
                }
            }
            get
            {
                IClrTypeWrapper clrVar = _value as IClrTypeWrapper;
                if(clrVar != null) { return clrVar.Value; }
                return _value;
            }
        }

		public Variable(string type)
		{
            System.Diagnostics.Debug.Assert(type != null, "Cannot initialize Variable with null type");

            Value = null;
            DefaultValue = null;
            initWith = null;
            InitType = InitTypes.Undefined;

            if(type.IndexOf(".") == -1)
            {
                // Make sure the first letter is uppercase
                if(char.IsLower(type, 0) == true)
                {
                    type = type.Insert(1, type[0].ToString().ToUpper());
                    type = type.Remove(0, 1);
                }

                type = StandardTypePrefix + type;
            }

            this.type = type;
		}

        public bool SetInitWith(string fqInitWith)
        {
            string[] bits = fqInitWith.Split(new char[] { InitWithDelimiter }, 2);

            if(bits.Length == 1)
            {
                initWith = fqInitWith;
                initWithType = InitWithTypes.Event;
            }
            else if(bits.Length == 2)
            {
                initWith = bits[1];
                try
                {
                    initWithType = (InitWithTypes) Enum.Parse(typeof(InitWithTypes), bits[0], true);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public Variable Clone()
        {
            Variable newVar = new Variable(this.type);
            newVar.InitType = this.InitType;
            newVar.initWithType = this.initWithType;

            if(this.DefaultValue != null)
            {
                newVar.DefaultValue = this.DefaultValue;
            }

            if(this.initWith != null)
            {
                newVar.initWith = this.initWith;
            }

            newVar.Value = LibraryManager.GetObjFromAssembly(_value.GetType().FullName) as IVariable;

            Debug.Assert(newVar.Value != null, "Unable to copy variable type: " + Value.GetType().FullName);

            return newVar;
        }

        public bool Assign(object obj)
        {
            if(obj != null)
            {     
                IClrTypeWrapper clrValue = _value as IClrTypeWrapper;
                if(clrValue != null && clrValue.Value != null)
                {      
                    if(clrValue.Value.GetType().IsAssignableFrom(obj.GetType()))
                    {
                        clrValue.Value = obj;
                        return true;
                    }
                    else if(obj is IConvertible)
                    {
                        object converted = System.Convert.ChangeType(obj, clrValue.Value.GetType());

                        if(converted != null)
                        {
                            clrValue.Value = converted;
                            return true;
                        }
                    }  
                }
                else if(_value == null) 
                { 
                    return false; 
                }
                else if(_value.GetType() == obj.GetType())
                {
                    _value = obj as IVariable;
                    return true;
                }
            }

            // Dynamic invoke of overloaded Parse method. Let the exceptions fly
            return Convert.ToBoolean(_value.GetType().InvokeMember(
                "Parse", 
                BindingFlags.InvokeMethod, 
                null, 
                _value, 
                new object[] { obj }));
        }

        public void Reset(LogWriter log)
        {
            Assertion.Check(log != null, "Cannot reset variables with null LogWriter");

            if(_value == null) { return; }

            _value = LibraryManager.CreateNativeType(_value.GetType(), log);
        }
	}
}

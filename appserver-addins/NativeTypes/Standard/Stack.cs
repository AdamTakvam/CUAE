using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Stack;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class Stack : IClrTypeWrapper
    {
        private System.Collections.Stack _value;

        public Stack()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.Stack) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            _value.Push(str);

            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.Stack();
        }

        #endregion
    }
}

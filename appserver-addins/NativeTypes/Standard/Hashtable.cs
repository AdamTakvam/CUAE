using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Hashtable;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class Hashtable : IClrTypeWrapper
    {
        private System.Collections.Hashtable _value;

        public Hashtable()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.Hashtable) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.Hashtable();
        }

        #endregion
    }
}

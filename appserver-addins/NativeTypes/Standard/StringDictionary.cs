using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.StringDictionary;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class StringDictionary : IClrTypeWrapper
    {
        private System.Collections.Specialized.StringDictionary _value;

        public StringDictionary()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.Specialized.StringDictionary) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.Specialized.StringDictionary();
        }

        #endregion
    }
}

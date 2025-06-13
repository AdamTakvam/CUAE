using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.StringCollection;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class StringCollection : IClrTypeWrapper
    {
        private System.Collections.Specialized.StringCollection _value;

        public StringCollection()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.Specialized.StringCollection) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            _value.Add(str);

            return true;
        }

        public bool Parse(string[] strArray)
        {
            if(strArray != null && strArray.Length > 0)
            {
                _value.AddRange(strArray);
            }

            return true;
        }
        
        public void Reset()
        {
            _value = new System.Collections.Specialized.StringCollection();
        }

        #endregion
    }
}

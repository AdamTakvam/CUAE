using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.SortedList;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class SortedList : IClrTypeWrapper
    {
        private System.Collections.SortedList _value;

        public SortedList()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.SortedList) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.SortedList();
        }

        #endregion
    }
}

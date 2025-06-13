using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.DataSet;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class DataSet : IClrTypeWrapper
    {
        private System.Data.DataSet _value;

        public DataSet()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Data.DataSet) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            return true;
        }
                
        public void Reset()
        {
            _value = new System.Data.DataSet();
        }

        #endregion
    }
}

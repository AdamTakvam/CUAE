using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.DataTable;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class DataTable : IClrTypeWrapper
    {
        private System.Data.DataTable _value;

        public DataTable()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Data.DataTable) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            return true;
        }
        
        public void Reset()
        {
            _value = new System.Data.DataTable();
        }

        #endregion
    }
}

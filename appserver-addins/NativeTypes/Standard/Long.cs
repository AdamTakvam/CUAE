using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Long;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class Long : IClrTypeWrapper
    {
        private long _value;

        public Long()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (long)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = long.Parse(str);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
                
        public void Reset()
        {
            _value = 0;
        }

        #endregion
    }
}

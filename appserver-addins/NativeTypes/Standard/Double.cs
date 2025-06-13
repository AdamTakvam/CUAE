using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Double;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class Double : IClrTypeWrapper
    {
        private double _value;

        public Double()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (double)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = double.Parse(str);
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

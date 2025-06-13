using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.UInt;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class UInt : IClrTypeWrapper
    {
        private uint _value;

        public UInt()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (uint)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = uint.Parse(str);
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

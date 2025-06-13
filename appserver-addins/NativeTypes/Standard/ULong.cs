using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.ULong;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class ULong : IClrTypeWrapper
    {
        private ulong _value;

        public ULong()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (ulong)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = ulong.Parse(str);
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

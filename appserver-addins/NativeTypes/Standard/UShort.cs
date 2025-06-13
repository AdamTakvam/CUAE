using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.UShort;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class UShort : IClrTypeWrapper
	{
        private ushort _value;

		public UShort()
		{
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (ushort)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = ushort.Parse(str);
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

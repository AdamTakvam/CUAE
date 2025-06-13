using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Int;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
	public class Int : IClrTypeWrapper
	{
        private Int32 _value;

		public Int()
		{
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (Int32)value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = int.Parse(str);
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

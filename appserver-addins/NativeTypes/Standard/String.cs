using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.String;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
	public class String : IClrTypeWrapper
	{
        private string _value;

		public String()
		{
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = value as System.String; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            _value = str;

            return true;
        }
                
        public void Reset()
        {
            _value = "";
        }

        #endregion
    }
}

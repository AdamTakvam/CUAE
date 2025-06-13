using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.ArrayList;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class ArrayList : IClrTypeWrapper
	{
        private System.Collections.ArrayList _value;

		public ArrayList()
		{
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.ArrayList) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            _value.Add(str);

            return true;
        }

        public bool Parse(System.Collections.ICollection collection)
        {
            if(collection != null && collection.Count > 0)
            {
                _value.AddRange(collection);
            }

            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.ArrayList();
        }

        #endregion
    }
}

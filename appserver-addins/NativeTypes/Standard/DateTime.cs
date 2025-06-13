using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.DateTime;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class DateTime : IClrTypeWrapper
    {
        private System.DateTime _value;

        public DateTime()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.DateTime) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = System.DateTime.Parse(str);
            }
            catch
            {
                return false;
            }

            return true;
        }
                
        public void Reset()
        {
            _value = new System.DateTime();
        }

        #endregion
    }
}

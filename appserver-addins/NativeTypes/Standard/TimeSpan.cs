using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.TimeSpan;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class TimeSpan : IClrTypeWrapper
    {
        private System.TimeSpan _value;

        public TimeSpan()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.TimeSpan) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            try
            {
                _value = System.TimeSpan.Parse(str);
            }
            catch
            {
                return false;
            }

            return true;
        }
                
        public void Reset()
        {
            _value = new System.TimeSpan();
        }

        #endregion
    }
}

using System;

using Metreos.ApplicationFramework;

using Metreos.PackageGeneratorCore.Attributes;
using Package = Metreos.Interfaces.PackageDefinitions.TypesTypes.Types.Queue;

namespace Metreos.Types
{
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, true)]
    public class Queue : IClrTypeWrapper
    {
        private System.Collections.Queue _value;

        public Queue()
        {
            Reset();
        }

        #region IClrTypeWrapper Members

        public object Value 
        { 
            get { return _value; } 
            set { _value = (System.Collections.Queue) value; }
        }

        #endregion

        #region IVariable Members

        public bool Parse(string str)
        {
            _value.Enqueue(str);

            return true;
        }
                
        public void Reset()
        {
            _value = new System.Collections.Queue();
        }

        #endregion
    }
}

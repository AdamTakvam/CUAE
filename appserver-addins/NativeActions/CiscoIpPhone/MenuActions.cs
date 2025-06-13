using System;
using System.Collections;

using Metreos.Samoa.ARE;

namespace Metreos.Native.CiscoIpPhone
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	public class CreateMenu : NativeActionBase
	{
        public CreateMenu() : base() {}

        public CreateMenu(CreateMenu cMenu) 
            : base(cMenu) {}

        public override bool Execute(Hashtable input, out string resultStr, 
            out object resultData)
        {
            resultStr = null;
            resultData = null;
            
            if(input == null)
            {
                return false;
            }

            //string title = input[];

            return true;
        }
	}
}

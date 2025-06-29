using System;
using System.Collections;

namespace Metreos.Utilities.Collections
{
    public class IntValueComparer : IComparer
    {
        public int Compare(object a, object b)
        {
            int num1 = Convert.ToInt32(a);
            int num2 = Convert.ToInt32(b);

            return num1 - num2;
        }
    }
}

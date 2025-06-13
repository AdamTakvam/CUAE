using System;
using System.Collections;

namespace Metreos.Max.Core.NativeType
{
    /// <summary>
    ///  Metreos Samoa Native Types are grouped by namespace,
    ///  and similiarly that group is found in one package file
    ///  This class exists to maintain that relationship.
    /// </summary>
    /// <remarks>
    ///  Contains MaxNativeTypeGroup's, which each contain MaxNativeType's
    /// </remarks>
    public class MaxNativeTypeNameSpaces : CollectionBase, ICollection
    {
        public MaxNativeTypeGroup this[int index]
        {
            get{ return this.List[index] as MaxNativeTypeGroup; }
            set{ this.List[index] = value; }
        }

        public MaxNativeTypeNameSpaces() : base()
        {
        }

        public bool Contains(MaxNativeTypeGroup nativeTypeGroup)
        {
            foreach(MaxNativeTypeGroup group in this.List)
                if (nativeTypeGroup.Name == group.Name)
                    return true;

            return false;
        }

        public void Add(MaxNativeTypeGroup nativeTypes)
        {
            this.List.Add(nativeTypes);
        }

        public void Remove(MaxNativeTypeGroup nativeTypes)
        {
            this.List.Remove(nativeTypes);
        }

    } // MaxNativeTypeNameSpace
} // Namespace Metreos.Max.Core.NativeType

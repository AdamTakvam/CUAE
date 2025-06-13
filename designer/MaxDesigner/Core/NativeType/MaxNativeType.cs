using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Metreos.Max.Framework;
using Metreos.Max.Core.Tool;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Max.Resources.Images;
using Metreos.Max.Manager;
using PropertyGrid.Core;


namespace Metreos.Max.Core.NativeType
{
    /// <summary>Represents a samoa action/event package</summary>
    /// <remarks>
    ///  Contains the bulk of the pertintent information in the Metreos.Max.Core.NativeType
    ///  namespace, due to the field 'rawType', which contains all 
    ///  information needed to describe a Metreos Native Type.
    /// </remarks>
    public class MaxNativeType
    {
        private   typeType          rawType;
        private   long              typeID;
        private MaxNativeTypeGroup  parent;

        public    long      TypeID       { get { return typeID;   } }
        public    string    DisplayName  { get { return rawType.displayName; } }
        public    string    Name         { get { return rawType.name; } }
        public    string    Description  { get { return rawType.description; } }
        public    typeType  RawType      { get { return rawType; } }
        public    bool      IsDisplayNameSpecified 
        { get { return rawType.displayName != null && rawType.displayName != String.Empty; 
        } }

        public MaxNativeType(MaxNativeTypeGroup parent, typeType type)
        {
            this.parent  = parent;
            this.rawType = type;
            this.typeID  = Const.Instance.NextNodeID;     
        } 
    } // MaxNativeType
} // Namespace Metreos.Max.Core.NativeType

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>Utilitly class to manage the images used in the property class</summary>
  public class PropertiesImageControl
  {
    public static ImageList images = new ImageList();

    public static readonly string[] imageNames = new string[] 
                          {
                            "Metreos.Max.Framework.Satellite.Property.Images.String.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.Variable.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.CSharp.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.On.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.Off.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.REx.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.FunctionVar.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.GlobalVar.bmp",
                            "Metreos.Max.Framework.Satellite.Property.Images.FunctionAndGlobVar.bmp"
                          };
  

    public static readonly int varTypeLiteralIndex            = 0;
    public static readonly int varTypeVariableIndex           = 1;
    public static readonly int varTypeCSharpNameIndex         = 2;
    public static readonly int logOnNameIndex                 = 3;
    public static readonly int logOffNameIndex                = 4;
    public static readonly int regularExpressionNameIndex     = 5;
    public static readonly int localVarIndex                  = 6;
    public static readonly int globalVarIndex                 = 7;
    public static readonly int localAndGlobVarIndex           = 8;

    public static Image str = null;
    public static Image var = null;
    public static Image csp = null;
    public static Image on  = null;
    public static Image off = null;
    public static Image rex = null;
    public static Image loc = null;
    public static Image glo = null;
    public static Image lg  = null;

    public static void SetImages(System.Reflection.Assembly assembly)
    {
      str = GetImage(assembly, imageNames[0]);
      var = GetImage(assembly, imageNames[1]);
      csp = GetImage(assembly, imageNames[2]);
      on  = GetImage(assembly, imageNames[3]);
      off = GetImage(assembly, imageNames[4]);
      rex = GetImage(assembly, imageNames[5]);
      loc = GetImage(assembly, imageNames[6]);
      glo = GetImage(assembly, imageNames[7]);
      lg  = GetImage(assembly, imageNames[8]);
    }

    public static Image GetImage(System.Reflection.Assembly assembly, string imageName)
    {
      Stream imageStream = null;

      try
      {
        imageStream = assembly.GetManifestResourceStream(imageName);
      }
      catch
      {
        return null;
      }

      Image image = null;

      try
      {
        image = Image.FromStream(imageStream);
      }
      catch
      {
        if(imageStream != null)
          imageStream.Close();
          
        return null;
      }

      return image;
    }
  } 
}

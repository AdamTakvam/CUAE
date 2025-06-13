using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Metreos.Max.Resources.XML
{

  public class MaxXmlResource 
  {
    public static readonly string prefix = "MaxDesigner.Resources.XML.";
    public static readonly string DefaultToolboxXmlFileName = "DefaultToolboxLayout.xml";

    public MaxXmlResource(string name)
    {
      this.path = prefix + name; this.content = null;
    }

    protected string path;
    public    string Path      { get { return path;    } }

    protected string content;
    public    string Content   { get { return content; } }

    public virtual string Load()
    {
      return null;
    }
  } // class MaxXmlResource 



  /// <summary> Wraps a bitmap resource embedded in the assembly</summary>
  public class MaxEmbeddedXmlResource: MaxXmlResource 
  {
    public MaxEmbeddedXmlResource(string name): base(name)
    {
    }

    /// <summary> Load XML from this assembly</summary>
    public override string Load()
    {
      Assembly assembly  = Assembly.GetAssembly(typeof(MaxEmbeddedXmlResource));  
      Stream   xmlStream = assembly.GetManifestResourceStream(this.path);
      if  (null == xmlStream) return null;
      int  length = 0;
      try {length = System.Convert.ToInt32(xmlStream.Length);} catch(Exception) { }

      byte[] xml  = new byte[length];    
      int  nread  = xmlStream.Read(xml, 0, length); 
      string s    = System.Text.Encoding.ASCII.GetString(xml); 
      this.content= s.Substring(s.IndexOf("<"));
      return this.content;
    }  
  } // class MaxEmbeddedXmlResource

}   // namespace














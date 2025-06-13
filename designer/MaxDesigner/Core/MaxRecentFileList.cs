using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Win32;



namespace Metreos.Max.Core
{
  /// <summary>
  /// Maintains a collection of recent project paths, for menu display.
  /// Persistence is via the registry
  /// </summary>
  public class MaxRecentFileList: System.Collections.Stack
  {
    private bool  dirty;
    public  bool  Dirty { get { return dirty; } set { dirty = value; } }

    public string First { get { return Count<1? null: (string)this.Peek(); } }


    /// <summary>Load recent file list from registry to internal stack</summary>
    /// <returns>Number of entries in recent file list</returns>
    public int Load()
    {
      RegistryKey regKey = Const.regMainKey;
      this.Clear();

      try
      { 
        regKey = regKey.OpenSubKey(Const.RegistryRecentProjectsKey, true);
        
        for(int i=0;; i++)
        {        
          string entrykey = i.ToString();   // Remove any excess entries 
          string path = (string)regKey.GetValue(entrykey);
          if (path == null) break;
          if (i >= Config.RecentFileListSize)
              regKey.DeleteValue(entrykey);
        }

        for(int i=Config.RecentFileListSize; i >= 0; --i)
        {
          // Load recent files keyed "0", "1", "2", in reverse order                   
          string path = (string)regKey.GetValue(i.ToString());
          if (path != null)  
              this.Push(path); 
        }
      }  
      catch { }

      return this.Count;
    } 


    /// <summary>Save current recent file list to registry</summary>
    /// <returns>Count of recent files saved</returns>
    public int Save()
    {
      if  (!dirty) return 0;
      RegistryKey mainKey = Const.regMainKey;
      int key = 0;

      try 
      { 
        RegistryKey regKey = mainKey.OpenSubKey(Const.RegistryRecentProjectsKey, true);
        if (regKey == null)
            regKey = mainKey.CreateSubKey(Const.RegistryRecentProjectsKey);
        int count  = this.Count;
            
        for(int i = this.Count; i > 0; i--) 
        { // Save recent files keyed "0", "1", "2", ...
          if (i > Config.RecentFileListSize) continue;   
      
          string path = (string)this.Pop();
          if (path == null) break;   
        
          regKey.SetValue((key++).ToString(), path);
        }
                                             
        for(int i = count; i < Config.RecentFileListSize; i++)
        {        
          string entrykey = i.ToString();   // Remove any excess entries 309 
          string path = (string)regKey.GetValue(entrykey);
          if (path != null) regKey.DeleteValue(entrykey);;         
        }    

        dirty = false;
      }  
      catch { }

      return key;
    } 


    /// <summary>Add path to head of list, first removing it if necessary</summary>
    public void Add(string filename) 
    {
      if  (this.First == filename)
           return;
      else
      if  (this.Contains(filename))
           this.Remove(filename);
     
      this.Push(filename); 
      dirty = true;
    }


    /// <summary>Remove entry from list</summary>
    public void Remove(string filename)
    {
      if  (this.Count < 1) return;
      Stack save = new Stack(this.Count);

      while(this.Count > 0)
      {
        string path = (string)this.Pop();
        if  (path == null) break;
        if  (path != filename)
             save.Push(path);
      } 

      this.Clear();    

      while(save.Count > 0)
      {
        string path = (string)save.Pop();
        if  (path == null) break;
        this.Push(path);
      }

      dirty = true;
    } // Remove()


    /// <summary>Return contents of stack as array of string</summary>
    public string[] Contents() 
    { 
      if  (this.Count < 1) return null;
      string[] array = new string[this.Count];
      this.ToArray().CopyTo(array,0);   
      return array;
    }

  } // class MaxRecentFileList

}   // namespace

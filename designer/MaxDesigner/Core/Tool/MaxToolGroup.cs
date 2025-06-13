using System;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.Images;



namespace Metreos.Max.Core.Tool
{
    /// <summary>Implemented by any collection of tools used in the toolbox</summary>
    public interface IMaxToolGroup
    {
        string    GroupName { get; }
        string    GroupText { get; }
        int       Count     { get; }
        ArrayList Tools     { get; }
        bool      Add(MaxTool tool);
        int       Add(MaxPackage package);
        MaxTool   this[MaxTool tool] { get; }
        MaxTool   this[string toolname, bool isAction] { get; }
    }


    public class MaxToolGroup: MaxObject, IMaxToolGroup
    {
        public MaxToolGroup(string groupname, string grouptext)
        {
            groupName = groupname; groupText = grouptext;
            tools = new ArrayList();
        }


        public int FindByTool(MaxTool tool)
        {
            MaxTool foundTool = FindTool(tool);                
            return (foundTool == null)? -1: this.Tools.IndexOf(foundTool);            
        }

        protected string    groupName;
        protected string    groupText;
        protected ArrayList tools;

        #region MaxObject Members
        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Collection; } }
        public void MaxSerialize(System.Xml.XmlTextWriter writer) { }
        public string ObjectDisplayName { get { return this.groupName; } }
        #endregion

        #region IMaxToolGroup Members

        public string GroupName { get { return groupName; } }
        public string GroupText { get { return groupText; } }
        public ArrayList Tools  { get { return tools;     } }
        
        
        public bool Add(MaxTool tool)
        {
            if (tool == null || this[tool] != null) return false;
            tools.Add(tool);
            return true;
        }


        public int Add(MaxPackage package) 
        {
            int count = 0;

            foreach(MaxTool tool in package.Tools) 
            {
                if (tool == null || this[tool] != null) continue;

                MaxEventTool eventtool = tool as MaxEventTool;  
                if (eventtool != null      // Omit async callbacks & triggers            
                    && (eventtool.PmEvent.Type == PropertyGrid.Core.EventType.asyncCallback  
                    || !eventtool.IsUnsolicitedEvent())) 
                    continue;             

                tools.Add(tool);
                count++;
            }
                     
            return count;
        }


        /// <summary>Fully qualified name matching and action/event matching</summary>
        public MaxTool this[MaxTool tool] { get { return FindTool(tool); } }


        /// <summary>
        ///     Supply the fully qualified toolname, as well as whether it is an action or an event
        ///     The action/event is important because an action and event can share the same name.
        ///     This can be used for custom tools, even if they are neither actions or events
        ///     
        ///     Though this accessor is somewhat ugly, 
        ///     it is necessary if we want to keep actions and events in the same
        ///     collection, and use opaque strings for lookup
        /// </summary>
        public MaxTool this[string toolname, bool isAction]
        {
            get
            {   // We check if the fully qualified name is the same, and if it is an action.
                // We have to make an exception for Stock Tools, because they do not fit either
                // criteria.  In this exception, we make another exception for the MaxCodeTool,
                // because even though it is a Stock Tool, it is an action, nonetheless.
                foreach(MaxTool tool in this.tools)
                    if  (tool.FullQualName == toolname && 
                        (isAction == tool.IsAction || 
                        (tool.Package is MaxStubPackage && (!(tool is MaxCodeTool))))) return tool;
     
                return null;
            }
        }


        public int Count { get { return tools.Count; } }


        /// <summary>
        ///  Compares for fully qualified name matching, and action/event matching.
        ///  Action/Event matching isn't used for Stock tools, except for the custom code tool
        /// </summary>
        /// <param name="tool"> The tool to look for </param>
        /// <returns> The tool from the collection if found, otherwise null </returns>
        protected MaxTool FindTool(MaxTool tool)
        {
            foreach(MaxTool thistool in this.Tools)
                if (thistool.FullQualName.Equals(tool.FullQualName) && thistool.Package.Name.Equals(tool.Package.Name) 
                    && tool.IsAction == thistool.IsAction && tool.IsEvent == tool.IsEvent)
                    return thistool;

            return null;
        }

        #endregion

    } // class MaxToolGroup

}   // namespace

using System;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Max.Framework.Satellite.Property;

namespace Metreos.Max.Framework
{
    // <summary>
    // Summary description for ScriptConstructionMetaData.
    // </summary>
    public class ScriptMap : ICollection, IMetreosScript
    {   
        #region ICollection Properties
        public int Count{ get { return functions.Count; } }
        public bool IsSynchronized { get { return false; } }
        public object SyncRoot { get { return null; } }

        /// <summary>
        /// Key is the function name
        /// Value is the FunctionMap object representing that function
        /// </summary>
        /// <returns>Enumerator for all functions</returns>
        public IDictionaryEnumerator GetFunctionEnumerator()
        {
            return functions.GetEnumerator();
        }
        #endregion

        #region IMetreosScript Properties

        public string Description { get { return description; } }
        public string ScriptName { get { return name; } }
        public globalVariablesType GlobalVariables { get { return globalVars; } }
        #endregion

        // public string Trigger { get { return triggerName; } set { triggerName = value; } }

        private Hashtable functions;
        private string name;
        private globalVariablesType globalVars;
        private string description;

        public ScriptMap(string scriptName,
            globalVariablesType globalVars, string description)
        {
            this.functions = new Hashtable();
            this.name = scriptName;
            this.globalVars = globalVars;
            this.description = description;
        }

        public void Reset()
        {
            functions.Clear();
        }

        public FunctionMap this[object index]
        {
            get
            {
                if(functions.Contains(index))
                    return functions[index] as FunctionMap;
                else
                    return null;
            }
        }

        public void AddFunction(string functionName, eventType @event, eventParamType[] eventParams,
            Hashtable actions,  Hashtable labels, NodeMap startNode)
        {
            functions[functionName] = new FunctionMap(functionName, @event, eventParams, actions, labels, startNode);
        }

        public void CopyTo(System.Array array, int i){}
    
        public IEnumerator GetEnumerator()
        {
            return this.functions.Values.GetEnumerator();
        }
    }


    public class FunctionMap : IFunction, ICollection, IMetreosFunction
    {
        public string Name { get { return name; } }
        public NodeMap StartNode { get { return startNode; } }  
        // public bool Trigger { get { return triggering; } }
        public ArrayList DirectChildrenLoops { get { return directChildrenLoops; } set { directChildrenLoops = value; } }

        #region ICollection Properties
        public int Count{ get { return actions.Count; } }
        public bool IsSynchronized { get { return false; } }
        public object SyncRoot { get { return null; } } 
        #endregion

        #region IMetreosFunction Properties

        public string FunctionName { get { return name; } }
        public eventType @Event { get { return @event; } }
        public Hashtable Actions { get { return actions; } }
        public variableType[] RegularVariables { get { return regularVars; } set { regularVars = value; } }
        public parameterType[] Parameters { get { return parameters; } set { parameters = value; } }
        public eventParamType[] EventParams { get { return eventParams; } }
        public loopType[] Loops { get { return loops; } }

        #endregion

        protected string name;
        protected ArrayList directChildrenLoops;
        protected Hashtable actions;
        protected Hashtable labels;
        protected NodeMap startNode;
        protected eventType @event;
        protected variableType[] regularVars;
        protected parameterType[] parameters;
        protected eventParamType[] eventParams; 
        protected loopType[] loops;

        public FunctionMap(){}

        public FunctionMap(string functionName, eventType @event, 
        eventParamType[] eventParams, Hashtable actions, Hashtable labels, NodeMap startNode)
        {
            this.name = functionName;
            this.@event = @event;
            this.eventParams = eventParams;
            this.actions = actions != null ? actions : new Hashtable();
            this.labels = labels != null ? labels : new Hashtable();
            this.directChildrenLoops = new ArrayList();
            this.startNode = startNode;
        }

        public IDictionaryEnumerator GetActionEnumerator()
        {  
            return actions.GetEnumerator();
        }

        #region ICollection Methods
        public ActionMap this[object id]
        {
            get
            {
                if(id is string)
                {
                    long longId = long.Parse(id.ToString());

                    return (actions.Contains(longId))?
                            actions[longId] as ActionMap: null;
                }
                else if(id is long)
                {
                    return (actions.Contains(id))?
                            actions[id] as ActionMap: null;
                }
                else return null;
            }
        }

        public void CopyTo(System.Array array, int i){}
    
        public IEnumerator GetEnumerator()
        {
            return this.actions.Values.GetEnumerator();
        }

        public IEnumerator GetLabelEnumerator()
        {
            return this.labels.Values.GetEnumerator();
        }

        #endregion
    }


    public class NodeMap
    {
        public long Id { get { return id; } } 
        public Link[] OutgoingLinks { get { return outgoingLinks; } }
        public Link[] IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }
        public string Name { get { return name; } }
        public bool IsStart { get { return isStart; } set { isStart = value; } }

        private bool isStart;
        protected Link[] outgoingLinks;
        protected Link[] incomingLinks;
        protected string name;
        protected long id;

        public NodeMap(long id, Link[] outgoingLinks, Link[] incomingLinks, string name)
        {
            this.id = id;
            this.outgoingLinks = outgoingLinks;
            this.incomingLinks = incomingLinks;
            this.name = name;
            this.isStart = false;
        }

        public void AddIncomingLink(Link newLink)
        {
            Link[] oneBigger = new Link[(incomingLinks != null ? incomingLinks.Length : 0) + 1];
            if(incomingLinks != null)
                incomingLinks.CopyTo(oneBigger, 0);
            oneBigger[oneBigger.Length -1] = newLink;
            incomingLinks = oneBigger;
        }

        public void AddOutgoingLink(Link newLink)
        {
            Link[] oneBigger = new Link[(outgoingLinks != null ?  outgoingLinks.Length : 0) + 1];
            if(outgoingLinks != null)
                outgoingLinks.CopyTo(oneBigger, 0);
            oneBigger[oneBigger.Length -1] = newLink;
            outgoingLinks = oneBigger;
        }

        public bool ContainsBranchCondition(string condition)
        {
            // Special values
            if(condition == ConstructionConst.LOG_EXIT || condition == ConstructionConst.LOG_ENTRY)
                return true;
             
            if(outgoingLinks == null || outgoingLinks.Length == 0)  return false;

            foreach(Link outgoing in outgoingLinks)
                if(outgoing.HasCondition(condition))  return true;

            return false;
        }
    }


    public class ActionMap : NodeMap, IViewport
    { 
        public actionType ActionXml { get { return actionXml; } }
        public Hashtable Logs { get { return logs; } set { logs = value; } }
        public bool Final { get { return final; } }
        public bool IsStop { get { return isStop; } set { isStop = value; } }
        private bool final;
        private bool isStop;
        private ViewportData dimensions;
        private actionType actionXml;
        private Hashtable logs;

        #region IViewport Properties
        public ViewportData Dimensions { get { return dimensions; } }
        #endregion 

        public ActionMap(long id, Link[] incomingLinks, Link[] outgoingLinks, string name, bool final, ViewportData dimensions, XmlNode canvasNode) : base(id, outgoingLinks, incomingLinks, name)
        {
            this.final = final;
            this.dimensions = dimensions;
            this.actionXml = new actionType();
            this.logs = new Hashtable();
            this.isStop = false;
        } 
    }


    public class LabelMap : NodeMap
    {  
        public LabelType Type { get { return type; } }
    
        private LabelType type;

        public enum LabelType
        {
            In,
            Out
        }

        public LabelMap(long id, Link[] outgoingLinks, Link[] incomingLinks, string name, LabelType type) : base(id, outgoingLinks, incomingLinks, name)
        {
            this.type = type;
        }
    }


    /// <summary> Serves to create the necessary information needed for later assembling 
    ///           the links on the map efficiently and concisely
    /// </summary>
    public class LoopMetadata : NodeMap
    {
        public  new string Name { get { return id.ToString(); } }
        public  int EntryPort { get { return entryPort; } }
        public  long Container { get { return container; } }
        public  Link EntryLink { get { return entryLink; } }
        public  ArrayList Children { get { return children; } set { children = value; } }
        public  bool IsTopLevel { get { return container == -1; } }
        public  LoopMetadata Parent { get { return parent; } set { parent = value; } }
        public  Hashtable Actions { get { return actions; } set { actions = value; } }
        public  loopCountType LoopCount { get { return loopCount; } }
        public  NodeMap StartNode { get { return startNode; } set { startNode = value; } }

        private int entryPort;
        private long container;
        private ArrayList children;
        private LoopMetadata parent;
        private Hashtable actions;
        private Link entryLink;
        private loopCountType loopCount;
        private NodeMap startNode;

        public LoopMetadata(long id, int entryPort, long container, 
            Link entryLink, Link[] outgoingLinks, loopCountType loopCount)
            : base(id, outgoingLinks, null, id.ToString())
        {
            this.id        = id;
            this.entryPort = entryPort;
            this.container = container;
            this.entryLink = entryLink;
            this.loopCount = loopCount;
            this.parent    = null;
            this.children  = new ArrayList();
            this.actions   = new Hashtable();
        }
    }


    public class LoopMap : FunctionMap, IFunction
    {
        public  FunctionMap Parent { get { return parent; } }
        public  NodeMap StopNode { get { return stopNode; } }
        public  loopCountType LoopCount { get { return loopCount; } }
        public  Link[] OutgoingLinks { get { return outgoingLinks; } set { outgoingLinks = value; } }
        public  Link[] IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }
        public  long Id { get { return id; } }
        public  bool IsStart { get { return isStart; } }

        private NodeMap stopNode = null;
        private long id;
        private loopCountType loopCount;
        private FunctionMap parent;    
        private Link[] incomingLinks;
        private Link[] outgoingLinks;
        private bool isStart;

        public LoopMap(long id, string name, loopCountType loopCount, NodeMap startNode,  Hashtable actions,
            Link[] outgoingLinks, Link[] incomingLinks, bool isStart, FunctionMap parent)
        {
            this.id = id; 
            this.name = name;
            this.loopCount = loopCount;
            this.actions = actions;
            this.startNode = startNode;
            // this.stopNode = stopNode;
            this.incomingLinks = incomingLinks;
            this.outgoingLinks = outgoingLinks;
            this.parent = parent;
            this.isStart = isStart;
            this.directChildrenLoops = new ArrayList();
        }      
    }


    public interface IFunction
    {
        string Name { get; }
        NodeMap StartNode { get; }
    }


    public interface ILoop
    {
        NodeMap StopNode { get; }
    }


    public class Link
    {
        public string ParentFunction { get { return parentFunction; } set { parentFunction = value; }}
        public long Id { get { return id; } set{ id = value; } }
        public string Condition { get { return condition; } set { condition = value; } }

        private string parentFunction;
        private long id;
        private string condition;


        public Link(string parentFunction, long id, string condition)
        {
            this.parentFunction = parentFunction;
            this.id = id;
            this.condition = condition;
        }

        public bool HasCondition(string condition)
        {
            return String.Compare(this.Condition, condition, true) == 0; 
        }
    }
}

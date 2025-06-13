using System;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework
{
    /// <summary>
    ///     If is treated as a node by the build process, then it must implement this interface
    /// </summary>
    public interface ICompileNode
    {
        CompileLinkCollection IncomingLinks { get;  set; }
        CompileLinkCollection OutgoingLinks { get;  set; }    
        Link[] OutgoingLinksFromMap { get; }
        Link[] IncomingLinksFromMap { get; }
        long IdFromMap { get; }
        long Id { get; }

        void AddIncomingLink(string condition, ICompileNode fromNode);
        void AddOutgoingLink(string condition, ICompileNode toNode);
        bool ContainsLink(Link link);
        ICompileNode GetEntryNode(Link outgoingLink);
        ICompileNode GetExitNode(Link incomingLink);

        #region Called By ScriptCompiler

        /// <summary> Create a true referenced link to and from this node </summary>
        /// <param name="allNodes"> All nodes in this function </param>
        void Link(CompileNodeCollection allNodes);

        /// <summary>Called on every MetreosAction node, to id it.  </summary>
        /// <param name="number">The number to use to id</param>
        /// <returns>The amount of ids used</returns>
        int Number(int number);

        /// <summary> Create links with long to/from fields </summary>
        void LinkByNumber();

        /// <summary> Retrieves the XML describing this node </summary>
        /// <returns> The XML describing this node</returns>
        actionType[] RetrieveXml();
        #endregion
    }


    public class CompileNodeCollection : ICollection
    {
        private ArrayList nodes;

        #region ICollection Members

        public bool IsSynchronized{ get { return false; } }

        public int Count { get { return nodes.Count; } }

        public void CopyTo(Array array, int index) { }

        public object SyncRoot { get { return null; } }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        #endregion

        public ICompileNode this[long id]
        {
            get
            {
                foreach(ICompileNode node in nodes)
                     if(node.Id == id) return node;

                return null;
            }
        }

        public void Add(ICompileNode node)
        {
            nodes.Add(node);
        }

        public void InsertAt(int i, object obj)
        {
            nodes.Insert(i, obj);
        }

        public void RemoveAt(int i)
        {
            nodes.RemoveAt(i);
        }

        public void Remove(object obj)
        {
            nodes.Remove(obj);
        }

        public ICompileNode this[int index] 
            {get { return nodes[index] as ICompileNode; } 
             set { nodes[index] = value; } }

        public CompileNodeCollection()
        {
            nodes = new ArrayList();
        }
    }


    public class CompileLink
    {
        public string Condition { get { return condition; } set { condition = value; } }
        public ICompileNode To { get { return toNode; } set { toNode = value; } }
        public ICompileNode From { get { return fromNode; } set { fromNode = value; } }
    
        private string condition;
        private ICompileNode toNode;
        private ICompileNode fromNode;

        public CompileLink(string condition, ICompileNode toNode, ICompileNode fromNode)
        {
            this.condition = condition;
            this.toNode = toNode;
            this.fromNode = fromNode;
        }
    }


    public class CompileLinkCollection : ICollection
    {
        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CompileLinkCollection.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                // TODO:  Add CompileLinkCollection.Count getter implementation
                return links.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            // TODO:  Add CompileLinkCollection.CopyTo implementation
        }

        public object SyncRoot
        {
            get
            {
                // TODO:  Add CompileLinkCollection.SyncRoot getter implementation
                return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return links.GetEnumerator();
        }

        #endregion

        public void Add(CompileLink link)
        {
            links.Add(link);
        }

        public CompileLink this[int index] 
           {get { return links[index] as CompileLink; } 
            set { links[index] = value; } }

        private ArrayList links;
  
        public CompileLinkCollection()
        {
            links = new ArrayList();
        }
    }

    public interface ILog
    {
        string Condition { get; }
    }
}

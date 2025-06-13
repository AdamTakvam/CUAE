using System;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework
{
	/// <summary> Represents a loop on the Max Canvas </summary>
	public class LoopNode : ICompileNode
	{
    public CompileLinkCollection OutgoingLinks { get { return outgoingLinks; } set { outgoingLinks = value; } }
    public CompileLinkCollection IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }
    public ICompileNode GetEntryNode(Link outgoingLink) { return this; }
    public ICompileNode GetExitNode(Link incomingLink) { return this; }
    public virtual bool ContainsLink(Link link) { return link.Id == loopMap.Id; }
    public Link[] OutgoingLinksFromMap{ get { return loopMap.OutgoingLinks; } }
    public Link[] IncomingLinksFromMap{ get { return loopMap.IncomingLinks; } }
    public long IdFromMap { get { return loopMap.Id; } }
    public long Id { get { return id; } }
    public loopCountType LoopCount { get { return loopCount; } }
    public nextActionType[] NextActions { get { return nextActions; } }

    private long id;
    private ScriptCompiler compiler;
    private LoopMap loopMap;
    private CompileLinkCollection incomingLinks;
    private CompileLinkCollection outgoingLinks;
    private nextActionType[] nextActions;
    private loopCountType loopCount;

	public LoopNode(ScriptCompiler compiler, LoopMap loopMap)
    {
      this.compiler = compiler;
      this.loopMap = loopMap;
      this.incomingLinks = new CompileLinkCollection();
      this.outgoingLinks = new CompileLinkCollection(); 
      this.id = loopMap.Id;
      this.nextActions = null;
      this.loopCount = loopMap.LoopCount;
    }

    #region ICompileNode Members   

    public virtual void AddIncomingLink(string condition, ICompileNode fromNode)
    {
      incomingLinks.Add(new CompileLink(condition, this, fromNode));
    }

    public virtual void AddOutgoingLink(string condition, ICompileNode toNode)
    {
      outgoingLinks.Add(new CompileLink(condition, toNode, this));
    }

    public virtual int Number(int number)
    {
      // As this is a node contained on the map, it should be considered to already
      // have a valid id, and should not be overwritten
      if(compiler.Scheme == NumberingScheme.RetainMaxNodes) return 0;

      id = number;
      int numIds = 1;
      return numIds;
    }  
  
    public virtual void Link(CompileNodeCollection allNodes)
    {
      LinkOutgoing(allNodes);

      LinkIncoming(allNodes);
    }

    private void LinkOutgoing(CompileNodeCollection allNodes)
    {
      if(OutgoingLinksFromMap == null)
        return; 

      LinkNormalOutgoing(allNodes);
    }

    private void LinkNormalOutgoing(CompileNodeCollection allNodes)
    {
      if(OutgoingLinksFromMap == null)  return;

      foreach(Link outgoingLink in OutgoingLinksFromMap)
      {
        bool foundLink = false;
        for(int i = 0; i < allNodes.Count; i++)
        {
          if(allNodes[i].ContainsLink(outgoingLink))
          {
            // Found our linked to action.
            AddOutgoingLink(outgoingLink.Condition, allNodes[i].GetEntryNode(outgoingLink));
            foundLink = true;
            break;
          }
        }

        if(!foundLink)
        {
          // If we reach this point, then this link must point to a loop not contained at this level
          AddOutgoingLink(outgoingLink.Condition, new DeadEndLoopNode(outgoingLink.Id));
        }
      }
    }

    private void LinkIncoming(CompileNodeCollection allNodes)
    {
      if(IncomingLinksFromMap == null)  return;
        
      foreach(Link incomingLink in IncomingLinksFromMap)
      {
        bool foundLink = false;
        for(int i = 0; i < allNodes.Count; i++)
        {
          if(allNodes[i].ContainsLink(incomingLink))
          {
            AddIncomingLink(incomingLink.Condition, allNodes[i].GetExitNode(incomingLink));
            foundLink = true;
            break;
          }
        }

        if(!foundLink)
        {
          // If this point is reached, then we assume it is linking to a loop node contained not at this level.
          AddIncomingLink(incomingLink.Condition, new DeadEndLoopNode(incomingLink.Id));
        }
      }
    }

    public virtual void LinkByNumber()
    {
      nextActions = outgoingLinks.Count != 0 ? new nextActionType[outgoingLinks.Count] : null;

      for(int i = 0; i < outgoingLinks.Count; i++)
      {
        nextActions[i] = new nextActionType();
        nextActions[i].returnValue = outgoingLinks[i].Condition;
        nextActions[i].Value = outgoingLinks[i].To.Id;
      }
    }

    /// <summary>This is stupid.  REFACTOR</summary>
    public virtual actionType[] RetrieveXml()
    {
      return null;
    }

    #endregion
  }

  public class DeadEndLoopNode : ICompileNode
  {
    public CompileLinkCollection OutgoingLinks { get { return null; } set {  } }
    public CompileLinkCollection IncomingLinks { get { return null; } set {  } }
    public ICompileNode GetEntryNode(Link outgoingLink) { return this; }
    public ICompileNode GetExitNode(Link incomingLink) { return this; }
    public virtual bool ContainsLink(Link link) { return link.Id == id; }
    public Link[] OutgoingLinksFromMap{ get { return null; } }
    public Link[] IncomingLinksFromMap{ get { return null; } }
    public long IdFromMap { get { return id; } }
    public long Id { get { return id; } }
   
    private long id;

    public DeadEndLoopNode(long id)
    {
      this.id = id;
    }

    #region ICompileNode Members   

    public virtual void AddIncomingLink(string condition, ICompileNode fromNode)
    {}

    public virtual void AddOutgoingLink(string condition, ICompileNode toNode)
    {}

    public virtual int Number(int number)
    {
      return 0;
    }  
  
    public virtual void Link(CompileNodeCollection allNodes)
    {}

    public virtual void LinkByNumber()
    {}

    /// <summary>This is stupid. REFACTOR</summary>
    public virtual actionType[] RetrieveXml()
    {
      return null;
    }

    #endregion
  }
}

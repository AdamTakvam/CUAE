using System;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;
using Metreos.Interfaces;

namespace Metreos.Max.Framework
{
    /// <summary>
    /// Summary description for MetreosActionNode.
    /// </summary>
    public class MetreosActionNode : ICompileNode
    {
        #region ICompileNode Members
        public CompileLinkCollection OutgoingLinks { get { return outgoingLinks; } set { outgoingLinks = value; } }
        public CompileLinkCollection IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }
        public ICompileNode GetEntryNode(Link outgoingLink) { return this; }
        public ICompileNode GetExitNode(Link incomingLink) { return this; }
        public virtual bool ContainsLink(Link link) { return link.Id == IdFromMap; }
        public Link[] OutgoingLinksFromMap{ get { return nodeMap.OutgoingLinks; } }
        public Link[] IncomingLinksFromMap{ get { return nodeMap.IncomingLinks; } }
        public long IdFromMap { get { return nodeMap.Id; } }

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
            numIds = 1;
            return numIds;
        }  

        public virtual void Link(CompileNodeCollection allNodes)
        {
            for(int i = 0; i < allNodes.Count; i++)
            {
                if(! (allNodes[i] is MaxParentNode) )
                    continue;

                MaxParentNode maxNode = allNodes[i] as MaxParentNode;
        
                if(maxNode.MainAction != this || maxNode.LoggingNodes == null)
                    continue;

                if(maxNode.HasEntry)
                    AddIncomingLink(ConstructionConst.LOG_NEXT_CONDITION, maxNode.EntryNode as EntryLogNode);

                IEnumerator logs = maxNode.LoggingNodes.GetEnumerator();
                while(logs.MoveNext())
                {
                    ILog log = logs.Current as ILog;
                    if(log.Condition == ConstructionConst.LOG_ENTRY || log.Condition == ConstructionConst.LOG_EXIT)
                        continue;

                    AddOutgoingLink(log.Condition, log as MetreosActionNode);
                }

                if(maxNode.HasExit)
                    if(maxNode.UnloggedConditions != null)
                        foreach(string condition in maxNode.UnloggedConditions)
                            AddOutgoingLink(condition, maxNode.ExitNode.GetExitNode(condition));   
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

        public virtual actionType[] RetrieveXml()
        {
            actionXml.id = id;
            actionXml.nextAction = nextActions;
            return new actionType[] { actionXml };
        }
        #endregion ICompileNode

        public long Id { get { return id; } set { id = value; } }
        public nextActionType[] NextActions { get { return nextActions; } set { nextActions = value; } }
        public actionType RepresentativeXml { get { return actionXml; } set { actionXml = value; } }
        public bool IsStart { get { return nodeMap.IsStart; } }
        protected CompileLinkCollection outgoingLinks;
        protected CompileLinkCollection incomingLinks; 
        protected NodeMap nodeMap;
        protected actionType actionXml;
        protected long id;
        protected int numIds;
        protected nextActionType[] nextActions;
        protected ScriptCompiler compiler;

        public MetreosActionNode(ScriptCompiler compiler, actionType actionXml, NodeMap nodeMap) : base()
        {
            this.incomingLinks = new CompileLinkCollection();
            this.outgoingLinks = new CompileLinkCollection();
            this.compiler = compiler;
            this.actionXml = actionXml;
            this.nodeMap = nodeMap;   
            this.id = actionXml.id;
            this.numIds = 0;
            this.nextActions = null;
        }

        public string[] OutgoingLinkConditionsFromMap
        {
            get
            {
                string[] allConditions = new string[nodeMap.OutgoingLinks.Length];

                for(int i = 0; i < nodeMap.OutgoingLinks.Length; i++)
                {
                    allConditions[i] = nodeMap.OutgoingLinks[i].Condition;
                }

                return allConditions;
            }
        }
    }


    public class LogNode : MetreosActionNode, ILog
    {
        public string Condition { get { return condition; } set { condition = value; } }
        public MetreosActionNode ParentAction { get { return parentAction; } }
        protected string condition;
        protected MetreosActionNode parentAction;

        public LogNode(ScriptCompiler compiler, actionType actionXml, string condition, MetreosActionNode parentAction) : base(compiler, actionXml, null)
        {   
            this.condition = condition;
            this.parentAction = parentAction;
        }

        public override void Link(CompileNodeCollection allNodes)
        {
            AddIncomingLink(condition, parentAction);
      
            for(int i = 0; i < allNodes.Count; i++)
            {
                if(allNodes[i] is MaxParentNode)
                {
                    MaxParentNode maxNode = allNodes[i] as MaxParentNode;
                    if(maxNode.MainAction == parentAction)
                    {
                        IEnumerator logs = maxNode.LoggingNodes.GetEnumerator();

                        while(logs.MoveNext())
                        {
                            ICompileNode log = logs.Current as ICompileNode;
                            if(log is ExitLogGroup)
                                AddOutgoingLink(condition, (log as ExitLogGroup).GetExitNode(condition));
                        }

                        break;
                    }
                }
            }
        }


        public override int Number(int number)
        {
            id = number;
            numIds = 1;
            return numIds;
        }


        // REFACTOR. I dont know if this is a kludge or not. 
        // Without this, exit log nodes have their next action
        // as the condition that created them (i.e., not necessarily "default")

        public override actionType[] RetrieveXml()
        {
            actionType[] xmls = base.RetrieveXml();
      
            if (xmls != null)
            {
                foreach(actionType actionXml in xmls)
                {
                    System.Diagnostics.Debug.Assert(actionXml.nextAction.Length == 1, IErrors.generatedLogMultiLink);

                    actionXml.nextAction[0].returnValue = ConstructionConst.LOG_NEXT_CONDITION;
                }
            }
      
            return xmls;
        }
    }


    public class ExitLogNode : LogNode
    {
        public ExitLogNode(ScriptCompiler compiler, actionType actionXml, string condition, 
            MetreosActionNode parentAction) : base (compiler, actionXml, condition, parentAction){}
  
        public override void Link(CompileNodeCollection allNodes) {} 
    }


    public class EntryLogNode : LogNode
    {
        public EntryLogNode(ScriptCompiler compiler, actionType actionXml, string condition, 
            MetreosActionNode parentAction) : base (compiler, actionXml, condition, parentAction){}
  
        public override void Link(CompileNodeCollection allNodes)
        {
            AddOutgoingLink(ConstructionConst.LOG_NEXT_CONDITION, parentAction);
        }
    }
}

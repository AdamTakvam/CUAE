using System;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework
{
    /// <summary>
    /// A MaxParentNod abstracts logging, so, here we lump an action and its log into one ICompileNode
    /// </summary>
    public class MaxParentNode : ICompileNode
    {
        #region ICompileNode
        public CompileLinkCollection OutgoingLinks { get { return outgoingLinks; } set { outgoingLinks = value; } }
        public CompileLinkCollection IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }
        public Link[] OutgoingLinksFromMap { get { return mainAction.OutgoingLinksFromMap; } }
        public Link[] IncomingLinksFromMap { get { return mainAction.IncomingLinksFromMap; } }
        public long IdFromMap { get { return mainAction.IdFromMap; } }
        public long Id { get { return mainAction.Id; } }
        public ICompileNode GetEntryNode(Link outgoingLink) { return hasEntry ? entryNode.GetEntryNode(outgoingLink) : mainAction.GetEntryNode(outgoingLink); }
        public ICompileNode GetExitNode(Link incomingLink) { return hasExit ? exitNode.GetEntryNode(incomingLink) : mainAction.GetExitNode(incomingLink); }
        public bool ContainsLink(Link link) { return link.Id == IdFromMap; }

        public void AddIncomingLink(string condition, ICompileNode fromNode)
        {
            if(hasEntry)
            {
                incomingLinks.Add(new CompileLink(condition, entryNode as EntryLogNode, fromNode));
                entryNode.AddIncomingLink(condition, fromNode);
            }
            else
            {
                incomingLinks.Add(new CompileLink(condition, mainAction, fromNode));
                mainAction.AddIncomingLink(condition, fromNode);
            }
        }

        public void AddOutgoingLink(string condition, ICompileNode toNode)
        {
            if(hasExit)
            {
                outgoingLinks.Add(new CompileLink(condition, toNode, exitNode.GetExitNode(condition)));
                exitNode.AddOutgoingLink(condition, toNode);
            }
            else
            {
                ICompileNode log;
                bool hasCondition = HasLogOnCondition(condition, logNodes, out log);
                ICompileNode fromNode = hasCondition ? log : mainAction;

                outgoingLinks.Add(new CompileLink(condition, toNode, fromNode as MetreosActionNode));
                fromNode.AddOutgoingLink(condition, toNode);
            }
        }

        public int Number(int number)
        {
            this.mainAction.Number(number);

            int numIds = 1;

            for(int i = 0; i < (logNodes != null ? logNodes.Count : 0) ; i++)
                numIds += logNodes[i].Number(number + numIds);

            return numIds;
        }

        public virtual void Link(CompileNodeCollection allNodes)
        {
            for(int i = 0; i < (logNodes != null ? logNodes.Count : 0) ; i++)
            {
                logNodes[i].Link(allNodes);
            }

            mainAction.Link(allNodes);

            LinkOutgoing(allNodes);

            LinkIncoming(allNodes);
        }

        public virtual void LinkByNumber()
        {
            mainAction.LinkByNumber();

            for(int i = 0; i < (logNodes != null ? logNodes.Count : 0); i++)
            {
                logNodes[i].LinkByNumber();
            }
        }

        public virtual actionType[] RetrieveXml()
        {
            ArrayList actionBuilder = new ArrayList();

            if(hasEntry)
                actionBuilder.AddRange(entryNode.RetrieveXml());

            actionBuilder.AddRange(mainAction.RetrieveXml());

            for(int i = 0; i < (logNodes != null ? logNodes.Count : 0); i++)
            {
                if(hasEntry)
                    if(logNodes[i] == entryNode)
                        continue;

                actionBuilder.AddRange(logNodes[i].RetrieveXml());
            }   

            actionType[] actions = new actionType[actionBuilder.Count];
            actionBuilder.CopyTo(actions);
            return actions;
        }

        #endregion

        public CompileNodeCollection LoggingNodes { get { return logNodes; } set { logNodes = value; } }
        public MetreosActionNode MainAction { get { return mainAction; } set { mainAction = value; } }
        public string[] UnloggedConditions { get { return unloggedConditions; } set { unloggedConditions = value; } }
        public ICompileNode EntryNode { get { return entryNode; } }
        public ExitLogGroup ExitNode  { get { return exitNode; } }
        public bool HasExit { get { return hasExit; } }
        public bool HasEntry { get { return hasEntry; } }
        private MetreosActionNode mainAction;
        private CompileLinkCollection outgoingLinks;
        private CompileLinkCollection incomingLinks;
        private CompileNodeCollection logNodes; 
        private bool hasEntry;
        private bool hasExit;
        private ICompileNode entryNode;
        private ExitLogGroup exitNode;
        protected string[] unloggedConditions;

        public MaxParentNode(MetreosActionNode mainAction, CompileNodeCollection logNodes)
        {
            this.incomingLinks = new CompileLinkCollection();
            this.outgoingLinks = new CompileLinkCollection();
            this.mainAction = mainAction;
            this.logNodes = logNodes;
            this.hasEntry = HasLogOnCondition(ConstructionConst.LOG_ENTRY, logNodes, out entryNode);
            this.hasExit = HasExitLog(logNodes, out exitNode);
            EstablishUnloggedConditions();
            PruneLogsWithoutBranch();
        }

        private bool HasExitLog(CompileNodeCollection logNodes, out ExitLogGroup exitLog)
        {
            exitLog = null;

            if(logNodes == null)
                return false;

            IEnumerator logs = logNodes.GetEnumerator();

            while(logs.MoveNext())
            {
                ILog log = logs.Current as ILog;

                if(log.Condition == ConstructionConst.LOG_EXIT)
                {
                    exitLog = log as ExitLogGroup;
                    return true;
                }
            }

            return false;
        }


        private bool HasLogOnCondition
        (string condition, CompileNodeCollection logNodes, out ICompileNode logNode)
        {
            logNode = null;

            if(logNodes == null)
                return false;

            IEnumerator logs = logNodes.GetEnumerator();

            while(logs.MoveNext())
            {
                ILog log = logs.Current as ILog;

                if(String.Compare(log.Condition, condition, true) == 0)
                {
                    logNode = log as ICompileNode;
                    return true;
                }
            }

            return false;
        }


        private void EstablishUnloggedConditions()
        {
            if(OutgoingLinksFromMap == null) return;

            ArrayList unloggedConditionBuilder = new ArrayList();

            foreach(string condition in mainAction.OutgoingLinkConditionsFromMap)
            {
                bool foundCondition = false;

                for(int i = 0; i < (logNodes != null ? logNodes.Count : 0) ; i++)
                {
                    string logCondition = (logNodes[i] as ILog).Condition;

                    if(String.Compare(logCondition, condition, true) == 0)
                    {
                        foundCondition = true;
                        break;
                    }
                }

                if(!foundCondition)
                    unloggedConditionBuilder.Add(condition);
            }
  
            if(unloggedConditionBuilder.Count == 0) return;

            unloggedConditions = new string[unloggedConditionBuilder.Count];
            unloggedConditionBuilder.CopyTo(unloggedConditions);
        } 


        private void PruneLogsWithoutBranch()
        {
            if(OutgoingLinksFromMap == null || logNodes == null) return;

            Queue toDelete = new Queue();
            for(int i = 0; i < logNodes.Count; i++)
            {
                string condition = (logNodes[i] as ILog).Condition;
                if(String.Compare(condition, ConstructionConst.LOG_ENTRY, true) == 0 || String.Compare(condition, ConstructionConst.LOG_EXIT, true) == 0)
                    continue;

                bool foundCondition = false;

                for(int j = 0; j < OutgoingLinksFromMap.Length; j++)
                {
                    if(String.Compare(OutgoingLinksFromMap[j].Condition, condition, true) == 0)
                        foundCondition = true;
                }
        
                if(!foundCondition)
                    toDelete.Enqueue(logNodes[i]);
            }

            for(int i = 0; i < toDelete.Count; i++)
            {
                logNodes.Remove(toDelete.Dequeue());
            }
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
    }
}

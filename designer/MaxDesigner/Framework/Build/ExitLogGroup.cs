using System;
using System.Collections;
using Metreos.ApplicationFramework.ScriptXml;


namespace Metreos.Max.Framework
{
    /// <summary>
    ///     The Designer auto-generates logging action nodes on the behalf of the user.
    ///     This action acts as the whole of log actions generated when the user defines
    ///     an 'exit' log.  A duplicate log must be generated for each exit condition
    ///     of the action, hence the 'group' of ExitLogs.
    /// </summary>
    public class ExitLogGroup : ICompileNode, ILog
    {
        #region ICompileNode Members
    
        public CompileLinkCollection OutgoingLinks { get { return outgoingLinks; } set { outgoingLinks = value; } }
        public CompileLinkCollection IncomingLinks { get { return incomingLinks; } set { incomingLinks = value; } }   
        public Link[] OutgoingLinksFromMap { get { return null; } }
        public Link[] IncomingLinksFromMap { get { return null; } }
        public long IdFromMap { get { return -1000; } }
        public long Id { get { return -1000; } }
        public ICompileNode GetEntryNode(Link outgoingLink) 
        { return GetExitNodeByCondition(outgoingLink.Condition); }
        public ICompileNode GetExitNode(Link incomingLink) 
        { return GetExitNodeByCondition(incomingLink.Condition); }

        public bool ContainsLink(Link link)
        { 
            System.Diagnostics.Debug.Assert(false, "REFACTOR");
            return false;
        }


        public virtual void AddIncomingLink(string condition, ICompileNode fromNode)
        {
            ExitLogNode exitLog = conditionToExitLog[condition] as ExitLogNode;
            incomingLinks.Add(new CompileLink(condition, exitLog, fromNode));
            exitLog.AddIncomingLink(condition, fromNode);
        }


        public virtual void AddOutgoingLink(string condition, ICompileNode toNode)
        {
            ExitLogNode exitLog = conditionToExitLog[condition] as ExitLogNode;
            outgoingLinks.Add(new CompileLink(condition, toNode, exitLog));
            exitLog.AddOutgoingLink(condition, toNode);
        }


        public virtual int Number(int number)
        {
            for(int i = 0; i < exitLogs.Count; i++)
            {
                ExitLogNode logNode = exitLogs[i] as ExitLogNode;
                number += logNode.Number(number);
            }

            return exitLogs.Count;
        }


        public virtual void Link(CompileNodeCollection allNodes)
        {
            for(int i = 0; i < allNodes.Count; i++)
            {
                if(allNodes[i] is MaxParentNode)
                {
                    MaxParentNode maxNode = allNodes[i] as MaxParentNode;

                    if(maxNode.MainAction != parentAction)
                        continue;

                    if(maxNode.UnloggedConditions != null)
                    {
                        foreach(string unloggedCondition in maxNode.UnloggedConditions)
                        {
                            (conditionToExitLog[unloggedCondition] as ExitLogNode).AddIncomingLink(unloggedCondition, parentAction);
                        }
                    }

                    IEnumerator logs = maxNode.LoggingNodes.GetEnumerator();
                    while(logs.MoveNext())
                    {
                        ILog log = logs.Current as ILog;
                        if(log is ExitLogGroup || log is EntryLogNode)
                            continue;

                        if(conditionToExitLog.Contains(log.Condition))
                            (conditionToExitLog[log.Condition] as ExitLogNode).AddIncomingLink(log.Condition, log as MetreosActionNode);
                    }
          
                    break;
                }
            }
        }
   

        public virtual void LinkByNumber()
        {
            for(int i = 0; i < exitLogs.Count; i++)
            {
                exitLogs[i].LinkByNumber();
            }
        }


        public virtual actionType[] RetrieveXml()
        {
            ArrayList actionBuilder = new ArrayList();

            for(int i = 0; i < exitLogs.Count; i++)
            {
                actionBuilder.AddRange(exitLogs[i].RetrieveXml());
            }

            actionType[] actions = new actionType[actionBuilder.Count];
            actionBuilder.CopyTo(actions);
            return actions;
        }

        #endregion

        public string Condition { get { return ConstructionConst.LOG_EXIT; } }
        public MetreosActionNode GetEntryNode(string condition) { return GetExitNodeByCondition(condition); }
        public MetreosActionNode GetExitNode(string condition) { return GetExitNodeByCondition(condition); }

        private ExitLogNode GetExitNodeByCondition(string condition)
        {
            return conditionToExitLog[condition] as ExitLogNode;
        }

        private CompileLinkCollection outgoingLinks;
        private CompileLinkCollection incomingLinks;
        private CompileNodeCollection exitLogs;
        private string[] conditions;
        private Hashtable conditionToExitLog;
        private MetreosActionNode parentAction;


        public ExitLogGroup(CompileNodeCollection exitLogs, string[] conditions, MetreosActionNode parentAction)
        {
            this.outgoingLinks = new CompileLinkCollection();
            this.incomingLinks = new CompileLinkCollection();
            this.exitLogs = exitLogs;
            this.conditions = conditions;
            this.parentAction = parentAction;
            this.conditionToExitLog = new Hashtable();
      
            LinkConnectionsToExitLogs();
        }


        private void LinkConnectionsToExitLogs()
        {
            for(int i = 0; i < conditions.Length; i++)
            {
                conditionToExitLog[conditions[i]] = exitLogs[i];
            }
        }   
    }
}

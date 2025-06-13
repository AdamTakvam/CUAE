using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;

using Metreos.Core;
using Metreos.LogSinks;
using Metreos.Configuration;
using Metreos.Interfaces;
using Metreos.MediaControl;
using Metreos.Messaging;
using Metreos.Utilities;

namespace MCPTest
{
    class Tester : PrimaryTaskBase
    {
        private const int DefNumThreads     = 1;
        private const bool DefUseAppDomain  = true;

        static void Main(string[] args)
        {
            int numThreads = DefNumThreads;
            if(args != null && args.Length > 0)
            {
                if(args[0] == "-h" || args[0] == "?")
                {
                    Console.WriteLine("Usage: MCPTest [numThreads]");
                    return;
                }
                else
                {
                    try { numThreads = Convert.ToInt32(args[0]); }
                    catch
                    {
                        Console.WriteLine("Error: Invalid number of threads specified");
                        return;
                    }
                }
            }

            Console.WriteLine("Be sure an app is installed on the local CUAE called '{0}' with the appropriate CRG settings prior to starting this test. The CUAE should be stopped.", AppName);
            Console.WriteLine();
            Console.WriteLine("Press <enter> to start or 'q' to quit");

            if(Console.ReadLine() == "q")
                return;

            using(ConsoleLoggerSink cls = new ConsoleLoggerSink(TraceLevel.Verbose))
            {
                using(Tester t = new Tester(numThreads, DefUseAppDomain))
                {
                    if(!t.Startup())
                        return;

                    string input = null;

                    while(input != "q")
                    {
                        Console.WriteLine("Press 'q' to quit.");
                        input = Console.ReadLine();
                    }

                    Console.WriteLine("Shutting down...");
                    t.Shutdown();
                }
            }
        }

        public const int ThreadSkewMs       = 2000;
        public const int CommandTimeout     = 2000;
        private const string AppName        = "InAndOut";
        private const string ScriptName     = "Script1";
        private const string PartitionName  = "default";

        private readonly MediaControlProvider mcp;
        private readonly MessageQueueWriter mcpWriter;
        private ManualResetEvent mcpStarted;
        private ManualResetEvent mcpStopped;
        private readonly ThreadInfo[] threads;
        private readonly Random rand;

        private int numCalls = 0;
        private long startTime;

        public Tester(int numThreads, bool useAppDomain)
            : base(IConfig.ComponentType.Core, "MCPTest", "MCP Tester", Config.Instance)
        {
            if(useAppDomain)
            {
                AppDomain childDomain = AppDomain.CreateDomain("MCP Domain");
                this.mcp = (MediaControlProvider) childDomain.CreateInstanceFromAndUnwrap("Metreos.MediaControl.dll", typeof(MediaControlProvider).FullName, true,
                    BindingFlags.CreateInstance, null, new object[] { Config.Instance }, null, null, null);
            }
            else
            {
                this.mcp = new MediaControlProvider(Config.Instance);
            }

            this.mcp.InitializeProvider(Metreos.LoggingFramework.Logger.Instance);
            this.mcp.SetRouterQueueWriter(taskQueue.GetWriter());
            MessageQueue mcpQ = mcp.GetMessageQueue() as MessageQueue;
            this.mcpWriter = mcpQ.GetWriter();

            this.startTime = HPTimer.Now();

            this.mcpStarted = new ManualResetEvent(false);
            this.mcpStopped = new ManualResetEvent(false);
            this.rand = new Random();

            this.threads = new ThreadInfo[numThreads];
            for(int i=0; i<numThreads; i++)
            {
                threads[i] = new ThreadInfo();
                threads[i].thread = new Thread(new ParameterizedThreadStart(TestThread));
                threads[i].thread.Name = "TestThread: " + i;
                threads[i].thread.IsBackground = true;
            }
        }

        /// <summary>Simulates a a script instance using media resources</summary>
        /// <remarks>ReserveConnection x 2 -> CreateConference -> JoinConference -> Wait -> DeleteConnection x 2 </remarks>
        private void TestThread(object state)
        {
            int threadIndex = Convert.ToInt32(state);
            long id = 0;

            Thread.Sleep(rand.Next(ThreadSkewMs));

            while(!shutdownRequested)
            {
                try
                {
                    GetMediaCaps(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                    continue;
                }

                try
                {
                    ReserveConnection1(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                    continue;
                }

                try
                {
                    ReserveConnection2(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);

                    try
                    {
                        DeleteConnection1(threadIndex, ref id);
                    }
                    catch { }
                    
                    continue;
                }

                try
                {
                    CreateConference(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                    
                    try
                    {
                        DeleteConnection1(threadIndex, ref id);
                        DeleteConnection2(threadIndex, ref id);
                    }
                    catch { }

                    continue;
                }

                try
                {
                    JoinConference(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);

                    try
                    {
                        DeleteConnection1(threadIndex, ref id);
                        DeleteConnection2(threadIndex, ref id);
                    }
                    catch { }

                    continue;
                }

                try
                {
                    DeleteConnection1(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                }

                try
                {
                    DeleteConnection2(threadIndex, ref id);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, e.Message);
                }

                ThreadInfo tInfo = threads[threadIndex];
                if(tInfo != null)
                    tInfo.Reset();

                Interlocked.Increment(ref numCalls);
            }
        }

        private void GetMediaCaps(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.GET_MEDIA_CAPS, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void ReserveConnection1(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.RESERVE_CONNECTION, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.TX_CODEC, IMediaControl.Codecs.G711u);
                msg.AddField(IMediaControl.Fields.TX_FRAMESIZE, 20);
                msg.AddField(IMediaControl.Fields.RX_CODEC, IMediaControl.Codecs.G711u);
                msg.AddField(IMediaControl.Fields.RX_FRAMESIZE, 20);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void ReserveConnection2(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.RESERVE_CONNECTION, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.TX_CODEC, IMediaControl.Codecs.G711u);
                msg.AddField(IMediaControl.Fields.TX_FRAMESIZE, 20);
                msg.AddField(IMediaControl.Fields.RX_CODEC, IMediaControl.Codecs.G711u);
                msg.AddField(IMediaControl.Fields.RX_FRAMESIZE, 20);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void CreateConference(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.CREATE_CONFERENCE, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.CONNECTION_ID, tInfo.connId1);
                msg.AddField(IMediaControl.Fields.TX_IP, "127.0.0.1");
                msg.AddField(IMediaControl.Fields.TX_PORT, 12345);
                msg.AddField(IMediaControl.Fields.HAIRPIN, true);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void JoinConference(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                // Join Conference
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.JOIN_CONFERENCE, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.CONNECTION_ID, tInfo.connId2);
                msg.AddField(IMediaControl.Fields.CONFERENCE_ID, tInfo.confId);
                msg.AddField(IMediaControl.Fields.TX_IP, "127.0.0.1");
                msg.AddField(IMediaControl.Fields.TX_PORT, 12345);
                msg.AddField(IMediaControl.Fields.HAIRPIN, true);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void DeleteConnection1(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.DELETE_CONNECTION, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.CONNECTION_ID, tInfo.connId1);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        private void DeleteConnection2(int threadIndex, ref long i)
        {
            ThreadInfo tInfo = threads[threadIndex];
            if(tInfo == null)
                throw new ApplicationException("Invalid thread ID: " + threadIndex);

            lock(tInfo.responseLock)
            {
                tInfo.currActionId = threadIndex + "." + i++;
                ActionMessage msg = 
                    messageUtility.CreateActionMessage(IMediaControl.Actions.DELETE_CONNECTION, tInfo.currActionId, AppName, ScriptName, PartitionName);
                msg.SessionGuid = tInfo.currActionId;
                msg.Locale = new System.Globalization.CultureInfo("en-US");
                msg.AddField(IMediaControl.Fields.CONNECTION_ID, tInfo.connId2);
                mcpWriter.PostMessage(msg);

                if(!Monitor.Wait(tInfo.responseLock, CommandTimeout))
                    throw new ApplicationException("Command timed out: " + msg);

                if(!tInfo.actionSuccess)
                    throw new ApplicationException("Action failed: " + msg);
            }
        }

        protected override bool HandleMessage(InternalMessage message)
        {
            ResponseMessage rMsg = message as ResponseMessage;
            if(rMsg == null)
            {
                if(message.MessageId == ICommands.REGISTER_PROV_NAMESPACE)
                    return true;

                log.Write(TraceLevel.Error, "Got non-response message: " + message.MessageId);
                return false;
            }

            if(rMsg.MessageId == IResponses.STARTUP_COMPLETE)
            {
                mcpStarted.Set();
            }
            else if((rMsg.MessageId == IResponses.SHUTDOWN_COMPLETE) ||
                    (rMsg.MessageId == IResponses.SHUTDOWN_FAILED))
            {
                mcpStopped.Set();
            }
            else
            {
                string[] ids = rMsg.InResponseToActionGuid.Split('.');
                if(ids == null || ids.Length != 2)
                {
                    log.Write(TraceLevel.Error, "Received response with invalid Action ID: " + rMsg);
                    return true;
                }

                int threadId = Convert.ToInt32(ids[0]);
                ThreadInfo tInfo = threads[threadId];
                if(tInfo == null)
                {
                    log.Write(TraceLevel.Error, "Invalid thread ID in response: " + rMsg);
                    return true;
                }

                lock(tInfo.responseLock)
                {
                    if(tInfo.currActionId == rMsg.InResponseToActionGuid)
                    {
                        tInfo.actionSuccess = rMsg.MessageId == IApp.VALUE_SUCCESS;

                        string connId = Convert.ToString(rMsg[IMediaControl.Fields.CONNECTION_ID]);
                        if(connId != null && connId != String.Empty)
                        {
                            if(tInfo.connId1 == null)
                                tInfo.connId1 = connId;
                            else if(tInfo.connId2 == null)
                                tInfo.connId2 = connId;
                        }

                        string confId = Convert.ToString(rMsg[IMediaControl.Fields.CONFERENCE_ID]);
                        if(confId != null && confId != String.Empty)
                            tInfo.confId = confId;

                        Monitor.Pulse(tInfo.responseLock);
                    }
                    else
                    {
                        log.Write(TraceLevel.Error, "Received response out of sequence: " + rMsg);
                    }
                }
            }

            return true;
        }

        public bool Startup()
        {
            try
            {
                OnStartup();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        protected override void OnStartup()
        {
            mcpStarted.Reset();

            CommandMessage msg = messageUtility.CreateCommandMessage(mcp.Name, ICommands.STARTUP);
            mcpWriter.PostMessage(msg);

            if(!mcpStarted.WaitOne(5000, false))
                throw new StartupFailedException("MCP failed to startup");

            for(int i=0; i<threads.Length; i++)
            {
                threads[i].thread.Start(i);
            }
        }

        public void Shutdown()
        {
            OnShutdown();
        }
       
        protected override void OnShutdown()
        {
            long testTime = HPTimer.SecondsSince(startTime);

            this.shutdownRequested = true;

            foreach(ThreadInfo t in threads)
            {
                t.Dispose();
            }

            mcpStopped.Reset();

            CommandMessage msg = messageUtility.CreateCommandMessage(mcp.Name, ICommands.SHUTDOWN);
            mcpWriter.PostMessage(msg);

            if(!mcpStopped.WaitOne(5000, false))
                Console.WriteLine("MCP failed to shutdown gracefully");
            
            mcp.Dispose();

            // Print stats
            Console.WriteLine();
            TimeSpan testTimeTS = new TimeSpan(0, 0, Convert.ToInt32(testTime));
            Console.WriteLine("Total test time: " + testTimeTS);
            Console.WriteLine("Number of InAndOut media connections: " + numCalls);
            Console.WriteLine("Average calls / second: " + (numCalls / (double) testTime));
        }

        protected override void RefreshConfiguration(string proxy)
        {
        }
    }
}

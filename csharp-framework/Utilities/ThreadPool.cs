// ThreadPool.cs
//
// This file defines a custom ThreadPool class that supports the following
// characteristics (property and method names shown in []):
//
// * can be explicitly started and stopped (and restarted) [Start,Stop,StopAndWait]
//
// * configurable thread priority [Priority]
//
// * configurable foreground/background characteristic [IsBackground]
//
// * configurable minimum thread count (called 'static' or 'permanent' threads) [constructor]
//
// * configurable maximum thread count (threads added over the minimum are
//   called 'dynamic' threads) [constructor, MaxThreadCount]
//
// * configurable dynamic thread creation trigger (the point at which
//   the pool decides to add new threads) [NewThreadTrigger]
//
// * configurable dynamic thread decay interval (the time period
//   after which an idle dynamic thread will exit) [DynamicThreadDecay]
//
// * configurable limit (optional) to the request queue size (by default unbounded) [RequestQueueLimit]
//
// * pool extends WaitHandle, becomes signaled when last thread exits [StopAndWait, WaitHandle methods]
//
// * operations enqueued to the pool are cancellable [IWorkRequest returned by PostRequest]
//
// * enqueue operation supports early bound approach (ala ThreadPool.QueueUserWorkItem)
//   as well as late bound approach (ala Control.Invoke/BeginInvoke) to posting work requests [PostRequest]
//
// * optional propogation of calling thread call context to target [PropogateCallContext]
//
// * optional propogation of calling thread principal to target [PropogateThreadPrincipal]
//
// * optional propogation of calling thread HttpContext to target [PropogateHttpContext]
//
// * support for started/stopped event subscription & notification [Started, Stopped]
//
// Known issues/limitations/comments:
//
// * The PropogateCASMarkers property exists for future support for propogating
//   the calling thread's installed CAS markers in the same way that the built-in thread
//   pool does.  Currently, there is no support for user-defined code to perform that
//   operation.
//
// * PropogateCallContext and PropogateHttpContext both use reflection against private
//   members to due their job.  As such, these two properties are set to false by default,
//   but do work on the first release of the framework (including .NET Server) and its
//   service packs.  These features have not been tested on Everett at this time.
//
// Mike Woodring
// http://staff.develop.com/woodring
//
using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Metreos.Utilities
{
    public delegate void LogDelegate(TraceLevel level, string message);
    public delegate void WorkRequestDelegate( object state );
    public delegate void ThreadPoolDelegate();

    #region IWorkRequest interface
    public interface IWorkRequest
    {
        bool Cancel();
    }
    #endregion

    #region ThreadPool class

    public sealed class ThreadPool : WaitHandle
    {
        #region ThreadPool constructors

        public ThreadPool( int initialThreadCount, int maxThreadCount, string poolName )
            : this( initialThreadCount, maxThreadCount, poolName,
                    DEFAULT_NEW_THREAD_TRIGGER_TIME,
                    DEFAULT_DYNAMIC_THREAD_DECAY_TIME,
                    DEFAULT_THREAD_PRIORITY,
                    DEFAULT_REQUEST_QUEUE_LIMIT )
        {
        }

        public ThreadPool( int initialThreadCount, int maxThreadCount, string poolName,
                           int newThreadTrigger, int dynamicThreadDecayTime,
                           ThreadPriority threadPriority, int requestQueueLimit )
        {
            SafeWaitHandle = stopCompleteEvent.SafeWaitHandle;

            if( maxThreadCount < initialThreadCount )
            {
                throw new ArgumentException("Maximum thread count must be >= initial thread count.", "maxThreadCount");
            }

            if( dynamicThreadDecayTime <= 0 )
            {
                throw new ArgumentException("Dynamic thread decay time cannot be <= 0.", "dynamicThreadDecayTime");
            }

            if( newThreadTrigger <= 0 )
            {
                throw new ArgumentException("New thread trigger time cannot be <= 0.", "newThreadTrigger");
            }
            
            this.initialThreadCount = initialThreadCount;
            this.maxThreadCount = maxThreadCount;
            this.requestQueueLimit = (requestQueueLimit < 0 ? DEFAULT_REQUEST_QUEUE_LIMIT : requestQueueLimit);
            this.decayTime = dynamicThreadDecayTime;
            this.newThreadTrigger = new TimeSpan(TimeSpan.TicksPerMillisecond * newThreadTrigger);
            this.threadPriority = threadPriority;
            this.requestQueue = new Queue(requestQueueLimit < 0 ? 4096 : requestQueueLimit);
            
            if( poolName == null )
            {
                throw new ArgumentNullException("poolName", "Thread pool name cannot be null");
            }
            else
            {
                this.threadPoolName = poolName;
            }
        }

        #endregion

        #region ThreadPool properties
        // The Priority & DynamicThreadDecay properties are not thread safe
        // and can only be set before Start is called.
        //
        public ThreadPriority Priority
        {
            get { return(threadPriority); }
            
            set
            {
                if( hasBeenStarted )
                {
                    throw new InvalidOperationException("Cannot adjust thread priority after pool has been started.");
                }

                threadPriority = value;
            }
        }

        public int DynamicThreadDecay
        {
            get { return(decayTime); }
            
            set
            {
                if( hasBeenStarted )
                {
                    throw new InvalidOperationException("Cannot adjust dynamic thread decay time after pool has been started.");
                }

                if( value <= 0 )
                {
                    throw new ArgumentException("Dynamic thread decay time cannot be <= 0.", "value");
                }

                decayTime = value;
            }
        }

        public int NewThreadTrigger
        {
            get { return((int)newThreadTrigger.TotalMilliseconds); }
            
            set
            {
                if( value <= 0 )
                {
                    throw new ArgumentException("New thread trigger time cannot be <= 0.", "value");
                }

                lock( this )
                {
                    newThreadTrigger = new TimeSpan(TimeSpan.TicksPerMillisecond * value);
                }
            }
        }

        public int RequestQueueLimit
        {
            get { return(requestQueueLimit); }
            set { requestQueueLimit = (value < 0 ? DEFAULT_REQUEST_QUEUE_LIMIT : value); }
        }

        public int MaxThreads
        {
            get { return(maxThreadCount); }

            set
            {
                if( value < initialThreadCount )
                {
                    throw new ArgumentException("Maximum thread count must be >= initial thread count.", "MaxThreads");
                }

                maxThreadCount = value;
            }
        }

        public bool IsStarted
        {
            get { return(hasBeenStarted); }
        }

        public bool PropogateThreadPrincipal
        {
            get { return(propogateThreadPrincipal); }
            set { propogateThreadPrincipal = value; }
        }

        public bool PropogateCallContext
        {
            get { return(propogateCallContext); }
            set { propogateCallContext = value; }
        }

        public bool PropogateHttpContext
        {
            get { return(propogateHttpContext); }
            set { propogateHttpContext = value; }
        }

        public bool PropogateCASMarkers
        {
            get { return(propogateCASMarkers); }

            // When CompressedStack get/set is opened up,
            // add the following setter back in.
            //
            // set { propogateCASMarkers = value; }
        }

        public bool IsBackground
        {
            get { return(useBackgroundThreads); }
            
            set
            {
                if( hasBeenStarted )
                {
                    throw new InvalidOperationException("Cannot adjust background status after pool has been started.");
                }

                useBackgroundThreads = value;
            }
        }
        #endregion

        #region ThreadPool events

        public event ThreadPoolDelegate Started;
        public event ThreadPoolDelegate Stopped;
        public event LogDelegate MessageLogged;
        
        #endregion

        public void Start()
        {
            lock( this )
            {
                if( hasBeenStarted )
                {
                    throw new InvalidOperationException("Pool has already been started.");
                }

                hasBeenStarted = true;

                WriteLog(TraceLevel.Verbose, "New thread pool '{0}' starting:\n"
                    + "  Initial thread count:      {1}\n"
                    + "  Max thread count:          {2}\n"
                    + "  New thread trigger:        {3} ms\n"
                    + "  Dynamic thread decay time: {4} ms\n"
                    + "  Request queue limit:       {5} entries",
                    threadPoolName, initialThreadCount, maxThreadCount,
                    newThreadTrigger.TotalMilliseconds, decayTime, requestQueueLimit);

                for( int n = 0; n < initialThreadCount; n++ )
                {
                    ThreadWrapper thread =
                        new ThreadWrapper( this, true, threadPriority,
                                        string.Format("{0} (static)", threadPoolName) );
                    thread.Start();
                }

                if( Started != null )
                {
                    Started(); // TODO: reconsider firing this event while holding the lock...
                }
            }
        }
        
        #region ThreadPool.Stop and InternalStop

        public void Stop()
        {
            InternalStop(false, Timeout.Infinite);
        }

        public void StopAndWait()
        {
            InternalStop(true, Timeout.Infinite);
        }

        public bool StopAndWait( int timeout )
        {
            return InternalStop(true, timeout);
        }

        private bool InternalStop( bool wait, int timeout )
        {
            if( !hasBeenStarted )
            {
                throw new InvalidOperationException("Cannot stop a thread pool that has not been started yet.");
            }

            lock(this)
            {
                WriteLog(TraceLevel.Verbose, "[{0}, {1}] Stopping pool (# threads = {2})",
                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name, _currentThreadCount);
                stopInProgress = true;
                Monitor.PulseAll(this);
            }

            if( wait )
            {
                bool stopComplete = WaitOne(timeout, true);

                if( stopComplete )
                {
                    // If the stop was successful, we can support being
                    // to be restarted.  If the stop was requested, but not
                    // waited on, then we don't support restarting.
                    //
                    hasBeenStarted = false;
                    stopInProgress = false;
                    requestQueue.Clear();
                    stopCompleteEvent.Reset();
                }

                return(stopComplete);
            }

            return(true);
        }

        #endregion

        #region Thread Metrics

        public int IncrementCurrentThreadCount()
        {
			lock(this)
			{
				_currentThreadCount += 1;
				Debug.Assert(_currentThreadCount <= maxThreadCount, "_currentThreadCount <= maxThreadCount");
				return _currentThreadCount;
			}
        }

        public int DecrementCurrentThreadCount()
        {
			lock(this)
			{
				_currentThreadCount -= 1;
				Debug.Assert(_currentThreadCount >= 0, "_currentThreadCount >= 0");
				Debug.Assert(_currentThreadsBusy <= _currentThreadCount, "_currentThreadsBusy <= _currentThreadCount");
				return _currentThreadCount;
			}
        }

        public int IncrementBusy()
        {
			lock(this)
			{
				_currentThreadsBusy += 1;
				Debug.Assert(_currentThreadsBusy <= _currentThreadCount, "_currentThreadsBusy <= _currentThreadCount");
				return _currentThreadsBusy;
			}
        }

        public int DecrementBusy()
        {
			lock(this)
			{
				_currentThreadsBusy -= 1;
				Debug.Assert(_currentThreadsBusy >= 0, "_currentThreadsBusy >= 0");
				return _currentThreadsBusy;
			}
		}

		public int AvailableThreads
		{
			get { return(maxThreadCount - _currentThreadCount); }
		}

		public bool AllThreadsBusy
		{
			get { return(_currentThreadsBusy == _currentThreadCount); }
		}

		private bool CanUseAnotherThread()
		{
			// called with lock(this) in effect.

			// i would assert that busy can never be greater than thread count.
			// what this code is trying to figure out is 1) if all threads are
			// currently busy and 2) can we make another thread?
			
			return AllThreadsBusy && (AvailableThreads > 0);
		}

        #endregion

        #region ThreadPool.PostRequest(early bound)

        // Overloads for the early bound WorkRequestDelegate-based targets.
        //
        public bool PostRequest( WorkRequestDelegate cb )
        {
            return PostRequest(cb, (object)null);
        }

        public bool PostRequest( WorkRequestDelegate cb, object state )
        {
            IWorkRequest notUsed;
            return PostRequest(cb, state, out notUsed);
        }

        public bool PostRequest( WorkRequestDelegate cb, object state, out IWorkRequest reqStatus )
        {
            WorkRequest request =
                new WorkRequest( cb, state,
                                 propogateThreadPrincipal, propogateCallContext,
                                 propogateHttpContext, propogateCASMarkers );
            reqStatus = request;
            return PostRequest(request);
        }

        #endregion

        #region ThreadPool.PostRequest(late bound)

        // Overloads for the late bound Delegate.DynamicInvoke-based targets.
        //
        public bool PostRequest( Delegate cb, object[] args )
        {
            IWorkRequest notUsed;
            return PostRequest(cb, args, out notUsed);
        }

        public bool PostRequest( Delegate cb, object[] args, out IWorkRequest reqStatus )
        {
            WorkRequest request =
                new WorkRequest( cb, args,
                                 propogateThreadPrincipal, propogateCallContext,
                                 propogateHttpContext, propogateCASMarkers );
            reqStatus = request;
            return PostRequest(request);
        }

        #endregion

        // The actual implementation of PostRequest.
        //
        bool PostRequest( WorkRequest request )
        {
            lock(this)
            {
                // A requestQueueLimit of -1 means the queue is "unbounded"
                // (subject to available resources).  IOW, no artificial limit
                // has been placed on the maximum # of requests that can be
                // placed into the queue.
                //
                if( (requestQueueLimit == -1) || (requestQueue.Count < requestQueueLimit) )
                {
                    if(CanUseAnotherThread())
                    {
                        // Note - the constructor for ThreadWrapper will update currentThreadCount.
                        ThreadWrapper newThread =
                            new ThreadWrapper( this, false, threadPriority,
                            string.Format("{0} (dynamic)", threadPoolName) );

                        WriteLog(TraceLevel.Verbose, "[{0}, {1}] Adding dynamic thread to pool",
                            Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);

                        newThread.Start();
                    }

                    try
                    {
                        requestQueue.Enqueue(request);
                        Monitor.Pulse(this);
                        return(true);
                    }
                    catch
                    {
                    }
                }
            }

            return(false);
        }

        #region Log Writing

        private void WriteLog(TraceLevel level, string format, params object[] args)
        {
            WriteLog(level, String.Format(format, args));
        }

        private void WriteLog(TraceLevel level, string message)
        {
            if(MessageLogged != null)
                MessageLogged(level, message);
        }

        #endregion

        #region Private ThreadPool constants

        // Default parameters.
        //
        const int DEFAULT_DYNAMIC_THREAD_DECAY_TIME = 5 /* minutes */ * 60 /* sec/min */ * 1000 /* ms/sec */;
        const int DEFAULT_NEW_THREAD_TRIGGER_TIME = 500; // milliseconds
        const ThreadPriority DEFAULT_THREAD_PRIORITY = ThreadPriority.Normal;
        const int DEFAULT_REQUEST_QUEUE_LIMIT = -1; // unbounded
        
        #endregion

        #region Private ThreadPool member variables

        private bool                hasBeenStarted = false;
        private bool                stopInProgress = false;
        private readonly string     threadPoolName;
        private readonly int        initialThreadCount;     // Initial # of threads to create (called "static threads" in this class).
        private int                 maxThreadCount;         // Cap for thread count.  Threads added above initialThreadCount are called "dynamic" threads.
        private int                 _currentThreadCount = 0; // Current # of threads in the pool (static + dynamic).
        private int                 _currentThreadsBusy = 0; // Current # of threads currently handling WorkRequests.
        private int                 decayTime;              // If a dynamic thread is idle for this period of time w/o processing work requests, it will exit.
        private TimeSpan            newThreadTrigger;       // If a work request sits in the queue this long before being processed, a new thread will be added to queue up to the max.
        private ThreadPriority      threadPriority;
        private ManualResetEvent    stopCompleteEvent = new ManualResetEvent(false); // Signaled after Stop called and last thread exits.
        private Queue               requestQueue;
        private int                 requestQueueLimit;      // Throttle for maximum # of work requests that can be added.
        private bool                useBackgroundThreads = true;
        private bool                propogateThreadPrincipal = false;
        private bool                propogateCallContext = false;
        private bool                propogateHttpContext = false;
        private bool                propogateCASMarkers = false;
        
        #endregion

        #region ThreadPool.ThreadInfo

        class ThreadInfo
        {
            public static ThreadInfo Capture( bool propogateThreadPrincipal, bool propogateCallContext,
                                              bool propogateHttpContext, bool propogateCASMarkers )
            {
                return new ThreadInfo( propogateThreadPrincipal, propogateCallContext,
                                       propogateHttpContext, propogateCASMarkers );
            }

            public static ThreadInfo Impersonate( ThreadInfo ti )
            {
                if( ti == null ) throw new ArgumentNullException("ti");

                ThreadInfo prevInfo = Capture(true, true, true, true);
                Restore(ti);
                return(prevInfo);
            }

            public static void Restore( ThreadInfo ti )
            {
                if( ti == null ) throw new ArgumentNullException("ti");

                // Restore call context.
                //
                if( miSetLogicalCallContext != null )
                {
                    miSetLogicalCallContext.Invoke(Thread.CurrentThread, new object[]{ti.callContext});
                }

                // Restore HttpContext with the moral equivalent of
                // HttpContext.Current = ti.httpContext;
                //
                CallContext.SetData(HttpContextSlotName, ti.httpContext);
                
                // Restore thread identity.  It's important that this be done after
                // restoring call context above, since restoring call context also
                // overwrites the current thread principal setting.  If propogateCallContext
                // and propogateThreadPrincipal are both true, then the following is redundant.
                // However, since propogating call context requires the use of reflection
                // to capture/restore call context, I want that behavior to be independantly
                // switchable so that it can be disabled; while still allowing thread principal
                // to be propogated.  This also covers us in the event that call context
                // propogation changes so that it no longer propogates thread principal.
                //
                Thread.CurrentPrincipal = ti.principal;
                
                if( ti.compressedStack != null )
                {
                    // TODO: Uncomment the following when Thread.SetCompressedStack is no longer guarded
                    //       by a StrongNameIdentityPermission.
                    //
                    // Thread.CurrentThread.SetCompressedStack(ti.compressedStack);
                }
            }

            private ThreadInfo( bool propogateThreadPrincipal, bool propogateCallContext,
                                bool propogateHttpContext, bool propogateCASMarkers )
            {
                if( propogateThreadPrincipal )
                {
                    principal = Thread.CurrentPrincipal;
                }

                if( propogateHttpContext )
                {
                    httpContext = HttpContext.Current;
                }
                
                if( propogateCallContext && (miGetLogicalCallContext != null) )
                {
                    callContext = (LogicalCallContext)miGetLogicalCallContext.Invoke(Thread.CurrentThread, null);
                    callContext = (LogicalCallContext)callContext.Clone();

                    // TODO: consider serialize/deserialize call context to get a MBV snapshot
                    //       instead of leaving it up to the Clone method.
                }

                if( propogateCASMarkers )
                {
                    // TODO: Uncomment the following when Thread.GetCompressedStack is no longer guarded
                    //       by a StrongNameIdentityPermission.
                    //
                    // compressedStack = Thread.CurrentThread.GetCompressedStack();
                }
            }

            IPrincipal principal;
            LogicalCallContext callContext;
            CompressedStack compressedStack = null; // Always null until Get/SetCompressedStack are opened up.
            HttpContext httpContext;

            // Cached type information.
            //
            const BindingFlags bfNonPublicInstance = BindingFlags.Instance | BindingFlags.NonPublic;
            const BindingFlags bfNonPublicStatic = BindingFlags.Static | BindingFlags.NonPublic;

            static MethodInfo miGetLogicalCallContext =
                    typeof(Thread).GetMethod("GetLogicalCallContext", bfNonPublicInstance);

            static MethodInfo miSetLogicalCallContext =
                    typeof(Thread).GetMethod("SetLogicalCallContext", bfNonPublicInstance);

            static string HttpContextSlotName;

            static ThreadInfo()
            {
                // Lookup the value of HttpContext.CallContextSlotName (if it exists)
                // to see what the name of the call context slot is where HttpContext.Current
                // is stashed.  As a fallback, if this field isn't present anymore, just
                // try for the original "HttpContext" slot name.
                //
                FieldInfo fi = typeof(HttpContext).GetField("CallContextSlotName", bfNonPublicStatic);

                if( fi != null )
                {
                    HttpContextSlotName = (string)fi.GetValue(null);
                }
                else
                {
                    HttpContextSlotName = "HttpContext";
                }
            }
        }

        #endregion

        #region ThreadPool.WorkRequest

        class WorkRequest : IWorkRequest
        {
            internal const int PENDING = 0;
            internal const int PROCESSED = 1;
            internal const int CANCELLED = 2;

            public WorkRequest( WorkRequestDelegate cb, object arg,
                                bool propogateThreadPrincipal, bool propogateCallContext,
                                bool propogateHttpContext, bool propogateCASMarkers )
            {
                targetProc = cb;
                procArg = arg;
                procArgs = null;

                Initialize( propogateThreadPrincipal, propogateCallContext,
                            propogateHttpContext, propogateCASMarkers );
            }

            public WorkRequest( Delegate cb, object[] args,
                                bool propogateThreadPrincipal, bool propogateCallContext,
                                bool propogateHttpContext, bool propogateCASMarkers )
            {
                targetProc = cb;
                procArg = null;
                procArgs = args;

                Initialize( propogateThreadPrincipal, propogateCallContext,
                            propogateHttpContext, propogateCASMarkers );
            }

            void Initialize( bool propogateThreadPrincipal, bool propogateCallContext,
                             bool propogateHttpContext, bool propogateCASMarkers )
            {
                threadInfo = ThreadInfo.Capture( propogateThreadPrincipal, propogateCallContext,
                                                 propogateHttpContext, propogateCASMarkers );
            }

            public bool Cancel()
            {
                // If the work request was pending, mark it cancelled.  Otherwise,
                // this method was called too late.  Note that this call can
                // cancel an operation without any race conditions.  But if the
                // result of this test-and-set indicates the request is in the
                // "processed" state, it might actually be about to be processed.
                //
                return(Interlocked.CompareExchange(ref state, CANCELLED, PENDING) == PENDING);
            }

            internal Delegate       targetProc;         // Function to call.
            internal object         procArg;            // State to pass to function.
            internal object[]       procArgs;           // Used with Delegate.DynamicInvoke.
            internal ThreadInfo     threadInfo;         // Everything we know about a thread.
            internal int            state = PENDING;    // The state of this particular request.
        }

        #endregion

        #region ThreadPool.ThreadWrapper

        class ThreadWrapper
        {
            ThreadPool      pool;
            bool            isPermanent;
            ThreadPriority  priority;
            string          name;
            
            public ThreadWrapper( ThreadPool pool, bool isPermanent,
                                  ThreadPriority priority, string name )
            {
                this.pool = pool;
                this.isPermanent = isPermanent;
                this.priority = priority;
                this.name = name;

                lock( pool )
                {
                    // Update the total # of threads in the pool.
                    pool.IncrementCurrentThreadCount();
                }
            }

            public void Start()
            {
                Thread t = new Thread(new ThreadStart(ThreadProc));
                t.SetApartmentState(ApartmentState.MTA);
                t.Name = name;
                t.Priority = priority;
                t.IsBackground = pool.useBackgroundThreads;
                t.Start();
            }

            void ThreadProc()
            {
                pool.WriteLog(TraceLevel.Verbose, "[{0}, {1}] Worker thread started",
                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);

                bool done = false;

                while( !done )
                {
                    WorkRequest wr = null;

                    lock( pool )
                    {
                        // As long as the request queue is empty and a shutdown hasn't
                        // been initiated, wait for a new work request to arrive.
                        //
                        bool timedOut = false;

                        while( !pool.stopInProgress && !timedOut && (pool.requestQueue.Count == 0) )
                        {
                            if( !Monitor.Wait(pool, (isPermanent ? Timeout.Infinite : pool.decayTime)) )
                            {
                                // Timed out waiting for something to do.  Only dynamically created
                                // threads will get here, so bail out.
                                //
                                timedOut = true;
                            }
                        }

                        // We exited the loop above because one of the following conditions
                        // was met:
                        //   - ThreadPool.Stop was called to initiate a shutdown.
                        //   - A dynamic thread timed out waiting for a work request to arrive.
                        //   - There are items in the work queue to process.

                        // If we exited the loop because there's work to be done,
                        // a shutdown hasn't been initiated, and we aren't a dynamic thread
                        // that timed out, pull the request off the queue and prepare to
                        // process it.
                        //
                        if( !pool.stopInProgress && !timedOut && (pool.requestQueue.Count > 0) )
                        {
                            wr = (WorkRequest)pool.requestQueue.Dequeue();
                            Debug.Assert(wr != null, "WorkRequest is null in ThreadPool");
                        }
                        else
                        {
                            // Should only get here if this is a dynamic thread that
                            // timed out waiting for a work request, or if the pool
                            // is shutting down.
                            //
                            Debug.Assert((timedOut && !isPermanent) || pool.stopInProgress, "ThreadPool is in invalid state");
                            int count = pool.DecrementCurrentThreadCount();

                            if( count == 0 )
                            {
                                // Last one out turns off the lights.
                                //
                                Debug.Assert(pool.stopInProgress, "ThreadPool is in invalid state");

                                if( pool.Stopped != null )
                                {
                                    pool.Stopped();
                                }

                                pool.stopCompleteEvent.Set();
                            }

                            done = true;
                        }
                    } // lock

                    // No longer holding pool lock here...

                    if( !done && (wr != null) )
                    {
                        // Check to see if this request has been cancelled while
                        // stuck in the work queue.
                        //
                        // If the work request was pending, mark it processed and proceed
                        // to handle.  Otherwise, the request must have been cancelled
                        // before we plucked it off the request queue.
                        //
                        if( Interlocked.CompareExchange(ref wr.state, WorkRequest.PROCESSED, WorkRequest.PENDING) != WorkRequest.PENDING )
                        {
                            // Request was cancelled before we could get here.
                            // Bail out.
                            continue;
                        }

                        // Dispatch the work request.
                        //
                        ThreadInfo originalThreadInfo = null;

                        try
                        {
                            // Increment the number of threads currently busy in the pool
                            pool.IncrementBusy();

                            // Impersonate (as much as possible) what we know about
                            // the thread that issued the work request.
                            originalThreadInfo = ThreadInfo.Impersonate(wr.threadInfo);

                            WorkRequestDelegate targetProc = wr.targetProc as WorkRequestDelegate;

                            if( targetProc != null )
                            {
                                targetProc(wr.procArg);
                            }
                            else
                            {
                                wr.targetProc.DynamicInvoke(wr.procArgs);
                            }
                        }
                        catch( Exception e )
                        {
                            pool.WriteLog(TraceLevel.Error, "Exception thrown performing callback: {0}",
                                    Exceptions.FormatException(e));
                        }
                        finally
                        {
                            // Restore our worker thread's identity.
                            ThreadInfo.Restore(originalThreadInfo);

                            // Decrement the number of threads busy in the pool
                            pool.DecrementBusy();
                        }
                    }
                }

                pool.WriteLog(TraceLevel.Verbose, "[{0}, {1}] Worker thread exiting pool",
                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
            }
        }

        #endregion
    }
    #endregion

}

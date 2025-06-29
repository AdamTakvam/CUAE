using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Core.IPC.Flatmaps.LogServer
{
	#region Message Queue
	/// One Lock Bounded Blocking Queue (e.g. Bounded Buffer).
	/// This queue is internally synchronized (thread-safe) and designed for one-many producers and one-many
	/// consumer threads.  This is ideal for pipelining or other consumer/producer needs.
	/// Fast and thread safe on single or multiple cpu machines.
	/// 
	/// Consumer thread(s) will block on Dequeue operations until another thread performs a Enqueue
	/// operation, at which point the first scheduled consumer thread will be unblocked and get the
	/// current object.  Producer thread(s) will block on Enqueue operations until another
	/// consumer thread calls Dequeue to free a queue slot, at which point the first scheduled producer
	/// thread will be unblocked to finish its Enqueue operation.  No user code is needed to
	/// handle this "ping-pong" between locking/unlocking consumers and producers. 
	/// </summary>
	public sealed class ServerLogQueue : ICollection
	{
		#region Fields
		private object[] buffer;			// Buffer used to store queue objects with max "Size".
		private int count;					// Current number of elements in the queue.
		private int size;					// Max number of elements queue can hold without blocking.
		private int head;					// Index of slot for object to remove on next Dequeue. 
		private int tail;					// Index of slot for next Enqueue object.
		private readonly object syncRoot;	// Object used to synchronize the queue.
		#endregion

		#region Constructors
		/// <summary>
		/// Create instance of Queue with Bounded number of elements.  After that
		/// many elements are used, another Enqueue operation will "block" or wait
		/// until a Consumer calls Dequeue to free a slot.  Likewise, if the queue
		/// is empty, a call to Dequeue will block until another thread calls
		/// Enqueue.
		/// </summary>
		/// <param name="size"></param>
		public ServerLogQueue(int size)
		{
			if ( size < 1 )
				throw new ArgumentOutOfRangeException("size must be greater then zero.");
			syncRoot = new object();
			this.size = size;
			buffer = new object[size];
			count = 0;
			head = 0;
			tail = 0;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the object values currently in the queue.  If queue is empty, this
		/// will return a zero length array.  The returned array length can be
		/// 0 to Size.  This method does not modify the queue, but returns a shallow copy
		/// of the queue buffer containing the objects contained in the queue.
		/// </summary>
		public object[] Values
		{
			get
			{
				// Copy used elements to a new array of "count" size.  Note a simple
				// Buffer copy will not work as head could be anywhere and we want
				// a zero based array.
				object[] values;
				lock(syncRoot)
				{
					values = new object[count];
					int pos = head;
					for(int i = 0; i < count; i++)
					{
						values[i] = buffer[pos];
						pos = (pos + 1) % size;
					}
				}
				return values;
			}
		}

		#endregion

		#region buffer methods

		/// <summary>
		/// Add the value to the queue if possible.
		/// </summary>
		/// <param name="value">the value to add.</param>
		/// <returns>true if the value was added.</returns>
		private bool DoEnqueue(object value)
		{
			if (count < size)
			{
				buffer[tail] = value;
				tail = (tail + 1) % size;
				count++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Remove a value from the queue if possible.
		/// </summary>
		/// <returns>the value if there was one, null otherwise.</returns>
		private object DoDequeue()
		{
			if (count > 0)
			{
				object value = buffer[head];
				buffer[head] = null;
				head = (head + 1) % size;
				count--;
				return value;
			}
			return null;
		}

		private object DoPeek()
		{
			if (count > 0)
				return buffer[head];
			
			return null;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds an object to the end of the queue. If queue is full, this method will
		/// block until another thread calls one of the Dequeue methods.  This method will wait
		/// "Timeout.Infinite" until queue has a free slot.
		/// </summary>
		/// <param name="value"></param>
		public void Enqueue(object value)
		{
			Enqueue(value, Timeout.Infinite);
		}

		/// <summary>
		/// Adds an object to the end of the queue. If queue is full, this method will
		/// block until another thread calls one of the Dequeue methods or millisecondsTimeout
		/// expires.  If timeout, method will throw QueueTimeoutException.
		/// </summary>
		/// <param name="value"></param>
		public void Enqueue(object value, int millisecondsTimeout)
		{
			lock(syncRoot)
			{
				while(!DoEnqueue(value))
					if ( ! Monitor.Wait(syncRoot, millisecondsTimeout) )
						throw new QueueTimeoutException();

//				while(count == size)
//				{
//					try
//					{
//						if ( ! Monitor.Wait(syncRoot, millisecondsTimeout) )
//							throw new QueueTimeoutException();
//					}
//					catch
//					{
//						// Monitor exited with exception.  Could be owner thread of monitor
//						// object was terminated or timeout on wait.  Pulse any/all waiting
//						// threads to ensure we don't get any "live locked" producers.
//						Monitor.PulseAll(syncRoot);
//						throw;
//					}
//				}
//				buffer[tail] = value;
//				tail = (tail + 1) % size;
//				count++;
				
				if ( WasEmpty() )	// Could have blocking Dequeue thread(s).
					Monitor.PulseAll(syncRoot);
			}
		}

		/// <summary>
		/// Non-blocking version of Enqueue().  If queue is full then dequeue before enqueue.
		/// </summary>
		/// <param name="value"></param>
		public void TryEnqueue(object value)
		{
			lock(syncRoot)
			{
				while (!DoEnqueue(value))
				{
					DoDequeue(); // toss the oldest message
					tossed++;
				}

//				if ( count == size )
//				{
//					// toss the oldest message
//					object obj;
//					obj = buffer[head];
//					buffer[head] = null;
//					head = (head + 1) % size;
//					count--;
//				}
//				buffer[tail] = value;
//				tail = (tail + 1) % size;
//				count++;
				
				if ( WasEmpty() )	// Could have blocking Dequeue thread(s).
					Monitor.PulseAll(syncRoot);
			}
		}

		private int tossed = 0;

		private bool WasEmpty()
		{
			return count == 1;
		}

		/// <summary>
		/// Removes and returns the object at the beginning of the Queue.
		/// If queue is empty, method will block until another thread calls one of
		/// the Enqueue methods.   This method will wait "Timeout.Infinite" until another
		/// thread Enqueues and object.
		/// </summary>
		/// <returns></returns>
		public object Dequeue()
		{
			return Dequeue(Timeout.Infinite);
		}

		/// <summary>
		/// Removes and returns the object at the beginning of the Queue.
		/// If queue is empty, method will block until another thread calls one of
		/// the Enqueue methods or millisecondsTimeout expires.
		/// If timeout, method will throw QueueTimeoutException.
		/// </summary>
		/// <returns>The object that is removed from the beginning of the Queue.</returns>
		public object Dequeue(int millisecondsTimeout)
		{
			lock(syncRoot)
			{
				object value;
				
				while ((value = DoDequeue()) == null)
					if ( ! Monitor.Wait(syncRoot, millisecondsTimeout) )
						throw new QueueTimeoutException();
				
//				while(count == 0)
//				{
//					try
//					{
//						if ( ! Monitor.Wait(syncRoot, millisecondsTimeout) )
//							throw new QueueTimeoutException();
//					}
//					catch
//					{
//						Monitor.PulseAll(syncRoot);
//						throw;
//					}
//				}
//				value = buffer[head];
//				buffer[head] = null;
//				head = (head + 1) % size;
//				count--;

				if ( WasFull() )	// Could have blocking Enqueue thread(s).
					Monitor.PulseAll(syncRoot);

				return value;
			}
		}

		/// <summary>
		/// Non-blocking version of Dequeue.
		/// </summary>
		/// <returns>The object that is removed from the beginning of the Queue or null if empty.</returns>
		public object TryDequeue()
		{
			lock(syncRoot)
			{
				object value = DoDequeue();
				
				if (value == null)
					return null;

//				if ( count == 0 )
//				{
//					value = null;
//					return false;
//				}
//				value = buffer[head];
//				buffer[head] = null;
//				head = (head + 1) % size;
//				count--;
				
				if ( WasFull() )	// Could have blocking Enqueue thread(s).
					Monitor.PulseAll(syncRoot);
				
				return value;
			}
		}

		/// <summary>
		/// Removes this object if it is still at the head of the queue.
		/// See TryPeekWait.
		/// </summary>
		/// <param name="value">the value previously returned by Peek, TryPeek, or TryPeekWait.</param>
		/// <param name="tossed">decrements the tossed message count by this value.</param>
		public void DequeueIf(object value, int tossed)
		{
			lock(syncRoot)
			{
				this.tossed -= tossed;

				if (value != DoPeek())
					return;
                
				DoDequeue();
				
				if ( WasFull() )	// Could have blocking Enqueue thread(s).
					Monitor.PulseAll(syncRoot);
			}
		}

		private bool WasFull()
		{
			return count == (size - 1);
		}

		/// <summary>
		/// Returns the object at the beginning of the queue without removing it.
		/// </summary>
		/// <returns>The object at the beginning of the queue.</returns>
		/// <remarks>
		/// This method is similar to the Dequeue method, but Peek does not modify the queue. 
		/// A null reference can be added to the Queue as a value. 
		/// To distinguish between a null value and the end of the queue, check the Count property or
		/// catch the InvalidOperationException, which is thrown when the Queue is empty.
		/// </remarks>
		/// <exception cref="InvalidOpertionException">The queue is empty.</exception>
		public object Peek()
		{
			lock(syncRoot)
			{
				object value = DoPeek();

				if ( value == null )
					throw new InvalidOperationException("The Queue is empty.");
				
				return value;
			}
		}

		/// <summary>
		/// Returns the object at the beginning of the Queue without removing it.
		/// Similar to the Peek method, however this method will not throw exception if
		/// queue is empty, but instead will return false.
		/// </summary>
		/// <param name="value">The object at the beginning of the Queue or null if empty.</param>
		/// <returns>The object at the beginning of the Queue.</returns>
		public object TryPeek()
		{
			lock(syncRoot)
			{
				return DoPeek();
			}
		}

		/// <summary>
		/// Attempts to get an entry from the head of the queue, waits
		/// a specified amount of time for any to arrive if the queue
		/// is empty, and also returns the number of tossed entries
		/// before this one.
		/// </summary>
		/// <param name="millisecondsTimeout"></param>
		/// <param name="tossed"></param>
		/// <returns></returns>
		public object TryPeekWait(int millisecondsTimeout, out int tossed)
		{
			lock(syncRoot)
			{
				object value;
				
				while((value = DoPeek()) == null)
				{
					if (!Monitor.Wait(syncRoot, millisecondsTimeout))
					{
						tossed = 0;
						return null;
					}
				}

				tossed = this.tossed;
				return value;
			}
		}

		/// <summary>
		/// Removes all objects from the Queue.
		/// </summary>
		/// <remarks>
		/// Count is set to zero. Size does not change.
		/// </remarks>
		public void Clear()
		{
			lock(syncRoot)
			{
				while (DoDequeue() != null)
				{
					// nothing else to do.
				}
				this.tossed = 0;
			}
		}

		#endregion

		#region ICollection Members
		/// <summary>
		/// Gets a value indicating whether access to the Queue is synchronized (thread-safe).
		/// </summary>
		public bool IsSynchronized
		{
			get	{ return true; }
		}

		/// <summary>
		/// Returns the max elements allowed in the queue before blocking Enqueue
		/// operations.  This is the size set in the constructor.
		/// </summary>
		public int Size
		{
			get { return this.size;	}
		}

		/// <summary>
		/// Gets the number of elements contained in the Queue.
		/// </summary>
		public int Count
		{
			get	{ lock(syncRoot) { return count; } }
		}

		/// <summary>
		/// Copies the Queue elements to an existing one-dimensional Array,
		/// starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from Queue. The Array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which copying begins. </param>
		public void CopyTo(Array array, int index)
		{
			object[] tmpArray = Values;
			tmpArray.CopyTo(array, index);
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the Queue.
		/// </summary>
		public object SyncRoot
		{
			get	{ return this.syncRoot; }
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// GetEnumerator not implemented.  You can't enumerate the active queue
		/// as you would an array as it is dynamic with active gets and puts.  You could
		/// if you locked it first and unlocked after enumeration, but that does not
		/// work well for GetEnumerator.  The recommended method is to Get Values
		/// and enumerate the returned array copy.  That way the queue is locked for
		/// only a short time and a copy returned so that can be safely enumerated using
		/// the array's enumerator.  You could also create a custom enumerator that would
		/// dequeue the objects until empty queue, but that is a custom need. 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException("Not Implemented.");
		}
		#endregion
	} // End BlockingQueue

	public class QueueTimeoutException : Exception
	{
		public QueueTimeoutException() : base("Queue method timed out on wait.")
		{
		}
	}	
	#endregion

	#region Flatmap Helper Classes

	public class IntroductionMessage 
	{
		public const int NameKey = 1000;

		public string Name { get { return name; } }
        
		private string name; 
       
		public IntroductionMessage(string name)
		{
			this.name = name;
		}

		public IntroductionMessage(FlatmapList message)
		{
			this.name = message.Find(NameKey, 1).dataValue as string;
		}

		public FlatmapList Create()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add(NameKey, name);
			return flatmap;
		}
	}

	public class WriteMessage
	{
		public const int LogLevelKey = 1000;
		public const int MessageKey = 1001;
		public const int TimeStampKey = 1002;

		public TraceLevel LogLevel { get { return logLevel; } }
		public string Message { get { return message; } }
		public string TimeStamp { get { return timeStamp; } }

		private TraceLevel logLevel;
		private string message;
		private string timeStamp;

		public WriteMessage(TraceLevel logLevel, string message, string timeStamp)
		{
			this.logLevel = logLevel;
			this.message  = message;
			this.timeStamp = timeStamp;
		}

		public WriteMessage(FlatmapList message)
		{
			this.logLevel = (TraceLevel) Enum.Parse(typeof(TraceLevel), 
				(string) (message.Find(LogLevelKey, 1).dataValue), true);
			this.message = message.Find(MessageKey, 1).dataValue as string;
			this.timeStamp = message.Find(TimeStampKey, 1).dataValue as string;
		}

		public FlatmapList Create()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add(LogLevelKey, logLevel.ToString());
			flatmap.Add(MessageKey, message);
			flatmap.Add(TimeStampKey, timeStamp);
			return flatmap;
		}
	}

	public class RefreshMessage
	{
		public const int NameKey = 1000;
 
		public string Name { get { return name; } }

		private string name;
      
		public RefreshMessage(string name, TraceLevel logLevel)
		{
			this.name = name;
		}

		public RefreshMessage(FlatmapList message)
		{
			this.name = message.Find(NameKey, 1).dataValue as string;
		}

		public FlatmapList Create()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add(NameKey, name);
			return flatmap;
		}
	}

	public class DisposeMessage
	{
		public DisposeMessage()
		{
		}

		public DisposeMessage(FlatmapList message)
		{
		}

		public FlatmapList Create()
		{
			return new FlatmapList();
		}
	}

	public class WriteResponse
	{
		public WriteResponse()
		{
		}

		public WriteResponse(FlatmapList message)
		{
		}

		public FlatmapList Create()
		{
			return new FlatmapList();
		}
	}

	public class IntroductionResponse
	{
		public uint SuccessKey = 1000;

		public bool Success { get { return success; } }
        
		private bool success;

		public IntroductionResponse(bool success)
		{
			this.success = success;
		}

		public IntroductionResponse(FlatmapList flatmap)
		{
			success = bool.Parse(flatmap.Find(SuccessKey, 1).dataValue.ToString());
		}

		public FlatmapList Create()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add(SuccessKey, success.ToString());
			return flatmap;
		}
	}

	#endregion
}

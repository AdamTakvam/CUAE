using System;
using System.Collections;
using System.Threading;

namespace Metreos.Utilities
{
	/// <summary>
	/// Delegate to perform action of an item in the queue.
	/// </summary>
	public delegate void QueueProcessorDelegate( QueueProcessor qp, object data );

	/// <summary>
	/// Delegate to inform of an exception thrown while processing an item.
	/// </summary>
	public delegate void QueueProcessorExceptionDelegate( QueueProcessor qp, object data, Exception e );

	/// <summary>
	/// A work queue which uses a thread from a thread pool to invoke
	/// the delegate of an item in the queue. The items are processed
	/// one at a time in the order added to the queue. The items are
	/// also processed fairly with respect to the other clients of the
	/// thread pool.
	/// </summary>
	public class QueueProcessor
	{
		/// <summary>
		/// Constructs the queue processor.
		/// </summary>
		/// <param name="qpeDelegate">delegate to inform of an exception
		/// thrown while processing an item.</param>
		/// <param name="tp">the thread pool to use to perform actions.</param>
		public QueueProcessor( QueueProcessorExceptionDelegate qpeDelegate,
			Metreos.Utilities.ThreadPool tp )
		{
			this.tp = tp;
			this.qpeDelegate = qpeDelegate;
			wrDelegate = new WorkRequestDelegate( Process );
		}
		
		private Metreos.Utilities.ThreadPool tp;

		private QueueProcessorExceptionDelegate qpeDelegate;

		private WorkRequestDelegate wrDelegate;

		/// <summary>
		/// Enqueues an action to be performed when it reaches the head of the queue.
		/// </summary>
		/// <param name="qpDelegate">delegate to perform action of an item in the queue.</param>
		/// <param name="data">data value to pass to the delegate when the action is performed.</param>
		public void Enqueue( QueueProcessorDelegate qpDelegate, object data )
		{
			lock (queue)
			{
				if (stopped)
					throw new Exception( "queue is stopped" );

				queue.Enqueue( new QueueElement( qpDelegate, data ) );

				if (queue.Count == 1 && !blocked)
					tp.PostRequest( wrDelegate );
			}
		}

		/// <summary>
		/// Stops the queue and waits for it to clear.
		/// </summary>
		/// <param name="force">true to forcibly clear the queue, false
		/// to wait until all the items have been processed.</param>
		public void Stop( bool force )
		{
			lock (queue)
			{
				stopped = true;
				
				if (force)
				{
					queue.Clear();
					Monitor.PulseAll( queue );
				}

				while (queue.Count > 0)
					Monitor.Wait( queue );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Block()
		{
			lock (queue)
			{
				blocked = true;
			}
		}

		public void Unblock()
		{
			lock (queue)
			{
				if (blocked)
				{
					blocked = false;
					if (queue.Count > 0)
						tp.PostRequest( wrDelegate );
				}
			}
		}

		private bool stopped;

		private bool blocked;

		/// <returns>returns the first item in the queue, or null if the queue is empty.</returns>
		private QueueElement Peek()
		{
			lock (queue)
			{
				if (queue.Count == 0)
					return null;

				return (QueueElement) queue.Peek();
			}
		}

		/// <summary>
		/// Removes the specified item if it is the first in the queue.
		/// </summary>
		/// <param name="item">An item which might be the first in the queue.</param>
		private void Dequeue( QueueElement item )
		{
			lock (queue)
			{
				if (Peek() == item)
				{
					queue.Dequeue();
					if (queue.Count > 0)
					{
						if (!blocked)
							tp.PostRequest( wrDelegate );
					}
					else
					{
						// wake a waiting Stop.
						Monitor.PulseAll( queue );
					}
				}
			}
		}

		/// <summary>
		/// Processes one item from the queue for the thread pool thread.
		/// </summary>
		/// <param name="unused">always null</param>
		internal void Process( object unused )
		{
			lock (processSync)
			{
				QueueElement item = Peek();
				if (item != null)
				{
					try
					{
						item.qpDelegate( this, item.data );
					}
					catch ( Exception e )
					{
						if (qpeDelegate != null)
							qpeDelegate( this, item.data, e );
					}
					finally
					{
						Dequeue( item );
					}
				}
			}
		}

		private Queue queue = new Queue();

		private object processSync = new object();
	}

	/// <summary>
	/// Record of a request for processing.
	/// </summary>
	internal class QueueElement
	{
		public QueueElement( QueueProcessorDelegate qpDelegate, object data )
		{
			this.qpDelegate = qpDelegate;
			this.data = data;
		}

		public QueueProcessorDelegate qpDelegate;
				
		public object data;
	}
}

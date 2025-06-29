using System;
using System.Diagnostics;
using System.Collections;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a queue of Events.
	/// </summary>
	public class EventQueue
	{
		/// <summary>
		/// Constructs an EventQueue.
		/// </summary>
		public EventQueue()
		{
			queue = Queue.Synchronized(new Queue());
		}

		/// <summary>
		/// Queue of Events.
		/// </summary>
		/// <remarks>
		/// Events are processed in order--hence a queue is used to hold them.
		/// </remarks>
		private readonly Queue queue;

		/// <summary>
		/// Enqueues event to the end of the queue.
		/// </summary>
		/// <param name="event_">Event to enqueue.</param>
		public void Enqueue(Event event_)
		{
			queue.Enqueue(event_);
		}

		/// <summary>
		/// Dequeues event from queue.
		/// </summary>
		/// <returns>Event taken off the front of the queue or null if queue is
		/// empty.</returns>
		public Event Dequeue()
		{
			Event event_;

			lock (queue.SyncRoot)
			{
				event_ = queue.Count > 0 ? queue.Dequeue() as Event : null;
			}

			return event_;
		}

		/// <summary>
		/// Property whose value is whether this EventQueue is empty.
		/// </summary>
		public bool IsEmpty { get { return queue.Count == 0; } }

		/// <summary>
		/// Property whose value is the number of Events contained in the
		/// EventQueue.
		/// </summary>
		public int Count { get { return queue.Count; } }

		/// <summary>
		/// Removes all Events from the EventQueue.
		/// </summary>
		public void Clear()
		{
			queue.Clear();
		}

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			return "EventQueue(size:" + queue.Count.ToString() + ")";
		}
	}
}

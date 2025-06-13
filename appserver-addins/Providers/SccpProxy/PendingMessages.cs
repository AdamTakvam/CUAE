using System;
using System.Diagnostics;
using System.Collections;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents the raw, binary messages whose minimal contents,
	/// e.g., transport address or even just message type, we have passed up to
	/// the app layer and are waiting for their return.
	/// </summary>
	/// <remarks>
	/// It is basically a thread-safe wrapper class around Hashtable.
	/// We pass up an identiying tag to the app and expect the app to call
	/// us back with the same tag. The app must do this even for messages
	/// it does not want sent because we need to remove the message from
	/// this table. If the app does not want the message sent, it calls us
	/// back with an empty address.
	/// </remarks>
	public class PendingMessages
	{
		/// <summary>
		/// Simple constructor.
		/// </summary>
		public PendingMessages()
		{
			messages = new Hashtable();
		}

		/// <summary>
		/// If a pendingMessage is older than this, we remove it from the
		/// table.
		/// </summary>
		private long pendingMessageMaxAgeNsec;

		/// <summary>
		/// This is the hash table that actually holds the messages.
		/// </summary>
		private Hashtable messages;

		/// <summary>
		/// Read-only property that has the value of the pendingMessages hashtable.
		/// </summary>
		public Hashtable Messages
		{
			get
			{
				return messages;
			}
		}

		/// <summary>
		/// Initialize the pending-messages object by assigning the max age.
		/// </summary>
		/// <param name="pendingMessageMaxAgeNsec">If a pendingMessage is
		/// older than this, we remove it from the table.</param>
		public void Initialize(long pendingMessageMaxAgeNsec)
		{
			this.pendingMessageMaxAgeNsec = pendingMessageMaxAgeNsec;
		}

		/// <summary>
		/// Remove all messages from the table.
		/// </summary>
		public void Cleanup()
		{
			messages.Clear();
		}

		/// <summary>
		/// Remove and return message from pendingMessages table that has this tag.
		/// </summary>
		/// <param name="tag">Integer tag that uniquely identifies the message.</param>
		/// <returns>The message with the provided tag.</returns>
		public Message Remove(int tag)
		{
			Debug.Assert(messages.Contains(tag), "Tag not found; cannot remove");

			Message message;
			lock (this)
			{
				message = (Message)messages[tag];
				messages.Remove(tag);
			}

			return message;
		}

		/// <summary>
		/// Create message with this contents and add to the pendingMessage table.
		/// </summary>
		/// <param name="tag">Integer tag that uniquely identifies the message.</param>
		/// <param name="payload">Message to add.</param>
		public void Add(int tag, Message message)
		{
			Debug.Assert(!messages.Contains(tag), "Duplicate tag; add anyway");

			lock (this)
			{
				messages.Add(tag, message);
			}
		}

		/// <summary>
		/// Remove all pending messages that are too older.
		/// </summary>
		public void RemoveOld()
		{
			// Create queue just to hold the list of tags to subsequently
			// remove from pendingMessages hash table. We can't remove them
			// while iterating through the hash table.
			Queue markedForDeath = new Queue();

			// Toss tags for any abandoned messages in the queue for subsequent
			// removal.
			IDictionaryEnumerator enumerator = messages.GetEnumerator();
			lock (this)
			{
				while (enumerator.MoveNext())
				{
					if (((Message)enumerator.Value).IsOlderThan(
						new TimeSpan(pendingMessageMaxAgeNsec)))
					{
						markedForDeath.Enqueue(enumerator.Key);
					}
				}

				// Now remove the messages from the hash table that we just
				// found.
				while (markedForDeath.Count > 0)
				{
					Remove((int)markedForDeath.Dequeue());
				}
			}
		}
	}
}

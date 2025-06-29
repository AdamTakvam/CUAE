/* $Id: QueuedTrace.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 15, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;





/**
 * A QueuedTrace puts trace messages on a queue and writes
 * them asynchronously using another thread (when started).
 * This way the traced code is only minimally slowed down.
 * 
 * QueuedTrace can also be used in a mode where it does not
 * queue the messages but processes them immediately. In
 * fact, it can switch back and forth between modes.
 */
abstract public class QueuedTrace extends Trace implements Runnable
{
	/**
	 * Constructs a QueuedTrace using specified parameters.
	 * 
	 * @param maxLen maximum length of the queue before attempting
	 * to slow down threads queuing messages. Must be greater than
	 * or equal to 0. 0 disables queuing (even if start is called).
	 * 
	 * @param addDelay number of milliseconds rprt will delay when the
	 * queue is too long. If less than 0, rprt will not delay. If
	 * equal to 0, rprt will delay until the queue is not too long.
	 * If greater than 0, rprt will delay the product of addDelay
	 * and the number of excess entries (but the delay is terminated
	 * early once the queue is not too long).
	 */
	public QueuedTrace( int maxLen, int addDelay )
	{
		setMaxLen( maxLen );
		setAddDelay( addDelay );
	}
	
	/**
	 * Constructs a QueuedTrace using default parameters.
	 * 
	 * @see #DEFAULT_MAXLEN
	 * @see #DEFAULT_ADDDELAY
	 */
	public QueuedTrace()
	{
		// do nothing.
	}
	
	/**
	 * Constructs the QueuedTrace.
	 *
	 * @param url
	 */
	public QueuedTrace( URL url )
	{
		super( url );
		
		Integer i;
		
		i = url.getIntegerTerm( "maxLen" );
		if (i != null)
			setMaxLen( i.intValue() );
		
		i = url.getIntegerTerm( "addDelay" );
		if (i != null)
			setAddDelay( i.intValue() );
	}

	private int maxLen = DEFAULT_MAXLEN;
	
	private int addDelay = DEFAULT_ADDDELAY;
	
	/**
	 * @param maxLen
	 */
	public void setMaxLen( int maxLen )
	{
		if (maxLen < 0)
			throw new IllegalArgumentException( "maxLen < 0" );
	
		if (queue != null)
			throw new IllegalStateException( "queue is already started" );
		
		this.maxLen = maxLen;
	}
	
	/**
	 * @param addDelay
	 */
	public void setAddDelay( int addDelay )
	{
		this.addDelay = addDelay;
	}
	
	/**
	 * Default value for maxLen in QueuedTrace constructor (100).
	 */
	public final static int DEFAULT_MAXLEN = 1000;
	
	/**
	 * Default value for addDelay in QueuedTrace constructor (20).
	 */
	public final static int DEFAULT_ADDDELAY = 20;

	////////////////////////
	// subclass overrides //
	////////////////////////

	/**
	 * Abstract method to actually show the report. This might
	 * format the report on an output stream, write it to a file,
	 * or send it to a network socket.
	 * 
	 * @param when
	 * @param eventMask
	 * @param who
	 * @param msg
	 * @param t
	 */
	private void showReport( long when, int eventMask, Object who,
		String msg, Throwable t )
	{
		try
		{
			synchronized (showReportSync)
			{
				showReport0( when, eventMask, who, msg, t );
			}
		}
		catch ( Exception e )
		{
			e.printStackTrace();
		}
	}

	private final Object showReportSync = new Object();
	
	/**
	 * @param when
	 * @param eventMask
	 * @param who
	 * @param msg
	 * @param t
	 * @throws Exception
	 */
	abstract protected void showReport0( long when, int eventMask, Object who,
		String msg, Throwable t ) throws Exception;

	@Override
	public Trace start()
	{
		synchronized (queueSync)
		{
			if (queue != null)
				throw new IllegalStateException( "queue is already started" );
			
			if (maxLen == 0)
			{
				// queuing is disabled!
				return this;
			}
			
			queue = new Queue( maxLen );
			
			// start the queue processing thread.
			
			thread = new Thread( this );
			thread.setDaemon( true );
			thread.start();
			
			// wait for the thread to actually start.
			
			try
			{
				queueSync.wait( 15000 );
			}
			catch ( InterruptedException e )
			{
				queue = null;
				thread = null;
				throw new RuntimeException( "timeout starting queuing" );
			}
		}
		return this;
	}
	
	private final Object queueSync = new Object();

	@Override
	protected void rprt( long when, int eventMask, Object who,
		M msg, Throwable t )
	{
		if (!matchesMask( eventMask ))
			return;
		
		rprt0( when, eventMask, who, msg.toString(), t );
	}

	/**
	 * @param when
	 * @param eventMask 
	 * @param who
	 * @param msg
	 * @param t
	 */
	private void rprt0( long when, int eventMask, Object who,
		String msg, Throwable t )
	{
		synchronized (queueSync)
		{
			int excessSize = queue( queue, when, eventMask, who, msg, t );
			if (excessSize >= 0)
			{
				if (excessSize == 0)
					return;
				
				handleTooLongQueue( queue, excessSize );
				return;
			}
			
			// excessSize < 0, indicating that there is no queue.
			// so we pass through the event forthwith.

			showReport( when, eventMask, who, msg, t );
		}
	}

	/**
	 * Handles the condition where the queue is too long.
	 * This method slows down the caller a bit (at least
	 * until the queue is not too long anymore) depending
	 * upon the setting of addDelay. another option might
	 * be to discard the older messages on the queue, etc.
	 * 
	 * @param q the queue that is now too long.
	 * 
	 * @param excessSize the excess length of the queue.
	 */
	protected void handleTooLongQueue( Queue q, int excessSize )
	{
		// here we slow the caller down a bit if the queue
		// is too long.
		int howLong = addDelay*excessSize;
		if (howLong >= 0)
			q.delay( howLong );
	}

	@Override
	public void stop()
	{
		Queue q;
		Thread t;
		
		synchronized (queueSync)
		{
			q = queue;
			queue = null;
			
			t = thread;
			thread = null;
		}
		
		if (q != null)
			flush( q );
		
		if (t != null)
		{
			try
			{
				t.join( 15000 );
			}
			catch ( InterruptedException e )
			{
				// ignore.
			}
		}
	}
	
	public void run()
	{
		Queue q;
		synchronized (queueSync)
		{
			q = queue;
			queueSync.notify();
		}
		if (q == null)
			return;
		
		try
		{
			run0( q );
		}
		catch ( RuntimeException e )
		{
			e.printStackTrace();
		}
		finally
		{
			synchronized (queueSync)
			{
				if (queue == q)
				{
					queue = null;
					thread = null;
				}
			}
			flush( q );
		}
	}
	
	private void run0( Queue q )
	{
		while (q == queue)
		{
			Entry entry = q.get( 1000 );
			if (entry != null)
				showReport( entry.when, entry.eventMask, entry.who,
					entry.msg, entry.t );
		}
	}

	private int queue( Queue q, long when, int eventMask, Object who, String msg,
		Throwable t )
	{
		if (q == null)
			return -1;
		
		return q.add( new Entry( when, eventMask, who, msg, t ) );
	}
	
	private void flush( Queue q )
	{
		Entry entry;
		while ((entry = q.get( -1 )) != null)
			showReport( entry.when, entry.eventMask, entry.who,
				entry.msg, entry.t );
	}
	
	/**
	 * The queue of waiting messages.
	 */
	protected Queue queue;
	
	private Thread thread;

	/////////////
	// CLASSES //
	/////////////
	
	/**
	 * A queue for waiting messages.
	 */
	public static class Queue
	{
		/**
		 * Constructs the Queue.
		 *
		 * @param maxLen
		 */
		public Queue( int maxLen )
		{
			this.maxLen = maxLen;
		}

		private final int maxLen;
		
		/**
		 * @param delay max time to wait in ms for an entry. If 0, get
		 * will wait forever for an entry. If < 0, get will not wait
		 * but return immediately.
		 * 
		 * @return next entry in the queue. if none, returns null.
		 */
		public synchronized Entry get( int delay )
		{
			while (size == 0)
			{
				if (delay < 0)
					return null;
				
				try
				{
					wait( delay );
					if (size == 0)
						return null;
				}
				catch ( InterruptedException e )
				{
					return null;
				}
			}
			
			Entry entry = removeEntry();
			
			if (size < maxLen)
			{
				// wake up any thread waiting on a full queue
				notifyAll();
			}
			
			return entry;
		}

		/**
		 * Adds an entry to the queue, and notifies if the queue was
		 * empty.
		 * 
		 * @param entry to be added.
		 * 
		 * @return the excess size if the queue is too long, or 0 if
		 * it is not too long.
		 */
		public synchronized int add( Entry entry )
		{
			addEntry( entry );
			
			if (size == 1)
			{
				// wake up one thread waiting for an entry to process
				notify();
			}
			
			if (size > maxLen)
				return size - maxLen;
			
			return 0;
		}

		/**
		 * @param howLong
		 */
		public synchronized void delay( int howLong )
		{
			try
			{
				wait( howLong );
			}
			catch ( InterruptedException e )
			{
				// ignore
			}
		}

		///////////////////
		// private stuff //
		///////////////////
		
		/**
		 * Adds an entry to the end of the queue.
		 * @param entry
		 */
		private void addEntry( Entry entry )
		{
			if (last != null)
			{
				last.next = entry;
				last = entry;
			}
			else
			{
				first = entry;
				last = entry;
			}
			size++;
		}

		/**
		 * @return the first entry in the queue.
		 */
		private Entry removeEntry()
		{
			Entry entry = first;
			
			first = entry.next;
			if (first == null)
				last = null;
			
			size--;
			return entry;
		}
		
		private int size = 0;
		
		private Entry first;
		
		private Entry last;
	}
	
	private static class Entry
	{
		/**
		 * Constructs the Entry.
		 *
		 * @param when
		 * @param eventMask
		 * @param who
		 * @param msg
		 * @param t
		 */
		public Entry( long when, int eventMask, Object who, String msg, Throwable t )
		{
			this.when = when;
			this.eventMask = eventMask;
			this.who = who;
			this.msg = msg;
			this.t = t;
		}
		
		/**
		 * The time of the event.
		 */
		public final long when;
		
		/**
		 * The mask of the event.
		 */
		public final int eventMask;
		
		/**
		 * The reporter of the event.
		 */
		public Object who;
		
		/**
		 * The event message.
		 */
		public String msg;
		
		/**
		 * An associated throwable.
		 */
		public Throwable t;
		
		/**
		 * The next entry in the queue.
		 */
		public Entry next;
	}
}

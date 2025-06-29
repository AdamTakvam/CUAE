/* $Id: AlarmManager.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 23, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.util.HashMap;
import java.util.Map;
import java.util.TreeSet;

/**
 * The alarm manager is used to implement alarms.
 * 
 * A listener desiring a wakeup call should add itself to the manager
 * with the specified delay in milliseconds. The listener may be removed
 * at any time. When the alarm goes off, the listener is also removed.
 * 
 * @see #add(AlarmListener, int)
 * @see #remove(AlarmListener)
 */
public class AlarmManager implements Runnable
{
	/**
	 * Adds the listener to the set of those getting wakeup calls. If the
	 * listener is already scheduled for a wakeup call, the schedule is
	 * adjusted. There can only be one outstanding wakeup call per listener.
	 * 
	 * This method is thread safe.
	 * 
	 * @param listener the target of the wakeup call.
	 * 
	 * @param delay the delay in milliseconds before the wakeup call. If
	 * delay <= 0, the call will occur the next time poll is called.
	 */
	public synchronized void add( AlarmListener listener, long delay )
	{
		checkStarted();
		
		long due = System.currentTimeMillis() + delay;
		
		Alarm alarm = getAlarm( listener );
		if (alarm != null)
		{
			// schedule is being adjusted
			dequeue( alarm );
			alarm.setDue( due );
			enqueue( alarm );
		}
		else
		{
			alarm = new Alarm( listener, due );
			addAlarm( listener, alarm );
			enqueue( alarm );
		}
		
		notifyWorker();
	}
	
	private synchronized void update( Alarm alarm, int delay )
	{
		long due = alarm.getDue() + delay;
		
		alarm.setDue( due );
		//addAlarm( alarm.listener, alarm );
		enqueue( alarm );
		
		notifyWorker();
	}
	
	/**
	 * Removes any scheduled wakeup call for this listener.
	 * 
	 * This method is thread safe.
	 * 
	 * @param listener the target of the wakeup call.
	 */
	public synchronized void remove( AlarmListener listener )
	{
		checkStarted();
		
		Alarm alarm = removeAlarm( listener );
		if (alarm != null)
			dequeue( alarm );
		
		notifyWorker();
	}
	
	private synchronized void remove( Alarm alarm )
	{
		removeAlarm( alarm.listener );
	}

	/**
	 * Starts the alarm manager with 1 worker.
	 */
	public void start()
	{
		start( 1 );
	}

	/**
	 * Starts the alarm manager with the specified number
	 * of workers.
	 * @param nWorkers
	 */
	public synchronized void start( int nWorkers )
	{
		checkNotStarted();
		setStarted( true );
		clearAlarms();
		clearQueue();
		
		for (int i = 0; i < nWorkers; i++)
			new Thread( this, "alarm worker "+i ).start();
	}

	/**
	 * Stops the alarm manager.
	 */
	public synchronized void stop()
	{
		checkStarted();
		setStarted( false );
		clearAlarms();
		clearQueue();
		notifyWorker();
	}
	
	private void wakeup( Alarm alarm )
	{
		try
		{
			int delay = alarm.listener.wakeup( this, alarm.getDue() );
			if (delay > 0)
				update( alarm, delay );
			else
				remove( alarm );
		}
		catch ( Exception e )
		{
			Trace.report( alarm.listener, e );
			remove( alarm );
		}
	}
	
	private Alarm getNextDueAlarm()
	{
		// ok, the worker needs to get the next due alarm and
		// then wait until its due time, then return it. if alerted
		// by notifyWorker, it should refresh the next due alarm.
		// one trick will be in excluding multiple workers from
		// coming in here at the same time.
		synchronized (getNextDueAlarmSync)
		{
			synchronized (this)
			{
				while (true)
				{
//					Trace.report( "getNextDueAlarm" );
					if (!isStarted())
						return null;
					
					Alarm alarm = getFirst();
					
					if (alarm == null)
					{
						try
						{
							wait( Long.MAX_VALUE );
							continue;
						}
						catch ( InterruptedException e )
						{
							return null;
						}
					}
					
					long delay = alarm.getDue() - System.currentTimeMillis();
					if (delay > 0)
					{
						try
						{
							wait( delay );
							continue;
						}
						catch ( InterruptedException e )
						{
							return null;
						}
					}
					
					// the alarm being returned has not been removed
					// from alarmsByListener. it is presumed that the
					// alarm will be set again. if not, it should be
					// removed.
					
					dequeue( alarm );
					return alarm;
				}
			}
		}
	}

	private final Object getNextDueAlarmSync = new Object();
	
	private void notifyWorker()
	{
		// the set of alarms has changed.
		notify();
	}
	
	public void run()
	{
		try
		{
			Alarm alarm;
			while ((alarm = getNextDueAlarm()) != null)
			{
//				Trace.report( "waking up "+alarm );
				wakeup( alarm );
			}
		}
		catch ( RuntimeException e )
		{
			Trace.report( this, e );
		}
//		finally
//		{
//			Trace.report( "done" );
//		}
	}

	/////////////
	// STARTED //
	/////////////
	
	private final static String STARTED = "started";
	
	private final static String NOT_STARTED = "not started";
	
	private boolean isStarted()
	{
		return started;
	}
	
	private void setStarted( boolean value 	)
	{
		started = value;
	}
	
	private void checkStarted()
	{
		if (!isStarted())
			throw new UnsupportedOperationException( NOT_STARTED );
	}
	
	private void checkNotStarted()
	{
		if (isStarted())
			throw new UnsupportedOperationException( STARTED );
	}
	
	private boolean started;

	////////////////////////
	// ALARMS BY LISTENER //
	////////////////////////
	
	private Alarm getAlarm( AlarmListener listener )
	{
		return alarmsByListener.get( listener );
	}
	
	private void addAlarm( AlarmListener listener, Alarm alarm )
	{
		alarmsByListener.put( listener, alarm );
	}
	
	private Alarm removeAlarm( AlarmListener listener )
	{
		return alarmsByListener.remove( listener );
	}
	
	private void clearAlarms()
	{
		alarmsByListener.clear();
	}
	
	private final Map<AlarmListener,Alarm> alarmsByListener = new HashMap<AlarmListener,Alarm>();

	////////////////////////
	// ALARMS BY DUE TIME //
	////////////////////////
	
	private Alarm getFirst()
	{
		if (alarms.isEmpty())
			return null;
		
		return alarms.first();
	}
	
	private void enqueue( Alarm alarm )
	{
		alarms.add( alarm );
	}

	private void dequeue( Alarm alarm )
	{
		alarms.remove( alarm );
	}
	
	private void clearQueue()
	{
		alarms.clear();
	}
	
	private final TreeSet<Alarm> alarms = new TreeSet<Alarm>();
	
	/////////////
	// CLASSES //
	/////////////

	private final static class Alarm implements Comparable
	{
		/**
		 * @param listener
		 * @param due
		 */
		Alarm( AlarmListener listener, long due )
		{
			this.listener = listener;
			this.due = due;
		}
		
		/**
		 * @return the time the alarm is due.
		 */
		long getDue()
		{
			return due;
		}

		/**
		 * @param due
		 */
		void setDue( long due )
		{
			this.due = due;
		}

		@Override
		public int hashCode()
		{
			return ((int) (due ^ (due >>> 32))) ^ ((int) (seq ^ (seq >>> 32)));
		}

		@Override
		public boolean equals( Object obj )
		{
			if (obj instanceof Alarm)
			{
				Alarm other = (Alarm) obj;
				return due == other.due && seq == other.seq;
			}
			return false;
		}

		public int compareTo( Object obj )
		{
			Alarm other = (Alarm) obj;
			
			if (due < other.due)
				return -1;
			
			if (due > other.due)
				return 1;
			
			// due time is the same for both, now we need to
			// compare the seq.
			
			if (seq < other.seq)
				return -1;
			
			if (seq > other.seq)
				return 1;
			
			return 0;
		}
		
		/**
		 * The listener for wakeup events.
		 */
		final AlarmListener listener;
		
		/**
		 * The time the alarm is due.
		 */
		private long due;
		
		/**
		 * A unique for all reasonable time sequence number.
		 */
		private final long seq = nextSeq();
		
		private static synchronized long nextSeq()
		{
			return nextSeq++;
		}
		
		private static long nextSeq = 0;
	}
}
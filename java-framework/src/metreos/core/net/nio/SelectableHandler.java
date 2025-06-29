/* $Id: SelectableHandler.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on May 12, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;

import metreos.util.SimpleTodo;
import metreos.util.Todo;
import metreos.util.Trace;
import metreos.util.Trace.M;


/**
 * Description of SelectableHandler.
 */
public abstract class SelectableHandler implements Handler
{
	/**
	 * Constructs the SelectableHandler.
	 *
	 * @param superSelector
	 * @param channel 
	 */
	public SelectableHandler( SuperSelector superSelector,
		SelectableChannel channel )
	{
		this.superSelector = superSelector;
		this.channel = channel;
	}
	
	/**
	 * Description of superSelector.
	 */
	private final SuperSelector superSelector;
	
	/**
	 * Description of channel.
	 */
	private final SelectableChannel channel;

	/**
	 * @return our SuperSelector.
	 */
	public final SuperSelector getSuperSelector()
	{
		return superSelector;
	}

	/**
	 * @return our selectable channel.
	 */
	public final SelectableChannel getSelectableChannel()
	{
		return channel;
	}

	//////////////
	// toString //
	//////////////
	
	/**
	 * Sets cachedToString to reflect the current handler identity.
	 */
	abstract protected void setupCachedToString();

	@Override
	public final String toString()
	{
		return cachedToString;
	}
	
	/**
	 * @param s the new cachedToString
	 */
	protected final void setCachedToString( String s )
	{
		cachedToString = s;
	}
	
	/**
	 * Description of cachedToString.
	 */
	private String cachedToString;

	//////////////////
	// REGISTRATION //
	//////////////////

	public final void register()
	{
		superSelector.addToTodoList( new SimpleTodo()
		{
			public void doit()
			{
				try
				{
					doRegister();
				}
				catch ( Exception e )
				{
					fireTodoException( e );
				}
			}
		} );
	}
	
	/**
	 * @throws IOException
	 */
	final void doRegister()
		throws IOException
	{
		key = superSelector.doRegister( channel, getInterestOps(), this );
	}
	
	private SelectionKey key;

	///////////
	// CLOSE //
	///////////

	public final void close()
	{
		close( CLOSE );
	}
	
	public final void close( final Object reason )
	{
		superSelector.addToTodoList( new SimpleTodo()
		{
			public void doit()
			{
				try
				{
					doClose( reason );
				}
				catch ( Exception e )
				{
					fireTodoException( e );
				}
			}
		} );
	}

	public synchronized void doClose( Object reason )
	{
		if (reason instanceof Throwable)
			report( (Throwable) reason );

		if (key != null && key.isValid())
		{
			try
			{
				key.cancel();
			}
			catch ( RuntimeException e )
			{
				report( e );
			}
		}

		if (channel != null && channel.isOpen())
		{
			try
			{
				channel.close();
			}
			catch ( Exception e )
			{
				report( e );
			}
		}
	}

	///////////////////////////
	// UPDATING INTEREST OPS //
	///////////////////////////
	
	public final void setSelectedInterestOps()
	{
		doSetInterestOps( selectedInterestOps() );
	}
	
	public final void resetInterestOps()
	{
		try
		{
			int newOps = getInterestOps();
			if (newOps != key.interestOps())
			{
				// this eventually calls doSetInterestOps to set the interest
				superSelector.addToTodoList( resetInterestOpsTodo );
			}
		}
		catch ( RuntimeException e )
		{
			// ignore.
		}
	}
	
	private final Todo resetInterestOpsTodo = new SimpleTodo()
	{
		public void doit()
		{
			try
			{
				doSetInterestOps( getInterestOps() );
			}
			catch ( Exception e )
			{
				fireTodoException( e );
			}
		}
	};
	
	/**
	 * @param ops
	 */
	final void doSetInterestOps( int ops )
	{
		if (key != null && key.isValid() && ops != key.interestOps())
			key.interestOps( ops );
	}
	
	//////////////////
	// INTEREST OPS //
	//////////////////

	/**
	 * Returns the current interest ops for the channel. This
	 * is called when the channel is first created and registered,
	 * after handleSelection has successfully returned, and other
	 * times when the interest ops need to be reset.
	 * 
	 * @return the current interest ops for the channel.
	 */
	abstract protected int getInterestOps();

	/**
	 * Returns the interest ops that should be set right after
	 * the channel has been selected (before handleSelection is
	 * called). This is called while the selector is still
	 * locked (so it should hurry).
	 * 
	 * @return value to set our interest ops to when we are selected.
	 * return 0 to block any more selection until we explicit set
	 * the interest ops or handleSelection returns. return the value
	 * of getInterestOps() to allow multiple threads to enter and
	 * work concurrently. return channel.validOps() & ~key.readyOps()
	 * to allow ops other than the currently ready ops to be selected
	 * by other threads.
	 */
	abstract protected int selectedInterestOps();

	////////////////////////
	// SELECTED INTERESTS //
	////////////////////////
	
	/**
	 * @return true if key.isAcceptable() returns true.
	 */
	protected final boolean isAcceptable()
	{
		return key.isAcceptable();
	}
	
	/**
	 * @return true if key.isReadable() returns true.
	 */
	protected final boolean isReadable()
	{
		return key.isReadable();
	}

	/**
	 * @return true if key.isWritable() returns true.
	 */
	protected final boolean isWritable()
	{
		return key.isWritable();
	}
	
	/**
	 * @return true if key.isConnectable() returns true.
	 */
	protected final boolean isConnectable()
	{
		return key.isConnectable();
	}

	/////////////////
	// TODO ERRORS //
	/////////////////
	
	/**
	 * @param e
	 */
	protected void fireTodoException( Exception e )
	{
		report( e );
	}

	//////////
	// MISC //
	//////////

	@Override
	public void finalize()
	{
		if ((key != null && key.isValid()) || channel.isOpen())
			report( "orphan handler not closed" );
	}

	///////////////
	// REPORTING //
	///////////////

	/**
	 * Reports an event on this channel.
	 * 
	 * @param msg a description of the event.
	 * 
	 * @see Trace#report(Object, M)
	 */
	public final void report( M msg )
	{
		Trace.report( this, msg );
	}

	/**
	 * Reports an event on this channel.
	 * 
	 * @param msg a description of the event.
	 * 
	 * @see Trace#report(Object, String)
	 */
	public final void report( String msg )
	{
		Trace.report( this, msg );
	}

	/**
	 * Reports an event on this channel.
	 * 
	 * @param t an exception that was caught.
	 * 
	 * @see Trace#report(Object, Throwable)
	 */
	public final void report( Throwable t )
	{
		Trace.report( this, t );
	}

	/**
	 * Reports an event on this channel.
	 * 
	 * @param msg a description of the event.
	 * 
	 * @param t an exception that was caught.
	 * 
	 * @see Trace#report(Object, M, Throwable)
	 */
	public final void report( M msg, Throwable t )
	{
		Trace.report( this, msg, t );
	}
}

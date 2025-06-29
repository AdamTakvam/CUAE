/* $Id: Handler.java 8308 2005-08-03 15:06:53Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;

/**
 * The interface that must be supported by channel selection
 * handlers.
 */
public interface Handler
{
	///////////////////////////
	// SuperSelector Methods //
	///////////////////////////

	/**
	 * Sets the interest ops of a just selected channel.
	 * 
	 * The selector is held synchronized while this method executes.
	 * 
	 * Should only be called by SuperSelector or a subclass thereof.
	 */
	public void setSelectedInterestOps();
	
	/**
	 * Closes a handler.
	 * 
	 * @param reason reason for the close. maybe one of the
	 * constants below, or something else (perhaps an exception).
	 * The selector is held synchronized while this method executes.
	 * 
	 * The selector is held synchronized while this method executes.
	 * 
	 * Should only be called by SuperSelector or a subclass thereof.
	 */
	public void doClose( Object reason );
	
	/**
	 * Handles selection of a channel for i/o operations.
	 * 
	 * The selector is not held synchronized while this method executes.
	 * 
	 * Should only be called by SuperSelector or a subclass thereof.
	 * 
	 * @throws IOException if thrown, the channel is considered
	 * dead and will be closed.
	 */
	public void handleSelection() throws IOException;

	////////////////////
	// PUBLIC METHODS //
	////////////////////

	/**
	 * Register this handler with the selector.
	 * 
	 * The selector is not held synchronized while this method executes.
	 * 
	 * Should only be called by SuperSelector or a subclass thereof.
	 */
	public void register();
	
	/**
	 * Resets the channel's interest ops. This is called when some
	 * output has become available, or when the handler is now ready
	 * to process some more input. Called automatically when
	 * handleSelection returns normally.
	 * 
	 * The selector is not held synchronized while this method executes.
	 */
	public void resetInterestOps();
	
	/**
	 * Closes the channel. Same as close( CLOSE ).
	 * 
	 * @see #close(Object)
	 * @see #CLOSE
	 */
	public void close();

	/**
	 * @param reason reason for the close. maybe one of the
	 * constants below, or something else (perhaps an exception).
	 * 
	 * The selector is not held synchronized while this method executes.
	 * 
	 * @see #CLOSE
	 * @see #EOF
	 * @see #STOP
	 */
	public void close( Object reason );

	///////////////////
	// CLOSE REASONS //
	///////////////////
	
	/**
	 * Reason passed to doClose to indicate that close was called.
	 */
	public String CLOSE = "CLOSE";
	
	/**
	 * Reason passed to doClose to indicate that handleSelection
	 * threw EOFException.
	 */
	public String EOF = "EOF";
	
	/**
	 * Reason passed to doClose to indicate that the SuperSelector
	 * was stopped.
	 */
	public String STOP = "STOP";
}

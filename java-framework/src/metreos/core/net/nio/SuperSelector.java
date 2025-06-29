/* $Id: SuperSelector.java 8308 2005-08-03 15:06:53Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.EOFException;
import java.io.IOException;
import java.net.DatagramSocket;
import java.net.ServerSocket;
import java.net.SocketAddress;
import java.net.SocketException;
import java.nio.channels.CancelledKeyException;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.ClosedSelectorException;
import java.nio.channels.DatagramChannel;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;

import metreos.util.Assertion;
import metreos.util.Todo;
import metreos.util.Trace;
import metreos.util.Trace.M;


/**
 * A super selector provides safe and delay free access to a selector.
 */
abstract public class SuperSelector implements Runnable
{
	/**
	 * Constructs the super selector.
	 * 
	 * @param nWorkers the number of worker threads to create. these
	 * perform actions such as reading and writing on selected
	 * channels as well as selecting for channels which need
	 * attention.
	 * @param priorityBoost 
	 */
	public SuperSelector( int nWorkers, int priorityBoost )
	{
		this.nWorkers = nWorkers;
		this.priorityBoost = priorityBoost;
	}
	
	private final int nWorkers;
	
	private final int priorityBoost;

	/**
	 * Starts the super selector.
	 * @throws Exception 
	 */
	public final synchronized void start()
		throws Exception
	{
		if (started)
			throw new IllegalStateException( "already started" );
		
		started = true;
		
		try
		{
			start0();
		}
		catch ( Exception e )
		{
			started = false;
			throw e;
		}
		
		startWorkers();
	}
	
	/**
	 * Description of started
	 */
	protected boolean started = false;

	private ThreadGroup workers = new ThreadGroup( "SuperSelector Workers" );
	
	private void startWorkers()
	{
		for (int i = 0; i < nWorkers; i++)
			startWorker();
	}
	
	/**
	 * Starts a worker with this as its runnable.
	 */
	protected void startWorker()
	{
		Thread t = new Thread( workers, this,
			"SuperSelector Worker "+(workerIndex++) );
		t.setPriority( t.getPriority()+priorityBoost );
		t.start();
	}
	
	private int workerIndex = 1;
	
	/**
	 * Stops the super selector.
	 * 
	 * @param maxDelay
	 */
	public final synchronized void stop( int maxDelay )
	{
		if (!started)
			throw new IllegalStateException( "not started" );
		
		started = false;
		
		long now = System.currentTimeMillis();
		long endTime = now + maxDelay;
		
		stop0( endTime );
		
		now = System.currentTimeMillis();
		while (endTime > now && workers.activeCount() > 0)
		{
			try
			{
				wait( 10 );
			}
			catch ( InterruptedException e )
			{
				break;
			}
			now = System.currentTimeMillis();
		}
		
		if (workers.activeCount() > 0)
			workers.interrupt();
	}

	/**
	 * Removes all the active handlers, but doesn't stop the selector.
	 * @param maxDelay 
	 * @return true if the selector is empty
	 */
	public synchronized boolean cleanup( int maxDelay )
	{
		return cleanup0( System.currentTimeMillis()+maxDelay );
	}

	/**
	 * @param bindAddr
	 * @param connectAddr
	 * @param factory
	 * @return a DatagramHandler wrapping a DatagramChannel
	 * @throws IOException
	 */
	public DatagramHandler newDatagramHandler( SocketAddress bindAddr,
		SocketAddress connectAddr, DatagramHandlerFactory factory )
		throws IOException
	{
		return newDatagramHandler( new PlainSocketAddressGen( bindAddr ),
			connectAddr, factory );
	}

	/**
	 * @param bindAddrGen
	 * @param connectAddr
	 * @param factory
	 * @return a DatagramHandler wrapping a DatagramChannel
	 * @throws IOException
	 */
	public DatagramHandler newDatagramHandler( SocketAddressGen bindAddrGen,
		SocketAddress connectAddr, DatagramHandlerFactory factory )
		throws IOException
	{
		DatagramChannel channel = DatagramChannel.open();
//		Trace.report( this, "opened channel "+channel );
		
		try
		{
			DatagramSocket socket = channel.socket();
			
			int n = bindAddrGen.count();
			SocketException problem = null;
			while (n-- > 0)
			{
				try
				{
					problem = null;
					socket.bind( bindAddrGen.next() );
					break;
				}
				catch ( SocketException e )
				{
					problem = e;
				}
			}
			
			if (problem != null)
				throw problem;
			
			if (connectAddr != null)
				channel.connect( connectAddr );
			
			channel.configureBlocking( false );
			
			DatagramHandler handler = factory.newDatagramHandler( this, channel );
			handler.register();
			return handler;
		}
		catch ( IOException e )
		{
			closeChannel( channel );
			throw e;
		}
		catch ( RuntimeException e )
		{
			closeChannel( channel );
			throw e;
		}
	}

	/**
	 * @param address
	 * @param backlog
	 * @param factory
	 * @return an AcceptHandler wrapping a ServerSocketChannel
	 * @throws IOException
	 */
	public AcceptHandler newAcceptHandler( SocketAddress address,
		int backlog, AcceptHandlerFactory factory )
		throws IOException
	{
		ServerSocketChannel channel = ServerSocketChannel.open();
		
		try
		{
			ServerSocket socket = channel.socket();
			
			socket.bind( address, backlog );
			
			channel.configureBlocking( false );
			
			AcceptHandler handler = factory.newAcceptHandler( this, channel );
			handler.register();
			return handler;
		}
		catch ( IOException e )
		{
			closeChannel( channel );
			throw e;
		}
		catch ( RuntimeException e )
		{
			closeChannel( channel );
			throw e;
		}
	}

	/**
	 * @param address
	 * @param factory
	 * @param synchronous 
	 * @return a StreamHandler wrapping a SocketChannel
	 * @throws IOException
	 */
	public StreamHandler newStreamHandler( SocketAddress address,
		boolean synchronous, StreamHandlerFactory factory )
		throws IOException
	{
		SocketChannel channel = SocketChannel.open();
		boolean connected = false;
		
		if (synchronous)
		{
			try
			{
				connected = channel.connect( address );
				Assertion.check( connected, "connected" );
			}
			catch ( IOException e )
			{
				closeChannel( channel );
				throw e;
			}
			catch ( RuntimeException e )
			{
				closeChannel( channel );
				throw e;
			}
		}
		
		try
		{
			channel.configureBlocking( false );
			
			if (!synchronous)
				connected = channel.connect( address );
			
			StreamHandler handler = factory.newStreamHandler( this, channel );
			
			if (connected)
				handler.doConnected();
			
			handler.register();
			
			return handler;
		}
		catch ( IOException e )
		{
			closeChannel( channel );
			throw e;
		}
		catch ( RuntimeException e )
		{
			closeChannel( channel );
			throw e;
		}
	}

	/**
	 * Closes a channel and reports any error which occurs. This should
	 * only be performed on channels which are not yet registered.
	 * 
	 * @param channel
	 */
	private void closeChannel( SelectableChannel channel )
	{
		try
		{
			channel.close();
		}
		catch ( Exception e )
		{
			// we are called because of an error, so just ignore
		}
	}

	////////////////////////
	// SUBCLASS OVERRIDES //
	////////////////////////
	
	/**
	 * Starts the subclass.
	 * @throws Exception 
	 */
	abstract protected void start0() throws Exception;
	
	/**
	 * Stops the subclass.
	 * @param endTime 
	 */
	abstract protected void stop0( long endTime );
	
	/**
	 * Cleans up the subclass. Doesn't stop the selector, but removes all the
	 * handlers.
	 * @param endTime 
	 * @return true if all clean, false otherwise
	 */
	abstract protected boolean cleanup0( long endTime );

	/**
	 * @return the number of channels being managed.
	 */
	abstract public int size();
	
	/**
	 * @param todo an item to be done with the selector synchronized
	 */
	abstract public void addToTodoList( Todo todo );
	
//	/**
//	 * Registers the handler to receive notification of channel readiness
//	 * to perform i/o operations. This must be done with its selector
//	 * synchronized.
//	 * 
//	 * @param handler
//	 * 
//	 * @throws IOException
//	 */
//	abstract protected void register( Handler handler ) throws IOException;

//	/**
//	 * Sets the interest ops of the selection key of the channel of the
//	 * handler. This must be done with its selector synchronized.
//	 * 
//	 * @param handler
//	 */
//	abstract protected void setInterestOps( Handler handler );
	
//	/**
//	 * Closes the handler by calling its doClose method. This must
//	 * be done with its selector synchronized.
//	 * 
//	 * @param handler
//	 * 
//	 * @param reason
//	 */
//	abstract protected void close( Handler handler, Object reason );

	////////////////////
	// WORKERS ACTION //
	////////////////////
	
	public void run()
	{
		while (started)
		{
			try
			{
				Handler handler = getNextSelectedHandler();
				if (handler != null)
					callHandler( handler );
			}
			catch ( ClosedSelectorException e )
			{
				// ignore
			}
			catch ( Exception e )
			{
				report( e );
			}
		}
	}
	
	private void callHandler( Handler handler )
	{
		try
		{
			handler.handleSelection();
//			setInterestOps( handler ); now responsibility of handler
		}
		catch ( CancelledKeyException e ) // subclass of IllegalStateException
		{
			// this happens when the selection key has been cancelled
			// and the handler tries to update the interest ops in the
			// selection key.
			//report( e );
		}
		catch ( ClosedChannelException e ) // subclass of IOException
		{
			// this happens when the channel has been closed and
			// the handler tries to access it.
			//report( e );
		}
		catch ( EOFException e )
		{
			// the handler throws this to signal that it is done.
			handler.close( Handler.EOF );
		}
		catch ( IOException e )
		{
			// some problem with the channel or handler.
			handler.close( e );
		}
		catch ( RuntimeException e )
		{
			// some problem with the channel or handler.
			handler.close( e );
		}
	}

	/**
	 * @return a selection key if one was selected.
	 * @throws IOException 
	 */
	abstract protected Handler getNextSelectedHandler() throws IOException;

	//////////
	// MISC //
	//////////

	/**
	 * @param msg
	 */
	void report( String msg )
	{
		Trace.report( this, msg );
	}
	
	/**
	 * @param msg
	 */
	void report( M msg )
	{
		Trace.report( this, msg );
	}

	/**
	 * @param t
	 */
	void report( Throwable t )
	{
		Trace.report( this, t );
	}

	/**
	 * @param channel
	 * @param interestOps
	 * @param handler
	 * @return the selection key of the registration.
	 * @throws IOException 
	 */
	abstract SelectionKey doRegister( SelectableChannel channel, int interestOps,
		SelectableHandler handler ) throws IOException;
}
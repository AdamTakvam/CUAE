/* $Id: Server.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

import java.io.IOException;
import java.io.InterruptedIOException;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

/**
 * Implementation of server inner loop. Opens a server socket, accepts
 * connections, and notifies of various server events including shutdown.
 */
public class Server extends Thread implements ServerMBean
{
	/**
	 * Constructs a server.
	 * 
	 * @param ssf the server socket factory that will make our server socket.
	 * 
	 * @param ch the connection handler.
	 * 
	 * @param intf the interface to listen on. Pass null to listen on all
	 * interfaces.
	 * 
	 * @param port the port to listen on. Pass 0 to have a random port assigned.
	 * 
	 * @param mbs the mbean server to register with (or null if we aren't supposed
	 * to register).
	 * 
	 * @see ServerSocketFactory
	 * @see ConnectionHandler
	 */
	public Server( ServerSocketFactory ssf, ConnectionHandler ch,
		InetAddress intf, int port, Object mbs ) // MBeanServer
	{
		super( getName( ch, intf, port ) );
		this.ssf = ssf;
		this.ch = ch;
		this.intf = intf;
		this.port = port;
		this.mbs = mbs;
	}
	
	private static String getName( ConnectionHandler ch, InetAddress intf,
		int port )
	{
		return "Server Listener for "+ch.getDescription()+" on "+
			(intf != null ? intf.getHostAddress() : "*")+":"+port;
	}

	private final ServerSocketFactory ssf;
	
	private final ConnectionHandler ch;
	
	private final InetAddress intf;
	
	private int port;
	
	private final Object mbs; // MBeanServer

	private boolean running = false;

	private int total = 0;

	private final Set<ServerListener> listeners = new HashSet<ServerListener>();
	
	/**
	 * Adds a listener for server events.
	 * 
	 * @param listener a listener to receive notifications of events.
	 * 
	 * @see ServerListener
	 */
	public void addListener( ServerListener listener )
	{
		synchronized (listeners)
		{
			listeners.add( listener );
		}
	}
	
	/**
	 * Removes a listener.
	 * 
	 * @param listener a listener to receive notifications of events.
	 * 
	 * @see ServerListener
	 */
	public void removeListener( ServerListener listener )
	{
		synchronized (listeners)
		{
			listeners.remove( listener );
			if (listeners.size() == 0)
				listeners.notifyAll();
		}
	}

	private Iterator<ServerListener> getListeners()
	{
		synchronized (listeners)
		{
			return new HashSet<ServerListener>( listeners ).iterator();
		}
	}
	
	/**
	 * Returns the interface that is being listened on.
	 * 
	 * @return the interface that is being listened on. If null
	 * is returned, it means all interfaces are being listened on.
	 */
	public InetAddress getIntf()
	{
		return intf;
	}
	
	/**
	 * Returns the port that is being listened on.
	 * 
	 * @return the port that is being listened on. If 0 is returned,
	 * it means that the port was not specified and an actual port
	 * has not yet been assigned.
	 */
	public int getPort()
	{
		return port;
	}
	
	/**
	 * Returns the running state of the server.
	 * 
	 * @return true if the server is running, false otherwise.
	 */
	public boolean isRunning()
	{
		return running;
	}
	
	/**
	 * Returns the total number of sockets accepted.
	 * 
	 * @return the total number of sockets accepted.
	 */
	public int getTotal()
	{
		return total;
	}

	@Override
	public void run()
	{
//		Object name = null; // ObjectName
		try
		{
			serverSocket = ssf.newServerSocket( intf, port );
//			serverSocket.setSoTimeout( 5000 );
			
			port = serverSocket.getLocalPort();
			setName( getName( ch, intf, port ) );
			
			synchronized (this)
			{
				running = true;
				notifyAll();
			}
			
			if (mbs != null)
			{
				StringBuffer sb = new StringBuffer();
				sb.append( getClass().getName() );
				sb.append( ':' );
				if (intf != null)
				{
					sb.append( "intf=" );
					sb.append( intf.getHostAddress() );
				}
				else
				{
					sb.append( "intf=@" );
				}
				sb.append( ",port=" );
				sb.append( port );
//				name = new ObjectName( sb.toString() );
//				mbs.registerMBean( this, name );
			}
			
			fireStarted();
			
			while (running)
			{
				Socket socket = null;
				try
				{
					socket = serverSocket.accept();
					total++;
					addListener( ch.newConnection( this, socket ) );
				}
				catch ( InterruptedIOException e )
				{
					continue;
				}
				catch ( SocketException e )
				{
					String s = e.getMessage();
					if (s != null && s.toLowerCase().contains( "socket closed" ))
						continue;
					
					fireException( "accept", e );

					try
					{
						if (socket != null)
							socket.close();
					}
					catch ( Exception e1 )
					{
						fireException( "s.close", e1 );
					}
				}
				catch ( RuntimeException e )
				{
					fireException( "accept", e );

					try
					{
						if (socket != null)
							socket.close();
					}
					catch ( Exception e1 )
					{
						fireException( "s.close", e1 );
					}
				}
			}
		}
		catch ( Exception e )
		{
			fireException( "newServerSocket", e );
		}
		finally
		{
			try
			{
				if (serverSocket != null)
					serverSocket.close();
			}
			catch ( Exception e )
			{
				fireException( "ss.close", e );
			}
			
			try
			{
//				if (mbs != null)
//					mbs.unregisterMBean( name );
			}
			catch ( Exception e )
			{
				fireException( "mbs.unregisterMBean", e );
			}
			
			fireStopped();
			running = false;
		}
	}
	
	private ServerSocket serverSocket;
	
	/**
	 * Shuts down the server. The listeners are also notified
	 * of the shutdown. Listeners which are handling connections
	 * should shutdown and stop listening.
	 * 
	 * @param reason the reason for the shutdown. This message
	 * will be reported to any listeners (who might send it to
	 * a client), so be nice.
	 * 
	 * @param wait how long (in milliseconds) to wait for all
	 * the listeners to be removed.
	 * 
	 * @return true if there are no more listeners, false otherwise.
	 */
	public synchronized boolean shutdown( String reason, int wait )
	{
		if (running)
		{
			running = false;
			
			fireShutdown( reason );
			
			try
			{
				if (serverSocket != null)
					serverSocket.close();
			}
			catch ( IOException e1 )
			{
				fireException( "serverSocket.close", e1 );
			}
			
			if (wait >= 0)
			{
				long endTime = wait > 0 ?
					System.currentTimeMillis() + wait :
					Long.MAX_VALUE;
				synchronized (listeners)
				{
					while (listeners.size() > 0)
					{
						long delay = endTime - System.currentTimeMillis();
						if (delay <= 0)
							break;
						
						try
						{
							listeners.wait( delay );
						}
						catch ( InterruptedException e )
						{
							break;
						}
					}
				}
			}
		}
		return listeners.size() == 0;
	}

	@Override
	public String toString()
	{
		return getName();
	}
	
	private void fireShutdown( String reason )
	{
		Iterator<ServerListener> i = getListeners();
		while (i.hasNext())
		{
			ServerListener listener = i.next();
			listener.shutdown( this, reason );
		}
	}

	private void fireStarted()
	{
		Iterator<ServerListener> i = getListeners();
		while (i.hasNext())
		{
			ServerListener listener = i.next();
			listener.started( this );
		}
	}

	private void fireStopped()
	{
		Iterator<ServerListener> i = getListeners();
		while (i.hasNext())
		{
			ServerListener listener = i.next();
			listener.stopped( this );
		}
	}

	private void fireException( String what, Exception e )
	{
		Iterator<ServerListener> i = getListeners();
		while (i.hasNext())
		{
			ServerListener listener = i.next();
			listener.exception( this, what, e );
		}
	}

	/**
	 * Waits for the server to actually start.
	 * 
	 * @param timeout max time to wait in milliseconds.
	 * @throws InterruptedException
	 */
	public synchronized void waitStarted( int timeout ) throws InterruptedException
	{
		if (!running)
			wait( timeout );
	}
}

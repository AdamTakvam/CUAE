/* $Id: IpcConnection.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.EOFException;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketException;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

import metreos.core.net.Server;
import metreos.core.net.ServerListener;
import metreos.util.SimpleTodo;
import metreos.util.TodoManager;

/**
 * A interprocess communication connection which sends messages and
 * also distributes received messages.
 * 
 * Client Usage:
 * 
 * IpcConnection connection = new IpcConnection( host, port );
 * connection.addListener( ... );
 * connection.start();
 * ...
 * connection.send( new IpcMessage( ... ) );
 * ...
 * connection.close();
 * 
 * The listener added will receive notifications of connection events,
 * including messages received.
 * 
 * Server Usage:
 * 
 * ServerSocketFactory ssf = new PlainServerSocketFactory();
 * ConnectionHandler ch = new IpcConnectionHandler()
 * {
 *     protected void addListeners( IpcConnection connection )
 *     {
 *         connection.addListener( ... );
 *     }
 * };
 * Server server = new Server( ssf, ch, intf, port );
 * server.start();
 * 
 * The listener added will receive notifications of connection events,
 * including messages received.
 */
public class IpcConnection extends Thread implements ServerListener
{
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the socket, and also to write packets to the socket.
	 * 
	 * @param server the server which accepted the connection.
	 * 
	 * @param socket the socket of the connection.
	 * 
	 * @param todoManager the todo manager to use for queued message sending.
	 * 
	 * @throws SocketException 
	 */
	public IpcConnection( Server server, Socket socket, TodoManager todoManager )
		throws SocketException
	{
		super( "IpcConnection@"+socket.getInetAddress().getHostAddress()+
			":"+socket.getPort() );
		this.server = server;
		this.socket = socket;
		this.todoManager = todoManager;
		
		socket.setTcpNoDelay( true );
		socket.setSoLinger( true, 30 );
		socket.setKeepAlive( true );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the socket, and also to write packets to the socket.
	 * 
	 * @param server the server which accepted the connection.
	 * 
	 * @param socket the socket of the connection.
	 * 
	 * @throws SocketException 
	 */
	public IpcConnection( Server server, Socket socket ) throws SocketException
	{
		this( server, socket, null );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the socket, and also to write packets to the socket.
	 * Presumably a client created this socket.
	 * 
	 * @param socket the socket of the connection.
	 * 
	 * @param todoManager the todo manager to use for queued message sending.
	 * 
	 * @throws SocketException 
	 */
	public IpcConnection( Socket socket, TodoManager todoManager ) throws SocketException
	{
		this( null, socket, todoManager );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the socket, and also to write packets to the socket.
	 * Presumably a client created this socket.
	 * 
	 * @param socket the socket of the connection.
	 * 
	 * @throws SocketException 
	 */
	public IpcConnection( Socket socket ) throws SocketException
	{
		this( null, socket, null );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param sockAddr the socket address of the connection.
	 * 
	 * @param todoManager the todo manager to use for queued message sending.
	 * 
	 * @throws IOException if there are problems making
	 * a connection.
	 */
	public IpcConnection( InetSocketAddress sockAddr, TodoManager todoManager )
		throws IOException
	{
		this( new Socket( sockAddr.getAddress(), sockAddr.getPort() ), todoManager );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param sockAddr the socket address of the connection.
	 * 
	 * @throws IOException if there are problems making a connection.
	 */
	public IpcConnection( InetSocketAddress sockAddr )
		throws IOException
	{
		this( new Socket( sockAddr.getAddress(), sockAddr.getPort() ) );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param host the host to connect to.
	 * 
	 * @param port the port on the host to connect to.
	 * 
	 * @param todoManager the todo manager to use for queued message sending.
	 * 
	 * @throws IOException if there are problems making
	 * a connection.
	 */
	public IpcConnection( InetAddress host, int port, TodoManager todoManager )
		throws IOException
	{
		this( new Socket( host, port ), todoManager );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param host the host to connect to.
	 * 
	 * @param port the port on the host to connect to.
	 * 
	 * @throws IOException if there are problems making
	 * a connection.
	 */
	public IpcConnection( InetAddress host, int port )
		throws IOException
	{
		this( new Socket( host, port ) );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param host the host to connect to.
	 * 
	 * @param port the port on the host to connect to.
	 * 
	 * @param todoManager the todo manager to use for queued message sending.
	 * 
	 * @throws IOException if there are problems making
	 * a connection.
	 */
	public IpcConnection( String host, int port, TodoManager todoManager )
		throws IOException
	{
		this( new Socket( host, port ), todoManager );
	}
	
	/**
	 * Constructs an IpcConnection to read and process packets
	 * from the specified host and port.
	 * 
	 * @param host the host to connect to.
	 * 
	 * @param port the port on the host to connect to.
	 * 
	 * @throws IOException if there are problems making
	 * a connection.
	 */
	public IpcConnection( String host, int port )
		throws IOException
	{
		this( new Socket( host, port ) );
	}
	
	private final Server server;
	
	private final Socket socket;
	
	private final TodoManager todoManager;

	private InputStream is;

	private OutputStream os;
	
	private boolean running = false;
	
	private Set<IpcListener> listeners = new HashSet<IpcListener>();

	/**
	 * Number of bits to right shift 1st value to get the flag.
	 */
	public static final int FLAG_SHIFT = 24;

	/**
	 * Mask of flag field in 1st value.
	 */
	public static final int FLAG_MASK = 0xff;
	
	/**
	 * Mask of length field in 1st value.
	 */
	public static final int LENGTH_MASK = 0x00ffffff;

	private static final int TIMEOUT = 100;

	/**
	 * Flag which signals that an additional flag word is present.
	 */
	public static final int HAS_ADDITIONAL_FLAG = 0x80;
	
	/**
	 * Flag which signals that the content is xml instead of bytes.
	 */
	public static final int IS_XML = 0x40;

	@Override
	public String toString()
	{
		return "IpcConnection( "+socket.getInetAddress().getHostAddress()+
			":"+socket.getPort()+" )";
	}
	
	/**
	 * @return local address of connection
	 */
	public InetAddress getLocalAddress()
	{
		return socket.getLocalAddress();
	}
	
	/**
	 * @return local port of connection
	 */
	public int getLocalPort()
	{
		return socket.getLocalPort();
	}
	
	/**
	 * @return remote address of connection
	 */
	public InetAddress getRemoteAddress()
	{
		return socket.getInetAddress();
	}
	
	/**
	 * @return remote port of connection
	 */
	public int getRemotePort()
	{
		return socket.getPort();
	}

	@Override
	public void run()
	{
		try
		{
			is = new BufferedInputStream( socket.getInputStream() );
			os = new BufferedOutputStream( socket.getOutputStream() );

			synchronized (this)
			{
				running = true;
				fireStarted();
				notifyAll();
			}
			
			while (running)
			{
				int flag;
				try
				{
					flag = readInt( TIMEOUT );
				}
				catch ( EOFException e )
				{
					break;
				}
				catch ( SocketTimeoutException e )
				{
					continue;
				}
				
//				Trace.report( "flag = "+flag );
				
				int length = flag & LENGTH_MASK;
				flag = flag >>> FLAG_SHIFT;
				
				int flag2;
				if ((flag & HAS_ADDITIONAL_FLAG) != 0)
					flag2 = readInt();
				else
					flag2 = 0;
				
				byte[] bytes = readBytes( length );
				
				fireReceived( new BasicIpcMessage( flag, flag2, bytes ) );
			}
		}
		catch ( Exception e )
		{
			fireException( e );
		}
		finally
		{
			if (server != null)
				server.removeListener( this );
			closeNE();
			fireShutdown();
		}
	}
	
	private void socketClose()
	{
		try
		{
			socket.close();
		}
		catch ( Exception e )
		{
			fireException( e );
		}
	}

	/**
	 * Sends a message.
	 * 
	 * @param message
	 */
	public void send( final IpcMessage message )
	{
		if (todoManager != null)
		{
			todoManager.add( new SimpleTodo()
			{
				public void doit() throws Exception
				{
					doSend( message );
				}
			} );
		}
		else
		{
			doSend( message );
		}
	}
	
	/**
	 * @param message
	 */
	void doSend( IpcMessage message )
	{
		try
		{
			send( message.getFlag(), message.getFlag2(), message.getBytes() );
		}
		catch ( Exception e )
		{
			fireException( e );
		}
	}

	/**
	 * Sends a message to the other end of the connection.
	 * 
	 * @param flag an 8-bit flag which indicates properties of the
	 * message. Two flags are defined: HAS_ADDITIONAL_FLAG and IS_XML.
	 * 
	 * @param flag2 a 32-bit flag which indicates additional properties
	 * of the message. Only present if (flag & HAS_ADDITIONAL_FLAG) is
	 * non-zero.
	 * 
	 * @param bytes the bytes of the message. If (flag & IS_XML) is
	 * true then the bytes should be interpreted as xml, else they are
	 * binary (and probably a Flatmap). It is up to the recipient to
	 * perform any conversions.
	 * 
	 * @throws IOException if there is a problem sending. If so, the
	 * connection is closed.
	 * 
	 * @see #HAS_ADDITIONAL_FLAG
	 * @see #IS_XML
	 * @see metreos.core.ipc.flatmaps.Flatmap
	 */
	public synchronized void send( int flag, int flag2, byte[] bytes )
		throws IOException
	{
		checkRunning();
		
		if ((flag & ~FLAG_MASK) != 0)
			throw new IllegalArgumentException( "illegal bits in flag" );
		
		int length = bytes.length;
		if ((length & ~LENGTH_MASK) != 0)
			throw new IllegalArgumentException( "bytes too long" );
		
		if (flag2 == 0)
			flag &= ~HAS_ADDITIONAL_FLAG;
		else
			flag |= HAS_ADDITIONAL_FLAG;
		
		try
		{
			writeInt( (flag << FLAG_SHIFT) | length );
			
			if (flag2 != 0)
				writeInt( flag2 );
			
			os.write( bytes );
			os.flush();
		}
		catch ( IOException e )
		{
			closeNE();
			throw e;
		}
		catch ( RuntimeException e )
		{
			closeNE();
			throw e;
		}
	}

	private void checkRunning() throws IOException
	{
		if (!running)
			throw new IOException( "not running" );
	}

	/**
	 * Closes the connection. The listening thread is told to
	 * close, and closes when it is finished with its processing.
	 * 
	 * @throws IOException if the connection is already closed.
	 */
	public synchronized void close() throws IOException
	{
		checkRunning();
		closeNE();
	}

	/**
	 * Closes the connection. The listening thread is told to
	 * close, and closes when it is finished with its processing.
	 */
	public synchronized void closeNE()
	{
		if (running)
		{
			running = false;
			socketClose();
		}
	}

	/**
	 * Adds a listener to the set of listeners for this connection.
	 * Listeners are notified of connection events, including messages
	 * received.
	 * 
	 * @param listener the listener to be added.
	 */
	public void addListener( IpcListener listener )
	{
		synchronized (listeners)
		{
			listeners.add( listener );
		}
	}

	/**
	 * Removes a listener from the set of listeners for this connection.
	 * 
	 * @param listener the listener to be removed.
	 */
	public void removeListener( IpcListener listener )
	{
		synchronized (listeners)
		{
			listeners.remove( listener );
		}
	}

	private Iterator<IpcListener> getListeners()
	{
		synchronized (listeners)
		{
			return new ArrayList<IpcListener>( listeners ).iterator();
		}
	}

	/**
	 * Notifies listeners that we are starting up the connection.
	 * @throws Exception if any one on the listener's startup
	 * failed.
	 */
	@SuppressWarnings("unchecked")
	private void fireStarted() throws Exception
	{
		Iterator<IpcListener> i = getListeners();
		while (i.hasNext())
		{
			IpcListener listener = i.next();
			listener.startup( this );
		}
	}

	/**
	 * Notifies listeners that we have received a message.
	 * 
	 * @param message the message that was received.
	 */
	@SuppressWarnings("unchecked")
	private void fireReceived( IpcMessage message )
	{
		Iterator<IpcListener> i = getListeners();
		while (i.hasNext())
		{
			IpcListener listener = i.next();
			listener.received( this, message );
		}
	}

	/**
	 * Notifies listeners that we are shutting down the connection.
	 */
	@SuppressWarnings("unchecked")
	private void fireShutdown()
	{
		Iterator<IpcListener> i = getListeners();
		while (i.hasNext())
		{
			IpcListener listener = i.next();
			listener.shutdown( this );
		}
	}
	
	/**
	 * Notifies listeners that we have caught an exception during processing.
	 * 
	 * @param e the exception caught
	 */
	@SuppressWarnings("unchecked")
	private void fireException( Exception e )
	{
		Iterator<IpcListener> i = getListeners();
		while (i.hasNext())
		{
			IpcListener listener = i.next();
			listener.exception( this, e );
		}
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ServerListener#started(metreos.core.net.Server)
	 */
	public void started( Server dummy )
	{
		// ignore
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ServerListener#stopped(metreos.core.net.Server)
	 */
	public void stopped( Server dummy )
	{
		// ignore
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ServerListener#exception(metreos.core.net.Server, java.lang.String, java.lang.Exception)
	 */
	public void exception( Server dummy, String what, Exception e )
	{
		// ignore
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ServerListener#shutdown(metreos.core.net.Server, java.lang.String)
	 */
	public void shutdown( Server dummy, String reason )
	{
		closeNE();
	}

	/**
	 * Reads a 32-bit integer from the input stream.
	 * 
	 * @return a 32-bit integer.
	 * 
	 * @throws IOException if there is a problem reading.
	 */
	private int readInt()
		throws IOException
	{
		return readInt( 0 );
	}

	/**
	 * Reads a 32-bit integer from the input stream, but maybe only waits so
	 * long before it times out.
	 * 
	 * @param timeout the number of milliseconds to wait before timing out.
	 * 
	 * @return a 32-bit integer.
	 * 
	 * @throws IOException if there is a problem reading.
	 */
	private int readInt( int timeout )
		throws IOException
	{
		socket.setSoTimeout( timeout );
		int i0 = readByte();
		socket.setSoTimeout( 0 );
		
		try
		{
			int i1 = readByte();
			int i2 = readByte();
			int i3 = readByte();
			
			return (i3 << 24) | (i2 << 16) | (i1 << 8) | i0;
		}
		catch ( EOFException e )
		{
			throw new IOException( "short read" );
		}
	}

	/**
	 * Writes a 32-bit integer to the output stream.
	 * 
	 * @param value a 32-bit integer.
	 * 
	 * @throws IOException if there is a problem writing.
	 */
	private void writeInt( int value )
		throws IOException
	{
		os.write( value );
		os.write( value >>> 8 );
		os.write( value >>> 16 );
		os.write( value >>> 24 );
	}
	
	/**
	 * Reads a byte from the input stream.
	 * 
	 * @return a value between 0 and 255.
	 * 
	 * @throws IOException if there is any problem reading.
	 */
	private int readByte()
		throws IOException
	{
		int b;
		try
		{
			b = is.read();
		}
		catch ( IOException e )
		{
			String s = e.getMessage();
			
			if (s != null)
			{
				if (s.equalsIgnoreCase( "Socket Closed" ))
					throw new EOFException();
				
				if (s.equalsIgnoreCase( "Connection Reset" ))
					throw new EOFException();
			}
			
			throw e;
		}
		
		if (b < 0)
			throw new EOFException();
		
		return b;
	}

	/**
	 * Reads the specified number of bytes from the input stream.
	 * 
	 * @param length the number of bytes to read.
	 * 
	 * @return the array of bytes read
	 * 
	 * @throws IOException if there is any problem reading.
	 */
	private byte[] readBytes( int length )
		throws IOException
	{
		byte[] bytes = new byte[length];
		int offset = 0;
		try
		{
			while (length > 0)
			{
				int n = is.read( bytes, offset, length );
				offset += n;
				length -= n;
			}
		}
		catch ( EOFException e )
		{
			if (offset > 0)
				throw new IOException( "short read" );
			throw e;
		}
		return bytes;
	}

	/**
	 * Waits until the connection listener has started.
	 * 
	 * @param timeout the time in ms to wait.
	 * @throws InterruptedException
	 */
	public synchronized void waitStarted( int timeout ) throws InterruptedException
	{
		if (!running)
			wait( timeout );
	}

	/**
	 * Returns true if the connection is running.
	 * @return true if the connection is running.
	 */
	public boolean isRunning()
	{
		return running;
	}
}

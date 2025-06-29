/* $Id: StdCommander.java 12108 2005-10-19 23:47:40Z wert $
 *
 * Created by wert on May 2, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import metreos.core.net.ipc.IpcConnection;
import metreos.util.Trace;

/**
 * Description of StdCommander.
 * @param <T> The type of ipc connection that we have.
 */
abstract public class StdCommander<T extends IpcConnection>
	implements FlatmapIpcListener<T>, StdMessageDefs
{
	/**
	 * Constructs the StdCommander.
	 *
	 * @param connection
	 * @param printer 
	 * @param traceMsgs 
	 */
	public StdCommander( T connection, PrettyPrinter printer,
		boolean traceMsgs )
	{
		this.myConnection = connection;
		this.myPrinter = printer;
		this.traceMsgs = traceMsgs;
	}

	private final T myConnection;
	
	private final PrettyPrinter myPrinter;
	
	private final boolean traceMsgs;
	
	/**
	 * @return my connection
	 */
	public T getConnection()
	{
		return myConnection;
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#startup(metreos.core.net.ipc.IpcConnection)
	 */
	public void startup( T connection )
	{
		if (traceMsgs)
			report( "startup" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#received(metreos.core.net.ipc.IpcConnection, metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public void received( T connection, FlatmapIpcMessage message )
	{
		if (traceMsgs)
			Trace.report( Trace.m( "received: " ).a( message ) );
		
		try
		{
			if (!received0( connection, message ))
				reportAndSend( newErrorMessage( UnknownMessageType, message ) );
		}
		catch ( MissingFieldException e )
		{
			reportAndSend( newMissingFieldMessage( e.getField(), message ) );
		}
		catch ( Exception e )
		{
			reportAndSend( newErrorMessage( GeneralFailure, message )
					.add( ErrorMessage, e.toString() ), e );
		}
	}

	/**
	 * @param connection
	 * @param message
	 * @return true if the message was handled, false otherwise.
	 * @throws Exception if there was a problem executing the message.
	 */
	abstract protected boolean received0( T connection,
		FlatmapIpcMessage message ) throws Exception;

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( T connection )
	{
		if (traceMsgs)
			report( "shutdown" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#exception(metreos.core.net.ipc.IpcConnection, java.lang.Exception)
	 */
	public void exception( T connection, Exception e )
	{
		report( e );
	}

	/**
	 * @param args
	 * @param key the key of the field
	 * @return the value of the field
	 * @throws MissingFieldException
	 */
	public String getString( FlatmapList args, int key )
		throws MissingFieldException
	{
		String s = args.getString( key );
		if (s == null)
			throw new MissingFieldException( key );
		return s;
	}

	/**
	 * @param args
	 * @param key the key of the field
	 * @return the value of the field
	 * @throws MissingFieldException
	 */
	public int getInt( FlatmapList args, int key )
		throws MissingFieldException
	{
		Integer i = args.getInteger( key );
		if (i == null)
			throw new MissingFieldException( key );
		return i.intValue();
	}

	/**
	 * Constructs a new message with the specified type and default printer.
	 * 
	 * @param messageType the message type of the new message.
	 * 
	 * @return a new message with the specified type and default printer.
	 *  
	 */
	public FlatmapIpcMessage newMessage( int messageType )
	{
		return new FlatmapIpcMessage( messageType, myPrinter, myConnection );
	}

	/**
	 * Constructs a new error message with the specified fail reason.
	 * 
	 * @param failReason the fail reason of the new message.
	 * 
	 * @param originalMessage the original message which caused the
	 * error (if any).
	 * 
	 * @return a new error message with the specified fail reason.
	 */
	public FlatmapIpcMessage newErrorMessage( int failReason,
		FlatmapIpcMessage originalMessage )
	{
		FlatmapIpcMessage emsg = newMessage( Error )
			.add( FailReason, failReason );
		
		if (originalMessage != null)
		{
			Integer requestId = originalMessage.args.getInteger( RequestId );
			emsg.add( MessageType, originalMessage.messageType )
				.add( Args, originalMessage.args )
				.add( RequestId, requestId );
		}
		
		return emsg;
	}
	
	/**
	 * @param messageField
	 * @param originalMessage
	 * @return a new missing field error message with the messageType
	 * and messageField filled in.
	 */
	public FlatmapIpcMessage newMissingFieldMessage( int messageField,
		FlatmapIpcMessage originalMessage )
	{
		return newErrorMessage( MissingField, originalMessage )
			.add( MessageField, messageField );
	}
	
	/**
	 * @param msg
	 */
	public void report( String msg )
	{
		Trace.report( this, msg );
	}
	
	/**
	 * @param msg
	 * @param t
	 */
	public void report( String msg, Throwable t )
	{
		Trace.report( this, msg, t );
	}
	
	/**
	 * @param t
	 */
	public void report( Throwable t )
	{
		Trace.report( this, t );
	}

	/**
	 * @param msg
	 */
	public void reportAndSend( FlatmapIpcMessage msg )
	{
		if (traceMsgs)
			Trace.report( this, Trace.m( "sending message: " ).a( msg ) );
		msg.send();
	}

	/**
	 * @param msg
	 * @param t
	 */
	public void reportAndSend( FlatmapIpcMessage msg, Throwable t )
	{
		if (traceMsgs)
			Trace.report( this, Trace.m( "sending message: " ).a( msg ), t );
		msg.send();
	}
}

/* $Id: SuperSelector0.java 8308 2005-08-03 15:06:53Z wert $
 *
 * Created by wert on Apr 21, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

import metreos.util.Assertion;
import metreos.util.EmptyIterator;
import metreos.util.SimpleTodo;
import metreos.util.Todo;


/**
 * Basic super selector.
 */
public class SuperSelector0 extends SuperSelector
{
	/**
	 * Constructs the SuperSelector0.
	 *
	 * @param nWorkers
	 * @param priorityBoost 
	 */
	public SuperSelector0( int nWorkers, int priorityBoost )
	{
		super( nWorkers, priorityBoost );
	}
	
	@Override
	protected void start0() throws IOException
	{
		selector = Selector.open();
		startWorker(); // an extra one for the selector
	}

	@Override
	protected void stop0( long endTime )
	{
		// ok, because the caller is synchronized and register will blow beets
		// if started is false, we know that there will be no new registrations
		// added to the todo list. thus the close sent to each handler below
		// should stop all the handlers.
		
		cleanup( (int) (endTime-System.currentTimeMillis()) );

		try
		{
			selector.close();
		}
		catch ( Exception e )
		{
			report( e );
		}
		
		selector = null;
	}

	@Override
	protected boolean cleanup0( long endTime )
	{
		addToTodoList( new SimpleTodo()
		{
			public void doit() throws Exception
			{
				// this is called with the selector locked.
				doCleanup();
			}
		} );
		
		long now = System.currentTimeMillis();
		while (endTime > now && (size() > 0 || hasTodoList()))
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
		return size() == 0;
	}
	
	/**
	 * Performs work of cleanup0 with selector synchronized.
	 */
	void doCleanup()
	{
		synchronized (selector)
		{
			Set<SelectionKey> myKeys = new HashSet<SelectionKey>( selector.keys() );
			Iterator<SelectionKey> i = myKeys.iterator();
			while (i.hasNext())
			{
				SelectionKey key = i.next();
				Handler handler = (Handler) key.attachment();
//				report( Trace.m( "stopping " ).a( handler ) );
				handler.doClose( Handler.STOP );
			}
			myKeys.clear();
		}
	}

	/**
	 * Description of selector.
	 */
	private Selector selector;

	@Override
	public int size()
	{
		if (selector == null)
			return 0;
		
		return selector.keys().size();
	}

//	protected void register( final Handler handler )
//	{
//		if (!started)
//			throw new IllegalStateException( "not started" );
//		
//		addToTodoList( new SimpleTodo()
//		{
//			public void doit() throws Exception
//			{
//				handler.doRegister( selector );
//			}
//		} );
//	}

//	protected void setInterestOps( Handler handler )
//	{
//		addToTodoList( handler );
//	}
	
//	protected void close( final Handler handler, final Object reason )
//	{
//		addToTodoList( new SimpleTodo()
//		{
//			public void doit() throws Exception
//			{
//				handler.doClose( reason );
//			}
//		} );
//	}

	///////////////////
	// TODOLIST IMPL //
	///////////////////
	
	private boolean hasTodoList()
	{
		return todoList != null;
	}

	@Override
	public void addToTodoList( Todo todo )
	{
		synchronized (todoListSync)
		{
			synchronized (todo)
			{
				Object oldOwner = todo.getOwner();
				if (oldOwner != null)
				{
					Assertion.check( oldOwner == this, "oldOwner == this" );
					return; // already on the list, no need to do anything more...
				}
				todo.setOwner( this );
			}
			
			if (todoList != null)
			{
				todoListEnd.setNextTodo( todo );
				todoListEnd = todo;
			}
			else
			{
				todoList = todo;
				todoListEnd = todo;
				selector.wakeup();
			}
		}
	}
	
	private Todo getNextTodo()
	{
		synchronized (todoListSync)
		{
			Todo todo = todoList;
			if (todo == null)
				return null;
			
			todoList = todo.setNextTodo( null );
			
			if (todoList == null)
				todoListEnd = null;
			
			todo.setOwner( null );
			
			return todo;
		}
	}
	
	private Todo todoList;
	
	private Todo todoListEnd;
	
	private final Object todoListSync = new Object();

	private void processTodoList()
	{
		Todo todo;
		while ((todo = getNextTodo()) != null)
		{
			try
			{
				todo.doit();
			}
			catch ( Exception e )
			{
				report( e );
			}
		}
	}

	///////////////////////
	// selector handling //
	///////////////////////

	@Override
	protected Handler getNextSelectedHandler() throws IOException
	{
		Selector s = selector;
		if (s == null)
			return null;
		
		synchronized (s)
		{
			if (!keys.hasNext())
			{
				processTodoList();
				
				s.selectedKeys().clear();
				int n = s.select();
				if (n == 0)
					return null;
				
				keys = s.selectedKeys().iterator();
				Assertion.check( keys.hasNext(), "keys.hasNext()" );
			}
			
			SelectionKey key = keys.next();
			Handler handler = (Handler) key.attachment();
			try
			{
				handler.setSelectedInterestOps();
				return handler;
			}
			catch ( RuntimeException e )
			{
				handler.doClose( e );
				return null;
			}
		}
	}
	
	private Iterator<SelectionKey> keys = new EmptyIterator<SelectionKey>();

	@Override
	public SelectionKey doRegister( SelectableChannel channel, int interestOps,
		SelectableHandler handler ) throws IOException
	{
		return channel.register( selector, interestOps, handler );
	}
}

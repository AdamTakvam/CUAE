/* $Id: TodoManager.java 11802 2005-10-12 15:16:22Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * A standalone verion of a processor for todo items.
 */
public class TodoManager implements Runnable
{
	/**
	 * Constructs the TodoManager.
	 *
	 * @param nWorkers
	 */
	public TodoManager( int nWorkers )
	{
		this.nWorkers = nWorkers;
	}
	
	private final int nWorkers;
	
	/**
	 * Starts the todo manager threads.
	 * @return this todo manager
	 */
	public synchronized TodoManager start()
	{
		if (running)
			throw new IllegalStateException( "already running" );
		
		running = true;
		
		for (int i = 0; i < nWorkers; i++)
			new Thread( this, "TodoManager worker "+i ).start();
		
		return this;
	}
	
	/**
	 * Stops the todo manager threads.
	 */
	public synchronized void stop()
	{
		if (!running)
			throw new IllegalStateException( "not running" );
		
		running = false;
		notify();
	}
	
	/**
	 * @param todo
	 */
	public synchronized void add( Todo todo )
	{
		if (!running)
			throw new IllegalStateException( "not running" );
		
		synchronized (todo)
		{
			Object oldOwner = todo.getOwner();
			if (oldOwner != null)
				throw new IllegalStateException( "todo already queued" );
			
			todo.setOwner( this );
		}
		
		if (todoListEnd != null)
		{
			todoListEnd.setNextTodo( todo );
			todoListEnd = todo;
		}
		else
		{
			todoList = todo;
			todoListEnd = todo;
			notify();
		}
	}
	
	public void run()
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
				Trace.report( todo, e );
			}
		}
	}
	
	private synchronized Todo getNextTodo()
	{
		while (running && todoList == null)
		{
			try
			{
				wait( 0 );
			}
			catch ( InterruptedException e )
			{
				return null;
			}
		}
		
		Todo todo = todoList;
		if (todo == null)
			return null;
		
		todoList = todo.setNextTodo( null );
		
		if (todoList == null)
			todoListEnd = null;
		
		todo.setOwner( null );
		
		return todo;
	}
	
	private boolean running = false;
	
	private Todo todoList;
	
	private Todo todoListEnd;

	//////////////////
	// STATIC STUFF //
	//////////////////
	
	/**
	 * @param todo
	 */
	public static void addTodo( Todo todo )
	{
		getTodoManager().add( todo );
	}

	/**
	 * @return the configured todo manager. if there isn't one, it makes
	 * one with one worker thread.
	 */
	public static TodoManager getTodoManager()
	{
		if (todoManager == null)
		{
			synchronized (TodoManager.class)
			{
				if (todoManager == null)
					todoManager = new TodoManager( 1 ).start();
			}
		}
		return todoManager;
	}
	
	/**
	 * @param newTodoManager
	 * @return the old todo manager.
	 */
	public static synchronized TodoManager setTodoManager( TodoManager newTodoManager )
	{
		TodoManager oldTodoManager = todoManager;
		todoManager = newTodoManager;
		return oldTodoManager;
	}
	
	/**
	 * Shuts down the currently configured static todo manager if any.
	 */
	public static void shutdown()
	{
		TodoManager oldTodoManager = setTodoManager( null );
		if (oldTodoManager != null)
			oldTodoManager.stop();
	}
	
	private static TodoManager todoManager;
}
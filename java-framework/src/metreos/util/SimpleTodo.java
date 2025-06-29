/* $Id: SimpleTodo.java 8308 2005-08-03 15:06:53Z wert $
 *
 * Created by wert on May 23, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * Description of SimpleTodo
 */
abstract public class SimpleTodo implements Todo
{
	public Object getOwner()
	{
		return owner;
	}
	
	public void setOwner( Object newOwner )
	{
		owner = newOwner;
	}
	
	private Object owner;

	public Todo getNextTodo()
	{
		return nextTodo;
	}
	
	public Todo setNextTodo( Todo newNextTodo )
	{
		Todo oldNextTodo = nextTodo;
		nextTodo = newNextTodo;
		return oldNextTodo;
	}
	
	private Todo nextTodo;
}

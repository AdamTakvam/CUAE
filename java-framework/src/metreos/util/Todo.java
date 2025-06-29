/* $Id: Todo.java 8308 2005-08-03 15:06:53Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * A Todo is used to perform a lightweight action while allocating
 * as little storage as possible. Thus, the todo includes methods
 * for linking together todo entries and for marking the entry busy.
 * 
 * @see TodoManager
 */
public interface Todo
{
	/**
	 * @return the owner of this todo.
	 */
	public Object getOwner();
	
	/**
	 * Sets the owner of this todo. This is generally the queue that
	 * this todo is on. A todo cannot have two different owners.
	 * 
	 * @param newOwner the new owner of this todo. Null means
	 * there is no owner.
	 */
	public void setOwner( Object newOwner );

	/**
	 * @return the nextTodo of this todo.
	 */
	public Todo getNextTodo();
	
	/**
	 * Sets the nextTodo of this todo.
	 * 
	 * @param newNextTodo the new nextTodo of this todo. Null means
	 * there is no nextTodo.
	 * 
	 * @return the old nextTodo.
	 */
	public Todo setNextTodo( Todo newNextTodo );
	
	/**
	 * Performs the action.
	 * 
	 * @throws Exception if there is a problem.
	 */
	public void doit() throws Exception;
}

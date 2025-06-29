/* $Id: SuperSelector1.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Apr 21, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;

import metreos.util.Todo;

/**
 * Description of SuperSelector1.
 */
public class SuperSelector1 extends SuperSelector
{
	/**
	 * Constructs the SuperSelector1.
	 *
	 * @param nWorkers
	 * @param priorityBoost 
	 * @param maxSelectorSize
	 */
	public SuperSelector1( int nWorkers, int priorityBoost, int maxSelectorSize )
	{
		super( nWorkers, priorityBoost );
//		this.maxSelectorSize = maxSelectorSize;
	}

	@Override
	protected void start0() throws Exception
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	protected void stop0( long endTime )
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	protected boolean cleanup0( long endTime )
	{
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public int size()
	{
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	protected Handler getNextSelectedHandler()
	{
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public void addToTodoList( Todo todo )
	{
		// TODO Auto-generated method stub
		
	}

	@Override
	SelectionKey doRegister( SelectableChannel channel, int interestOps, SelectableHandler handler )
	{
		// TODO Auto-generated method stub
		return null;
	}
}

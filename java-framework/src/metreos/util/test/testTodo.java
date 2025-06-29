package metreos.util.test;

import metreos.util.SimpleTodo;
import metreos.util.TodoManager;
import metreos.util.Trace;

/**
 * Description of testTodo.
 */
public class testTodo
{
	/**
	 * @param args
	 * @throws InterruptedException
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		TodoManager.setTodoManager( new TodoManager( 20 ).start() );
		
		Thread.sleep( 10000 );
		
//		Thread.currentThread().setPriority( Thread.currentThread().getPriority()+1 );
		
		for (int i = 0; i < 100; i++)
		{
			final String msg = "todo "+i;
			TodoManager.addTodo( new SimpleTodo()
			{
				public void doit() throws Exception
				{
					Thread.sleep( 1000 );
					Trace.report( msg );
				}
			} );
		}
		
		Thread.sleep( 10000 ); // 100 todos sleeping 1000 echo with 20 threads to run them should only take 5000
		
		TodoManager.shutdown();
	}
}

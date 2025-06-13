using System;
using System.Threading;
using Metreos.Utilities;

namespace TestTimerManager
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class testTimerManager
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine( "hello world" );
			TimerManager tm = new TimerManager( "test", null, null, 1, 5 );

			tm.Add( 1000, new WakeupDelegate( wakeup1 ) );
			tm.Add( 3000, new WakeupDelegate( wakeup2 ) );
			tm.Add( 5000, new WakeupDelegate( wakeup3 ) );
			Thread.Sleep( 15000 );
			tm.RemoveAll();

			for (int i = 0; i < 500; i++)
				tm.Add( 5000, new WakeupDelegate( wakeup3 ) );
			Thread.Sleep( 120000 );
			tm.Shutdown();

			Console.WriteLine( "done" );
		}

		static long wakeup1( TimerHandle th, object state )
		{
			System.Console.WriteLine( "wakeup1 called at "+DateTime.Now );
			return 1000;
		}

		static long wakeup2( TimerHandle th, object state )
		{
			System.Console.WriteLine( "wakeup2 called at "+DateTime.Now );
			return 3000;
		}

		static long wakeup3( TimerHandle th, object state )
		{
			System.Console.WriteLine( "wakeup3 called at "+DateTime.Now );
			return 5000;
		}
	}
}

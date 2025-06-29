using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Metreos.Utilities.Selectors
{
	public class SuperSelector: SelectorBase
	{
		public SuperSelector( SelectedDelegate defaultSelected,
			SelectedExceptionDelegate defaultSelectedException,
            LogDelegate defaultLog)
			: base( defaultSelected, defaultSelectedException, defaultLog )
		{
			// nothing else to do.
		}

		protected override void DoStart()
		{
			// nothing to do
		}

		protected override void DoStop()
		{
			foreach (MiniSelector selector in GetSelectors( true ))
				selector.Stop();
		}

		private MiniSelector[] GetSelectors( bool clear )
		{
			lock (this)
			{
				MiniSelector[] x = new MiniSelector[selectors.Count];
				selectors.CopyTo( x, 0 );
				if (clear)
					selectors.Clear();
				return x;
			}
		}

		private IList selectors = new ArrayList();

		// //// //
		// KEYS //
		// //// //

		public override void Register( SelectionKey key )
		{
			lock (this)
			{
				foreach (MiniSelector selector in GetSelectors( false ))
				{
					if (selector.AvailSockets > 0)
					{
						selector.Register( key );
						return;
					}
				}
				
				// all selectors busy. add another.

				MiniSelector s = new MiniSelector( this, 0, defaultSelected, defaultSelectedException, defaultLog);
				s.Start();

				selectors.Add( s );

				s.Register( key );
			}
		}
	}
}

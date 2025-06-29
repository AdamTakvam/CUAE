using System;
using System.Collections;

namespace Metreos.Toolset.NotifyWindow
{
	/// <summary>
	/// Summary description for NotifyManager.
	/// </summary>
	public class NotifyManager : IDisposable
	{
		static NotifyManager instance = null;
		static readonly object padlock = new object();
		private OutlookNotifier lastNotifier = null;

		NotifyManager()
		{
		}

		public static NotifyManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new NotifyManager();
					}
					return instance;
				}
			}
		}

		public void AddNotifier(int width, int height)
		{
			int xo = 0;
			int yo = 0;
			if (lastNotifier != null && lastNotifier.IsDisposed == false)
			{
				// offset it
				xo = lastNotifier.Right;
				yo = lastNotifier.Top;
			}

			lastNotifier = new OutlookNotifier();
			lastNotifier.SetDimensions(width, height);

			lastNotifier.Notify(xo, yo);	
		}

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}

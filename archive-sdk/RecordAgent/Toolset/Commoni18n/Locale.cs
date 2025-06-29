using System;
using System.Globalization;

namespace Metreos.Toolset.Commoni18n
{
	public class Locale
	{
		static Locale instance = null;
		static readonly object padlock = new object();

		public delegate void LocaleChangedEventHandler(object sender, LocaleEventArgs e);
		public event LocaleChangedEventHandler LocaleChanged;

		private CultureInfo currentCulture;

		public CultureInfo CurrentCulture
		{ 
			get { return currentCulture; } 
			set 
			{ 
				if (value == currentCulture)
					return;

				currentCulture = value;
				// Raise Locale Changed event
				if (this.LocaleChanged != null)
				{
					this.LocaleChanged(this, new LocaleEventArgs(currentCulture));
				}
			} 
		}

		Locale()
		{
			// default to US English.
			currentCulture = new CultureInfo("en-US");
		}

		public static Locale Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Locale();
					}
					return instance;
				}
			}
		}
	}

	/// <summary>
	/// Summary description for LocaleChangedEvent.
	/// </summary>
	public class LocaleEventArgs : EventArgs
	{
		private CultureInfo culture;

		public CultureInfo Culture { get { return culture; } }

		public LocaleEventArgs(CultureInfo culture)
		{
			this.culture = culture;
		}
	}
}

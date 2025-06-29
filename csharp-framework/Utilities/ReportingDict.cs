using System;
using System.Collections;

namespace Metreos.Utilities
{
	public delegate void DictionaryReportDelegate( String name, IDictionary dict );

	public class ReportingDict: IDictionary
	{
		public ReportingDict( String name, IDictionary dict )
		{
			this.name = name;
			this.dict = dict;
		}

		private String name;

		private IDictionary dict;

		// spy method

		private void Spy()
		{
			lock (SyncRoot)
			{
				if (!spying)
				{
					spying = true;
					TimerManager.StaticAdd( interval, new WakeupDelegate( Report ) );
				}
			}
		}

		// wakeup method

		private long Report( TimerHandle th, object data )
		{
			lock (SyncRoot)
			{
				if (OnDictionaryReport != null)
					OnDictionaryReport( name, dict );
				
				if (Count > 0)
					return interval;
				
				spying = false;
				return 0;
			}
		}

		private bool spying;

		// static stuff

		public static IDictionary Wrap( String name, IDictionary dict )
		{
			if (interval > 0)
				return new ReportingDict( name, dict );
			
			return dict;
		}

		public static long Interval
		{
			get
			{
				return interval;
			}

			set
			{
				if (value < 0)
					throw new ArgumentException( "value < 0" );

				interval = value;
			}
		}

		private static long interval = DEFAULT_INTERVAL;

		public const long DEFAULT_INTERVAL = 60000; // 60 seconds

		public static DictionaryReportDelegate OnDictionaryReport;

		// Object overrides

		public override bool Equals(object obj)
		{
			return dict.Equals (obj);
		}

		public override int GetHashCode()
		{
			return dict.GetHashCode();
		}

		public override string ToString()
		{
			return dict.ToString ();
		}

		// ICollection override

		public void CopyTo( System.Array array, int index )
		{
			dict.CopyTo( array, index );
		}

		public object SyncRoot
		{
			get { return dict.SyncRoot; }
		}

		public int Count
		{
			get { return dict.Count; }
		}

		public bool IsSynchronized
		{
			get { return dict.IsSynchronized; }
		}

		// IDictionary override

		public bool IsFixedSize
		{
			get { return dict.IsFixedSize; }
		}

		public bool IsReadOnly
		{
			get { return dict.IsReadOnly; }
		}

		public ICollection Keys
		{
			get { return dict.Keys; }
		}

		public ICollection Values
		{
			get { return dict.Values; }
		}

		public void Add( Object key, Object value )
		{
			dict.Add( key, value );
			Spy();
		}

		public Object this[Object key]
		{
			get { return dict[key]; }
			set { dict[key] = value; Spy(); }
		}

		public void Remove( Object key )
		{
			dict.Remove( key );
		}

		public void Clear()
		{
			dict.Clear();
		}

		public bool Contains( Object key )
		{
			return dict.Contains( key );
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dict.GetEnumerator();
		}
	}
}

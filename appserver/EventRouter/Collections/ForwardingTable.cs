using System;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.AppServer.EventRouter.Collections
{
	/// <summary>
	/// Holds information regarding app-requested routing GUID forwarding
	/// </summary>
	public class ForwardingTable
	{
		private StringDictionary table;

        public int Count { get { return table.Count; } }

		public string this[string srcGuid] { get { return table[srcGuid]; } }

		public ForwardingTable()
		{
			table = new StringDictionary();
		}

		public void Add(string srcGuid, string destGuid)
		{
			table.Add(srcGuid, destGuid);
		}

		public void Remove(string destGuid)
		{
			string victim = null;

			foreach(DictionaryEntry de in table)
			{
				if(destGuid == de.Value as string)
				{
					victim = de.Key as string;
					break;
				}
			}

			if(victim != null)
			{
				table.Remove(victim);
			}
		}

		public void Clear()
		{
			table.Clear();
		}
	}
}

using System;

namespace Metreos.MMSTestTool
{
	/// <summary>
	/// Simple static class to serve out transaction IDs.
	/// The transaction IDs should range from 0 to 4,294,967,295.
	/// </summary>
	internal abstract class TransactionIdFactory
	{
		private static object lastIdLock = new Object();
		private static volatile uint lastId = 1;

		public static uint GetTransactionId()
		{
			uint id;

			lock(lastIdLock)
			{
				lastId++;

				id = lastId;
			}

			return id;
		}
	}
}

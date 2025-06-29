using System;

namespace Metreos.MmsTester.Custom.Clients
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

    public abstract class TransactionIdFactory
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


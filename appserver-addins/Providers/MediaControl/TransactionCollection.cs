using System;
using System.Diagnostics;
using System.Collections;

namespace Metreos.MediaControl
{
	/// <summary>Collection of media server transactions</summary>
	public class TransactionCollection : IEnumerable
	{
        /// <summary>A table of transactions</summary>
        /// <remarks>transactionId (uint) -> transactionInfo (TransactionInfo)</remarks>
        private readonly Hashtable transactions;

        public object SyncRoot { get { return transactions.SyncRoot; } }

        /// <summary>Returns the requested transaction object</summary>
        internal TransactionInfo this[uint transId]
        {
            get { return transactions[transId] as TransactionInfo; }
        }

		internal TransactionCollection()
		{
            this.transactions = Hashtable.Synchronized(new Hashtable());
		}

        internal void Add(TransactionInfo trans)
        {
            lock(transactions.SyncRoot)
            {
                if(transactions.Contains(trans.ID))
                {
                    Debug.Fail("Transaction already exists in table: " + trans.ID);
                    return;
                }

                transactions[trans.ID] = trans;
            }
        }

        /// <summary>
        /// Returns the requested transaction object and removes it from the collection
        /// </summary>
        /// <param name="transId">Transaction ID</param>
        /// <returns>Transaction object</returns>
        internal TransactionInfo Take(uint transId)
        {
            lock(transactions.SyncRoot)
            {
                TransactionInfo trans = transactions[transId] as TransactionInfo;
                if(trans != null)
                    transactions.Remove(transId);
                return trans;
            }
        }

        /// <summary>
        /// Removes the requested transaction object from the collection
        /// </summary>
        /// <param name="transId">Transaction ID</param>
        internal void Remove(uint transId)
        {
            transactions.Remove(transId);
        }

        /// <summary>Remove all items from the collection</summary>
        internal void Clear()
        {
            transactions.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return transactions.GetEnumerator();
        }

        #endregion
    }
}

using System;
using System.Threading;

namespace Metreos.MediaControl
{
    /// <summary>Generates unique transaction IDs</summary>
    public abstract class TransactionIdFactory
    {
        private abstract class Consts
        {
            public const int SeedValue = 0;  // Numbering will start at 1
        }

        private static int transId = Consts.SeedValue;

        /// <summary>Generates new non-zero transaction ID</summary>
        public static uint GenerateId()
        {
            uint newId = Convert.ToUInt32(Interlocked.Increment(ref transId));
            if(newId == 0)
                return GenerateId();
            return newId;
        }
    }
}

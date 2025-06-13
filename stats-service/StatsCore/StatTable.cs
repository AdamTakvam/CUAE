using System;
using System.Collections;

namespace Metreos.Stats
{
    public class StatTable
    {
        // OID (int) -> StatData
        private readonly Hashtable statTable;

        public StatTable()
        {
            statTable = Hashtable.Synchronized(new Hashtable());
        }

        public void Add(int oid, long Value)
        {
            lock(statTable.SyncRoot)
            {
                StatData data = statTable[oid] as StatData;

                if(data == null)
                {
                    data = new StatData();
                    statTable[oid] = data;
                }

                data.SetValue(Value);
            }
        }

        public bool Contains(int oid)
        {
            return statTable.Contains(oid);
        }

        public long GetAverageValue(int oid, bool reset)
        {
            StatData data = statTable[oid] as StatData;
            if(data != null)
            {
                long val = data.AvgValue;

                if(reset)
                    data.ResetInterval();

                return val;
            }
            return 0;
        }

        public long GetMaxValue(int oid, bool reset)
        {
            StatData data = statTable[oid] as StatData;
            if(data != null)
            {
                long val = data.MaxValue;

                if(reset)
                    data.ResetInterval();

                return val;
            }
            return 0;
        }

        public long GetCurrentValue(int oid)
        {
            StatData data = statTable[oid] as StatData;
            if(data != null)
                return data.CurrentValue;
            return 0;
        }
    }

    internal sealed class StatData
    {
        /// <summary>Current value</summary>
        public long CurrentValue { get { return currValue; } }
        private long currValue = 0;

        /// <summary>Maximum value received since last reset</summary>
        public long MaxValue { get { return maxValue; } }
        private long maxValue = 0;

        /// <summary>Average of all values received since last reset</summary>
        public long AvgValue { get { return avgValue; } }
        private long avgValue = 0;

        private long numValues = 0;

        public StatData() { }

        public void SetValue(long Value)
        { 
            numValues++;

            // Determine running max value
            if(Value > maxValue)
                maxValue = Value;
            
            // Calculate running average
            avgValue = (avgValue + Value) / numValues;

            currValue = Value;
        }

        /// <summary>Resets average and max values</summary>
        public void ResetInterval()
        {
            avgValue = currValue;
            maxValue = currValue;
        }
    }
}

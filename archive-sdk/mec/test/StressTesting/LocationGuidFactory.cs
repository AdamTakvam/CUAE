using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for LocationGuidFactory.
	/// </summary>
	public abstract class LocationGuidFactory
	{
        private static object lastIdLock = new Object();
        private static volatile uint lastId = 1;

        public static string GetLocationGuid()
        {
            uint id;

            lock(lastIdLock)
            {
                lastId++;

                id = lastId;
            }

            return id.ToString();
		}
	}
}

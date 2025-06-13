using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for LocationGuidFactory.
	/// </summary>
	public abstract class ConferenceGuidFactory
	{
        private static object lastIdLock = new Object();
        private static volatile int lastId = -1;

        public static string GetConferenceGuid()
        {
            int id;

            lock(lastIdLock)
            {
                lastId--;

                id = lastId;
            }

            return id.ToString();
		}
	}
}

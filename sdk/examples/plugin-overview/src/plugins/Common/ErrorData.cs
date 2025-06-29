using System;

namespace Metreos.DatabaseScraper.Common
{
	/// <summary>
	///     Encapsulates a row of the 'errors' table    
	/// </summary>
    [Serializable()]
    public class ErrorData
    {
        public string Description { get { return description; } }
        public DateTime Time { get { return time; } }

        private string description;
        private DateTime time;
		
        public ErrorData(string description, DateTime time)
		{
            this.description = description;
            this.time = time;
		}
	}
}

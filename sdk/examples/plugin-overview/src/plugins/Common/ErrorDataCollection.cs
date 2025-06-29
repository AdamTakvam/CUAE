using System;
using System.Collections;

namespace Metreos.DatabaseScraper.Common
{
	/// <summary>
	///     A collection of error data rows
	/// </summary>
    [Serializable()]
    public class ErrorDataCollection : CollectionBase, IEnumerable
    {
        public ErrorData this[int i] { get { return this.InnerList[i] as ErrorData; } }

        public ErrorDataCollection() : base()
        {
        }

        public void Add(ErrorData errorData)
        {
            this.InnerList.Add(errorData);
        }
    }
}

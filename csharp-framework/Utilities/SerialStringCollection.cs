using System;
using System.Collections.Specialized;

namespace Metreos.Utilities
{
	/// <summary>A StringCollection which can be serialized to and from a string value</summary>
	public class SerialStringCollection : StringCollection
	{
		public const char DELIMITER = '|';

        /// <summary>Default contructor</summary>
		public SerialStringCollection()
			: base() {}

        /// <summary>Constructs a StringCollection from a previously serialized version</summary>
        /// <param name="serializedString">A serialized version of this class</param>
		public SerialStringCollection(string serializedString)
			: base()
		{
			serializedString = serializedString.Trim(DELIMITER);
			string[] bits = serializedString.Split(DELIMITER);

			for(int i=0; i<bits.Length; i++)
			{
				this.Add(bits[i]);
			}
		}

        /// <summary>Serializes the contents of this collection into a string</summary>
        /// <returns>A string representation of the values in this collection (suitable for embedding in XML)</returns>
		public override string ToString()
		{
			string serializedString = "";

			for(int i=0; i<this.Count; i++)
			{
				serializedString += this[i] + DELIMITER;
			}

			return serializedString.TrimEnd(DELIMITER);
		}

	}
}

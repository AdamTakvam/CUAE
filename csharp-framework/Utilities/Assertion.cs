using System;
using System.Runtime.Serialization;

namespace Metreos.Utilities
{
	/// <summary>
	/// Assertion supports making assertions about program state.
	/// </summary>
	[Serializable]
	public class Assertion: Exception
	{
        /// <summary>For serialization use only</summary>
        public Assertion(SerializationInfo info, StreamingContext context) 
            : base(info, context) {}

		private Assertion( string msg )
			: base( msg )
		{
			// nothing to do
		}

		// ////////////// //
		// STATIC METHODS //
		// ////////////// //

		/// <summary>
		/// Checks that a given condition is true. Throws an Assertion exception
		/// if not with the message "assertion failed: "+condStr.
		/// </summary>
		/// <param name="cond">a boolean expression which should normally be true</param>
		/// <param name="condStr">a description of the above expression</param>
		/// <example>Assertion.Check( i == 17, "i == 17" );</example>
		public static void Check( bool cond, string condStr )
		{
			if (!cond)
				throw new Assertion( FAILED+condStr );
		}

		/// <summary>
		/// Checks that a given condition is true. Throws an Assertion exception
		/// if not with the message "assertion failed: "+String.Format( fmtStr, args ).
		/// </summary>
		/// <param name="cond">a boolean expression which should normally be true</param>
		/// <param name="fmtStr">a description of the above expression with argument
		/// substitutions</param>
		/// <param name="args">the args to use for formatting</param>
		/// <example>Assertion.Check( i == 17, "i == 17: i = {0}", i );</example>
		public static void Check( bool cond, string fmtStr, params object[] args )
		{
			if (!cond)
				throw new Assertion( FAILED+String.Format( fmtStr, args ) );
		}

		private const string FAILED = "assertion failed: ";
	}
}

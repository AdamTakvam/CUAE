using System;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for logging information messages
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// Adds entry to log
		/// </summary>
		/// <param name="Info">message string</param>
		/// <param name="Error">true, if the error is logged</param>
		public static void Log(string Info, bool Error)
		{
			try
			{
				if (Globals.main == null) return;
				lock(Globals.main)
				{
					Globals.main.AddLogEvent(Info, Error);
				}

			}
			catch(Exception) {}
		}

		/// <summary>
		/// Adds entry to log
		/// </summary>
		/// <param name="Info">message string</param>
		public static void Log(string Info)
		{
			Log(Info, false);
		}
		
		public Logger()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}

using System;

using JabberExample;

namespace ConsoleApplication1
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			JabberCommunication j = new JabberCommunication();

			Console.ReadLine();
		}
	}
}

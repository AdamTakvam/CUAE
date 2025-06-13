using System;

namespace Main
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			AppDeployTest.AppDeployer test = new AppDeployTest.AppDeployer(args[1], args[0]);
      test.Start();

      Console.Read();
      test.Stop();

		}
	}
}

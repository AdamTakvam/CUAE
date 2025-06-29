using System;
using Metreos.MmsTester.ClientEmulatorFramework;

namespace Metreos.MmsTester.Core
{
	/// <summary>
	/// Emulates the Application Server, in that commands relating to media are sent
	/// </summary>
	public class ApplicationServerEmulator : ClientEmulatorBase
	{

		public ApplicationServerEmulator()
		{
			displayName = "Standard Client";
		}
	}
}

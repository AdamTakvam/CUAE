using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Singleton CallID generator interface
	/// </summary>
	public interface ICallIdFactory
	{
		long GenerateCallId();
	}
}

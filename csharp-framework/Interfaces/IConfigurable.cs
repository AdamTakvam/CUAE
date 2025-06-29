using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Interface for classes which make use of global configuration variables.
	/// </summary>
	public interface IConfigurable
	{
		void RefreshConfiguration();
	}
}

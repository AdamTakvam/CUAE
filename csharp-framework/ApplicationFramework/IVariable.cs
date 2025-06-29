using System;

namespace Metreos.ApplicationFramework
{
	/// <summary>
	/// Interface for custom variables used in application scripts.
	/// </summary>
	public interface IVariable
	{
        bool Parse(string str);

        //void Reset();
	}
}

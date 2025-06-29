using System;

namespace Metreos.ApplicationFramework
{
	/// <summary>
	/// For Metreos internal use only.
	/// </summary>
	public interface IClrTypeWrapper : IVariable
	{
        object Value { get; set; }
	}
}

using System;

namespace Metreos.Interfaces
{
	/// <summary>Interface for progress bars</summary>
	public interface IProgressIndicator
	{
        int Value { get; set; }
        int Maximum { get; }
        bool Canceled { get; }
	}
}

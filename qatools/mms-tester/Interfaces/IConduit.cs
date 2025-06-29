using System;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Summary description for IConduit.
	/// </summary>
	public abstract class IConduit
	{
		public delegate InternalMessage ConduitDelegate(InternalMessage im);
        public delegate bool VisualDelegate(InternalMessage im);
	}
}

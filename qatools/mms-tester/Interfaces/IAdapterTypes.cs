using System;
using System.Collections;
using Metreos.Samoa.Core;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class IAdapterTypes
	{

        public delegate InternalMessage ResponseFromMediaServerDelegate(InternalMessage im, string messageType);

        // Look up information for adapter assemblies
        public const string MMS_MQ_ADAPTER_DISPLAY_NAME = "Message Queuing";
        public const string MMS_MQ_ADAPTER = "Metreos.MmsTester.MmsMqAdapter.MmsMqAdapter";
	}
}

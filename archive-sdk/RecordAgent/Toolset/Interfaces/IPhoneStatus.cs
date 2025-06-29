using System;

namespace Metreos.Toolset.Interfaces
{
	/// <summary>
	/// Phone related status
	/// </summary>
	public abstract class IPhoneStatus
	{
		// IM Status
		public enum PHONE_STATUS
		{
			UNKNOWN,			// UNKNOWN
			ON_HOOK,			// ON HOOK
			OFF_HOOK,			// OFF HOOK
		}

		// Default values
		public abstract class DefaultValues
		{
			public const string NUM_MISSED_CALLS				= "missed calls";
		}

		// Status change event
		public class PhoneStatusChangeEvent : EventArgs 
		{
			private PHONE_STATUS status;          
            
			public PhoneStatusChangeEvent(PHONE_STATUS status) 
			{
				this.status = status;
			}
            
			public PHONE_STATUS Status
			{
				get 
				{
					return this.status;
				}
			}
		}
	}
}

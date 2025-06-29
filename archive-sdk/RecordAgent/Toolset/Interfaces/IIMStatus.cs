using System;

namespace Metreos.Toolset.Interfaces
{
	/// <summary>
	/// IM related status
	/// </summary>
	public abstract class IIMStatus
	{
		// IM Status
		public enum IM_STATUS
		{
			UNKNOWN,			// UNKNOWN
			ONLINE,				// ONLINE
			BUSY,				// BUSY
			BE_RIGHT_BACK,		// BE RIGHT BACK
			AWAY,				// AWAY
			OUT_TO_LUNCH,		// OUT TO LUNCH
			APPEAR_OFFLINE,		// APPEAR OFFLINE
		}

		// Default values
		public abstract class DefaultValues
		{
			public const string IM_DISPLAY_ONLINE			= "Online";
			public const string IM_DISPLAY_BUSY				= "Busy";
			public const string IM_DISPLAY_BE_RIGHT_BACK    = "Be Right Back";
			public const string IM_DISPLAY_AWAY				= "Away";
			public const string IM_DISPLAY_OUT_TO_LUNCH     = "Out To Lunch";
			public const string IM_DISPLAY_APPEAR_OFFLINE   = "Appear Offline";
		}

		// Status change event
		public class IMStatusChangeEvent : EventArgs 
		{
			private IM_STATUS status;          
            
			public IMStatusChangeEvent(IM_STATUS status) 
			{
				this.status = status;
			}
            
			public IM_STATUS Status
			{
				get 
				{
					return this.status;
				}
			}
		}
	}
}


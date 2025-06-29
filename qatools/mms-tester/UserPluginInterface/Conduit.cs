using System;

using Metreos.Samoa.Core;
using Metreos.MmsTester.Interfaces;

namespace Metreos.MmsTester.Conduit
{
	/// <summary>
	/// Provides the interface to the external world for the Media Server Test Tool
	/// </summary>
	public class Conduit
	{
        

		public Conduit()
		{
			
		}

        #region Commands From External Source
        public bool SendMediaServerCommand(InternalMessage im, IConduit.ConduitDelegate outsideSourceCallback)
        {
            return false;
        }

        public bool SendSystemCommand(InternalMessage im, IConduit.ConduitDelegate outsideSourceCallback)
        {
            return false;
        }

        public bool SendTestConfigurationCommand(InternalMessage im, IConduit.ConduitDelegate outsideSourceCallback)
        {
            return false;
        }


        #endregion Commands From External Source
       
        #region Unsolicited Events

        #endregion Unsolicited Events
	}
}

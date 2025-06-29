using System;
using System.Diagnostics;



namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for EmptyProvider.
	/// </summary>
    public class EmptyProvider : ProviderBase
    {
        public EmptyProvider() : base(Constants.EMPTY_PROVIDER_NAME, Constants.EMPTY_PROVIDER_NS, TraceLevel.Info,
        Constants.EMPTY_PROVIDER_DESC, Constants.EMPTY_PROVIDER_VERSION)
        {

        }
    
        public override bool Initialize()
        {
            return true;
        }

        protected override void RefreshConfiguration()
        {
        }
	}
}

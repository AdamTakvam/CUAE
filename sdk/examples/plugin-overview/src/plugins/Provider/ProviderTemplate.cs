using System;
using System.Data; 
using System.Diagnostics;
using System.Timers;

using MySql.Data.MySqlClient;

using Metreos.Core;            
using Metreos.ProviderFramework;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
namespace ProviderTemplate
{
    [ProviderDecl("Provider Name")] 
    [PackageDecl("Provider Package Namespace", "Description of the provider")]
    public class ProviderTemplate : ProviderBase 
    {
        public ProviderTemplate(IConfigUtility configUtility) 
            : base("Provider Name", "Provider Namespace", "Provider Description", configUtility)
        {
        }

        #region ProviderBase Implementation
        protected override bool Initialize()
        {
            ///     TODO:
            ///     1.  Defining which methods handle which actions that originate from apps
            ///     2.  Defining configuration items 

            return true;
        }

        protected override void RefreshConfiguration()
        {
            ///    TODO:
            ///    1.  Retrieve configuration values, and apply to provider
        }

        /// <summary>
        ///     If this method fires, then an event we fired was handled by no application.
        /// </summary>
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {      
        }

        /// <summary>
        ///     * Runs in "your" thread – not the application manager – 
        ///           so that it doesn't slow down the startup of other providers.
        ///     * You must call RegisterNamespace() here or applications can not use the actions of this provider.
        ///     * Perform possibly time-consuming actions, e.g., initializing stack.
        ///     * Note: Your provider should not send any events 
        ///           (and will not receive any actions) until this method completes.
        /// </summary>
        protected override void OnStartup()
        {
            RegisterNamespace();

            scrapeTimer.Start();

            // Do an initial scrape
            ScrapeDatabase();

            base.OnStartup();
        }

        /// <summary>
        ///     Guaranteed to be called on a graceful shutdown of the Application Server
        /// </summary>
        protected override void OnShutdown()
        {
            if(scrapeTimer != null)
            {
                scrapeTimer.Elapsed -= timerFireDelegate;
                scrapeTimer.Close();
                scrapeTimer.Dispose();
                scrapeTimer = null;
            }
        }

        #endregion

        #region Provider Events

    

        #endregion

        #region Actions
        


        #endregion
    }
}
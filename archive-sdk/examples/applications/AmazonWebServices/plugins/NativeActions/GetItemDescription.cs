using System;
using System.Diagnostics;
using System.Collections;

using Metreos.LoggingFramework;
using Metreos.Interfaces;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;
using Metreos.AmazonWebServices.Common;

namespace Metreos.Native.AmazonWebServices
{
    /// <summary> Uses Amazon Web Services to do get more information about a specific item. </summary>
    public class GetItemDescription : INativeAction
    {
        /// <summary> The unique token to use the Amazon web service. Available from Amazon </summary>
        [ActionParamField("The unique token to use the Amazon web service. Available from Amazon", true)]
        public  string Devtag { set { devtag = value; } }
        private string devtag;

        /// <summary> The type of item to get the description for </summary>
        [ActionParamField("The type of item to get the description for", false)]
        public  string ItemType { set { itemType = value; } }
        private string itemType;

        /// <summary> The identifier for this item </summary>
        [ActionParamField("The identifier for this item", true)]
        public  string Asin { set { asin = value; } }
        private string asin;

        /// <summary> Proxy URL </summary>
        [ActionParamField("proxy", false)]
        public  string Proxy { set { proxy = value; } }
        private string proxy;

        /// <summary> Formatted text describing item </summary>
        [ResultDataField("Formatted text description of item")]
        public  string ResultData { get { return resultData; } }
        private string resultData;

        /// <summary> Makes a request to Amazon for the description of a particular item </summary>
        [Action("GetItemDescription", true, "Get Item Description", "Using the Asin, retrieves a text description of an item")]
        public string Execute(LogWriter log, SessionData sessionData, IConfigUtility configUtility)
        { 
            if(asin == String.Empty)
            {
                return IApp.VALUE_FAILURE;
            }

            resultData = AmazonUtilities.GetItemDescription(asin, itemType, devtag, proxy);

            return IApp.VALUE_SUCCESS;
        }
        
        public void Clear()
        {
            asin            = null;
            proxy           = null;
            devtag          = null;
            itemType        = null;
            resultData      = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
    }
}

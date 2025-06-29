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
    /// <summary> Uses Amazon Web Services to do a keyword search. </summary>
    [PackageDecl("Metreos.Native.AmazonWebServices", "Utilities for querying Amazon web services")]
    public class SearchByKeyword : INativeAction
    {
        /// <summary> Search phrase </summary>
        [ActionParamField("Search phrase", true)]
        public  string Keyword { set { keyword = value; } }
        private string keyword;

        /// <summary> The unique token to use the Amazon web service. Available from Amazon </summary>
        [ActionParamField("The unique token to use the Amazon web service. Available from Amazon", true)]
        public  string Devtag { set { devtag = value; } }
        private string devtag;

        /// <summary> The type of item to get the description for </summary>
        [ActionParamField("The identifier for this item", false)]
        public  string ItemType { set { itemType = value; } }
        private string itemType;

        /// <summary> Which page to return results from </summary>
        [ActionParamField("Page", true)]
        public  ushort Page { set { page = value; } }
        private ushort page;

        /// <summary> Proxy URL </summary>
        [ActionParamField("Proxy URL", false)]
        public  string Proxy { set { proxy = value; } }
        private string proxy;

        /// <summary> Formatted text strings of items </summary>
        [ResultDataField("Formatted text strings of items")]
        public  ArrayList ResultData { get { return resultData; } }
        private ArrayList resultData;

        /// <summary> Number of pages ahead </summary>
        [ResultDataField("Number of pages ahead")]
        public  ushort NumMorePages { get { return numMorePages ; } }
        private ushort numMorePages;

        /// <summary> Searches Amazon by keywords </summary>
        [Action("SearchByKeyword", true, "Search By Keyword", "Searches Amazon by keywords")]
        public string Execute(LogWriter log, SessionData sessionData, IConfigUtility configUtility)
        { 
            if(keyword == String.Empty)
            {
                return IApp.VALUE_FAILURE;
            }

            resultData = AmazonUtilities.SendKeywordRequest(keyword, itemType, proxy, devtag, page, out numMorePages);

            if(resultData == null)
            {
                resultData = new ArrayList();
                return IApp.VALUE_FAILURE;
            }
            return IApp.VALUE_SUCCESS;
        }
        
        public void Clear()
        {
            devtag          = null;
            keyword         = null;
            itemType        = null;
            resultData      = null;
            numMorePages    = 0;
            page            = 1;
        }

        public bool ValidateInput()
        {
            return true;
        }
    }
}

using System;
using System.Net;
using System.Text;
using System.Collections;

using Metreos.AmazonWebService.Common.com.amazon.soap;

namespace Metreos.AmazonWebServices.Common
{
	/// <summary> Utilites for accessing the AmazonSearchService </summary>
	public class AmazonUtilities
	{
        public const string None = "none"; // Special marker for null Asin numbers, which shouldn't occur ever anyway,
                                           // but who's to say
        
        /// <summary> Sends a basic search request to Amazon </summary>
        /// <param name="keyword"> The search phrase to search on </param>
        /// <param name="itemType"> The type of item to search on </param>
        /// <param name="proxy"> A proxy URI to use, if there is one </param>
        /// <param name="devtag"> A unique string token available from Amazon </param>
        /// <param name="currentPage"> The current page to display from this request </param>
        /// <param name="numMorePages"> How many pages are there beyond the currentPage </param>
        /// <returns> A list of string[2].  [0] is ASIN, [1] is formatted text string intended for menu item </returns>
        /// <remarks> ASIN = Amazon Standard Item Number </remarks>
        public static ArrayList SendKeywordRequest(string keyword, string itemType, 
            string proxy, string devtag, ushort currentPage, out ushort numMorePages)
        {
            try
            {
                using(AmazonSearchService service = CreateService(proxy))
                {
                    ProductInfo info = CreateKeyWordRequest(keyword, itemType, devtag, service, currentPage, out numMorePages);
                    return FormatResults(info);
                }    
            }
            catch
            {
                numMorePages = 0;
                return null;
            }
        }

        /// <summary> Provided an ASIN number (Amazon Standard Item Number) and an optional itemType, 
        ///           pull down specific information for an item. </summary>
        /// <param name="asin"> Amazon Standard Item Number </param>
        /// <param name="itemType"> The type of item being queried </param>
        /// <param name="devtag"> A unique string token available from Amazon </param>
        /// <returns> Formatted item description </returns>
        public static string GetItemDescription(string asin, string itemType, string devtag, string proxy)
        {
            try
            {
                using(AmazonSearchService service = CreateService(proxy))
                {
                    ProductInfo info = CreateAsinRequest(asin, itemType, devtag, service);
                    return FormatItemDescription(info);
                }     
            }
            catch
            {
                return "No item found matching that description";
            }
        }

        /// <summary> Create the service, using a proxy if specified </summary>
        /// <param name="proxyPath"> The URI of the proxy to use, if any </param>
        /// <returns> An initialized AmazonSearchService </returns>
        private static AmazonSearchService CreateService(string proxyPath)
        {
            AmazonSearchService service = new AmazonSearchService();

            if(proxyPath != null && proxyPath != String.Empty && proxyPath != "NULL")
            {
                WebProxy proxy = new WebProxy(proxyPath);
                service.Proxy = proxy;
            }        
            
            return service;
        }

        /// <summary> Populates the keyword request structure using combination of input parameters
        ///           and defaults </summary>
        /// <param name="keyword"> The search phrase to search on </param>
        /// <param name="itemType"> The type of item to search on </param>
        /// <param name="devtag"> A unique string token available from Amazon </param>
        /// <param name="service"> An initialized AmazonSearchService </param>
        /// <param name="currentPage"> The page to return results from </param>
        /// <param name="numMorePages"> Number of pages after this currentPage </param>
        /// <returns> The data structure containing the results of the query </returns>
        private static ProductInfo CreateKeyWordRequest(string keyword, string itemType, 
            string devtag, AmazonSearchService service, ushort currentPage, out ushort numMorePages)
        {
            if(itemType == null || itemType == None) { itemType = String.Empty; } 

            KeywordRequest keywordReq = new KeywordRequest();
            keywordReq.devtag   = devtag;
            keywordReq.keyword  = keyword;
            keywordReq.locale   = String.Empty;
            keywordReq.mode     = itemType;
            keywordReq.page     = currentPage.ToString();
            keywordReq.tag      = "webservices-20";
            keywordReq.type     = "heavy";
            keywordReq.sort     = String.Empty;

            ProductInfo info = service.KeywordSearchRequest(keywordReq);

            try
            {
                ushort totalPages = ushort.Parse(info.TotalPages);

                if(totalPages - currentPage < 0)
                {
                    numMorePages = 0;
                }

                numMorePages = (ushort)(totalPages - currentPage);
            }
            catch
            {
                numMorePages = 0;
            }

            return info;
        }

        /// <summary> Populates the ASIN request structure using combination of input parameters
        ///           and defaults </summary>
        /// <param name="asin"> Amazon Standard Item Number </param>
        /// <param name="itemType"> The type of item being queried </param>
        /// <param name="devtag"> A unique string token available from Amazon </param>
        /// <param name="service"> An initialized AmazonSearchService </param>
        /// <returns> The data structure containing the results of the query </returns>
        private static ProductInfo CreateAsinRequest(string asin, string itemType, 
            string devtag, AmazonSearchService service)
        {
            if(itemType == null || itemType == None) { itemType = String.Empty; } 

            AsinRequest asinRequest = new AsinRequest();
            asinRequest.asin        = asin;
            asinRequest.locale      = String.Empty;
            asinRequest.mode        = itemType;
            asinRequest.tag         = "webservices-20";
            asinRequest.devtag      = devtag;
            asinRequest.offer       = String.Empty;
            asinRequest.offerpage   = String.Empty;
            asinRequest.type        = "heavy";

            return service.AsinSearchRequest(asinRequest);
        }

        /// <summary> Creates an easy to use listing of items found by the search </summary>
        /// <remarks> Ease of use in the Application Development enviroment is what 
        ///           larger steers the logic in this method.  With just a cast to string[], one can show an already
        ///           formatted, short string describing each item--perfect for using with CiscoIPPhoneMenuItem.
        ///           The ASIN can be used as a query parameter, which will later be used to do an ASIN query for
        ///           that item. ASIN = Amazon Standard Item Number </remarks>
        /// <param name="info"> Data structure generated by the WSDL tools in .NET </param>
        /// <returns>  A list of string[2]'s.  [0] is ASIN, [1] is formatted text string intended for menu item </returns>
        private static ArrayList FormatResults(ProductInfo info)
        {
            if(info.Details == null || info.Details.Length == 0)
            {
                return null;
            }

            ArrayList items = new ArrayList();
    
            foreach(Details detail in info.Details)
            {
                StringBuilder sb = new StringBuilder();

                // Item information
                sb.Append(detail.ProductName);
                sb.Append("--");
                sb.Append("Price: ");
                sb.Append(detail.OurPrice);
                sb.Append(", ");
                
                // Determine author/artist/owner

                if(detail.Authors != null && detail.Authors.Length > 0)
                {
                    sb.Append("Author: ");
                    sb.Append(detail.Authors[0]);

                    if(detail.Authors.Length > 1)
                    {
                        sb.Append("...");
                    }
                }
                else if(detail.Artists != null && detail.Artists.Length > 0)
                {
                    sb.Append("Artist: ");
                    sb.Append(detail.Artists[0]);

                    if(detail.Artists.Length > 1)
                    {
                        sb.Append("...");
                    }
                }
                else if(detail.Starring != null && detail.Starring.Length > 0)
                {
                    sb.Append("Starring: ");
                    sb.Append(detail.Starring[0]);

                    if(detail.Starring.Length > 1)
                    {
                        sb.Append("...");
                    }
                }
                else if(detail.Directors != null && detail.Directors.Length > 0)
                {
                    sb.Append("Director: ");
                    sb.Append(detail.Directors[0]);

                    if(detail.Directors.Length > 1)
                    {
                        sb.Append("...");
                    }
                }
                else if(detail.Manufacturer != null && detail.Manufacturer.Length > 0)
                {
                    sb.Append("Manufacturer: ");
                    sb.Append(detail.Manufacturer[0]);

                    if(detail.Manufacturer.Length > 1)
                    {
                        sb.Append("...");
                    }
                }
                items.Add(new string[] { detail.Asin != null ? detail.Asin : None, ClampMenuText(sb) } );
            }
            return items;
        }

        /// <summary> A formatted string describing an item in detail </summary>
        /// <remarks> Ease of use in the Application Development enviroment is what 
        ///           larger steers the logic in this method.  With just a string, one can show an already
        ///           formatted, long string describing an item </remarks>
        /// <param name="info"> Data structure generated by the WSDL tools in .NET </param>
        /// <returns> Formatted text description of an item </returns>
        private static string FormatItemDescription(ProductInfo info)
        {
            // Expecting 1 item
            if(info == null || info.Details == null || info.Details.Length == 0)
            {
                return "No item found matching that description";
            }
            Details item = info.Details[0]; 

            StringBuilder sb = new StringBuilder();
            sb.Append(item.ProductName);
            sb.Append(System.Environment.NewLine);
            sb.Append(System.Environment.NewLine);
            FormatLine("Description", item.ProductDescription, sb);
            FormatLine("Amazon Price", item.OurPrice, sb);
            FormatLine("List Price", item.ListPrice, sb);
            FormatLine("Availability", item.Availability, sb);
            FormatLine("Reading Level", item.ReadingLevel, sb);
            FormatLine("Author", item.Authors, sb);
            FormatLine("Artist", item.Artists, sb);
            FormatLine("Starring", item.Starring, sb);
            FormatLine("Release Date", item.ReleaseDate, sb);
            FormatLine("Director", item.Directors, sb);
            FormatLine("Distributor", item.Distributor, sb);
            FormatLine("Manufacturer", item.Manufacturer, sb);
            
            return sb.ToString();
        }
        
        /// <summary> Prints a name-value pair with a line break after, skipping the command entirely if 
        ///           the value is null</summary>
        private static void FormatLine(string name, string @value, StringBuilder sb)
        {
            if(@value == null || @value == String.Empty) { return; } 
            sb.Append(name);
            sb.Append(": ");
            sb.Append(@value);
            sb.Append(System.Environment.NewLine);
        }

        /// <summary> Prints a name-values pair with a line break after, skipping the command entirely if 
        ///           the value is null</summary>
        private static void FormatLine(string name, string[] values, StringBuilder sb)
        {
            if(values == null || values.Length == 0) { return; } 
            sb.Append(name);
            sb.Append(": ");
            
            foreach(string @value in values)
            {
                sb.Append(@value);
                sb.Append(", ");
            }

            // Hack off last ", ";
            sb.Remove(sb.Length - 2, 2);
            sb.Append(System.Environment.NewLine);
        }

        /// <summary> Send more that 60 (or so) characters to CiscoIPPhone will cause it to display nothing,
        ///           so we clamp down the item blurb to 60 characters </summary>
        /// <remarks> Cisco documents say 64 characters, but a CiscoIPPhoneMenu failed with 64 characters </remarks>
        /// <param name="builder"> StringBuilder </param>
        /// <returns> 60 character or less string </returns>
        private static string ClampMenuText(StringBuilder builder)
        {
            if(builder.Length < 60)
            {
                return builder.ToString();
            }
            else
            {
                return builder.ToString(0, 60);
            }
        }
	}
}

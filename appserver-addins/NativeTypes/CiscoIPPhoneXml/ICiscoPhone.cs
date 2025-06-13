using System;
using Metreos.Configuration;

namespace Metreos.Types.CiscoIpPhone
{
	/// <summary>
	/// Summary description for ICiscoPhone.
	/// </summary>
	public abstract class ICiscoPhone
	{
        // These names have to match the namespace and class name *exactly*
        public const string MENU                = "Metreos.Types.CiscoIpPhone.Menu";
        public const string TEXT                = "Metreos.Types.CiscoIpPhone.Text";
        public const string INPUT               = "Metreos.Types.CiscoIpPhone.Input";
        public const string DIRECTORY           = "Metreos.Types.CiscoIpPhone.Directory";
        public const string IMAGE               = "Metreos.Types.CiscoIpPhone.Image";
        public const string IMAGE_FILE          = "Metreos.Types.CiscoIpPhone.ImageFile";
        public const string GRAPHIC_MENU        = "Metreos.Types.CiscoIpPhone.GraphicMenu";
        public const string GRAPHIC_FILE_MENU   = "Metreos.Types.CiscoIpPHone.GraphicFileMenu";
        public const string ICON_MENU           = "Metreos.Types.CiscoIpPhone.IconMenu";
        public const string EXECUTE             = "Metreos.Types.CiscoIpPhone.Execute";
        public const string RESPONSE            = "Metreos.Types.CiscoIpPhone.Response";

        public const string ELEMENT_MENU                = "CiscoIPPhoneMenu";
        public const string ELEMENT_TEXT                = "CiscoIPPhoneText";
        public const string ELEMENT_INPUT               = "CiscoIPPhoneInput";
        public const string ELEMENT_DIRECTORY           = "CiscoIPPhoneDirectory";
        public const string ELEMENT_IMAGE               = "CiscoIPPhoneImage";
        public const string ELEMENT_GRAPHIC_MENU        = "CiscoIPPhoneGraphicMenu";
        public const string ELEMENT_ICON_MENU           = "CiscoIPPhoneIconMenu";
        public const string ELEMENT_EXECUTE             = "CiscoIPPhoneExecute";
        public const string ELEMENT_IMAGE_FILE          = "CiscoIPPhoneImageFile";
        public const string ELEMENT_GRAPHIC_FILE_MENU   = "CiscoIPPhoneGraphicFileMenu";

        // Primary sub-elements
        public const string SUBELEMENT_MENU         = "MenuItem";
        public const string SUBELEMENT_SOFTKEY      = "SoftKeyItem";
        public const string SUBELEMENT_INPUT        = "InputItem";
        public const string SUBELEMENT_DIRECTORY    = "DirectoryEntry";
        public const string SUBELEMENT_ICON         = "IconItem";
        public const string SUBELEMENT_TITLE        = "Title";
        public const string SUBELEMENT_PROMPT       = "Prompt";
        public const string SUBELEMENT_TEXT         = "Text";
        public const string SUBELEMENT_URL          = "URL";
        public const string SUBELEMENT_PRIORITY     = "Priority";
        public const string SUBELEMENT_LOC_X        = "LocationX";
        public const string SUBELEMENT_LOC_Y        = "LocationY";
        public const string SUBELEMENT_WIDTH        = "Width";
        public const string SUBELEMENT_HEIGHT       = "Height";
        public const string SUBELEMENT_DEPTH        = "Depth";
        public const string SUBELEMENT_DATA         = "Data";

        // Secondary sub-elements
        public const string SUBELEMENT_INDEX        = "Index";
        public const string SUBELEMENT_ICON_INDEX   = "IconIndex";
        public const string SUBELEMENT_NAME         = "Name";
        public const string SUBELEMENT_POSITION     = "Position";
        public const string SUBELEMENT_PHONE        = "Telephone";
        public const string SUBELEMENT_DISP_NAME    = "DisplayName";
        public const string SUBELEMENT_QSTRING      = "QueryStringParam";
        public const string SUBELEMENT_DEFAULT      = "DefaultValue";
        public const string SUBELEMENT_INPUT_FLAGS  = "InputFlags";
        public const string SUBELEMENT_TOUCH_AREA   = "TouchArea";

        // Attributes
        public const string SUBELEMENT_TOUCHAREA_X1 = "TouchArea-X1";
        public const string SUBELEMENT_TOUCHAREA_X2 = "TouchArea-X2";
        public const string SUBELEMENT_TOUCHAREA_Y1 = "TouchArea-Y1";
        public const string SUBELEMENT_TOUCHAREA_Y2 = "TouchArea-Y2";

        // Parameters of the SendExecute native action
        public const string EXECUTE_MESSAGE         = "message";
        public const string EXECUTE_USERNAME        = "username";
        public const string EXECUTE_PASSWORD        = "password";

        // Additional URL's allowed for CreateExecute
        public const string SUBELEMENT_URL1 = SUBELEMENT_URL + "1";
        public const string SUBELEMENT_URL2 = SUBELEMENT_URL + "2";
        public const string SUBELEMENT_URL3 = SUBELEMENT_URL + "3";
        public const string SUBELEMENT_PRIORITY1 = SUBELEMENT_PRIORITY + "1";
        public const string SUBELEMENT_PRIORITY2 = SUBELEMENT_PRIORITY + "2";
        public const string SUBELEMENT_PRIORITY3 = SUBELEMENT_PRIORITY + "3";

        public static string FormatXML(string xml)
        {
            // Rip out xml metadata
            int start = xml.IndexOf("<?");
            int end = xml.IndexOf("?>");
            end += 4;

            if((end-start) > 0)
            {
                xml = xml.Remove(start, (end-start));
            }

            start = xml.IndexOf("xmlns");
            end = xml.IndexOf(">", start);
            start--;

            if((end-start) > 0)
            {
                xml = xml.Remove(start, (end-start));
            }

            // JDL, localization, force UTF8
            if(Config.Instance.ShouldIncludeXMLHeader())
                xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xml;

            return xml;
        }

        public static string FormatUrl(string test)
        {
            if(NeedsHttpAppend(test) == true)   return "http://" + test;
            else                                return test;
        }

        /// <summary> Tests if a URL value for a Cisco IP Phone element is prepended with http:// or exempt due to embedded phone URL type</summary>
        /// <remarks> This code was timed against a compiled regular expression to do the same check, and was 20% faster.</remarks>
        public static bool NeedsHttpAppend(string test)
        {
            if(test != null && test != String.Empty) test = test.ToLower();
            else return false;

            if(test.StartsWith("http://")) return false;

            if(test.StartsWith("key") || 
                test.StartsWith("softkey") || 
                test.StartsWith("play") ||
                test.StartsWith("dial") || 
                test.StartsWith("editdial") || 
                test.StartsWith("init") || 
                test.StartsWith("rtpmtx") || 
                test.StartsWith("rtpmrx") || 
                test.StartsWith("rtptx") || 
                test.StartsWith("rtprx"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
	}
}

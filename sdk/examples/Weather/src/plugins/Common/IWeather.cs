using System;
using System.Collections;

namespace Metreos.Weather.Common
{
	/// <summary> Common constants </summary>
	public abstract class IWeather
	{
        public const string PACKAGE_DECL = "Metreos.Native.Weather";
        public const string PACKAGE_DESC = "Handles the retrieval, display and formatting of weather information on a Cisco IP Phone.";
        public const string ACTION_GET_WEATHER_STATUS = "GetWeatherStatus";
        public const string ACTION_GET_WEATHER_STATUS_DISPLAY_NAME = "Get Weather Status";
        public const string ACTION_GET_WEATHER_STATUS_DESC = "Retrieves the weather alerts for a particular station.";
        public const string PARAM_STATE = "State";
        public const string PARAM_STATE_DESC = "State abbreviation";
        public const string TYPES_PACKAGE_DECL = "Metreos.Types.Weather";
        public const string TYPE_STATE_WEATHER = "StateWeather";
        public const string TYPE_STATE_WEATHER_DESC = "Contains the weather information for a state";
        public const string ACTION_GET_WEATHER_RESULT = "Metreos.Types.Weather.StateWeather";
        
        public static SortedList States = StationInfo.GetStates();

        #region State Abbrevation Listing
        public static string[][] stateLookupTable = new string[][] {
            new string[] { "AL", "Alabama" }, 
            new string[] { "AK", "Alaska" },
            new string[] { "AZ", "Arizona" },
            new string[] { "AR", "Arkansas" },
            new string[] { "CA", "California" },
            new string[] { "CO", "Colorado" },
            new string[] { "CT", "Connecticut" }, 
            new string[] { "DE", "Delaware" },
            new string[] { "FL", "Florida" },
            new string[] { "GA", "Georgia" },
            new string[] { "HI", "Hawaii" },
            new string[] { "ID", "Idaho" },
            new string[] { "IL", "Illinois" },
            new string[] { "IN", "Indiana" },
            new string[] { "IA", "Iowa" },
            new string[] { "KS", "Kansas" },
            new string[] { "KY", "Kentucky" }, 
            new string[] { "LA", "Louisiana" },
            new string[] { "ME", "Maine" },
            new string[] { "MD", "Maryland" },
            new string[] { "MA", "Massachusets" },
            new string[] { "MI", "Michigan" },
            new string[] { "MN", "Minnesota" },
            new string[] { "MS", "Mississippi" },
            new string[] { "MO", "Missouri" },
            new string[] { "MT", "Montana" },
            new string[] { "NE", "Nebraska" },
            new string[] { "NV", "Nevada" },
            new string[] { "NH", "New Hampshire" },
            new string[] { "NJ", "New Jersey" },
            new string[] { "NM", "New Mexico" },
            new string[] { "NY", "New York" },
            new string[] { "NC", "North Carolina" },
            new string[] { "ND", "North Dakota" },
            new string[] { "OH", "Ohio" },
            new string[] { "OK", "Oklahoma" },
            new string[] { "OR", "Oregon" },
            new string[] { "PA", "Pennsylvania" },
            new string[] { "RI", "Rhode Island" },
            new string[] { "SC", "South Carolina" },
            new string[] { "SD", "South Dakota" },
            new string[] { "TN", "Tennessee" },
            new string[] { "TX", "Texas" },
            new string[] { "UT", "Utah" },
            new string[] { "VT", "Vermont" },
            new string[] { "VA", "Virginia" },
            new string[] { "WA", "Washington" },
            new string[] { "WV", "West Virginia" },
            new string[] { "WI", "Wisconsin" },
            new string[] { "WY", "Wyoming" }
                                                 };
        public enum State
        {
            al,
            ak,
            az,
            ar,
            ca,
            co,
            ct,
            de,
            fl,
            ga,
            hi,
            id,
            il,
            @in,
            ia,
            ks,
            ky,
            la,
            me,
            md,
            ma,
            mi,
            mn,
            ms,
            mo,
            mt,
            ne,
            nv,
            nh,
            nj,
            nm,
            ny,
            nc,
            nd,
            oh,
            ok,
            or,
            pa,
            ri,
            sc,
            sd,
            tn,
            tx,
            ut,
            vt,
            va,
            wa,
            wv,
            wi,
            wy
        }
            #endregion

        public static string CreateMandatoryParamMissingError(string param, string packageName, string actionName)
        {
            return String.Format("Mandatory parameter {0} missing in action {1}.{2}", param, packageName, actionName);
        }

        public static bool IsParamDefined(string param)
        {
            return param != null ? (param != String.Empty ? true : false ) : false;
        }
	}
}

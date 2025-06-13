using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Net;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.ApplicationFramework;
using Metreos.LoggingFramework;
using Metreos.Weather.Common;

using Const = Metreos.Weather.Common.IWeather;

namespace Metreos.Native.Weather
{
  
    /// <summary> 
    ///           Makes a request to the National Weather Service for station weather information
    /// </summary>
    [PackageDecl(Const.PACKAGE_DECL, Const.PACKAGE_DESC)]
    public class GetWeatherStatus : INativeAction
    {
        private static XmlSerializer deseri = new XmlSerializer(typeof(CurrentObservation));

        public LogWriter Log { set { log = value; } }
        private LogWriter log;

        [ActionParamField("State", true)]
        public  string State { set { state = value; } }
        private string state; 

        [ActionParamField("Station", true)]
        public  string Station { set { station = value; } }
        private string station;

        [ResultDataField("Weather status of a station")]
        public  string ResultData { get { return resultData; } }
        private string resultData;

        [Action(Const.ACTION_GET_WEATHER_STATUS, false, Const.ACTION_GET_WEATHER_STATUS_DISPLAY_NAME, Const.ACTION_GET_WEATHER_STATUS_DESC)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SortedList allStations = Const.States[state] as SortedList;

            if(allStations == null)
            {
                return IApp.VALUE_FAILURE;
            }

            string[] nameAndUrl = allStations[station] as string[];

            CurrentObservation weather = null;
            bool requestSuccess = RequestWeatherInfo(nameAndUrl[1], out weather);

            if(!requestSuccess)
            {
                log.Write(TraceLevel.Error, "The weather service could not be reached.");
                return IApp.VALUE_FAILURE;
            }

            resultData = FormatWeatherString(weather);
            return IApp.VALUE_SUCCESS;
        }

        /// <summary> Contacts a web-based xml dump of station weather </summary>
        private static bool RequestWeatherInfo(string url, out CurrentObservation weather)
        {
            weather = null;

            HttpWebResponse         response        = null;
            Stream                  responseStream  = null;
            StreamReader            reader          = null;
            string                  xmlDump         = null;

            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

                request.Method = "GET";

                response = request.GetResponse() as HttpWebResponse;
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                xmlDump = reader.ReadToEnd();
            }   
            catch
            {   
                return false;
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
                if(response != null)
                {
                    response.Close();
                }
                if(responseStream != null)
                {
                    responseStream.Close();
                }
            }

            StringReader stringReader = null;
            XmlTextReader xmlReader = null;
            try
            { 
                stringReader = new StringReader(xmlDump);
                xmlReader = new XmlTextReader(stringReader);
                weather = deseri.Deserialize(xmlReader) as CurrentObservation;
            }
            catch
            {
                return false;
            }
            finally
            {
                if(stringReader != null)
                {
                    stringReader.Close();
                }
                if(xmlReader != null)
                {
                    xmlReader.Close();
                }
            }

            return true;
        }

        private string FormatWeatherString(CurrentObservation weather)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(weather.location);
            sb.Append(System.Environment.NewLine);
            AddLine(sb, "Condition", weather.weather);
            AddLine(sb, "Temperature", weather.temp_f);
            AddLine(sb, "Humidity", weather.relative_humidity);
            AddLine(sb, "Wind (mph)", weather.wind_mph);
            AddLine(sb, "Wind Desc", weather.wind_string);
            AddLine(sb, "Wind Dir", weather.wind_dir);
            AddLine(sb, "Wind Chill", weather.windchill_f);
            AddLine(sb, "Heat Index", weather.heat_index_f);
            AddLine(sb, "Heat Index Desc", weather.heat_index_string);
            AddLine(sb, "Dewpoint", weather.dewpoint_f);
            AddLine(sb, "Pressure (in)", weather.pressure_in);
            AddLine(sb, "Visibility", weather.visibility);
            AddLine(sb, "Updated", weather.observation_time);

            return sb.ToString();
        }
        private void AddLine(StringBuilder sb, string name, string @value)
        {
            if (@value == null)
            {
                sb.Append(name + ": NA");
                sb.Append(System.Environment.NewLine);
            }
            else if(0 != String.Compare(@value.Trim(), "Not Applicable"))
            {
                sb.Append(name + ": " + @value);
                sb.Append(System.Environment.NewLine);
            }
        }

        public void Clear()
        {
            station = null;
            state = null;
            resultData = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
    }
}

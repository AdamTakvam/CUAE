using System;
using System.Diagnostics;
using System.Collections;
using System.Xml.Serialization;

using Metreos.Weather.Common;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;

using Const = Metreos.Weather.Common.IWeather;

namespace Metreos.Native.Weather
{
    /// <summary> Returns long name of each station for a state </summary>
    [PackageDecl(Const.PACKAGE_DECL, Const.PACKAGE_DESC)]
    public class GetAllStations : INativeAction
    {
        public LogWriter Log { set { log = value; } }
        private LogWriter log;

        [ActionParamField("State", true)]
        public  string State { set { state = value; } }
        private string state;

        [ResultDataField("Listing of all stations for a state. Each element contains a 2 length string[] Position 0 is key, Position 1 is friendly name")]
        public  ArrayList ResultData { get { return resultData; } }
        private ArrayList resultData;

        [Action("GetAllStations", true, "Get All Stations", "Listing of all stations.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        { 
            SortedList allStations = Const.States[state] as SortedList;

            ArrayList allStationsList = new ArrayList();

            if(allStations == null)
            {
                resultData = null;
                return IApp.VALUE_FAILURE;
            }

            IDictionaryEnumerator dictEnum = allStations.GetEnumerator();
            while(dictEnum.MoveNext())
            {
                string stationId = dictEnum.Key as string;
                string[] stationNameUrl = dictEnum.Value as string[];

                string[] stationIdName = new string[] { stationId, stationNameUrl[0] };

                allStationsList.Add(stationIdName);
            }

            resultData = allStationsList;
            return IApp.VALUE_SUCCESS;
        }
        
        public void Clear()
        {
            state = null;
            resultData = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
    }
}

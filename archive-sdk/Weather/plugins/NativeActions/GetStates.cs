using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.Interfaces;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;

using Const = Metreos.Weather.Common.IWeather;

namespace Metreos.Native.Weather
{
    /// <summary> Returns each state abbreviation </summary>
    public class GetStates : INativeAction
    {

        public LogWriter Log { set { log = value; } }
        private LogWriter log;

        [ResultDataField("Listing of all states.")]
        public  ArrayList ResultData { get { return resultData; } }
        private ArrayList resultData;

        [Action("GetStates", true, "GetStates", "Listing of all states.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            ArrayList allStateNames = new ArrayList();
            foreach(string stateName in Const.States.Keys)
            {
                allStateNames.Add(stateName);
            }

            resultData = allStateNames;
            return IApp.VALUE_SUCCESS;
        }
        
        public void Clear()
        {
            resultData = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
    }
}

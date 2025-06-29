using System;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.ProcessStrings
{
    public class AddTwoStrings : INativeAction
    {
        [ActionParamField("Input String One", true)]
        public string StringOne { set { stringOne = value; } }
        private string stringOne;

        [ResultDataField("Output String")]
        public string StringTwo { get { return stringTwo; } }
        private string stringTwo;

        public LogWriter Log { get { return log; } set { log = value; } }
        private LogWriter log;

        public void Clear()
        {
            stringOne = null;
            stringTwo = null;
        }

        public AddTwoStrings()
        {
            Clear();
        }


        public bool ValidateInput()
        {
            return true;
        }

        [Action("AddTwoStrings", false, "Get Concatenated Strings", "Returns the incoming string + incoming string")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            stringTwo = stringOne + stringOne;

            return IApp.VALUE_SUCCESS;
        }
    }
}

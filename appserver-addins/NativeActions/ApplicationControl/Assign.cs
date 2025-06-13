using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Package = Metreos.Interfaces.PackageDefinitions.ApplicationControl.Actions.Assign;

namespace Metreos.Native.Standard
{
    /// <summary>
    ///     Adds easy-to-use assignment of variables
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.ApplicationControl.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.ApplicationControl.Globals.PACKAGE_DESCRIPTION)]
    public class Assign : INativeAction
    {
        private LogWriter log;
        public LogWriter Log { set { log = value; } }
        
        [ResultDataField(Package.Results.ResultData.DISPLAY, Package.Results.ResultData.DESCRIPTION)]
        public object ResultData { get { return resultData; } }

        [ResultDataField(Package.Results.ResultData2.DISPLAY, Package.Results.ResultData2.DESCRIPTION)]
        public object ResultData2 { get { return resultData2; } }

        [ResultDataField(Package.Results.ResultData3.DISPLAY, Package.Results.ResultData3.DESCRIPTION)]
        public object ResultData3 { get { return resultData3; } }

        [ResultDataField(Package.Results.ResultData4.DISPLAY, Package.Results.ResultData4.DESCRIPTION)]
        public object ResultData4 { get { return resultData4; } }

        [ActionParamField(Package.Params.Value.DISPLAY, Package.Params.Value.DESCRIPTION, false, Package.Params.Value.DEFAULT)]
        public object Value { set { _value = value; } }

        [ActionParamField(Package.Params.Value2.DISPLAY, Package.Params.Value2.DESCRIPTION, false, Package.Params.Value2.DEFAULT)]
        public object Value2 { set { _value2 = value; } }
        
        [ActionParamField(Package.Params.Value3.DISPLAY, Package.Params.Value3.DESCRIPTION, false, Package.Params.Value3.DEFAULT)]
        public object Value3 { set { _value3 = value; } }

        [ActionParamField(Package.Params.Value4.DISPLAY, Package.Params.Value4.DESCRIPTION, false, Package.Params.Value4.DEFAULT)]
        public object Value4 { set { _value4 = value; } }
        
        private object _value;
        private object _value2;
        private object _value3;
        private object _value4;

        private object resultData;
        private object resultData2;
        private object resultData3;
        private object resultData4;
                  
        public Assign() 
        {
            Clear();
        }

        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            try
            {
                resultData  = _value;
                resultData2 = _value2;
                resultData3 = _value3;
                resultData4 = _value4;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to assign.\n" + e);
                return IApp.VALUE_FAILURE;
            }
            
            return IApp.VALUE_SUCCESS;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            _value     = _value2     = _value3     = _value4     = null;
            resultData  = resultData2 = resultData3 = resultData4 = null;
        }
    }
}

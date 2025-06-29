using System;
using System.Diagnostics;
using SoapException = System.Web.Services.Protocols.SoapException;

using Metreos.LoggingFramework;

namespace Metreos.AxlSoap
{
    /// <summary>
    /// Summary description for IAxlSoap.
    /// </summary>
    public abstract class IAxlSoap
    {
        public const string DefaultCcmAdmin = "Administrator";

        public static string DetermineChosenBetweenStrings(string name, string id)
        {
            if(id != null && id != String.Empty)
            {
                return id;
            }
            else
            {
                return name;
            }
        }

        public static object DetermineChosenBetweenStringsType(string name, string id, object enum1, object enum2)
        {
            if(id != null && id != String.Empty)
            {
                return enum2;
            }
            else
            {
                return enum1;
            }
        }

        public static object ConvertEnum(object value, object original1, object original2, object replacement1, object replacement2)
        {
            if(value == original1)  return replacement1;
            else                    return replacement2;
        }

        public static void DetermineChosenBetweenStrings(string name, string id, ref string nameObj, ref string idObj)
        {
            if(id != null && id != String.Empty)
            {
                idObj = id;
            }
            else
            {
                nameObj = name;
            }
        }

        public static void UuidOrPattern(string pattern, string uuid, ref string routePartitionNameObj, ref string routeFilterNameObj, ref string uuidObj)
        {
            if(pattern != null && pattern != String.Empty)
            {
                uuid = null;
            }
            else
            {
                routePartitionNameObj = null;
                routeFilterNameObj = null;
            }
        }

        public static void ReportSoapError(SoapException e, LogWriter log, ref int code, ref string message)
        {
            try
            {
                code = int.Parse(e.Detail.ChildNodes[1].ChildNodes[1].ChildNodes[0].Value);
            }
            catch { }

            string detailXml = null;
            if(e.Detail != null)
            {
                detailXml = e.Detail.InnerXml;
                message = e.Detail.InnerText;
            }
            else
            {
                detailXml = "Unspecified";
                message = "Unspecified";
            }

            log.Write(TraceLevel.Warning, "Soap Fault.\n" + detailXml);
        }
    }
}

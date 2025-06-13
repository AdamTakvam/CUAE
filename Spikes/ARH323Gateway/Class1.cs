using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace ARH323Gateway
{
    class Class1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CommandLineArguments parser = new CommandLineArguments(args);

            string ccmIp = parser["ccmIp"][0];
            string ccmUsername = parser["ccmUser"][0];
            string ccmPassword = parser["ccmPass"][0];
            int maxAxlWrite = int.Parse(parser["axl"][0]);
            string routeList = parser["rl"][0];
            string routeListPattern = parser["rlpatt"][0];
            string routeListPartition = parser["rlpart"][0];
            string routeGroup = parser["rg"][0];
            string translationPartition = parser["tp"][0];
            string translationCss = parser["tc"][0];
            string prefixOutDigits = parser["tprefix"][0];
            string gatewaysClump = parser["g"][0];
            string gatewayCss = parser["gcss"][0];
            string deviceRangesClump = parser["devices"][0];
            
            string[] gatewayIps = gatewaysClump.Split('|');
            string[] devicesDefinition = deviceRangesClump.Split('|');
            ArrayList devicesList = new ArrayList();
            foreach(string deviceDefinition in devicesDefinition)
            {
                string[] deviceBits = deviceDefinition.Split('.');
                devicesList.Add(new GatewayManipulator.DeviceRange( long.Parse(deviceBits[0]) , int.Parse(deviceBits[1]), (GatewayManipulator.RangeType) Enum.Parse(typeof(GatewayManipulator.RangeType), deviceBits[2], true) )); 
            }
            GatewayManipulator.DeviceRange[] devices = new GatewayManipulator.DeviceRange[devicesList.Count];
            devicesList.CopyTo(devices);

            ArrayList gatewayList = new ArrayList();
            addH323Gateway[] gateways = new addH323Gateway[gatewayIps.Length];
            foreach(string gatewayIp in gatewayIps)
            {
                addH323Gateway newGateway = new addH323Gateway();
                newGateway.gateway = new XH323Gateway();
                newGateway.gateway.description = "MCE Oracle Phase 1";
                newGateway.gateway.name = gatewayIp;
                newGateway.gateway.Item2 = gatewayCss;
                gatewayList.Add(newGateway);
            }

            gatewayList.CopyTo(gateways);

            Metreos.AxlSoap413.AXLAPIService service = new AXLAPIService(ccmIp, ccmUsername, ccmPassword);

            GatewayManipulator mani = new GatewayManipulator(service, maxAxlWrite);

            addRoutePattern addRouteListPattern = new addRoutePattern();
            addRouteListPattern.newPattern = new XNPRoutePattern();
            addRouteListPattern.newPattern.pattern = routeListPattern;
            addRouteListPattern.newPattern.usage = XPatternUsage.Route;
            addRouteListPattern.newPattern.Item = routeListPartition;

            bool success = mani.CreateOraclePhase1(routeList, routeGroup, gateways, addRouteListPattern, devices, translationPartition, translationCss, prefixOutDigits);
            
            Console.WriteLine(success);
        }

      
    }
}
using System;
using System.Runtime.InteropServices;

namespace Metreos.LicensingFramework
{
    /// <summary>
    /// Wrapper class for pinvoke'd lmgr10.dll function calls. 
    /// </summary>
    internal static class FLEXlm
    {
        [DllImport("CUAELicMgr.dll", CharSet = CharSet.Ansi)]
        public static extern unsafe int getAppServerLicenseInfo([MarshalAs(UnmanagedType.Struct)] ref LicenseInformationCUAE licenseInfo);

        [DllImport("CUAELicMgr.dll", CharSet = CharSet.Ansi)]
        public static extern unsafe int validateLicenseFile([MarshalAs(UnmanagedType.LPStr)] string licensePath);    
    }
}


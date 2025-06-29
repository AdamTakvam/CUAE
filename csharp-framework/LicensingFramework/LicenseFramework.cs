using System;
using System.Runtime.InteropServices;

namespace Metreos.LicensingFramework
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct LicenseInformationCUAE
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string licenseMode;
        public int licenseModeThreshold;
        public int scriptInstances;
        public int errorCode;
        public int nMode;
    };

    public abstract class Constants
    {
        public const int CUAE_UNHANDLED_EXCEPTION = -32768;
        public const int CUAE_UNINITIALIZED = -32767;

        /// <summary>
        /// Enum of used CUAE license modes
        /// </summary>
        public enum LicenseModes
        {
            SDK      = 0,
            SmallEnv,
            Standard,
            Enhanced,
            StdEnh,
            StdPrem,
            EnhPrem,
            Premium,
            NOLIC
        };
    }

    /// <summary>
    /// Functions that utilize the FLEXlm pinvokes to check-out and check-in licensed features.
    /// </summary>
    public static class LicenseUtilities
    {
        /// <summary>
        /// Checks out a licensed feature from a license server
        /// </summary>
        /// <param name="feature">name of the feature to check out</param>
        /// <param name="version">version of the feature to check out</param>
        /// <param name="licensePath">the path of the license file/server</param>
        /// <param name="numberCheckedOut">Populated with the number of checked-out instances of the requested feature</param>
        /// <returns></returns>
        public static unsafe int CheckOut(ref LicenseInformationCUAE licenseInfo)
        {
            int errorCode = FLEXlm.getAppServerLicenseInfo(ref licenseInfo);
            return errorCode;
        }

        /// <summary>
        /// Validates that a license file is properly signed. 
        /// </summary>
        /// <param name="licenseFilePath">Path to the file to validate</param>
        /// <returns>FLEXlm integer error code</returns>
        public static unsafe int ValidateLicenseFile(string licenseFilePath)
        {
            return FLEXlm.validateLicenseFile(licenseFilePath);
        }
    }
}

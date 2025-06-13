using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LicensingFlexTrivial
{
    class FLEXlm
    {
        public const int LM_RESTRICTIVE = 0x0001;
        public const int LM_QUEUE = 0x0002;
        public const int LM_FAILSAFE = 0x0003;
        public const int LM_LENIENT = 0x0004;
        public const int LM_MANUAL_HEARTBEAT = 0x0100;
        public const int LM_RETRY_RESTRICTIVE = 0x0200;
        public const int LM_CHECK_BADDATE = 0x0800;

        #region Trivial API extern declarations
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern int lt_checkout(int iPolicy, String FeatureName, String FeatureVersion, String LicPath);
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern void lt_checkin();
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern int lt_heartbeat();
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern unsafe char* lt_errstring();
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern void lt_perror(String szErrStr);
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern void lt_pwarn(String szErrStr);
        [DllImport("lmgr10.dll", CharSet=CharSet.Ansi)]
        public static extern unsafe char* lt_warning();
        #endregion
    }
}

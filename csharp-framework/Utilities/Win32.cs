using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Metreos.Utilities
{
	/// <summary>PInvoke wrappers for Win32 calls</summary>
	/// <remarks>Just remove this class from the project and recompile 
	/// everything to see what code isn't platform-independent</remarks>
	public class Win32
	{
		[DllImport("user32.dll")]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool PostThreadMessage(
			[MarshalAs(UnmanagedType.U4)] int threadId, 
			[MarshalAs(UnmanagedType.U4)] int Msg,
			IntPtr wParam,
			IntPtr lParam);

		[DllImport("user32.dll")]
		[return:MarshalAs(UnmanagedType.Bool)]
		public static extern bool PostMessage(
			IntPtr hWnd, 
			[MarshalAs(UnmanagedType.U4)] int Msg,
			IntPtr wParam,
			IntPtr lParam);

        [DllImport("kernel32.dll",CharSet=CharSet.Auto, SetLastError=true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetEnvironmentVariable(
            string lpName, 
            string lpValue);

        [DllImport("winmm.dll")]
        public static extern int timeBeginPeriod(uint period);

        [DllImport("winmm.dll")]
        public static extern int timeEndPeriod(uint period);

        public static bool SetEnvironmentVariableEx(string environmentVariable, string variableValue)
        {
            // Get the write permission to set the environment variable.
            EnvironmentPermission environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Write,environmentVariable);
            environmentPermission.Demand(); 
            return SetEnvironmentVariable(environmentVariable, variableValue);
        }

	}

}

using System;
using System.Text;
using System.Runtime.InteropServices;


namespace Metreos.Toolset.CommonUtility
{
	public class ShellApi
	{
		// Contains information used by ShellExecuteEx
		[StructLayout(LayoutKind.Sequential)]
		public struct SHELLEXECUTEINFO
		{
			public UInt32 cbSize;					// Size of the structure, in bytes. 
			public UInt32 fMask;					// Array of flags that indicate the content and validity of the 
													// other structure members.
			public IntPtr hwnd;						// Window handle to any message boxes that the system might produce
													// while executing this function. 
			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpVerb;					// String, referred to as a verb, that specifies the action to 
													// be performed. 
			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpFile;					// Address of a null-terminated string that specifies the name of 
													// the file or object on which ShellExecuteEx will perform the 
													// action specified by the lpVerb parameter.
			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpParameters;				// Address of a null-terminated string that contains the 
													// application parameters.
			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpDirectory;				// Address of a null-terminated string that specifies the name of 
													// the working directory. 
			public Int32 nShow;						// Flags that specify how an application is to be shown when it 
													// is opened.
			public IntPtr hInstApp;					// If the function succeeds, it sets this member to a value 
													// greater than 32.
			public IntPtr lpIDList;					// Address of an ITEMIDLIST structure to contain an item identifier
													// list uniquely identifying the file to execute.
			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpClass;					// Address of a null-terminated string that specifies the name of 
													// a file class or a globally unique identifier (GUID). 
			public IntPtr hkeyClass;				// Handle to the registry key for the file class.
			public UInt32 dwHotKey;					// Hot key to associate with the application.
			public IntPtr hIconMonitor;				// Handle to the icon for the file class. OR Handle to the monitor 
													// upon which the document is to be displayed. 
			public IntPtr hProcess;					// Handle to the newly started application.
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
		public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

		// Get file information
		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath,
												uint dwFileAttributes,
												ref SHFILEINFO psfi,
												uint cbSizeFileInfo,
												uint uFlags);

		// Performs an operation on a specified file.
		[DllImport("shell32.dll")]
		public static extern IntPtr ShellExecute(
			IntPtr hwnd,			// Handle to a parent window.
			[MarshalAs(UnmanagedType.LPStr)]
			String lpOperation,		// Pointer to a null-terminated string, referred to in this case as a verb, 
									// that specifies the action to be performed.
			[MarshalAs(UnmanagedType.LPStr)]
			String lpFile,			// Pointer to a null-terminated string that specifies the file or object on which 
									// to execute the specified verb.
			[MarshalAs(UnmanagedType.LPStr)]
			String lpParameters,	// If the lpFile parameter specifies an executable file, lpParameters is a pointer 
									// to a null-terminated string that specifies the parameters to be passed 
									// to the application.
			[MarshalAs(UnmanagedType.LPStr)]
			String lpDirectory,		// Pointer to a null-terminated string that specifies the default directory. 
			Int32 nShowCmd);		// Flags that specify how an application is to be displayed when it is opened.

		// Performs an action on a file. 
		[DllImport("shell32.dll")]
		public static extern Int32 ShellExecuteEx(
			ref SHELLEXECUTEINFO lpExecInfo);	// Address of a SHELLEXECUTEINFO structure that contains and receives 
												// information about the application being executed. 

	}
}



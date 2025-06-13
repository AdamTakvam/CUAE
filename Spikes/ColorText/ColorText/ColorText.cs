// change text color in Windows console mode
// colors are 0=black 1=blue 2=green 4=red and so on to 15=white
// colorattribute = foreground + background * 16
// to get red text on yellow use 4 + 14*16 = 228
// light red on yellow would be 12 + 14*16 = 236
//
// the necessary WinApi functions are in kernel32.dll
// in C# Handle = IntPtr  DWORD = uint   WORD = int
// STD_OUTPUT_HANDLE = 0xfffffff5 (from winbase.h)
//
// this is a Console Application

using System;
using System.Runtime.InteropServices; // DllImport()

namespace ColorText
{
	class MainClass
	{
		[DllImport("kernel32.dll")]
		public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput,
			int wAttributes);
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(uint nStdHandle);

		public static void Main(string[] args)
		{
			uint STD_OUTPUT_HANDLE = 0xfffffff5;
			IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
			// increase k for more color options
			for (int k = 1; k < 255; k++)
			{
				SetConsoleTextAttribute(hConsole, k);
				Console.WriteLine("{0:d3}  I want to be nice today!",k);
			}
			// final setting
			SetConsoleTextAttribute(hConsole, 236);

			Console.WriteLine("Press Enter to exit ...");
			Console.Read();  // wait
		}
	}
}
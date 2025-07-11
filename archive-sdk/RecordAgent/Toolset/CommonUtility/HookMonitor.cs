using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Metreos.Toolset.CommonUtility
{
	internal enum Win32Hook : int
	{
		WH_MIN             = -1,
		WH_MSGFILTER       = -1,
		WH_JOURNALRECORD   =  0,
		WH_JOURNALPLAYBACK =  1,
		WH_KEYBOARD        =  2,
		WH_GETMESSAGE      =  3,
		WH_CALLWNDPROC     =  4,
		WH_CBT             =  5,
		WH_SYSMSGFILTER    =  6,
		WH_MOUSE           =  7,
		WH_HARDWARE        =  8,
		WH_DEBUG           =  9,
		WH_SHELL           = 10,
		WH_FOREGROUNDIDLE  = 11,
		WH_CALLWNDPROCRET  = 12,
		WH_KEYBOARD_LL     = 13,
		WH_MOUSE_LL        = 14
	}

	internal delegate int Win32HookProcHandler(int nCode, IntPtr wParam, IntPtr lParam);

	internal class User32
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto,
			 CallingConvention = CallingConvention.StdCall)]
		internal static extern int SetWindowsHookEx(int idHook, Win32HookProcHandler lpfn,
			IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto,
			 CallingConvention = CallingConvention.StdCall)]
		internal static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll", CharSet = CharSet.Auto,
			 CallingConvention = CallingConvention.StdCall)]
		internal static extern int CallNextHookEx(int idHook, int nCode,
			IntPtr wParam, IntPtr lParam);

		private User32()
		{
		}
	}

	/// <summary>
	/// Class for monitoring user inactivity by incorporating hooks
	/// </summary>
	public class HookMonitor : BaseMonitor
	{
		#region Private Fields

		private bool disposed    = false;
		private bool globalHooks = false;

		private int keyboardHookHandle = 0;
		private int mouseHookHandle    = 0;

		private Win32HookProcHandler keyboardHandler = null;
		private Win32HookProcHandler mouseHandler    = null;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Specifies if the instances monitors mouse events
		/// </summary>
		public override bool MonitorMouseEvents
		{
			get
			{
				return base.MonitorMouseEvents;
			}
			set
			{
				if (disposed)
					throw new ObjectDisposedException("Object has already been disposed");

				if (base.MonitorMouseEvents != value)
				{
					base.MonitorMouseEvents = value;
					if (value)
						RegisterMouseHook(globalHooks);
					else
						UnRegisterMouseHook();
				}
			}
		}

		/// <summary>
		/// Specifies if the instances monitors keyboard events
		/// </summary>
		public override bool MonitorKeyboardEvents
		{
			get
			{
				return base.MonitorKeyboardEvents;
			}
			set
			{
				if (disposed)
					throw new ObjectDisposedException("Object has already been disposed");
				
				if (base.MonitorKeyboardEvents != value)
				{
					base.MonitorKeyboardEvents = value;
					if (value)
						RegisterKeyboardHook(globalHooks);
					else
						UnRegisterKeyboardHook();
				}
			}
		}

		#endregion Public Properties

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="HookMonitor"/>
		/// </summary>
		/// <param name="global">
		/// True if the system-wide activity will be monitored, otherwise only
		/// events in the current thread will be monitored
		/// </param>
		public HookMonitor(bool global) : base()
		{
			globalHooks = global;
			if (MonitorKeyboardEvents)
				RegisterKeyboardHook(globalHooks);
			if (MonitorMouseEvents)
				RegisterMouseHook(globalHooks);
		}

		#endregion Constructors

		#region Deconstructor

		/// <summary>
		/// Deconstructor method for use by the garbage collector
		/// </summary>
		~HookMonitor()
		{
			Dispose(false);
		}

		#endregion Deconstructor

		#region Protected Methods

		/// <summary>
		/// Actual deconstructor in accordance with the dispose pattern
		/// </summary>
		/// <param name="disposing">
		/// True if managed and unmanaged resources will be freed
		/// (otherwise only unmanaged resources are handled)
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
				UnRegisterKeyboardHook();
				UnRegisterMouseHook();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Private Methods

		private void ResetBase()
		{
			if (TimeElapsed && !ReactivatedRaised)
				OnReactivated(new EventArgs());
			base.Reset();
		}

		private int KeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				base.keyboardEntered = true;
				ResetBase();
			}
			return User32.CallNextHookEx(keyboardHookHandle, nCode, wParam, lParam);
		}

		private int MouseHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				base.mouseMoved = true;
				ResetBase();
			}
			return User32.CallNextHookEx(mouseHookHandle, nCode, wParam, lParam);
		}

		private void RegisterKeyboardHook(bool global)
		{
			if (keyboardHookHandle == 0)
			{
				keyboardHandler = new Win32HookProcHandler(KeyboardHook);
				if (global)
					keyboardHookHandle = User32.SetWindowsHookEx(
						(int)Win32Hook.WH_KEYBOARD_LL, keyboardHandler,
						Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
						(int)0);
				else
					keyboardHookHandle = User32.SetWindowsHookEx(
						(int)Win32Hook.WH_KEYBOARD, keyboardHandler,
						(IntPtr)0, AppDomain.GetCurrentThreadId());
				if (keyboardHookHandle == 0)
					base.MonitorKeyboardEvents = false;
			}
		}

		private void UnRegisterKeyboardHook()
		{
			if (keyboardHookHandle != 0)
			{
				if (!User32.UnhookWindowsHookEx(keyboardHookHandle))
					base.MonitorKeyboardEvents = true;
				else
				{
					keyboardHookHandle = 0;
					keyboardHandler = null;
				}
			}
		}

		private void RegisterMouseHook(bool global)
		{
			if (mouseHookHandle == 0)
			{
				mouseHandler = new Win32HookProcHandler(MouseHook);
				if (global)
					mouseHookHandle = User32.SetWindowsHookEx(
						(int)Win32Hook.WH_MOUSE_LL, mouseHandler,
						Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
						(int)0);
				else
					mouseHookHandle = User32.SetWindowsHookEx(
						(int)Win32Hook.WH_MOUSE, mouseHandler,
						(IntPtr)0, AppDomain.GetCurrentThreadId());
				if (mouseHookHandle == 0)
					base.MonitorMouseEvents = false;
			}
		}

		private void UnRegisterMouseHook()
		{
			if (mouseHookHandle != 0)
			{
				if (!User32.UnhookWindowsHookEx(mouseHookHandle))
					base.MonitorMouseEvents = true;
				else
				{
					mouseHookHandle = 0;
					mouseHandler    = null;
				}
			}
		}

		#endregion Private Methods
	}
}

/* Shamelessly stolen from MmsController.cs. */

/*
    This class starts and stops a process. If the process terminates, this
    class starts up another to replace it. Here is a typical calling sequence:
    
    processController = new ProcessController();
    processController.Start("SccpProcess", "/noprompt",
        "x:\\Build\\AppServer\\SccpProcess.exe",
        new OnNotifyProcessToStopDelegate(KillProcessCallback), 20000);
    processController.Stop();
*/

using System;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Metreos.Utilities
{
    /// <summary>
    /// Delegate for callback into consumer so that consumer can notify the
    /// process to stop. We'd do it, but this is consumer-dependent.
    /// </summary>
    public delegate void OnNotifyProcessToStopDelegate(Process process);

    public class ProcessController : IDisposable
    {
        /// <summary>The process's executable file.</summary>
        FileInfo file;

        /// <summary>The process object.</summary>
        private Process process;

        /// <summary>Indicates whether the service is currently 
        /// shutting down.</summary>
        private volatile bool shutdownInProgress;

        /// <summary>Indicates whether we should automatically restart 
        /// the process if it terminates unexpectedly.</summary>
        private bool autoRestart;

        /// <summary>Timer used to restart the process 
        /// after it has exited without us asking it to do so.</summary>
        private Timer processRestartTimer;

        /// <summary>
        /// Delegate called when we need to notify the process to stop.
        /// </summary>
        public OnNotifyProcessToStopDelegate notifyProcessToStop;

        public ProcessController(FileInfo execFile)
        {
            this.file = execFile;

            this.notifyProcessToStop = null;
            this.autoRestart = false;
            this.shutdownInProgress = false;

            if(DiscoverProcess() == false)
            {
                InitializeProcess();
            }
        }

        /// <summary>Start the process without a window.</summary>
        /// <param name="onNotifyToStop">Callback into consumer where
        /// process is notified to terminate (kill).</param>
        /// <param name="processArgs">Process arguments, e.g.,
        /// "/noprompt".</param>
        /// <returns>True if the process was started successfuly,
        /// false otherwise.</returns>
        public bool Start(string processArgs)
        {
            return Start(processArgs, false, false);
        }

        /// <summary>Start the process.</summary>
        /// <param name="onNotifyToStop">Callback into consumer where
        /// process is notified to terminate (kill).</param>
        /// <param name="processArgs">Process arguments, e.g.,
        /// "/noprompt".</param>
        /// <param name="autoRestart">Indicates whether we should automatically restart 
        /// the process if it terminates unexpectedly.</param>
        /// <param name="window">Whether a window is created for the process to
        /// display debug info, etc.</param>
        /// <returns>True if the process was started successfuly,
        /// false otherwise.</returns>
        public bool Start(string processArgs, bool autoRestart, bool window)
        {
            Debug.Assert(processArgs != null,
                "missing process arguments (can be empty string)");

            if(process == null)
                throw new ObjectDisposedException(typeof(ProcessController).Name);

            this.autoRestart = autoRestart;

            // CreateNoWindow apparently has no effect if UseShellExecute
            // is true--the process is run within a new shell window and
            // console output goes to that window. However, if
            // UseShellExecute is false, CreateNoWindow determines whether
            // the console output is "thrown on the floor"
            // (CreateNoWindow=true) or displayed in the window of the
            // spawning parent process (CreateNoWindow=false).
            process.StartInfo.CreateNoWindow    = true;
            process.StartInfo.UseShellExecute   = window;

            process.StartInfo.Arguments         = processArgs;

            return Start();
        }
            
        /// <summary>Start the process.</summary>
        /// <returns>True if the process was started successfuly,
        /// false otherwise.</returns>
        private bool Start()
        {
            if(this.shutdownInProgress)
                return false;

            if(IsProcessRunning())
                return true;
            else
                return process.Start();
        }

        /// <summary>Stop the currently executing process.</summary>
        /// <remarks> The method will wait up to 60 seconds for
        /// the process to exit before returning.</remarks>
        /// <param name="shutdownTimeoutMs">Number of milliseconds to
        /// wait for process to terminate before killing it.</param>
        /// <returns>True if the process was stopped or if the process
        /// was not executing when the method was called; otherwise, false
        /// is returned.</returns>
        public bool Stop(int shutdownTimeoutMs)
        {
            if(process == null)
                throw new ObjectDisposedException(typeof(ProcessController).Name);

            shutdownInProgress = true;

            if (processRestartTimer != null)
            {
                processRestartTimer.Dispose();
                processRestartTimer = null;
            }

            if (IsProcessRunning() == false)
                return true;

            Debug.Assert(process != null, "process is null");

            if (notifyProcessToStop != null)
            {
                // Call back into consumer to actually notify the process to
                // stop since we don't know how to do that.
                notifyProcessToStop(process);
            }

            if(process.WaitForExit(shutdownTimeoutMs) == false)
            {
                try { process.Kill(); }
                catch {}
            }

            return process.HasExited;
        }


        /// <summary>Determines whether the process 
        /// is running or not.  If the current reference to the 
        /// process is null then discovery is attempted before 
        /// returning a result.</summary>
        /// <returns>True if the process is currently running, 
        /// false otherwise.</returns>
        public bool IsProcessRunning()
        {
            if(process == null)
                throw new ObjectDisposedException(typeof(ProcessController).Name);

            process.Refresh();
            
            bool exited = false;
            try { exited = process.HasExited; }
            catch { return false; }

            return !exited;
        }

        /// <summary>Discovers the process as it is 
        /// executing on the system.</summary>
        /// <returns>True if the process was discovered and there 
        /// are only one executing instance of the process; otherwise, 
        /// false is returned.</returns>
        private bool DiscoverProcess()
        {
            bool discovered = false;

            // Rip off the file extension to get the "friendly" name.
            string friendlyName = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
            Process[] procs = Process.GetProcessesByName(friendlyName);

            if (procs != null && procs.Length != 0)
            {
                discovered = ProcessArrayOfProcesses(procs);
            }
            else
            {
                // If regular process name didn't work, try the Purify mangled process name,
                // e.g., H323WrapperProcess --> H323WrapperProcess$Purify_X_Build_AppServer
                string mangledDirectoryName = file.DirectoryName;
                mangledDirectoryName = mangledDirectoryName.Replace(":", "");
                mangledDirectoryName = mangledDirectoryName.Replace('/', '_');
                mangledDirectoryName = mangledDirectoryName.Replace('\\', '_');
                string mangledFriendlyName = friendlyName + "$Purify_" + mangledDirectoryName;

                procs = Process.GetProcessesByName(mangledFriendlyName);
                if (procs != null && procs.Length != 0)
                {
                    discovered = ProcessArrayOfProcesses(procs);
                }
            }

            return discovered;
        }
        
        private bool ProcessArrayOfProcesses(Process[] procs)
        {
            bool discovered = false;

            if (procs.Length > 1)
            {
                // Kill all existing instances.
                foreach(Process proc in procs)
                {
                    try { proc.Kill(); }
                    catch {}
                }
            }
            else
            {
                process = procs[0];

                // Install an event handler so we are notified 
                // when the process exits.  
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(process_Exited);
                discovered = true;
            }

            return discovered;
        }

        /// <summary>Initializes a new Process object to be used 
        /// by the controller for starting and stopping 
        /// the process.</summary>
        private void InitializeProcess()
        {
            this.shutdownInProgress = false;

            if (process != null)
            {
                process.Dispose();
                process = null;
            }

            process = new Process();
            process.StartInfo.FileName           = file.FullName;
            process.StartInfo.WorkingDirectory   = file.DirectoryName;
            process.StartInfo.ErrorDialog        = true;

            // Install an event handler so we are notified 
            // when the process exits.  
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(process_Exited);
        }

        
        /// <summary>Restarts the process.  This 
        /// method is intended to be fired as a call back 
        /// from a timer.</summary>
        /// <remarks>This method will take no action if the 
        /// shutdownInProgress flag is set to true.</remarks>
        /// <param name="state">State to be used by the method.</param>
        private void RestartProcess(object state)
        {
            if (!shutdownInProgress && process != null)
            {
                bool window = process.StartInfo.UseShellExecute;
                string args = process.StartInfo.Arguments;

                Stop(0);
                InitializeProcess();
                Start(args, this.autoRestart, window);
            }

            if (processRestartTimer != null) 
            {
                processRestartTimer.Dispose();
                processRestartTimer = null;
            }
        }

        /// <summary> Event handler executed when the
        /// process exits.</summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event arguments.</param>
        private void process_Exited(object sender, EventArgs e)
        {
            if (!shutdownInProgress && autoRestart)
            {
                // Restart the process. Do it with a timer because we need
                // to give the process time to exit and our process object
                // time to update its state.
                processRestartTimer =
                    new Timer(new TimerCallback(RestartProcess), null,
                    2000, Timeout.Infinite);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (process != null)
            {
                process.Dispose();
                process = null;
            }

            if (processRestartTimer != null) 
            {
                processRestartTimer.Dispose();
                processRestartTimer = null;
            }
        }

        #endregion
    }
}

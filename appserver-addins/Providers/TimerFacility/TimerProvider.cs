using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Core.ConfigData;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Utilities;

using Package = Metreos.Interfaces.PackageDefinitions.TimerFacility;

namespace Metreos.Providers.TimerFacility
{
    /// <summary>A provider to deliver timer facilities.</summary>
    [ProviderDecl(Package.Globals.PACKAGE_NAME)]
    [PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
    public sealed class TimerFacility : ProviderBase
    {
        /// <summary>
        /// Container class to hold various pieces of timer
        /// specific information (ID, user data, recurrence, etc).
        /// </summary>
        private sealed class TimerData
        {
            public TimerHandle timer;
            public string userData;
            public bool recurrence;
            public AsyncAction action;
            public bool fireAndReschedule;
            public long rescheduleDelay;
            public long delay;

            public TimerData(TimerHandle timer, long delay, string userData, bool recurrence, 
				AsyncAction action, bool fireAndReschedule, long rescheduleDelay)
            {                
                this.timer = timer;                
                this.delay = delay;
                this.userData = userData;
                this.recurrence = recurrence;
                this.action = action;
                this.fireAndReschedule = fireAndReschedule;
                this.rescheduleDelay = rescheduleDelay;
            }
        }

		#region Constants

        private abstract class Consts
        {
			public abstract class ReservedIds
			{
				public const string MinuteTimer			= "Minutely_Timer";
				public const string HourlyTimer			= "Hourly_Timer";
				public const string DailyTimer			= "Daily_Timer";
			}

            public abstract class Defaults
            {
                public const int InitTimerThreads		= 1;	// There's no need for concurrent timer callbacks
                public const int MaxTimerThreads		= 1;    //   since we do very little work on timer fire
            }

			public readonly static long MinuteMs		= Convert.ToInt64(TimeSpan.FromMinutes(1).TotalMilliseconds);
			public readonly static long HourMs			= Convert.ToInt64(TimeSpan.FromHours(1).TotalMilliseconds);
			public readonly static long DayMs			= Convert.ToInt64(TimeSpan.FromDays(1).TotalMilliseconds);
        }

		#endregion

        /// <summary>Metreos timer manager</summary>
        private static TimerManager timerManager;
        
        /// <summary>Delegate fired when a timer event triggered</summary>
        private readonly WakeupDelegate onWakeupDelegate;

        private bool minutely = false;  // Is there a better word?
        private bool hourly = false;
        private bool daily = false;

        // Timer ID (string) -> TimerData (object)
        private readonly Hashtable timers;

        /// <summary>Construct a new timer provider.</summary>
        /// <param name="pal">A reference to the protocol abstraction layer component.</param>
        public TimerFacility(IConfigUtility configUtility)
            : base(typeof(TimerFacility), Package.Globals.DISPLAY_NAME, configUtility)
        {
            this.timers = Hashtable.Synchronized(new Hashtable());

            this.onWakeupDelegate = new WakeupDelegate(this.TimerFiredCallback);
            timerManager = new TimerManager("Timer provider timers", onWakeupDelegate, null, 
                Consts.Defaults.InitTimerThreads, Consts.Defaults.MaxTimerThreads);
        }

		#region ProviderBase Implementation

        /// <summary>Initialize the provider.</summary>
        /// <returns>True on success, false otherwise.</returns>
        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            this.messageCallbacks.Add(Package.Actions.AddTriggerTimer.FULLNAME, new HandleMessageDelegate(this.OnAddTimer));
            this.messageCallbacks.Add(Package.Actions.RemoveTimer.FULLNAME, new HandleMessageDelegate(this.OnRemoveTimer));
            this.messageCallbacks.Add(Package.Actions.AddNonTriggerTimer.FULLNAME, new HandleMessageDelegate(this.OnAddScriptTimer));

            // Set default config values
            configItems = new ConfigEntry[3];
            configItems[0] = new ConfigEntry("TimerEventsEveryMinute", "Enable Minute Events", false, "If true, minute by minute timer events will be generated", IConfig.StandardFormat.Bool, true);
            configItems[1] = new ConfigEntry("TimerEventsEveryHour", "Enable Hourly Events", false, "If true, hourly timer events will be generated", IConfig.StandardFormat.Bool, true);
            configItems[2] = new ConfigEntry("TimerEventsEveryDay", "Enable Daily Events", false, "If true, daily timer events will be generated", IConfig.StandardFormat.Bool, true);

            // No extensions
            extensions = null;

            return true;
        }

		protected override void RefreshConfiguration()
		{
            minutely = Convert.ToBoolean(GetConfigValue("TimerEventsEveryMinute"));
            hourly = Convert.ToBoolean(GetConfigValue("TimerEventsEveryHour"));
            daily = Convert.ToBoolean(GetConfigValue("TimerEventsEveryDay"));

			// Set/Clear minute timer
            if(minutely)
            {
				if(!timers.Contains(Consts.ReservedIds.MinuteTimer))
				{
                    log.Write(TraceLevel.Info, "Minute timer activated");

					TimerHandle timer = timerManager.Add(Consts.MinuteMs, Consts.ReservedIds.MinuteTimer);
					timers.Add(Consts.ReservedIds.MinuteTimer, new TimerData(timer, Consts.MinuteMs, null, true, null, false, 0));
				}
            }
            else
            {
                if(RemoveTimer(Consts.ReservedIds.MinuteTimer))
                    log.Write(TraceLevel.Info, "Minute timer deactivated");
            }

			// Set/Clear hourly timer
			if(hourly)
			{
				if(!timers.Contains(Consts.ReservedIds.HourlyTimer))
				{
                    log.Write(TraceLevel.Info, "Hourly timer activated");

					TimerHandle timer = timerManager.Add(Consts.HourMs, Consts.ReservedIds.HourlyTimer);
					timers.Add(Consts.ReservedIds.HourlyTimer, new TimerData(timer, Consts.HourMs, null, true, null, false, 0));
				}
			}
			else
			{
				if(RemoveTimer(Consts.ReservedIds.HourlyTimer))
                    log.Write(TraceLevel.Info, "Hourly timer deactivated");
			}

			// Set/Clear daily timer
			if(daily)
			{
				if(!timers.Contains(Consts.ReservedIds.DailyTimer))
				{
                    log.Write(TraceLevel.Info, "Daily timer activated");

					TimerHandle timer = timerManager.Add(Consts.DayMs, Consts.ReservedIds.DailyTimer);
					timers.Add(Consts.ReservedIds.DailyTimer, new TimerData(timer, Consts.DayMs, null, true, null, false, 0));
				}
			}
			else
			{
				if(RemoveTimer(Consts.ReservedIds.DailyTimer))
                    log.Write(TraceLevel.Info, "Daily timer deactivated");
			}
		}


        protected override void OnStartup()
        {
            this.RegisterNamespace();
        }

        protected override void OnShutdown()
        {
            ResetTimerTable();
            // Shut down timer manager
            timerManager.Shutdown();
        }

        public override void Cleanup()
        {
            ResetTimerTable();

            base.Cleanup();
        }

        #endregion

        #region Actions / Events

        /// <summary>Adds a new triggering timer.</summary>
        /// <param name="im">The message containing the add command.</param>
        [Action(Package.Actions.AddTriggerTimer.FULLNAME, false, Package.Actions.AddTriggerTimer.DISPLAY, Package.Actions.AddTriggerTimer.DESCRIPTION, false)]
        [ActionParam(Package.Actions.AddTriggerTimer.Params.timerDateTime.NAME, Package.Actions.AddTriggerTimer.Params.timerDateTime.DISPLAY, typeof(DateTime), useType.required, false, Package.Actions.AddTriggerTimer.Params.timerDateTime.DESCRIPTION, Package.Actions.AddTriggerTimer.Params.timerDateTime.DEFAULT)]
        [ActionParam(Package.Actions.AddTriggerTimer.Params.timerRecurrenceInterval.NAME, Package.Actions.AddTriggerTimer.Params.timerRecurrenceInterval.DISPLAY, typeof(TimeSpan), useType.optional, false, Package.Actions.AddTriggerTimer.Params.timerRecurrenceInterval.DESCRIPTION, Package.Actions.AddTriggerTimer.Params.timerRecurrenceInterval.DEFAULT)]
        [ActionParam(Package.Actions.AddTriggerTimer.Params.timerUserData.NAME, Package.Actions.AddTriggerTimer.Params.timerUserData.DISPLAY, typeof(string), useType.required, false, Package.Actions.AddTriggerTimer.Params.timerUserData.DESCRIPTION, Package.Actions.AddTriggerTimer.Params.timerUserData.DEFAULT)]
        [ResultData(Package.Actions.AddTriggerTimer.Results.timerId.NAME, Package.Actions.AddTriggerTimer.Results.timerId.DISPLAY, typeof(string), Package.Actions.AddTriggerTimer.Results.timerId.DESCRIPTION)]
        private void OnAddTimer(ActionBase actionBase)
        {
            AddTimer(actionBase);
        }

        /// <summary>Adds a new non-triggering timer.</summary>
        /// <param name="im">The message containing the add command.</param>
        [Action(Package.Actions.AddNonTriggerTimer.FULLNAME, false, Package.Actions.AddNonTriggerTimer.DISPLAY, Package.Actions.AddNonTriggerTimer.DESCRIPTION, false, new string[] { Package.Events.TimerFire.FULLNAME })]
        [ActionParam(Package.Actions.AddNonTriggerTimer.Params.timerDateTime.NAME, Package.Actions.AddNonTriggerTimer.Params.timerDateTime.DISPLAY, typeof(DateTime), useType.required, false, Package.Actions.AddNonTriggerTimer.Params.timerDateTime.DESCRIPTION, Package.Actions.AddNonTriggerTimer.Params.timerDateTime.DEFAULT)]
        [ActionParam(Package.Actions.AddNonTriggerTimer.Params.timerRecurrenceInterval.NAME, Package.Actions.AddNonTriggerTimer.Params.timerRecurrenceInterval.DISPLAY, typeof(TimeSpan), useType.optional, false, Package.Actions.AddNonTriggerTimer.Params.timerRecurrenceInterval.DESCRIPTION, Package.Actions.AddNonTriggerTimer.Params.timerRecurrenceInterval.DEFAULT)]
        [ActionParam(Package.Actions.AddNonTriggerTimer.Params.timerUserData.NAME, Package.Actions.AddNonTriggerTimer.Params.timerUserData.DISPLAY, typeof(string), useType.optional, false, Package.Actions.AddNonTriggerTimer.Params.timerUserData.DESCRIPTION, Package.Actions.AddNonTriggerTimer.Params.timerUserData.DEFAULT)]
        [ResultData(Package.Actions.AddNonTriggerTimer.Results.timerId.NAME, Package.Actions.AddNonTriggerTimer.Results.timerId.DISPLAY, typeof(string), Package.Actions.AddNonTriggerTimer.Results.timerId.DESCRIPTION)]
        private void OnAddScriptTimer(ActionBase actionBase)
        {
            AddTimer(actionBase);
        }

		/// <summary>Adds a triggering or non-triggering timer</summary>
		/// <param name="action">AddTimer or AddScriptTimer action</param>
		private void AddTimer(ActionBase action)
		{
			DateTime initialTime;
			TimeSpan timerPeriod;
			string userData;

			bool timerPresent;
			bool timerPeriodPresent;
			try
			{
				action.InnerMessage.GetString(Package.Actions.AddTriggerTimer.Params.timerUserData.NAME, false, out userData);
                timerPresent = action.InnerMessage.GetDateTime(Package.Actions.AddTriggerTimer.Params.timerDateTime.NAME, true, out initialTime);
                timerPeriodPresent = action.InnerMessage.GetTimeSpan(Package.Actions.AddTriggerTimer.Params.timerRecurrenceInterval.NAME, false, out timerPeriod);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, e.Message);
				action.SendResponse(false);
				return;
			}

			// No timer data or recurrence interval
			if (!timerPresent && !timerPeriodPresent)
			{
				log.Write(TraceLevel.Error, "No timer data or recurrence interval.");
				action.SendResponse(false);
				return;
			}
         
			TimeSpan initialTimerTarget = TimeSpan.Zero;
			if (timerPresent)
			{
				// How far from now should the timer fire, initially?
				initialTimerTarget = initialTime.Subtract(DateTime.Now);

				// MSC: added to squash any negative initial timer times.  Reasoning: if the user enters System.DateTime.Now into MAX, 
				// it is rather undesirable that it would throw the ArgumentOutOfRangeException. So, we will simple force all negative TimeSpans
				// to be concated to 0, at the expense of hiding the possibility of completely wrong math on the part of the user 
				// (for instance, if the user was somehow incorrectly computing the initial time to be 1981, they would perhaps like to know 
				// that they are doing something incorrect.  So, the best course is to throw a warning...
				if(TimeSpan.Zero > initialTimerTarget)
				{
					log.Write(TraceLevel.Warning, "The field '" + Package.Actions.AddTriggerTimer.Params.timerDateTime.NAME + "' with a value of '" + initialTime + "' was found to have a target date time in the past.  " + 
						"The initial date time will be reset to DateTime.Now");

					initialTimerTarget = TimeSpan.Zero;
				}
			}

			string timerGuid = System.Guid.NewGuid().ToString();

			TimerHandle newTimer = null;
			long delay = 0;
			long rescheduleDelay = 0;
			bool fireAndReschedule = false;

			if (timerPresent && timerPeriodPresent)
			{
				// fire one time and recurrence
				if (initialTimerTarget == TimeSpan.Zero)
					delay = 1;      // set to 1 ms if already passed
				else
					delay = Convert.ToInt32(initialTimerTarget.TotalMilliseconds);

				rescheduleDelay = Convert.ToInt32(timerPeriod.TotalMilliseconds);
				fireAndReschedule = true;
			}
			else if (timerPeriodPresent)
			{
				// recurrence only
				delay = Convert.ToInt32(timerPeriod.TotalMilliseconds);
			}
			else if (timerPresent)
			{
				// fire one time only
				if (initialTimerTarget == TimeSpan.Zero)
					delay = 1;      // set to 1 ms if already passed
				else
					delay = Convert.ToInt32(initialTimerTarget.TotalMilliseconds);
			}

			try
			{
				newTimer = timerManager.Add(delay, timerGuid);
			}
			catch(ArgumentOutOfRangeException e)
			{
				log.Write(TraceLevel.Error, "Failed to add a new timer to timer manager:\n" + e.ToString());
			}

			if(newTimer != null)
			{
				TimerData tData = new TimerData(newTimer, delay, userData, timerPeriodPresent, 
					action as AsyncAction, fireAndReschedule, rescheduleDelay);
				timers.Add(timerGuid, tData);
				SendAddTimerComplete(action, timerGuid);
			}
			else
			{
				SendAddTimerFailed(action, timerGuid);
			}
		}

        /// <summary>
        /// Remove a timer callback.
        /// </summary>
        /// <param name="im">The message containing the remove command.</param>
        [Action(Package.Actions.RemoveTimer.FULLNAME, false, Package.Actions.RemoveTimer.DISPLAY, Package.Actions.RemoveTimer.DESCRIPTION, false)]
        [ActionParam(Package.Actions.RemoveTimer.Params.timerId.NAME, Package.Actions.RemoveTimer.Params.timerId.DISPLAY, typeof(string), useType.required, false, Package.Actions.RemoveTimer.Params.timerId.DESCRIPTION, Package.Actions.RemoveTimer.Params.timerId.DEFAULT)]
        private void OnRemoveTimer(ActionBase actionBase)
        {
            SyncAction action = null;
            if(actionBase is SyncAction)
            {
                action = actionBase as SyncAction;
            }
            else
            {
                action.SendResponse(false);
                return;
            }

            string timerId;
            
            try
            {
                action.InnerMessage.GetString(Package.Actions.RemoveTimer.Params.timerId.NAME, true, out timerId);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                action.SendResponse(false);
                return;
            }


            log.Write(TraceLevel.Verbose, "Removing timer: " + timerId);

            if(!RemoveTimer(timerId))
            {
                log.Write(TraceLevel.Warning, "No timer with id " + timerId + " found in timers table. No timer has been removed");
            }

            action.SendResponse(true);
        }

        [Event(Package.Events.TimerFire.FULLNAME, eventTypeType.hybrid, null, Package.Events.TimerFire.DISPLAY, Package.Events.TimerFire.DESCRIPTION)]
        [EventParam(Package.Events.TimerFire.Params.timerId.NAME, Package.Events.TimerFire.Params.timerId.DISPLAY, typeof(string), true, Package.Events.TimerFire.Params.timerId.DESCRIPTION)]
        [EventParam(Package.Events.TimerFire.Params.timerUserData.NAME, Package.Events.TimerFire.Params.timerUserData.DISPLAY, typeof(string), false, Package.Events.TimerFire.Params.timerUserData.DESCRIPTION)]
        private void TimerFiredEvent() {}

        /// <summary>
        /// The timer fired callback. Whenever a timer fires from the table, this 
        /// method will be called.
        /// </summary>
        /// <param name="timerGuid">The guid, as set by the OnAddTimer() method.</param>
        private long TimerFiredCallback(TimerHandle timerHandle, object timerGuid)
        {
            long nextDelay = 0;

            TimerData timerData = timers[timerGuid as string] as TimerData;

            if(timerData != null)
            {
                EventMessage im;
                if(timerData.action != null)
                {
                    im = this.CreateEventMessage(
                        Package.Events.TimerFire.FULLNAME, 
                        EventMessage.EventType.NonTriggering, 
                        timerData.action.RoutingGuid);

                    im.UserData = timerData.action.InnerMessage.UserData as string;

                    log.Write(TraceLevel.Verbose, "A non-triggering timer with id: " + timerGuid + " has fired.");
                }
                else
                {
                    im = this.CreateEventMessage(
                        Package.Events.TimerFire.FULLNAME,
                        EventMessage.EventType.Triggering,
                        System.Guid.NewGuid().ToString());

                    log.Write(TraceLevel.Verbose, "A triggering timer with id: " + timerGuid + " has fired.");
                }

                im.AddField(Package.Events.TimerFire.Params.timerId.NAME, timerGuid as string);
                im.AddField(Package.Events.TimerFire.Params.timerUserData.NAME, timerData.userData);

                palWriter.PostMessage(im);

                if(timerData.recurrence == false)
                {
					timerData.timer.Cancel();
                    timers.Remove(timerGuid as string);   
                }
                else
                {
                    // Do we need to reschedule the timer?
                    if (timerData.fireAndReschedule == true)
                    {
                        timerData.timer.Reschedule(timerData.rescheduleDelay);
                        timerData.fireAndReschedule = false;
                        timerData.delay = timerData.rescheduleDelay;
                    }
                    nextDelay = timerData.delay;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, "A timer with id: " + timerGuid as string + " is not in the list of currently running timers. " + 
                    "Timer callback is ignored.");
            }
 
            return nextDelay;
        }

        #endregion 

        #region Callbacks
        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            string timerRemoveFailureReason;
            string timerId = originalEvent[Package.Events.TimerFire.Params.timerId.NAME] as string;

            if(timerId != null)
            {
                // We want to ignore these global timers.
                if( timerId != Consts.ReservedIds.MinuteTimer && 
					timerId != Consts.ReservedIds.HourlyTimer && 
					timerId != Consts.ReservedIds.DailyTimer)
                {
                    TimerData info = timers[timerId] as TimerData;

                    if(info != null)
                    {
                        if(info.timer != null)
                        {
                            // dispose the timer
                            info.timer.Cancel();
                            info.timer = null;
                            return;
                        }
                        else
                        {
                            // timer already removed, if this is non-recurrence then ignore.
                            if (info.recurrence)
                                timerRemoveFailureReason = "unable to dispose of a null timer";
                            else
                                return;
                        }
                    }
                    else
                    {
                        timerRemoveFailureReason = "unable to find the timer in the timer list";
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                timerRemoveFailureReason = "unable to extract the timerId from the no-handler message.";
            }

            log.Write(TraceLevel.Warning, "Received a no handler, but could not remove the timer,"
                + " because the Timer Provider was {0}", timerRemoveFailureReason); 
        }

        #endregion 

        #region Utilities

		/// <summary>Removes a timer from the table</summary>
		/// <param name="timerId">Timer ID</param>
		/// <returns>True if the timer was in the table</returns>
		private bool RemoveTimer(string timerId)
		{
			TimerData info = timers[timerId] as TimerData;

			if(info == null)
				return false;

			if(info.timer != null)
				info.timer.Cancel();                        

			timers.Remove(timerId);
			return true;
		}

        /// <summary>
        /// Reset the timer table. This will dispose of all the timers currently in the
        /// timer table hash.
        /// </summary>
		public void ResetTimerTable()
		{
			lock(timers.SyncRoot)
			{
				foreach(TimerData timerData in timers.Values)
				{
					timerData.timer.Cancel();
				}

				timers.Clear();
			}
		}

        private void SendAddTimerComplete(ActionBase action, string timerGuid)
        {
            log.Write(TraceLevel.Verbose, "Sending add timer complete for actionGuid: " + action.Guid);

            action.SendResponse(true, new Field(Package.Events.TimerFire.Params.timerId.NAME, timerGuid));
        }

        private void SendAddTimerFailed(ActionBase action, string timerGuid)
        {
            log.Write(TraceLevel.Verbose, "Sending add timer failed for actionGuid: " + action.Guid);

            action.SendResponse(false);
        }

        #endregion 
    }
}

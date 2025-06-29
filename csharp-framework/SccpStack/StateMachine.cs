using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an abstract state machine.
	/// </summary>
	/// <remarks>
	/// A StateMachine contains a collection of States, a State contains a
	/// collection of (possible) Transitions, and an Event triggers a
	/// Transition and optionally causes the StateMachine to perform an action
	/// associated with the Transition.
	/// </remarks>
	public abstract class StateMachine
	{
		/// <summary>
		/// Constructs a StateMachine by having the subclass define the
		/// collection of States.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="control">Object through which StateMachine assures
		/// that static state machine is constructed only once.</param>
		public StateMachine(LogWriter log, ref StateMachineStaticControl control)
		{
			this.log = log;

			// Build actual, underlying static state machine if not already
			// built for this (sub)class.
			AssureBuilt(ref control);
		}

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		protected readonly LogWriter log;

		/// <summary>
		/// Current State.
		/// </summary>
		private volatile State currentState;

		/// <summary>
		/// Property whose value is the current State.
		/// </summary>
		public State CurrentState { get { return currentState; } }

		/// <summary>
		/// Returns whether this StateMachine is currently in the specified
		/// State.
		/// </summary>
		/// <param name="state">State on which to check.</param>
		/// <returns>Whether this StateMachine is currently in the specified
		/// State.</returns>
		public bool IsState(State state)
		{
			return currentState == state;
		}

		/// <summary>
		/// Action previously processed by Trigger().
		/// </summary>
		private ActionDelegate previousAction;

		/// <summary>
		/// Event previously processed by Trigger().
		/// </summary>
		private Event previousEvent;

		/// <summary>
		/// Next State previously processed by Trigger().
		/// </summary>
		private State previousNextState;

		/// <summary>
		/// Previous State previously processed by Trigger().
		/// </summary>
		private State previousCurrentState;

		/// <summary>
		/// Translates a timer expiry to a specific event based on the timeout
		/// type (typically).
		/// </summary>
		/// <remarks>Unless overriden, a state machine does not use
		/// timers.</remarks>
		/// <param name="timer">Timer with associated data hanging off of
		/// it.</param>
		public virtual void TimerExpiry(EventTimer timer)
		{
			Debug.Fail("SccpStack: this state machine does not use timers; " +
				"can't do anything for timer expiry");
		}

		/// <summary>
		/// Translates an int to a string that represents one of this class'
		/// event types.
		/// </summary>
		/// <param name="enumValue">Int value to translate.</param>
		/// <returns>String that represents the corresponding event-type
		/// enumeration.</returns>
		public abstract string IntToEventEnumString(int enumValue);

		/// <summary>
		/// Translates an int to a string that represents one of this class'
		/// timer types.
		/// </summary>
		/// <param name="enumValue">Int value to translate.</param>
		/// <returns>String that represents the corresponding timer-type
		/// enumeration.</returns>
		public abstract string IntToTimerEnumString(int enumValue);

		#region Pre-building StateMachine

		/// <summary>
		/// Constants referenced within this class.
		/// </summary>
		private abstract class Const
		{
			/// <summary>
			/// Reflection bind flags of the expected StateMachine's
			/// action-handling and state-definition methods as well as action
			/// delegates and state variables.
			/// </summary>
			/// <remarks>
			/// Essentially, they all have to be declared static and are
			/// typically declared private but could be public I suppose.
			/// </remarks>
			public const BindingFlags Bindings =
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
		}

		/// <summary>
		/// Builds actual, underlying static state machine if not already built
		/// for this (sub)class.
		/// </summary>
		/// <param name="control">Object through which StateMachine assures
		/// that static state machine is constructed only once.</param>
		private void AssureBuilt(ref StateMachineStaticControl control)
		{
			try
			{
				// Lock on the control reference so that static state machine
				// only gets constructed once.
				// (This is just a lock that complains if it takes too long.)
				for (int i = 1; !Monitor.TryEnter(control, SccpStack.LockPollMs); ++i)
				{
					log.Write(TraceLevel.Warning,
						"StM: {0}: waited {1} times for this lock within AssureBuilt()",
						this, i);
				}
				long lockAcquiredNs = HPTimer.Now();
				try
				{
					// If an initial state has not yet been assigned for this
					// state-machine class (not instance), the static state
					// machine has not been built, either, so build it now.
					if (control.InitialState == null)
					{
						// Find the decorated StateMachine in this class'
						// inheritance tree (we may be constructing a child of
						// the decorated StateMachine).
						Type type;
						StateMachineAttribute attribute;
						if (!FindStateMachine(out type, out attribute))
						{
							// [StateMachine()] attribute missing from this (or
							// parent's) class definition.
							throw new ArgumentException("missing decorated StateMachine");
						}

						// Build underlying static state machine and assign
						// initial state used by all instances of this class.
						control.InitialState = Build(type, attribute);
					}

					if (currentState != null)
					{
						throw new ArgumentException("current State already initialized");
					}

					// Initialize current state of this state-machine instance
					// to the initial state determined when the static state
					// machine was built.
					currentState = control.InitialState;
				}
				finally
				{
					Monitor.Exit(control);
				}
				long lockHeldMs = (HPTimer.Now() - lockAcquiredNs) / 1000 / 1000;
				if (lockHeldMs > SccpStack.LockPollMs)
				{
					log.Write(TraceLevel.Warning,
						"StM: {0}: lock held long time ({1}ms) within AssureBuilt()",
						this, lockHeldMs);
				}
			}
			// (In case thread gets nuked while waiting on lock.)
			catch (ThreadInterruptedException e)
			{
				log.Write(TraceLevel.Warning, "StM: {0}: {1}",
					this, e);
			}
		}

		/// <summary>
		/// Finds the decorated StateMachine in this class' inheritance tree.
		/// </summary>
		/// <param name="type">Type of StateMachine that is decorated with the
		/// StateMachine attribute or the value is undefined if none of the
		/// parent classes in the inheritance tree are so decorated.</param>
		/// <param name="attribute">StateMachineAttribute of the found type or
		/// undefined if not found.</param>
		/// <returns>Whether found a StateMachine that has the StateMachine
		/// attribute.</returns>
		private bool FindStateMachine(out Type type, out StateMachineAttribute attribute)
		{
			type = null;
			attribute = null;

			// Search from this class up to but not including the base
			// StateMachine class.
			for (Type nextType = this.GetType();
				nextType != typeof(StateMachine); nextType = nextType.BaseType)
			{
				if (HasStateMachineAttribute(nextType, out attribute))
				{
					type = nextType;
					break;
				}
			}

			return type != null;
		}

		/// <summary>
		/// Determines whether this member is decorated with the StateMachine
		/// attribute.
		/// </summary>
		/// <param name="memberInfo">Member to check whether
		/// StateMachine-decorated.</param>
		/// <param name="attribute">StateMachineAttribute of the found type or
		/// undefined if not found.</param>
		/// <returns>Whether this member is decorated with the StateMachine
		/// attribute.</returns>
		private bool HasStateMachineAttribute(MemberInfo memberInfo,
			out StateMachineAttribute attribute)
		{
			attribute = null;

			object[] attributes = memberInfo.GetCustomAttributes(false);
			foreach (object nextAttribute in attributes)
			{
				if (nextAttribute is StateMachineAttribute)
				{
					attribute = (StateMachineAttribute)nextAttribute;
					break;
				}
			}

			return attribute != null;
		}

		/// <summary>
		/// Builds underlying static state machine and assigns initial
		/// state used by all class instances.
		/// </summary>
		/// <param name="type">StateMachine Type for which to build static
		/// state machine.</param>
		/// <param name="attribute">StateMachineAttribute of the StateMachine.</param>
		/// <returns>Initial state used by all class instances.</returns>
		private State Build(Type type, StateMachineAttribute attribute)
		{
			string actionPrefix = attribute.ActionPrefix;
			string stateDefinitionPrefix = attribute.StateDefinitionPrefix;

			// Find all members of this StateMachine which are likely to be
			// action-handling and state-definition methods.
			MemberInfo[] memberInfos = type.FindMembers(
				MemberTypes.Method, Const.Bindings,
				new MemberFilter(DelegateToSearchCriteria),
				new SearchParameters(actionPrefix, stateDefinitionPrefix));

			// Assign all callback methods for this StateMachine to
			// corresponding delegate fields. Those delegate fields are used in
			// state-machine tables to indicate what actions to take when an
			// event is triggered.
			AssignActionDelegates(memberInfos, type, actionPrefix);

			// Construct all State fields, call all state-definition methods,
			// and return initial state.
			return DefineStates(memberInfos, type, stateDefinitionPrefix);
		}

		/// <summary>
		/// Determines whether the member meets the action-handling- or
		/// state-definition-method search criteria.
		/// </summary>
		/// <param name="objMemberInfo">Member to check.</param>
		/// <param name="objSearch">Parameters used to check, i.e.,
		/// action-handling- and state-definition-method prefixes.</param>
		/// <returns>Whether member name starts with either prefix or whether
		/// either prefix is missing.</returns>
		public static bool DelegateToSearchCriteria(MemberInfo objMemberInfo,
			Object objSearch)
		{
			bool matches;

			SearchParameters searchParameters = objSearch as SearchParameters;
			Debug.Assert(searchParameters != null, "StateMachine: SearchParameters missing");

			if (searchParameters == null)
			{
				// Internal error--searchParameters should always by
				// present--so treat as if never matches.
				matches = false;
			}
			else
			{
				// If either prefix is missing, this method matches the search
				// criteria. The caller will subsequently inspect the method
				// closer to determine whether it is indeed an action or state
				// definition. We are just trying to weed out any methods that
				// we can say for sure are not action-handling or
				// state-definition methods.
				if (searchParameters.actionPrefix == null ||
					searchParameters.actionPrefix.Length == 0 ||
					searchParameters.stateDefinitionPrefix == null ||
					searchParameters.stateDefinitionPrefix.Length == 0)
				{
					matches = true;
				}
				else
				{
					// See if method name starts with either prefix.
					matches = objMemberInfo.Name.StartsWith(searchParameters.actionPrefix)
						|| objMemberInfo.Name.StartsWith(searchParameters.stateDefinitionPrefix);
				}
			}

			return matches;
		}

		/// <summary>
		/// Contains the search parameters for Type.FindMembers().
		/// </summary>
		/// <remarks>Trivial container class.</remarks>
		private class SearchParameters
		{
			public SearchParameters(string actionPrefix, string stateDefinitionPrefix)
			{
				this.actionPrefix = actionPrefix;
				this.stateDefinitionPrefix = stateDefinitionPrefix;
			}

			public string actionPrefix;
			public string stateDefinitionPrefix;
		}

		/// <summary>
		/// Assigns all callback methods for this StateMachine to corresponding
		/// delegate fields.
		/// </summary>
		/// <remarks>
		/// The delegate fields are used in state-machine tables to indicate
		/// what actions to take when an event is triggered.
		/// </remarks>
		/// <param name="memberInfos">Members that are action-method
		/// candidates (we'll weed out any that aren't).</param>
		/// <param name="type">Type of StateMachine containing action methods
		/// and their corresponding delegate fields.</param>
		/// <param name="actionPrefix">Prefix to remove from method name in
		/// order to determine name of delegate field if explicit delegate
		/// field name is not specified.</param>
		private void AssignActionDelegates(MemberInfo[] memberInfos, Type type,
			string actionPrefix)
		{
			// For each method that is decorated with the ActionAttribute,
			// determine the name of the corresponding field that will hold
			// a reference to it in the state machine.
			foreach (MemberInfo memberInfo in memberInfos)
			{
				MethodInfo methodInfo = (MethodInfo)memberInfo;
				ActionAttribute attribute =
					(ActionAttribute)GetMethodAttribute(methodInfo, typeof(ActionAttribute));
				if (attribute != null)
				{
					// If a Delegate field name is specified, just use it;
					// otherwise, use the method name without the prefix.
					string fieldName = attribute.Delegate;
					if (fieldName == null)
					{
						fieldName = FieldName(methodInfo.Name, actionPrefix, false);
					}

					// Get delegate field for this type according to name and bindings.
					FieldInfo fieldInfo = type.GetField(fieldName, Const.Bindings);
					if (fieldInfo == null)
					{
						// The class must already contain the indicated delegate field.
						throw new ArgumentException("no static delegate");
					}

					// Assign a reference to the action-handling method to the delegate field.
					fieldInfo.SetValue(this,
						Delegate.CreateDelegate(typeof(ActionDelegate), methodInfo));
				}
			}
		}

		/// <summary>
		/// Constructs all State fields, calls all state-definition methods,
		/// and returns initial state.
		/// </summary>
		/// <remarks>
		/// States have to be constructed (initialized) before being defined
		/// because they may be self-referential.
		/// </remarks>
		/// <param name="memberInfos">Members that are state-definition-method
		/// candidates (we'll weed out any that aren't).</param>
		/// <param name="type">Type of StateMachine containing state-definition
		/// methods and their corresponding state fields.</param>
		/// <param name="stateDefinitionPrefix">Prefix to remove from method
		/// name in order to determine name of state field if explicit state
		/// field name is not specified.</param>
		/// <returns>Initial state used by all class instances.</returns>
		private State DefineStates(MemberInfo[] memberInfos, Type type,
			string stateDefinitionPrefix)
		{
			State initialState =
				InitializeStates(memberInfos, type, stateDefinitionPrefix);
			DefineInitializedStates(memberInfos, type, stateDefinitionPrefix);

			return initialState;
		}

		/// <summary>
		/// Constructs all State fields and returns initial State.
		/// </summary>
		/// <param name="memberInfos">Members that are state-definition-method
		/// candidates (we'll weed out any that aren't).</param>
		/// <param name="type">Type of StateMachine containing state-definition
		/// methods and their corresponding state fields.</param>
		/// <param name="stateDefinitionPrefix">Prefix to remove from method
		/// name in order to determine name of state field if explicit state
		/// field name is not specified.</param>
		/// <returns>Initial state used by all class instances.</returns>
		private State InitializeStates(MemberInfo[] memberInfos, Type type,
			string stateDefinitionPrefix)
		{
			State initialState = null;

			// For each method that is decorated with the StateAttribute,
			// determine the name of the corresponding field that will hold
			// the new State that the method defines.
			foreach (MemberInfo memberInfo in memberInfos)
			{
				MethodInfo methodInfo = (MethodInfo)memberInfo;
				StateAttribute attribute =
					(StateAttribute)GetMethodAttribute(methodInfo, typeof(StateAttribute));
				if (attribute != null)
				{
					// If a Variable field name is specified, just use it;
					// otherwise, use the method name without the prefix and
					// with first character lower case.
					string fieldName = attribute.Variable;
					if (fieldName == null)
					{
						fieldName = FieldName(methodInfo.Name, stateDefinitionPrefix, true);
					}

					// Get state field for this type according to name and bindings.
					FieldInfo fieldInfo = type.GetField(fieldName, Const.Bindings);
					if (fieldInfo == null)
					{
						// The class must already contain the indicated variable field.
						throw new ArgumentException("no static variable");
					}

					// Assign a new State to the variable field.
					State state = new State(fieldName);
					fieldInfo.SetValue(this, state);

					// If this attribute has its Initial property set to true,
					// eventually return the state defined by this method as
					// the initial state for all instances of the state machine.
					if (attribute.Initial)
					{
						if (initialState != null)
						{
							// A StateMachine can only have a single initial State.
							throw new ArgumentException("initial state already defined");
						}
						initialState = state;
					}
				}
			}

			if (initialState == null)
			{
				// A StateMachine must have an initial State.
				throw new ArgumentException("initial state not defined");
			}

			return initialState;
		}

		/// <summary>
		/// Calls all state-definition methods for this StateMachine.
		/// </summary>
		/// <param name="memberInfos">Members that are state-definition-method
		/// candidates (we'll weed out any that aren't).</param>
		/// <param name="type">Type of StateMachine containing state-definition
		/// methods and their corresponding state fields.</param>
		/// <param name="stateDefinitionPrefix">Prefix to remove from method
		/// name in order to determine name of state field if explicit state
		/// field name is not specified.</param>
		private void DefineInitializedStates(MemberInfo[] memberInfos, Type type,
			string stateDefinitionPrefix)
		{
			// For each method that is decorated with the StateAttribute,
			// determine the name of the corresponding field that holds
			// the new State that the method defined.
			foreach (MemberInfo memberInfo in memberInfos)
			{
				MethodInfo methodInfo = (MethodInfo)memberInfo;
				StateAttribute attribute =
					(StateAttribute)GetMethodAttribute(methodInfo, typeof(StateAttribute));
				if (attribute != null)
				{
					// If a Variable field name is specified, just use it;
					// otherwise, use the method name without the prefix and
					// with first character lower case.
					string fieldName = attribute.Variable;
					if (fieldName == null)
					{
						fieldName = FieldName(methodInfo.Name, stateDefinitionPrefix, true);
					}

					// Get state field for this type according to name and bindings.
					FieldInfo fieldInfo = type.GetField(fieldName, Const.Bindings);
					if (fieldInfo == null)
					{
						throw new ArgumentException("no static variable");
					}

					// Call this state-definition method, passing the
					// corresponding state field as a parameter.
					object[] parameters = new object[1];
					parameters[0] = fieldInfo.GetValue(this);
					methodInfo.Invoke(this, parameters);
				}
			}
		}

		/// <summary>
		/// Returns the Attribute for the method of the specified Attribute type.
		/// </summary>
		/// <param name="info">Method in which to search for Attribute.</param>
		/// <param name="attributeType">Type of Attribute for which to search.</param>
		/// <returns>Attribute of the specified type for this method.</returns>
		private Attribute GetMethodAttribute(MethodInfo info, Type attributeType)
		{
			Attribute attribute = null;

			object[] methodAttributes = info.GetCustomAttributes(false);
			foreach (object methodAttribute in methodAttributes)
			{
				if (methodAttribute.GetType() == attributeType)
				{
					attribute = (Attribute)methodAttribute;
					break;
				}
			}

			return attribute;
		}

		/// <summary>
		/// Determine name of field that corresponds to either an
		/// action-handling- or state-definition-method, as specified by prefix
		/// and case of first character.
		/// </summary>
		/// <remarks>
		/// Caller must have already determined that the method name starts
		/// with the specified prefix and also that the method name is longer
		/// than the prefix.
		/// </remarks>
		/// <param name="methodName">Name of method on which field name is
		/// based.</param>
		/// <param name="prefix">Prefix to remove from method name to arrive at
		/// corresponding field name.</param>
		/// <param name="lowerCaseFirstCharacter">Whether first character after
		/// removing the prefix is to be set to lower case; otherwise, don't
		/// change.</param>
		/// <returns>Field name that corresopnds to method name.</returns>
		private static string FieldName(string methodName, string prefix,
			bool lowerCaseFirstCharacter)
		{
			string instanceName;

			if (!methodName.StartsWith(prefix))
			{
				throw new ArgumentException("method name does not start with " + prefix);
			}

			instanceName = methodName.Remove(0, prefix.Length);
			if (instanceName.Length == 0)
			{
				throw new ArgumentException("method name just a prefix");
			}

			if (lowerCaseFirstCharacter)
			{
				instanceName = instanceName.Substring(0, 1).ToLower() + instanceName.Substring(1);
			}

			return instanceName;
		}
		#endregion

		/// <summary>
		/// Processes an external event and all followup, internal events
		/// generated as a result.
		/// </summary>
		/// <remarks>
		/// External events include timer expiry, message received on the
		/// wire, and "message" received from a higher, application layer.
		/// Conversely, internal events are basically events returned from
		/// actions, directly or indirectly as a result of an external event.
		/// </remarks>
		/// <param name="event_">Event to process, along with any returned
		/// followup Events.</param>
		public static void ProcessEvent(Event event_)
		{
			Queue eventQueue = new Queue();

			// Process Events in the order that they were Enqueued. This means
			// that a new Event is processed after all already-Enqueued Events
			// have been processed. This is as opposed to processing an Event
			// and all Events it generates before the next Event in the Queue.
			while (true)
			{
				ProcessSingleInternalEvent(event_, ref eventQueue);

				// If nothing else to do--no internal Events remaining--leave
				// loop and return. Otherwise, process the next Event on the
				// Queue.
				if (eventQueue.Count == 0)
				{
					break;
				}

				event_ = (Event)eventQueue.Dequeue();
			}
		}

		/// <summary>
		/// Processes an Event by invoking the Trigger method on the specified
		/// StateMachine.
		/// </summary>
		/// <param name="event_">Event to process, along with any returned
		/// followup events.</param>
		/// <param name="followupEvents">List of events to process subsequently.</param>
		private static void ProcessSingleInternalEvent(Event event_, ref Queue followupEvents)
		{
			if (event_ == null)
			{
				throw new ApplicationException("no event_");
			}
			else
			{
				if (event_.EventStateMachine == null)
				{
					throw new ApplicationException("no event_.EventStateMachine " + event_.ToString());
				}
				else
				{
					event_.EventStateMachine.Trigger(event_, ref followupEvents);
				}
			}
		}

		/// <summary>
		/// Triggers a transition based on a full Event object and returns any
		/// followup events.
		/// </summary>
		/// <param name="event_">Event to process.</param>
		/// <param name="followupEvents">List of events to process after this
		/// method returns.</param>
		/// <returns>Whether a transition is defined for this Event.</returns>
		private bool Trigger(Event event_, ref Queue followupEvents)
		{
			Transition transition = null;

			if (event_ == null)
			{
				throw new ApplicationException("no event_");
			}
			else
			{
				if (currentState == null)
				{
					throw new ApplicationException("no currentState");
				}
				else
				{
					transition = UpdateState(event_);

					// Perform the action outside the lock to avoid deadlocks
					// between state machines.
					if (transition != null)
					{
						// null Action means do nothing.
						if (transition.Action != null)
						{
							transition.Action(this, event_, ref followupEvents);
						}
					}
					else
					{
						log.Write(TraceLevel.Error,
							"StM: {0}: no transition defined for {1} state",
							event_, CurrentState);

						HandleUnexpectedEvent(event_);
					}
				}
			}

			return transition != null;
		}

		/// <summary>
		/// Processes the event by updating the state variable and returning
		/// the corresponding transition.
		/// </summary>
		/// <param name="event_">Event to process.</param>
		/// <returns>Resulting Transition for this Event.</returns>
		private Transition UpdateState(Event event_)
		{
			Transition transition = null;

			try
			{
				// State transitions must be serialized.
				// (This is just a lock that complains if it takes too long.)
				for (int i = 1; !Monitor.TryEnter(this, SccpStack.LockPollMs); ++i)
				{
					log.Write(TraceLevel.Warning,
						"StM: {0}({1})/({2}): waited {3} times for this lock within Trigger(); {4} {5}->{6} {7}",
						this, this.GetHashCode(),
						Thread.CurrentThread.GetHashCode(), i,
						previousEvent, previousCurrentState, previousNextState,
						previousAction == null ? "?" : previousAction.Method.Name);
				}
				long lockAcquiredNs = HPTimer.Now();
				try
				{
					transition = currentState[event_.Id];
					if (transition != null)
					{
						LogTransition(event_, transition.NextState);

						// Record info about processing this event so it's
						// available to the diagnostic if blocked by the lock.
						previousAction = transition.Action;
						previousEvent = event_;
						previousCurrentState = currentState;
						previousNextState = transition.NextState;

						// null NextState means stay in the current state.
						if (transition.NextState != null)
						{
							currentState = transition.NextState;
						}
					}
				}
				finally
				{
					Monitor.Exit(this);
				}
				long lockHeldMs = (HPTimer.Now() - lockAcquiredNs) / 1000 / 1000;
				if (lockHeldMs > SccpStack.LockPollMs)
				{
					log.Write(TraceLevel.Warning,
						"StM: {0}({1})/({2}): lock held long time ({3}ms) by {4} processing {5} within Trigger()",
						this, this.GetHashCode(),
						Thread.CurrentThread.GetHashCode(), lockHeldMs,
						(transition == null || transition.Action == null) ? "?" : transition.Action.Method.Name,
						event_);
				}
			}
			// (In case thread gets nuked while waiting on lock.)
			catch (ThreadInterruptedException e)
			{
				log.Write(TraceLevel.Error, "StM: {0}: {1}", this, e);
			}
			catch (Exception e)
			{
				log.Write(TraceLevel.Error, "StM: {0}: {1}", this, e);
			}

			return transition;
		}

		/// <summary>
		/// Gets the Transition at the specified index in the current State's
		/// collection of Transitions.
		/// </summary>
		public Transition this [int id] { get { return currentState[id]; } }

		/// <summary>
		/// Whether to log the transitions for this state machine.
		/// </summary>
		/// <remarks>
		/// If such a thing were possible, this would be an abstract static
		/// property, but parents cannot access their children's static
		/// members.
		/// </remarks>
		protected virtual bool LogTransitions { get { return true; } }

		/// <summary>
		/// Logs a State Transition.
		/// </summary>
		/// <param name="event_">Event triggering the transition.</param>
		/// <param name="nextState">The State to which we are
		/// transitioning.</param>
		protected virtual void LogTransition(Event event_, State nextState) { }

		/// <summary>
		/// Log starting an EventTimer.
		/// </summary>
		/// <param name="isSet">Whether we are setting as opposed to resetting a timer.</param>
		/// <param name="interval">Milliseconds to delay before invoking the
		/// StateMachine TimerExpiry method.</param>
		/// <param name="type">Type of timer; typically an enum cast to an
		/// int.</param>
		internal virtual void LogEventTimerStart(bool isSet, int interval, int type) { }

		/// <summary>
		/// Performs a default action when the state machine attempts to
		/// trigger on an event that cannot be found in the list of expected
		/// events for the current state.
		/// </summary>
		/// <param name="event_">Event that was unexpected.</param>
		protected virtual void HandleUnexpectedEvent(Event event_) { }
	}

	#region Pre-building StateMachine helper classes

	/// <summary>
	/// Class through which StateMachine assures that static state machine
	/// is constructed only once and distributes the initial State to all
	/// StateMachine instances.
	/// </summary>
	public class StateMachineStaticControl
	{
		private State initialState = null;

		/// <summary>
		/// Initial State that is determined when the static state machine is
		/// constructed.
		/// </summary>
		internal State InitialState { get { return initialState; } set { initialState = value; } }
	}

	/// <summary>
	/// Inidicates whether a StateMachine subclass defines the states and
	/// actions.
	/// </summary>
	/// <remarks>
	/// There can be intermediate subclasses between the actual StateMachine
	/// class and the subclass that defines the states and actions. That
	/// subclass can be further subclassed.
	/// </remarks>
	/// <example>
	/// [StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	/// internal class Call : StateMachine
	/// {
	///   public Call(SccpProvider provider, Device device,
	///     LogWriter log, long callId, string to, string from,
	///     string originalTo, string displayName) : base(log, ref control)
	///   {
	///     ...
	///   }
	///   public Call(LogWriter log, ref StateMachineStaticControl control) :
	///     base(log, ref control) { }
	///   private static StateMachineStaticControl control = new StateMachineStaticControl();
	/// }
	/// </example>
	public class StateMachineAttribute : Attribute
	{
		private string actionPrefix = null;

		/// <summary>
		/// Specifies the prefix that StateMachine reflection uses to identify
		/// an action-handling method and for implicitly identifying the
		/// corresponding delegate field name.
		/// </summary>
		/// <remarks>
		/// This is optional--the [Action()] attribute of the action-handling
		/// method is ultimately what determines whether a method is an action.
		/// However, if not present, one must use the Delegate property of the
		/// ActionAttribute to explicitly identify the corresponding delegate
		/// field name.
		/// </remarks>
		public string ActionPrefix
		{ get { return actionPrefix; } set { actionPrefix = value; } }

		private string stateDefinitionPrefix = null;

		/// <summary>
		/// Specifies the prefix that StateMachine reflection uses to identify
		/// a state-definition method and for implicitly identifying the
		/// corresponding state variable name.
		/// </summary>
		/// <remarks>
		/// This is optional--the [State()] attribute of the state method is
		/// ultimately what determines whether a method is a state-definition
		/// method. However, if not present, one must use the Variable property
		/// of the StateAttribute to explicitly identify the corresponding state
		/// variable name.
		/// </remarks>
		public string StateDefinitionPrefix
		{ get { return stateDefinitionPrefix; } set { stateDefinitionPrefix = value; } }
	}

	/// <summary>
	/// Identifies a method as a state-definition method, which adds
	/// Transitions to the passed in State object.
	/// </summary>
	/// <example>
	///	[State(Initial=true)]
	///	private static void DefineIdle(State state)
	/// {
	///   //				Event				Action						Next State
	///   state.Add(EventType.Open,				Open);
	///   state.Add(EventType.PrimaryCheck,		PrimaryCheck,				primaryCheck);
	///   state.Add(EventType.Timeout,			PrimaryCheck,				primaryCheck);
	///   state.Add(EventType.DeviceNotify,		PrimaryCheck,				primaryCheck);
	///   state.Add(EventType.Reset,			Reset_);
	///   state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
	///   state.Add(EventType.Done,				Done);
	/// }
	/// private static State idle = null;
	/// private static State primaryCheck = null;
	/// private static State findPrimary = null;
	/// private static State secondaryCheck = null;
	/// private static State resetting = null;
	/// </example>
	public class StateAttribute : Attribute
	{
		private bool initial = false;

		/// <summary>
		/// Specifies whether this is the initial state of the StateMachine.
		/// </summary>
		public bool Initial { get { return initial; } set { initial = value; } }

		private string variable = null;

		/// <summary>
		/// Specifies the name of the corresponding state variable field where
		/// the State is stored.
		/// </summary>
		/// <remarks>
		/// If not present, StateMachine reflection assumes that the field name
		/// is based on the name of the state-definition method--without the
		/// prefix and with first character lower case.
		/// </remarks>
		public string Variable { get { return variable; } set { variable = value; } }
	}

	/// <summary>
	/// Identifies a method as an action-handling method, which handles the
	/// triggering of an event.
	/// </summary>
	/// <example>
	/// [Action()]
	/// private static Queue HandleResetRequest(StateMachine stateMachine, Event event_) 
	/// {
	///   Queue events = null;
	///   Discovery this_ = stateMachine as Discovery;
	///   CloseDeviceRequest message = event_.EventMessage as CloseDeviceRequest;
	///   this_.miscTimer.Stop();
	///   this_.clientResetCause = message.cause;
	///   switch (message.cause)
	///   {
	///     case Device.Cause.CallManagerRestart:
	///       this_.alarmCondition = Alarm.LastRestart;
	///       break;
	///     case Device.Cause.CallManagerReset:
	///       this_.alarmCondition = Alarm.LastReset;
	///       break;
	///     default:
	///       this_.alarmCondition = Alarm.LastKeypad;
	///       break;
	///   }
	///   if (this_.CloseCallManagers())
	///   {
	///     this_.miscTimer.Start(closeMs, this_, (int)TimerType.WaitingForClose);
	///   }
	///   else
	///   {
	///     (events = new Queue(1)).Enqueue((int)EventType.Timeout,
	///       new TimeoutEvent((int)TimerType.WaitingForClose));
	///   }
	///   return events;
	/// }
	/// private static ActionDelegate Open = null;
	/// private static ActionDelegate PrimaryCheck = null;
	/// private static ActionDelegate Reset_ = null;
	/// private static ActionDelegate ResetRequest = null;
	/// private static ActionDelegate Done = null;
	/// </example>
	public class ActionAttribute : Attribute
	{
		private string delegate_ = null;

		/// <summary>
		/// Specifies the name of the corresponding delegate field where a
		/// reference to the method is stored.
		/// </summary>
		/// <remarks>
		/// If not present, StateMachine reflection assumes that the field name
		/// is based on the name of the action-handling method--without the
		/// prefix.
		/// </remarks>
		public string Delegate { get { return delegate_; } set { delegate_ = value; } }
	}

	#endregion
}

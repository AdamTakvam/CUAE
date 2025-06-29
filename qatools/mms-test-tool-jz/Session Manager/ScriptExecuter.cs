using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;

using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Transactions;

namespace Metreos.MMSTestTool.Sessions
{
	
	/// <summary>
	/// Responsible for executing a specific instance of a script.
	/// </summary>
	public class ScriptExecuter
	{
		
		#region Variable declarations

		private const int CMDNAME_INDEX = 0;
		private const int CMDVALUE_INDEX = 1;

		//the table that holds all named commands so that their values can be looked up by other commands, should that be required
		private Hashtable variableTable;
		
		//holds the instance of the script to execute
		private Script script;

		//hashtable that holds all the outstanding transactions
		private Hashtable pendingCommands;
		
		//the currently executing command
		private Command currentCommand;
		
		//reference to the currently execution transaction object
		private MsTransactionInfo currentTransaction;

		//the variable that we check to see whether to continue execution
		private volatile bool run = false;

        /// <summary>
        /// An AutoResetEvent that controls when the next command gets sent out.
        /// </summary>
        private AutoResetEvent sendNextEvent = new AutoResetEvent(true);

        /// <summary>
        /// Keeps track of whether we're waiting for a final response from some command. 
        /// </summary>
        private volatile bool waitForFinal = false;

        //timers for the session and commands
		private Timer sessionTimeOutTimer;
		private Hashtable commandTimerTable = new Hashtable();
		private int commandTimeOutInterval;

		//object used for locking the receive command
		private object receiveLock;

        //object used for locking the current command
        private object currentCommandLock;

        //Send delegate for this ScriptExecuter, used for sending events to the Fixture.
        public event SessionManager.SendDelegate sendEvent;
		#endregion

        public ScriptExecuter(Script script)
		{
			this.script = script;
            commandTimeOutInterval = script.CommandTimeoutMsecs;
		            
			variableTable = new Hashtable();
			PopulateVariableTable();
			pendingCommands = new Hashtable();
            
            receiveLock = new object();
            currentCommandLock = new object();

		}

        #region Methods dealing with timers
		private void StartCommandTimer(object sendargs)
		{
			if (commandTimeOutInterval <= 0 || sendargs == null)
                return;
            try
            {
                SessionManager.SendEventArgs args = sendargs as SessionManager.SendEventArgs;
                MsTransactionInfo transaction = args.transaction as MsTransactionInfo;

                lock (commandTimerTable.SyncRoot)
                {
                    if (!commandTimerTable.ContainsKey(transaction.Id))
                    {   

                        commandTimerTable.Add(transaction.Id, new Timer(new TimerCallback(this.CommandTimedOut), args, commandTimeOutInterval,0));
                    }
                    else
                    {
                        ArrayList message = new ArrayList();
                        message.Add("WARNING: TRANSACTION BEING ADDED ALREADY IN transactionTimerTable!");
                        Output(message);
                    }
                }
            }
            catch
            {
                ArrayList error = new ArrayList();
                error.Add("ERROR: CANNOT ADD TRANSACTION TO transactionTimerTable!");
                Output(error);
            }
		}

        private void ResetCommandTimer(SessionManager.SendEventArgs args)
        {
            if (commandTimeOutInterval <= 0)
                return;

            try
            {
                lock (commandTimerTable.SyncRoot)
                {
                    Timer commandTimeOutTimer = (Timer)commandTimerTable[args.transaction.Id];
                    commandTimeOutTimer.Change(commandTimeOutInterval,0);
                }
            }
            catch
            {
                ArrayList error = new ArrayList();
                error.Add("WARNING: Problem Occured while attempting to reset a commandTimer.");
                Output(error);
            }
        }
        
        /// <summary>
        /// Disables the time-out timer for a command.
        /// </summary>
        /// <param name="transaction"></param>
        private void StopCommandTimer(SessionManager.SendEventArgs args)
        {
            if (commandTimeOutInterval <= 0)
                return;
            
            try
            {
                Timer commandTimeOutTimer = (Timer)commandTimerTable[args.transaction.Id];
                lock (commandTimerTable.SyncRoot)
                {
                    commandTimerTable.Remove(args.transaction.Id);
                    commandTimeOutTimer.Dispose();
                }

            }
            catch
            {
                ArrayList error = new ArrayList();
                error.Add("WARNING: ATTEMPTED TO REMOVE A NON-EXISTING ENTRY FROM transactionTimerTable!");
                Output(error);
            }
        }

		private void CommandTimedOut(Object sendargs)
		{
            try
            {
                SessionManager.SendEventArgs args = sendargs as SessionManager.SendEventArgs;
                Command command = PopPendingCommand(args.transaction.Id);
                args.transaction.status = MsTransactionInfo.TRANSACTION_RESULT.COMMAND_TIMEOUT;
                OutputTransactionResults(args);
                OutputCommand(command);
                StopExecution();
            }
            catch (System.Exception e)
            {
                ArrayList message = new ArrayList();
                message.Add("ERROR: EXCEPTION " + e.Message + " OCCURED IN CommandTimeOut().");
                message.Add("Printing stack trace:");
                message.Add(e.StackTrace);
                Output(message);
            }
		}
        #endregion

        #region Methods dealing with running and stopping the script
		private void RunScript()
		{
			//Implement some checking to see if session manager is connected			
			while (run)
			{
                //Console.WriteLine("The number of elements in pendingCommands is: " + pendingCommands.Count);
                //if we're supposed to send a command...
                sendNextEvent.WaitOne();

                lock (currentCommandLock)
                {
                    currentCommand = script.GetNextCommand();

                    if (currentCommand != null && !waitForFinal)
                    {
                        //send the command and start the command timeout timer
                        StartCommandTimer(SendCommand(currentCommand));
                    }
                    else if (currentCommand == null && !waitForFinal)
                        StopExecution();
                }
            }
        }

        public void StartExecution()
        {
            run = true;
            RunScript();
        }

        public void StopExecution()
        {
            if (run)
            {
                run = false;
                //throws an invalid cast sometimes for some reason?
                try
                {
                    lock (commandTimerTable.SyncRoot)
                    {
                        foreach (Timer t in commandTimerTable.Values)
                            t.Dispose();
                        SessionManager.ServerDisconnect();
                    }
                }
                catch (System.Exception e)
                {
                    ArrayList message = new ArrayList();
                    message.Add(e.Message);
                    Output(message);
                }
            }
        }
        #endregion

		#region Receiving and handling of incoming messages
        /// <summary>
        /// This method is called when the SessionManager receives a response to a command associated with this
        /// script instance. It decides if the response is to a sync or async command, and calls the proper handler.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="message"></param>
		public void ReceiveResponse(SessionManager.SendEventArgs args, ParameterContainer message)
		{
			lock (receiveLock)
			{
                if (run)
                {
                    // if the transaction we're dealing with is asynchronous...
                    if (args.transaction is MsAsyncTransactionInfo)
                        HandleAsyncResponse(args, message);
                    else
                    {
                        bool result = HandleSyncResponse(args,message);
                        waitForFinal = false; 
                        // we've received and handled an OK response, we check what the result of the asserts was,
                        // and then we either move to the next command or stop execution.
                        if (result)
                            sendNextEvent.Set();
                        else
                            StopExecution();
                    }
                }
			}
		}

		/// <summary>
		/// Handle responses to synchronous commands
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="mesage"></param>
		private bool HandleSyncResponse(SessionManager.SendEventArgs args, ParameterContainer message)
		{
			Command command = PopPendingCommand(args.transaction.Id);
			if (command == null)
			{
				args.transaction.status = MsTransactionInfo.TRANSACTION_RESULT.RECEIVED_COMMAND_NOT_FOUND;
                OutputTransactionResults(args as SessionManager.SendEventArgs);
                Output(message.Output());
                return false;
			}

            StopCommandTimer(args);

            string resultCode = GetMessageFieldByName(IMediaServer.FIELD_MS_RESULT_CODE, message);
			if (resultCode == null)
			{
				args.transaction.status = MsTransactionInfo.TRANSACTION_RESULT.RESULT_CODE_NOT_FOUND;
                OutputTransactionResults(args);
                Output(message.Output());
                return false;
			}

			// Write the returned values to the command
			WriteReturns(ref command.finalReturns, message);

			// if the transaction failed...
            if (!resultCode.Equals(IMediaServer.MS_RESULT_OK))
            {
                args.transaction.status = MsTransactionInfo.TRANSACTION_RESULT.FAILED;
                OutputTransactionResults(args);
                Output(message.Output());
                return false;
            }
            else
            {
                args.transaction.status = MsTransactionInfo.TRANSACTION_RESULT.SUCCEEDED;
                OutputTransactionResults(args);
                bool result = ProcessFinalAsserts(args.transaction, ref command);
                OutputAssertResults(args.transaction, command.finalReturns, command.finalAsserts);
                
                return result;
            }
		}

        
        /// <summary>
        /// This method handles responses to asynchronous commands. If the response is a provisional one, provisional asserts are
        /// performed. If it is the final response, it is handed over to HandleSyncResponse.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="message"></param>
        private void HandleAsyncResponse(SessionManager.SendEventArgs args, ParameterContainer message)
        {
            MsAsyncTransactionInfo transaction = args.transaction as MsAsyncTransactionInfo;

            if (!transaction.provisionalReceived)
                ResetCommandTimer(args);
            
            string resultCode = GetMessageFieldByName(IMediaServer.FIELD_MS_RESULT_CODE,message);
            if (resultCode == null)
            {
                transaction.status = MsTransactionInfo.TRANSACTION_RESULT.RESULT_CODE_NOT_FOUND;
                StopCommandTimer(args);
                OutputTransactionResults(args);
                Output(message.Output());
                return;
            }
            
            // send this over to handlesync for processing if the command completed
            if (resultCode == IMediaServer.MS_RESULT_OK)
            {
                HandleSyncResponse(args, message);
                return;
            }
            
            Command command = null;
            command = PopPendingCommand(transaction.Id);
    		
            if (command == null)
            {
                transaction.status = MsTransactionInfo.TRANSACTION_RESULT.RECEIVED_COMMAND_NOT_FOUND;
                StopCommandTimer(args);
                OutputTransactionResults(args);
                Output(message.Output());
                return;
            }

            if (resultCode == IMediaServer.MS_RESULT_TRANSACTION_EXECUTING)
            {
                // make sure this is not a duplicate provisional response
                if (transaction.provisionalReceived == false)
                {
                    // Reset the timeout timer
                    transaction.provisionalReceived = true;
                    transaction.status = MsTransactionInfo.TRANSACTION_RESULT.EXECUTING;
                    
                    // Output the result of this provisional transaction
                    OutputTransactionResults(args);
                    
                    // ParameterContainer provisionalReturns 
                    WriteReturns(ref command.provisionalReturns, message);

                    // read provisional response returns and perform asserts
                    bool result = ProcessProvisionalAsserts(transaction, ref command);

                    // Add the command back to the pending table to wait for final response
                    AddPendingCommand(transaction,command);

                    // Output the provisional asserts
                    OutputAssertResults(transaction, command.provisionalReturns, command.provisionalAsserts);
                    
                    // we've received and handled an OK response, we check what the result of the asserts was,
                    // and then we either move to the next command or stop execution.
                    if (result)
                        sendNextEvent.Set();
                    else
                        StopExecution();

                    return;
                }
                else
                {
                    transaction.status = MsTransactionInfo.TRANSACTION_RESULT.DUPLICATE_PROVISIONAL;
                    OutputTransactionResults(args);
                    Output(message.Output());
                    
                    // we've received and handled an OK response, so it's okay to move on to the next command
                    sendNextEvent.Set();

                    return;
                }
            }
            else
            {
                transaction.status = MsTransactionInfo.TRANSACTION_RESULT.FAILED;
                OutputTransactionResults(args);
                Output(message.Output());
                StopExecution();
                return;
            }
            
        }
        #endregion

		#region Deal with writing results back to the command and veryfing asserts
        /// <summary> Writes results return from the MMS to the appropriate command </summary>
        /// <param name="returns"></param>
        /// <param name="message"></param>
		private void WriteReturns(ref ParameterContainer returns, ParameterContainer message)
		{
			returns = new ParameterContainer();
			foreach (ParameterField field in message)
    			returns.Add(field.Name,field.Value);
		}

		/// <summary>
		/// Processes the final asserts of a command, by way of ProcessAsserts, and calls the output methods
		/// </summary>
		/// <param name="command"></param>
		private bool ProcessFinalAsserts(MsTransactionInfo transaction, ref Command command)
		{
			MsTransactionInfo.ASSERT_RESULT assertsResult = ProcessAsserts(command.finalReturns, command.finalAsserts);
			bool result = false;
			switch (assertsResult)
			{
				case MsTransactionInfo.ASSERT_RESULT.ASSERT_FAILED : assertsResult = MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_FAILED; break;
				case MsTransactionInfo.ASSERT_RESULT.ASSERT_NOT_MATCHED : assertsResult = MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_NOT_MATCHED; break;
				case MsTransactionInfo.ASSERT_RESULT.ASSERTS_OK : assertsResult = MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_OK; result = true; break;
			}
			
            transaction.finalAssertResult = assertsResult;
            return result;
		}

		/// <summary>
		/// Processes the provisional asserts of a command, by way of ProcessAsserts, and calls the output methods
		/// </summary>
		private bool ProcessProvisionalAsserts(MsAsyncTransactionInfo transaction, ref Command command)
		{
			MsTransactionInfo.ASSERT_RESULT assertsResult = ProcessAsserts(command.provisionalReturns, command.provisionalAsserts);
			bool result = false;
            switch (assertsResult)
			{
				case MsTransactionInfo.ASSERT_RESULT.ASSERT_FAILED : assertsResult = MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_FAILED; break;
				case MsTransactionInfo.ASSERT_RESULT.ASSERT_NOT_MATCHED : assertsResult = MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_NOT_MATCHED; break;
				case MsTransactionInfo.ASSERT_RESULT.ASSERTS_OK : assertsResult = MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_OK; result = true; break;
			}
			
            transaction.provisionalAssertResult = assertsResult;
            return result;

		}

		/// <summary>
		/// Goes through the specified lists of asserts, and compares them to the parameters
		/// returned from the MMS to insure that all are valid. If not, returns a condition
		/// describing the type of the failure.
		/// </summary>
		/// <param name="returns"></param>
		/// <param name="asserts"></param>
		private MsTransactionInfo.ASSERT_RESULT ProcessAsserts(ParameterContainer returns, AssertContainer asserts)
		{
			bool matched;
            MsTransactionInfo.ASSERT_RESULT result = MsTransactionInfo.ASSERT_RESULT.ASSERTS_OK;

            foreach (AssertField assertField in asserts)
            {
                matched = true;
                foreach (ParameterField field in returns)
                {
                    if (string.Compare(field.Name,assertField.Name, true) == 0)
                        matched = matched && ProcessAssert(field, assertField);
                    //If there was a mismatch, check to see if the var we're asserting on was returned from the mms
                    if (!matched)
                    {
                        if (returns.GetFieldByName(field.Name) == null)
                            result = MsTransactionInfo.ASSERT_RESULT.ASSERT_NOT_MATCHED;
                        else
                            result = MsTransactionInfo.ASSERT_RESULT.ASSERT_FAILED;
                    }
               }
				
			}

			return result;
		}

		/// <summary>
		/// checks a single received field against a single assert field
		/// </summary>
		/// <param name="receivedField"></param>
		/// <param name="fieldToCheckAgainst"></param>
		/// <returns></returns>
		private bool ProcessAssert(ParameterField receivedField, AssertField fieldToCheckAgainst)
		{

            switch (fieldToCheckAgainst.Operator)
			{
				case "==" : fieldToCheckAgainst.Success = (string.Compare(receivedField.Value,fieldToCheckAgainst.Value,true) == 0); break;
				case "!=" : fieldToCheckAgainst.Success = (string.Compare(receivedField.Value,fieldToCheckAgainst.Value,true) != 0); break;
				case "<"  : fieldToCheckAgainst.Success = (int.Parse(receivedField.Value) < int.Parse(fieldToCheckAgainst.Value)); break;
				case "<=" : fieldToCheckAgainst.Success = (int.Parse(receivedField.Value) <= int.Parse(fieldToCheckAgainst.Value)); break;
				case ">"  : fieldToCheckAgainst.Success = (int.Parse(receivedField.Value) > int.Parse(fieldToCheckAgainst.Value)); break;
				case ">=" : fieldToCheckAgainst.Success = (int.Parse(receivedField.Value) >= int.Parse(fieldToCheckAgainst.Value)); break; 
				default   : fieldToCheckAgainst.Success = false; break;
			}
            
            return fieldToCheckAgainst.Success;
		}
		#endregion

		#region Preparation and sending of outgoing messages
		/// <summary>
		/// Sends a command down the chain to the Session, adding a transactionId
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public SessionManager.SendEventArgs SendCommand(Command command)
		{
            //if the command specifies that we are to wait for final response before sending the next command...
            if (command.optionsTable.ContainsKey("waitForFinal"))
            {
                if ((string)command.optionsTable["waitForFinal"] == "true")
                    waitForFinal = true;
            }

            MsTransactionInfo transaction;
			
			if (command.isAsync)
				transaction = new MsAsyncTransactionInfo();
			else
				transaction = new MsTransactionInfo();


			currentTransaction = transaction;

			//add a transactionId for a command
			bool success = command.parameters.SetFieldValue(IMediaServer.FIELD_MS_TRANSACTION_ID,transaction.Id);
            			
			//If the command does not contain a transactionId parameter, we need to add it
			if (!success)
				command.parameters.Add(IMediaServer.FIELD_MS_TRANSACTION_ID, transaction.Id);

			//check the parameters of this command to see if any of them are variableTable references
			ArrayList varReferences = FindVariableReferences(command.parameters);
			
			//if there are such references, we must reference them into the proper named command's returns section
			//in the variableTable
			if (varReferences.Count > 0)
				ReferenceVariables(command.parameters, varReferences, ParameterBase.ParameterType.FINALRETURN);
			
			//Add command to the pending commands table
            AddPendingCommand(transaction, command);

            SessionManager.SendEventArgs args = new SessionManager.SendEventArgs();

            // trigger the sendEvent
            if (sendEvent != null)
            {
                args.executer = this;
                args.command = command;
                args.transaction = transaction;
                sendEvent(this, args);
            }
            else
            {
                ArrayList message = new ArrayList();
                message.Add("ERROR: null sendEvent!!!");
                Output(message);
            }

			return args;
		}

		/// <summary>
		/// Looks at an array of string arrays, examines a particular field to see if there are any variable references
		/// of the format $commandName.parameterName, and returns an array of parameters that have such references
		/// but not the commandName nor parameterName. valueFieldIndex specifies the field to look at.
		/// </summary>
		/// <param name="varList"></param>
		/// <param name="fieldToLookAt"></param>
		/// <returns></returns>
		private ArrayList FindVariableReferences(ParameterContainer fieldList)
		{
			ArrayList references = new ArrayList();

			foreach (ParameterField field in fieldList.Fields)
			{
				if (field.Value.StartsWith("$"))
					references.Add(field);
			}
			
			return references;
		}


		/// <summary>
		/// Takes the fields specified, looks at which named command they are pointing to, and references that field
		/// to the variableTable
		/// </summary>
		private void ReferenceVariables(ParameterContainer varList, ArrayList references, ParameterBase.ParameterType type)
		{
			//holds the commandName and parameterName fields of $commandName.parameterName
			string[] referenceSections;
			string commandName;
			string parameterName;

			//this is the command (commandName) that we want a parameter to reference
			Command referencedCommand = null;
			//and this is the list of commandToPointTo fields we want to reference into
			ParameterContainer referencedFieldSection = null;

			//for every parameter that contains a reference to another command...
			foreach (ParameterField reference in references)
			{
				
				//obtain commandName and parameterName
				referenceSections = reference.Value.Substring(1).Split(new char[] {'.'});
				Debug.Assert(referenceSections.Length == 2,"WARNING: Variable specification not in commandName.paramName format");

				commandName = referenceSections[CMDNAME_INDEX];
				parameterName = referenceSections[CMDVALUE_INDEX];

				//use the first field of the referencePair array, which is the name of referenced command
				//to find the specific command in the command table
				referencedCommand = RetrieveCommandFromTable(commandName);
				Debug.Assert(referencedCommand != null, "WARNING: Command " + commandName + " NOT found in the variableTable");

				switch (type)
				{
					case ParameterBase.ParameterType.FINALRETURN : referencedFieldSection = referencedCommand.finalReturns; break;
					case ParameterBase.ParameterType.PROVISIONALRETURN : referencedFieldSection = referencedCommand.provisionalReturns; break;
				}

				
				Debug.Assert(referencedFieldSection != null, "WARNING: Referenced field section not found!");
				string referencedValueField = referencedFieldSection.GetFieldByName(parameterName).Value;
				Debug.Assert(referencedValueField != null,"WARNING: The referenced field was NOT found!");

				bool success = varList.SetFieldValue(reference.Name,referencedValueField);

				Debug.Assert(success, "WARNING: Failed to reference to " + commandName + "." + parameterName);
			}
		}

		#endregion

        #region Methods dealing with creating output.
		/// <summary>
		/// Prints an ArrayList of strings.
		/// </summary>
		/// <param name="message"></param>
        public void Output(ArrayList message)
		{
			foreach (string outputLine in message)
				Console.WriteLine(outputLine);
		}

		/// <summary>
		/// Creates an ArrayList of messages describing the result of the transaction as returned by the media server. 
		/// </summary>
		/// <param name="result"></param>
		/// <param name="transaction"></param>
		/// <param name="message"></param>
        private void OutputTransactionResults(SessionManager.SendEventArgs args)
		{
            ArrayList outputMessage = new ArrayList();
            string outputString; 
            
            if (args.transaction.status == MsTransactionInfo.TRANSACTION_RESULT.SUCCEEDED)
            {
                outputString = string.Format("SESSION {0}: Transaction {1} result from the MMS was:  SUCCESS", args.session.Name, args.transaction.Id);
                outputMessage.Add(string.Copy(outputString));
                Output(outputMessage);
            }
            else if (args.transaction.status == MsTransactionInfo.TRANSACTION_RESULT.EXECUTING)
            {
                outputString = string.Format("SESSION {0}: Transaction {1} result from the MMS was:  EXECUTING", args.session.Name, args.transaction.Id);
                outputMessage.Add(string.Copy(outputString));
                Output(outputMessage);
            }
            else
            {
                if (args.transaction.status == MsTransactionInfo.TRANSACTION_RESULT.COMMAND_TIMEOUT)
                {
                    outputString = string.Format("SESSION {0}: Transaction {1} result from the MMS was: TIMEOUT.\n Printing timed-out command:", args.session.Name, args.transaction.Id);
                    outputMessage.Add(string.Copy(outputString));
                    Output(outputMessage);
                }
                else
                {
                    outputString = string.Format("SESSION {0}: Transaction {1} result from the MMS was: NON-OK", args.session.Name, args.transaction.Id);
                    outputMessage.Add(string.Copy(outputString));
                    outputString = string.Format("SESSION {0}: Printing transaction {1} response:", args.session.Name, args.transaction.Id);
                    outputMessage.Add(string.Copy(outputString));
                    Output(outputMessage);
                }
            }
		}

        private void OutputCommand(Command cmd)
        {
            ArrayList outputMessage = new ArrayList();
            string outputString; 

            if (cmd.Name != string.Empty)
            {
                outputString = string.Format("Command name: {0}", cmd.Name);
                outputMessage.Add(string.Copy(outputString));
            }
            outputMessage.Add("Printing parameters:");
            outputMessage.AddRange(cmd.parameters.Output());
            if (cmd.isAsync)
            {
                outputMessage.Add("Printing provisional asserts:");
                outputMessage.AddRange(cmd.provisionalAsserts.Output());
            }

            outputMessage.Add("Printing final asserts:");
            outputMessage.AddRange(cmd.finalAsserts.Output());
            
            Output(outputMessage);        
        }
            

        private void OutputAssertResults(MsTransactionInfo transaction, ParameterContainer returns, AssertContainer assertsFields)
		{
            string typeOfAssert = string.Empty;
            string outcome = string.Empty;

            string outputString = string.Empty; 
            ArrayList outputMessage = new ArrayList();
            
            MsTransactionInfo.ASSERT_RESULT result; 

            if (transaction is MsAsyncTransactionInfo)
            {
                MsAsyncTransactionInfo asyncTranaction = transaction as MsAsyncTransactionInfo;
                if (asyncTranaction.finalAssertResult == MsTransactionInfo.ASSERT_RESULT.NOT_PERFORMED)
                    result = asyncTranaction.provisionalAssertResult;
                else
                    result = asyncTranaction.finalAssertResult;
            }
            else
                result = transaction.finalAssertResult;

			switch (result)
			{
				case MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_FAILED : typeOfAssert = "Final Assert"; outcome = "FAIL"; break;
				case MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_NOT_MATCHED : typeOfAssert = "Final Assert"; outcome = "NOT MATCHED"; break;
				case MsTransactionInfo.ASSERT_RESULT.FINAL_ASSERT_OK : typeOfAssert = "Final Assert"; outcome = "SUCCESS"; break;
				case MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_FAILED : typeOfAssert = "Provisional Assert"; outcome = "FAIL"; break;
				case MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_NOT_MATCHED : typeOfAssert = "Provisional Assert"; outcome = "NOT MATCHED"; break;
				case MsTransactionInfo.ASSERT_RESULT.PROVISIONAL_ASSERT_OK : typeOfAssert = "Provisional Assert"; outcome = "SUCCESS"; break;
			}

			outputString = string.Format("{0:-15} {1:10} {2}", typeOfAssert, "check result: ", outcome);
			outputMessage.Add(string.Copy(outputString));
			outputString = "Displaying matched list of received fields to assert fields...";
			outputMessage.Add(string.Copy(outputString));
			
			AssertField matchedField;
			foreach (ParameterField field in returns)
			{
                outputString = string.Format("{0,-15}: {1,15}\t", field.Name, field.Value);;

				matchedField = assertsFields.GetFieldByName(field.Name);
								
				//If the field was not matched
				if (matchedField == null)
				{
					outputString += "Not defined";
				}
				else
				{
					if (matchedField.Success)
                        outputString += string.Format("{0:-10} {1:-3} {2:-10} {3}", matchedField.Name, matchedField.Operator, matchedField.Value, "PASSED");
					else
						outputString += string.Format("{0:-10} {1:-3} {2:-10} {3}", matchedField.Name, matchedField.Operator, matchedField.Value, "FAILED");
				}
				outputMessage.Add(string.Copy(outputString));
        	}
            Output(outputMessage);
		}
        #endregion

        #region Methods dealing with the pendingCommands table
        /// <summary>
		/// Adds a command to the pendingCommands table, using the transactionId as hash.
		/// </summary>
		/// <param name="sessionScript"></param>
		/// <returns></returns>
		private void AddPendingCommand(MsTransactionInfo transaction, Command command)
		{
			lock (pendingCommands.SyncRoot)
			{
				Debug.Assert(transaction.Id != string.Empty, "WARNING: Script instance Attempted to add a command without a transactionId");
				Debug.Assert(!pendingCommands.ContainsKey(transaction.Id),"WARNING: Failed to add duplicate entry into pending commands table");
				pendingCommands.Add(transaction.Id,command);
			}
		}
		
		/// <summary>
		/// looks for a command associated to a specific transactionId, then removes it from the hash and return the command
		/// </summary>
		/// <param name="transactionId"></param>
		/// <returns></returns>
		private Command PopPendingCommand(string transactionId)
		{
			lock (pendingCommands.SyncRoot)
			{
				if (!pendingCommands.ContainsKey(transactionId))
					return null;

				Command command = (Command)pendingCommands[transactionId];
				pendingCommands.Remove(transactionId);
				return command;
			}
			
		}
        #endregion

		private string GetMessageFieldByName(string fieldName, ParameterContainer message)
		{
			foreach (ParameterField field in message)
			{
				if (string.Compare(fieldName, field.Name, true) == 0)
					return field.Value;
			}

			return null;
		}
												 
		#region variableTable methods
		/// <summary>
		/// Fills the variableTable with those commands for which a name was specified. 
		/// </summary>
		private void PopulateVariableTable()
		{
			foreach (Command cmd in script.Commands)
			{
				//if the command has a name, it needs to be in the variable table for later access
                if (cmd.Name != string.Empty)
                {
                    try
                    {
                        variableTable.Add(cmd.Name,cmd);
                    }
                    catch
                    {
                        Console.WriteLine("WARNING: TWO DIFFERENT COMMANDS HAVE THE SAME NAME, CHECK YOUR SCRIPT!");
                        //Needs to be replaced with a SessionManager.Shutdown()
                        System.Environment.Exit(0);
                    }
                }
			}
		}
		
		//TODO: ReadAssert and WriteParameter need to be abstracted
		/// <summary>
		/// Finds a named command in the variableTable, and returns it. returns null if the command is not found.
		/// </summary>
		/// <param name="cmdName"></param>
		/// <returns></returns>
		private Command RetrieveCommandFromTable(string cmdName)
		{
			try
			{
				if (variableTable.ContainsKey(cmdName))
				{
					Command commandToRetreive = (Command)variableTable[cmdName];
					return commandToRetreive;
				}
			}
			catch (System.Exception e)
			{}
				
			return null;
		}

		
		/// <summary>
		/// reads the value fieldName from the specified field
		/// May return null if the field is not found
		/// </summary>
		/// <param name="commandToReadFrom"></param>
		/// <param name="fieldList"></param>
		/// <param name="fieldName"></param>
		/// <param name="valueIndex"></param>
		/// <returns></returns>
		private string ReadFieldValue(ParameterContainer fields, string fieldName)
		{
			return fields.GetFieldByName(fieldName).Value;
		}

		/// <summary>
		/// writes the value val to field fieldName
		/// returns true if the write succeeded, false otherwise.
		/// </summary>
		/// <param name="fieldList"></param>
		/// <param name="fieldName"></param>
		/// <param name="val"></param>
		/// <param name="valueIndex"></param>
		/// <returns></returns>
		private bool WriteFieldValue(ParameterContainer fields, string fieldName, string val)
		{
			return fields.SetFieldValue(fieldName,val);
		}

		#endregion
		
	}
}

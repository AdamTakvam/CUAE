using System;
using System.Diagnostics;

using System.Collections;
using System.Xml.Serialization;
using Metreos.MMSTestTool.Sessions;


namespace Metreos.MMSTestTool.Commands
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Command : CommandBase
	{
	
        #region Properties
        /// <summary>
        /// returns the type of the command (ie "connect")
        /// </summary>
		public string CommandType
		{
			get
			{
				return commandType;
			}
		}

		/// <summary>
		/// Returns true if this is an asynchronous transaction
		/// </summary>
		public bool isAsync
		{
			get
			{
				return this.async;
			}
        }
        #endregion
        
		#region variable declarations
		
        //Containers that hold the parameters and asserts of the command, as defined
		public ParameterContainer parameters;
		public AssertContainer provisionalAsserts;
		public AssertContainer finalAsserts;
		
        //Containers that hold the values returned from the MMS
		public ParameterContainer finalReturns;
		public ParameterContainer provisionalReturns;

        //Hashtables used to keep track of how many occurances of each assert or parameter there are. 
        //These need to be removed from the class definition and passed as parameters instead, since 
        //they are not used after the command specification is verified. 
		protected Hashtable parameterCount;
		protected Hashtable provisionalAssertCount;
		protected Hashtable finalAssertCount;

        /// <summary>
        /// This hashtable stores the command options for this command.
        /// </summary>
		public Hashtable optionsTable;
		
		//refers to the relevant CommandDescription element in CommandsDescriptions
		private CommandDescription commandDescriptionReference;

		private bool async;
		private bool waitForFinal = false;
		private string commandType;
        #endregion
		
		#region Constructors

        public Command()
        {
            //Empty default constructor
        }

		public Command(bool async, CommandDescription commandDescriptionReference, string commandType, string name, ArrayList separatedParameters, ArrayList separatedProvisionalAsserts, ArrayList separatedAsserts, ArrayList separatedOptions)
		{
            this.async = async;
            this.commandDescriptionReference = commandDescriptionReference;
            this.commandType = commandType;
            this.name = name;
            
            parameters				= new ParameterContainer();
            provisionalAsserts		= new AssertContainer();
            finalAsserts			= new AssertContainer();
			
            parameterCount			= new Hashtable();
            provisionalAssertCount	= new Hashtable();
            finalAssertCount		= new Hashtable();

            finalReturns            = new ParameterContainer();
            provisionalReturns      = new ParameterContainer();
            
            optionsTable            = new Hashtable();
            
            if (separatedOptions.Count != 0)
                ProcessOptions(separatedOptions);

            //ParameterType is an enum that holds parameter, provisionalAssert, finalAssert
            //so that the ValidateAndPopulate method knows what it's dealing with
            if (separatedParameters.Count > 0)
                this.ValidateAndPopulate(separatedParameters, ParameterBase.ParameterType.PARAMETER);
            if (separatedProvisionalAsserts.Count > 0)
                this.ValidateAndPopulate(separatedProvisionalAsserts, ParameterBase.ParameterType.PROVISIONALASSERT);
            if (separatedAsserts.Count > 0)
                this.ValidateAndPopulate(separatedAsserts, ParameterBase.ParameterType.ASSERT);	

		}
		
		/// <summary>
		/// copy constructor for Command
		/// </summary>
		/// <param name="template"></param>
		public Command(Command template)
		{
			this.async = template.async;
			if (template.name != null)
				this.name = string.Copy(template.name);
			this.commandType = string.Copy(template.commandType);
			
			this.parameters = new ParameterContainer(template.parameters);
			this.finalAsserts = new AssertContainer(template.finalAsserts);
			this.provisionalAsserts = new AssertContainer(template.provisionalAsserts);

			this.finalReturns = new ParameterContainer(template.finalReturns);
			this.provisionalReturns = new ParameterContainer(template.provisionalReturns);

			//copy the respective counts - shallow copy is good enough for this
			this.parameterCount = (Hashtable)template.parameterCount.Clone();
			this.finalAssertCount = (Hashtable)template.finalAssertCount.Clone();
			this.provisionalAssertCount = (Hashtable)template.provisionalAssertCount.Clone();

            this.optionsTable = (Hashtable)template.optionsTable.Clone();
		}
		#endregion

		#region Methods dealing with validating parameters and asserts
		
        /// <summary>
        /// Processes the arraylist of command options such as waitForFinal and places them into the optionsTable
        /// </summary>
        /// <param name="options"></param>
        private void ProcessOptions(ArrayList options)
        {
               int name = 0; int val = 1;
               foreach (string[] option in options)
                   this.optionsTable.Add(option[name],option[val]);
        }

		/// <summary>
		/// Performs semantic validation of finalAsserts, provisionalAssert, and parameters blocks of the command.
		/// </summary>
		/// <param name="paramStringsList"></param>
		/// <param name="ptype"></param>
		/// <returns></returns>
		public ValidationReturns ValidateAndPopulate(ArrayList paramStringsList,ParameterBase.ParameterType ptype)
		{
		
			ParameterContainer destinationContainer = null;

			//holds return values of the ValidateAndCorrectNames, ValidateCardinality, and ValidateRequirements methods.
			ValidationReturns nameCheck			= ValidateAndCorrectNames(paramStringsList, ptype);
			ValidationReturns cardinalityCheck	= ValidateCardinality(paramStringsList, ptype);
			ValidationReturns requirementsCheck	= ValidateRequirements();

			if (nameCheck == cardinalityCheck && cardinalityCheck == requirementsCheck 
				&& requirementsCheck == ValidationReturns.VALID)
			{
				switch (ptype)
				{
					case ParameterBase.ParameterType.ASSERT				: destinationContainer = this.finalAsserts; break;
					case ParameterBase.ParameterType.PROVISIONALASSERT	: destinationContainer = this.provisionalAsserts; break;
					case ParameterBase.ParameterType.PARAMETER			: destinationContainer = this.parameters; break;
				}

				foreach (string[] param in paramStringsList)
					destinationContainer.Add(param);
				
				return ValidationReturns.VALID;
			}
            else throw (new System.Exception("Command " + this.commandType + " FAILED Validation."));

			return ValidationReturns.INVALID;
		}


		/// <summary>
		/// Check to see that all the requirements for this command have been specified
		/// </summary>
		/// <returns></returns>
		//commandDescriptionReference points to the CommandDescription describing this type of command.
		private ValidationReturns ValidateRequirements()
		{

			//check parameters first
			for (int i = 0; i < commandDescriptionReference.parameters.Length; i++)
			{
				//Check for required parameters, and the andOrWith condition (two parameters are considered required, but
				//only one of them needs to be present)
				if (commandDescriptionReference.parameters[i].required)
				{
					if (!parameterCount.ContainsKey(commandDescriptionReference.parameters[i].name))
					{
						if (commandDescriptionReference.parameters[i].andor != null && 
								!parameterCount.ContainsKey(commandDescriptionReference.parameters[i].andor))
							return ValidationReturns.MISSING_REQUIRED_PARAM;
					}
				}

				//check for excluding conditions on parameters
				if (commandDescriptionReference.parameters[i].excludes != null)
				{
					for (int j = 0; j < commandDescriptionReference.parameters[i].excludes.Length; j++)
					{
						if (parameterCount.ContainsKey(commandDescriptionReference.parameters[i].excludes[j]))
							return ValidationReturns.CONFLICTING_PARAMETERS;
					}
				}
			}

			//check asserts to see if the required ones are there
			for (int i = 0; i < commandDescriptionReference.asserts.Length; i++)
			{
				if (commandDescriptionReference.asserts[i].required)
				{
					if (!finalAssertCount.ContainsKey(commandDescriptionReference.asserts[i].name))
						return ValidationReturns.MISSING_REQUIRED_ASSERT;
				}
			}

			return ValidationReturns.VALID;
		}

							
        /// <summary>
        /// returns ValidationReturns.VALID if the number of parameters currently known is consistent with the commands descriptions.
        /// checks to see if the element is allowed to apprear more than once, then updates the count in the
        /// count hashtable. currently doesn't look at provisionalAsserts cardinality
        /// </summary>
        /// <param name="paramStringsList"></param>
        /// <param name="ptype"></param>
        /// <returns></returns>
		private ValidationReturns ValidateCardinality(ArrayList paramStringsList, ParameterBase.ParameterType ptype)
		{
			bool isKeyInHashtable;
			ParameterBase[] checkAgainst = null;
			ParameterBase matchingElement;

			Hashtable countTable = null;
			switch (ptype)
			{
				case ParameterBase.ParameterType.ASSERT :
					countTable = this.finalAssertCount;
					checkAgainst = this.commandDescriptionReference.asserts;
					break;
				case ParameterBase.ParameterType.PROVISIONALASSERT:
					countTable = this.provisionalAssertCount;
					checkAgainst = this.commandDescriptionReference.asserts;
					break;
				case ParameterBase.ParameterType.PARAMETER :
					countTable = this.parameterCount;
					checkAgainst = this.commandDescriptionReference.parameters;
					break;
			}

			if (checkAgainst == null || countTable == null)
				return ValidationReturns.INVALID_PARAMETER_NAME;

			foreach (string[] parameterElement in paramStringsList)
			{
				matchingElement = null;

				foreach (ParameterBase elementIterator in checkAgainst)
				{
					if (string.Compare(parameterElement[0],elementIterator.name) == 0)
					{
						matchingElement = elementIterator;
						break;
					}
				}
            
				if (matchingElement == null)
					return ValidationReturns.INVALID_PARAMETER_NAME;
				
				isKeyInHashtable = countTable.ContainsKey(parameterElement[0]);

				if (matchingElement.allowMultiple)
				{
					if (!isKeyInHashtable)
						countTable[parameterElement[0]] = 1;
					else
						countTable[parameterElement[0]] = (int)countTable[parameterElement[0]] + 1;
				}
				else
				{
					if (!isKeyInHashtable)
						countTable[parameterElement[0]] = 1;
					else
						return ValidationReturns.INVALID;
				}
			}

			return ValidationReturns.VALID;
		}

		/// <summary>
		/// this method will check the names of the parameters specified against either the 
		/// asserts or parameters array of the commandDescriptionReference
		/// </summary>
		/// <param name="paramStringsList"></param>
		/// <param name="checkAgainst"></param>
		private ValidationReturns ValidateAndCorrectNames(ArrayList paramStringsList, ParameterBase.ParameterType ptype) 
		{
			ParameterBase matchingElement;
			ParameterBase[] checkAgainst = null;
			
			switch (ptype)
			{
				case ParameterBase.ParameterType.ASSERT : //fall through
				case ParameterBase.ParameterType.PROVISIONALASSERT:
					 checkAgainst = commandDescriptionReference.asserts;
					 break;
				case ParameterBase.ParameterType.PARAMETER :
					 checkAgainst = commandDescriptionReference.parameters;
					 break;
			}

			if (checkAgainst == null)
				return ValidationReturns.INVALID;

			foreach (string[] paramStringArray in paramStringsList)
			{
				//initialize matchingElement to null for each pass of the below loop
				matchingElement = null;
				
				//find the parameter in the assert/parameter object of CommandDescription
				foreach (ParameterBase element in checkAgainst)
				{
					if (string.Compare(paramStringArray[0], element.name, true) == 0)
					{
						matchingElement = element;
						break;
					}
				}

				if (matchingElement == null)
					return ValidationReturns.INVALID_PARAMETER_NAME;
			
				//set the parameter name to be the same as the specification
				paramStringArray[0] = matchingElement.name;
			}
			
			return ValidationReturns.VALID;
		}

		#endregion

        public override string ToString()
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            sBuilder.AppendFormat("{0} {1}{2}", this.commandType, this.name, "\n{\n");
            string tabulator = "\t";

            if (this.parameterCount.Count > 0)
            {
                foreach (string pf in this.parameters.Output())
                    sBuilder.AppendFormat("{0}{1}{2}", tabulator, pf, "\n");
            }
            if (this.provisionalAssertCount.Count > 0)
            {
                sBuilder.Append("\n");
                sBuilder.AppendFormat("{0}{1}{0}{2}", tabulator, "ProvisionalAsserts\n", "{\n");
                tabulator = "\t\t";
                foreach (string paf in this.provisionalAsserts.Output())
                    sBuilder.AppendFormat("{0}{1}{2}", tabulator, paf, "\n");
                
                tabulator = "\t";
                sBuilder.AppendFormat("{0}{1}", tabulator, "}\n");
            }
            if (this.finalAssertCount.Count > 0)
            {
                sBuilder.Append("\n");
                sBuilder.AppendFormat("{0}{1}{0}{2}", tabulator, "FinalAsserts\n", "{\n");
                tabulator = "\t\t";
                foreach (string faf in this.finalAsserts.Output())
                    sBuilder.AppendFormat("{0}{1}{2}", tabulator, faf, "\n");
                
                tabulator = "\t";
                sBuilder.AppendFormat("{0}{1}", tabulator, "}\n");
            }
            sBuilder.Append("}");

            return sBuilder.ToString();
        }

        /// <summary>
        /// Enum type, used for specifying the results of command validation.
        /// </summary>
		public enum ValidationReturns
		{
			VALID, INVALID, MISSING_REQUIRED_PARAM, MISSING_REQUIRED_ASSERT, CONFLICTING_PARAMETERS, TOO_MANY_PARAM_OCCURANCES,
			TOO_MANY_ASSERT_OCCURANCES, INVALID_PARAMETER_NAME
		}

	}
}

using System;
using System.Collections;


namespace Metreos.MMSTestTool.Commands
{
	
	/// <summary>
	/// This class contains multiple instances of ParameterField objects and provides methods for retrieving and setting their values
	/// </summary>
	public class ParameterContainer : IEnumerable
	{
		protected ArrayList fields;

		public ArrayList Fields
		{
			get
			{
				return fields;
			}
		}

		public int NumberOfParameters
		{
			get
			{
				return fields.Count;
			}
		}

		public ParameterContainer()
		{
			fields = new ArrayList();
		}

		public ParameterContainer(ParameterContainer template)
		{
			this.fields = new ArrayList();

			foreach (ParameterField field in template.fields)
			{
				this.fields.Add(new ParameterField(string.Copy(field.Name),string.Copy(field.Value)));
			}
		}
        
		public virtual void Add(string[] param)
		{
			fields.Add(new ParameterField(param[ParameterField.NAME_INDEX],param[ParameterField.VALUE_INDEX]));
		}

		public virtual void Add(string name, string val)
		{
			fields.Add(new ParameterField(name,val));
		}

        public virtual void Add(ParameterField pfield)
        {
            fields.Add(pfield);
        }

		//removes all occurances of a specific parameter from the fields list
		public void Remove(string name)
		{
			foreach (ParameterField param in this.fields)
			{
                if (string.Compare(name,param.Name,true) == 0)
                {
                    fields.Remove(param);
                    this.Remove(name);
                    break;
                }
			}
		}
		
					
        /// <summary>
        /// Outputs the parameters contained by this container
        /// </summary>
        /// <param name="container"></param>
        public virtual ArrayList Output()
        {
            ArrayList message = new ArrayList();
            string output; 

            if (this.fields.Count == 0)
            {
                output = "None defined.";
                message.Add(string.Copy(output));
                return message;
            }
            else
            {
                foreach (ParameterField field in this.fields)
                {
                    output = string.Format("{0}: {1}",field.Name,field.Value);
                    message.Add(string.Copy(output));
                }
                
                return message;
            }
        }

		/// <summary>
		/// This method takes the name of the field whose value we wish to know, and returns that value or null, if the field was not found
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ParameterField GetFieldByName(string name)
		{
			foreach (ParameterField param in this.fields)
			{
				if (string.Compare(name, param.Name, true) == 0)
					return param;
			}

			//if field was not found, we return null
			return null;
		}

		/// <summary>
		/// Sets the value of the specified field
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public bool SetFieldValue(string name, string val)
		{
			ParameterField fieldToSet = this.GetFieldByName(name);
			
			//if the field was not found
			if (fieldToSet == null)
				return false;

			fieldToSet.Value = val; 
						
			return true;
		}
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			// TODO:  Add ParameterContainer.GetEnumerator implementation
			return fields.GetEnumerator();
		}

		#endregion
	}
	
	
	public class AssertContainer : ParameterContainer
	{
		/// <summary>
		/// copy constructor.
		/// </summary>
		/// <param name="template"></param>
		public AssertContainer(AssertContainer template)
		{
			this.fields = new ArrayList();

			foreach (AssertField field in template.fields)
				this.fields.Add(new AssertField(string.Copy(field.Name), string.Copy(field.Operator), string.Copy(field.Value)));
		}

		/// <summary>
		/// empty public constructor
		/// </summary>
		public AssertContainer() : base() {}

		public override void Add(string[] assert)
		{
			fields.Add(new AssertField(assert[AssertField.NAME_INDEX], assert[AssertField.OPERATOR_INDEX], assert[AssertField.VALUE_INDEX]));
		}

		
		public void Add(string name, string op, string val)
		{
			fields.Add(new AssertField(name, op, val));
		}

		public new AssertField GetFieldByName(string name)
		{
			foreach (AssertField assert in this.fields)
			{
				if (string.Compare(name, assert.Name, true) == 0)
					return assert;
			}

			//if field was not found, we return null
			return null;
		}
        
        /// <summary>
        /// Outputs the asserts contained by this container
        /// </summary>
        /// <param name="container"></param>
        public override ArrayList Output()
        {
            ArrayList message = new ArrayList();
            string output; 

            if (this.fields.Count == 0)
            {
                output = "None defined.";
                message.Add(string.Copy(output));
                return message;
            }
            else
            {
                foreach (AssertField field in this.fields)
                {
                    output = string.Format("{0} {1} {2}",field.Name, field.Operator, field.Value);
                    message.Add(string.Copy(output));
                }
                
                return message;
            }
        }
	}
	
	/// <summary>
	/// This Class holds the Name and Value of a paremeter
	/// </summary>
	public class ParameterField
	{
		public const int NAME_INDEX = 0;
		public const int VALUE_INDEX = 1;

		public ParameterField(string name, string val)
		{
			this.name = name;
			this.val = val;
		}

		public string Name
		{
			get 
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
			}
		}

		protected string name;
		protected string val;
	}
	
	/// <summary>
	/// This class holds the Name, Operator, and Value fields of an assert.
	/// </summary>
	public class AssertField : ParameterField
	{
		public new const int VALUE_INDEX = 2;
        public const int OPERATOR_INDEX = 1;
        protected string op;
        private bool success = false;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

		public string Operator
		{
			get 
			{
				return op;
			}
			set
			{
				op = value;
			}
		}

		public AssertField(string name, string op, string val) : base(name,val)
		{
			this.op = op;
		}
    }
}

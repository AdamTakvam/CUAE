using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;

using Metreos.MMSTestTool;


namespace Metreos.MMSTestTool.Commands
{

    /// <summary>
    /// This is ugly. Needs to be re-written in parts, but it works for the time being.
    /// </summary>
	public class CommandDescriptionContainerHandler
	{
	
		public static bool Initialized
		{
			get
			{
				return initialized;
			}
			set
			{
				initialized = value;
			}
		}
		

		public static CommandDescription[] CommandList
		{
			get
			{
				return descriptions.CommandList;
			}
			set
			{
				descriptions.CommandList = value;
			}
		}

			
		private static CommandDescriptionCollection descriptions = new CommandDescriptionCollection();
		
		private static bool initialized = false;		
		//we need the public constructor for deserialization

        /// <summary>
        /// Reads the XML description for the commands from the specified filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
		public static bool ReadXmlDescription(string filename)
		{
            if (!CommandDescriptionContainerHandler.Initialized)
            {
				
                FileInfo file = new FileInfo(filename);
                using (FileStream stream = file.Open(FileMode.Open))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CommandDescriptionCollection));
                        CommandDescriptionContainerHandler.descriptions = serializer.Deserialize(stream) as CommandDescriptionCollection;
                        CommandDescriptionContainerHandler.Initialized = true;
				
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
				

                return true;
            }
            else
            {
                CommandDescriptionContainerHandler.Initialized = false;
                CommandDescriptionContainerHandler.descriptions = null;
                return ReadXmlDescription(filename);
            }

			return false;
		}
		


		/// <summary>
		/// returns an object of type CommandDescription for a specific type that will be used by Command.ValidateAndPopulate
		/// </summary>
		/// <param name="commandType"></param>
		public static CommandDescription FindCommand(string commandType)
		{
			int result;
			for (int i = 0; i < CommandDescriptionContainerHandler.CommandList.Length; i++)
			{
				result = string.Compare(commandType,CommandDescriptionContainerHandler.CommandList[i].name,true);
				if (result == 0)
					return CommandDescriptionContainerHandler.CommandList[i];
			}
			return null;

		}
	}

		

	[Serializable()]
    [XmlRoot("CommandDescriptionCollection")]
    public class CommandDescriptionCollection
    {
		[XmlIgnore]
		public CommandDescription[] CommandList
		{
			get
			{
				return commandList;
			}
			
			set
			{
				commandList = value;
			}
		}

		[XmlElement("CommandDescription")]
		public CommandDescription[] commandList;

		public CommandDescriptionCollection() {}
    }

    [Serializable()]
    [XmlRoot("CommandDescription")]
	public class CommandDescription
	{
		public CommandDescription() {}

		[XmlAttribute("name")]
        public string name;

		[XmlAttribute("asynchronous")]
		public bool async;

        [XmlArrayItem("parameter")]
        public Parameter[] parameters;

        [XmlArrayItem("assert")]
        public Assert[] asserts;
		
		public Parameter FindParameter(string param)
		{
			return (Parameter)this.Find(param, ParameterBase.ParameterType.PARAMETER);		
		}

		public Assert FindAssert(string param)
		{
			return (Assert)this.Find(param, ParameterBase.ParameterType.ASSERT);
		}

		private Object Find(string param, ParameterBase.ParameterType ptype)
		{
			switch (ptype)
			{
				//fall through
				case ParameterBase.ParameterType.PROVISIONALASSERT : 
				case ParameterBase.ParameterType.ASSERT : 
					for (int i = 0; i < asserts.Length; i++)
					{
						if (string.Compare(param, asserts[i].name, true) == 0)
							return (Object)asserts[i];
					}
					return null; 
				case ParameterBase.ParameterType.PARAMETER :
					for (int i = 0; i < parameters.Length; i++)
					{
						if (string.Compare(param,parameters[i].name,true) == 0)
							return (Object)parameters[i];
					}
					return null; 
				default : return null; 
			}
		}
	}


	[Serializable()]
	[XmlInclude(typeof(Parameter))]
	[XmlInclude(typeof(Assert))]
	public class ParameterBase
	{
		[XmlAttribute("required")]
		public bool required;

		[XmlAttribute("multiple")]
		public bool allowMultiple;

		[XmlText]
		public string name;
		
		public ParameterBase() {}
		
		public enum ParameterType
		{
			PARAMETER, ASSERT, PROVISIONALASSERT, FINALRETURN, PROVISIONALRETURN
		}
	}

    [Serializable()]
    [XmlRoot("parameter")]
    public class Parameter : ParameterBase
    {
        /// <summary>
        /// used for required parameters that form a matrix
        /// </summary>
		//currently only used for the disconnect command, potentially update later
		//to allow for more than one command
		[XmlAttribute("andOrWith")]
		public string andor;

		[XmlAttribute("excludes")]
		public string[] excludes;

		public Parameter() : base() {} 
    }

    [Serializable()]
	[XmlRoot("assert")]
    public class Assert : ParameterBase
    {
        //specifies whether this value _CAN_ be a member of the ProvisionalAssert
		[XmlAttribute("provisional")]
		public bool provisional;

		public Assert() : base() {} 
    }
}

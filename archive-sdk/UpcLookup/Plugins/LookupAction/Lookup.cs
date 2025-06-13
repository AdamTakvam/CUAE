using System;
using System.IO;
using System.Text;
using System.Collections;

using Metreos.Core;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.Native.Upc
{
	/// <summary>
	/// Native actions to build Cisco IP phone XML
	/// </summary>
	[PackageDecl("Metreos.Native.Upc")]
	public class Lookup : INativeAction
	{
		private static Hashtable loadedContents = Load();
	
		public class Data
		{
			public string title;
			public string type;

			public Data(string title, string type)
			{
				this.title = title;
				this.type  = type;
			}
		}
		private static Hashtable Load()
		{
			Hashtable allItems = new Hashtable();

			FileStream stream = null;
			StreamReader reader = null;
			try
			{
				stream = File.Open("c:\\items.csv", FileMode.Open);

				reader = new StreamReader(stream);

				while(reader.Peek() != -1)
				{
					string line = reader.ReadLine();
					string[] itemData = line.Split(new char[] {','});

					if(itemData != null && itemData.Length == 3)
					{
						if(itemData[0] == null) continue;
						string upc = itemData[0].Trim();
						string type = itemData[1].Trim();
						string title = itemData[2].Trim();

						allItems[upc] = new Data(title, type);
					}
				}
			}
			catch
			{
				Console.WriteLine("UPC Codes not installed into c:\\");
			}
			finally
			{
				if(reader != null)
				{
					reader.Close();
				}
			}

			return allItems;
		}



		private LogWriter log;
		public LogWriter Log { set { log = value; } }
        
		[ResultDataField("The name of the item.")]
		public string Name { get { return name; } }
		private string name;

		[ResultDataField("The type of the item.")]
		public string Type { get { return type; } }
		private string type;

		[ActionParamField(true)]
		public string Upc { set { upc = value; } }
		private string upc;

		public Lookup() { }

		public bool ValidateInput()
		{
			return true;
		}

		public void Clear()
		{
			name = String.Empty;
			type = String.Empty;
		}

		[Action("Lookup", false, "Lookup", "Looks up the information for a UPC code.")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility) 
		{
            name = String.Empty;
            type = String.Empty;

			Data data = loadedContents[upc] as Data;
			
			if(data == null) return IApp.VALUE_FAILURE;

			else 
			{
				name = data.title;
				type = data.type;
			}

			log.Write(System.Diagnostics.TraceLevel.Info, "Name {0} Type {1}");
			if(name == null) name = String.Empty;
			if(type == null) type = String.Empty;

			return IApp.VALUE_SUCCESS;
		}
	}
}

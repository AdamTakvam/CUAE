using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;

using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.AxlSoap;
using Metreos.AxlSoap504;
using Metreos.Types.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.ParseSQLQuery;

namespace Metreos.Native.AxlSoap504
{
	/// <summary> Wraps up the 'ExecuteSQLQuery' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class ParseSQLQuery: INativeAction
	{

        [ActionParamField(Package.Params.ExecuteSQLQueryResponse.DISPLAY, Package.Params.ExecuteSQLQueryResponse.DESCRIPTION, false, Package.Params.ExecuteSQLQueryResponse.DEFAULT)]
        public ExecuteSQLQueryResponse ExecuteSQLQueryResponse { set { response = value; } }
 
        [ResultDataField(Package.Results.DataTable.DISPLAY, Package.Results.DataTable.DESCRIPTION)]
        public DataTable DataTable { get { return table; } }

        public LogWriter Log { set { log = value; } }

		private ExecuteSQLQueryResponse response; 
        private DataTable table;
        private LogWriter log;

		public ParseSQLQuery()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.response						= new ExecuteSQLQueryResponse();
            this.table							= new DataTable();
        }

        public bool ValidateInput()
        {
            return true;
        } 

        public enum Result
        {
            success,
            failure,
            fault,
        }

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {

			if(response == null || response.Response == null || response.Response.@return == null || response.Response.@return.Length == 0)
			{
				// No items in response, do nothing
			}
			else
			{
				object[] rows = response.Response.@return;

				bool firstPass = true;

				foreach(object row in rows)
				{
					if(row != null)
					{
						// Build rows

						if(row is System.Array)
						{
							Array elements = row as Array;

							if(firstPass)
							{
								foreach(object element in elements)
								{
									if(element is XmlElement)
									{
										XmlElement xmlNode = element as XmlElement;
										string columnName = xmlNode.Name;
										table.Columns.Add(columnName, typeof(string));
									}
								}
								firstPass = false;
							}

							DataRow newRow = table.NewRow();
								
							foreach(object element in elements)
							{
								if(element is XmlElement)
								{
									XmlElement xmlNode = element as XmlElement;

									string columnName = xmlNode.Name;
									string columnValue = null;
									if(!xmlNode.IsEmpty) // empty tag means null, (<tag/>), full tag means value or empty string (<tag></tag>)
									{
										columnValue = xmlNode.InnerText;
									}

									newRow[columnName] = columnValue;
								}
							}

							table.Rows.Add(newRow);
						}
						else
						{
							// assume 1 element return

							DataRow newRow = table.NewRow();

							if(row is XmlElement)
							{
								if(firstPass)
								{
									XmlElement xmlNode = row as XmlElement;
									string columnName = xmlNode.Name;
									table.Columns.Add(columnName);
									firstPass = false;
								}

								XmlElement xmlNode2 = row as XmlElement;
								
								string columnName2 = xmlNode2.Name;
								string columnValue2 = null;
								if(!xmlNode2.IsEmpty) // empty tag means null, (<tag/>), full tag means value or empty string (<tag></tag>)
								{
									columnValue2 = xmlNode2.InnerText;
								}

								newRow[columnName2] = columnValue2;
							}

							table.Rows.Add(newRow);
							firstPass = false;
						}
					}
				}
			}
            
            return IApp.VALUE_SUCCESS;
        }           
	}
}

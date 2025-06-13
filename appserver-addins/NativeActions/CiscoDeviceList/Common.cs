using System;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Native.CiscoDeviceList
{
	/// <summary> Methods and resources common to all CDeviceListX Native Actions </summary>
	public class Common
	{
        public enum SearchType 
        {
            Query,
            Exclude
        }

		public Common()
		{
		}

        public static void SearchOnCriteria(Hashtable criteria, DataTable data, SearchType searchType)
        {
            foreach(DataColumn column in data.Columns)
            {
                string searchValue = criteria[column.ColumnName] as String;
                if(searchValue != null)
                {
                    SearchRows(searchType, data, column.ColumnName, searchValue);
                }
            }
        }

        /// <summary>
        ///     Iterate through the entire table, keeping/discarding rows based on criteria
        /// </summary>
        private static void SearchRows(SearchType searchType, DataTable data, string columnName, string searchValue)
        {
            if(data != null)
            {
                for(int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow row = data.Rows[i];
                    if(SearchType.Query == searchType)
                    {
                        if(searchValue != row[columnName] as string) // This is case sensitive.  sep/SEP?
                        {
                            data.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        if(searchValue == row[columnName] as string) // This is case sensitive.  sep/SEP?
                        {
                            data.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public static DataTable GetDeviceInfo(
            Hashtable criteria, 
            LogWriter log, 
            IConfigUtility configUtility, 
            SearchType searchType,
            out string response)
        {
            response = IApp.VALUE_FAILURE;

            string dsn = Database.FormatDSN(ICiscoDeviceList.DB_NAME, configUtility.DatabaseHost, configUtility.DatabasePort,
                configUtility.DatabaseUsername, configUtility.DatabasePassword, true);

            DataTable table = null;

            using(IDbConnection dbConn = Database.CreateConnection(Database.DbType.mysql, dsn))
            {
                try
                {
                    dbConn.Open();
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Could not open DeviceListX database at '" + dsn + "': " + e.Message);
                    response = ICiscoDeviceList.RESP_DB_ERROR;
                    return null;
                }

                string sqlCmd = null;
                if(searchType == SearchType.Query)
                {
                    sqlCmd = BuildSelectCommand(criteria);
                }
                else
                {
                    sqlCmd = BuildFilterCommand(criteria);
                }

                if(sqlCmd == null)
                {
                    log.Write(TraceLevel.Error, "A query attempt was made with no criteria specified.");
                    response = ICiscoDeviceList.RESP_NO_CRIT;
                    return null;
                }

                try 
                { 
                    using(IDbCommand command = dbConn.CreateCommand())
                    {
                        command.CommandText = sqlCmd;
                        using(IDataReader reader = command.ExecuteReader())
                        {
                            table = Database.GetDataTable(reader);
                        }
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Failed to execute database query. Error: " + e.Message);
                    response = ICiscoDeviceList.RESP_NOT_FOUND;
                    return null;
                }
            }

            if(table != null && table.Rows.Count > 0)
            {
                // Disallow blank status field in response
                foreach(DataRow row in table.Rows)
                {
                    if(System.Convert.IsDBNull(row["Status"]) || (row["Status"] as string == ""))
                    {
                        row["Status"] = "0";
                    }
                }

                response = IApp.VALUE_SUCCESS;
            }
            else
            {
                response = ICiscoDeviceList.RESP_NOT_FOUND;
            }
            
            return table;
        }  

        private static string BuildSelectCommand(Hashtable criteria)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, ICiscoDeviceList.TABLE_DEVICE_INFO);

            string _value = criteria[ICiscoDeviceList.FIELD_TYPE] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_TYPE, _value); }

            _value = criteria[ICiscoDeviceList.FIELD_NAME] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_NAME, _value); }

            _value = criteria[ICiscoDeviceList.FIELD_DESCR] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_DESCR, _value); }

            _value = criteria[ICiscoDeviceList.FIELD_SPACE] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_SPACE, _value); }

            _value = criteria[ICiscoDeviceList.FIELD_POOL] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_POOL, _value); }

            _value = criteria[ICiscoDeviceList.FIELD_IP] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_IP, _value); }

            // if device status was not specified by user, restrict query results to
            // devices that are registered
            _value = criteria[ICiscoDeviceList.FIELD_STATUS] as string;
            if (_value != null) 
                sql.where.Add(ICiscoDeviceList.FIELD_STATUS, _value);
            else
                sql.where.Add(ICiscoDeviceList.FIELD_STATUS, ICiscoDeviceList.STATUS_REGISTERED);

            _value = criteria[ICiscoDeviceList.FIELD_CCMIP] as string;
            if(_value != null) { sql.where.Add(ICiscoDeviceList.FIELD_CCMIP, _value); }

            return sql.ToString();
        }

        private static string BuildFilterCommand(Hashtable criteria)
        {
            if(criteria == null) { return null; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, ICiscoDeviceList.TABLE_DEVICE_INFO);
            sql.appendSemicolon = false;

            StringBuilder whereFilter = new StringBuilder(sql.ToString());
            whereFilter.Append(" WHERE ");
            
            int builderUntouchedCount = whereFilter.Length;

            string _value = criteria[ICiscoDeviceList.FIELD_TYPE] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_TYPE, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_NAME] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_NAME, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_DESCR] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_DESCR, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_SPACE] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_SPACE, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_POOL] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_POOL, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_IP] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_IP, _value);
            }

            _value = criteria[ICiscoDeviceList.FIELD_STATUS] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_STATUS, _value);
            }
            
            _value = criteria[ICiscoDeviceList.FIELD_CCMIP] as string;
            if(_value != null) 
            {
                whereFilter.AppendFormat("{0} != '{1}' AND ", ICiscoDeviceList.FIELD_CCMIP, _value);
            }

            // If we added a '!=' filter, then we will need to rip off the trailing AND
            if(builderUntouchedCount != whereFilter.Length)
            {
                whereFilter.Remove(whereFilter.Length-5, 5);

            }

            return whereFilter.ToString();
        }
	}
}

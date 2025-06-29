using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;


namespace Metreos.Utilities
{
	/// <summary>Creates SQL statements</summary>
	/// <remarks>This class is intended to work specifically with MySQL, 
	/// but may work with other databases as well.</remarks>
	[Serializable]
	public class SqlBuilder
	{
        public enum Method
        {
            Unspecified,
            INSERT,
            UPDATE,
            DELETE,
            REPLACE,
            SELECT
        }

        /// <summary>The SQL operation</summary>
        public Method method;

        /// <summary>The table to query or modify</summary>
        public string table = null;

        /// <summary>The names of the fields involved</summary>
        public StringCollection fieldNames = null;

        /// <summary>
        /// The field values (corresponding to the names in the fieldNames table)
        /// </summary>
        public ArrayList fieldValues = null;

        /// <summary>
        /// A list of SqlConditions to use for the where clause.
        /// Look at SqlConditions class and child classes for use.
        /// </summary>
        private ArrayList conditions = null;

        /// <summary>Restrictions on this command ('where' [key]=[value])</summary>
        public Hashtable where = null;

        /// <summary>Restrictions of the SQL "x LIKE y" format that get appended to the end of the where clause.
        public Hashtable like = null;

        /// <summary>Set ordering for this sql using "ORDER BY [key] [value]" where key = column name and key = ASC|DESC</summary>
        public Hashtable order = null;

        /// <summary>
        /// Indicates whether or not to append a semicolon to the resulting SQL statement
        /// </summary>
        public bool appendSemicolon;

        public SqlBuilder()
            : this(Method.Unspecified, null) {}

        public SqlBuilder(Method method)
            : this(method, null) {}

        public SqlBuilder(Method method, string table)
        {
            Clear();

            this.method = method;
            this.table = table;
        }

        /// <summary>Adds a field value modifier (VALUES) to this statement</summary>
        /// <param name="name"></param>
        /// <param name="Value"></param>
        public void AddFieldValue(string name, object Value)
        {
            fieldNames.Add(name);
            fieldValues.Add(Value);
        }

        public void AddCondition(SqlCondition conditionObj)
        {
            if (conditions == null) { conditions = new ArrayList(); }
            conditions.Add(conditionObj);
        }

        /// <summary>
        /// Creates a SQL statement based on the method, table, fields, 
        /// and conditions previously supplied
        /// </summary>
        /// <returns>A valid SQL statement or null</returns>
        public override string ToString()
        {
            switch(method)
            {
                case Method.SELECT:
                    return BuildQuery();
                case Method.DELETE:
                    return BuildDelete();
                case Method.UPDATE:
                    return BuildUpdate();
                case Method.INSERT:
                case Method.REPLACE:
                    return BuildInsert();
            }
            
            return null;
        }

        private string BuildQuery()
        {
            // Verify data
            if(table == null) { return null; }

            // If no field names are specified, assume all (*)
            if(fieldNames.Count == 0)
            {
                fieldNames.Add("*");
            }

            // Build string
            StringBuilder sb = new StringBuilder(method.ToString());
            sb.Append(" ");
            
            for(int i=0; i<fieldNames.Count; i++)
            {
                sb.Append(fieldNames[i]);
                
                if(i < (fieldNames.Count-1))
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" FROM ");
            sb.Append(table);

            if((where.Count + like.Count + conditions.Count) > 0)
            {
                sb.Append(" WHERE ");
                sb.Append(BuildWhereClause());
            }

            if (order.Count > 0)
            {
                sb.Append(" ORDER BY ");
                sb.Append(BuildOrderClause());
            }

            if(appendSemicolon)
            {
                sb.Append(";");
            }

            return sb.ToString();
        }

        private string BuildInsert()
        {
            // Verify data
            if(table == null) { return null; }
            if(fieldValues.Count == 0) { return null; }

            string methodStr = method.ToString();

            StringBuilder sb = new StringBuilder(methodStr);
            sb.Append(" INTO ");
            sb.Append(table);

            if(fieldNames.Count > 0)
            {
                if(fieldNames.Count != fieldValues.Count) { return null; }

                sb.Append(" (");

                for(int i=0; i<fieldNames.Count; i++)
                {
                    sb.Append(fieldNames[i]);
                    
                    if(i < (fieldNames.Count-1))
                    {
                        sb.Append(", ");
                    }
                }

                sb.Append(")");
            }

            sb.Append(" VALUES (");
            
            for(int i=0; i<fieldValues.Count; i++)
            {
                sb.Append(FormatValue(fieldValues[i]));

                if(i < (fieldValues.Count-1))
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");

            if(appendSemicolon)
            {
                sb.Append(";");
            }

            return sb.ToString();
        }

        private string BuildUpdate()
        {
            // Verify data
            if(table == null) { return null; }
            if(fieldNames.Count == 0) { return null; }
            if(fieldNames[0] == null) { return null; }
            if(fieldValues.Count == 0) { return null; }
            if(where.Count == 0 && like.Count == 0 && conditions.Count == 0) { return null; }


            if(fieldNames.Count != fieldValues.Count) { return null; }
            
            StringBuilder sb = new StringBuilder(method.ToString());
            sb.Append(" ");
            sb.Append(table);
            sb.Append(" SET ");

            for(int i=0; i<fieldNames.Count; i++)
            {
                sb.Append(fieldNames[i]);
                sb.Append(" = ");
                
                sb.Append(FormatValue(fieldValues[i]));
                
                if(i < (fieldNames.Count-1))
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" WHERE ");
            sb.Append(BuildWhereClause());
            
            if(appendSemicolon)
            {
                sb.Append(";");
            }

            return sb.ToString();
        }

        private string BuildDelete()
        {
            // Verify data
            if(table == null) { return null; }
          
            StringBuilder sb = new StringBuilder(method.ToString());
            sb.Append(" FROM ");
            sb.Append(table);

            if((where.Count + like.Count + conditions.Count) > 0)
            {
                sb.Append(" WHERE ");
                sb.Append(BuildWhereClause());
            }

            if(appendSemicolon)
            {
                sb.Append(";");
            }

            return sb.ToString();
        }

        private string BuildWhereClause()
        {
            StringBuilder sb = new StringBuilder();
            string returnString = null;
         
            foreach(DictionaryEntry de in where)
            {
                if(de.Key == null) { continue; }

                sb.Append(de.Key.ToString());
                if(de.Value == null)
                {
                    sb.Append(" IS "); 
                }
                else
                {
                    sb.Append("=");
                }
                sb.Append(FormatValue(de.Value));
                sb.Append(" AND ");
            }

            foreach (SqlCondition cond in conditions)
            {
                if (cond == null) { continue; }
                cond.ValueFormatter = new SqlCondition.Formatter(FormatValue);
                sb.Append(cond.ToString());
                sb.Append(" AND ");
            }

            foreach(DictionaryEntry de in like)
            {
                if(de.Key == null) { continue; }

                sb.Append(de.Key.ToString());
                sb.Append(" LIKE ");
                sb.Append(FormatValue(de.Value));
                sb.Append(" AND ");
            }

            returnString = sb.ToString();
            if (returnString != null)
            {
                if (returnString.EndsWith(" AND "))
                   returnString = returnString.Remove(sb.Length-5, 5); // Remove trailing AND
                else if (returnString.EndsWith(" LIKE "))
                   returnString = returnString.Remove(sb.Length-6, 6); // Remove trailing LIKE
            }

            return returnString;
        }

        private string BuildOrderClause()
        {
            if (order != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DictionaryEntry de in order)
                {
                    if (de.Key == null) { continue; }
                    sb.AppendFormat("{0} {1}, ", de.Key.ToString(), de.Value.ToString());
                }
                sb.Remove(sb.Length-2, 2);

                return sb.ToString();
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            appendSemicolon = true;
            method = Method.Unspecified;
            table = null;

            if(fieldNames != null) { fieldNames.Clear(); }
            else { fieldNames = new StringCollection(); }

            if(fieldValues != null) { fieldValues.Clear(); }
            else { fieldValues = new ArrayList(); }

            if (conditions != null) { conditions.Clear(); }
            else { conditions = new ArrayList(); }

            if(where != null) { where.Clear(); }
            else { where = new Hashtable(); }

            if (order != null) { order.Clear(); }
            else { order = new Hashtable(); }

            if(like != null) { like.Clear(); }
            else { like = new Hashtable(); }
        }

        private string FormatValue(object Value)
        {
            StringBuilder sb = new StringBuilder();

            if((Value == null) || (Value == Convert.DBNull))
            {
                sb.Append("NULL");  // Warning: Convert.DbNull.ToString() == ""
            } 
            else if(Value is string)
            {
                string valStr = Value as String;

                // Escape illegal characters
                valStr = valStr.Replace("\\", "\\\\");
                valStr = valStr.Replace("'", "\\'");
                valStr = valStr.Replace("\"", "\\\"");

                sb.Append("'");
                sb.Append(valStr);
                sb.Append("'");
            }
            else if(Value.GetType().IsEnum)
            {
                // Convert enums to ints
                sb.Append( ((int)Value).ToString() );
            }
            else if(Value is bool)
            {
                sb.Append( ((bool)Value) == true ? "1" : "0");
            }
            else if(Value is DateTime)
            {
                sb.Append("STR_TO_DATE('");
                sb.Append(((DateTime)Value).ToString("yyyy-MM-dd HH.mm.ss"));
                sb.Append("', GET_FORMAT(DATETIME, 'USA'))");
            }
            else if(Value is TimeSpan)
            {
                long seconds = Convert.ToInt64(((TimeSpan)Value).TotalSeconds);
                sb.Append("SEC_TO_TIME('");
                sb.Append(seconds.ToString());
                sb.Append("')");
            }
            else if(Value is SqlBuilder)
            {
                SqlBuilder innerBuilder = (SqlBuilder) Value;
                innerBuilder.appendSemicolon = false;
                sb.Append("(");
                sb.Append(Value.ToString());
                sb.Append(")");
            }
            else
            {
                sb.Append(Value.ToString());
            }

            return sb.ToString();
        }

        [Serializable]
        public class PreformattedValue
        {
            public string preformattedText;

            /// <summary>
            ///     The SqlBuilder will not format this text in anyway; rather, will
            ///     allow it to pass directly through untouched
            /// </summary>
            /// <param name="preformattedText"> The preformatted text to insert </param>
            public PreformattedValue(string preformattedText)
            {
                this.preformattedText = preformattedText;
            }

            public override string ToString()
            {
                return preformattedText;
            }
        }
	}
}

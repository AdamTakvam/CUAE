using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// Interpretation of TableBuilder for MySQL
    /// </summary>
    public class MysqlTableBuilder : IDbTableBuilder
    {
        public MysqlTableBuilder(string name)
            : base(name)
        {}

        public override string ToString()
        {
            if (this.Columns.Count == 0) { return null; }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("CREATE TABLE `{0}` (", this.name);

            // columns
            foreach (MysqlColumnBuilder col in this.Columns)
            {
                sb.AppendFormat(" {0},", col.ToString());
            }

            if (this.primaryKey != null)
            {
                sb.AppendFormat(" PRIMARY KEY ({0}),", this.primaryKey);
            }

            // keys
            foreach (KeyValuePair<string,string> kv in this.Keys)
            {
                sb.AppendFormat(" KEY `{0}` (`{1}`),", kv.Key, kv.Value);
            }

            // unique keys
            foreach (KeyValuePair<string,string> kv in this.UniqueKeys)
            {
                sb.AppendFormat(" UNIQUE KEY `{0}` (`{1}`),", kv.Key, kv.Value);
            }

            // foreign keys
            foreach (ForeignKey fk in this.foreignKeys)
            {
                sb.AppendFormat(" CONSTRAINT `{0}` FOREIGN KEY (`{1}`) REFERENCES `{2}` (`{3}`),",
                    fk.indexName, fk.thisTableCol, fk.refTableName, fk.refTableCol);
            }

            // remove last comma
            sb.Remove(sb.Length-1, 1);

            sb.Append(") ENGINE=InnoDB DEFAULT CHARSET=utf8;");
            return sb.ToString();
        }
    }
}

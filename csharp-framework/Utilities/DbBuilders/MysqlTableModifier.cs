using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// Interpretation of TableModifier for MySQL
    /// </summary>
    public class MysqlTableModifier : IDbTableModifier
    {

        public MysqlTableModifier(string name):
            base(name) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (this.drop)
            {
                return sb.AppendFormat("DROP TABLE `{0}`", this.name).ToString();
            }

            sb.AppendFormat("ALTER TABLE `{0}`", this.name);

            // Columns
            foreach (IDbColumnBuilder ac in this.addColumns)
            {
                sb.AppendFormat(" ADD COLUMN {0},", ac.ToString());
            }
            foreach (KeyValuePair<string, IDbColumnBuilder> cc in this.changeColumns)
            {
                sb.AppendFormat(" CHANGE COLUMN `{0}` {1},", cc.Key, cc.Value.ToString());
            }
            foreach (string dc in this.dropColumns)
            {
                sb.AppendFormat(" DROP COLUMN `{0}`,", dc);
            }

            // Drop Keys
            foreach (string dfk in this.dropForeignKeys)
            {
                sb.AppendFormat(" DROP FOREIGN KEY `{0}`,", dfk);
            }
            foreach (string dk in this.dropKeys)
            {
                sb.AppendFormat(" DROP KEY `{0}`,", dk);
            }

            // Add Keys
            foreach (KeyValuePair<string, string> ak in this.addKeys)
            {
                sb.AppendFormat(" ADD KEY `{0}` (`{1}`),", ak.Key, ak.Value);
            }
            foreach (KeyValuePair<string, string> auk in this.addUniqueKeys)
            {
                sb.AppendFormat(" ADD UNIQUE KEY `{0}` (`{1}`),", auk.Key, auk.Value);
            }
            foreach (ForeignKey afk in this.addForeignKeys)
            {
                sb.AppendFormat(" ADD CONSTRAINT `{0}` FOREIGN KEY (`{1}`) REFERENCES `{2}` (`{3}`),",
                    afk.indexName, afk.thisTableCol, afk.refTableName, afk.refTableCol);
            }

            // remove last comma
            sb.Remove(sb.Length-1, 1);
            sb.Append(";");
            return sb.ToString();
        }

    }
}

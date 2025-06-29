using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// Interpretation of ColumnBuilder for MySQL
    /// </summary>
    public class MysqlColumnBuilder : IDbColumnBuilder
    {
        
        public MysqlColumnBuilder(string name, ColType coltype)
            : base(name, coltype)
        { }

        public override string ToString()
        {

            bool zerofill = false;
            bool unsigned = false;
            bool notnull = false;
            bool auto_inc = false;

            foreach (ColModifier mod in this.modifiers)
            {
                switch (mod)
                {
                    case ColModifier.AUTOINCREMENT:
                        auto_inc = true;
                        break;
                    case ColModifier.UNSIGNED:
                        unsigned = true;
                        break;
                    case ColModifier.NOTNULL:
                        notnull = true;
                        break;
                    case ColModifier.ZEROFILL:
                        zerofill = true;
                        break;
                    default:
                        break;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("`{0}` {1}", this.name, this.colType.ToString());
            if (this.length > 0)
                sb.AppendFormat("({0})", this.length.ToString());

            if (unsigned)
                sb.Append(" UNSIGNED");
            if (zerofill)
                sb.Append(" ZEROFILL");
            if (notnull)
                sb.Append(" NOT NULL");

            if (this.colDefault != null)
                sb.AppendFormat(" DEFAULT '{0}'", this.colDefault);

            if (auto_inc)
                sb.Append(" AUTO_INCREMENT");

            if (this.relativePosition == ColPosition.AFTER)
            {
                sb.AppendFormat(" AFTER {0}", this.relativeToColumn);
            }
            else if (this.relativePosition == ColPosition.FIRST)
            {
                sb.Append(" FIRST");
            }

            return sb.ToString();
        }

    }
}

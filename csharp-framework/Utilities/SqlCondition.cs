using System;
using System.Text;
using System.Collections;

namespace Metreos.Utilities
{
    /// <summary>
    /// I'll comment later.  Honest.
    /// </summary>
    public abstract class SqlCondition
    {

        public delegate string Formatter(object x);
        public Formatter ValueFormatter;
        
        protected bool negate;

        protected SqlCondition()
        {
            this.negate = false;
            this.ValueFormatter = new Formatter(this.DefaultFormatter);
        }

        public SqlCondition Not()
        {
            this.negate = true;
            return this;
        }

        protected string AddNot(string sql)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("NOT({0})", sql);
            return sb.ToString();
        }

        private string DefaultFormatter(object x)
        {
            return x.ToString();
        }

    }


    public class SqlCompareCondition : SqlCondition
    {

        public enum Comparator
        {
            EQUAL,
            NOTEQUAL,
            LT,
            GT,
            LTE,
            GTE
        }

        private string name;
        private object value;
        private Comparator op;

        public SqlCompareCondition(string name, Comparator op, object value)
            : base()
        {
            this.name = name;
            this.op = op;
            this.value = value;
        }

        public override string ToString()
        {
            string opString;
            switch (this.op)
            {
                case Comparator.EQUAL :
                    opString = "=";
                    break;
                case Comparator.NOTEQUAL :
                    opString = "<>";
                    break;
                case Comparator.GT :
                    opString = ">";
                    break;
                case Comparator.GTE :
                    opString = ">=";
                    break;
                case Comparator.LT :
                    opString = "<";
                    break;
                case Comparator.LTE :
                    opString = "<=";
                    break;
                default:
                    throw new Exception("A SQL comparative operator was undefined");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2}", this.name, opString, this.ValueFormatter(value));

            if (this.negate)
                return this.AddNot(sb.ToString());
            else
                return sb.ToString();
        } 
    }


    public class SqlBetweenCondition : SqlCondition
    {
        private string name;
        private object min;
        private object max;

        public SqlBetweenCondition(string name, object min, object max)
            : base()
        {
            this.name = name;
            this.min = min;
            this.max = max;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} BETWEEN {1} AND {2}", 
                this.name, this.ValueFormatter(this.min), this.ValueFormatter(this.max));

            if (this.negate)
                return this.AddNot(sb.ToString());
            else
                return sb.ToString();
        }
    }


    public class SqlLogicalCondition : SqlCondition
    {
        public enum LogicalOp
        {
            AND,
            OR
        }

        private SqlCondition lh;
        private SqlCondition rh;
        private LogicalOp op;

        public SqlLogicalCondition(LogicalOp op, SqlCondition x, SqlCondition y)
            : base()
        {
            this.lh = x;
            this.rh = y;
            this.op = op;
        }

        public override string ToString()
        {
            this.lh.ValueFormatter = this.ValueFormatter;
            this.rh.ValueFormatter = this.ValueFormatter;

            string template = null;
            if (this.op == LogicalOp.OR)
                template = "({0} OR {1})";
            else
                template = "{0} AND {1}";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(template, this.lh.ToString(), this.rh.ToString());

            if (this.negate)
                return this.AddNot(sb.ToString());
            else
                return sb.ToString();
        }
    }

    public class SqlInCondition: SqlCondition
    {
        private string name;
        private ArrayList set;

        public SqlInCondition(string name)
            : base()
        {
            this.name = name;
            this.set = new ArrayList();
        }

        public SqlInCondition Add(string value)
        {
            this.set.Add(value);
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} IN (", this.name);
            foreach (string value in set)
            {
                sb.AppendFormat("{0},", this.ValueFormatter(value));
            }
            sb.Remove(sb.Length-1, 1);
            sb.Append(")");

            if (this.negate)
                return this.AddNot(sb.ToString());
            else
                return sb.ToString();
        }
    }


}

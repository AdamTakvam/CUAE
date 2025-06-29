using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// I'll comment later.  Honest.
    /// </summary>
    public abstract class IDbTableModifier
    {

        #region Internal Structs

        protected struct ForeignKey
        {
            public string indexName;
            public string thisTableCol;
            public string refTableName;
            public string refTableCol;
        }

        #endregion


        #region Members

        protected string name;
        protected bool drop;

        protected List<IDbColumnBuilder> addColumns;
        protected Dictionary<string, IDbColumnBuilder> changeColumns;
        protected List<string> dropColumns;

        protected Dictionary<string, string> addKeys;
        protected List<string> dropKeys;

        protected Dictionary<string, string> addUniqueKeys;
        protected List<string> dropUniqueKeys;

        protected List<ForeignKey> addForeignKeys;
        protected List<string> dropForeignKeys;

        #endregion


        #region Methods

        protected IDbTableModifier(string name)
        {
            this.name = name;
            this.drop = false;

            this.addColumns = new List<IDbColumnBuilder>();
            this.changeColumns = new Dictionary<string, IDbColumnBuilder>();
            this.dropColumns = new List<string>();

            this.addKeys = new Dictionary<string, string>();
            this.addUniqueKeys = new Dictionary<string,string>();
            this.addForeignKeys = new List<ForeignKey>();

            this.dropKeys = new List<string>();
            this.dropForeignKeys = new List<string>();
        }

        public IDbTableModifier Drop()
        {
            this.drop = true;
            return this;
        }

        public IDbTableModifier AddColumn(IDbColumnBuilder col)
        {
            this.addColumns.Add(col);
            return this;
        }

        public IDbTableModifier ChangeColumn(string name, IDbColumnBuilder col)
        {
            this.changeColumns.Add(name, col);
            return this;
        }

        public IDbTableModifier DropColumn(string name)
        {
            this.dropColumns.Add(name);
            return this;
        }

        public IDbTableModifier AddKey(string index_name, string column)
        {
            this.addKeys.Add(index_name, column);
            return this;
        }

        public IDbTableModifier AddUniqueKey(string index_name, string column)
        {
            this.addUniqueKeys.Add(index_name, column);
            return this;
        }

        public IDbTableModifier AddForeignKey(string index_name, string this_column, string ref_table, string ref_column)
        {
            ForeignKey fk = new ForeignKey();
            fk.indexName = index_name;
            fk.thisTableCol = this_column;
            fk.refTableName = ref_table;
            fk.refTableCol = ref_column;
            this.addForeignKeys.Add(fk);
            this.addKeys.Add(index_name, this_column);
            return this;
        }

        public IDbTableModifier DropKey(string index_name)
        {
            this.dropKeys.Add(index_name);
            return this;
        }

        public IDbTableModifier DropForeignKey(string index_name)
        {
            this.dropForeignKeys.Add(index_name);
            this.dropKeys.Add(index_name);
            return this;
        }

        abstract public override string ToString();

        #endregion

    }

}

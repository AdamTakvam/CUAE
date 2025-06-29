using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// I'll comment later.  Honest.
    /// </summary>
    public abstract class IDbTableBuilder
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
        protected string primaryKey;
        public List<IDbColumnBuilder> Columns;
        public Dictionary<string, string> Keys;
        public Dictionary<string, string> UniqueKeys;
        protected List<ForeignKey> foreignKeys;

        #endregion


        #region Properties

        public string PrimaryKey
        {
            set
            {
                this.primaryKey = value;
            }
        }

        #endregion


        #region Methods

        protected IDbTableBuilder(string name)
        {
            this.name = name;
            this.primaryKey = null;
            this.Columns = new List<IDbColumnBuilder>();
            this.Keys = new Dictionary<string,string>();
            this.UniqueKeys = new Dictionary<string, string>();
            this.foreignKeys = new List<ForeignKey>();
        }

        public void AddForeignKey(string index_name, string column, string reference_table, string reference_column)
        {
            ForeignKey fk = new ForeignKey();
            fk.indexName = index_name;
            fk.thisTableCol = column;
            fk.refTableName = reference_table;
            fk.refTableCol = reference_column;
            this.foreignKeys.Add(fk);
            // Any foreign key must reference a regular key to the table, so let's add one.
            this.Keys.Add(index_name, column);
        }

        public abstract override string ToString();

        #endregion
    }
}

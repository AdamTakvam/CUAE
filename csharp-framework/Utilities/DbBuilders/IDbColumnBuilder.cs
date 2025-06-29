using System;
using System.Collections.Generic;
using System.Text;

namespace Metreos.Utilities.DbBuilders
{
    /// <summary>
    /// I'll comment later.  Honest.
    /// </summary>
    public abstract class IDbColumnBuilder
    {

        #region Members

        protected string name;
        protected ColType colType;
        protected int length;
        protected string colDefault;
        protected List<ColModifier> modifiers;
        protected ColPosition relativePosition;
        protected string relativeToColumn;

        #endregion


        protected IDbColumnBuilder(string name, ColType type)
        {
            this.name = name;
            this.colType = type;
            this.length = 0;
            this.colDefault = null;
            this.modifiers = new List<ColModifier>();
            this.relativePosition = ColPosition.Unspecified;
            this.relativeToColumn = null;
        }

        public IDbColumnBuilder SetLength(int length)
        {
            this.length = length;
            return this;
        }

        public IDbColumnBuilder SetDefault(string value)
        {
            this.colDefault = value;
            return this;
        }

        public IDbColumnBuilder PlaceFirst()
        {
            this.relativePosition = ColPosition.FIRST;
            return this;
        }

        public IDbColumnBuilder PlaceAfterColumn(string column)
        {
            this.relativePosition = ColPosition.AFTER;
            this.relativeToColumn = column;
            return this;
        }

        public IDbColumnBuilder AddModifier(ColModifier mod)
        {
            this.modifiers.Add(mod);
            return this;
        }

        public abstract override string ToString();

    }
}

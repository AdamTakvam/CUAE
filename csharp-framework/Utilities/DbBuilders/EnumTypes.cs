using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Some enumerated values to use for the ColumnBuilders for defining types and attributes
/// </summary>

namespace Metreos.Utilities.DbBuilders
{

    public enum ColType
    {
        TINYINT,
        INT,
        BIGINT,
        FLOAT,
        DECIMAL,
        CHAR,
        VARCHAR,
        BINARY,
        VARBINARY,
        BLOB,
        TEXT,
        DATE,
        DATETIME,
        TIMESTAMP,
        TIME
        /*  ENUM
         *  YEAR
         *  SET
         */
    }

    public enum ColModifier
    {
        NOTNULL,
        UNSIGNED,
        ZEROFILL,
        AUTOINCREMENT
    }

    public enum ColPosition
    {
        Unspecified,
        FIRST,
        AFTER
    }

}

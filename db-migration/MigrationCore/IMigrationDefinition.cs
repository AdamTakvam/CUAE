using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MigrationCore
{
    interface IMigrationDefinition
    {
        string CurrentVersion
        {
            get;
        }
        string NextVersion
        {
            get;
        }
        IDbTransaction Transaction
        {
            set;
        }

        bool IsReady();
        bool DoUpgrade();
        bool DoRollback();
    }
}

using System;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Package = Metreos.Interfaces.PackageDefinitions.DatabaseTypes.Types.SqlStatement;

namespace Metreos.Types.Database
{
    [Serializable]
    [TypeDecl(Package.DISPLAY, Package.DESCRIPTION, false)]
    public class SqlStatement : IVariable
    {
        private SqlBuilder _value;

        public bool AppendSemiColon { get { return _value.appendSemicolon; } 
                                      set { _value.appendSemicolon = value; } }
        public SqlBuilder.Method Method { set { _value.method = value; } }
        public string Table { set { _value.table = value; } }
        public StringCollection FieldNames { get { return _value.fieldNames; } }
        public ArrayList fieldValues { get { return _value.fieldValues; } }
        public Hashtable Where { get { return _value.where; } }

        public SqlStatement()
        {
            Reset();
        }

        #region IVariable Members

        [TypeInput("String", Package.CustomMethods.Parse_String.DESCRIPTION)]        
        public bool Parse(string str)
        {
            // Not supported
            return true;
        }
                
        [TypeMethod(Package.CustomMethods.Reset.DESCRIPTION)]        
        public void Reset()
        {
            _value = new SqlBuilder();
        }

        #endregion

        [TypeMethod(Package.CustomMethods.AddFieldValue_String.DESCRIPTION)]        
        public void AddFieldValue(string field, object Value)
        {
            _value.AddFieldValue(field, Value);
        }

        [TypeMethod(Package.CustomMethods.ToString.DESCRIPTION)]        
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}

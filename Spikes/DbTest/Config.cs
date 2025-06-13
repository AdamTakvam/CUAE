using System;
using System.Data;

namespace DbTest
{
	public class Config : MarshalByRefObject, IConfigUtility
	{
        private IDbConnection db;

		public Config(IDbConnection database)
		{
            db = database;
		}
	}
}

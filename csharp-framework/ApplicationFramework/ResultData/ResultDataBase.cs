using System;

namespace Metreos.ApplicationFramework.ResultData
{
	public abstract class ResultDataBase
	{
		public ResultDataBase() {}

        public abstract ResultDataBase Clone();

        public abstract void Clear();
	}
}

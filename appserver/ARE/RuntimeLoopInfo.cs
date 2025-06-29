using System;
using System.Collections;

using Metreos.ApplicationFramework.Loops;

namespace Metreos.AppServer.ARE
{
	public class RuntimeLoopInfo
	{
        public string id;
        public Loop loop;
        public int loopLimit;
        public int loopIndex;
        public IEnumerator loopEnum;
        public IDictionaryEnumerator loopDictEnum;

		public RuntimeLoopInfo()
		{
            id = null;
            loop = null;
            loopIndex = -1;
            loopEnum = null;
            loopDictEnum = null;
		}
	}
}

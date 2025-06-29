using System;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>Holds metadata about a call route group member</summary>
	[Serializable]
	public abstract class CrgMember
	{
        private readonly ComponentInfo cInfo;
        public ComponentInfo ComponentInfo { get { return cInfo; } }

		public CrgMember(ComponentInfo cInfo)
		{
	        this.cInfo = cInfo;
		}

        public abstract IConfig.ComponentType GetComponentType();
	}
}

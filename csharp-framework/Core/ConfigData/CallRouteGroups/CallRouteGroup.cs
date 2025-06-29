using System;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>A group of CTI devices or servers</summary>
	[Serializable]
	public sealed class CallRouteGroup
	{
        /// <summary>Ordered collection of group members</summary>
        private readonly CrgMember[] members;
        private int currServerIndex = -1;

		internal CallRouteGroup(CrgMember[] members)
		{
            this.members = members;
		}

        /// <summary>Gets the next member of the CRG</summary>
        /// <returns>CrgMember or null</returns>
        public CrgMember GetNextMember()
        {
            currServerIndex++;
            return GetCurrentMember();
        }

        /// <summary>Returns the last member returned by GetNextMember()</summary>
        /// <returns>CrgMember or null</returns>
        public CrgMember GetCurrentMember()
        {
            if(currServerIndex >= 0 && currServerIndex < members.Length)
                return members[currServerIndex];
            else
                return null;
        }
	}
}

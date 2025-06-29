using System;

namespace Metreos.Utilities
{
	/// <summary>Parses action GUIDs into runtime GUIDs and action IDs</summary>
	public abstract class ActionGuid
	{
        /// <summary>Determines whether the input string is a valid action GUID</summary>
        /// <param name="guid">The string in question</param>
        /// <returns>Whether or not it is valid</returns>
        public static bool IsValid(string guid)
        {
            if(guid == null)
            {
                return false;
            }

            string[] bits = guid.Split('.');
            if(bits.Length == 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>Extracts the action ID from an action GUID</summary>
        /// <param name="guid">An action GUID</param>
        /// <returns>The action ID</returns>
        public static string GetActionId(string guid)
        {
            if(guid == null)
            {
                return null;
            }

            string[] str = guid.Split(new char[] {'.'}, 2);

            if(str.Length != 2)
            {
                return null;
            }
            return str[1];
        }

        /// <summary>Extracts the routing GUID from an action GUID</summary>
        /// <param name="guid">An action GUID</param>
        /// <returns>The routing GUID</returns>
        public static string GetRoutingGuid(string guid)
        {
            if(guid == null)
            {
                return null;
            }

            string[] str = guid.Split(new char[] {'.'});

            if(str.Length != 2)
            {
                return null;
            }
            return str[0];
        }

        /// <summary>Creates an action GUID</summary>
        /// <param name="routingGuid">A routing GUID</param>
        /// <param name="actionId">An action ID</param>
        /// <returns>An action GUID</returns>
        public static string Create(string routingGuid, string actionId)
        {
            if((routingGuid != null) && (actionId != null))
            {
                string actionGuid = routingGuid + "." + actionId;
                if(ActionGuid.IsValid(actionGuid))
                {
                    return actionGuid;
                }
                return null;
            }
            return null;
        }
	}
}

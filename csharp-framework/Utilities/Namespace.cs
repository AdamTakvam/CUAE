using System;

namespace Metreos.Utilities
{
	/// <summary>
	/// Utility methods to parse namespaces into usable
	/// components.
	/// </summary>
	public abstract class Namespace
	{
        /// <summary>
        /// Retrieve the name component from the fully qualified
        /// namespace value.
        /// </summary>
        /// <example>
        /// Given a namespace: "This.Is.A.Namespace.Name", this will
        /// return "Name".
        /// </example>
        /// <param name="fullName">The complete namespace name.</param>
        /// <returns>
        /// A string containing the "name" component, the last
        /// value after the namespace. If fullName is null, an
        /// empty string is returned.
        /// </returns>
        public static string GetName(string fullName)
        {
            if(fullName == null)
            {
                return "";
            }

            string[] bits = fullName.Split(new char[] {'.'});

            return bits.Length > 0 ? bits[bits.Length-1] : "";
        }

        /// <summary>
        /// Retrieve the namespace component from a fully qualified
        /// namespace.
        /// </summary>
        /// <param name="fullName">The complete namespace name.</param>
        /// <returns>
        /// A string containing the namespace component. If fullName is null,
        /// an empty string is returned.
        /// </returns>
        public static string GetNamespace(string fullName)
        {
            if(fullName == null)
            {
                return "";
            }

            string nSpace = "";
            string[] bits = fullName.Split(new char[] {'.'});

            for(int i = 0; i < (bits.Length-1); i++)
            {
                if(i < (bits.Length-2))
                {
                    nSpace += bits[i] + ".";
                }
                else
                {
                    nSpace += bits[i];
                }
            }

            bits = null;

            return nSpace;
        }
	}
}

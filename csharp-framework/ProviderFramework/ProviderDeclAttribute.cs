using System;

namespace Metreos.ProviderFramework
{
    /// <summary>
    /// Attribute to be used to declare a class that implements 
    /// ProviderBase and thus IProvider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
	public class ProviderDeclAttribute : System.Attribute
	{
        private string name;

		public ProviderDeclAttribute(string name)
		{
            this.name = name;
		}

        public string Name
        {
            get { return name; }
        }
	}
}

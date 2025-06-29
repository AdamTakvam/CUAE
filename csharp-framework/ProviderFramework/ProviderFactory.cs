using System;
using System.Reflection;

using Metreos.Utilities;

namespace Metreos.ProviderFramework
{
    public class ProviderFactory : MarshalByRefObject
    {
        public ProviderFactory()
        { 
        }

        /// <summary>Instantiates a protocol provider from the specified assembly file</summary>
        /// <param name="appDomain">New AppDomain for provider</param>
        /// <param name="assemblyFile">File in which the provider is defined</param>
        /// <param name="ccpArgs">Arguments to pass to constructor of call control providers</param>
        /// <param name="args">Arguments to pass to constructor of generic providers</param>
        /// <returns>Provider interface</returns>
        public IProvider Create(AssemblyName assemblyName, object[] ccpArgs, object[] args)
        {
            Assembly providerAssembly = Assembly.Load(assemblyName);

            // NOTE: This will only load a single provider from the assembly, and it will be the
            // first class encountered that has the ProviderDeclAttribute.
            string typeName = GetProviderTypeName(providerAssembly);
            if(typeName == null)
                throw new CreateProviderException(CreateProviderException.NO_AVAILABLE_PROVIDERS);

            if(providerAssembly.GetType(typeName, true).IsSubclassOf(typeof(CallControlProviderBase)))
            {
                return (IProvider) Activator.CreateInstanceFrom(assemblyName.CodeBase, typeName, false,
                    BindingFlags.CreateInstance, null, ccpArgs, null, null, null).Unwrap();
            }
            else
            {
                return (IProvider) Activator.CreateInstanceFrom(assemblyName.CodeBase, typeName, false,
                    BindingFlags.CreateInstance, null, args, null, null, null).Unwrap();
            }
        }

        /// <summary>
        /// Search an assembly for valid provider implementations.
        /// </summary>
        /// <param name="a">The assembly to search.</param>
        /// <param name="typeNames">(out) The type names of valid provider implementations.</param>
        /// <returns></returns>
        private string GetProviderTypeName(Assembly a)
        {
            Assertion.Check(a != null, "GetProviderTypeName: Assembly is null");

            foreach(System.Type t in a.GetTypes())
            {
                if(t.IsClass == true)
                {
                    foreach(System.Attribute attr in t.GetCustomAttributes(false))
                    {
                        if(attr is ProviderDeclAttribute)
                        {
                            return t.FullName;
                        }
                    }
                }
            }

            return null;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }   
    }
}

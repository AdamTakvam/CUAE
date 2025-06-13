using System;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;

namespace Metreos.MediaControl
{
    /// <summary>Simple container class for media server resources.</summary>
    public sealed class MediaServerResources
    {
        public int ipResInstalled   = 0;
        public int ipResAvail       = 0;
        public int voxResInstalled  = 0;
        public int voxResAvail      = 0;
        public int lbrResInstalled  = 0;
        public int lbrResAvail      = 0;
        public int confResInstalled = 0;
        public int confResAvail     = 0;

        public MediaServerResources()
        {}

        public MediaServerResources(string[] resources)
        {
            LoadResources(resources);
        }

        /// <summary>Sets all resource values to 0.</summary>
        public void Zero()
        {
            ipResInstalled      = 0;
            ipResAvail          = 0;
            voxResInstalled     = 0;
            voxResAvail         = 0;
            confResInstalled    = 0;
            confResAvail        = 0;
        }

        /// <summary>
        /// Loads resources from a given string array.
        /// </summary>
        /// <remarks>
        /// Each element in the string array must be of this format:
        ///   "[resourceName] [resourceValue]"
        ///   
        /// Name and value must NOT have any spaces in them.
        /// </remarks>
        /// <param name="resources">The array of resources to load from.</param>
        public void LoadResources(string[] resources)
        {
            Assertion.Check(resources != null, "MMS resources is null");

            string[] resourceBits;
            string resource;
            int resourceValue = 0;

            foreach(string s in resources)
            {
                try
                {
                    resourceBits = s.Split(new char[]{' '}, 2);

                    Assertion.Check(resourceBits[0] != null, "MMS resources data is malformed");
                    Assertion.Check(resourceBits[1] != null, "MMS resources data is malformed");

                    resource = resourceBits[0];
                    resourceValue = int.Parse(resourceBits[1]);

                    UpdateResourceValue(resource, resourceValue);
                }
                catch(Exception)
                {}
            }

            resourceBits = null;
            resource = null;
        }

        
        /// <summary>
        /// Updates an individual resource value.
        /// </summary>
        /// <param name="resource">The name of the resource value to update.</param>
        /// <param name="resourceValue">The new value of the resource.</param>
        private void UpdateResourceValue(string resource, int resourceValue)
        {
            switch(resource)
            {
                case IMediaServer.Stats.IPResInstalled:
                    this.ipResInstalled = resourceValue;
                    break;
                case IMediaServer.Stats.IPResAvailable:
                    this.ipResAvail = resourceValue;
                    break;
                case IMediaServer.Stats.VoiceResInstalled:
                    this.voxResInstalled = resourceValue;
                    break;
                case IMediaServer.Stats.VoiceResAvailable:
                    this.voxResAvail = resourceValue;
                    break;
                case IMediaServer.Stats.ConfResInstalled:
                    this.confResInstalled = resourceValue;
                    break;
                case IMediaServer.Stats.ConfResAvailable:
                    this.confResAvail = resourceValue;
                    break;
                case IMediaServer.Stats.LbrResInstalled:
					this.lbrResInstalled = resourceValue;
					break;
                case IMediaServer.Stats.LbrResAvailable:
					this.lbrResAvail = resourceValue;
					break;
            }
        }
    }
}

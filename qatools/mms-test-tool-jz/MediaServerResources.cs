using System;
using System.Diagnostics;



namespace Metreos.MMSTestTool
{
	/// <summary>
	/// Simple container class for media server resources.
	/// </summary>
	public sealed class MediaServerResources
	{
		public int ipResInstalled   = 0;
		public int ipResAvail       = 0;
		public int voxResInstalled  = 0;
		public int voxResAvail      = 0;
		public int confResInstalled = 0;
		public int confResAvail     = 0;

		public MediaServerResources()
		{}


		public MediaServerResources(string[] resources)
		{
			LoadResources(resources);
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
			Debug.Assert(resources != null);

			string[] resourceBits;
			string resource;
			int resourceValue = 0;

			foreach(string s in resources)
			{
				try
				{
					resourceBits = s.Split(new char[]{' '}, 2);

					Debug.Assert(resourceBits[0] != null);
					Debug.Assert(resourceBits[1] != null);

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
				case IMediaServer.MS_STAT_IP_RES_INSTALLED:
					this.ipResInstalled = resourceValue;
					break;

				case IMediaServer.MS_STAT_IP_RES_AVAIL:
					this.ipResAvail = resourceValue;
					break;

				case IMediaServer.MS_STAT_VOICE_RES_INSTALLED:
					this.voxResInstalled = resourceValue;
					break;

				case IMediaServer.MS_STAT_VOICE_RES_AVAIL:
					this.voxResAvail = resourceValue;
					break;

				case IMediaServer.MS_STAT_CONF_RES_INSTALLED:
					this.confResInstalled = resourceValue;
					break;

				case IMediaServer.MS_STAT_CONF_RES_AVAIL:
					this.confResAvail = resourceValue;
					break;
			}
		}
	}
}

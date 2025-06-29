using System;
using System.Globalization;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
	/// <summary>Metadata about an application partition</summary>
	[Serializable]
	public class AppPartitionInfo
	{
        public string Name { get { return name; } }
        private readonly string name;

        public DateTime Created { get { return created; } }
        private readonly DateTime created;

        public string Description { get { return description; } }
        private readonly string description;

        public bool Enabled { get { return enabled; } }
        private readonly bool enabled;

        public CultureInfo Culture { get { return culture; } }
        private readonly CultureInfo culture;

        public bool EarlyMedia { get { return earlyMedia; } }
        public readonly bool earlyMedia;

        public IMediaControl.Codecs PreferredCodec { get { return preferredCodec; } }
        private readonly IMediaControl.Codecs preferredCodec;

        public uint PreferredFramesize { get { return preferredFramesize; } }
        private readonly uint preferredFramesize;

        public AppPartitionInfo(string name, DateTime created, string description, bool enabled, bool earlyMedia,
            CultureInfo culture, IMediaControl.Codecs preferredCodec, uint preferredFramesize)
		{
            this.name = name;
            this.created = created;
            this.description = description;
            this.enabled = enabled;
            this.earlyMedia = earlyMedia;
            this.culture = culture;
            this.preferredCodec = preferredCodec;
            this.preferredFramesize = preferredFramesize;
		}
	}
}

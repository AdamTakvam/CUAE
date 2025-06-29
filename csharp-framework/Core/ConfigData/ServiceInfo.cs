using System;

namespace Metreos.Core.ConfigData
{
    /// <summary>Container for metadata about USE services</summary>
    public class ServiceInfo
    {
        public enum State
        {
            Unknown,
            Continuing,
            Stopped,
            Stopping,
            Paused,
            Pausing,
            Starting,
            Started
        }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private string displayName;
        public string DisplayName { get { return displayName; } set { displayName = value; } }

        private State status;
        public State Status { get { return status; } set { status = value; } }

        private bool enabled;
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        private bool userStopped;
        public bool UserStopped { get { return userStopped; } set { userStopped = value; } }

        /// <summary>Default constructor</summary>
        /// <param name="name">Service name</param>
        /// <param name="displayName">Service display name</param>
        /// <param name="enabled">Is service enabled</param>
        /// <param name="userStopped">Is service manually stopped from mceadmin</param>
        public ServiceInfo(string name, string displayName, bool enabled, bool userStopped)
        {
            this.name = name;
            this.displayName = displayName;
            this.enabled = enabled;
            this.userStopped = userStopped;
            this.status = State.Unknown;
        }
    }
}


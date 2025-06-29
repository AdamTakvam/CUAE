using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Diagnostics;

using Metreos.Interfaces;
using Metreos.Core.ConfigData;

namespace Metreos.Core
{
    public interface IConfigUtility
    {
        // Properties
        string MessageQueueProvider { get; }
        string DatabaseName { get; }
        string DatabaseHost { get; }
        ushort DatabasePort { get; }
        string DatabaseUsername { get; }
        string DatabasePassword { get; }
		bool DeveloperMode { get; }
        Exception LastChildDomainException { set; }

        // Config Value Management
        object GetEntryValue(IConfig.ComponentType componentType, string componentName, string valueName);
		object GetEntryValue(IConfig.ComponentType componentType, string componentName, string valueName, string partitionName);
        ConfigEntry GetEntry(IConfig.ComponentType componentType, string componentName, string valueName);
		ConfigEntry GetEntry(IConfig.ComponentType componentType, string componentName, string valueName, string partitionName);
		IDictionary GetEntries(IConfig.ComponentType componentType, string componentName, string partitionName);
		bool AddEntry(IConfig.ComponentType type, string componentName, ConfigEntry cValue);
        bool AddEntry(IConfig.ComponentType type, string componentName, ConfigEntry cValue, bool overwrite);
        bool RemoveEntry(IConfig.ComponentType type, string componentName, string valueName, string partitionName);

        // Component Management
        bool AddComponent(ComponentInfo cInfo, string locale);
        ComponentInfo[] GetComponents(IConfig.ComponentType type);
        ComponentInfo GetComponentInfo(IConfig.ComponentType type, string name);
        bool RemoveComponent(IConfig.ComponentType type, string name);
        IConfig.Status GetStatus(IConfig.ComponentType type, string name);
        bool UpdateStatus(IConfig.ComponentType type, string name, IConfig.Status status);
        bool UpdateComponent(IConfig.ComponentType type, string name, string displayName, string version,
            string description, string author, string copyright);

        // Groups
        ComponentInfo[] GetCallRouteGroup(string appName, string partitionName);
        int GetCallRouteGroupType(string appName, string partitionName);  // returns IConfig.ComponentType
        ComponentInfo[] GetMediaResourceGroup(string appName, string partitionName);
        ComponentInfo[] GetFailoverMRG(string appName, string partitionName);

        // App partitions
        AppPartitionInfo GetPartitionInfo(string appName, string partitionName);

        // Line-oriented devices
        SipDeviceInfo[] GetSipDevices(ComponentInfo iptServerInfo);
        DeviceInfo[] GetSccpDevices(ComponentInfo iptServerInfo);
        DeviceInfo[] GetCtiDevices(ComponentInfo iptServerInfo);
        bool UpdateDeviceStatus(string deviceName, IConfig.DeviceType type, IConfig.Status status);
        bool SetDirectoryNumber(string deviceName, IConfig.DeviceType type, string dn);

        SipDomainInfo GetSipDomainInfo(string domain);

        // Provider Extensions
        bool AddExtension(string providerName, Extension ext);
        void RemoveExtensions(string providerName);

        // CCM Clusters
        CallManagerCluster[] GetCallManagerClusters();

        // Users
        IConfig.AccessLevel ValidateUser(string username, string password, string componentGroupName);

        // Utilities
        bool DatabaseExists(string name);
        void DatabaseDrop(string name);
        bool DatabaseCreate(string name);
        IDbConnection DatabaseConnect(string name);

        // Functional Test Framework helpers
        bool Test_CreateCallRouteGroup(string testname, string routeGroupName); 
        bool Test_CreateMediaResourceGroup(string testname, string mediaGroupName);
        bool Test_RemoveScriptTriggerParam(string appName, string scriptName, string partitionName, string paramName);
            // Not Functional Test Framework specific, but is added to this interface solely for FTF
        bool AddScriptTriggerParam(string appName, string scriptName, string partitionName, string paramName, object paramValue);
    }   
}

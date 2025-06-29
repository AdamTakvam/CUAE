using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

using Metreos.Interfaces;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>Handles the state of remoting with the Functional Test Provider.</summary>
	public class OamCommunicator
	{
        #region Singleton

        public static OamCommunicator Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new OamCommunicator();
                }

                return instance;
            }
        }

        private static OamCommunicator instance;

        private OamCommunicator()
	    {
    		
	    }

        #endregion

        public IManagement Server { get { return oamServer; } }

        private HttpChannel oamChannel;
        private IManagement oamServer;
        
        private CommonTypes.OutputLine outputLine;
        private Settings settings;

        /// <summary>Sets up the remoting channels and gets a reference
        ///  to the server object hosted by the Funct Test Provider</summary>
        public void Initialize(CommonTypes.OutputLine outputLine, Settings settings)
        {
            this.settings = settings;
            this.outputLine = outputLine;

            SortedList channelProps = new SortedList();
            channelProps["name"] = "RemotingInterface";

            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

            oamChannel = new HttpChannel(channelProps, clientProv, null);  

            if(ChannelServices.GetChannel(oamChannel.ChannelName) == null)
            {
                ChannelServices.RegisterChannel(oamChannel);
            }
                   
            string oamUrl = Utilities.GetOamRemotingUri(settings.SamoaIP, settings.SamoaPort, Constants.OamRemotingUrl);
            oamServer = Activator.GetObject(typeof(IManagement), oamUrl) as IManagement; 
        }

        public void Reconnect()
        {
            ChannelServices.UnregisterChannel(oamChannel);

            string url = "http://" + settings.SamoaIP+':' + settings.SamoaPort + "/RemotingInterface";
            
            SortedList channelProps = new SortedList();
            channelProps["name"] = "RemotingInterface";

            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

            oamChannel = new HttpChannel(channelProps, clientProv, null);  

            if(ChannelServices.GetChannel(oamChannel.ChannelName) == null)
            {
                ChannelServices.RegisterChannel(oamChannel);
            }

            oamServer = (IManagement) Activator.GetObject(typeof(IManagement), url); 
        }

        public void Cleanup()
        {
			if(ChannelServices.GetChannel(oamChannel.ChannelName) != null)
			{
				ChannelServices.UnregisterChannel(oamChannel);
			}
        }


	}
}

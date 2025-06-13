using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;

using Metreos.Common.Mec;
using Metreos.Core;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// The implementation of running Randomized Conference
	/// </summary>
    public abstract class TestBase	
    {
        #region VariableDeclarations
        private object numberOfLiveConnectionsLock;
        private object numberOfLiveConferencesLock;
        private object numberOfTotalConnectionsLock;
        private object numberOfTotalConferencesLock;
        private object numberOfTotalKicksLock;
        
        public int maximumNumberOfConnections;
        public int maximumNumberOfConferences;
        
        #endregion VariableDeclarations

        public TestBase()
        {

            numberOfLiveConnectionsLock = new object();
            numberOfLiveConferencesLock = new object();
            numberOfTotalConnectionsLock = new object();
            numberOfTotalConferencesLock = new object();
            numberOfTotalKicksLock = new object();


            maximumNumberOfConnections = 0;
            maximumNumberOfConferences = 0;
        }
        
        public virtual void Reset()
        {

        }
        public abstract bool Start();
        public abstract bool EndTest();    
    }
}

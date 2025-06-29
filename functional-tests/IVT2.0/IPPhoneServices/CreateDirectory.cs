using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendExecuteTest = Metreos.TestBank.IVT.IVT.SendExecute;
using CreateDirectoryTest = Metreos.TestBank.IVT.IVT.CreateDirectory;
using CreateTextTest = Metreos.TestBank.IVT.IVT.CreateText;

namespace Metreos.FunctionalTests.IVT2._0.IPPhoneServices
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class CreateDirectory : FunctionalTestBase
    {
        private const string url = "url";
        private const string ip = "ip";
        private const string username = "username";
        private const string password = "password";

        private bool sendExecuteSuccess;

        public CreateDirectory() : base(typeof( CreateDirectory ))
        {
            
        }

        public override void Initialize()
        {
            sendExecuteSuccess = false;
        }

        public override void Cleanup()
        {
            sendExecuteSuccess = false;
        }

        public override bool Execute()
        {  
            Hashtable args = new Hashtable();
            args[ip] = input[ip];
            args[username] = input[username];
            args[password] = input[password];
            args[url] = MakeUri("CreateDirectory");

                TriggerScript(SendExecuteTest.script1.FullName, args);

            if(!WaitForSignal( SendExecuteTest.script1.S_Signal.FullName, 10 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Never received response from the application for SendExecute completion");
                return false;
            }

            return sendExecuteSuccess;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData ipField = new TestTextInputData("Phone IP", 
                "The IP address of the phone.", ip, 80);
            TestTextInputData usernameField = new TestTextInputData("Username", 
                "The username of the user associated with the phone.", username, 80);
            TestTextInputData passwordField = new TestTextInputData("Password", 
                "The password of the user associated with the phon.", password, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            return inputs;
        }

        public void SendExecuteComplete(ActionMessage im)
        {
            sendExecuteSuccess = (bool) im["success"];

            if(!sendExecuteSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The SendExecute command failed");
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { SendExecuteTest.FullName, CreateDirectoryTest.FullName, CreateTextTest.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( SendExecuteTest.script1.S_Signal.FullName,
                                          new FunctionalTestSignalDelegate(SendExecuteComplete)) };
        }

        protected string MakeUri(string url)
        {
            return "http://" + settings.AppServerIps[0] + ":8000/" + url;
        }
    } 
}

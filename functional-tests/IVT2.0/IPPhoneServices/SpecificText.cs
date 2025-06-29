using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendExecuteTest = Metreos.TestBank.IVT.IVT.SendExecute;
using SpecificTextTest = Metreos.TestBank.IVT.IVT.SpecificText;
using CreateTextTest = Metreos.TestBank.IVT.IVT.CreateText;

namespace Metreos.FunctionalTests.IVT2._0.IPPhoneServices
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class SpecificText : FunctionalTestBase
    {
        private const string url = "url";
        private const string ip = "ip";
        private const string username = "username";
        private const string password = "password";
        private const string title = "title";
        private const string prompt = "prompt";
        private const string text = "text";

        private bool receiveOneMessage;
        private bool sendExecuteSuccess;
        private string routingGuid;

        public SpecificText() : base(typeof( SpecificText ))
        {
            
        }

        public override void Initialize()
        {
            sendExecuteSuccess = false;
            receiveOneMessage = false;
        }

        public override void Cleanup()
        {
            sendExecuteSuccess = false;
            receiveOneMessage = false;
        }

        public override bool Execute()
        {  
            Hashtable args = new Hashtable();
            args[ip] = input[ip];
            args[username] = input[username];
            args[password] = input[password];
            args[url] = MakeUri("SpecificText");

            TriggerScript(SendExecuteTest.script1.FullName, args);

            if(!WaitForSignal( SendExecuteTest.script1.S_Signal.FullName, 10 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Phone never made a request back to the Application Server");
                return false;
            }

            args.Clear();
            args[title] = input[title];
            args[prompt] = input[prompt];
            args[text] = input[text];

            SendEvent( "SpecificText", routingGuid, args);


            if(!WaitForSignal( SendExecuteTest.script1.S_Signal.FullName, 10 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Never received response from the application for SendExecute completion");
                return false;
            }

            if(!sendExecuteSuccess)
            {
                return false;
            }
            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData ipField = new TestTextInputData("Phone IP", 
                "The IP address of the phone.", ip, 80);
            TestTextInputData usernameField = new TestTextInputData("Username", 
                "The username of the user associated with the phone.", username, 80);
            TestTextInputData passwordField = new TestTextInputData("Password", 
                "The password of the user associated with the phon.", password, 80);
            TestTextInputData titleField = new TestTextInputData("Title", "The title of the text object",
                title, "", 80);
            TestTextInputData promptField = new TestTextInputData("Prompt", "The prompt of the text object",
                prompt, "", 80);
            TestMultiLineTextInputData textField = new TestMultiLineTextInputData("Text", "The text of the text object",
                text, 80, String.Empty);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(titleField);
            inputs.Add(promptField);
            inputs.Add(textField);

            return inputs;
        }

        public void SendExecuteComplete(ActionMessage im)
        {
            if(!receiveOneMessage)
            {
                routingGuid = im.RoutingGuid;
                receiveOneMessage = true;
            }
            else
            {
                sendExecuteSuccess = (bool) im["success"];

                if(!sendExecuteSuccess)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "The SendExecute command failed");
                }

                
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { SendExecuteTest.FullName, SpecificTextTest.FullName, CreateTextTest.FullName };
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

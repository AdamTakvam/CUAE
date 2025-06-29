using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SendExecuteTest = Metreos.TestBank.IVT.IVT.SendExecute;
using SpecificMenuTest = Metreos.TestBank.IVT.IVT.SpecificMenu;
using CreateTextTest = Metreos.TestBank.IVT.IVT.CreateText;

namespace Metreos.FunctionalTests.IVT2._0.IPPhoneServices
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class SpecificMenu : FunctionalTestBase
    {
        private const string url = "url";
        private const string ip = "ip";
        private const string username = "username";
        private const string password = "password";
        private const string title = "title";
        private const string prompt = "prompt";
        private const string menuName1 = "menuName1";
        private const string menuName2 = "menuName2";
        private const string menuUrl1 = "menuUrl1";
        private const string menuUrl2 = "menuUrl2";

        private bool receiveOneMessage;
        private bool sendExecuteSuccess;
        private string routingGuid;

        public SpecificMenu() : base(typeof( SpecificMenu ))
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
            args[url] = MakeUri("SpecificMenu");

            TriggerScript(SendExecuteTest.script1.FullName, args);

            if(!WaitForSignal( SendExecuteTest.script1.S_Signal.FullName, 10 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Phone never made a request back to the Application Server");
                return false;
            }
            
            args.Clear();
            args[title] = input[title];
            args[prompt] = input[prompt];
            args[menuName1] = input[menuName1];
            args[menuName2] = input[menuName2];
            args[menuUrl1] = input[menuUrl1];
            args[menuUrl2] = input[menuUrl2];

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
            TestTextInputData menuName1Field = new TestTextInputData("Menu Name 1", "The name of the first menu item",
                menuName1, "", 80);
            TestTextInputData menuUrl1Field = new TestTextInputData("Menu Url 1", "The url of the first menu item",
                menuUrl1, "", 80);
            TestTextInputData menuName2Field = new TestTextInputData("Menu Name 2", "The name of the second menu item",
                menuName2, "", 80);
            TestTextInputData menuUrl2Field = new TestTextInputData("Menu Url 2", "The url of the second menu item",
                menuUrl2, "", 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(ipField);
            inputs.Add(usernameField);
            inputs.Add(passwordField);
            inputs.Add(titleField);
            inputs.Add(promptField);
            inputs.Add(menuName1Field);
            inputs.Add(menuName2Field);
            inputs.Add(menuUrl1Field);
            inputs.Add(menuUrl2Field);

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

                receiveOneMessage = true;
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { SendExecuteTest.FullName, SpecificMenuTest.FullName, CreateTextTest.FullName };
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

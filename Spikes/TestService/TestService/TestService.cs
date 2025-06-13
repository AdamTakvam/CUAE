using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace TestService
{
    public partial class TestService : ServiceBase
    {
        public TestService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string blah = DummyLib.Class1.SomeString;
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}

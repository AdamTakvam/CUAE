using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace TestService
{
    [RunInstaller(true)]
    public partial class TestServiceInstaller : Installer
    {
        public TestServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
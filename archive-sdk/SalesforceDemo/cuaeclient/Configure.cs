using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuaeLocalServer
{
    public partial class Configure : Form
    {
        public Configure()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;
        }

        public void InitializeFields()
        {
            cuaeIPAddress.Text = Configuration.Default.CUAEIP;
            deviceName.Text = Configuration.Default.DeviceName;
            outbound.Text = Configuration.Default.PhoneNumber;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Configuration.Default.CUAEIP = cuaeIPAddress.Text;
            Configuration.Default.DeviceName = deviceName.Text;
            Configuration.Default.PhoneNumber = outbound.Text;
            Configuration.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
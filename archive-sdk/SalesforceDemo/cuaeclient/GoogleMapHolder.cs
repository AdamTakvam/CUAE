using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuaeLocalServer
{
    public partial class GoogleMapHolder : Form
    {
        public WebBrowser Browser { get { return webBrowser1; } }

        public GoogleMapHolder()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(GoogleMapHolder_FormClosing);
        }

        void GoogleMapHolder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
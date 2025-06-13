using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Metreos.Max.Core;

namespace Metreos.Max.Drawing
{
    public partial class LocalizationHelpDlg : Form
    {
        private static Size localeBasicsSize = new Size(342, 156);
        private static Size localeUsageSize = new Size(341, 117);

        public LocalizationHelpDlg()
        {
            InitializeComponent();

            // Post process settings because VS default editor not working
            basicsLabel.Text = Const.Localization.LocaleEditorBasics;
            basicsLabel.Size = localeBasicsSize;
            usageLabel.Text = Const.Localization.LocaleEditorUsage;
            usageLabel.Size = localeUsageSize;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
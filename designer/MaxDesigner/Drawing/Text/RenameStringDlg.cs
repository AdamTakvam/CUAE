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
    public partial class RenameString : Form
    {
        public string ChosenName { get { return chosenName; } }
        private string chosenName;

        public string InitialText 
        { 
            set 
            {
                initialText = value;
                textBox.Text = value;
            } 
        }
        private string initialText;

        public MaxLocalizationEditor Host { set { host = value; } }
        private MaxLocalizationEditor host;

        public RenameString()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Check for already existing
            if ((initialText != textBox.Text) && host.HasPrompt(textBox.Text))
            {
                Utl.DuplicateStringDefined();
            }
            else if (!Utl.ValidateLocalizableStringName(textBox.Text))
            {
                // Do nothing
            }
            else
            {
                chosenName = textBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RenameString_Load(object sender, EventArgs e)
        {

        }

    }
}
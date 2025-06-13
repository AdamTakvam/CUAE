using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Metreos.Max.Core;
using Metreos.Max.Framework;

namespace Metreos.Max.Drawing
{
    public partial class MediaFileLocaleDlg : Form
    {
        public CultureInfo SelectedCulture { get { return selectedCulture; } }
        private CultureInfo selectedCulture;

        public CultureInfo ForceCulture { set { forceCulture = value; SetForceBehavior(); } }
        private CultureInfo forceCulture;

        public string SelectedPath { get { return selectedPath; } }
        private string selectedPath;

        public MediaFileLocaleDlg()
        {
            InitializeComponent();
            this.Text = Const.AddAudioDlgTitle;

            this.DialogResult = DialogResult.Cancel;
            mediaFilePath.TextChanged += new EventHandler(MediaPathTextChanged);

            CultureInfo[] cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
            SortedList<string, CultureInfo> cultureList = new SortedList<string, CultureInfo>();
            foreach (CultureInfo culture in cultures)
            {
                cultureList.Add(culture.DisplayName, culture);
            }
            CultureInfo[] sortedCultures = new CultureInfo[cultures.Length];
            cultureList.Values.CopyTo(sortedCultures, 0);
            this.localeComboBox.Items.AddRange(sortedCultures);
            this.localeComboBox.SelectedItem = Const.DefaultLocale;
            this.okButton.Enabled = false;
        }

        private void SetForceBehavior()
        {
            if (forceCulture != null)
            {
                this.localeComboBox.SelectedItem = forceCulture;
                this.localeComboBox.Enabled = false;
            }
        }

        private void Reset()
        {
            this.forceCulture = null;
            this.localeComboBox.SelectedItem = Const.DefaultLocale;
            this.okButton.Enabled = false;
            this.localeComboBox.Enabled = true;
            this.mediaFilePath.Text = String.Empty;
        }

        private void MediaPathTextChanged(object sender, EventArgs e)
        {
                string filename = Path.GetFileNameWithoutExtension(mediaFilePath.Text);

            if (File.Exists(mediaFilePath.Text))
            {
                okButton.Enabled = true;

                CultureInfo embeddedCulture = null;

                // attempt to parse file for embedded culture
                int lastUnderscorePos = filename.LastIndexOf('_');
                if (lastUnderscorePos > 0 && lastUnderscorePos < filename.Length) // Don't care about filenames starting in underscore after all
                {
                    try
                    {
                        string embeddedCultureName = filename.Substring(lastUnderscorePos + 1);
                        embeddedCulture = new CultureInfo(embeddedCultureName);
                    }
                    catch { }
                }

                if (embeddedCulture != null)
                {
                    // This most match forcedCulture if set--otherwise, reject attempt
                    if (forceCulture == null)
                    {
                        localeComboBox.SelectedItem = embeddedCulture;
                        localeComboBox.Enabled = false;
                    }
                    else
                    {
                        if (forceCulture.Name == embeddedCulture.Name)
                        {
                            // no need to set localeComboBox because it's set on cstr
                            // no need to enable localeComboBox if forcedCulture
                            //   is set, because in this mode it should never be set to enabled
                        }
                        else
                        {
                            // Should warn user that this is an invalid file
                            okButton.Enabled = false;
                        }
                    }
                }
                else
                {
                    if (forceCulture == null)
                    {
                        localeComboBox.Enabled = true;
                    }
                }
            }
            else
            {
                okButton.Enabled = false;
                if (forceCulture == null)
                {
                    localeComboBox.Enabled = true;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Pull out path and locale
            selectedCulture = localeComboBox.SelectedItem as CultureInfo;
            selectedPath = mediaFilePath.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = Const.AddAudioDlgTitle;
            dlg.DefaultExt = null;
            dlg.FileName = null;
            dlg.Filter = Const.MaxAudioFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = MaxMain.ProjectFolder;
            dlg.RestoreDirectory = true;

            string path = dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
            mediaFilePath.Text = path;  
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
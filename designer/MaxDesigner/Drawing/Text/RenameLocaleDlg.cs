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
    public partial class RenameLocale : Form
    {
        public CultureInfo SelectedCulture { get { return selectedCulture; } }
        private CultureInfo selectedCulture;

        public MaxLocalizationEditor Host { set { host = value; } }
        private MaxLocalizationEditor host;

        public CultureInfo InitialCulture 
        {
            set 
            {
                initialCulture = value;
                localeComboBox.SelectedItem = value;
                this.Text = String.Format("Assign New Locale over " + value.Name);
            }
        }
        private CultureInfo initialCulture;
       
        public RenameLocale()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;

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
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            selectedCulture = this.localeComboBox.SelectedItem as CultureInfo;

            if ((selectedCulture.Name != initialCulture.Name) && 
                host != null && selectedCulture != null && 
                host.HasLocale(selectedCulture))
            {
                Utl.DuplicateLocaleDefined();
            }
            else
            {
                // Check for already existing
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
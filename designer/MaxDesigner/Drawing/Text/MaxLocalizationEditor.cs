using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Resources;
using System.Reflection;
using Metreos.AppArchiveCore.Xml;
using Metreos.Max.Core;

namespace Metreos.Max.Drawing
{
    public partial class MaxLocalizationEditor : UserControl
    {
        private static XmlSerializer seri = new XmlSerializer(typeof(LocaleTableType));

        private MaxTabContent host;
        private bool noNotify;
        public bool NoNotify { set { noNotify = value; } get { return noNotify; } }

        private const int defaultWidth = 100;

        private NewLocale localeDlg;
        private NewString stringDlg;
        private RenameString renameStringDlg;
        private RenameLocale renameLocaleDlg;
        private LocalizationHelpDlg helpDlg;
        private bool sortAlphaToggle;

        private static XmlDocument cDataCreator = new XmlDocument();

        public MaxLocalizationEditor(MaxTabContent tab)
        {
            InitializeComponent();

            this.sortAlphaToggle = true;
            this.host = tab;
            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Fill;

            this.localeDlg = new NewLocale();
            this.stringDlg = new NewString();
            this.renameLocaleDlg = new RenameLocale();
            this.renameStringDlg = new RenameString();
            this.helpDlg = new LocalizationHelpDlg();
            this.localeDlg.Host = this;
            this.stringDlg.Host = this;
            this.renameStringDlg.Host = this;
            this.renameLocaleDlg.Host = this;

            this.Load += new EventHandler(Loaded);
            this.dataGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(EditingControlShowing);
            this.dataGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.dataGrid.ColumnAdded += new DataGridViewColumnEventHandler(ColumnsAdded);
            this.dataGrid.ColumnRemoved += new DataGridViewColumnEventHandler(ColumnsRemoved);
            this.dataGrid.RowsRemoved += new DataGridViewRowsRemovedEventHandler(RowsRemoved);
            this.dataGrid.RowsAdded += new DataGridViewRowsAddedEventHandler(RowsAdded);
            this.dataGrid.CellValueChanged += new DataGridViewCellEventHandler(CellValueChanged);
            this.dataGrid.ColumnWidthChanged += new DataGridViewColumnEventHandler(ColumnWidthChanged);
            this.dataGrid.RowHeightChanged += new DataGridViewRowEventHandler(RowHeightChanged);
            this.dataGrid.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(ColumnDispayIndexChanged);
            this.dataGrid.ColumnNameChanged += new DataGridViewColumnEventHandler(DGColumnNameChanged);
            this.dataGrid.Sorted += new EventHandler(Sorted);
            this.dataGrid.MouseDown += new MouseEventHandler(GenericSortRequest);
            this.dataGrid.TopLeftHeaderCell.Value = Const.LocalizableEditorTopLeftCell;
            this.dataGrid.TopLeftHeaderCell.ToolTipText = "Click to sort";
            this.dataGrid.ColumnHeadersVisible = true;
            this.dataGrid.RowHeadersWidth = 100;
            this.dataGrid.AutoGenerateColumns = false;
            this.dataGrid.AllowUserToResizeColumns = dataGrid.AllowUserToResizeRows = dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.MultiSelect = false;
            this.dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        #region Custom Item Sorting

        private class RowComparer : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            public RowComparer(SortOrder sortOrder)
            {
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow dataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow dataGridViewRow2 = (DataGridViewRow)y;

                // Try to sort based on the Last Name column.
                int CompareResult = System.String.Compare(
                    dataGridViewRow1.HeaderCell.Value.ToString(),
                    dataGridViewRow2.HeaderCell.Value.ToString());

                return CompareResult * sortOrderModifier;
            }
        }


        private void GenericSortRequest(object sender, MouseEventArgs e)
        {
            if (this.dataGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.TopLeftHeader)
            {
                this.dataGrid.Sort(new RowComparer(sortAlphaToggle ? SortOrder.Ascending : SortOrder.Descending));
                sortAlphaToggle = !sortAlphaToggle;
            }
        }
        #endregion

        #region Reasons for going Dirty

        private void Sorted(object sender, EventArgs e)
        {
            MarkDirty();
        }

        private void DGColumnNameChanged(object sender, DataGridViewColumnEventArgs e)
        {
            MarkDirty();
        }

        private void ColumnDispayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            MarkDirty();
        }

        private void RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            MarkDirty();
        }

        private void ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            MarkDirty();
        }

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            MarkDirty();
        }

        private void RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            MarkDirty();
        }

        void RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            MarkDirty();
        }

        // Columns Removed (see below)
        // Colums Added (see below)

        public void MarkDirty()
        {
            if (!noNotify && !host.Dirty)
            {
                host.SignalDirty();
            }
        }
        #endregion

        #region Validation Logic
        public bool HasPrompt(string promptName)
        {
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.HeaderCell.Value != null &&
                    row.HeaderCell.Value.ToString() == promptName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasLocale(CultureInfo culture)
        {
            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                if (culture.Name == column.Name)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Max Editor Save/Open/Load

        /// <summary>Open file and load into editor</summary>
        public bool Open(string path)
        {
            bool result = false;
            try { result = this.LoadData(path); }
            catch { }

            if (!noNotify) host.SignalNotDirty();
            return result;
        }

        /// <summary>Save editor file</summary>
        public bool Save(string path)
        {
            bool result = false;
            try { result = this.SaveFile(path); }
            catch { }

            host.SignalNotDirty();
            return result;
        }


        public bool SaveFile(string path)
        {
            bool saved = false;

            // Make localeTableType 
            LocaleTableType localeTable = null;

            localeTable = new LocaleTableType();
            localeTable.Locales = new Locales();
            localeTable.Locales.@default = null; // not implemented
            localeTable.Locales.@readonly = false; // not implemented


            SortedList<int, Locale> localeList = new SortedList<int, Locale>();

            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                string name = column.Name;
                Locale locale = new Locale();
                locale.name = name;
                locale.devmode = false; // not implemented
                locale.width = column.Width;

                localeList.Add(column.DisplayIndex, locale);

                List<PromptInfo> promptInfoList = new List<PromptInfo>();

                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    DataGridViewTextBoxCell cell = row.Cells[column.Index] as DataGridViewTextBoxCell;
                    PromptInfo promptInfo = new PromptInfo();
                    promptInfo.Value = cDataCreator.CreateCDataSection(cell.Value == null ? null : cell.Value.ToString());
                    promptInfo.@ref = row.HeaderCell.Value.ToString();
                    promptInfoList.Add(promptInfo);
                }

                PromptInfo[] promptInfos = new PromptInfo[promptInfoList.Count];
                promptInfoList.CopyTo(promptInfos);
                locale.PromptInfos = promptInfos;
            }

            Locale[] locales = new Locale[localeList.Values.Count];
            localeList.Values.CopyTo(locales, 0);
            localeTable.Locales.Locale = locales;

            List<Prompt> promptList = new List<Prompt>();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                Prompt prompt = new Prompt();
                prompt.name = row.HeaderCell.Value.ToString();
                prompt.height = row.Height;
                promptList.Add(prompt);
            }
            Prompt[] prompts = new Prompt[promptList.Count];
            promptList.CopyTo(prompts);
            localeTable.Prompts = new Prompts();
            localeTable.Prompts.Prompt = prompts;

            try
            {
                using (FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write))
                {
                    seri.Serialize(stream, localeTable);
                }
                saved = true;
            }
            catch { }

            return saved;
        }

        #endregion

        #region Add/Remove/Request Items

        private void ColumnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == Const.Localization.RenameContextItemName)
            {
                DataGridViewHeaderCell ownerCell = e.ClickedItem.Tag as DataGridViewHeaderCell;
                string columnName = ownerCell.OwningColumn.Name;
                renameLocaleDlg.InitialCulture = new CultureInfo(columnName);
                if (renameLocaleDlg.ShowDialog() == DialogResult.OK)
                {
                    if (renameLocaleDlg.SelectedCulture != null)
                    {
                        ownerCell.OwningColumn.Name = renameLocaleDlg.SelectedCulture.Name;
                    }
                }

                e.ClickedItem.Owner.Hide();
            }
            else if (e.ClickedItem.Text == Const.Localization.RemoveContextItemName)
            {
                DataGridViewHeaderCell ownerCell = e.ClickedItem.Tag as DataGridViewHeaderCell;
                // check if this is the last column
                if (dataGrid.Columns.Count == 1)
                {
                    // Give user last chance to backout--deleting this causes all 
                    // rows to dissappear!
                    if (Utl.WarnDeletionAllStrings() == DialogResult.OK)
                    {
                        dataGrid.Columns.Remove(ownerCell.OwningColumn);
                    }
                }
                else
                {
                    dataGrid.Columns.Remove(ownerCell.OwningColumn);
                }

                e.ClickedItem.Owner.Hide();
            }

        }

        private void RowItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == Const.Localization.RenameContextItemName)
            {
                DataGridViewHeaderCell cell = e.ClickedItem.Tag as DataGridViewHeaderCell;
                string rowName = cell.Value.ToString();
                // Pop up rename editor after setting the initial text
                renameStringDlg.InitialText = rowName;
                if (renameStringDlg.ShowDialog() == DialogResult.OK)
                {
                    if (renameStringDlg.ChosenName != null && renameStringDlg.ChosenName != String.Empty)
                    {
                        // Find the real header cell
                        foreach (DataGridViewRow row in dataGrid.Rows)
                        {
                            if (rowName == row.HeaderCell.Value.ToString())
                            {
                                // keep both real cell and fake cell in tandem
                                cell.Value = renameStringDlg.ChosenName;
                                row.HeaderCell.Value = cell.Value;
                                break;
                            }
                        }
                    }
                }
                e.ClickedItem.Owner.Hide();
            }
            else if (e.ClickedItem.Text == Const.Localization.RemoveContextItemName)
            {
                DataGridViewHeaderCell cell = e.ClickedItem.Tag as DataGridViewHeaderCell;
                string rowName = cell.Value.ToString();
                // Find row manually--don't use OwnerRow proprety on cell.
                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    if (rowName == row.HeaderCell.Value.ToString())
                    {
                        dataGrid.Rows.Remove(row);
                        break;
                    }
                }
                e.ClickedItem.Owner.Hide();
            }
        }

        #endregion

        #region DataGridView Load

        public static string CreateDefaultLocalesText()
        {
            Metreos.AppArchiveCore.Xml.LocaleTableType locales = CreateDefaultLocales();
            System.Text.StringBuilder localesText = new System.Text.StringBuilder();
            StringWriter writer = new StringWriter(localesText);
            seri.Serialize(writer, locales);
            return localesText.ToString();
        }

        private static Metreos.AppArchiveCore.Xml.LocaleTableType CreateDefaultLocales()
        {
            Metreos.AppArchiveCore.Xml.LocaleTableType locales = new Metreos.AppArchiveCore.Xml.LocaleTableType();
            locales.Locales = new Locales();
            locales.Locales.@readonly = false;
            locales.Locales.@default = null;
            locales.Prompts = new Prompts();
            return locales;
        }

        private bool LoadData(string filePath)
        {
            bool found = false;
            LocaleTableType localeTable = null;
            if (File.Exists(filePath))
            {
                found = true;
                FileStream stream = null;
                try
                {
                    stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                    XmlTextReader reader = new XmlTextReader(stream);
                    localeTable = seri.Deserialize(reader) as LocaleTableType;
                }
                catch { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                }
            }

            if (found)
            {
                return PopulateDataGrid(localeTable);
            }
            else
            {
                return false;
            }
        }


        private bool PopulateDataGrid(LocaleTableType localeTable)
        {
            if (localeTable == null || localeTable.Locales == null ||
                localeTable.Locales.Locale == null || localeTable.Locales.Locale.Length == 0)
            {
                // Empty table 
                this.addString.Enabled = false;
            }
            else
            {
                // Add colums first, which are the locales themselves
                foreach (Locale locale in localeTable.Locales.Locale)
                {
                    string localeName = locale.name;
                    int localeWidth = locale.width;
                    // try to create a culture from this locale name
                    // if not possible, then this locale will be dropped from the editor
                    CultureInfo culture = null;
                    try
                    {
                        culture = new CultureInfo(localeName);
                    }
                    catch
                    {
                        // skip this locale
                        continue;
                    }

                    AddLocale(culture.Name, localeWidth);
                }

                if (localeTable.Prompts == null || localeTable.Prompts.Prompt == null ||
                    localeTable.Prompts.Prompt.Length == 0)
                {
                    // no prompts
                }
                else
                {
                    foreach (Prompt prompt in localeTable.Prompts.Prompt)
                    {
                        string promptName = prompt.name;
                        int promptHeight = prompt.height;

                        AddPrompt(promptName, promptHeight);

                        DataGridViewRow row = dataGrid.Rows[dataGrid.Rows.Count - 1];

                        // Iterate through each cell in the row, populating the value for the prompt text
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Get locale name for this cell
                            string localeName = cell.OwningColumn.Name;

                            Locale locale = FindLocale(localeName, localeTable);
                            PromptInfo promptInfo = FindPrompt(promptName, locale);

                            string cellText = null;
                            if (promptInfo != null)
                            {
                                cellText = promptInfo.Value.Value;
                            }

                            cell.Value = cellText;
                        }
                    }
                }
            }

            dataGrid.ClearSelection();

            return true;
        }


        private void Loaded(object sender, EventArgs e)
        {
            if (this.dataGrid.Columns.Count == 0)
            {
                addString.Enabled = false;
            }
        }


        #endregion

        #region Grid Content Logic

        private void EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // fix right-to-left temporarily--best I can do MSC
            // the issue is that I have no clue how to get to the underlying editing control
            CultureInfo currentCulture = new CultureInfo(dataGrid.CurrentCell.OwningColumn.Name);
            e.Control.RightToLeft = currentCulture.TextInfo.IsRightToLeft ? RightToLeft.Yes : RightToLeft.No;
        }




        private Locale FindLocale(string localeName, LocaleTableType localeTable)
        {
            foreach (Locale locale in localeTable.Locales.Locale)
            {
                if (locale.name == localeName)
                {
                    return locale;
                }
            }

            return null;
        }

        private PromptInfo FindPrompt(string promptName, Locale locale)
        {
            PromptInfo prompt = null;
            if (locale != null && locale.PromptInfos != null)
            {
                foreach (PromptInfo promptInfo in locale.PromptInfos)
                {
                    if (promptInfo.@ref == promptName)
                    {
                        prompt = promptInfo;
                        break;
                    }
                }
            }
            return prompt;
        }

        private void AddLocale(string localeName)
        {
            AddLocale(localeName, defaultWidth);
        }

        private void AddLocale(string localeName, int width)
        {
            // add a new column
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = localeName;
            if (width > 0)
            {
                col.Width = width;
            }
            else
            {
                col.Width = defaultWidth;
            }
            ContextMenuStrip strip = new ContextMenuStrip();
            strip.Leave += new EventHandler(StripLeave);
            strip.Items.Add(Const.Localization.RenameContextItemName);
            strip.Items[strip.Items.Count - 1].Tag = col.HeaderCell;
            strip.ItemClicked += new ToolStripItemClickedEventHandler(ColumnItemClicked);

            strip.Items.Add(Const.Localization.RemoveContextItemName);
            strip.Items[strip.Items.Count - 1].Tag = col.HeaderCell;

            col.HeaderCell.ContextMenuStrip = strip;

            dataGrid.Columns.Add(col);
        }

        private void StripLeave(object sender, EventArgs e)
        {
            ContextMenuStrip strip = sender as ContextMenuStrip;
            strip.Hide();
        }

        private void AddPrompt(string promptName)
        {
            AddPrompt(promptName, -1);
        }

        private void AddPrompt(string promptName, int height)
        {
            // add a new row
            DataGridViewRow row = new DataGridViewRow();
            row.HeaderCell.Value = promptName;
            if (height > 0)
            {
                row.Height = height;
            }
            ContextMenuStrip strip = new ContextMenuStrip();
            strip.Leave += new EventHandler(StripLeave);
            strip.Items.Add(Const.Localization.RenameContextItemName);
            strip.ItemClicked += new ToolStripItemClickedEventHandler(RowItemClicked);
            strip.Items[strip.Items.Count - 1].Tag = row.HeaderCell;

            strip.Items.Add(Const.Localization.RemoveContextItemName);
            strip.Items[strip.Items.Count - 1].Tag = row.HeaderCell;

            row.HeaderCell.ContextMenuStrip = strip;

            this.dataGrid.Rows.Add(row);

            // You must add the row after it's been added, as pulled out from the collection
        }

        private void addLocale_Click(object sender, EventArgs e)
        {
            if (localeDlg.ShowDialog() == DialogResult.OK)
            {
                if (localeDlg.SelectedCulture != null)
                {
                    AddLocale(localeDlg.SelectedCulture.Name);
                }
            }
        }

        private void addString_Click(object sender, EventArgs e)
        {
            if (stringDlg.ShowDialog() == DialogResult.OK)
            {
                string promptName = stringDlg.ChosenName;
                AddPrompt(promptName);
            }
        }

        private void helpButtonClick(object sender, EventArgs e)
        {
            helpDlg.ShowDialog();
        }

        private void ColumnsRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            MarkDirty();

            if (this.dataGrid.Columns.Count == 0)
            {
                addString.Enabled = false;
            }
        }

        private void ColumnsAdded(object sender, DataGridViewColumnEventArgs e)
        {
            MarkDirty();

            addString.Enabled = true;
        }

        #endregion
    }
}
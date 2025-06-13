using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

using Metreos.Max.Core;

namespace Metreos.Max.Drawing
{
    /// <summary>
    ///   Provides some overriding of the default behavior of the datagrid text box.
    ///   Primarily we can have notification of changing of the value contained in the
    ///   datagrid entry, as well as performing a password mask of a textbox's values
    ///   when in non-edit mode.
    ///   
    ///   Also we can validate the value entered into the textbox by looking up which
    ///   column this editor is in, and validate the value entered by the user.   
    ///   For instance, if a user edits the 'Name' column, a empty value is not supported
    ///   and a popup window informs the user of the problem
    /// </summary>
    public class StringColumn : DataGridTextBoxColumn 
    {
        public  const char PasswordChar = '*';
        public  const string UndefinedText = "";
        protected MaxInstallerEditor host;
        protected string oldValue;

        // Temps for editing storage
        protected CurrencyManager manager;
        protected int rowNum;

        public StringColumn(MaxInstallerEditor parent) : base()
        {
            this.host               = parent;
            this.TextBox.Multiline  = true;
            this.TextBox.Leave     += new EventHandler(Leave);  
        }
 
        #region Edit/Paint/Leave Implementations
        //These 3 methods ideally should be enough for all TextBox based derivates to work from

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            this.rowNum = rowNum;
            this.manager = source;
            object currentValue = this.GetColumnValueAtRow(source, rowNum);
            bounds.Y += 0;
            bounds.X += 2;
            SetPasswordBehavior(host.IsPassword(rowNum) && IsPasswordable());
            TextBox.Bounds = bounds;
            TextBox.Visible = true;
            TextBox.Text = ConvertFromInternalToUser(currentValue);
            oldValue = TextBox.Text;
            TextBox.Focus();
        }

        protected virtual void Leave(object sender, EventArgs e) 
        {
            if(manager == null) return;

            TextBox textBox = sender as TextBox;
            string enteredValue = textBox.Text;
            object saveValue = enteredValue;

            bool valid = Validate(enteredValue, rowNum);

            // if not already valid, skip further validations
            if(valid)
            {
                if(IsUserUndefined(enteredValue))
                {
                    saveValue = GetUndefinedInternalValue();
                }
                else
                {
                    valid = ConvertFromUserToInternal(enteredValue, out saveValue);
                }
       
                if(valid && rowNum == manager.Position)
                {
                    this.SetColumnValueAtRow(manager, rowNum, saveValue);
                }
            }

            this.TextBox.Visible = false;
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
        {
            System.Diagnostics.Debug.Assert(false, "Here");
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
        {
            System.Diagnostics.Debug.Assert(false, "Here2");
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            object currentValue  = this.GetColumnValueAtRow(source, rowNum);

            // vertical offset to account for frame of textbox
            //bounds.Y += 2;

            g.FillRectangle(backBrush, bounds);

            string paintValue = DeterminePaintValue(rowNum, currentValue);

            bounds.Height -= 2;
 
            g.DrawString(paintValue, this.TextBox.Font, foreBrush, bounds);  
        }
        #endregion

        #region Virtual Methods for Strongly Typed String Based Entry to Override

        /// <summary>
        ///   A converter method from string (userEntry) to the underlying type
        /// </summary>
        /// <remarks>In this case, their is nothing to convert, because the
        /// underlying type is string</remarks>
        protected virtual bool ConvertFromUserToInternal(string userEntry, out object converted)
        {
            converted = userEntry;
            return true;
        }

        protected virtual string ConvertFromInternalToUser(object currentValue)
        {
            string userValue;
            if(IsInternalUndefined(currentValue))
            {
                userValue = GetUndefinedPaintValue();   
            }
            else
            {
                userValue = currentValue as string;
            }

            return userValue;
        }

        /// <summary>
        ///   Is the type undefined from the user's perspective
        /// </summary>
        protected virtual bool IsUserUndefined(string currentValue)
        {
            return currentValue == null;
        }

        protected virtual bool Validate(string newValue, int rowNum)
        {
            bool valid = true;
            if(MaxInstallerEditor.ConfigWrapper.IsNameColumn(MappingName))
            {
                if(newValue == null || newValue == String.Empty && Metreos.Max.Manager.MaxProject.Instance.Dirty)
                {
                    MessageBox.Show(Const.UndefinedConfigItemMsg, Const.InvalidConfItemDlgTitle);
                    valid = false;
                }
                else if(!host.CanNameConfig(newValue, rowNum) && Metreos.Max.Manager.MaxProject.Instance.Dirty)
                {
                    MessageBox.Show(Const.DuplicateConfigItemMsg, Const.InvalidConfItemDlgTitle);
                    valid = false;
                }
            }
            return valid;
        }

        /// <summary>
        ///   Check if the incoming type is considered undefined
        /// </summary>
        protected virtual bool IsInternalUndefined(object currentValue)
        {
            return currentValue == null;
        }

        /// <summary>
        ///   Returns what the user should see when the type is undefined
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUndefinedPaintValue()
        {
            return UndefinedText;
        }

        /// <summary>
        ///   Returns what the internal type maps to when undefined
        /// </summary>
        /// <returns></returns>
        protected virtual object GetUndefinedInternalValue()
        {
            return null;
        }

        /// <summary>
        ///   Called before paint.  
        /// </summary>
        protected virtual string DeterminePaintValue(int rowNum, object currentValue)
        {
            string finalPaintValue = null;;
            if(IsInternalUndefined(currentValue))
            {
                finalPaintValue = GetUndefinedPaintValue();
            }
            else
            {
                finalPaintValue = currentValue as string;
            }

            if(host.IsPassword(rowNum) && IsPasswordable())
            {
                finalPaintValue = CreatePasswordMask(currentValue as string);
            }

            return finalPaintValue;
        }
        #endregion

        #region Password Utilities
        // Ideally refactored into class just for the 'Name' column, because
        // that is the field that uses password utilities

        protected string CreatePasswordMask(string toMask)
        {
            if(toMask == null)
            {
                toMask = UndefinedText;
            }

            string passwordProtectedValue;
            // Make password version of char
            if(toMask == UndefinedText)
            {
                passwordProtectedValue = toMask;
            }
            else
            {
                passwordProtectedValue = new string(PasswordChar, toMask.Length);
            }

            return passwordProtectedValue;
        }

    
        private bool IsPasswordable()
        {
            return this.MappingName == MaxInstallerEditor.ConfigWrapper.DefaultValueMapping; 
        }

        private void SetPasswordBehavior(bool isPassword)
        {
            if(isPassword)
            {
                // If the user is actively editing the password, let's just let them see what they are typing.
                // So for now, we will 
                // this.TextBox.PasswordChar = PasswordChar;
            }
            else
            {
                this.TextBox.PasswordChar = (char) 0;
            }
        }
        #endregion
    }

    /// <summary>
    ///     Adds integer validation to entry of value. (the text value
    ///     must parse to an integer)
    /// </summary>
    public class IntColumn : StringColumn
    {
        public new const string UndefinedText = "[Empty]";
        public IntColumn(MaxInstallerEditor parent) : base(parent)
        {
        }

        #region Strong Type Overrides

        protected override bool ConvertFromUserToInternal(string userEntry, out object converted)
        {
            bool ableToConvert = false;
            int userIntEntry;
            converted = null;
            try
            {
                userIntEntry = int.Parse(userEntry);
                ableToConvert = true;
                converted = MaxInstallerEditor.ConfigWrapper.NillableInteger.CreateNonNullInteger(userIntEntry);
            }
            catch
            {
                converted = MaxInstallerEditor.ConfigWrapper.NillableInteger.NullInteger;
            }

            return ableToConvert;
        }

        protected override string ConvertFromInternalToUser(object currentValue)
        {
            string userValue;
            if(IsInternalUndefined(currentValue))
            {
                userValue = GetUndefinedPaintValue();   
            }
            else
            {
                MaxInstallerEditor.ConfigWrapper.NillableInteger nillInt = currentValue as MaxInstallerEditor.ConfigWrapper.NillableInteger;
                userValue = nillInt.Value.ToString();
            }

            return userValue;
        }

        protected override bool Validate(string newValue, int rowNum)
        {
            bool valid = true;
            if(MaxInstallerEditor.ConfigWrapper.IsMinColumn(MappingName))
            {
                if(host.IsGreaterThanMax(newValue, rowNum) && Metreos.Max.Manager.MaxProject.Instance.Dirty)
                {
                    MessageBox.Show(Const.MinBoundaryMsg, Const.InvalidConfItemDlgTitle);
                    valid = false;
                }
            }
            else if(MaxInstallerEditor.ConfigWrapper.IsMaxColumn(MappingName))
            {
                if(host.IsLessThanMin(newValue, rowNum) && Metreos.Max.Manager.MaxProject.Instance.Dirty)
                {
                    MessageBox.Show(Const.MaxBoundaryMsg, Const.InvalidConfItemDlgTitle);
                    valid = false;
                }
            }

            return valid;
        }


        /// <summary>
        ///   Is the type undefined from the user's perspective
        /// </summary>
        protected override bool IsUserUndefined(string currentValue)
        {
            return currentValue == null;
        }

        /// <summary>
        ///   Check if the incoming type is considered undefined
        /// </summary>
        protected override bool IsInternalUndefined(object currentValue)
        {
            return currentValue == MaxInstallerEditor.ConfigWrapper.NillableInteger.NullInteger;
        }

        /// <summary>
        ///   Returns what the user should see when the type is undefined
        /// </summary>
        /// <returns></returns>
        protected override string GetUndefinedPaintValue()
        {
            return UndefinedText;
        }

        /// <summary>
        ///   Returns what the internal type maps to when undefined
        /// </summary>
        /// <returns></returns>
        protected override object GetUndefinedInternalValue()
        {
            return MaxInstallerEditor.ConfigWrapper.NillableInteger.NullInteger;
        }

        /// <summary>
        ///   Called before paint.  
        /// </summary>
        protected override string DeterminePaintValue(int rowNum, object currentValue)
        {
            string finalPaintValue;
            if(IsInternalUndefined(currentValue))
            {
                finalPaintValue = GetUndefinedPaintValue();
            }
            else
            {
                MaxInstallerEditor.ConfigWrapper.NillableInteger nillInt = (MaxInstallerEditor.ConfigWrapper.NillableInteger) currentValue;
                finalPaintValue = nillInt.Value.ToString();
            }

            return finalPaintValue;
        }
        #endregion
    }

    /// <summary>
    ///     Adds boolean validation to entry of value. (the text value entered
    ///     must be 'true' or 'false'.
    /// </summary>
    /// <remarks>
    ///     Not used
    /// </remarks>
    public class BoolColumn : StringColumn
    {
        public BoolColumn(MaxInstallerEditor parent) : base(parent)
        {
        }

        #region Strong Type Overrides

        protected override bool ConvertFromUserToInternal(string userEntry, out object converted)
        {
            bool ableToConvert = false;
            bool userBoolEntry;
            converted = null;
            try
            {
                userBoolEntry = bool.Parse(userEntry);
                ableToConvert = true;
                converted = MaxInstallerEditor.ConfigWrapper.NillableBoolean.CreateNonNullBoolean(userBoolEntry);
            }
            catch
            {
                converted = MaxInstallerEditor.ConfigWrapper.NillableBoolean.NullBoolean;
            }

            return ableToConvert;
        }

        protected override string ConvertFromInternalToUser(object currentValue)
        {
            string userValue;
            if(IsInternalUndefined(currentValue))
            {
                userValue = GetUndefinedPaintValue();   
            }
            else
            {
                MaxInstallerEditor.ConfigWrapper.NillableBoolean nillBool = currentValue as MaxInstallerEditor.ConfigWrapper.NillableBoolean;
                userValue = nillBool.Value.ToString();
            }

            return userValue;
        }

        /// <summary>
        ///   Is the type undefined from the user's perspective
        /// </summary>
        protected override bool IsUserUndefined(string currentValue)
        {
            return currentValue == null;
        }

        /// <summary>
        ///   Check if the incoming type is considered undefined
        /// </summary>
        protected override bool IsInternalUndefined(object currentValue)
        {
            return currentValue == MaxInstallerEditor.ConfigWrapper.NillableBoolean.NullBoolean;
        }

        /// <summary>
        ///   Returns what the user should see when the type is undefined
        /// </summary>
        /// <returns></returns>
        protected override string GetUndefinedPaintValue()
        {
            return UndefinedText;
        }

        /// <summary>
        ///   Returns what the internal type maps to when undefined
        /// </summary>
        /// <returns></returns>
        protected override object GetUndefinedInternalValue()
        {
            return MaxInstallerEditor.ConfigWrapper.NillableBoolean.NullBoolean;
        }

        /// <summary>
        ///   Called before paint.  
        /// </summary>
        protected override string DeterminePaintValue(int rowNum, object currentValue)
        {
            string finalPaintValue;
            if(IsInternalUndefined(currentValue))
            {
                finalPaintValue = GetUndefinedPaintValue();
            }
            else
            {
                MaxInstallerEditor.ConfigWrapper.NillableBoolean nillBool = (MaxInstallerEditor.ConfigWrapper.NillableBoolean) currentValue;
                finalPaintValue = nillBool.Value.ToString();
            }

            return finalPaintValue;
        }
        #endregion
    }

    /// <summary>
    ///   Overrides the TextBoxColumn and replaces with a checkbox
    ///   Preferable to BoolColumn. More Windows-formish.
    /// </summary>
    public class CheckBoxColumn : StringColumn
    {
        private bool attachedToDataGrid;
        private CheckBox checkBox;
        private CheckBox drawCheckBox;
        private bool initialized;
        public CheckBoxColumn(MaxInstallerEditor parent) : base(parent)
        {
            this.attachedToDataGrid = false;
            this.initialized = false;
            this.checkBox = new CheckBox();
            this.checkBox.CheckAlign = ContentAlignment.MiddleCenter;
            this.checkBox.Visible = false; // Not positioned initially
            this.drawCheckBox = new CheckBox();
            this.drawCheckBox.CheckAlign = ContentAlignment.MiddleCenter;
            this.drawCheckBox.Visible = false;
            //this.checkBox.CheckedChanged += new EventHandler(CheckedStateChanged);
            //this.checkBox.Leave += new EventHandler(checkBox_Leave);
        }
 
        #region Edit/Paint/CheckChange Implementations

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            this.manager = source;
            this.rowNum = rowNum;

            if(!attachedToDataGrid)
            { // We have to attach this to the controls of the datagrid
                this.DataGridTableStyle.DataGrid.Controls.Add(checkBox);
                this.DataGridTableStyle.DataGrid.Controls.Add(drawCheckBox);
                attachedToDataGrid = true;
            }

            bool isChecked = (bool) this.GetColumnValueAtRow(source, rowNum);
            checkBox.Checked = isChecked;
            checkBox.Bounds = bounds; // checkbox is positioned; ok to set visible
            checkBox.Visible = true;
      
            checkBox.Focus();

            Metreos.Max.Framework.MaxMain.MessageWriter.WriteLine(String.Format("Edit S{0} R{1}", checkBox.Checked ? 1 : 0, rowNum));
        }

        private void CheckedStateChanged(object sender, EventArgs e)
        {
            if(manager == null) return;

            if(initialized)
            {
                host.MarkDirty();

                CheckBox checkBox = sender as CheckBox;
  
                if(rowNum == manager.Position)
                {
                    this.SetColumnValueAtRow(manager, rowNum, checkBox.Checked);
                    Metreos.Max.Framework.MaxMain.MessageWriter.WriteLine(String.Format("LC S{0} R{1}", checkBox.Checked ? 1 : 0, rowNum));
                }
            }
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            //      this.manager = source;
            //      this.rowNum = rowNum;
      
            if(!attachedToDataGrid)
            { // We have to attach this to the controls of the datagrid
                this.DataGridTableStyle.DataGrid.Controls.Add(checkBox);
                this.DataGridTableStyle.DataGrid.Controls.Add(drawCheckBox);
                attachedToDataGrid = true;
            }
      
            bool isChecked = (bool) this.GetColumnValueAtRow(source, rowNum);
            g.FillRectangle(backBrush, bounds);
            drawCheckBox.Bounds = bounds;
            drawCheckBox.Visible = true;
            initialized = true;

            Metreos.Max.Framework.MaxMain.MessageWriter.WriteLine(String.Format("Paint S{0} R{1}", isChecked ? 1 : 0, rowNum));
        }
        #endregion

        private void checkBox_Leave(object sender, EventArgs e)
        {
            if(initialized)
            {
                host.MarkDirty();

                CheckBox checkBox = sender as CheckBox;
  
                if(rowNum == manager.Position)
                {
                    this.SetColumnValueAtRow(manager, rowNum, checkBox.Checked);

                    Metreos.Max.Framework.MaxMain.MessageWriter.WriteLine(String.Format("LC S{0} R{1}", checkBox.Checked ? 1 : 0, rowNum));
                }
            }
        }
    }

    /// <summary>
    ///   Overrides the TextBoxColumn and replaces with a special combobox for the format field
    /// </summary>
    public class FormatColumn : StringColumn
    {
        public const string EditFormatsTag = "Edit...";
        private bool attachedToDataGrid;
        private ComboBox comboBox;
        public FormatColumn(MaxInstallerEditor parent) : base(parent)
        {
            this.attachedToDataGrid = false;
            this.comboBox = new ComboBox();
            this.comboBox.Font = TextBox.Font;
            this.comboBox.Visible = false; // Not positioned initially
            this.comboBox.Leave += new EventHandler(Leave);
            this.comboBox.KeyPress += new KeyPressEventHandler(comboBox_KeyPress);
        }
 
        #region Edit/Paint/Leave Implementations

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            this.manager = source;
            this.rowNum = rowNum;

            if(!attachedToDataGrid)
            { // We have to attach this to the controls of the datagrid
                this.DataGridTableStyle.DataGrid.Controls.Add(comboBox);
                this.ReadOnly = true;
                attachedToDataGrid = true;
            }
 
            MaxInstallerEditor.ConfigWrapper.FormatTypes types = this.GetColumnValueAtRow(source, rowNum) as MaxInstallerEditor.ConfigWrapper.FormatTypes;
            comboBox.Bounds = bounds; // checkbox is positioned; ok to set visible
            comboBox.BeginUpdate();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(MaxInstallerEditor.ConfigWrapper.FormatTypes.AllTypes);
            comboBox.Visible = true;
      
            if(MaxInstallerEditor.ConfigWrapper.FormatTypes.CurrentType == EditFormatsTag)
            {
            }
            else
            {
                comboBox.SelectedItem = MaxInstallerEditor.ConfigWrapper.FormatTypes.CurrentType;
            }

            comboBox.EndUpdate();
            comboBox.Focus();
        }

        protected override void Leave(object sender, EventArgs e)
        {
            if(manager == null) return;

            string selectedValue = comboBox.SelectedItem as string;

            host.MarkDirty();

            MaxInstallerEditor.ConfigWrapper.FormatTypes.CurrentType = selectedValue;
            if(manager.Position == rowNum)
            {
                this.SetColumnValueAtRow(manager, rowNum, MaxInstallerEditor.ConfigWrapper.FormatTypes.Instance);
            }
            comboBox.Visible = false;
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            if(!attachedToDataGrid)
            { // We have to attach this to the controls of the datagrid
                this.DataGridTableStyle.DataGrid.Controls.Add(comboBox);
                attachedToDataGrid = true;
            }

            MaxInstallerEditor.ConfigWrapper.FormatTypes types = this.GetColumnValueAtRow(source, rowNum) as MaxInstallerEditor.ConfigWrapper.FormatTypes;
            g.FillRectangle(backBrush, bounds);   
            g.DrawString(MaxInstallerEditor.ConfigWrapper.FormatTypes.CurrentType, this.comboBox.Font, foreBrush, bounds);
        }
        #endregion

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}

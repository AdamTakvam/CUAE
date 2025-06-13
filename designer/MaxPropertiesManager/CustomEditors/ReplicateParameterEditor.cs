using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// The control that will be dropped down when the down arrow on a vertex property is selected.
    /// </summary>
    public class ReplicateParameterGrowableEditor : System.Windows.Forms.UserControl
    {       
        private System.ComponentModel.Container components = null;
        private MaxPropertiesManager.RemovePropertyFromGrid removeProperty;
        private IWindowsFormsEditorService edSvc;
        private ITypeDescriptorContext context;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private ActionParameterPropertyGrowable actionParamGrower;

        public ReplicateParameterGrowableEditor(
            ITypeDescriptorContext context, 
            IWindowsFormsEditorService edSvc, 
            object value, 
            MaxPropertiesManager.RemovePropertyFromGrid removeProperty)
        {
            InitializeComponent();		
	
            this.context = context;
            this.edSvc = edSvc;
            this.actionParamGrower = value as ActionParameterPropertyGrowable;
            this.removeProperty = removeProperty;

            InitializeEditor();
        }

        protected void InitializeEditor()
        {
            removeButton.Enabled = false;

            MaxProperty actionParams = (MaxProperty) context.PropertyDescriptor;
            PropertyDescriptorCollection allProperties = actionParams.Container;
            
            InitializeComboBox(allProperties);
            InitializeInfoGrid(allProperties);
        }

        protected void InitializeInfoGrid(PropertyDescriptorCollection allProperties)
        {
            listView1.Items.Clear();

            foreach(MaxProperty property in allProperties)
            {
                if(property is ActionParameterProperty)
                {                
                    ActionParameterProperty parameter = property as ActionParameterProperty;
                    if(parameter.AllowMultiple)                     // Can be replicated; add to list
                    {
                        UserTypeProperty type = parameter.ChildrenProperties[DataTypes.USERTYPE] as UserTypeProperty;
                        ListViewItem item = 
                            new ListViewItem(
                            new string[] { 
                                             parameter.Name, 
                                             parameter.Value != null ? parameter.Value.ToString(): String.Empty, 
                                             type.Value.ToString().Substring(0, 3) });
                        item.Tag = parameter;
                        listView1.Items.Add(item);
                    }
                }
            }
        }

        protected void InitializeComboBox(PropertyDescriptorCollection allProperties)
        {
            StringCollection alreadyUsed = new StringCollection();

            foreach(MaxProperty property in allProperties)
                if(property is ActionParameterProperty)
                {                
                    ActionParameterProperty parameter = property as ActionParameterProperty;
                    if(parameter.AllowMultiple)                     // Can be replicated; add to list
                    {
                        if(alreadyUsed.Contains(parameter.Name)) continue;
                        alreadyUsed.Add(parameter.Name);

                        comboBox1.Items.Add(new ListPropertyItem(parameter));                
                    }
                }

            if(comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedItem = comboBox1.Items[0];
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)components.Dispose();          
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.NameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.Value = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.BackColor = System.Drawing.SystemColors.Control;
            this.addButton.Location = new System.Drawing.Point(112, 32);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(96, 24);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add Parameter";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Replicable Parameter Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorLabel
            // 
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(8, 64);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(200, 40);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.BackColor = System.Drawing.SystemColors.Control;
            this.removeButton.Location = new System.Drawing.Point(144, 240);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(64, 24);
            this.removeButton.TabIndex = 9;
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.NameColumnHeader,
                                                                                        this.Value,
                                                                                        this.Type});
            this.listView1.Location = new System.Drawing.Point(8, 104);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(192, 128);
            this.listView1.TabIndex = 10;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // NameColumnHeader
            // 
            this.NameColumnHeader.Text = "Name";
            this.NameColumnHeader.Width = 78;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 69;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 41;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.Location = new System.Drawing.Point(112, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(96, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // ReplicateParameterGrowableEditor
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addButton);
            this.Name = "ReplicateParameterGrowableEditor";
            this.Size = new System.Drawing.Size(216, 280);
            this.ResumeLayout(false);

        }

        #endregion      

        private void CloseDialog(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();	
        }

        private void SuggestCorrectActionToUser()
        {
            comboBox1.Focus();
            comboBox1.SelectAll();
        }

        private void ResetErrorState()
        {
            errorLabel.Text = String.Empty;
        }


        private void AddButton_Click(object sender, System.EventArgs e)
        {
            ListPropertyItem selectedItem = comboBox1.SelectedItem as ListPropertyItem;
            
            ActionParameterProperty replicateThis = selectedItem.Property as ActionParameterProperty;
            
            if( RequiredUserIntervention(replicateThis.DisplayName) )
                return;
            else 
                ResetErrorState();

            ActionParameterProperty replicated;
            if(replicateThis is MediaFileChooserProperty)
            {
                MediaFileChooserProperty mediaReplicate = replicateThis as MediaFileChooserProperty; 
                replicated = new MediaFileChooserProperty(
               (replicateThis as MediaFileChooserProperty).Subtype,
                replicateThis.Name,
                replicateThis.DisplayName,
                String.Empty,
                replicateThis.Required,
                replicateThis.AllowMultiple,
                replicateThis.Description,
                replicateThis.Mpm,
                replicateThis.Subject,
                replicateThis.Container);
            }
            else
            {
                replicated = new ActionParameterProperty(
                    replicateThis.Name,
                    replicateThis.DisplayName,
                    String.Empty,
                    replicateThis.Required,
                    true,
                    replicateThis.Description,
                    replicateThis.EnumValues,
                    replicateThis.Mpm,
                    replicateThis.Subject,
                    replicateThis.Container);
            }
            
            UserTypeProperty userType = new UserTypeProperty(actionParamGrower.Mpm.GetDefaultUserTypeDelegate(), replicateThis.Mpm,
                replicateThis.Subject, replicateThis.Container);

            MaxProperty typeProperty = RetrieveTypeProperty(replicateThis.ChildrenProperties);
            if(typeProperty != null)
            {
                replicated.ChildrenProperties.Add(typeProperty);
            }

            replicated.ChildrenProperties.Add(userType);
            replicated.Container.Add(replicated);

            InitializeInfoGrid(replicated.Container);
        }

        private void removeButton_Click(object sender, System.EventArgs e)
        {
            ResetErrorState();

            if(listView1.SelectedItems == null)     return;
            if(listView1.SelectedItems.Count == 0)  return;

            ArrayList toRemove = new ArrayList();
            ArrayList unRemovable = new ArrayList();
            ListViewItem[] items = new ListViewItem[listView1.SelectedItems.Count];
            listView1.SelectedItems.CopyTo(items, 0);
            
            foreach(ListViewItem item in items)
            {
                ActionParameterProperty actionParam = item.Tag as ActionParameterProperty;                

                if(IsRemovable(actionParam))
                {
                    listView1.Items.Remove(item);
                    removeProperty(null, actionParam);
                }
                else
                    unRemovable.Add(item);
            }

            if(unRemovable.Count == 0) return;
;
            System.Text.StringBuilder errorMessage = new System.Text.StringBuilder(20);
            if(unRemovable.Count == 1)
                errorMessage.Append("Parameter ");
            else 
                errorMessage.Append("Parameters "); 

            foreach(ListViewItem item in unRemovable)
            {
                ActionParameterProperty actionParam = item.Tag as ActionParameterProperty;
                errorMessage.Append(String.Format("'{0}', ", actionParam.Name));
            }
            errorMessage.Remove(errorMessage.Length - 2, 2);
            if(unRemovable.Count == 1)
                errorMessage.Append(" is chosen for removal but is the last parameter of that name.");
            else
                errorMessage.Append(" are chosen for removal but are the last parameters of that name.");

            errorLabel.Text = errorMessage.ToString();
        }

        private bool IsRemovable(ActionParameterProperty actionParam)
        {
            if(actionParam == null) return false;
            string actionParamName = actionParam.Name;

            int i = 0;
            foreach(MaxProperty property in actionParam.Container)
                if(property is ActionParameterProperty)
                {
                    ActionParameterProperty actionProp = property as ActionParameterProperty;
                    if(actionProp.Name == actionParamName)
                    {
                        i++;
                        if(i > 1)
                            return true;
                    }
                }

            return false;
        }

        private MaxProperty RetrieveTypeProperty(PropertyDescriptorCollection collection)
        {
            foreach(MaxProperty property in collection)
            {
                if(property.Name == DataTypes.VARIABLE_TYPES && property is ReflectorProperty) return property;
                else if(property.Name == DataTypes.VARIABLE_TYPES && property is GenericProperty) return property;
            }

            return null;
        }

        private bool RequiredUserIntervention(string textToCheck)
        {
            if(textToCheck == null || textToCheck == String.Empty)    
            {
                errorLabel.Text = Defaults.noReplicateParam;
                SuggestCorrectActionToUser();
                return true;
            }

            return false;
        }

        private void ItemChecked(object sender, ItemCheckEventArgs e)
        {
            // Handle the check logic
            if(e.CurrentValue == CheckState.Unchecked || e.CurrentValue == CheckState.Indeterminate)
            {
                e.NewValue = CheckState.Checked;
            }
            else if(e.CurrentValue == CheckState.Checked)
            {
                e.NewValue = CheckState.Unchecked;
            }

            // We know the checked list is going to grow, so go ahead and enable the remove button
            if(e.NewValue == CheckState.Checked)
            {
                removeButton.Enabled = true;
                return;
            }

            // The check list is becoming completely unchecked
            if(e.NewValue == CheckState.Unchecked && listView1.CheckedItems.Count == 1)
            {
                removeButton.Enabled = false;
                return;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
    
        }

        private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            removeButton.Enabled = true;
        }
    }
}

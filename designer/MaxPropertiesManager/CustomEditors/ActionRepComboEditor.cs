using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;


namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// The control that will be dropped down when the down arrow on a vertex property is selected.
    /// </summary>
    public class ActionRepComboEditor : System.Windows.Forms.UserControl
    {       
        private System.ComponentModel.Container components = null;
        private MaxPropertiesManager.RemovePropertyFromGrid removeProperty;
        private IWindowsFormsEditorService edSvc;
        private ITypeDescriptorContext context;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.CheckedListBox listBox1;
        private System.Windows.Forms.TextBox newParamNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label errorLabel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button removeButton2;
        private System.Windows.Forms.Button addButton2;
        private ActionParameterPropertyGrowable actionParamGrower;

        public ActionRepComboEditor(ITypeDescriptorContext context, IWindowsFormsEditorService edSvc, object value, MaxPropertiesManager.RemovePropertyFromGrid removeProperty)
        {
            InitializeComponent();		
	    
            this.context = context;
            this.edSvc = edSvc;
            this.actionParamGrower = value as ActionParameterPropertyGrowable;
            this.removeProperty = removeProperty;

            InitializeGrowEditor();
            InitializeRepEditor();
        }

        protected void InitializeGrowEditor()
        {
            removeButton.Enabled = false;

            MaxProperty actionParams = (MaxProperty) context.PropertyDescriptor;

            foreach(MaxProperty actionParameter in actionParams.ChildrenProperties)
                listBox1.Items.Add(new ListPropertyItem(actionParameter));
        }

        protected void InitializeRepEditor()
        {
            removeButton.Enabled = false;

            MaxProperty actionParams = (MaxProperty) context.PropertyDescriptor;
            PropertyDescriptorCollection allProperties = actionParams.Container;
            
            InitializeComboBox(allProperties);
            InitializeInfoGrid(allProperties);
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
            this.removeButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.CheckedListBox();
            this.newParamNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.NameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.Value = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.removeButton2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.addButton2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.errorLabel2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.BackColor = System.Drawing.SystemColors.Control;
            this.addButton.Location = new System.Drawing.Point(104, 48);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(88, 24);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add Parameter";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.BackColor = System.Drawing.SystemColors.Control;
            this.removeButton.Location = new System.Drawing.Point(136, 272);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(56, 24);
            this.removeButton.TabIndex = 3;
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(8, 128);
            this.listBox1.MultiColumn = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(184, 139);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemChecked);
            // 
            // newParamNameBox
            // 
            this.newParamNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.newParamNameBox.Location = new System.Drawing.Point(104, 16);
            this.newParamNameBox.Name = "newParamNameBox";
            this.newParamNameBox.Size = new System.Drawing.Size(88, 20);
            this.newParamNameBox.TabIndex = 5;
            this.newParamNameBox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "New Parameter Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(8, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Existing Parameters";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // errorLabel
            // 
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(8, 80);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(176, 23);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.Location = new System.Drawing.Point(104, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(96, 21);
            this.comboBox1.TabIndex = 16;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.NameColumnHeader,
                                                                                        this.Value,
                                                                                        this.Type});
            this.listView1.Location = new System.Drawing.Point(8, 128);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(192, 136);
            this.listView1.TabIndex = 15;
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
            // removeButton2
            // 
            this.removeButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton2.BackColor = System.Drawing.SystemColors.Control;
            this.removeButton2.Location = new System.Drawing.Point(144, 272);
            this.removeButton2.Name = "removeButton2";
            this.removeButton2.Size = new System.Drawing.Size(56, 24);
            this.removeButton2.TabIndex = 14;
            this.removeButton2.Text = "Remove";
            this.removeButton2.Click += new System.EventHandler(this.removeButton2_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Replicable Parameter Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // addButton2
            // 
            this.addButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton2.BackColor = System.Drawing.SystemColors.Control;
            this.addButton2.Location = new System.Drawing.Point(104, 48);
            this.addButton2.Name = "addButton2";
            this.addButton2.Size = new System.Drawing.Size(96, 24);
            this.addButton2.TabIndex = 12;
            this.addButton2.Text = "Add Parameter";
            this.addButton2.Click += new System.EventHandler(this.addButton2_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "Existing Parameters";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // errorLabel2
            // 
            this.errorLabel2.ForeColor = System.Drawing.Color.Red;
            this.errorLabel2.Location = new System.Drawing.Point(24, 75);
            this.errorLabel2.Name = "errorLabel2";
            this.errorLabel2.Size = new System.Drawing.Size(176, 32);
            this.errorLabel2.TabIndex = 18;
            this.errorLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.errorLabel);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.addButton);
            this.groupBox1.Controls.Add(this.removeButton);
            this.groupBox1.Controls.Add(this.newParamNameBox);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 312);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom Parameters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.removeButton2);
            this.groupBox2.Controls.Add(this.errorLabel2);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.addButton2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(200, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 304);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Replicatable Parameters";
            // 
            // ActionRepComboEditor
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "ActionRepComboEditor";
            this.Size = new System.Drawing.Size(408, 304);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion      

        private void CloseDialog(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();	
        }

        private void SuggestCorrectActionToUser()
        {
            newParamNameBox.Focus();
            newParamNameBox.SelectAll();
        }

        private void ResetErrorState()
        {
            errorLabel.Text = String.Empty;
        }

        private void ResetErrorState2()
        {
            errorLabel.Text = String.Empty;
        }

        private bool RequiredUserIntervention(string textToCheck)
        {
            if(textToCheck == null)    
            {
                errorLabel.Text = Defaults.noActionParamName;
                SuggestCorrectActionToUser();
                return true;
            }

            if(textToCheck == String.Empty)
            {
                errorLabel.Text = Defaults.noActionParamName;
                SuggestCorrectActionToUser();
                return true;
            }

            // Check duplicate param names
            if(listBox1.Items != null)
            {
                foreach(ListPropertyItem item in listBox1.Items)
                {
                    if(String.Compare(textToCheck, item.Property.DisplayName, true) == 0)
                    {
                        errorLabel.Text = Defaults.duplicateActionParamName;
                        SuggestCorrectActionToUser();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool RequiredUserIntervention2(string textToCheck)
        {
            if(textToCheck == null || textToCheck == String.Empty)    
            {
                errorLabel2.Text = Defaults.noReplicateParam;
                SuggestCorrectActionToUser();
                return true;
            }

            return false;
        }

        private void AddButton_Click(object sender, System.EventArgs e)
        {
            if( RequiredUserIntervention(newParamNameBox.Text) )
                return;
            else 
                ResetErrorState();                                           
      
            string nameForNewProperty = newParamNameBox.Text;
       
            ActionParameterProperty actionProperty = 
                new ActionParameterProperty(nameForNewProperty, nameForNewProperty, String.Empty,
                false, true, nameForNewProperty, null, actionParamGrower.Mpm, actionParamGrower.Subject, actionParamGrower.Container);
          
            UserTypeProperty userTypeProperty = 
                new UserTypeProperty(actionParamGrower.Mpm.GetDefaultUserTypeDelegate(), actionParamGrower.Mpm,
                actionParamGrower.Subject, actionParamGrower.Container);
               
            RenamingProperty renamingProperty = 
                new RenamingProperty(nameForNewProperty, actionProperty, actionParamGrower.Mpm, actionParamGrower.Subject, actionParamGrower.Container);

            actionProperty.ChildrenProperties.Add(userTypeProperty);;
            actionProperty.ChildrenProperties.Add(renamingProperty);
            actionParamGrower.ChildrenProperties.Add(actionProperty);

            listBox1.Items.Add(new ListPropertyItem(actionProperty));
        }

        private void RemoveButton_Click(object sender, System.EventArgs e)
        {
            if(listBox1.CheckedItems == null)     return;
            if(listBox1.CheckedItems.Count == 0)  return;

            while(listBox1.CheckedItems.Count != 0)
            {
                ListPropertyItem actionParam = listBox1.CheckedItems[0] as ListPropertyItem;
                listBox1.Items.Remove(actionParam);
                removeProperty(actionParamGrower, actionParam.Property);
            }
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
            if(e.NewValue == CheckState.Unchecked && listBox1.CheckedItems.Count == 1)
            {
                removeButton.Enabled = false;
                return;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
    
        }

        private void addButton2_Click(object sender, System.EventArgs e)
        {
            ListPropertyItem selectedItem = comboBox1.SelectedItem as ListPropertyItem;
            
            ActionParameterProperty replicateThis = selectedItem.Property as ActionParameterProperty;
            
            if( RequiredUserIntervention2(replicateThis.DisplayName) )
                return;
            else 
                ResetErrorState2();

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

        private void removeButton2_Click(object sender, System.EventArgs e)
        {
            ResetErrorState2();

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

            errorLabel2.Text = errorMessage.ToString();
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

        private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            removeButton.Enabled = true;
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
    }
}

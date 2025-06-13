using System;
using System.Collections;
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
    public class ResultDataGrowableEditor : System.Windows.Forms.UserControl
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
        private object value_;

        public ResultDataGrowableEditor(ITypeDescriptorContext context, IWindowsFormsEditorService edSvc, object value, MaxPropertiesManager.RemovePropertyFromGrid removeProperty)
        {
            InitializeComponent();		
	
            this.context = context;
            this.edSvc = edSvc;
            this.value_ = value;
            this.removeProperty = removeProperty;

            InitializeEditor();
        }

        protected void InitializeEditor()
        {
            removeButton.Enabled = false;

            MaxProperty resultParams = (MaxProperty) context.PropertyDescriptor;

            foreach(MaxProperty resultVar in resultParams.ChildrenProperties)
                listBox1.Items.Add(new ListPropertyItem(resultVar));
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
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.BackColor = System.Drawing.SystemColors.Control;
            this.addButton.Location = new System.Drawing.Point(80, 32);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(112, 24);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add Result Variable";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.listBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemChecked);
            // 
            // newParamNameBox
            // 
            this.newParamNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.newParamNameBox.Location = new System.Drawing.Point(80, 8);
            this.newParamNameBox.Name = "newParamNameBox";
            this.newParamNameBox.Size = new System.Drawing.Size(112, 20);
            this.newParamNameBox.TabIndex = 5;
            this.newParamNameBox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(-8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "New Result Variable Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "Existing Result Variables";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // errorLabel
            // 
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(8, 64);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(176, 23);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResultDataGrowableEditor
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.newParamNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Name = "ResultDataGrowableEditor";
            this.Size = new System.Drawing.Size(200, 304);
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

        private bool RequiredUserIntervention(string textToCheck)
        {
            if(textToCheck == null)    
            {
                errorLabel.Text = Defaults.noResultParam;
                SuggestCorrectActionToUser();
                return true;
            }

            if(textToCheck == String.Empty)
            {
                errorLabel.Text = Defaults.noResultParam;
                SuggestCorrectActionToUser();
                return true;
            }

            // Check duplicate param names
            if(listBox1.Items != null)
            {
                foreach(ListPropertyItem item in listBox1.Items)
                {
                    if(String.Compare(textToCheck, item.Property.DisplayName) == 0)
                    {
                        errorLabel.Text = Defaults.duplicateActionParamName;
                        SuggestCorrectActionToUser();
                        return true;
                    }
                }
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

            ResultDataPropertyGrowable resultParams = (ResultDataPropertyGrowable) value_;
            ResultDataProperty resultProperty 
                = new ResultDataProperty(nameForNewProperty, nameForNewProperty, String.Empty, resultParams.Mpm, resultParams.Subject, resultParams.Container);

            RenamingProperty renamingProperty 
                = new RenamingProperty(nameForNewProperty, resultProperty, resultParams.Mpm, resultParams.Subject, resultParams.Container);

            resultProperty.ChildrenProperties.Add(renamingProperty);
            resultParams.ChildrenProperties.Add(resultProperty);

            ListPropertyItem item = new ListPropertyItem(resultProperty);
            listBox1.Items.Add(item);
        }

        private void RemoveButton_Click(object sender, System.EventArgs e)
        {
            if(listBox1.CheckedItems == null)     return;
            if(listBox1.CheckedItems.Count == 0)  return;

            while(listBox1.CheckedItems.Count != 0)
            {
                object obj = listBox1.CheckedItems[0];
                ListPropertyItem resultVar = obj as ListPropertyItem;
                listBox1.Items.Remove(resultVar);
                removeProperty(value_ as ResultDataPropertyGrowable, resultVar.Property);
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
    }
}

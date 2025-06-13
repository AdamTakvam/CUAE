using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Metreos.Max.Core;
namespace Metreos.Max.Drawing
{
	/// <summary> 
	///     A form to allow the addition and removal of configuration enum entities.
	///     Those entities can then have string values added and removed to them.
	///     Essentially, it is a dialog to create enums, but to be used specifically
	///     with the configuration editor
	/// </summary>
	public class CustomConfigManager : System.Windows.Forms.Form
	{
        public ArrayList ConfigItems { get { return configItemsCollection; } }

        private System.Windows.Forms.Label configItemLabel;
        private System.Windows.Forms.Button addConfigButton;
        private System.Windows.Forms.Button addConfigValueButton;
        private System.Windows.Forms.Button removeConfigButton;
        private System.Windows.Forms.Button removeConfigValueButton;
        private System.Windows.Forms.Label configValueLabel;
        private System.Windows.Forms.Button updateButton;
        private ArrayList configItemsCollection;
        private System.Windows.Forms.ListBox configItemsList;
        private System.Windows.Forms.ListBox configValuesList;
        private EventHandler updateHandler;

		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;

        public CustomConfigManager(ArrayList configItemsCollection)
        {
            InitializeComponent();

            this.configItemsCollection                   = new ArrayList();
            this.configItemsList.SelectedIndexChanged   += new EventHandler(CurrentConfigItemChanged);
            this.addConfigButton.Click                  += new EventHandler(AddConfigItem);
            this.removeConfigButton.Click               += new EventHandler(RemoveConfigItem);
            this.addConfigValueButton.Click             += new EventHandler(AddConfigValueItem);
            this.removeConfigValueButton.Click          += new EventHandler(RemoveConfigValueItem);
            this.updateHandler                           = new EventHandler(Update);
            this.updateButton.Click                     += updateHandler;
            DialogResult                                 = DialogResult.Cancel;

            LoadDisplay(configItemsCollection);
        }

        public void LoadDisplay(ArrayList configItemsCollection)
        {
            if(configItemsCollection != null)
            {
                Copy(configItemsCollection);
            }

            LoadItemList();
        }

        private void Copy(ArrayList toCopy)
        {
            foreach(UserConfigType userConfig in toCopy)
            {
                configItemsCollection.Add(userConfig.Copy());
            }

        }

        private void LoadItemList()
        {
            configItemsList.Items.Clear();
            
            foreach(UserConfigType customConfig in configItemsCollection)
            {
                configItemsList.Items.Add(customConfig);
            }

            if(configItemsList.Items.Count > 0)
            {
                configItemsList.SelectedIndex = 0;
                SyncValuesToConfigItem(configItemsList.Items[0] as UserConfigType);
            }
        }

        private void ClearValues()
        {
            configValuesList.Items.Clear();
        }

        private void SyncValuesToConfigItem(UserConfigType configItem)
        {
            string[] values = new string[configItem.Values.Count];
            configItem.Values.CopyTo(values, 0);
            
            configValuesList.Items.Clear();
            configValuesList.Items.AddRange(values);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            this.updateButton.Click -= updateHandler;

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.configItemLabel = new System.Windows.Forms.Label();
            this.addConfigButton = new System.Windows.Forms.Button();
            this.addConfigValueButton = new System.Windows.Forms.Button();
            this.removeConfigButton = new System.Windows.Forms.Button();
            this.removeConfigValueButton = new System.Windows.Forms.Button();
            this.configValueLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.configItemsList = new System.Windows.Forms.ListBox();
            this.configValuesList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // configItemLabel
            // 
            this.configItemLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.configItemLabel.Location = new System.Drawing.Point(2, 8);
            this.configItemLabel.Name = "configItemLabel";
            this.configItemLabel.Size = new System.Drawing.Size(120, 23);
            this.configItemLabel.TabIndex = 2;
            this.configItemLabel.Text = "Configuration Items";
            this.configItemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // addConfigButton
            // 
            this.addConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addConfigButton.Location = new System.Drawing.Point(8, 272);
            this.addConfigButton.Name = "addConfigButton";
            this.addConfigButton.Size = new System.Drawing.Size(56, 23);
            this.addConfigButton.TabIndex = 4;
            this.addConfigButton.Text = "Add";
            // 
            // addConfigValueButton
            // 
            this.addConfigValueButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addConfigValueButton.Location = new System.Drawing.Point(160, 272);
            this.addConfigValueButton.Name = "addConfigValueButton";
            this.addConfigValueButton.Size = new System.Drawing.Size(56, 23);
            this.addConfigValueButton.TabIndex = 6;
            this.addConfigValueButton.Text = "Add";
            // 
            // removeConfigButton
            // 
            this.removeConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.removeConfigButton.Location = new System.Drawing.Point(72, 272);
            this.removeConfigButton.Name = "removeConfigButton";
            this.removeConfigButton.Size = new System.Drawing.Size(56, 23);
            this.removeConfigButton.TabIndex = 7;
            this.removeConfigButton.Text = "Remove";
            // 
            // removeConfigValueButton
            // 
            this.removeConfigValueButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.removeConfigValueButton.Location = new System.Drawing.Point(224, 272);
            this.removeConfigValueButton.Name = "removeConfigValueButton";
            this.removeConfigValueButton.Size = new System.Drawing.Size(56, 23);
            this.removeConfigValueButton.TabIndex = 8;
            this.removeConfigValueButton.Text = "Remove";
            // 
            // configValueLabel
            // 
            this.configValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.configValueLabel.Location = new System.Drawing.Point(151, 8);
            this.configValueLabel.Name = "configValueLabel";
            this.configValueLabel.Size = new System.Drawing.Size(136, 23);
            this.configValueLabel.TabIndex = 9;
            this.configValueLabel.Text = "Configuration Item Values";
            this.configValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // updateButton
            // 
            this.updateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.updateButton.Location = new System.Drawing.Point(104, 304);
            this.updateButton.Name = "updateButton";
            this.updateButton.TabIndex = 10;
            this.updateButton.Text = "Update";
            // 
            // configItemsList
            // 
            this.configItemsList.Location = new System.Drawing.Point(3, 32);
            this.configItemsList.Name = "configItemsList";
            this.configItemsList.Size = new System.Drawing.Size(133, 238);
            this.configItemsList.TabIndex = 11;
            // 
            // configValuesList
            // 
            this.configValuesList.Location = new System.Drawing.Point(152, 32);
            this.configValuesList.Name = "configValuesList";
            this.configValuesList.Size = new System.Drawing.Size(136, 238);
            this.configValuesList.TabIndex = 12;
            // 
            // CustomConfigManager
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 334);
            this.Controls.Add(this.configValuesList);
            this.Controls.Add(this.configItemsList);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.configValueLabel);
            this.Controls.Add(this.removeConfigValueButton);
            this.Controls.Add(this.removeConfigButton);
            this.Controls.Add(this.addConfigValueButton);
            this.Controls.Add(this.addConfigButton);
            this.Controls.Add(this.configItemLabel);
            this.Name = "CustomConfigManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration Manager";
            this.ResumeLayout(false);

        }
        #endregion

        private void CurrentConfigItemChanged(object sender, EventArgs e)
        {
            int i = configItemsList.SelectedIndex;
            if(i >= 0)
            {
                UserConfigType selectedItem = configItemsList.SelectedItem as UserConfigType;

                SyncValuesToConfigItem(selectedItem);
            }
            else
            {
                ClearValues();
            }
        }

        private void AddConfigItem(object sender, EventArgs e)
        {
            // Ask user for new name
            NewNameDlg newNameDlg = new NewNameDlg(Const.NewConfItemDlgTitle, Const.NewConfItemMsg);
            DialogResult result = newNameDlg.ShowDialog();

            if(result == DialogResult.OK)
            {
                string name = newNameDlg.NewName;

                if(IsNewConfigNameValid(name))
                {
                    UserConfigType newConfig = new UserConfigType(name);
                    configItemsList.Items.Add(newConfig);
                    configItemsList.SelectedIndex = configItemsList.Items.Count - 1;
                    configItemsCollection.Add(newConfig);
                }                
            }

            newNameDlg.Dispose();
        }

        private bool IsNewConfigNameValid(string name)
        {
            bool valid = (name != null && name != String.Empty);

            if(valid) // Check for duplicates if basic check validates
            {
                foreach(UserConfigType config in configItemsList.Items)
                {
                    if(config.IsSameName(name))
                    {
                        valid = false;
                    }
                }
            }

            return valid;
        }

        private void RemoveConfigItem(object sender, EventArgs e)
        {
            UserConfigType selectedConfig = configItemsList.SelectedItem as UserConfigType;
            int i = configItemsList.SelectedIndex;
            if(i >= 0)
            {
                configItemsList.Items.RemoveAt(i);
                configItemsCollection.Remove(selectedConfig);
            }

            // Since an item that was selected was removed, 
            // we should clear that item
             // ClearValues(); SelectedIndexChanged handler solves this
        }

        private void AddConfigValueItem(object sender, EventArgs e)
        {
            UserConfigType configItem;
            if(configItemsList.SelectedIndex < 0)
            {
                // Abort if there is no action configuration item 
                return;
            }
            else
            {
                configItem = configItemsList.SelectedItem as UserConfigType;
            }

            // Ask user for new name
            NewNameDlg newNameDlg = new NewNameDlg(Const.NewConfValueDlgTitle, Const.NewConfValueMsg);
            DialogResult result = newNameDlg.ShowDialog();

            if(result == DialogResult.OK)
            {
                string name = newNameDlg.NewName;

                if(IsNewConfigValueValid(name))
                {
                    configValuesList.Items.Add(name);
                    configItem.Values.Add(name);
                }
            }

            newNameDlg.Dispose();
        }

        private bool IsNewConfigValueValid(string newValue)
        {
            bool valid = (newValue != null && newValue != String.Empty);

            if(valid) // Check for duplicates if basic check validates
            {
                foreach(string current in configValuesList.Items)
                {
                    if(0 == String.Compare(current, newValue, true))
                    {
                        valid = false;
                    }
                }
            }

            return valid;
        }

        private void RemoveConfigValueItem(object sender, EventArgs e)
        {
            UserConfigType configItem;
            if(configItemsList.SelectedIndex < 0)
            {
                // Abort if there is no action configuration item 
                return;
            }
            else
            {
                configItem = configItemsList.SelectedItem as UserConfigType;
            }

            string name = configValuesList.SelectedItem as string;
            int selectedIndex = configValuesList.SelectedIndex;
            if(selectedIndex >= 0)
            {
                configValuesList.Items.RemoveAt(selectedIndex);
                configItem.Values.Remove(name);
            }
        }

        private void Update(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Controls;

namespace PagedRequest
{
	public class Form1 : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox hostBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox firstNameBox;
        private System.Windows.Forms.TextBox lastNameBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox usernameBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();

            UpdateSearchers(checkBox1.Checked);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.portBox = new System.Windows.Forms.TextBox();
            this.hostBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(64, 32);
            this.portBox.Name = "portBox";
            this.portBox.TabIndex = 0;
            this.portBox.Text = "389";
            // 
            // hostBox
            // 
            this.hostBox.Location = new System.Drawing.Point(64, 8);
            this.hostBox.Name = "hostBox";
            this.hostBox.TabIndex = 1;
            this.hostBox.Text = "192.168.1.151";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Host";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Location = new System.Drawing.Point(8, 136);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(360, 407);
            this.listBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(128, 552);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Make Request";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(168, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "firstName";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(168, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "lastName";
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(224, 8);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.TabIndex = 7;
            this.firstNameBox.Text = "";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(224, 32);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.TabIndex = 6;
            this.lastNameBox.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(224, 96);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Full Dump";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(64, 80);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.TabIndex = 11;
            this.passwordBox.Text = "metreos";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "Username";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(64, 56);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.TabIndex = 13;
            this.usernameBox.Text = "Directory Manager";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 589);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.firstNameBox);
            this.Controls.Add(this.lastNameBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hostBox);
            this.Controls.Add(this.portBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

        
        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateSearchers(checkBox1.Checked);   
        }

        private void UpdateSearchers(bool state)
        {
            if(state)
            {
                firstNameBox.Enabled = false;
                lastNameBox.Enabled = false;
            }
            else
            {
                firstNameBox.Enabled = true;
                lastNameBox.Enabled = true;
            }
        }

        private void button1_Click(object sender, System.EventArgs args)
        {
            try
            {
                bool result = PerformQuery(hostBox.Text, portBox.Text, usernameBox.Text, 
                    passwordBox.Text, firstNameBox.Text, lastNameBox.Text, checkBox1.Checked);

                if(result)
                {
                    listBox1.Items.Add("Query succeeded.");
                    listBox1.Items.Add("----------------");
                }
                else
                {
                    listBox1.Items.Add("Query failed.");
                    listBox1.Items.Add("-------------");
                }

            }
            catch(Exception e)
            {
                MessageBox.Show("Doh!" + System.Environment.NewLine + e.Message);
            }
        }

        private bool PerformQuery(string hostname, string port, string username, 
            string password, string firstName, string lastName, bool dump)
        {

            string login = CreateLogin(username);
            LdapConnection connection = new LdapConnection();
            
            connection.Connect(hostname, int.Parse(port));
            //connection.Bind(3, login, password);

            LdapControl[] requestControls = new LdapControl[2];
            LdapSortKey[] keys = new LdapSortKey[1];
            keys[0] = new LdapSortKey("cn");
            
            requestControls[0] = new LdapSortControl(keys, true);

            int beforeCount = 0;
            int afterCount = 32;

            requestControls[1]
                = new LdapVirtualListControl("cn=f*", beforeCount, afterCount);

            LdapConstraints constraints = connection.Constraints;

            constraints.setControls(requestControls);
            
            connection.Constraints = constraints;

            LdapSearchResults results
                = connection.Search("ou=people, dc=metreos, dc=com",
                                    LdapConnection.SCOPE_SUB,
                                    "objectclass=person", 
                                    null, 
                                    false);


            int rowCount = 0;

            while(results.hasMore())
            {
                rowCount++;

                LdapEntry entry;
                
                try
                {
                    entry = results.next();
                }
                catch(LdapException e)
                {
                    if(e is LdapReferralException)
                    {
                        continue;
                    }
                    else
                    {
                        MessageBox.Show("Doh in reading result entries!" );
                        continue;
                    }
                }

                LdapAttributeSet attributes = entry.getAttributeSet();

                StringBuilder builder = new StringBuilder(rowCount + ": ");
                
                if(attributes == null)
                {
                    listBox1.Items.Add("Null in attributes");
                }

                foreach(LdapAttribute attribute in attributes )
                {
                    builder.Append(attribute.Name + " = " + attribute.StringValue + ", "); 
                }

                if(attributes.Count != 0)
                {
                    builder.Remove(builder.Length - 3, 2);
                }

                listBox1.Items.Add(builder.ToString());
            }

            LdapControl[] controls = results.ResponseControls;

            if(controls == null)
            {
                MessageBox.Show("Controls retreival failed. *NOT* cool.");
                Failed();
                return false;
            }

            for(int i = 0; i < controls.Length; i++)
            {
                if(controls[i] is LdapSortResponse)
                {
                    LdapSortResponse sortResponse = controls[i] as LdapSortResponse;

                    if(sortResponse.FailedAttribute != null)
                    {
                        MessageBox.Show("Offending attribute: " + sortResponse.FailedAttribute);
                    }
                }
                else if(controls[i] is LdapVirtualListResponse)
                {
                    LdapVirtualListResponse pageResponse = controls[i] as LdapVirtualListResponse;

                    listBox1.Items.Add("First position = " + pageResponse.FirstPosition);
                    listBox1.Items.Add("Content count = " + pageResponse.ContentCount);
                    listBox1.Items.Add("Result code = " + pageResponse.ResultCode);
                    listBox1.Items.Add("Context = " + pageResponse.Context);


                }
            }


            if(connection.Connected)
            {
                connection.Disconnect();
            }

            return true;

        }

        private void Failed()
        {
            listBox1.Items.Add("Failed in request");
         }

        private string CreateLogin(string username)
        {
            return String.Format("cn={0}", username);
        }

        private static string CreateSearchString(string orgUnit)
        {
            return String.Format("ou={1}", orgUnit);
        }
	}
}

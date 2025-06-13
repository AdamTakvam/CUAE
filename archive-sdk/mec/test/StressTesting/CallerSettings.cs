using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace StressTesting
{
	/// <summary>
	/// Summary description for CallerSettings.
	/// </summary>
	public class CallerSettings : System.Windows.Forms.Form
	{
        private Regex numberRegEx = new Regex(@"\d+");
        private StressTesting.Settings settings;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox lowerBound;
        private System.Windows.Forms.TextBox upperBound;
        private System.Windows.Forms.TextBox callGenIp;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox rangeToDelete;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox call;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CallerSettings(StressTesting.Settings settings)
		{
            this.settings = settings;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			FillInValues();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lowerBound = new System.Windows.Forms.TextBox();
            this.upperBound = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.callGenIp = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.rangeToDelete = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.call = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.call.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.columnHeader1,
                                                                                        this.columnHeader2,
                                                                                        this.columnHeader3});
            this.listView1.Location = new System.Drawing.Point(8, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(224, 96);
            this.listView1.TabIndex = 2;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Range #";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Lower Bound";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Upper Bound";
            this.columnHeader3.Width = 78;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(104, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Current Ranges to Call";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(184, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "Add Range";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lowerBound
            // 
            this.lowerBound.Location = new System.Drawing.Point(8, 184);
            this.lowerBound.Name = "lowerBound";
            this.lowerBound.Size = new System.Drawing.Size(72, 20);
            this.lowerBound.TabIndex = 7;
            this.lowerBound.Text = "";
            // 
            // upperBound
            // 
            this.upperBound.Location = new System.Drawing.Point(96, 184);
            this.upperBound.Name = "upperBound";
            this.upperBound.Size = new System.Drawing.Size(72, 20);
            this.upperBound.TabIndex = 8;
            this.upperBound.Text = "";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(8, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Lower Bound";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(96, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Upper Bound";
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 24);
            this.label6.TabIndex = 12;
            this.label6.Text = "Range Manipulation";
            // 
            // callGenIp
            // 
            this.callGenIp.Location = new System.Drawing.Point(8, 40);
            this.callGenIp.Name = "callGenIp";
            this.callGenIp.Size = new System.Drawing.Size(120, 20);
            this.callGenIp.TabIndex = 15;
            this.callGenIp.Text = "";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "CallGen Ip";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(80, 488);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 24);
            this.button2.TabIndex = 17;
            this.button2.Text = "Apply";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(160, 488);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 24);
            this.button3.TabIndex = 18;
            this.button3.Text = "Cancel";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(176, 232);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 24);
            this.button4.TabIndex = 19;
            this.button4.Text = "Remove Range";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // rangeToDelete
            // 
            this.rangeToDelete.Location = new System.Drawing.Point(96, 232);
            this.rangeToDelete.Name = "rangeToDelete";
            this.rangeToDelete.Size = new System.Drawing.Size(72, 20);
            this.rangeToDelete.TabIndex = 20;
            this.rangeToDelete.Text = "";
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label9.Location = new System.Drawing.Point(88, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "Range To Delete";
            // 
            // checkBox1
            // 
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBox1.Location = new System.Drawing.Point(8, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(136, 32);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "Route calls to CallGen through CallManager";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 64);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 20);
            this.textBox1.TabIndex = 23;
            this.textBox1.Text = "";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "CallManager routing number";
            // 
            // call
            // 
            this.call.Controls.Add(this.checkBox1);
            this.call.Controls.Add(this.textBox1);
            this.call.Controls.Add(this.label3);
            this.call.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.call.Location = new System.Drawing.Point(8, 72);
            this.call.Name = "call";
            this.call.Size = new System.Drawing.Size(264, 96);
            this.call.TabIndex = 25;
            this.call.TabStop = false;
            this.call.Text = "CallManager Routing";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.call);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.callGenIp);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 304);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 176);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CallGen Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.rangeToDelete);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Controls.Add(this.lowerBound);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.upperBound);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(8, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 272);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SimClient Settings";
            // 
            // CallerSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 517);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "CallerSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CallerSettings";
            this.call.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        public void FillInValues()
        {
            for(int i = 0; i < settings.numberOfRanges; i++)
            {
                listView1.Items.Add(new ListViewItem(new string[] {
                                                                      i.ToString(),
                                                                      settings.lowerBounds[i],
                                                                      settings.upperBounds[i]
                                                                  }, -1));
            }
            

            callGenIp.Text = settings.callGenIp;
            textBox1.Text = settings.routingNumber;
            checkBox1.Checked = settings.allowRoute;

            textBox1.Enabled = checkBox1.Checked;
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            this.Dispose(true);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            listView1.Items.Add(new ListViewItem(new string[] {
                                                                  (listView1.Items.Count + 1).ToString(),
                                                                  lowerBound.Text,
                                                                  upperBound.Text
                                                              }, -1));
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            FileStream file;
            try
            {
                file = new FileStream("CallerSettings.config", FileMode.Create);
            }
            catch
            {
                MessageBox.Show("Directory can not be found for settings file.  Reinstall necessary");
                return;
            }
          
            XmlDocument doc = new XmlDocument();

            XmlNode xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement configurationTag = doc.CreateElement("configuration");
            doc.AppendChild(configurationTag);
            configurationTag.InnerText = "\n\t";

            XmlElement appSettingsTag = doc.CreateElement("appSettings");
            configurationTag.AppendChild(appSettingsTag);
            appSettingsTag.InnerText = "\n\t\t";

          
            // callGenIp Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "callGenIp", callGenIp.Text); 

            // allowRoute Creation
            if(checkBox1.Checked)
            {
                CreateKeyValueAttributes(ref doc, ref appSettingsTag, "allowRoute", "1"); 
            }
            else
            {
                CreateKeyValueAttributes(ref doc, ref appSettingsTag, "allowRoute", "0"); 
            }

            // routingNumber Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "routingNumber", textBox1.Text); 

            // ranges Creation
            StringBuilder range = new StringBuilder();

            System.Windows.Forms.ListView.ListViewItemCollection items = listView1.Items;
            for(int i = 0; i < items.Count; i++)
            {
                ListViewItem item = items[i];

                ListViewItem.ListViewSubItem subItemOne = item.SubItems[1];
                ListViewItem.ListViewSubItem subItemTwo = item.SubItems[2];
                

                range.Append(subItemOne.Text + ':'+ subItemTwo.Text + ';');

            }

            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "rangesList", range.ToString());
      
            
            // Overwrite old settings file

            try
            {
                StreamWriter writer = new StreamWriter(file);
                writer.Write(doc.InnerXml.ToString());

                writer.Close();
                file.Close();
            }
            catch
            {
                Console.Write(e.ToString());
            }
            settings.Refresh();

            this.Dispose(true);
        }

        public void CreateKeyValueAttributes(ref XmlDocument doc, ref XmlElement appSettingsTag, string keyValue, string valueValue)
        {
            XmlElement addTag = doc.CreateElement("add");
            addTag.IsEmpty = true;
            XmlAttributeCollection attributesForAddTag = addTag.Attributes;

            XmlAttribute keyAppServerIp = doc.CreateAttribute("key");
            keyAppServerIp.Value = keyValue;
            XmlAttribute valueAppServerIp = doc.CreateAttribute("value");
            valueAppServerIp.Value = valueValue;

            attributesForAddTag.Append(keyAppServerIp);
            attributesForAddTag.Append(valueAppServerIp);

            appSettingsTag.AppendChild(addTag);
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            try
            {
                if(numberRegEx.IsMatch(rangeToDelete.Text))
                {
                    ListView.ListViewItemCollection items = listView1.Items;

                    int positionToAdjustFrom = -1;

                    for(int i = 0; i < items.Count; i++)
                    {
                        if(Int32.Parse(rangeToDelete.Text) == i)
                        {
                            items.RemoveAt(i);
                            positionToAdjustFrom = i;
                            break;
                        }
                    }        

                    if(positionToAdjustFrom != -1)
                    {
                        for(int i = 0; i < items.Count; i++)
                        {
                            if(positionToAdjustFrom <= i)
                            {
                                ListViewItem item = items[i];
                                int decimalRepresentationOfItem = System.Int32.Parse(item.Text);
                                decimalRepresentationOfItem--;
                                item.Text = decimalRepresentationOfItem.ToString();
                            }
                        }
                    }                 
                }
                else
                {
                    // do alot of nothing
                }
            }
            catch
            {
                //MessageBox.Show(ea.ToString());
            }
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                this.textBox1.Enabled = true;
            }
            else
            {
                this.textBox1.Enabled = false;
            }
        }      
	}
}

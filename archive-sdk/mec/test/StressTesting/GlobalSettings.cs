using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StressTesting
{
    /// <summary>
    /// Summary description for GlobalSettings.
    /// </summary>
    public class GlobalSettings : System.Windows.Forms.Form
    {     
        private StressTesting.Settings settings;

        internal System.Windows.Forms.Label globalSettings_ApplicationServerIp_Label;
        private System.Windows.Forms.TextBox globalSetting_ApplicationServerIp;
        private System.Windows.Forms.Button globalSettings_Done_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox createPoll;
        private System.Windows.Forms.TextBox createTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox joinTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox joinPoll;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox kickTimeout;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox kickPoll;
        private System.Windows.Forms.TextBox textBox2;
        internal System.Windows.Forms.Label label8;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public GlobalSettings(StressTesting.Settings settings)
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
            this.globalSettings_ApplicationServerIp_Label = new System.Windows.Forms.Label();
            this.globalSetting_ApplicationServerIp = new System.Windows.Forms.TextBox();
            this.globalSettings_Done_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.createPoll = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.createTimeout = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.joinTimeout = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.joinPoll = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.kickTimeout = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.kickPoll = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // globalSettings_ApplicationServerIp_Label
            // 
            this.globalSettings_ApplicationServerIp_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.globalSettings_ApplicationServerIp_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.globalSettings_ApplicationServerIp_Label.Location = new System.Drawing.Point(0, 8);
            this.globalSettings_ApplicationServerIp_Label.Name = "globalSettings_ApplicationServerIp_Label";
            this.globalSettings_ApplicationServerIp_Label.Size = new System.Drawing.Size(136, 24);
            this.globalSettings_ApplicationServerIp_Label.TabIndex = 1;
            this.globalSettings_ApplicationServerIp_Label.Text = "Application Server IP";
            this.globalSettings_ApplicationServerIp_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // globalSetting_ApplicationServerIp
            // 
            this.globalSetting_ApplicationServerIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.globalSetting_ApplicationServerIp.Location = new System.Drawing.Point(144, 8);
            this.globalSetting_ApplicationServerIp.Name = "globalSetting_ApplicationServerIp";
            this.globalSetting_ApplicationServerIp.Size = new System.Drawing.Size(88, 22);
            this.globalSetting_ApplicationServerIp.TabIndex = 2;
            this.globalSetting_ApplicationServerIp.Text = "";
            // 
            // globalSettings_Done_Button
            // 
            this.globalSettings_Done_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.globalSettings_Done_Button.Location = new System.Drawing.Point(128, 464);
            this.globalSettings_Done_Button.Name = "globalSettings_Done_Button";
            this.globalSettings_Done_Button.Size = new System.Drawing.Size(56, 24);
            this.globalSettings_Done_Button.TabIndex = 9;
            this.globalSettings_Done_Button.Text = "Cancel";
            this.globalSettings_Done_Button.Click += new System.EventHandler(this.globalSettings_Done_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 40);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Type:";
            // 
            // radioButton2
            // 
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButton2.Location = new System.Drawing.Point(112, 16);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "CallGen";
            // 
            // radioButton1
            // 
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButton1.Location = new System.Drawing.Point(16, 16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(104, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "SimClient";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(64, 464);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 24);
            this.button1.TabIndex = 11;
            this.button1.Text = "Apply";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "CallManager IP";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(144, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(88, 22);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBox1.Location = new System.Drawing.Point(24, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(136, 24);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Error Checking On";
            // 
            // createPoll
            // 
            this.createPoll.Location = new System.Drawing.Point(8, 24);
            this.createPoll.Name = "createPoll";
            this.createPoll.Size = new System.Drawing.Size(80, 20);
            this.createPoll.TabIndex = 15;
            this.createPoll.Text = "5000";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(96, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "Polling Interval";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(8, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 296);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Error Checking";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.createTimeout);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.createPoll);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox5.Location = new System.Drawing.Point(24, 48);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(184, 72);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Create Conference";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(96, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Timeout";
            // 
            // createTimeout
            // 
            this.createTimeout.Location = new System.Drawing.Point(8, 48);
            this.createTimeout.Name = "createTimeout";
            this.createTimeout.Size = new System.Drawing.Size(80, 20);
            this.createTimeout.TabIndex = 17;
            this.createTimeout.Text = "15000";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.joinTimeout);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.joinPoll);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(24, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(184, 72);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Join Conference";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(96, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Timeout";
            // 
            // joinTimeout
            // 
            this.joinTimeout.Location = new System.Drawing.Point(8, 48);
            this.joinTimeout.Name = "joinTimeout";
            this.joinTimeout.Size = new System.Drawing.Size(80, 20);
            this.joinTimeout.TabIndex = 17;
            this.joinTimeout.Text = "15000";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(96, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Polling Interval";
            // 
            // joinPoll
            // 
            this.joinPoll.Location = new System.Drawing.Point(8, 24);
            this.joinPoll.Name = "joinPoll";
            this.joinPoll.Size = new System.Drawing.Size(80, 20);
            this.joinPoll.TabIndex = 15;
            this.joinPoll.Text = "5000";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.kickTimeout);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.kickPoll);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Location = new System.Drawing.Point(24, 208);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(184, 72);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Kick Conference";
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Location = new System.Drawing.Point(96, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 18;
            this.label6.Text = "Timeout";
            // 
            // kickTimeout
            // 
            this.kickTimeout.Location = new System.Drawing.Point(8, 48);
            this.kickTimeout.Name = "kickTimeout";
            this.kickTimeout.Size = new System.Drawing.Size(80, 20);
            this.kickTimeout.TabIndex = 17;
            this.kickTimeout.Text = "15000";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label7.Location = new System.Drawing.Point(96, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Polling Interval";
            // 
            // kickPoll
            // 
            this.kickPoll.Location = new System.Drawing.Point(8, 24);
            this.kickPoll.Name = "kickPoll";
            this.kickPoll.Size = new System.Drawing.Size(80, 20);
            this.kickPoll.TabIndex = 15;
            this.kickPoll.Text = "5000";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(144, 72);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(88, 22);
            this.textBox2.TabIndex = 19;
            this.textBox2.Text = "";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 24);
            this.label8.TabIndex = 18;
            this.label8.Text = "Database Resource";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GlobalSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(240, 493);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.globalSetting_ApplicationServerIp);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.globalSettings_Done_Button);
            this.Controls.Add(this.globalSettings_ApplicationServerIp_Label);
            this.Name = "GlobalSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GlobalSettings";
            this.Load += new System.EventHandler(this.GlobalSettings_Load);
            this.Closed += new System.EventHandler(this.GlobalSettings_Closed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void globalSettings_Done_Button_Click(object sender, System.EventArgs e)
        {
            this.Dispose(true);
        }

        private void GlobalSettings_Load(object sender, System.EventArgs e)
        {
        
        }

        private void GlobalSettings_Closed(object sender, System.EventArgs e)
        {
            this.Dispose(true);
        }

        public void FillInValues()
        {
            this.textBox1.Text = settings.callManagerIp;
            this.globalSetting_ApplicationServerIp.Text = settings.appServerIp;
            this.textBox2.Text = settings.databaseName;
            this.radioButton1.Checked = settings.chooseSim;
            this.radioButton2.Checked = settings.chooseCall;
            this.checkBox1.Checked = settings.errorChecking;
            this.createPoll.Text = settings.createPoll;
            this.createTimeout.Text = settings.createTimeout;
            this.joinPoll.Text = settings.joinPoll;
            this.joinTimeout.Text = settings.joinTimeout;
            this.kickPoll.Text = settings.kickPoll;
            this.kickTimeout.Text = settings.kickTimeout;
        }

        // Saving settings
        private void button1_Click(object sender, System.EventArgs e)
        {
            FileStream file;
            try
            {
                file = new FileStream("Globals.config", FileMode.Create);
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

          
            // appServerIp Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "appServerIp", globalSetting_ApplicationServerIp.Text); 
            
            // radioSelectSimCal Creation
            string whichSelected;

            if(radioButton1.Checked == true)
            {
                whichSelected = "0";
            }
            else if(radioButton2.Checked == true)
            {
                whichSelected = "1";
            }
            else
            {
                whichSelected = "0";
            }

            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "radioSelectSimCall", whichSelected);
      
            // callManagerIp Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "callManagerIp", textBox1.Text);

            // databaseName creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "databaseName", textBox2.Text);
            
            // errorChecking Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "errorChecking", checkBox1.Checked.ToString());

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "createPoll", createPoll.Text);

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "createTimeout", createTimeout.Text);

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "joinPoll", joinPoll.Text);

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "joinTimeout", joinTimeout.Text);

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "kickPoll", kickPoll.Text);

            // initialPause Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "kickTimeout", kickTimeout.Text);



            try
            {
                // Overwrite old settings file
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
    }
}

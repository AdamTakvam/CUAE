using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for TestSettings.
	/// </summary>
	public class TestSettings : System.Windows.Forms.Form
	{
        private StressTesting.Settings settings;
        public System.Windows.Forms.Label randomizedConference_Label;
        public System.Windows.Forms.Panel randomizedConference_Particular_Panel;
        public System.Windows.Forms.TextBox randomizedConference_Particular_AveragePeriod;
        public System.Windows.Forms.Label randomizedConference_Particular_ConnectionIntensity_Label;
        public System.Windows.Forms.TrackBar randomizedConference_Particular_ConnectionIntensity;
        public System.Windows.Forms.TextBox randomizedConference_Particular_DurationTest;
        public System.Windows.Forms.Label randomizedConference_Particular_DurationTest_Label;
        public System.Windows.Forms.Label randomizedConference_Particular_PeriodBetweenCalls_Label;
        public System.Windows.Forms.TextBox randomizedConference_Particular_PeriodBetweenCalls;
        public System.Windows.Forms.TextBox randomizedConference_Particular_MaximumConferences;
        public System.Windows.Forms.Label randomizedConference_Particular_MaximumConferences_Label;
        public System.Windows.Forms.TextBox randomizedConference_Particular_MaximumConnections;
        public System.Windows.Forms.Label randomizedConference_Particular_MaximumConnections_Label;
        public System.Windows.Forms.Label randomizedConference_Particular_AveragePeriod_Label;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox randomizedConference_Particular_AverageSpike;
        public System.Windows.Forms.Label randomizedConference_Particular_SpikeAverage_Label;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestSettings(StressTesting.Settings settings)
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
            this.randomizedConference_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_Panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_AverageSpike = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_SpikeAverage_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_AveragePeriod = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_ConnectionIntensity_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_ConnectionIntensity = new System.Windows.Forms.TrackBar();
            this.randomizedConference_Particular_DurationTest = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_DurationTest_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_PeriodBetweenCalls_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_PeriodBetweenCalls = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_MaximumConferences = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_MaximumConferences_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_MaximumConnections = new System.Windows.Forms.TextBox();
            this.randomizedConference_Particular_MaximumConnections_Label = new System.Windows.Forms.Label();
            this.randomizedConference_Particular_AveragePeriod_Label = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.randomizedConference_Particular_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomizedConference_Particular_ConnectionIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // randomizedConference_Label
            // 
            this.randomizedConference_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.randomizedConference_Label.Location = new System.Drawing.Point(8, 8);
            this.randomizedConference_Label.Name = "randomizedConference_Label";
            this.randomizedConference_Label.Size = new System.Drawing.Size(272, 24);
            this.randomizedConference_Label.TabIndex = 4;
            this.randomizedConference_Label.Text = "Test Settings";
            this.randomizedConference_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // randomizedConference_Particular_Panel
            // 
            this.randomizedConference_Particular_Panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.randomizedConference_Particular_Panel.Controls.Add(this.textBox1);
            this.randomizedConference_Particular_Panel.Controls.Add(this.label1);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_AverageSpike);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_SpikeAverage_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_AveragePeriod);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_ConnectionIntensity_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_ConnectionIntensity);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_DurationTest);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_DurationTest_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_PeriodBetweenCalls_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_PeriodBetweenCalls);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_MaximumConferences);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_MaximumConferences_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_MaximumConnections);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_MaximumConnections_Label);
            this.randomizedConference_Particular_Panel.Controls.Add(this.randomizedConference_Particular_AveragePeriod_Label);
            this.randomizedConference_Particular_Panel.Location = new System.Drawing.Point(32, 40);
            this.randomizedConference_Particular_Panel.Name = "randomizedConference_Particular_Panel";
            this.randomizedConference_Particular_Panel.Size = new System.Drawing.Size(200, 400);
            this.randomizedConference_Particular_Panel.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(24, 296);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(144, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "60";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(24, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 32);
            this.label1.TabIndex = 25;
            this.label1.Text = "Average Spike Interval (seconds)";
            // 
            // randomizedConference_Particular_AverageSpike
            // 
            this.randomizedConference_Particular_AverageSpike.Location = new System.Drawing.Point(24, 240);
            this.randomizedConference_Particular_AverageSpike.Name = "randomizedConference_Particular_AverageSpike";
            this.randomizedConference_Particular_AverageSpike.Size = new System.Drawing.Size(144, 20);
            this.randomizedConference_Particular_AverageSpike.TabIndex = 6;
            this.randomizedConference_Particular_AverageSpike.Text = "60";
            // 
            // randomizedConference_Particular_SpikeAverage_Label
            // 
            this.randomizedConference_Particular_SpikeAverage_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_SpikeAverage_Label.Location = new System.Drawing.Point(24, 208);
            this.randomizedConference_Particular_SpikeAverage_Label.Name = "randomizedConference_Particular_SpikeAverage_Label";
            this.randomizedConference_Particular_SpikeAverage_Label.Size = new System.Drawing.Size(144, 32);
            this.randomizedConference_Particular_SpikeAverage_Label.TabIndex = 23;
            this.randomizedConference_Particular_SpikeAverage_Label.Text = "Average Create/Destroy Interval (seconds)";
            // 
            // randomizedConference_Particular_AveragePeriod
            // 
            this.randomizedConference_Particular_AveragePeriod.Location = new System.Drawing.Point(24, 184);
            this.randomizedConference_Particular_AveragePeriod.Name = "randomizedConference_Particular_AveragePeriod";
            this.randomizedConference_Particular_AveragePeriod.Size = new System.Drawing.Size(144, 20);
            this.randomizedConference_Particular_AveragePeriod.TabIndex = 5;
            this.randomizedConference_Particular_AveragePeriod.Text = "11";
            // 
            // randomizedConference_Particular_ConnectionIntensity_Label
            // 
            this.randomizedConference_Particular_ConnectionIntensity_Label.Location = new System.Drawing.Point(32, 320);
            this.randomizedConference_Particular_ConnectionIntensity_Label.Name = "randomizedConference_Particular_ConnectionIntensity_Label";
            this.randomizedConference_Particular_ConnectionIntensity_Label.Size = new System.Drawing.Size(152, 16);
            this.randomizedConference_Particular_ConnectionIntensity_Label.TabIndex = 21;
            this.randomizedConference_Particular_ConnectionIntensity_Label.Text = "Initial Test Intensity";
            // 
            // randomizedConference_Particular_ConnectionIntensity
            // 
            this.randomizedConference_Particular_ConnectionIntensity.Location = new System.Drawing.Point(24, 336);
            this.randomizedConference_Particular_ConnectionIntensity.Maximum = 21;
            this.randomizedConference_Particular_ConnectionIntensity.Minimum = 1;
            this.randomizedConference_Particular_ConnectionIntensity.Name = "randomizedConference_Particular_ConnectionIntensity";
            this.randomizedConference_Particular_ConnectionIntensity.Size = new System.Drawing.Size(152, 42);
            this.randomizedConference_Particular_ConnectionIntensity.TabIndex = 8;
            this.randomizedConference_Particular_ConnectionIntensity.Value = 21;
            // 
            // randomizedConference_Particular_DurationTest
            // 
            this.randomizedConference_Particular_DurationTest.Location = new System.Drawing.Point(24, 72);
            this.randomizedConference_Particular_DurationTest.Name = "randomizedConference_Particular_DurationTest";
            this.randomizedConference_Particular_DurationTest.Size = new System.Drawing.Size(144, 20);
            this.randomizedConference_Particular_DurationTest.TabIndex = 3;
            this.randomizedConference_Particular_DurationTest.Text = "1";
            // 
            // randomizedConference_Particular_DurationTest_Label
            // 
            this.randomizedConference_Particular_DurationTest_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_DurationTest_Label.Location = new System.Drawing.Point(24, 56);
            this.randomizedConference_Particular_DurationTest_Label.Name = "randomizedConference_Particular_DurationTest_Label";
            this.randomizedConference_Particular_DurationTest_Label.Size = new System.Drawing.Size(152, 16);
            this.randomizedConference_Particular_DurationTest_Label.TabIndex = 18;
            this.randomizedConference_Particular_DurationTest_Label.Text = "Test Time (minutes)";
            // 
            // randomizedConference_Particular_PeriodBetweenCalls_Label
            // 
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.Location = new System.Drawing.Point(24, 96);
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.Name = "randomizedConference_Particular_PeriodBetweenCalls_Label";
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.Size = new System.Drawing.Size(152, 32);
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.TabIndex = 13;
            this.randomizedConference_Particular_PeriodBetweenCalls_Label.Text = "Minimum Time Between Calls/Hangups (seconds)";
            // 
            // randomizedConference_Particular_PeriodBetweenCalls
            // 
            this.randomizedConference_Particular_PeriodBetweenCalls.AcceptsReturn = true;
            this.randomizedConference_Particular_PeriodBetweenCalls.Location = new System.Drawing.Point(24, 128);
            this.randomizedConference_Particular_PeriodBetweenCalls.Name = "randomizedConference_Particular_PeriodBetweenCalls";
            this.randomizedConference_Particular_PeriodBetweenCalls.Size = new System.Drawing.Size(144, 20);
            this.randomizedConference_Particular_PeriodBetweenCalls.TabIndex = 4;
            this.randomizedConference_Particular_PeriodBetweenCalls.Text = "10";
            // 
            // randomizedConference_Particular_MaximumConferences
            // 
            this.randomizedConference_Particular_MaximumConferences.Location = new System.Drawing.Point(24, 32);
            this.randomizedConference_Particular_MaximumConferences.Name = "randomizedConference_Particular_MaximumConferences";
            this.randomizedConference_Particular_MaximumConferences.Size = new System.Drawing.Size(64, 20);
            this.randomizedConference_Particular_MaximumConferences.TabIndex = 1;
            this.randomizedConference_Particular_MaximumConferences.Text = "4";
            // 
            // randomizedConference_Particular_MaximumConferences_Label
            // 
            this.randomizedConference_Particular_MaximumConferences_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_MaximumConferences_Label.Location = new System.Drawing.Point(24, 16);
            this.randomizedConference_Particular_MaximumConferences_Label.Name = "randomizedConference_Particular_MaximumConferences_Label";
            this.randomizedConference_Particular_MaximumConferences_Label.Size = new System.Drawing.Size(72, 16);
            this.randomizedConference_Particular_MaximumConferences_Label.TabIndex = 10;
            this.randomizedConference_Particular_MaximumConferences_Label.Text = "Max # Conf\'s";
            // 
            // randomizedConference_Particular_MaximumConnections
            // 
            this.randomizedConference_Particular_MaximumConnections.Location = new System.Drawing.Point(104, 32);
            this.randomizedConference_Particular_MaximumConnections.Name = "randomizedConference_Particular_MaximumConnections";
            this.randomizedConference_Particular_MaximumConnections.Size = new System.Drawing.Size(64, 20);
            this.randomizedConference_Particular_MaximumConnections.TabIndex = 2;
            this.randomizedConference_Particular_MaximumConnections.Text = "8";
            // 
            // randomizedConference_Particular_MaximumConnections_Label
            // 
            this.randomizedConference_Particular_MaximumConnections_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_MaximumConnections_Label.Location = new System.Drawing.Point(104, 16);
            this.randomizedConference_Particular_MaximumConnections_Label.Name = "randomizedConference_Particular_MaximumConnections_Label";
            this.randomizedConference_Particular_MaximumConnections_Label.Size = new System.Drawing.Size(88, 16);
            this.randomizedConference_Particular_MaximumConnections_Label.TabIndex = 8;
            this.randomizedConference_Particular_MaximumConnections_Label.Text = "Max # of Conn\'s";
            // 
            // randomizedConference_Particular_AveragePeriod_Label
            // 
            this.randomizedConference_Particular_AveragePeriod_Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.randomizedConference_Particular_AveragePeriod_Label.Location = new System.Drawing.Point(24, 152);
            this.randomizedConference_Particular_AveragePeriod_Label.Name = "randomizedConference_Particular_AveragePeriod_Label";
            this.randomizedConference_Particular_AveragePeriod_Label.Size = new System.Drawing.Size(144, 32);
            this.randomizedConference_Particular_AveragePeriod_Label.TabIndex = 16;
            this.randomizedConference_Particular_AveragePeriod_Label.Text = "Average Call/Hangup Interval (seconds)";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(56, 456);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 24);
            this.button1.TabIndex = 9;
            this.button1.Text = "Apply";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(144, 456);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 24);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(264, 493);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.randomizedConference_Label);
            this.Controls.Add(this.randomizedConference_Particular_Panel);
            this.Name = "TestSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestSettings";
            this.randomizedConference_Particular_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.randomizedConference_Particular_ConnectionIntensity)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Dispose(true);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            FileStream file;

            try
            {
                file = new FileStream("TestSettings.config", FileMode.Create);
            }
            catch(DirectoryNotFoundException)
            {
                MessageBox.Show("Directory can not be found for settings file.  Reinstall necessary");
                return;
            }
            catch
            {
                MessageBox.Show("Unexpected lock on TestSettings.config");
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

          
            // maximumConferences Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "maximumConferences", this.randomizedConference_Particular_MaximumConferences.Text); 
            
            // maximumConnections Creation 
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "maximumConnections", this.randomizedConference_Particular_MaximumConnections.Text); 
            
            // testTime Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "testTime", this.randomizedConference_Particular_DurationTest.Text); 
      
            // minimumTimeBetween Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "minimumTimeBetweenCalls", this.randomizedConference_Particular_PeriodBetweenCalls.Text);
 
            // averageCallInterval Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "averageCallInterval", this.randomizedConference_Particular_AveragePeriod.Text); 
            
            // averageSpikeInterval Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "averageSpike", this.randomizedConference_Particular_AverageSpike.Text); 

            // averageLittleSpikeInterval Creation
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "averageLittleSpike", this.textBox1.Text); 

            // initialIntensity
            CreateKeyValueAttributes(ref doc, ref appSettingsTag, "initialIntensity", this.randomizedConference_Particular_ConnectionIntensity.Value.ToString()); 

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

        public void FillInValues()
        {
            this.randomizedConference_Particular_AveragePeriod.Text = settings.averageCallInterval.ToString();
            this.randomizedConference_Particular_AverageSpike.Text = settings.averageSpike.ToString();
            this.randomizedConference_Particular_ConnectionIntensity.Value = settings.initialIntensity;
            this.randomizedConference_Particular_DurationTest.Text = settings.testTime.ToString();
            this.randomizedConference_Particular_MaximumConferences.Text = settings.maximumConferences.ToString();
            this.randomizedConference_Particular_MaximumConnections.Text = settings.maximumConnections.ToString();
            this.randomizedConference_Particular_PeriodBetweenCalls.Text = settings.minimumTimeBetweenCalls.ToString();
            this.textBox1.Text = settings.averageLittleSpike.ToString();
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

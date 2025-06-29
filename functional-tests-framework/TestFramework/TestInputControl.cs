using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for TestInputControl.
	/// </summary>
	public class TestInputControl : System.Windows.Forms.UserControl
	{
        private const int START_X               = 20;
        private const int START_Y               = 20;
        private const int VERTICAL_SPACING      = 8;
        private const int HORIZ_SPACING         = 10;
        private const int RADIO_BUTTON_DIST     = 56;

        /// <summary>
        /// An array list of TestInputData objects, or windows controls that will be
        /// used to render the user input control area.
        /// </summary>
        private ArrayList testInputs;

		/// <summary>
		/// Called every time the value of avariable is changed.
		/// </summary>
		private CommonTypes.UpdateTestSettings updateTestSettings;
        /// <summary>
        /// A hashtable that holds the current values of the
        /// current user input index by variable name.
        /// </summary>
        private Hashtable values;

        private System.Windows.Forms.ToolTip toolTip;
        private System.ComponentModel.IContainer components;

        public Hashtable Values { get { return values; } }

		public TestInputControl(ArrayList inputs, CommonTypes.UpdateTestSettings updateTestSettings)
		{
            values = new Hashtable();
            testInputs = new ArrayList();
			this.updateTestSettings = updateTestSettings;

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            Reset(inputs);
		}


        /// <summary>
        /// Resets the user control to a new set of dynamic inputs.
        /// </summary>
        /// <param name="inputs">Array list of TestInputData objects to use for rendering.</param>
        public void Reset(ArrayList inputs)
        {
            Debug.Assert(inputs != null);
            
            values.Clear();

            this.SuspendLayout();
            Controls.Clear();
            this.ResumeLayout();

            testInputs = inputs;

            LayoutTestInputs();
        }


        private void LayoutTestInputs()
        {
            Point labelPoint = new System.Drawing.Point(START_X, START_Y);

            this.SuspendLayout();

            int vertSize = 0;

            foreach(TestInputDataBase data in testInputs)
            {
                if(data is TestMultiLineTextInputData)
                {
                    vertSize = PlaceMultiLineStringInput(data as TestMultiLineTextInputData, labelPoint);
                }
                else if(data is TestTextInputData)
                {
                    vertSize = PlaceStringInput(data as TestTextInputData, labelPoint);
                }
                else if(data is TestBooleanInputData)
                {
                    vertSize = PlaceBooleanInput(data as TestBooleanInputData, labelPoint);
                }
                else if(data is TestOptionsInputData)
                {
                    vertSize = PlaceSelectOptionInput(data as TestOptionsInputData, labelPoint);
                }
                else if(data is TestUserEvent)
                {
                    vertSize = PlaceUserEventInput(data as TestUserEvent, labelPoint);
                } 
                else if(data is TestControl)
                {
                    vertSize = PlaceControl(data as TestControl, labelPoint);
                }

                labelPoint.Offset(0, vertSize + VERTICAL_SPACING);
            }

            this.ResumeLayout(false);
        }


        private int PlaceStringInput(TestTextInputData data, Point labelPoint)
        {
            // Set the default value.
            values[data.Variable] = data.DefaultValue;

            Label inputLabel = CreateInputLabel(data, ref labelPoint);

            data.TextBox.Location = labelPoint;
            data.TextBox.Validating += new CancelEventHandler(inputBox_Validating);
            data.TextBox.Tag = data;
            this.toolTip.SetToolTip(data.TextBox, data.Description);

            Controls.Add(inputLabel);
            Controls.Add(data.TextBox);

            return data.TextBox.Size.Height;
        }

        private int PlaceMultiLineStringInput(TestMultiLineTextInputData data, Point labelPoint)
        {
            // Set the default value.
            values[data.Variable] = data.DefaultValue;

            Label inputLabel = CreateInputLabel(data, ref labelPoint);
            
            data.TextBox.Location = labelPoint;
            data.TextBox.Validating += new CancelEventHandler(inputBox_Validating);
            data.TextBox.Tag = data;
            this.toolTip.SetToolTip(data.TextBox, data.Description);
           
            Controls.Add(inputLabel);
            Controls.Add(data.TextBox);

            return data.TextBox.Size.Height;
        }


        private int PlaceBooleanInput(TestBooleanInputData data, Point labelPoint)
        {
            int vertOffSet = 9;

            // Default for booleans set to false. If true is selected
            // by default then an event will fire and this will change.
            values[data.Variable] = false;
            
            // Offset the label by +9 pixels to accomodate for the extra
            // space that the group box needs.
            labelPoint.Offset(0, vertOffSet);

            Label inputLabel = CreateInputLabel(data, ref labelPoint);

            // Reset the location so that we can insert our group box.
            labelPoint.Offset(0, -vertOffSet);

            data.RadioBox.Location = labelPoint;
            labelPoint.Offset(5, 5);
            labelPoint.Offset(RADIO_BUTTON_DIST, 0);

            data.TrueRadio.CheckedChanged += new EventHandler(trueRadio_CheckedChanged);
            data.TrueRadio.Tag = data;
            this.toolTip.SetToolTip(data.RadioBox, data.Description);
            this.toolTip.SetToolTip(data.FalseRadio, data.Description);
            this.toolTip.SetToolTip(data.TrueRadio, data.Description);
            

            Controls.Add(inputLabel);
            Controls.Add(data.RadioBox);

            data.RadioBox.ResumeLayout();

            return data.RadioBox.Size.Height;
        }


        private int PlaceSelectOptionInput(TestOptionsInputData data, Point labelPoint)
        {
            Label inputLabel = CreateInputLabel(data, ref labelPoint);
            
            // Set the default value for our input data.
            values[data.Variable] = data.Options[0];

            data.ComboBox.Tag = data;
            data.ComboBox.Location = labelPoint;
            data.ComboBox.SelectedValueChanged += new EventHandler(comboBox_SelectedValueChanged);

            Controls.Add(inputLabel);
            Controls.Add(data.ComboBox);

            return data.ComboBox.Size.Height;
        }


        private int PlaceUserEventInput(TestUserEvent data, Point labelPoint)
        {
            Label inputLabel = CreateInputLabel(data, ref labelPoint);

            data.Button.Tag = data;
            data.Button.Location = labelPoint;
            data.Button.Click += new EventHandler(button_Click);

            Controls.Add(inputLabel);
            Controls.Add(data.Button);

            return data.Button.Size.Height;
        }

        private int PlaceControl(TestControl control, Point labelPoint)
        {
            Label inputLabel = CreateInputLabel(control, ref labelPoint);

            control.Control.Name = control.Variable;
            control.Control.Location = labelPoint;
            Controls.Add(inputLabel);
            Controls.Add(control.Control);

            return control.Control.Height;
        }
        

        private Label CreateInputLabel(TestInputDataBase data, ref Point labelPoint)
        {
            Label label = new Label();

            label.Location = labelPoint;
            label.Text = data.Prompt;
            label.Name = data.Prompt;
            label.TextAlign = ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(label, data.Description);

            labelPoint.Offset(label.Size.Width + HORIZ_SPACING, 0);

            return label;
        }

        #region IDisposable

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

        #endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            // 
            // TestInputControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "TestInputControl";
            this.Size = new System.Drawing.Size(640, 576);

        }
        #endregion

        #region Event Handlers

        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            values[comboBox.Name] = comboBox.SelectedText;
            TestInputDataBase input = comboBox.Tag as TestInputDataBase;
            input.SavedDefaultValue = comboBox.SelectedItem.ToString();
			UpdateTestSettings(comboBox.Tag as TestInputDataBase, comboBox.Name, comboBox.SelectedText); 
        }

        private void inputBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            values[textBox.Name] = textBox.Text;
            TestInputDataBase input = textBox.Tag as TestInputDataBase;
            input.SavedDefaultValue = textBox.Text;
            
			UpdateTestSettings(textBox.Tag as TestInputDataBase, textBox.Name, textBox.Text);
        }

        private void trueRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            values[radioButton.Name] = radioButton.Checked;
			TestInputDataBase input = radioButton.Tag as TestInputDataBase;
            input.SavedDefaultValue = radioButton.Checked.ToString();
            UpdateTestSettings(radioButton.Tag as TestInputDataBase, radioButton.Name, radioButton.Checked.ToString());
        }

		private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TestUserEvent userEvent = button.Tag as TestUserEvent;
            
            if(userEvent != null)
            {
                if(userEvent.CallBack != null)
                {
                    userEvent.CallBack(userEvent.Variable, "");
                }
            }
        }
        #endregion

		private void UpdateTestSettings(TestInputDataBase testData, string variableName, string variableValue)
		{
			this.updateTestSettings(testData.FullTestName, variableName, variableValue);
		}
    }
}

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Metreos.Samoa.FunctionalTestFramework
{
    /// <summary>
    /// Base class for all input types.
    /// </summary>
    public abstract class TestInputDataBase
    {
        protected string fullTestName;
        protected string testName;
        protected string variableName;
        protected string inputPrompt;
        protected string inputDescription;
        protected string defaultValue = "";
        protected string savedDefaultValue = "";

        public string SavedDefaultValue { set { savedDefaultValue = value; }}
        public string TestName{ get { return testName; } set{ testName = value; }}
        public string FullTestName { get { return fullTestName; } set { fullTestName = value; } }
        public string Prompt { get { return inputPrompt; } } 
        public string Description { get { return inputDescription; } }
        public string Variable { get { return variableName; } } 

        public TestInputDataBase(string prompt, string desc, string varName)
        {
            Debug.Assert(prompt != null, "prompt can not be null");
            Debug.Assert(varName != null, "varName can not be null");
            Debug.Assert(varName != "", "varName can not be empty");
            
            this.variableName = varName;
            this.inputPrompt = prompt;
            this.inputDescription = desc;
        }
    }


    /// <summary>
    /// Represents a single line text input area.
    /// </summary>
    public class TestTextInputData : TestInputDataBase
    {
        protected const int NUM_PIXELS_PER_LINE   = 20;
        protected TextBox inputBox;
        protected int textWidth = 200;
        protected int numLines = 1;
       
        public string DefaultValue { get { return savedDefaultValue == "" ? defaultValue : savedDefaultValue ; } } 
        public TextBox TextBox  { get { return inputBox; } }
		

        public int Width { get { return textWidth; } }

        public int NumLines { get { return numLines; } }

        public TestTextInputData(string prompt, string desc, string varName) :
            this(prompt, desc, varName, String.Empty, 200)
        {}


        public TestTextInputData(string prompt, string desc, string varName, int width) : 
            this(prompt, desc, varName, String.Empty, width)
        {
        }


        public TestTextInputData(string prompt, string desc, string varName, string defText, int width) : 
            base(prompt, desc, varName)
        {
            textWidth = width;
            defaultValue = defText;

            inputBox = new TextBox();
            inputBox.Name = varName;
            inputBox.Text = defText;
            inputBox.TextAlign = HorizontalAlignment.Left;
            inputBox.Multiline = false;
            inputBox.Size = new Size(width, NUM_PIXELS_PER_LINE * numLines);
        }
    }


    /// <summary>
    /// Represents a multi-line text input area.
    /// </summary>
    public class TestMultiLineTextInputData : TestTextInputData
    {
        public TestMultiLineTextInputData(string prompt, string desc, string varName, int width, string defText) : 
            base(prompt, desc, varName)
        {
            this.textWidth = width;
            this.defaultValue = defText;
            inputBox = new TextBox();
            inputBox.Name = varName;
            inputBox.Text = defText;
            inputBox.Size = new Size(width, NUM_PIXELS_PER_LINE * numLines);
            inputBox.TextAlign = HorizontalAlignment.Left;
            inputBox.Multiline = true;
        }
    }

    
    /// <summary>
    /// Represents an input area for true/false data.
    /// </summary>
    public class TestBooleanInputData : TestInputDataBase
    {
        protected bool defaultTrue = true;
        protected RadioButton trueRadio;
        protected RadioButton falseRadio;
        protected GroupBox radioBox;

        public  GroupBox RadioBox { get { return radioBox; } }
        public RadioButton TrueRadio { get { return trueRadio; } }
        public RadioButton FalseRadio { get { return falseRadio; } }

        public bool DefaultTrue
        {
            get 
            { 
                try
                {
                    if(this.savedDefaultValue != null)
                    {
                        object eu = Convert.ChangeType(this.savedDefaultValue, typeof(bool));
                        return System.Boolean.Parse(this.savedDefaultValue);
                    }
                }
                catch { }	

                return defaultTrue;
            } 
        }

        public TestBooleanInputData(string prompt, string desc, string varName) :
            this(prompt, desc, varName, true)
        {}


        public TestBooleanInputData(string prompt, string desc, string varName, bool defTrue) : 
            base(prompt, desc, varName)
        {
            radioBox = new GroupBox();
            trueRadio = new RadioButton();
            falseRadio = new RadioButton();

            radioBox.SuspendLayout();
            radioBox.Size = new System.Drawing.Size(128, 40);
            

            // Only handle an event for the true value changing.
            trueRadio.Location = new System.Drawing.Point(8, 14);
            trueRadio.Name = varName;
            trueRadio.Size = new System.Drawing.Size(48, 16);
            trueRadio.Text = "True";
                
            falseRadio.Location = new System.Drawing.Point(64, 14);
            falseRadio.Name = varName;
            falseRadio.Size = new System.Drawing.Size(56, 16);
            falseRadio.Text = "False";

            trueRadio.Checked = defTrue ? true : false;
            falseRadio.Checked = trueRadio.Checked ? false : true;

            radioBox.Controls.Add(trueRadio);
            radioBox.Controls.Add(falseRadio);

            defaultTrue = defTrue;
        }
    }


    /// <summary>
    /// Represents an input area for selectable data (drop-down box).
    /// </summary>
    public class TestOptionsInputData : TestInputDataBase
    {
        protected ArrayList selectOptions;
        protected ComboBox comboBox;

        public ArrayList Options { get { return selectOptions; } }
        public ComboBox ComboBox { get { return comboBox; } }

        public object DefaultOption
        {
            get
            {
                if(this.savedDefaultValue == "")
                {
                    return selectOptions[0];
                }
                else
                {
                    for(int i = 0; i < selectOptions.Count; i++)
                    {
                        if(savedDefaultValue == selectOptions[i].ToString())
                        {
                            return selectOptions[i];
                        }
                    }

                    return selectOptions[0];
                }
            }
        }


        public TestOptionsInputData(string prompt, string desc, string varName, ArrayList options) :
            base(prompt, desc, varName)
        {
            Debug.Assert(options.Count > 0, "Can not have an empty options list");

            selectOptions = options;

            comboBox = new ComboBox();
            comboBox.Name = varName;
            comboBox.Items.AddRange(Options.ToArray());
            comboBox.SelectedItem = DefaultOption;
        }
    }

    public class TestUserEvent : TestInputDataBase
    {
        protected CommonTypes.AsyncUserInputCallback callback;
        protected string buttonText;
        protected Button button;

        public string ButtonText { get { return buttonText; } }
        public Button Button { get { return button; } }

        public CommonTypes.AsyncUserInputCallback CallBack { get { return callback; } }

        public TestUserEvent(string prompt, string desc, string varName, string buttonText, CommonTypes.AsyncUserInputCallback callback) :
            base(prompt, desc, varName)
        {
            this.callback = callback;
            this.buttonText = buttonText;

            button = new Button();
            button.Name = varName;
            button.Text = buttonText;

        }
    }

    public class TestControl : TestInputDataBase
    {
        protected Control control;
        public Control Control { get { return control; } }

        public TestControl(string prompt, string desc, string varName, Control control) :
            base(prompt, desc, varName)
        {
            this.control = control;
        }
    }
}

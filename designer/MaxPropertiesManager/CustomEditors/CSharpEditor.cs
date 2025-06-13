using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Metreos.Max.Framework.Satellite.Property
{
    /// <summary>
    /// Summary description for CSharpEditor2.
    /// </summary>
    public class CSharpEditor : System.Windows.Forms.Form
    {
        public string   Value { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public bool     Ok { get { return ok; } set { ok = value; } }
        public string[] Usings { get { return ExtractUsingsFromCombo(); } }

        private bool ok;
        private AutoComplete autoComplete = null;
        private System.Windows.Forms.RichTextBox textBox1;
        private int oldCursorPosition;
        private bool autoCompletFirstShown;
        private Metreos.Max.Framework.Satellite.Property.AddRemoveCombo addRemoveCombo1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CSharpEditor(string[] initialUsings, string initialText)
        {
            InitializeComponent();
      
            this.ok = false;
            this.textBox1.Text = initialText;
            this.okButton.Click += new EventHandler(okButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);

            this.addRemoveCombo1.Initialize(initialUsings);
            this.addRemoveCombo1.MarkReadOnly(Metreos.ApplicationFramework.Assembler.Assembler.usings);
            this.addRemoveCombo1.SizeChanged += new EventHandler(addRemoveCombo1_SizeChanged);
        }

        protected string[] ExtractUsingsFromCombo()
        {
            if(addRemoveCombo1.Items.Count == 0)  return null;

            string[] usings = new string[addRemoveCombo1.Items.Count];
            for(int i = 0; i < addRemoveCombo1.Items.Count; i++)
            {
                AddRemoveComboItem currentItem = addRemoveCombo1.Items[i] as AddRemoveComboItem;
                usings[i] = currentItem.Value;
            }

            return usings;
        }

        /// <summary> Combines arrays, excluding duplicate entries. </summary>
        /// <param name="arrays"> Arrays to combines </param>
        /// <returns> The same type of array  </returns>
        protected static string[] CombineArrays(params string[][] arrays)
        {
            if(arrays == null || arrays.Length == 0)  return null;
            ArrayList combiner = new ArrayList();

            foreach(string[] array in arrays)
            {
                if(array == null) continue;

                foreach(string @value in array)
                    if(!combiner.Contains(@value))
                        combiner.Add(@value);
            }

            if(combiner.Count == 0) return null;

            string[] returnArray = new string[combiner.Count];
            combiner.CopyTo(returnArray);
            return returnArray;
        }

        /// <summary> Clean up any resources being used. </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
                if(components != null)
                    components.Dispose();

            base.Dispose( disposing );
        }

        private void addRemoveCombo1_SizeChanged(object sender, EventArgs e)
        {
            // Clamp down combo box to max size of Defaults.maxAddRemComboWidth
            if(addRemoveCombo1.Size.Width > Defaults.maxAddRemComboWidth)
            {
                Size comboSize = addRemoveCombo1.Size;
                comboSize.Width = Defaults.maxAddRemComboWidth;
                addRemoveCombo1.Size = comboSize;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            ok = true;
            this.Close();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            ok = false;
            this.Close();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.addRemoveCombo1 = new Metreos.Max.Framework.Satellite.Property.AddRemoveCombo();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AcceptsTab = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(744, 346);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(656, 371);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(568, 371);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            // 
            // addRemoveCombo1
            // 
            this.addRemoveCombo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addRemoveCombo1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.addRemoveCombo1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.addRemoveCombo1.Location = new System.Drawing.Point(504, 0);
            this.addRemoveCombo1.Name = "addRemoveCombo1";
            this.addRemoveCombo1.Size = new System.Drawing.Size(240, 21);
            this.addRemoveCombo1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(464, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Using";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CSharpEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(744, 398);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.addRemoveCombo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CSharpEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Editor";
            this.Load += new System.EventHandler(this.CSharpEditor_Load);
            this.ResumeLayout(false);

        }
        #endregion

        #region Unused Intellisense Code
        private void CSharpEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Space)
            {
                autoCompletFirstShown = true;
                e.Handled = true;
                bool userTypingVariable;
                string onlyMatch;
                string word = WordBeingTyped(textBox1, out userTypingVariable);
                if(!userTypingVariable)
                    autoComplete.InitializeForMethods(word, FindType(VariableAlreadyTyped(textBox1.Text, word)), out onlyMatch);
                else
                    autoComplete.InitializeForVariables(word, out onlyMatch);
                if(onlyMatch != null)
                {
                    ValueChosen(onlyMatch);
                }
                else
                {
                    autoComplete.Show();   
                    autoComplete.Location = ComputeAutoCompleteLocation();
                    this.Focus();
                }
            } 
            else if(!e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Space)
            {
                autoComplete.Hide();
            }

            if(autoComplete.Visible)
            {
                if(e.KeyCode == Keys.Down)
                {
                    autoComplete.NextRequested();
                    e.Handled = true;
                }
                else if(e.KeyCode == Keys.Up)
                {
                    autoComplete.PreviousRequested();
                    e.Handled = true;
                }
                else if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    autoComplete.HideWithValue();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="userTypingVariable">true: user is typing variable, false: user is typing a method</param>
        /// <returns></returns>
        private string WordBeingTyped(RichTextBox textBox, out bool userTypingVariable)
        {
            if(textBox.SelectedText.Length == 0)
            {
                if(textBox.SelectionStart == 0)
                {
                    userTypingVariable = true;
                    return "";
                }
                else
                {
                    int beginningOfWord = FindBeginningTextBeforeCursor(textBox.Text, textBox.SelectionStart, out  userTypingVariable);
                    if(beginningOfWord == textBox.SelectionStart)
                    {
                        return "";
                    }
                    else
                    {
                        return textBox.Text.Substring(beginningOfWord, textBox.SelectionStart - beginningOfWord);
                    }
                }
            }
            else
            {
                FindBeginningTextBeforeCursor(textBox.Text, textBox.SelectionStart, out userTypingVariable);
                return textBox.SelectedText;
            }
        }

        private string VariableAlreadyTyped(string allText, string wordBeingTyped)
        {
            return null;
        }

        private string FindType(string variable)
        {
            if(variable == null)
            {
                // The user isnt typing a non variable. Cant do anything for them.
                // return null;
                // TEMPORARY:

                return "Metreos.Types.Integer";
            }
            else
            {
                // Look up the type
                // TODO: with new getGlobalVars and new getFunctionVars, find type.
                return "Metreos.Types.Integer";
            }
        }
        private int FindBeginningTextBeforeCursor(string textToSearch, int cursorPlacement, out bool userTypingVariable)
        {
            userTypingVariable = true;

            if(cursorPlacement == 0)
                return 0; 

            char previousChar = textToSearch[cursorPlacement - 1];

            if((previousChar >= 48 && previousChar <= 57) || 
                (previousChar >= 65 && previousChar <= 90) || 
                (previousChar >= 97 && previousChar <= 122) ||
                previousChar == 95)
            {
                return FindBeginningTextBeforeCursor(textToSearch, --cursorPlacement, out userTypingVariable);
            }
            else
            {
                if(previousChar == ' ')
                    userTypingVariable = true;
                else if(previousChar == '.')
                    userTypingVariable = false;

                return cursorPlacement;
            }
        }

        protected void ValueChosen(string value)
        {
            bool userTypingVariable;
            string word = WordBeingTyped(textBox1, out userTypingVariable);   
            int saveSelection = textBox1.SelectionStart;
            string temp = String.Copy(textBox1.Text);
            StringBuilder builder = new StringBuilder(temp);  

            if(textBox1.SelectedText == "")
            {
                builder.Remove(textBox1.SelectionStart - word.Length, word.Length);
                builder.Insert(textBox1.SelectionStart - word.Length, value); 
            }
            else
            {
                builder.Remove(textBox1.SelectionStart, textBox1.SelectedText.Length);
                builder.Insert(textBox1.SelectionStart, value);
            }

            textBox1.Text = builder.ToString();
            textBox1.SelectionStart = saveSelection + value.Length;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == ' ' && autoCompletFirstShown)
            {
                autoCompletFirstShown = false;
                e.Handled = true;
            } 
        }

        private Point ComputeAutoCompleteLocation()
        {
            return new Point(this.Location.X, this.Location.Y - autoComplete.Height);
        }

        private void CSharpEditor2_Move(object sender, EventArgs e)
        {
            autoComplete.Location = ComputeAutoCompleteLocation();
        }

        private void textBox1_SelectionChanged(object sender, EventArgs e)
        {
            if(autoComplete.Visible)
            {
                // The user has either typed a letter, or moved the cursor in a major way

                if(oldCursorPosition != textBox1.SelectionStart)
                {
                    if(oldCursorPosition == textBox1.SelectionStart - 1 || oldCursorPosition == textBox1.SelectionStart + 1)
                    {
                        bool userTypingVariable;
                        string onlyMatch;
                        autoComplete.MoveIndexToMatch(WordBeingTyped(textBox1, out userTypingVariable), out onlyMatch);
                    }
                }
            }
            oldCursorPosition = textBox1.SelectionStart;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(autoComplete.Visible)
            {
                autoComplete.Hide();
            }
        }
        #endregion

        private void CSharpEditor_Load(object sender, System.EventArgs e)
        {
        
        }
    }
}

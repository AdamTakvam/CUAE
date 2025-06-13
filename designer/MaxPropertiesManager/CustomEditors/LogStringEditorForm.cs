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
  public class LogStringEditorForm : System.Windows.Forms.UserControl
  {
    private System.ComponentModel.Container components = null;

    private bool receivedFocusOnce;  
    private MaxPropertiesManager.GetGlobalVars getGlobalVars;
    private MaxPropertiesManager.GetFunctionVars getFunctionVars;
    private IWindowsFormsEditorService edSvc;
    private ITypeDescriptorContext context;
    private System.Windows.Forms.TextBox textBox1;
    private MaxProperty property;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private object value_;

    public string Value
    {
      get
      {
        return textBox1.Text;
      }
      set
      {
        textBox1.Text = value;
      }
    }


    public LogStringEditorForm(ITypeDescriptorContext context, 
      IWindowsFormsEditorService edSvc, object value, 
      MaxPropertiesManager.GetGlobalVars   passedGetGlobalVars, 
      MaxPropertiesManager.GetFunctionVars passedGetFunctionVars)
    {
      InitializeComponent();		
      this.context = context;
      this.edSvc = edSvc;
      this.property = (MaxProperty) value;
      this.value_ = value;
      this.getFunctionVars = passedGetFunctionVars;
      this.getGlobalVars = passedGetGlobalVars;
      this.comboBox1.Items.AddRange(getFunctionVars(property.Subject));
      this.comboBox1.Items.AddRange(getGlobalVars());

      if(comboBox1.Items.Count >= 1)
      {
        this.comboBox1.Text = comboBox1.Items[0].ToString();
      }
      else
      {
        this.comboBox1.Text = "None defined";
      }
			
      textBox1.Text = property.Value.ToString();
    }                                          

    public IWindowsFormsEditorService EditorService 
    {
      get  
      {
        return edSvc;
      }
      set 
      {
        this.edSvc = value;
      }
    }

       
    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null) components.Dispose();          
      base.Dispose(disposing);
    }

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(16, 24);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(168, 136);
      this.textBox1.TabIndex = 0;
      this.textBox1.Text = "textBox1";
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.Location = new System.Drawing.Point(16, 192);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(168, 21);
      this.comboBox1.TabIndex = 0;
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.BackColor = System.Drawing.SystemColors.Control;
      this.label1.Location = new System.Drawing.Point(16, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(136, 16);
      this.label1.TabIndex = 6;
      this.label1.Text = "Enter Text";
      // 
      // label2
      // 
      this.label2.BackColor = System.Drawing.SystemColors.Control;
      this.label2.Location = new System.Drawing.Point(16, 176);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(136, 16);
      this.label2.TabIndex = 7;
      this.label2.Text = "Available Variables";
      // 
      // LogStringEditorForm
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.comboBox1);
      this.Name = "LogStringEditorForm";
      this.Size = new System.Drawing.Size(200, 232);
      this.ResumeLayout(false);

    }

    #endregion

    private void CloseDialog(object sender, EventArgs e)
    {
      //            if(this.receivedFocusOnce)
      //            {
      edSvc.CloseDropDown();	
      //            }

      if (this.receivedFocusOnce) { } // Prevent unused var warning
    }

    private void StringEditor2_GotFocus(object sender, EventArgs e)
    {
      this.receivedFocusOnce = true;
    }

    private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      string variableName = comboBox1.SelectedItem.ToString();
    }
  }
}

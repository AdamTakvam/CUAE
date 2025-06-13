using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace Metreos.Max.Framework.Satellite.Property
{
	/// <summary>
	/// Summary description for AutoComplete2.
	/// </summary>
	public class AutoComplete : System.Windows.Forms.Form
	{
    public delegate void ValueChosen(string value);

    private AutoResetEvent selectedIndexLock;
    private string chosen;
    private ValueChosen valueChosen;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.ComponentModel.IContainer components;
    private MaxPropertiesManager.GetFunctionVars getFunctionVars;
    private MaxPropertiesManager.GetGlobalVars getGlobalVars;
    private MaxPropertiesManager.GetNativeTypesInfo getNativeTypesInfo;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ImageList imageList1;
    private object subject;
    
    public AutoComplete(MaxPropertiesManager.GetGlobalVars getGlobalVars, MaxPropertiesManager.GetFunctionVars getFunctionVars, MaxPropertiesManager.GetNativeTypesInfo getNativeTypesInfo, ValueChosen valueChosen, object subject)
    {
      chosen = null;
      this.valueChosen = valueChosen;
      this.getGlobalVars = getGlobalVars;
      this.getFunctionVars = getFunctionVars;
      this.getNativeTypesInfo = getNativeTypesInfo;
      this.subject = subject;
      selectedIndexLock = new AutoResetEvent(false);
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      listView1.SelectedIndexChanged += new EventHandler(listView1_SelectedIndexChanged);
      listView1.MouseMove += new MouseEventHandler(listView1_MouseMove);
      listView1.DoubleClick += new EventHandler(listView1_DoubleClick);
      listView1.KeyDown += new KeyEventHandler(listView1_KeyDown);
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
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AutoComplete));
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.listView1 = new System.Windows.Forms.ListView();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.listView1.FullRowSelect = true;
      this.listView1.Location = new System.Drawing.Point(0, 0);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(144, 67);
      this.listView1.SmallImageList = this.imageList1;
      this.listView1.TabIndex = 0;
      this.listView1.View = System.Windows.Forms.View.List;
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      // 
      // imageList1
      // 
      this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // AutoComplete
      // 
      this.AllowDrop = true;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(144, 67);
      this.ControlBox = false;
      this.Controls.Add(this.listView1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AutoComplete";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AutoComplete";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.AutoComplete_Load);
      this.ResumeLayout(false);

    }
		#endregion

    public void InitializeForVariables(string toMatch, out string onlyMatch)
    {
      onlyMatch = null;
      object[] functionVars = getFunctionVars(subject);
      object[] globalVars = getGlobalVars();

      if(functionVars == null && globalVars == null)
      {
        onlyMatch = "";
        return;
      }

      listView1.Items.Clear();      
      
      if(functionVars != null)
      {
        for(int i = 0; i < functionVars.Length; i++)
        {
          string[] functionVar = functionVars[i] as string[];
          AutoCompleteRow autoComplete = new AutoCompleteRow(functionVar[0], functionVar[1]);
          ListViewItem item = new ListViewItem(autoComplete.Text);//, Defaults.IMAGES_FUNCTION_VARIABLE);
          item.Tag = autoComplete;
          listView1.Items.Add(item);
        }
      }

      if(globalVars != null)
      { 
        for(int i = 0; i < globalVars.Length; i++)
        {
          string[] globalVar = globalVars[i] as string[];
          AutoCompleteRow autoComplete = new AutoCompleteRow(globalVar[0], globalVar[1]);
          ListViewItem item = new ListViewItem(autoComplete.Text); //, Defaults.IMAGES_GLOBAL_VARIABLE);
          item.Tag = autoComplete;
          listView1.Items.Add(item);
        }
      }

      MoveIndexToMatch(toMatch, out onlyMatch);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="variableType">The type of the variable</param>
    /// <param name="onlyMatch">If null, then there are multiple possible matches
    /// if not null, then there is only one possible answer.</param>
    public void InitializeForMethods(string toMatch, string variableType, out string onlyMatch)
    {
      onlyMatch = null;
      
      if(variableType == null)
      {
        return;
      }
      else
      {
        Metreos.PackageGeneratorCore.PackageXml.nativeTypePackageType[] nativeTypes = getNativeTypesInfo();
        for(int i = 0; i < (nativeTypes != null ? nativeTypes.Length : 0); i++)
        {
          for(int j = 0; j < (nativeTypes[i].type != null ? nativeTypes[i].type.Length : 0); j++)
          {
            if(nativeTypes[i].name + '.' + nativeTypes[i].type[j].name == variableType)
            {
              listView1.Items.Clear();
              for(int k = 0; k < (nativeTypes[i].type[j].customMethod != null ? nativeTypes[i].type[j].customMethod.Length : 0); k++)
              {
                string parameters = "";
                if(nativeTypes[i].type[j].customMethod[k].parameter != null)
                {
                  StringBuilder parametersBuilder = new StringBuilder();
                  for(int l = 0; l < nativeTypes[i].type[j].customMethod[k].parameter.Length; l++)
                  {
                    parametersBuilder.Append(", " + nativeTypes[i].type[j].customMethod[k].parameter[l].Value);
                  }

                  parametersBuilder.Remove(0, 2);
                  parameters = parametersBuilder.ToString();
                }           

                string description = nativeTypes[i].type[j].customMethod[k].returnType + ' ' + 
                  variableType + '.' + nativeTypes[i].type[j].customMethod[k].name + '(' + parameters + ')' + '\n' +
                  nativeTypes[i].type[j].customMethod[k].description;
                  AutoCompleteRow row = new AutoCompleteRow(nativeTypes[i].type[j].customMethod[k].name, 
                  description);
                ListViewItem item = new ListViewItem(row.Text); //Defaults.IMAGES_METHOD);
                item.Tag = row;
                listView1.Items.Add(item);

              }

              MoveIndexToMatch(toMatch, out onlyMatch);

              return;
            }
          }
        }

        // means that no variableType match was found, so the AutoComplete returns the only possible value: ""
        onlyMatch = "";
      }
    }

    public void MoveIndexToMatch(string toMatch, out string onlyMatch)
    {
      onlyMatch = null;
      bool makeSelected;

      int index = 0;
      if(listView1.SelectedIndices.Count != 0)
        index = listView1.SelectedIndices[0];  
      
      int compareResult = CaseInsensitiveComparer.Default.Compare(toMatch, (listView1.Items[index].Tag as AutoCompleteRow).Text); 
      if(0 == compareResult)
      {
        // Found perfect match. Exit;
        onlyMatch = toMatch;
        return ;
      } 
      else if(0 < compareResult)
      {
        FindBestMatch(toMatch, ref index, true, out makeSelected);
      } 
      else
      {
        FindBestMatch(toMatch, ref index, false, out makeSelected);
      }

      if(makeSelected)
      {
        listView1.Items[index].Selected = true;
      }
      else
      {
        if(listView1.Items[index].Selected != true)
        {
         
          listView1.Items[index].Selected = true;
          
        }

        //listView1.Items[index].Selected = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toMatch"></param>
    /// <param name="previousDirection">false is down, true is up</param>
    public void FindBestMatch(string toMatch, ref int index, bool previousDirection, out bool makeSelected)
    {
      makeSelected = false;
      int compareResult = CaseInsensitiveComparer.Default.Compare(toMatch, (listView1.Items[index].Tag as AutoCompleteRow).Text);
      if(0 == compareResult)
      {
        // Found perfect match. Exit;
        return;
      } 
        // is postive
      else if(0 < compareResult)
      {
        // toMatch is greater, so move to next field
        // Check boundary condition
        if(index == listView1.Items.Count - 1)
        {
          // This is as close as it gets.
          return;
        }
        else
        {
          // Bobble point detected.  Compare the two on edges.
          if(previousDirection == false)
          {
            // First see if the toMatch is even close to whats in the listView.s
            if((listView1.Items[index].Tag as string).IndexOf(toMatch) != -1 || (listView1.Items[index].Tag as string).IndexOf(toMatch) != -1)
            {
              makeSelected = true;
            }
            if(!WhichIsCloser(toMatch, listView1.Items[index].Tag as string, listView1.Items[index + 1].Tag as string))
            {
              index++;
            }
          }
          else
          {
            // Move one up, and keep looking
            index++;
            FindBestMatch(toMatch, ref index, true, out makeSelected);
          }
        }
      }
        // is negative
      else
      {
        // toMatch is less, so move to previous field
        // Check boundary condition
        if(index == 0)
        {
          // This is as close as it gets.
          return;
        }
        else
        {
          // Bobble point detected.  Compare the two on edges.
          if(previousDirection == true)
          {
            // First see if the toMatch is even close to whats in the listView.s
            if((listView1.Items[index].Tag as string).IndexOf(toMatch) != -1 || (listView1.Items[index].Tag as string).IndexOf(toMatch) != -1)
            {
              makeSelected = true;
            }
            if(!WhichIsCloser(toMatch, listView1.Items[index].Tag as string, listView1.Items[index - 1].Tag as string))
            {
              index--;
            }
          }
          else
          {
            // Move one up, and keep looking
            index--;
            FindBestMatch(toMatch, ref index, false, out makeSelected);
          }
        }
      }
    }

    private bool WhichIsCloser(string toMatch, string current, string previous)
    {
      toMatch = toMatch.ToLower();
      current = current.ToLower();
      previous = previous.ToLower();

      return Compare(toMatch, current, previous, 0);
    }

    private bool Compare(string toMatch, string current, string previous, int index)
    {
      char toMatchChar = (char)0;

      if(toMatch.Length <= index)
        toMatchChar = toMatch[toMatch.Length - 1];  
      else
        toMatchChar = toMatch[index];
      
      char currentChar = current[index];
      char previousChar = previous[index];

      if(toMatchChar == currentChar && toMatchChar == previousChar)
      {  
        index++;

        if(current.Length == index)
          return true;
        else if(previous.Length == index)
          return false;
        else
          return Compare(toMatch, current, previous, index);
      }
      else
      {
        char currentDifference = (char)(toMatchChar - currentChar);
        char previousDifference = (char)(toMatchChar - previousChar);

        if(currentDifference < previousDifference)
          return true;
        else
          return false;
      }
    }
    public void PreviousRequested()
    {
      if(listView1.SelectedItems.Count != 0)
      {
        if(listView1.SelectedIndices[0] > 0)
        {
          listView1.Items[listView1.SelectedIndices[0] - 1].Selected = true;
        }
      }
      else
      {
        listView1.Items[listView1.Items.Count - 1].Selected = true;
      }
    }

    public void NextRequested()
    {
      if(listView1.SelectedItems.Count != 0)
      {
        if(listView1.SelectedIndices[0] < listView1.Items.Count - 1)
        {
          listView1.Items[listView1.SelectedIndices[0] + 1].Selected = true;
        }
      }
      else
      {
        listView1.Items[0].Selected = true;
      }
    }

    public void HideWithValue()
    {
      UserChoseValue();
    }

    private void UserChoseValue()
    {
      if(listView1.SelectedItems.Count != 0)
      {
        valueChosen((listView1.SelectedItems[0].Tag as AutoCompleteRow).Text);
        this.Hide();
      }
    }

    #region Event Handlers

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if(listView1.SelectedItems.Count != 0)
      {
        chosen = listView1.SelectedItems[0].Tag as string;
      }
      selectedIndexLock.Set();
    }

    private void listView1_MouseMove(object sender, MouseEventArgs e)
    {
      Point point = new Point(e.X, e.Y);
      ListViewItem item = listView1.GetItemAt(point.X, point.Y);
      if(item != null)
        toolTip1.SetToolTip(listView1, (item.Tag as AutoCompleteRow).Description);
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      UserChoseValue();
    } 

    private void listView1_KeyDown(object sender, KeyEventArgs e)
    {
      if(!e.Shift && !e.Control && !e.Alt && e.KeyCode == Keys.Enter)
      {
        UserChoseValue();
      }
    }

    private void AutoComplete_Load(object sender, EventArgs e)
    {
      toolTip1.AutoPopDelay = 5000;
      toolTip1.InitialDelay = 1000;
      toolTip1.ReshowDelay = 500;
      toolTip1.ShowAlways = true;
    }
    #endregion

  }

  [TypeConverter(typeof(AutoCompleteRowTypeConverter))]
  public class AutoCompleteRow
  {
    public string Text { get { return text; } }
    public string Description { get { return description; } }
  
    private string text;
    private string description;

    public AutoCompleteRow(string text, string description)
    {
      this.text = text;
      this.description = description;
    }

    public override string ToString()
    {
      return text.ToString();
    }
  }

  internal class AutoCompleteRowTypeConverter : TypeConverter
  {
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if(destinationType == typeof(string))
      {
        return true;
      }
      else
      {
        return base.CanConvertTo (context, destinationType);
      }
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
      if(destinationType == typeof(string))
      {
        return (context.Instance as AutoCompleteRow).Text.ToString();
      }
      else
      {
        return base.ConvertTo (context, culture, value, destinationType);
      }
    }
  }

}

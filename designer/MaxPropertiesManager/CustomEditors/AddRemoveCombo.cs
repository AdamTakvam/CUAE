using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace Metreos.Max.Framework.Satellite.Property
{
	/// <summary> A Combobox which allows the removal and addition of items from using
	///           only the combobox </summary>
	public class AddRemoveCombo : ComboBox
	{
    private int lastSelectedIndex = -1;

		public AddRemoveCombo() : base()
		{
      this.DrawMode = DrawMode.OwnerDrawVariable;
		}

    public void Initialize(string[] entries)
    {
      this.Items.Clear();

      if(entries == null)     return;
      if(entries.Length == 0) return;

      foreach(string entry in entries)
        Items.Add(new AddRemoveComboItem(entry, true));
    }

    public void MarkReadOnly(string[] readonlyEntries)
    {
      if(readonlyEntries == null || readonlyEntries.Length == 0)  return;

      foreach(string readonlyEntry in readonlyEntries)
        foreach(AddRemoveComboItem currentItem in Items)
          if(String.Compare(readonlyEntry, currentItem.Value) == 0)
          {
            currentItem.Editable = false;
            break;
          }
    }

    /// <summary> Changes the state of the combobox to reflect
    ///  that the user has just selected a new ite </summary>
    /// <param name="e"></param>
    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      if(Items.Count < this.SelectedIndex)
      {
        AddRemoveComboItem newlySelected = Items[this.SelectedIndex] as AddRemoveComboItem;
        
        if(!newlySelected.Editable)
          this.SelectedIndex = -1;
      }

      this.lastSelectedIndex = this.SelectedIndex;
      base.OnSelectedItemChanged (e);
    }

    /// <summary> Checks that the user is attempting to delete or add a member </summary>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      if(this.DroppedDown)            return;

      // Waiting on an enter in the text editable portion of the combobox
      if(e.KeyChar != 0x0D)           return;

      // User is not making a deletion
      if(this.Text == String.Empty)     
        DeletingAttempt();
      else                            
        AddingAttempt();

      base.OnKeyPress (e);
    }

    /// <summary> User is attempting to delete an item </summary>
    private void DeletingAttempt()
    {
      // Check that an item is even selected
      if(this.lastSelectedIndex == -1)          return;
      if(lastSelectedIndex >= this.Items.Count) return;

      AddRemoveComboItem currentItem = Items[lastSelectedIndex] as AddRemoveComboItem;

      // Can we delete this item?
      if(!currentItem.Editable)                 return;

      this.Items.RemoveAt(lastSelectedIndex);
      this.SelectedIndex = -1;
    }

    private void AddingAttempt()
    {
      // User is attempting to add an item that is already existent
      if(ContainsText(this.Text))     return;

      this.Items.Add(new AddRemoveComboItem(this.Text, true));

      // Set the index to nothing, and clear the text of the combobox,
      // letting the user know he just made an addition.
      this.Text = String.Empty;
      this.SelectedIndex = -1;
    }

    /// <summary> Does a case-sensitive check that the items in this
    ///           combobox contain the text in the question </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private bool ContainsText(string text)
    {
      foreach(AddRemoveComboItem currentItem in Items)
      {
        if(currentItem == null)       continue;
        if(currentItem.Value == null) continue;

        if(String.Compare(text, currentItem.Value, false) == 0) return true;
      }

      return false;
    }

    /// <summary> Based on if the currently draw item is editable or not, 
    /// the item is drawn normally or in italics, respectively </summary>
    protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
    {	
      if(e.Index > -1)
      {
        AddRemoveComboItem currentItem = Items[e.Index] as AddRemoveComboItem;
    
        Color textColor = DetermineTextColor(currentItem.Editable, e.State == DrawItemState.Focus);
        Color backgroundColor = DetermineBackgroundColor(e.State == DrawItemState.Focus);

        e.Graphics.FillRectangle(new SolidBrush(backgroundColor),
          e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        e.Graphics.DrawString(currentItem.Value, this.Font, new SolidBrush(textColor),
          e.Bounds.X, e.Bounds.Y);
      }
      base.OnDrawItem(e);
    }

    protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
    {	
      if(e.Index > -1)
      {
        AddRemoveComboItem currentItem = Items[e.Index] as AddRemoveComboItem;						
        Graphics g = CreateGraphics();

        SizeF currentItemSize = g.MeasureString(currentItem.Value, 
          this.Font);

        e.ItemHeight = (int) currentItemSize.Height;
        e.ItemWidth  = (int) currentItemSize.Width;
      }	
								
      base.OnMeasureItem(e);
    }

    protected Color DetermineBackgroundColor(bool focused)
    {
      return focused ? SystemColors.Highlight : SystemColors.Window;
    }
    protected Color DetermineTextColor(bool editable, bool focused)
    {
      if(focused) return SystemColors.WindowText;
      return editable ? SystemColors.WindowText : SystemColors.GrayText;
    }
	}

  public class AddRemoveComboItem
  {
    public string Value { get { return innerValue; } }
    public bool Editable { get { return editable; } set { editable = value; } } 

    private string innerValue;
    private bool editable;

    public AddRemoveComboItem(string innerValue, bool editable)
    {
      this.innerValue = innerValue;
      this.editable = editable;
    }

    public override string ToString()
    {
      if(innerValue == null)    return null;
      else return innerValue;
    }
  }
}

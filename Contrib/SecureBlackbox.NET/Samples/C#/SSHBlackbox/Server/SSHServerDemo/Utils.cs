using System;
using System.Windows;
using System.Windows.Forms;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Utils
{
	public Utils()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    delegate void ListViewAddItemCallback(Form form, ListView lv, ListViewItem lvi);
    delegate void ListViewItemAddSubItemCallback(Form form, ListView lv, ListViewItem lvi, string s);
    delegate void ListViewItemSetTextCallback(Form form, ListView lv, ListViewItem lvi, string s);
    delegate void ListViewClearCallback(Form form, ListView lv);
    delegate void ListViewItemClearSubItemsCallback(Form form, ListView lv, ListViewItem lvi);

    public static void ListViewAddItem(Form form, ListView lv, ListViewItem lvi)
    {
        if (lv.InvokeRequired)
        {
            ListViewAddItemCallback d = new ListViewAddItemCallback(ListViewAddItem);
            form.Invoke(d, new object[] { form, lv, lvi });
        }
        else
        {
            lv.Items.Add(lvi);
        }
    }

    public static void ListViewItemAddSubItem(Form form, ListView lv, ListViewItem lvi, string s)
    {
        if (lv.InvokeRequired)
        {
            ListViewItemAddSubItemCallback d = new ListViewItemAddSubItemCallback(ListViewItemAddSubItem);
            form.Invoke(d, new object[] { form, lv, lvi, s });
        }
        else
        {
            lvi.SubItems.Add(s);
        }
    }

    public static void ListViewItemSetText(Form form, ListView lv, ListViewItem lvi, string s)
    {
        if (lv.InvokeRequired)
        {
            ListViewItemSetTextCallback d = new ListViewItemSetTextCallback(ListViewItemSetText);
            form.Invoke(d, new object[] { form, lv, lvi, s });
        }
        else
        {
            lvi.Text = s;
        }
    }

    public static void ListViewClear(Form form, ListView lv)
    {
        if (lv.InvokeRequired)
        {
            ListViewClearCallback d = new ListViewClearCallback(ListViewClear);
            form.Invoke(d, new object[] { form, lv });
        }
        else
        {
            lv.Items.Clear();
        }
    }

    public static void ListViewItemClearSubItems(Form form, ListView lv, ListViewItem lvi)
    {
        if (lv.InvokeRequired)
        {
            ListViewItemClearSubItemsCallback d = new ListViewItemClearSubItemsCallback(ListViewItemClearSubItems);
            form.Invoke(d, new object[] { form, lv, lvi });
        }
        else
        {
            lvi.SubItems.Clear();
        }
    }

}

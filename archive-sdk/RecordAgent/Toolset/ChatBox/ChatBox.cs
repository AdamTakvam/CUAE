using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;

using Metreos.Toolset.CommonUtility;
using Metreos.Toolset.Commoni18n;

namespace Metreos.Toolset
{
	/// <summary>
	/// An extended RichTextBox that contains a toolbar.
	/// </summary>
	public class ChatBox : System.Windows.Forms.UserControl
	{
		public delegate void OnRichTextBoxExKeyDownDelegate(object sender, System.Windows.Forms.KeyEventArgs e);
		public event OnRichTextBoxExKeyDownDelegate OnRichTextBoxExKeyDown;

		private Locale i18n;

		private ImageList emoticons;
		private ImageListPopup emoPop;
		private ImageList colors;
		private ImageListPopup colorPop;
		private txt2emo txt2emo;

		#region Windows Generated
		private System.Windows.Forms.ToolBar tb1;
		private System.Windows.Forms.ImageList imgList1;
		private System.Windows.Forms.ToolBarButton tbbBold;
		private System.Windows.Forms.ToolBarButton tbbItalic;
		private System.Windows.Forms.ToolBarButton tbbUnderline;
		private System.Windows.Forms.ToolBarButton tbbCenter;
		private System.Windows.Forms.ToolBarButton tbbRight;
		private System.Windows.Forms.ToolBarButton tbbStrikeout;
		private System.Windows.Forms.ToolBarButton tbbColor;
		private System.Windows.Forms.ToolBarButton tbbEmoticons;
		private System.Windows.Forms.ToolBarButton tbbOpen;
		private System.Windows.Forms.ToolBarButton tbbSave;
		private System.Windows.Forms.ToolBarButton tbbUndo;
		private System.Windows.Forms.ToolBarButton tbbRedo;
		private System.Windows.Forms.ToolBarButton tbbSeparator2;
		private System.Windows.Forms.ToolBarButton tbbSeparator3;
		private System.Windows.Forms.ToolBarButton tbbSeparator4;
		private System.Windows.Forms.ToolBarButton tbbSeparator1;
		private System.Windows.Forms.ToolBarButton tbbLeft;
		private System.Windows.Forms.OpenFileDialog ofd1;
		private System.Windows.Forms.SaveFileDialog sfd1;
		private Metreos.Toolset.RichTextBoxEx rtb1;
		private System.Windows.Forms.ContextMenu cm1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItemCut;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemPaste;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItemSelectAll;
		private Metreos.Toolset.GraphicMenu gm1;
		private System.ComponentModel.IContainer components;
		private System.Resources.ResourceManager resources;


		public ChatBox()
		{
			this.resources = new System.Resources.ResourceManager(typeof(ChatBox));

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			i18n = Locale.Instance;
			i18n.LocaleChanged += new Locale.LocaleChangedEventHandler(this.OnLocaleChanged);

			// Build up emoticons
			Bitmap emo1 = new Bitmap(typeof(ChatBox), "Images.emoticon1.png");
			Bitmap emo2 = new Bitmap(typeof(ChatBox), "Images.emoticon2.png");
			Bitmap emo3 = new Bitmap(typeof(ChatBox), "Images.emoticon3.png");
			Bitmap emo4 = new Bitmap(typeof(ChatBox), "Images.emoticon4.png");
			emoticons = new ImageList();
			emoticons.ImageSize=new Size(emo1.Height, emo1.Height);		// width and height are the same
			emoticons.ColorDepth = ColorDepth.Depth24Bit;

			emoticons.Images.AddStrip(emo1);
			emoticons.Images.AddStrip(emo2);
			emoticons.Images.AddStrip(emo3);
			emoticons.Images.AddStrip(emo4);
			emoticons.TransparentColor = Color.FromArgb(255, 255, 255);

			// Creation of the first ImageListPopup
			emoPop = new ImageListPopup();
			emoPop.Init(emoticons, 6, 6, 10, 6);
			emoPop.ItemClick += new ImageListPopupEventHandler(OnEmoticonClicked);

			// Build up colors
			Bitmap clr = new Bitmap(typeof(ChatBox), "Images.colors.png");
			colors = new ImageList();
			colors.ImageSize=new Size(clr.Height, clr.Height);		// width and height are the same
			colors.ColorDepth = ColorDepth.Depth16Bit;

			colors.Images.AddStrip(clr);
			colors.TransparentColor = Color.FromArgb(255, 255, 255);

			// Creation of the first ImageListPopup
			colorPop = new ImageListPopup();
			colorPop.Init(colors, 8, 8, 4, 4);
			colorPop.ItemClick += new ImageListPopupEventHandler(OnColorsClicked);

			gm1.Init(cm1);
			gm1.AddIcon(menuItemCut, new Bitmap(typeof(ChatBox), "Images.cut.bmp"));
			gm1.AddIcon(menuItemCopy, new Bitmap(typeof(ChatBox), "Images.copy.bmp"));
			gm1.AddIcon(menuItemPaste, new Bitmap(typeof(ChatBox), "Images.paste.bmp"));
			gm1.AddIcon(menuItemDelete, new Bitmap(typeof(ChatBox), "Images.delete.bmp"));

			// drag and drop handler for rtb
			this.rtb1.DragEnter += new DragEventHandler(this.rtb1_DragEnter);
			this.rtb1.DragDrop += new DragEventHandler(this.rtb1_DragDrop);


			//Update the graphics on the toolbar
			UpdateToolbar();

			txt2emo = new txt2emo();

			UpdateLocale(i18n.CurrentCulture);
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ChatBox));
			this.tb1 = new System.Windows.Forms.ToolBar();
			this.tbbSave = new System.Windows.Forms.ToolBarButton();
			this.tbbOpen = new System.Windows.Forms.ToolBarButton();
			this.tbbSeparator3 = new System.Windows.Forms.ToolBarButton();
			this.tbbBold = new System.Windows.Forms.ToolBarButton();
			this.tbbItalic = new System.Windows.Forms.ToolBarButton();
			this.tbbUnderline = new System.Windows.Forms.ToolBarButton();
			this.tbbStrikeout = new System.Windows.Forms.ToolBarButton();
			this.tbbSeparator1 = new System.Windows.Forms.ToolBarButton();
			this.tbbLeft = new System.Windows.Forms.ToolBarButton();
			this.tbbCenter = new System.Windows.Forms.ToolBarButton();
			this.tbbRight = new System.Windows.Forms.ToolBarButton();
			this.tbbSeparator2 = new System.Windows.Forms.ToolBarButton();
			this.tbbUndo = new System.Windows.Forms.ToolBarButton();
			this.tbbRedo = new System.Windows.Forms.ToolBarButton();
			this.tbbSeparator4 = new System.Windows.Forms.ToolBarButton();
			this.tbbColor = new System.Windows.Forms.ToolBarButton();
			this.tbbEmoticons = new System.Windows.Forms.ToolBarButton();
			this.imgList1 = new System.Windows.Forms.ImageList(this.components);
			this.ofd1 = new System.Windows.Forms.OpenFileDialog();
			this.sfd1 = new System.Windows.Forms.SaveFileDialog();
			this.rtb1 = new Metreos.Toolset.RichTextBoxEx();
			this.cm1 = new System.Windows.Forms.ContextMenu();
			this.menuItemCut = new System.Windows.Forms.MenuItem();
			this.menuItemCopy = new System.Windows.Forms.MenuItem();
			this.menuItemPaste = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemSelectAll = new System.Windows.Forms.MenuItem();
			this.gm1 = new Metreos.Toolset.GraphicMenu();
			this.SuspendLayout();
			// 
			// tb1
			// 
			this.tb1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tb1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																				   this.tbbSave,
																				   this.tbbOpen,
																				   this.tbbSeparator3,
																				   this.tbbBold,
																				   this.tbbItalic,
																				   this.tbbUnderline,
																				   this.tbbStrikeout,
																				   this.tbbSeparator1,
																				   this.tbbLeft,
																				   this.tbbCenter,
																				   this.tbbRight,
																				   this.tbbSeparator2,
																				   this.tbbUndo,
																				   this.tbbRedo,
																				   this.tbbSeparator4,
																				   this.tbbColor,
																				   this.tbbEmoticons});
			this.tb1.ButtonSize = new System.Drawing.Size(16, 16);
			this.tb1.Divider = false;
			this.tb1.DropDownArrows = true;
			this.tb1.ImageList = this.imgList1;
			this.tb1.Location = new System.Drawing.Point(0, 0);
			this.tb1.Name = "tb1";
			this.tb1.ShowToolTips = true;
			this.tb1.Size = new System.Drawing.Size(352, 26);
			this.tb1.TabIndex = 0;
			this.tb1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tb1_ButtonClick);
			// 
			// tbbSave
			// 
			this.tbbSave.ImageIndex = 11;
			this.tbbSave.Tag = "Save";
			this.tbbSave.ToolTipText = "Save";
			// 
			// tbbOpen
			// 
			this.tbbOpen.ImageIndex = 10;
			this.tbbOpen.Tag = "Open";
			this.tbbOpen.ToolTipText = "Open";
			// 
			// tbbSeparator3
			// 
			this.tbbSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbbBold
			// 
			this.tbbBold.ImageIndex = 0;
			this.tbbBold.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbBold.Tag = "Bold";
			this.tbbBold.ToolTipText = "Bold";
			// 
			// tbbItalic
			// 
			this.tbbItalic.ImageIndex = 1;
			this.tbbItalic.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbItalic.Tag = "Italic";
			this.tbbItalic.ToolTipText = "Italic";
			// 
			// tbbUnderline
			// 
			this.tbbUnderline.ImageIndex = 2;
			this.tbbUnderline.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbUnderline.Tag = "Underline";
			this.tbbUnderline.ToolTipText = "Underline";
			// 
			// tbbStrikeout
			// 
			this.tbbStrikeout.ImageIndex = 3;
			this.tbbStrikeout.Tag = "Strikeout";
			this.tbbStrikeout.ToolTipText = "Strikeout";
			// 
			// tbbSeparator1
			// 
			this.tbbSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbbLeft
			// 
			this.tbbLeft.ImageIndex = 4;
			this.tbbLeft.Tag = "Left";
			this.tbbLeft.ToolTipText = "Left";
			// 
			// tbbCenter
			// 
			this.tbbCenter.ImageIndex = 5;
			this.tbbCenter.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbCenter.Tag = "Center";
			this.tbbCenter.ToolTipText = "Center";
			// 
			// tbbRight
			// 
			this.tbbRight.ImageIndex = 6;
			this.tbbRight.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbRight.Tag = "Right";
			this.tbbRight.ToolTipText = "Right";
			// 
			// tbbSeparator2
			// 
			this.tbbSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbbUndo
			// 
			this.tbbUndo.ImageIndex = 12;
			this.tbbUndo.Tag = "Undo";
			this.tbbUndo.ToolTipText = "Undo";
			// 
			// tbbRedo
			// 
			this.tbbRedo.ImageIndex = 13;
			this.tbbRedo.Tag = "Redo";
			this.tbbRedo.ToolTipText = "Redo";
			// 
			// tbbSeparator4
			// 
			this.tbbSeparator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbbColor
			// 
			this.tbbColor.ImageIndex = 7;
			this.tbbColor.Tag = "Color";
			this.tbbColor.ToolTipText = "Color";
			// 
			// tbbEmoticons
			// 
			this.tbbEmoticons.ImageIndex = 14;
			this.tbbEmoticons.Tag = "Emoticons";
			this.tbbEmoticons.ToolTipText = "Emoticons";
			// 
			// imgList1
			// 
			this.imgList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imgList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList1.ImageStream")));
			this.imgList1.TransparentColor = System.Drawing.Color.LimeGreen;
			// 
			// ofd1
			// 
			this.ofd1.DefaultExt = "rtf";
			this.ofd1.Filter = "Rich Text Files|*.rtf";
			this.ofd1.Title = "Open File";
			// 
			// sfd1
			// 
			this.sfd1.DefaultExt = "rtf";
			this.sfd1.Filter = "Rich Text File|*.rtf";
			this.sfd1.Title = "Save As";
			// 
			// rtb1
			// 
			this.rtb1.AllowDrop = true;
			this.rtb1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtb1.ContextMenu = this.cm1;
			this.rtb1.HiglightColor = Metreos.Toolset.RtfColor.White;
			this.rtb1.Location = new System.Drawing.Point(0, 24);
			this.rtb1.Name = "rtb1";
			this.rtb1.Size = new System.Drawing.Size(352, 56);
			this.rtb1.TabIndex = 1;
			this.rtb1.Text = "";
			this.rtb1.TextColor = Metreos.Toolset.RtfColor.Black;
			this.rtb1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb1_KeyDown);
			this.rtb1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtb1_KeyPress);
			this.rtb1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtb1_LinkClicked);
			// 
			// cm1
			// 
			this.cm1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				this.menuItemCut,
																				this.menuItemCopy,
																				this.menuItemPaste,
																				this.menuItemDelete,
																				this.menuItem5,
																				this.menuItemSelectAll});
			this.cm1.Popup += new System.EventHandler(this.cm1_Popup);
			// 
			// menuItemCut
			// 
			this.menuItemCut.Enabled = false;
			this.menuItemCut.Index = 0;
			this.menuItemCut.Text = "Cut";
			this.menuItemCut.Click += new System.EventHandler(this.menuItemCut_Click);
			// 
			// menuItemCopy
			// 
			this.menuItemCopy.Enabled = false;
			this.menuItemCopy.Index = 1;
			this.menuItemCopy.Text = "Copy";
			this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
			// 
			// menuItemPaste
			// 
			this.menuItemPaste.Enabled = false;
			this.menuItemPaste.Index = 2;
			this.menuItemPaste.Text = "Paste";
			this.menuItemPaste.Click += new System.EventHandler(this.menuItemPaste_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Enabled = false;
			this.menuItemDelete.Index = 3;
			this.menuItemDelete.Text = "Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "-";
			// 
			// menuItemSelectAll
			// 
			this.menuItemSelectAll.Enabled = false;
			this.menuItemSelectAll.Index = 5;
			this.menuItemSelectAll.Text = "Select All";
			this.menuItemSelectAll.Click += new System.EventHandler(this.menuItemSelectAll_Click);
			// 
			// gm1
			// 
			this.gm1.AutoBind = true;
			this.gm1.BitmapBackColor = System.Drawing.Color.Gainsboro;
			this.gm1.Font = null;
			this.gm1.MenuItemBackColorEnd = System.Drawing.Color.Gainsboro;
			this.gm1.MenuItemBackColorSelected = System.Drawing.Color.PeachPuff;
			this.gm1.MenuItemBackColorStart = System.Drawing.Color.Snow;
			this.gm1.MenuItemBorderSelected = System.Drawing.Color.Gray;
			this.gm1.MenuItemDithered = true;
			this.gm1.MenuItemForeColor = System.Drawing.Color.Navy;
			this.gm1.MenuItemForeColorDisabled = System.Drawing.Color.Gray;
			// 
			// ChatBox
			// 
			this.Controls.Add(this.rtb1);
			this.Controls.Add(this.tb1);
			this.Name = "ChatBox";
			this.Size = new System.Drawing.Size(352, 80);
			this.ResumeLayout(false);

		}
		#endregion

		#region Image Pops Handler
		private void OnEmoticonClicked(object sender, ImageListPopupEventArgs e)
		{
			rtb1.InsertImage(emoticons.Images[e.SelectedItem]);
		}

		private void OnColorsClicked(object sender, ImageListPopupEventArgs e)
		{
			rtb1.TextColor = (RtfColor)e.SelectedItem;
		}
		#endregion

		#region Toolbar button handlers
		/// <summary>
		///     Handler for the toolbar button click event
		/// </summary>
		private void tb1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			//Switch based on the tooltip of the button pressed
			//OR: This could be changed to switch on the actual button pressed (e.g. e.Button and the case would be tbbBold)

			string s = (string)e.Button.Tag;

			switch(s.ToLower())
			{
				case "bold": 
				{
					//using bitwise Exclusive OR to flip-flop the value
					rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Bold);
				} break;
				case "italic": 
				{
					//using bitwise Exclusive OR to flip-flop the value
					rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Italic);
				} break;
				case "underline":
				{
					//using bitwise Exclusive OR to flip-flop the value
					rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Underline);
                } break;
				case "strikeout":
				{
					//using bitwise Exclusive OR to flip-flop the value
					rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Strikeout);
				} break;
				case "left":
				{
					//change horizontal alignment to left
					rtb1.SelectionAlignment = HorizontalAlignment.Left; 
				} break;
				case "center":
				{
					//change horizontal alignment to center
					rtb1.SelectionAlignment = HorizontalAlignment.Center;
				} break;
				case "right":
				{
					//change horizontal alignment to right
					rtb1.SelectionAlignment = HorizontalAlignment.Right;
				} break;
				case "color":
				{
					Point pt = PointToScreen(new Point(e.Button.Rectangle.Left, e.Button.Rectangle.Bottom));
					colorPop.Show(pt.X, pt.Y);
				} break;
				case "emoticons":
				{
					Point pt = PointToScreen(new Point(e.Button.Rectangle.Left, e.Button.Rectangle.Bottom));
					emoPop.Show(pt.X, pt.Y);
				} break;
				case "undo":
				{
					rtb1.Undo();
				} break;
				case "redo":
				{
					rtb1.Redo();
				} break;
				case "open":
				{
					try
					{
						if (ofd1.ShowDialog() == DialogResult.OK && ofd1.FileName.Length > 0)
							rtb1.LoadFile(ofd1.FileName, RichTextBoxStreamType.RichText);
					}
					catch (ArgumentException ae)
					{
						if(ae.Message == "Invalid file format.")
							MessageBox.Show("There was an error loading the file: " + ofd1.FileName);				
					}
				} break;
				case "save":
				{
					if(sfd1.ShowDialog() == DialogResult.OK && sfd1.FileName.Length > 0)
						rtb1.SaveFile(sfd1.FileName);	
				} break;
			} //end select
			UpdateToolbar(); //Update the toolbar buttons
		}
		#endregion

		#region Update Toolbar
		/// <summary>
		///     Update the toolbar button statuses
		/// </summary>
		public void UpdateToolbar()
		{
			if (rtb1.SelectionFont != null) //make sure there is a selected font
			{
				tbbBold.Pushed		= rtb1.SelectionFont.Bold; //bold button
				tbbItalic.Pushed	= rtb1.SelectionFont.Italic; //italic button
				tbbUnderline.Pushed	= rtb1.SelectionFont.Underline; //underline button
				tbbStrikeout.Pushed	= rtb1.SelectionFont.Strikeout; //strikeout button
				tbbLeft.Pushed		= (rtb1.SelectionAlignment == HorizontalAlignment.Left); //justify left
				tbbCenter.Pushed	= (rtb1.SelectionAlignment == HorizontalAlignment.Center); //justify center
				tbbRight.Pushed		= (rtb1.SelectionAlignment == HorizontalAlignment.Right);	//justify right
			}
		}
		#endregion

		#region Update Toolbar Seperators
		private void UpdateToolbarSeperators()
		{
			//Save & Open
			if(!tbbSave.Visible && !tbbOpen.Visible) 
				tbbSeparator3.Visible = false;
			else
				tbbSeparator3.Visible = true;

			//Bold, Italic, Underline, & Strikeout
			if(!tbbBold.Visible && !tbbItalic.Visible && !tbbUnderline.Visible && !tbbStrikeout.Visible)
				tbbSeparator1.Visible = false;
			else
				tbbSeparator1.Visible = true;

			//Left, Center, & Right
			if(!tbbLeft.Visible && !tbbCenter.Visible && !tbbRight.Visible)
				tbbSeparator2.Visible = false;
			else
				tbbSeparator2.Visible = true;

			//Undo & Redo
			if(!tbbUndo.Visible && !tbbRedo.Visible) 
				tbbSeparator4.Visible = false;
			else
				tbbSeparator4.Visible = true;
		}
#endregion

		#region RichTextBox Selection Change
		/// <summary>
		///		Change the toolbar buttons when new text is selected
		/// </summary>
		private void rtb1_SelectionChanged(object sender, System.EventArgs e)
		{
			UpdateToolbar(); //Update the toolbar buttons
		}
		#endregion

		#region Keyboard event handler
		private void rtb1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (rtb1.ReadOnly)
			{
				e.Handled = true;
				return;
			}
			else
			{
				if (e.KeyCode == Keys.Enter)
				{
					bool bold		= rtb1.SelectionFont.Bold; 
					bool italic	= rtb1.SelectionFont.Italic; 
					bool underline	= rtb1.SelectionFont.Underline;
					bool strikeout	= rtb1.SelectionFont.Strikeout; 
					HorizontalAlignment al = rtb1.SelectionAlignment;
					RtfColor rc = rtb1.TextColor;

					if (OnRichTextBoxExKeyDown != null)
						OnRichTextBoxExKeyDown(sender, e);

					if (bold)
						rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Bold);

					if (italic)
						rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Italic);

					if (underline)
						rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Underline);

					if (strikeout)
						rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style ^ FontStyle.Strikeout);

					rtb1.SelectionAlignment = al;

					rtb1.TextColor = rc;

					UpdateToolbar();
				}
			}
		}

		private void rtb1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (txt2emo.Lookup(e.KeyChar))
			{
				string word = txt2emo.Word;
				int id = txt2emo.EmoId;

				if (word.Length > 0 && id >= 0)
				{
					string sText = rtb1.Text + e.KeyChar;
					int _index = sText.IndexOf(word);
					if (_index > -1) 
					{
						rtb1.Select(_index, word.Length-1);
						rtb1.InsertImage(emoticons.Images[id]);
						e.Handled = true;
					}
				}
			}
		}
		#endregion

		#region Drag and Drop handler
		private void rtb1_DragEnter(object sender, DragEventArgs e) 
		{
			// If the data is file drop, show effect.
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void rtb1_DragDrop(object sender, DragEventArgs e) 
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				for (int i=0; i<files.Length; i++)
				{
					try
					{
						IntPtr hImgSmall;    //the handle to the system image list
						ShellApi.SHFILEINFO shinfo = new ShellApi.SHFILEINFO();

						hImgSmall = ShellApi.SHGetFileInfo(files[i], 0, ref shinfo,
							(uint)Marshal.SizeOf(shinfo),
							ShellApi.SHGFI_ICON |
							ShellApi.SHGFI_SMALLICON);

						System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
						Bitmap bmp = myIcon.ToBitmap();

						rtb1.AppendDropFile(files[i], bmp);

						// TODO: Send the bytes here...
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message);
						continue;
					}
				}
			}
		}
		#endregion

		#region Context Menu Handler
		private void cm1_Popup(object sender, System.EventArgs e)
		{
			// control menu items here
			if (rtb1.Text.Length > 0)
			{	
				if (rtb1.SelectionLength > 0)
				{
					menuItemCut.Enabled = true;
					menuItemCopy.Enabled = true;
					menuItemDelete.Enabled = true;

					menuItemSelectAll.Enabled = false;
				}
				else
				{
					menuItemCut.Enabled = false;
					menuItemCopy.Enabled = false;
					menuItemDelete.Enabled = false;

					menuItemSelectAll.Enabled = true;
				}
			}

			// make sure clipboard has content
			if((Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) || 
				(Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap)) ||
				(Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf)))
			{
				menuItemPaste.Enabled = true;
			}

			if (rtb1.ReadOnly)
			{
				menuItemCut.Visible = false;
				menuItemDelete.Visible = false;
				menuItemPaste.Visible = false;
			}
		}

		private void menuItemSelectAll_Click(object sender, System.EventArgs e)
		{
			if (rtb1.SelectedText.Length > 0)
				return;

			rtb1.Select(0, rtb1.TextLength);
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (rtb1.SelectedText.Length > 0)
			{
				string s = rtb1.Text.Substring(0, rtb1.SelectionStart) + rtb1.Text.Substring(rtb1.SelectionStart + rtb1.SelectionLength);
				rtb1.Text = s;
			}
		}

		private void menuItemCut_Click(object sender, System.EventArgs e)
		{
			if (rtb1.SelectedText.Length > 0)
				rtb1.Cut();
		}

		private void menuItemCopy_Click(object sender, System.EventArgs e)
		{
			if (rtb1.SelectionLength > 0)
				rtb1.Copy();
		}

		private void menuItemPaste_Click(object sender, System.EventArgs e)
		{
			if((Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) || 
				(Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap)) ||
				(Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf)))
			{
				rtb1.Paste();
			}		
		}
		#endregion

		#region URL link event handler
		private void rtb1_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			ShellExecute shellExecute = new ShellExecute();
			shellExecute.Verb = ShellExecute.OpenFile;
			shellExecute.Path = e.LinkText;			
			shellExecute.OwnerHandle = this.Handle;
			shellExecute.ShowMode = ShellExecute.ShowWindowCommands.SW_SHOWNORMAL;
			shellExecute.Execute();
		}
		#endregion

		#region Public Properties
		/// <summary>
		///     The toolbar that is contained with-in the ChatBox control
		/// </summary>
		[Description("The internal toolbar control"),
		Category("Internal Controls")]
		public ToolBar Toolbar
		{
			get { return tb1; }
		}

		/// <summary>
		///     The RichTextBox that is contained with-in the ChatBox control
		/// </summary>
		[Description("The internal richtextbox control"),
		Category("Internal Controls")]
		public RichTextBoxEx RichTextBoxEx
		{
			get	{ return rtb1; }
		}

		/// <summary>
		///     Read-Only flag for RTF edit control
		/// </summary>
		[Description("Rich Text Box is Read Only"),
		Category("Internal Controls")]
		public Boolean ReadOnly
		{
			get { return rtb1.ReadOnly; }
			set { rtb1.ReadOnly = value; }
		}

		/// <summary>
		///     Show the save button or not
		/// </summary>
		[Description("Show the save button or not"),
		Category("Appearance")]
		public Boolean ShowSave
		{
			get { return tbbSave.Visible; }
			set { tbbSave.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///    Show the open button or not 
		/// </summary>
		[Description("Show the open button or not"),
		Category("Appearance")]
		public Boolean ShowOpen
		{
			get { return tbbOpen.Visible; }
			set	{ tbbOpen.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the color button or not
		/// </summary>
		[Description("Show the color button or not"),
		Category("Appearance")]
		public Boolean ShowColors
		{
			get { return tbbColor.Visible; }
			set { tbbColor.Visible = value; }
		}

		/// <summary>
		///     Show the emoticons button or not
		/// </summary>
		[Description("Show the emoticons button or not"),
		Category("Appearance")]
		public Boolean ShowEmoticons
		{
			get { return tbbEmoticons.Visible; }
			set { tbbEmoticons.Visible = value; }
		}

		/// <summary>
		///     Show the undo button or not
		/// </summary>
		[Description("Show the undo button or not"),
		Category("Appearance")]
		public Boolean ShowUndo
		{
			get { return tbbUndo.Visible; }
			set { tbbUndo.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the redo button or not
		/// </summary>
		[Description("Show the redo button or not"),
		Category("Appearance")]
		public Boolean ShowRedo
		{
			get { return tbbRedo.Visible; }
			set { tbbRedo.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the bold button or not
		/// </summary>
		[Description("Show the bold button or not"),
		Category("Appearance")]
		public Boolean ShowBold
		{
			get { return tbbBold.Visible; }
			set { tbbBold.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the italic button or not
		/// </summary>
		[Description("Show the italic button or not"),
		Category("Appearance")]
		public Boolean ShowItalic
		{
			get { return tbbItalic.Visible; }
			set { tbbItalic.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the underline button or not
		/// </summary>
		[Description("Show the underline button or not"),
		Category("Appearance")]
		public Boolean ShowUnderline
		{
			get { return tbbUnderline.Visible; }
			set { tbbUnderline.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the strikeout button or not
		/// </summary>
		[Description("Show the strikeout button or not"),
		Category("Appearance")]
		public Boolean ShowStrikeout
		{
			get { return tbbStrikeout.Visible; }
			set { tbbStrikeout.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the left justify button or not
		/// </summary>
		[Description("Show the left justify button or not"),
		Category("Appearance")]
		public Boolean ShowLeftJustify
		{
			get { return tbbLeft.Visible; }
			set { tbbLeft.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the right justify button or not
		/// </summary>
		[Description("Show the right justify button or not"),
		Category("Appearance")]
		public Boolean ShowRightJustify
		{
			get { return tbbRight.Visible; }
			set { tbbRight.Visible = value; UpdateToolbarSeperators(); }
		}

		/// <summary>
		///     Show the center justify button or not
		/// </summary>
		[Description("Show the center justify button or not"),
		Category("Appearance")]
		public Boolean ShowCenterJustify
		{
			get { return tbbCenter.Visible; }
			set { tbbCenter.Visible = value; UpdateToolbarSeperators(); }
		}
		#endregion	

		private void UpdateLocale(CultureInfo ci)
		{
			if (tb1.Buttons.Count > 0)
			{
				// Toolbar
				tb1.Buttons[0].ToolTipText = resources.GetString(ChatBoxResourceKeys.SAVE, ci);
				tb1.Buttons[1].ToolTipText = resources.GetString(ChatBoxResourceKeys.OPEN, ci);
				// sep
				tb1.Buttons[3].ToolTipText = resources.GetString(ChatBoxResourceKeys.BOLD, ci);
				tb1.Buttons[4].ToolTipText = resources.GetString(ChatBoxResourceKeys.ITALIC, ci);
				tb1.Buttons[5].ToolTipText = resources.GetString(ChatBoxResourceKeys.UNDERLINE, ci);
				tb1.Buttons[6].ToolTipText = resources.GetString(ChatBoxResourceKeys.STRIKEOUT, ci);
				// sep
				tb1.Buttons[8].ToolTipText = resources.GetString(ChatBoxResourceKeys.LEFT, ci);
				tb1.Buttons[9].ToolTipText = resources.GetString(ChatBoxResourceKeys.CENTER, ci);
				tb1.Buttons[10].ToolTipText = resources.GetString(ChatBoxResourceKeys.RIGHT, ci);
				// sep
				tb1.Buttons[12].ToolTipText = resources.GetString(ChatBoxResourceKeys.UNDO, ci);
				tb1.Buttons[13].ToolTipText = resources.GetString(ChatBoxResourceKeys.REDO, ci);
				// sep
				tb1.Buttons[15].ToolTipText = resources.GetString(ChatBoxResourceKeys.COLOR, ci);
				tb1.Buttons[16].ToolTipText = resources.GetString(ChatBoxResourceKeys.EMOTICONS, ci);
			}

			// Context menu
			menuItemCut.Text = resources.GetString(ChatBoxResourceKeys.CUT, ci);
			menuItemCopy.Text = resources.GetString(ChatBoxResourceKeys.COPY, ci);
			menuItemPaste.Text = resources.GetString(ChatBoxResourceKeys.PASTE, ci);
			menuItemDelete.Text = resources.GetString(ChatBoxResourceKeys.DELETE, ci);
			menuItemSelectAll.Text = resources.GetString(ChatBoxResourceKeys.SELECTALL, ci);

			this.Invalidate();
		}

		private void OnLocaleChanged(object sender, LocaleEventArgs e)
		{
			UpdateLocale(e.Culture);
		}

        public void LoadFile(string filePath)
        {
            rtb1.LoadFile(filePath, RichTextBoxStreamType.RichText);            
        }

        public void SaveFile(string filePath)
        {
            rtb1.SaveFile(filePath);            
        }

        public void Clear()
        {
            rtb1.Text = "";
        }
	} //end class
} //end namespace

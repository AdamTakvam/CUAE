using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Metreos.Toolset.NotifyWindow
{
	/// <summary>
	/// Summary description for OutlookNotifier.
	/// </summary>
	public class OutlookNotifier : System.Windows.Forms.Form
	{
		public enum BackgroundStyles { BackwardDiagonalGradient, ForwardDiagonalGradient, HorizontalGradient, VerticalGradient, Solid };

		private System.Windows.Forms.Timer timerFadin;
		private System.Windows.Forms.Timer timerFadout;
		private System.ComponentModel.IContainer components;

		public BackgroundStyles BackgroundStyle;
		public System.Drawing.Drawing2D.Blend Blend;
		public System.Drawing.Color GradientColor;

		protected Rectangle rClose, rDisplay, rScreen, rGlobDisplay;
		protected int ActualWidth, ActualHeight;
		protected int offsetX, offsetY;
		protected bool dragging;
		private Metreos.Toolset.ImageButton imageButtonOptions;
		private Metreos.Toolset.ImageButton imageButton1;
		private System.Windows.Forms.ContextMenu cm1;
		private System.Windows.Forms.MenuItem menuItemAnswerCall;
		private System.Windows.Forms.MenuItem menuItemVoiceMail;
		private System.Windows.Forms.MenuItem menuItemCallerProfile;
		private System.Windows.Forms.MenuItem menuItemBlock;
		private System.Windows.Forms.MenuItem menuItemIM;
		private System.Windows.Forms.MenuItem menuItemEmail;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private Metreos.Toolset.GraphicMenu gm1;
		protected int CAPTION_HEIGHT = 8;
        private static int staySolidCount = 0;

		public OutlookNotifier()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			SetStyle (ControlStyles.UserMouse, true);
			SetStyle (ControlStyles.UserPaint, true);
			SetStyle (ControlStyles.AllPaintingInWmPaint, true);		// WmPaint calls OnPaint and OnPaintBackground
			SetStyle (ControlStyles.DoubleBuffer, true);

			BackgroundStyle = BackgroundStyles.VerticalGradient;
			this.BackColor = Color.FromArgb(168, 198, 238);
			GradientColor = Color.FromArgb(214, 231, 252);

			ActualWidth = this.Width;
			ActualHeight = this.Height;

			offsetX = 0;
			offsetY = 0;

			dragging = false;

            rDisplay = new Rectangle(0, 0, ActualWidth, ActualHeight);

			gm1.Init(cm1);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			timerFadin.Stop();
			timerFadout.Stop();

			timerFadin.Dispose();
			timerFadout.Dispose();

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OutlookNotifier));
            this.timerFadin = new System.Windows.Forms.Timer(this.components);
            this.timerFadout = new System.Windows.Forms.Timer(this.components);
            this.imageButtonOptions = new Metreos.Toolset.ImageButton();
            this.cm1 = new System.Windows.Forms.ContextMenu();
            this.menuItemAnswerCall = new System.Windows.Forms.MenuItem();
            this.menuItemVoiceMail = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemIM = new System.Windows.Forms.MenuItem();
            this.menuItemEmail = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemCallerProfile = new System.Windows.Forms.MenuItem();
            this.menuItemBlock = new System.Windows.Forms.MenuItem();
            this.imageButton1 = new Metreos.Toolset.ImageButton();
            this.gm1 = new Metreos.Toolset.GraphicMenu();
            this.SuspendLayout();
            // 
            // timerFadin
            // 
            this.timerFadin.Interval = 20;
            this.timerFadin.Tick += new System.EventHandler(this.timerFadin_Tick);
            // 
            // timerFadout
            // 
            this.timerFadout.Interval = 20;
            this.timerFadout.Tick += new System.EventHandler(this.timerFadout_Tick);
            // 
            // imageButtonOptions
            // 
            this.imageButtonOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imageButtonOptions.BackColor = System.Drawing.Color.Transparent;
            this.imageButtonOptions.ButtonForm = Metreos.Toolset.eButtonForm.Rectangle;
            this.imageButtonOptions.ContextMenu = this.cm1;
            this.imageButtonOptions.HideBorder = true;
            this.imageButtonOptions.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.imageButtonOptions.HottrackImage = ((System.Drawing.Image)(resources.GetObject("imageButtonOptions.HottrackImage")));
            this.imageButtonOptions.Location = new System.Drawing.Point(288, 8);
            this.imageButtonOptions.Name = "imageButtonOptions";
            this.imageButtonOptions.NormalImage = ((System.Drawing.Image)(resources.GetObject("imageButtonOptions.NormalImage")));
            this.imageButtonOptions.OnlyShowBitmap = true;
            this.imageButtonOptions.PressedImage = ((System.Drawing.Image)(resources.GetObject("imageButtonOptions.PressedImage")));
            this.imageButtonOptions.Size = new System.Drawing.Size(23, 23);
            this.imageButtonOptions.TextAlign = Metreos.Toolset.eTextAlign.Bottom;
            this.imageButtonOptions.ToolTip = null;
            this.imageButtonOptions.MouseHover += new System.EventHandler(this.imageButtonOptions_MouseHover);
            // 
            // cm1
            // 
            this.cm1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.menuItemAnswerCall,
                                                                                this.menuItemVoiceMail,
                                                                                this.menuItem4,
                                                                                this.menuItemIM,
                                                                                this.menuItemEmail,
                                                                                this.menuItem5,
                                                                                this.menuItemCallerProfile,
                                                                                this.menuItemBlock});
            // 
            // menuItemAnswerCall
            // 
            this.menuItemAnswerCall.Index = 0;
            this.menuItemAnswerCall.Text = "Answer Call";
            // 
            // menuItemVoiceMail
            // 
            this.menuItemVoiceMail.Index = 1;
            this.menuItemVoiceMail.Text = "Send to Voice Mail";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItemIM
            // 
            this.menuItemIM.Index = 3;
            this.menuItemIM.Text = "Send Instant Message";
            // 
            // menuItemEmail
            // 
            this.menuItemEmail.Index = 4;
            this.menuItemEmail.Text = "Send Email";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 5;
            this.menuItem5.Text = "-";
            // 
            // menuItemCallerProfile
            // 
            this.menuItemCallerProfile.Index = 6;
            this.menuItemCallerProfile.Text = "Show Caller Profile";
            // 
            // menuItemBlock
            // 
            this.menuItemBlock.Index = 7;
            this.menuItemBlock.Text = "Block Caller";
            // 
            // imageButton1
            // 
            this.imageButton1.BackColor = System.Drawing.Color.Transparent;
            this.imageButton1.ButtonForm = Metreos.Toolset.eButtonForm.Rectangle;
            this.imageButton1.HideBorder = true;
            this.imageButton1.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.imageButton1.HottrackImage = ((System.Drawing.Image)(resources.GetObject("imageButton1.HottrackImage")));
            this.imageButton1.Location = new System.Drawing.Point(308, 8);
            this.imageButton1.Name = "imageButton1";
            this.imageButton1.NormalImage = ((System.Drawing.Image)(resources.GetObject("imageButton1.NormalImage")));
            this.imageButton1.OnlyShowBitmap = true;
            this.imageButton1.PressedImage = ((System.Drawing.Image)(resources.GetObject("imageButton1.PressedImage")));
            this.imageButton1.Size = new System.Drawing.Size(23, 23);
            this.imageButton1.TextAlign = Metreos.Toolset.eTextAlign.Bottom;
            this.imageButton1.ToolTip = null;
            this.imageButton1.Click += new System.EventHandler(this.imageButton1_Click);
            this.imageButton1.MouseHover += new System.EventHandler(this.imageButton1_MouseHover);
            // 
            // gm1
            // 
            this.gm1.AutoBind = true;
            this.gm1.BitmapBackColor = System.Drawing.SystemColors.ControlLight;
            this.gm1.Font = null;
            this.gm1.MenuItemBackColorEnd = System.Drawing.Color.AliceBlue;
            this.gm1.MenuItemBackColorSelected = System.Drawing.Color.Lavender;
            this.gm1.MenuItemBackColorStart = System.Drawing.Color.Snow;
            this.gm1.MenuItemBorderSelected = System.Drawing.Color.SteelBlue;
            this.gm1.MenuItemDithered = true;
            this.gm1.MenuItemForeColor = System.Drawing.Color.Navy;
            this.gm1.MenuItemForeColorDisabled = System.Drawing.Color.Gray;
            // 
            // OutlookNotifier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(330, 75);
            this.ControlBox = false;
            this.Controls.Add(this.imageButton1);
            this.Controls.Add(this.imageButtonOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OutlookNotifier";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OutlookNotifier_MouseDown);
            this.Load += new System.EventHandler(this.OutlookNotifier_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OutlookNotifier_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OutlookNotifier_MouseMove);
            this.MouseEnter += new System.EventHandler(this.OutlookNotifier_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.OutlookNotifier_MouseLeave);
            this.ResumeLayout(false);

        }
		#endregion

		private void OutlookNotifier_Load(object sender, System.EventArgs e)
		{
		}

		private void timerFadin_Tick(object sender, System.EventArgs e)
		{
            if(this.Opacity < 1)
                this.Opacity += 0.0125;
            else
            {
                if (staySolidCount > 40)
                    Fadeout();
                else
                    staySolidCount++;
            }
		}

		private void timerFadout_Tick(object sender, System.EventArgs e)
		{
			if(this.Opacity > 0)
				this.Opacity -= 0.0125;
			else
				this.Close();		
		}

		private void Fadeout()
		{
			timerFadin.Stop();
			timerFadout.Enabled = true;
            staySolidCount = 0;
			timerFadout.Start();
		}

		private void OutlookNotifier_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Rectangle rCaptionBar = new Rectangle(0, 0, ActualWidth, CAPTION_HEIGHT);
				if ((e.X <= ActualWidth) && (e.Y <=CAPTION_HEIGHT))
				{
					dragging = true;
					offsetX = e.X;
					offsetY = e.Y;
				}
			}
		}

		private void OutlookNotifier_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				dragging = false;
		}

		private void OutlookNotifier_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dragging)
			{
				this.Left = e.X + Location.X - offsetX;
				this.Top = e.Y + Location.Y - offsetY;
			}
		}

		private void OutlookNotifier_MouseEnter(object sender, System.EventArgs e)
		{
			timerFadin.Stop();
			timerFadout.Stop();

			this.Opacity = 1;
		}

		private void OutlookNotifier_MouseLeave(object sender, System.EventArgs e)
		{
			timerFadout.Enabled = true;
			timerFadout.Start();
		}

		#region Public Methods
		/// <summary>
		/// Sets the width and height of the NotifyWindow.
		/// </summary>
		public void SetDimensions (int width, int height)
		{
			if (width != -1)
				ActualWidth = width;

			if (height != -1)
				ActualHeight = height;
		}

        /// <summary>
        /// show or hide option button
        /// </summary>
        /// <param name="visible"></param>
        public void ShowOptions(bool visible)
        {
            imageButtonOptions.Visible = visible;
        }

        /// <summary>
        /// Stop fading effect
        /// </summary>
        public void StopFading()
        {
            timerFadin.Stop();
            timerFadout.Stop();

            this.Opacity = 1;		
        }

		/// <summary>
		/// Displays the NotifyWindow.
		/// </summary>
		public void Notify(int xo, int yo)
		{
			Width = ActualWidth;
			rScreen = Screen.GetWorkingArea (Screen.PrimaryScreen.Bounds);
			rDisplay = new Rectangle (0, 0, ActualWidth, ActualHeight);
			rClose = new Rectangle (Width - 11, 10, 13, 13);

			// Use unmanaged ShowWindow() and SetWindowPos() instead of the managed Show() to display the window - this method will display
			// the window TopMost, but without stealing focus (namely the SW_SHOWNOACTIVATE and SWP_NOACTIVATE flags)
			ShowWindow (Handle, SW_SHOWNOACTIVATE);

			if (xo != 0 && yo != 0)
			{
				if (yo-ActualHeight < 0)
					SetWindowPos (Handle, HWND_TOPMOST, rScreen.Width-ActualWidth, rScreen.Bottom-ActualHeight, ActualWidth, ActualHeight, SWP_NOACTIVATE);
				else
					SetWindowPos (Handle, HWND_TOPMOST, xo-ActualWidth, yo-ActualHeight, ActualWidth, ActualHeight, SWP_NOACTIVATE);
			}
			else
				SetWindowPos (Handle, HWND_TOPMOST, rScreen.Width-ActualWidth, rScreen.Bottom-ActualHeight, ActualWidth, ActualHeight, SWP_NOACTIVATE);

			this.Opacity = 0;
			timerFadin.Enabled = true;
			timerFadin.Start();
		}
		#endregion

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			/*
			
			// Draw the close button and text.
			drawCloseButton (e.Graphics);

			Font useFont;  Color useColor;
			if (Title != null)
			{
				if (titleHot)
					useFont = TitleHoverFont;
				else
					useFont = TitleFont;
				if (titlePressed)
					useColor = PressedColor;
				else
					useColor = TitleColor;
				using (SolidBrush sb = new SolidBrush (useColor))
					e.Graphics.DrawString (Title, useFont, sb, rTitle, this.StringFormat);
			}

			if (textHot)
				useFont = HoverFont;
			else
				useFont = Font;
			if (textPressed)
				useColor = PressedColor;
			else
				useColor = TextColor;
			using (SolidBrush sb = new SolidBrush (useColor))
				e.Graphics.DrawString (Text, useFont, sb, rText, this.StringFormat);		
			*/	
		}

		protected override void OnPaintBackground (System.Windows.Forms.PaintEventArgs e)
		{
			// First paint the background
			if (BackgroundStyle == BackgroundStyles.Solid)
			{
				using (SolidBrush sb = new SolidBrush (BackColor))
					e.Graphics.FillRectangle (sb, rDisplay);
			}
			else
			{
				LinearGradientMode lgm;
				switch (BackgroundStyle)
				{
					case BackgroundStyles.BackwardDiagonalGradient:
						lgm = LinearGradientMode.BackwardDiagonal;
						break;
					case BackgroundStyles.ForwardDiagonalGradient:
						lgm = LinearGradientMode.ForwardDiagonal;
						break;
					case BackgroundStyles.HorizontalGradient:
						lgm = LinearGradientMode.Horizontal;
						break;
					default:
					case BackgroundStyles.VerticalGradient:
						lgm = LinearGradientMode.Vertical;
						break;
				}
				using (LinearGradientBrush lgb = new LinearGradientBrush (rDisplay, GradientColor, BackColor, lgm))
				{
					if (this.Blend != null)
						lgb.Blend = this.Blend;
					e.Graphics.FillRectangle (lgb, rDisplay);
				}
			}

			// Next draw borders...
			drawBorder (e.Graphics);
		}

		protected virtual void drawBorder (Graphics fx)
		{
			Rectangle rCaptionBar = new Rectangle(0, 0, ActualWidth, CAPTION_HEIGHT);
			Color tc = Color.FromArgb(89, 135, 214);
			Color bc = Color.FromArgb(12, 64, 154);
			using (LinearGradientBrush lgb = new LinearGradientBrush (rCaptionBar, tc, bc, LinearGradientMode.Vertical))
			{
				if (this.Blend != null)
					lgb.Blend = this.Blend;
				fx.FillRectangle (lgb, rCaptionBar);
			}

			SolidBrush sb1 = new SolidBrush (Color.FromArgb(40, 50, 71));
			SolidBrush sb2 = new SolidBrush (Color.FromArgb(249, 249, 251));
			int y1 = 2;
			int x1 = (ActualWidth - 35) / 2;
			Rectangle s1 = new Rectangle(x1, y1, 2, 2);

			int y2 = 3;
			int x2 = (ActualWidth - 35) / 2 + 1;
			Rectangle s2 = new Rectangle(x2, y2, 2, 2);

			fx.FillRectangle (sb1, s1);
			fx.FillRectangle (sb2, s2);
			for (int i=0; i<8; i++)
			{
				s1.Offset(4, 0);
				s2.Offset(4, 0);
				fx.FillRectangle (sb1, s1);
				fx.FillRectangle (sb2, s2);
			}

			Pen p = new Pen(Color.FromArgb(0, 0, 128));
			fx.DrawRectangle(Pens.Blue, 0, 0, ActualWidth-1, ActualHeight-1);			
		}

		#region P/Invoke
		// DrawThemeBackground()
		protected const Int32 WP_CLOSEBUTTON = 18;
		protected const Int32 CBS_NORMAL = 1;
		protected const Int32 CBS_HOT = 2;
		protected const Int32 CBS_PUSHED = 3;
		[StructLayout (LayoutKind.Explicit)]
			protected struct RECT
		{
			[FieldOffset (0)] public Int32 Left;
			[FieldOffset (4)] public Int32 Top;
			[FieldOffset (8)] public Int32 Right;
			[FieldOffset (12)] public Int32 Bottom;

			public RECT (System.Drawing.Rectangle bounds)
			{
				Left = bounds.Left;
				Top = bounds.Top;
				Right = bounds.Right;
				Bottom = bounds.Bottom;
			}
		}

		// SetWindowPos()
		protected const Int32 HWND_TOPMOST = -1;
		protected const Int32 SWP_NOACTIVATE = 0x0010;

		// ShowWindow()
		protected const Int32 SW_SHOWNOACTIVATE = 4;

		// UxTheme.dll
		[DllImport ("UxTheme.dll")]
		protected static extern Int32 IsThemeActive();
		[DllImport ("UxTheme.dll")]
		protected static extern IntPtr OpenThemeData (IntPtr hWnd, [MarshalAs (UnmanagedType.LPTStr)] string classList);
		[DllImport ("UxTheme.dll")]
		protected static extern void CloseThemeData (IntPtr hTheme);
		[DllImport ("UxTheme.dll")]
		protected static extern void DrawThemeBackground (IntPtr hTheme, IntPtr hDC, Int32 partId, Int32 stateId, ref RECT rect, ref RECT clipRect);

		// user32.dll
		[DllImport ("user32.dll")]
		protected static extern bool ShowWindow (IntPtr hWnd, Int32 flags);
		[DllImport ("user32.dll")]
		protected static extern bool SetWindowPos (IntPtr hWnd, Int32 hWndInsertAfter, Int32 X, Int32 Y, Int32 cx, Int32 cy, uint uFlags);
		#endregion

		private void imageButtonOptions_MouseHover(object sender, System.EventArgs e)
		{
			timerFadin.Stop();
			timerFadout.Stop();

			this.Opacity = 1;		
		}

		private void imageButton1_MouseHover(object sender, System.EventArgs e)
		{
			timerFadin.Stop();
			timerFadout.Stop();

			this.Opacity = 1;		
		}

		private void imageButton1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}

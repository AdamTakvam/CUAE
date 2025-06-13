using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Resources.Images;
using Metreos.Max.GlobalEvents;



namespace Metreos.Max.Debugging
{
    ///<summary>Debugger remote console view</summary>
    public class MaxConsoleView: RichTextBox
    {
        protected MaxConsoleWindow parent;


        public MaxConsoleView(MaxConsoleWindow parent)
        {
            this.parent   = parent;
            this.ReadOnly = true;
            this.Dock     = DockStyle.Fill;
            this.Location = new Point(0, 0);
            this.TabIndex = 0;
            this.Font     = new Font(FontFamily.GenericMonospace, Config.outputWindowFontSize);
            this.WordWrap = false;
            this.BorderStyle = BorderStyle.None; 
            colorBackground  = Config.ConsoleBackground? Color.White: Color.Black;           
            this.SetColor();
        }


        /// <summary>Post-construction initialization</summary>
        public void Create()
        {
            MaxDebugUtil util = MaxDebugger.Instance.Util;
            mcColor   = new MenuCommand(Const.menuConsoleInvertColors, 
                        new EventHandler(OnMenuColor)); 
            mcClear   = new MenuCommand(Const.menuOutputClearAll,
                        new EventHandler(OnMenuClear)); 
            mcStart   = new MenuCommand(Const.menuConsoleStart,                     
                        new EventHandler(util.OnMenuStartConsole));
            mcStop    = new MenuCommand(Const.menuConsoleStop,                     
                        new EventHandler(util.OnMenuStopConsole));
        }


        /// <summary>Write text</summary>
        public void Write(string text)
        {
            this.AppendText(text); 
            this.MonitorLineCount();
            this.ScrollToEnd();                   
        }


        /// <summary>Write a line of text followed by carriage return</summary>
        public void WriteLine(string text)
        {
            this.AppendText(text); 
            this.AppendText(Const.crlf);
            this.MonitorLineCount();
            this.ScrollToEnd();                   
        }


        /// <summary>Return total lines</summary>
        public int LineCount()
        {
            return this.Lines.Length;
        }


        /// <summary>Scroll to last page view</summary>
        public void ScrollToEnd()
        {     
            IntPtr handle = Utl.GetFocus();
            if (handle == IntPtr.Zero) return;  

            Control previouslyFocused = Form.FromHandle(handle);
            if (previouslyFocused == null) return;

            this.Focus();
            this.SelectionStart = this.TextLength;

            previouslyFocused.Focus();
        }


        /// <summary>Ensure # lines does not exceed configured maximum</summary>
        public void MonitorLineCount()
        {
            int  numlines  = this.LineCount();
            if  (numlines  < Config.outputWindowMaxLines) return;

            int  startline = numlines - Config.outputWindowSaveLines;
            if  (startline < 1) return;
     
            string[] oldtext = this.Lines;
            string[] newtext = new string[numlines - startline];

            for(int i=0; i < newtext.Length; i++)
                newtext[i] = oldtext[i+startline];

            this.Clear();
            this.Lines = newtext;        
        }


        ///<summary>Act on context click</summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Right) this.PopContextMenu();      
        }


        ///<summary>Pop background context menu</summary>
        public void PopContextMenu()
        {
            PopupMenu contextmenu = new PopupMenu();

            contextmenu.MenuCommands.Add(parent.Streaming? mcStop: mcStart); 
            contextmenu.MenuCommands.Add(mcClear);
            contextmenu.MenuCommands.Add(MaxDebugListView.separator);
            contextmenu.MenuCommands.Add(mcColor); 
            MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
        }


        /// <summary>Invoked on Clear All selected from context menu</summary>
        public void OnMenuClear(object sender, EventArgs e)
        {       
            this.Clear();
        }


        /// <summary>Invoked on Invert Background selected from context menu</summary>
        public void OnMenuColor(object sender, EventArgs e)
        {       
            this.SetColor();

            Config.ConsoleBackground = colorBackground == Color.Black;
        }


        /// <summary>Toggle console window background color</summary>
        private void SetColor()
        {
            if  (colorBackground != Color.Black)         
                 colorBackground  = Color.Black;
            else colorBackground  = Color.White;

            this.BackColor = colorBackground;

            this.ForeColor = colorBackground == Color.Black?
                 Color.LightGray: Color.FromArgb(60,60,60);
        }

        private MenuCommand mcClear;
        private MenuCommand mcColor;
        private MenuCommand mcStart;
        private MenuCommand mcStop;
        private Color colorBackground;
        
    } // class MaxConsoleView

} // namespace

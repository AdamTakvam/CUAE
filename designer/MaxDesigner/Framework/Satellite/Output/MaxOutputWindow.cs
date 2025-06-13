using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Framework.Satellite.Output
{
    /// <summary>Native IDE output window control</summary>
    public class MaxOutputWindow: System.Windows.Forms.RichTextBox, MaxSatelliteWindow
    {
        private System.ComponentModel.Container components = null;
        private MaxMain main;
        private MenuCommand mcClear;
        private MenuCommand mcGoToNode;
        private MenuCommand mcSeparator;
        private static MaxOutputWindow thisx;


        public MaxOutputWindow(MaxMain main)
        {
            this.main = main; 
            InitializeComponent();
            this.Font      = new Font(FontFamily.GenericMonospace, Config.outputWindowFontSize);
            int  rgbno     = Config.outputWindowFontGray;
            this.ForeColor = Color.FromArgb(rgbno,rgbno,rgbno);
            this.WordWrap  = Config.outputWindowWordWrap;
            this.KeyDown  += new KeyEventHandler(this.OnKeyDown);
            thisx = this;

            this.mcClear     = new MenuCommand (Const.menuOutputClearAll,
                               new EventHandler(OnMenuClearAll)); 
            this.mcGoToNode  = new MenuCommand (Const.menuOutputGoToNode,
                               new EventHandler(OnMenuGoToNode)); 
            this.mcSeparator = new MenuCommand(Const.dash);
        }


        /// <summary>Write a line of text followed by carriage return</summary>
        public void WriteLine(string text)
        {
            if  (this.IsDisposed) return;

            this.AppendText(text); 
            this.AppendText(Const.crlf);

            this.MonitorLineCount();
            this.ScrollToEnd();                   
            main.ShowOutputWindow();              // Ensure output window in view
        }


        /// <summary>Static write line, no frills</summary>
        public static void Trace(string text)
        {
            if (thisx.IsDisposed) return;

            thisx.AppendText(text); 
            thisx.AppendText(Const.crlf);
        }


        /// <summary>Copy selection to clipboard</summary>
        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            // We will not see this key combo here if the Ctrl+C menu shortcut is .
            // assigned. In that case, MaxMenuHandler.OnEditCopy invokes 
            // MaxOutputWindow.Copy() when focus is here.

            if ((e.KeyData == Keys.C) && e.Control && !e.Alt && !e.Shift)
            {
                this.Copy(); 
            }
        }


        /// <summary>Trap and handle esc</summary>
        protected override bool IsInputKey(Keys keyData)
        {
            // When focus is here, we want the escape key to clear our selected text,
            // rather than clearing the max canvas selection

            if ((keyData & Keys.KeyCode) == Keys.Escape && (keyData & Keys.Modifiers) == Keys.None)         
                this.Select(0, 0);  
                                
            return base.IsInputKey (keyData);
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

            // Could not grab handle, so we won't scroll
            if(handle == IntPtr.Zero) return;  

            Control previouslyFocused = Form.FromHandle(handle);

            // Could not find control, so we won't scroll
            if(previouslyFocused == null) return;

            this.Focus();
            this.SelectionStart = this.TextLength;

            previouslyFocused.Focus();
            #region Alternative Scrolling Mechanism
            //      float fontHt = this.Font.GetHeight(Graphics.FromHwnd(this.Handle));
            //      int linesVisible = ((int)(this.Height / fontHt) - 4);
            //      if (linesVisible <= 0) return;
            //      Utl.SendMessage(this.Handle, Const.EM_LINESCROLL, 0, 99999);      
            //      Utl.SendMessage(this.Handle, Const.EM_LINESCROLL, 0, 0 - linesVisible);
            #endregion
        }


        /// <summary>Ensure # lines in output window does not exceed configured max</summary>
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


        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) this.PopContextMenu(e);
            base.OnMouseUp (e);
        }


        /// <summary>Show output window context menu</summary>
        protected void PopContextMenu(MouseEventArgs e)
        {
            PopupMenu contextmenu = new PopupMenu();
            contextmenu.MenuCommands.AddRange
                (new MenuCommand[] { mcClear, mcSeparator, mcGoToNode });

            Point pt = new Point(e.X, e.Y);
            pt = this.PointToScreen(pt);
            mcGoToNode.Tag = pt;
            mcGoToNode.Enabled = main.ProjectExists;
      
            contextmenu.TrackPopup(pt);
        }


        /// <summary>Clear All selected from menu</summary>
        protected void OnMenuClearAll(object sender, EventArgs e) 
        {
            this.Clear();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // "GoTo Node" support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Go To Node selected from menu</summary>
        protected void OnMenuGoToNode(object sender, EventArgs e) 
        {
            Point pt = (Point)(sender as MenuCommand).Tag;
            OnMenuGoToNode(pt);
        }


        public void OnMenuGoToNode()
        {
            OnMenuGoToNode(Const.point00);
        }


        public void OnMenuGoToNode(Point pt)
        {
            MaxNodeIdDlg dlg = new MaxNodeIdDlg(pt);
            if (DialogResult.OK != dlg.ShowDialog()) return;

            long nodeID = dlg.NodeID;             
            this.GoToNode(nodeID);     
        }


        /// <summary>Navigate to node specified</summary>
        protected void GoToNode(long nodeID)
        {
            string[] appFileRelPaths = MaxMainUtil.PeekProjectFileFiles
                (MaxMain.ProjectPath, Const.xmlValFileSubtypeApp);
            if (appFileRelPaths == null) return;
            NodeSearchInfo  info = null;

            foreach(string relpath in appFileRelPaths)
            {
                string appFileFullPath = MaxMain.ProjectFolder + Const.bslash + relpath;
                info = this.FindNode(appFileFullPath, nodeID);
                if (info != null) break;
            }

            if (info == null) return;

            this.NavigateToErrorApp(info);  // Signal Max to navigate to app
                                            // Signal Max to navigate to node
            main.SignalUserInput(new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.GoToNode, info.appname, nodeID)); 
        }


        /// <summary>Make error app the current app</summary>
        protected void NavigateToErrorApp(NodeSearchInfo info)
        {
            if (main.CurrentViewType.Equals(MaxMain.ViewTypes.App) 
             && main.CurrentViewName.Equals(info.appname)) return;

            main.Dialog.OnOpenScriptRequest(info.appname, null);
        }


        /// <summary>Identify canvas and invoke node parse on each canvas found</summary>
        protected NodeSearchInfo FindNode(string appfilepath, long nodeID)
        {
            ArrayList projectPackages = new ArrayList();
            XmlTextReader  rdr  = null;
            NodeSearchInfo info = new NodeSearchInfo();
            info.searchid = nodeID;

            try                                    
            {  rdr = new XmlTextReader(appfilepath); 

                XmlDocument xdoc = new XmlDocument();                         
                xdoc.Load(rdr);                                            
                XmlNode root = xdoc.DocumentElement;  // <Application>
                if (root.Name != Const.xmlEltApplication) return null;       
                info.appname = Utl.XmlAttr(root, Const.xmlAttrName);

                foreach(XmlNode appcomponent in root)
                {                                     // <canvas>
                    if (appcomponent.Name.Equals(Const.xmlEltCanvas)
                     && FindNodeInCanvas(appcomponent, info))
                        break;   
                } 
            }
            catch { }

            if (rdr != null) rdr.Close();
            return info.nodeid == info.searchid? info: null;
        }


        /// <summary>Check this "node" xmlnode to see if a match for search ID</summary>
        protected bool FindNodeInCanvas(XmlNode canvasnode, NodeSearchInfo info)
        {
            info.funcname = Utl.XmlAttr(canvasnode, Const.xmlAttrName);

            foreach(XmlNode nodenode in canvasnode)
            {                                     // <node>
                if (!nodenode.Name.Equals(Const.xmlEltNode)) continue;
                info.nodeID    = Utl.XmlAttr(nodenode, Const.xmlAttrID);
                info.nodeid    = Utl.atol(info.nodeID);
                if (info.nodeid != info.searchid) continue;

                info.nodename  = Utl.XmlAttr(nodenode, Const.xmlAttrName);
                info.toolgroup = Utl.XmlAttr(nodenode, Const.xmlAttrPath);
                return true;
            }

            return false;
        }
  

        /// <summary>Node search parameters and return arguments</summary>
        public class NodeSearchInfo
        {
            public string appname;
            public string funcname;
            public string nodename;
            public long   searchid;
            public long   nodeid;
            public string nodeID;
            public string toolgroup;
        }

        protected override void Dispose(bool disposing)
        {
            if  (disposing && components != null) components.Dispose();     
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        
        private void InitializeComponent()
        {
            // 
            // OutputWindow
            // 
            this.ReadOnly = true;
            this.Size = new System.Drawing.Size(300, 300);

        }
        #endregion

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get{ return SatelliteTypes.Output; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuViewOutput; } 
        }

        #endregion
    }
}

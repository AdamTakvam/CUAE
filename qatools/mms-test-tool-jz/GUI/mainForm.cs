using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Configuration;
using Microsoft.Win32;

using Metreos.MMSTestTool.Parser;
using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Commands;

using CommonAST				= antlr.CommonAST;
using AST					= antlr.collections.AST;
using RecognitionException	= antlr.RecognitionException;
using TokenStreamException	= antlr.TokenStreamException;


namespace Metreos.MMSTestTool
{
    /// <summary>
    /// Summary description for mainForm.
    /// </summary>
    public class mainForm : System.Windows.Forms.Form
    {
        #region Forms object declarations
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuItem fileMenu;
        private System.Windows.Forms.MenuItem loadMenuItem;
        private System.Windows.Forms.MenuItem editMenu;
        private System.Windows.Forms.MenuItem preferencesMenuItem;
        private System.Windows.Forms.TreeView fixtureTreeView;
        private System.Windows.Forms.Label loadedFixturesLabel;
        private System.Windows.Forms.Label outputLabelLabel;
        private System.Windows.Forms.RichTextBox scriptDisplay;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.Label scriptElementDisplayLabel;
        private System.Windows.Forms.RichTextBox transactionResultTextBox;
        private System.Windows.Forms.Label transactionResultOutputLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.fileMenu = new System.Windows.Forms.MenuItem();
            this.loadMenuItem = new System.Windows.Forms.MenuItem();
            this.editMenu = new System.Windows.Forms.MenuItem();
            this.preferencesMenuItem = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.fixtureTreeView = new System.Windows.Forms.TreeView();
            this.loadedFixturesLabel = new System.Windows.Forms.Label();
            this.outputLabelLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.scriptDisplay = new System.Windows.Forms.RichTextBox();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.scriptElementDisplayLabel = new System.Windows.Forms.Label();
            this.transactionResultTextBox = new System.Windows.Forms.RichTextBox();
            this.transactionResultOutputLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.fileMenu,
                                                                                      this.editMenu});
            // 
            // fileMenu
            // 
            this.fileMenu.Index = 0;
            this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                     this.loadMenuItem});
            this.fileMenu.Text = "&File";
            this.fileMenu.Click += new System.EventHandler(this.fileMenu_Click);
            // 
            // loadMenuItem
            // 
            this.loadMenuItem.Index = 0;
            this.loadMenuItem.Text = "&Load";
            this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // editMenu
            // 
            this.editMenu.Index = 1;
            this.editMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                     this.preferencesMenuItem});
            this.editMenu.Text = "&Edit";
            this.editMenu.Click += new System.EventHandler(this.editMenu_Click);
            // 
            // preferencesMenuItem
            // 
            this.preferencesMenuItem.Index = 0;
            this.preferencesMenuItem.Text = "&Preferences";
            this.preferencesMenuItem.Click += new System.EventHandler(this.preferencesMenuItem_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 691);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(768, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar1_PanelClick);
            // 
            // fixtureTreeView
            // 
            this.fixtureTreeView.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.fixtureTreeView.CheckBoxes = true;
            this.fixtureTreeView.ImageIndex = -1;
            this.fixtureTreeView.Location = new System.Drawing.Point(16, 32);
            this.fixtureTreeView.Name = "fixtureTreeView";
            this.fixtureTreeView.SelectedImageIndex = -1;
            this.fixtureTreeView.Size = new System.Drawing.Size(272, 560);
            this.fixtureTreeView.TabIndex = 2;
            this.fixtureTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.fixtureTreeView_AfterCheck);
            this.fixtureTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fixtureTreeView_AfterSelect);
            // 
            // loadedFixturesLabel
            // 
            this.loadedFixturesLabel.Location = new System.Drawing.Point(16, 8);
            this.loadedFixturesLabel.Name = "loadedFixturesLabel";
            this.loadedFixturesLabel.Size = new System.Drawing.Size(120, 16);
            this.loadedFixturesLabel.TabIndex = 3;
            this.loadedFixturesLabel.Text = "Loaded Fixtures:";
            // 
            // outputLabelLabel
            // 
            this.outputLabelLabel.Location = new System.Drawing.Point(312, 8);
            this.outputLabelLabel.Name = "outputLabelLabel";
            this.outputLabelLabel.Size = new System.Drawing.Size(184, 24);
            this.outputLabelLabel.TabIndex = 4;
            this.outputLabelLabel.Text = "Output:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "mmt";
            this.openFileDialog1.Filter = "Tester Script|*.mmt|All Files|*.*";
            this.openFileDialog1.ValidateNames = false;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // scriptDisplay
            // 
            this.scriptDisplay.AcceptsTab = true;
            this.scriptDisplay.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.scriptDisplay.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scriptDisplay.Location = new System.Drawing.Point(312, 288);
            this.scriptDisplay.Name = "scriptDisplay";
            this.scriptDisplay.ReadOnly = true;
            this.scriptDisplay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.scriptDisplay.Size = new System.Drawing.Size(440, 168);
            this.scriptDisplay.TabIndex = 7;
            this.scriptDisplay.TabStop = false;
            this.scriptDisplay.Text = "";
            this.scriptDisplay.WordWrap = false;
            // 
            // outputTextBox
            // 
            this.outputTextBox.AcceptsTab = true;
            this.outputTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.outputTextBox.Location = new System.Drawing.Point(312, 32);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(440, 216);
            this.outputTextBox.TabIndex = 8;
            this.outputTextBox.TabStop = false;
            this.outputTextBox.Text = "";
            // 
            // scriptElementDisplayLabel
            // 
            this.scriptElementDisplayLabel.AutoSize = true;
            this.scriptElementDisplayLabel.Location = new System.Drawing.Point(312, 264);
            this.scriptElementDisplayLabel.Name = "scriptElementDisplayLabel";
            this.scriptElementDisplayLabel.Size = new System.Drawing.Size(138, 16);
            this.scriptElementDisplayLabel.TabIndex = 9;
            this.scriptElementDisplayLabel.Text = "Script Element Description";
            this.scriptElementDisplayLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // transactionResultTextBox
            // 
            this.transactionResultTextBox.AcceptsTab = true;
            this.transactionResultTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.transactionResultTextBox.Location = new System.Drawing.Point(312, 496);
            this.transactionResultTextBox.Name = "transactionResultTextBox";
            this.transactionResultTextBox.ReadOnly = true;
            this.transactionResultTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.transactionResultTextBox.Size = new System.Drawing.Size(440, 192);
            this.transactionResultTextBox.TabIndex = 10;
            this.transactionResultTextBox.Text = "";
            this.transactionResultTextBox.WordWrap = false;
            // 
            // transactionResultOutputLabel
            // 
            this.transactionResultOutputLabel.AutoSize = true;
            this.transactionResultOutputLabel.Location = new System.Drawing.Point(312, 472);
            this.transactionResultOutputLabel.Name = "transactionResultOutputLabel";
            this.transactionResultOutputLabel.Size = new System.Drawing.Size(107, 16);
            this.transactionResultOutputLabel.TabIndex = 11;
            this.transactionResultOutputLabel.Text = "Transaction Results:";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(24, 624);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(96, 32);
            this.startButton.TabIndex = 12;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(184, 624);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(96, 32);
            this.stopButton.TabIndex = 13;
            this.stopButton.Text = "S&top";
            // 
            // mainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(768, 713);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.transactionResultOutputLabel);
            this.Controls.Add(this.transactionResultTextBox);
            this.Controls.Add(this.scriptElementDisplayLabel);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.scriptDisplay);
            this.Controls.Add(this.outputLabelLabel);
            this.Controls.Add(this.loadedFixturesLabel);
            this.Controls.Add(this.fixtureTreeView);
            this.Controls.Add(this.statusBar);
            this.Menu = this.mainMenu1;
            this.Name = "mainForm";
            this.Text = "MMS Tester";
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // List used to store fixtures read in from the file
        private ArrayList fixtureList = new ArrayList();

        public mainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            // Pull the path of the XML Command description file from the registry, if field exists
            RegistryKey regKey = Constants.regMain;
            regKey = regKey.CreateSubKey(Constants.regMetKey + "\\" + Constants.regMMSTesterKey);
            string xmlDescription = regKey.GetValue(Constants.regDescriptionKey) as string;
            if (!(xmlDescription == null))
                Commands.CommandDescriptionContainerHandler.ReadXmlDescription(xmlDescription);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new mainForm());
        }

        #region Currently Unused Event Methods
        private void fileMenu_Click(object sender, System.EventArgs e)
        {

        }

        private void statusBar1_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {
        
        }

        private void outputLabel_Click(object sender, System.EventArgs e)
        {
        
        }

        private void fixtureTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            PrintNode(e.Node, scriptDisplay);
            PrintScriptElementResult(e.Node);
        }

        private void editMenu_Click(object sender, System.EventArgs e)
        {
        
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
        
        }

        private void outputLabel_Click_1(object sender, System.EventArgs e)
        {
        
        }

        private void label1_Click(object sender, System.EventArgs e)
        {
        
        }
        #endregion

        #region Methods relating to printing Nodes/Results to TextBoxes
        private void PrintScriptElementResult(TreeNode node)
        {
            transactionResultTextBox.Clear();
            object tag = node.Tag;

            if (tag is Command)
            {
                Command objectToPrint = tag as Command;
                TextBoxWrite(objectToPrint.Result.Replace("\t", Constants.TAB), transactionResultTextBox);
            }
            else if (tag is Script)
            {
                Script objectToPrint = tag as Script;
                TextBoxWrite(objectToPrint.Result.Replace("\t", Constants.TAB), transactionResultTextBox);
            }
            else if (tag is TestFixture)
            {
                TestFixture objectToPrint = tag as TestFixture;
                TextBoxWrite(objectToPrint.Result.Replace("\t", Constants.TAB), transactionResultTextBox);
            }
            else if (node.Parent == null)
            {
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sBuilder.Append("Test Fixtures:\n");
                ArrayList fixtureList = tag as ArrayList;
                foreach (TestFixture tf in fixtureList)
                    sBuilder.AppendFormat("\t{0} {1}{2}{3}\n", Constants.FIXTURE_STRING, 
                        (tf.Name == string.Empty || tf.Name == null) ? "- <Name was not specified>" : tf.Name,
                            ":\t", tf.Result);
                TextBoxWrite(sBuilder.ToString().Replace("\t", Constants.TAB), transactionResultTextBox);
            }
        }
            
        private void PrintNode(TreeNode node, RichTextBox output)
        {
            object tag = node.Tag;

            if (tag is Command)
            {
                Command objectToPrint = tag as Command;
                output.Text = objectToPrint.ToString().Replace("\t", Constants.TAB);
            }
            else if (tag is Script)
            {
                Script objectToPrint = tag as Script;
                output.Text = objectToPrint.ToString().Replace("\t", Constants.TAB);
            }
            else if (tag is TestFixture)
            {
                TestFixture objectToPrint = tag as TestFixture;
                output.Text = objectToPrint.ToString().Replace("\t", Constants.TAB);
            }
            else if (node.Parent == null)
            {
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sBuilder.Append("Test Fixtures:\n");
                ArrayList fixtureList = tag as ArrayList;
                foreach (TestFixture tf in fixtureList)
                    sBuilder.AppendFormat("\t{0} {1}\n", Constants.FIXTURE_STRING, 
                        (tf.Name == string.Empty || tf.Name == null) ? "- <Name was not specified>" : tf.Name);
                output.Text = sBuilder.ToString().Replace("\t", Constants.TAB);
            }
            else
                return;
        }


        private void TextBoxWrite(object objectToPrint, TextBoxBase textbox)
        {
            try
            {
                textbox.Text += objectToPrint.ToString();
            }
            catch {}
        }

        private void TextBoxWriteLine(object objectToPrint, TextBoxBase textbox)
        {
            try
            {
                textbox.Text += objectToPrint.ToString() + "\n";
            }
            catch {}
        }
        #endregion

        private void fixtureTreeView_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Checked)
                {
                    CheckChildNodes(e.Node, true);
                    CheckParentNodes(e.Node, true);
                    e.Node.ExpandAll();
                }
                else
                {
                    CheckChildNodes(e.Node, false);
                }
            }
            else if (e.Node.Parent == null)
            {
                if (e.Node.Checked)
                {
                    startButton.Enabled = true;
                    stopButton.Enabled = false;                
                }
                else
                {
                    startButton.Enabled = false;
                    stopButton.Enabled = false;     
                }
            }
        }


        private void CheckChildNodes(TreeNode node, bool nodeChecked)
        {
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode tn in node.Nodes)
                    CheckChildNodes(tn, nodeChecked);
            }
            
            node.Checked = nodeChecked;
            CheckScriptElementNode(node, nodeChecked);
        }

        private void CheckParentNodes(TreeNode node, bool nodeChecked)
        {
            if (node.Parent != null)
            {
                node.Parent.Checked = nodeChecked;
                CheckScriptElementNode(node.Parent, nodeChecked);
                CheckParentNodes(node.Parent, nodeChecked);
            }
        }

        private void CheckScriptElementNode(TreeNode node, bool nodeChecked)
        {
            if (node.Tag is Command)
            {
                Command command = node.Tag as Command;
                command.Execute = nodeChecked;
                return;
            }

            if (node.Tag is Script)
            {
                Script script = node.Tag as Script;
                script.Execute = nodeChecked;
                return;
            }

            if (node.Tag is TestFixture)
            {
                TestFixture fixture = node.Tag as TestFixture;
                fixture.Execute = nodeChecked;
                return;
            } 
        }

        private void preferencesMenuItem_Click(object sender, System.EventArgs e)
        {
            
            openFileDialog1.DefaultExt = "xml";
            openFileDialog1.Filter = "XML Files|*.xml|All Files|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Select command description file...";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Commands.CommandDescriptionContainerHandler.ReadXmlDescription(openFileDialog1.FileName);
                RegistryKey regKey = Constants.regMain;
                regKey = regKey.CreateSubKey(Constants.regMetKey + "\\" + Constants.regMMSTesterKey);
                regKey.SetValue(Constants.regDescriptionKey, openFileDialog1.FileName);
            }
        }
        
        /// <summary>
        /// Creates an object that describes the way the fixtures are viewed
        /// </summary>
        /// <param name="fixtureList"></param>
        /// <returns></returns>
        private object GenerateView(ArrayList fixtureList)
        {
            TreeNode tnc = new TreeNode("Test Fixtures");
            tnc.Tag = fixtureList;

            foreach (TestFixture tf in fixtureList)
            {
                TreeNode fixtureNode = new TreeNode(string.Format("{0} {1}", Constants.FIXTURE_STRING, (tf.Name == null ? "" : tf.Name)));
                fixtureNode.Tag = tf;
                foreach (Script script in tf.Scripts.Values)
                {
                    TreeNode scriptNode = new TreeNode(string.Format("{0} {1}", Constants.SCRIPT_STRING, script.Name));
                    scriptNode.Tag = script;
                    foreach (Command command in script.Commands)
                    {
                        TreeNode commandNode = new TreeNode(string.Format("{0} {1}", command.CommandType, command.Name)); 
                        commandNode.Tag = command;
                        scriptNode.Nodes.Add(commandNode);
                    }

                    fixtureNode.Nodes.Add(scriptNode);
                }
                
                tnc.Nodes.Add(fixtureNode);
                CheckChildNodes(tnc, true);
            }

            return tnc;
        }
        
        private void loadMenuItem_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.DefaultExt = "mmt";
            openFileDialog1.Filter = "Tester script|*.mmt|All Files|*.*";
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.Title = "Select test fixture file...";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try 
                {
                    string testFilename = openFileDialog1.FileName;

                    MMSScriptLexer lexer = new MMSScriptLexer(new StreamReader(new FileStream(testFilename, FileMode.Open, FileAccess.Read)));
                    MMSScriptParser parser = new MMSScriptParser(lexer);
                    TextBoxWriteLine(Constants.PARSER_EXEC_BEGIN, outputTextBox);
                    parser.fixture();
                    TextBoxWriteLine(Constants.PARSER_EXEC_END, outputTextBox);
                    fixtureList = parser.GetFixtures();
                    TextBoxWriteLine(Constants.SM_EXEC_BEGIN, outputTextBox);
                    
                    fixtureTreeView.Nodes.Clear();
                    fixtureTreeView.Nodes.Add((TreeNode)GenerateView(fixtureList));
                    fixtureTreeView.ExpandAll();

                    if (fixtureTreeView.Nodes.Count > 0)
                    {
                        startButton.Enabled = true;
                        stopButton.Enabled = false;  
                    }
                    
                    SessionManager sessionManager = new SessionManager("Metreos.MMSTestTool.Messaging.XmlMMSMessageFactory","Metreos.MMSTestTool.TransportLayer.MSMQTransport");
                } 
                    //exceptions need to be un-fudged, and try/catch blocks need to be more atomic 
                catch(TokenStreamException exception) 
                {
                    TextBoxWriteLine("TokenStreamException exception: " + exception.Message, outputTextBox);
                }
                catch(RecognitionException exception) 
                {
                    TextBoxWriteLine("RecognitionException exception: " + exception.Message, outputTextBox);
                }
                catch(antlr.ANTLRException exception)
                {
                    TextBoxWriteLine("antlr Exception: " + exception.Message, outputTextBox);
                }
                catch(Exception exception)
                {
                    TextBoxWriteLine(exception.Message, outputTextBox);
                }
            }
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            ArrayList sessionFixtures = new ArrayList();
            foreach (TestFixture tf in fixtureList)
            {
                if (tf.Execute)
                    sessionFixtures.Add(tf);
            }
            
            if (sessionFixtures.Count > 0)
            {
                SessionManager.AddSession("Session1", sessionFixtures, 1, 15000);
                //SessionManager.ServerConnect("10","5");
                SessionManager.StartExecution();
            }
        }
    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SBMIME;
using SBSMIMECore;
using SBSMIMESignatures;
using SBMIMEStream;
using SBMIMEClasses;
using SecureBlackbox.System;

namespace MimeViewer
{

	/// <summary>
	/// Summary description for MimeViewer_PlugControl.
	/// </summary>
	public class MimeViewer_PlugControl : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MimeViewer_PlugControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
				UnRegistedPartHandler();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            //
            // MimeViewer_PlugControl
            //
            this.Name = "MimeViewer_PlugControl";
            this.Size = new System.Drawing.Size(592, 364);
            this.Load += new System.EventHandler(this.MimeViewer_PlugControl_Load);

        }
		#endregion

        private void MimeViewer_PlugControl_Load(object sender, System.EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        protected TagInfo fTagInfo = TagInfo.tiNull;
        protected TElMessagePart fElMessagePart = null;
        protected TreeNodeInfo fNode = null;

        internal static Hashtable hashPlugControls = new Hashtable();

        internal void RegistedPartHandler()
        {
          Object obj = hashPlugControls[this];
          if ( obj == null )
            hashPlugControls.Add(this, this);
        }

        internal void UnRegistedPartHandler()
        {
          hashPlugControls.Remove(this);
        }

        protected string fCaption = "";
        public virtual string GetCaption()
        {
          return fCaption;
        }

        public virtual void UpdateView() {}

        public virtual void BeforeRemoveParent() {}

        public virtual bool IsSupportedMessagePart(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem)
        {
          return false;
        }

        protected virtual void Init(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem, bool bShow)
        {
          fTagInfo = tagInfo;
          fElMessagePart = messagePart;
          fNode = treeNodeItem;
        }
        public void InitSafe(TElMessagePart messagePart, TagInfo tagInfo, TreeNodeInfo treeNodeItem, bool bShow)
        {
          if ( (fNode != treeNodeItem) || (fTagInfo == TagInfo.tiOptions) ) {
            try {
              Init(messagePart, tagInfo, treeNodeItem, bShow);
            }
            catch //(System.Exception e)
            {
              // skip any error
            }
          }
        }//of: InitSafe


        public static String Version
        {
          get
          {
            return "2004.04.08";
          }
        }
	}//of: class MimeViewer_PlugControl


    public class ElMimeParserTask : Object, IDisposable
    {
     internal TreeNode fParent = null;
     internal string fFileName = "";
     internal TAnsiStringStream fDataStream = null;
     internal string fDefaultHeaderCharset = "";
     internal string fDefaultBodyCharset = "";
     internal bool fDefaultActivatePartHandlers = false;
     internal string fErrorMsg = "";
     internal ElMessageDemo fMsg = null;
     internal Exception fErrorException = null;
     internal IElProcessController fProcessController = null;
     internal bool fUseBackgroundParser = false;
     internal TreeNodeInfo fNode = null;
     internal DateTime fStartTime = DateTime.Now;

     private bool fFreeOnTerminate = false;

     public bool FreeOnTerminate{
       get
       {
         return fFreeOnTerminate;
       }
       set
       {
         fFreeOnTerminate = value;
       }
     }

     public ElMimeParserTask(TreeNode aParent, string aFileName,
       TAnsiStringStream aDataStream)
     {
       fParent = aParent;
       fFileName = aFileName;
       fDataStream = aDataStream;

       fDefaultHeaderCharset = ProjectOptions.fDefaultHeaderCharset;
       fDefaultBodyCharset = ProjectOptions.fDefaultBodyCharset;
       fDefaultActivatePartHandlers = ProjectOptions.fDefaultActivatePartHandlers;
       fUseBackgroundParser = ProjectOptions.fUseBackgroundParser;

       fFreeOnTerminate = false;

       if ( fUseBackgroundParser) {
         fProcessController = new TElSimpleProcessController();
         fProcessController.Init();
         Resume();
       }
       else
       {
         Cursor.Current = Cursors.WaitCursor;
         try
         {
           Execute();
         }
         finally
         {
           Cursor.Current = Cursors.Default;
         }
       }
     }//of: ElMimeParserTask()

     public delegate void ThreadSynchronizeMethod();

     protected void Synchronize(ThreadSynchronizeMethod syncMethod)
     {
       //todo: ???
       //syncMethod();
     }

     private static string[] headerAddressesFields =
     {
       "From",
       "To",
       "Reply-To",
       "Sender",
       "CC",
       "BCC",
       "Resent-From",
       "Resent-To",
       "Resent-Reply-To",
       "Resent-Sender",
       "Resent-CC",
       "Resent-BCC"
     };

     protected void Execute()
     {
       try
       {
         fMsg = new ElMessageDemo();

         if ( fUseBackgroundParser )
           Synchronize(new ThreadSynchronizeMethod(AddMessageToItems) ); // create node and add it to TreeView
         else
           AddMessageToItems();

         // initialize fields

         fMsg.fUseBackgroundParser = fUseBackgroundParser;
         fMsg.fParserThread = this;
         fMsg.ProcessController = fProcessController;
         fMsg.fDataStream.ProcessController = fProcessController;

         if (fDataStream == null) {
           fMsg.fDataFile = fFileName;
           fMsg.fDataStream.LoadFromFile(fMsg.fDataFile);
         }
         else
         {
           fMsg.fDataStream.Memory = fDataStream.Memory;
         }
         fStartTime = DateTime.Now;

         fMsg.fResult =
           fMsg.ParseMessage(
             fMsg.fDataStream,
             fDefaultHeaderCharset,
             fDefaultBodyCharset,
               SBMIME.Unit.mpoStoreStream |
               SBMIME.Unit.mpoLoadData |
               SBMIME.Unit.mpoCalcDataSize,
             false,
             false,
             fDefaultActivatePartHandlers
         );

         if ( (fMsg.fResult == SBMIME.Unit.EL_OK) ||
           (fMsg.fResult == SBMIME.Unit.EL_WARNING) )
         {
          // parse rfc header fields to e-mail AddressList collections:

           fMsg.InitMailAdressFields(headerAddressesFields);
         }

		 int fres = fMsg.fResult;

         if ( ! ( ( (fMsg.fResult == SBMIME.Unit.EL_OK) || (fMsg.fResult == SBMIME.Unit.EL_WARNING) ))) {
           fErrorMsg = System.String.Format(
             "Error parsing mime/smime message \"{0}\". ElMime error code: {1}",
             fFileName, fres.ToString()
           );
         }

         if ( fUseBackgroundParser )
           Synchronize(new ThreadSynchronizeMethod(UnlinkMessage));
         else
           UnlinkMessage();

       }
       catch (SBMIMEClasses.EAbort) {
         if ( fUseBackgroundParser )
           Synchronize(new ThreadSynchronizeMethod(UnlinkMessage));
         else
           UnlinkMessage();
       }
       catch (Exception e) {
         fErrorMsg = e.Message;
         fErrorException = e;
         if ( fUseBackgroundParser ) {
           Synchronize(new ThreadSynchronizeMethod(UnlinkMessage));
           Synchronize(new ThreadSynchronizeMethod(ShowError));
         }
         else
         {
           UnlinkMessage();
           ShowError();
         }
       }
     }

     protected void Resume()
     {
       // todo: ???
       throw new Exception("Background thread parser not implemented");
     }

     public void Terminate()
     {
       //fTerminated = true;
       // todo: ???
     }

     public void WaitFor()
     {
       // todo: ???
     }

     public void Dispose()
     {
       if ( fDataStream != null) {
         IDisposable iDis = fDataStream as IDisposable;
         if ( iDis != null )
           iDis.Dispose();
         fDataStream = null;
       }
     }//of: Dispose()

     private void ShowError()
     {
       throw fErrorException; //???
     }//of: ShowError()

     public static string ExtractFileName(string fileName)
     {
       string sFileName = ""+fileName;
       for ( int i = sFileName.Length-1; i>=0; i--) {
         if ( (sFileName[i] == "\\"[0]) || (sFileName[i] == "/"[0]) )
         {
           return sFileName.Remove(0, i);
         }
       };
       return sFileName;
     }//of: ExtractFileName

     private void AddMessageToItems()
     {
       TreeNodeInfo NullNode = null;
       string S = "";

       if (fNode == null) {
       // BEFORE PARSE
         //S = fStartTime.ToString("hh:nn:ss.zzz", null);
         S = fStartTime.ToString();
         if (fFileName.Length > 0)
           S += " | \"" + ExtractFileName(fFileName) + "\"";
         else
           S += " | \"//Attached Message//\"";
         if (fUseBackgroundParser)
           S = "...wait..." + S;
         fNode = new TreeNodeInfo(fParent.Nodes, TagInfo.tiParsedMessage, fMsg);
         fNode.Text = S;
         //fNode.ImageIndex := fNode.ImageIndex-1;
         //fNode.SelectedIndex := fNode.ImageIndex;
         fNode.fLocked = true; // do not allow remove fro TreeView
         fParent.Nodes.Add(fNode);
       }
       else
       {
       // AFTER PARSE
         //fNode.ImageIndex := fNode.ImageIndex+1;
         //fNode.SelectedIndex := fNode.ImageIndex;
         //S = fStartTime.ToString("hh:nn:ss.zzz", null);
         S = DateTime.Now.Subtract(fStartTime).ToString();
         if (fFileName.Length > 0)
           S = "[ " + S + " ] \"" + ExtractFileName(fFileName) + "\"";
         else
           S = "[ " + S + " ] \"//Attached Message//\"";
         fNode.Text = S;
         //if (fErrorMsg.Length == 0)
         {
           NullNode = new TreeNodeInfo(fParent.Nodes, TagInfo.tiNull, null);
           NullNode.Text = "...";
           fNode.Nodes.Add(NullNode);
         }
         if (fErrorMsg.Length > 0)
         {
           NullNode = new TreeNodeInfo(fParent.Nodes, TagInfo.tiError, null);
           NullNode.Text = fErrorMsg;
           fNode.Nodes.Add(NullNode);
         }

         fNode.fLocked = false; // allow remove from TreeView
       }
       if ( (fNode != null)/* && (fParent.Parent == null)*/ )
         fParent.Expand();

     }//of: AddMessageToItems()

     private void UnlinkMessage()
     {
       fFreeOnTerminate = true;
       if (fMsg != null) {
         if ( fDefaultActivatePartHandlers )
           Dispose();
         else
           fMsg.fDataStream.ProcessController = null;

         fMsg.fParserThread = null;
         fMsg.ProcessController = null;
       }
       if ( fNode != null )
         AddMessageToItems();
     }//of: UnlinkMessage()


    }//of: class ElMimeParserTask

    public class ElMessageDemo : TElMessage, IDisposable
    {
      internal string fDataFile;
      internal TAnsiStringStream fDataStream = null;
      internal int fResult = SBMIME.Unit.EL_OK;
      internal ElMimeParserTask fParserThread;
      internal bool fUseBackgroundParser = false;

      private static string xMailer = "EldoS ElMime Demos, ver: "+MimeViewer_PlugControl.Version +
        " ( " +  SBMIME.Unit.cXMailerDefaultFieldValue + " )";

      public ElMessageDemo(): base(xMailer)
      {
        fDataStream = new TAnsiStringStream();
      }

      ~ElMessageDemo()
      {
        if (fParserThread != null) {
          if ( fUseBackgroundParser ) {
            fParserThread.fProcessController.Status = TElProcessControllerStatus.pcsTerminate;
            fParserThread.Terminate();
            fParserThread.WaitFor();
          }
          fParserThread.Dispose();
        }
        fDataFile = null;
        if ( fDataStream != null ) {
          IDisposable iDis = fDataStream as IDisposable;
          if ( iDis != null )
            iDis.Dispose();
          fDataStream = null;
        }
      }

      public string DataFile
      {
        get
        {
          return fDataFile;
        }
        set
        {
          fDataFile = value;
        }
      }

      public int Result
      {
        get
        {
          return fResult;
        }
      }

      public bool UseBackgroundParser
      {
        get
        {
          return fUseBackgroundParser;
        }
      }

      public TAnsiStringStream DataStream
      {
        get
        {
          return fDataStream;
        }
      }

      public void InitMailAdressFields(string[] aFields){
        for (int i = 0; i< aFields.GetLength(0); i++)
        {
         if ( GetHeaderField(aFields[i]) != null )
          GetMailAddressList( aFields[i] );
       }
      }


    }//of: class ElMessageDemo

    public enum TagInfo
    {
        tiNull, // non calculated child subnode
        tiOptions, // customize interface node
        tiError, tiWarning, tiText, // information node
        // message node
        tiParsedMessage, tiAssembledMessage,
        // message child subnodes
        tiHeaders, tiField, // header and field
        tiComment, // todo: comment to tiField
        tiParamList, tiParam, // field params
        tiFromList, tiGroup, tiFrom,
        tiBody, // body
        tiPartList, tiPart, // multipart body
        tiPartHandler, tiPartBodyHandler // part and body handlers
    };

    public class TreeNodeInfo : TreeNode
    {
        internal bool fLocked = false;
        protected TagInfo fTagInfo = TagInfo.tiNull;
        protected Object fTagObj = null;
        protected MimeViewer_PlugControl fPlugFrame = null;
        protected int fLevel = 0;

        public TreeNodeInfo(TreeNodeCollection owner, TagInfo tagInfo, Object tagObject)
        {
            LinkTagObj(tagInfo, tagObject);
        }

        public int Level
        {
            get
            {
                return fLevel;
            }
            set
            {
                fLevel = value;
            }
        }

        public TagInfo TagInfo
        {
            get
            {
                return fTagInfo;
            }
        }

        public Object TagObj
        {
            get
            {
                return fTagObj;
            }
        }

        public MimeViewer_PlugControl PlugFrame
        {
            get
            {
                return fPlugFrame;
            }
            set
            {
                fPlugFrame = value;
            }
        }

        public bool Locked
        {
            get
            {
                return fLocked;
            }
        }

        public static TElMessagePart GetMessagePartFromTagObject( Object tagObject )
        {
            TElMessagePart mp = tagObject as TElMessagePart;
            if ( mp == null )
            {
                ElMessageDemo md = tagObject as ElMessageDemo;
                if ( md != null )
                    mp = md.MainPart;
            }
            return mp;
        }

        protected void InitPlugFrame()
        {
            fPlugFrame = null;
            if ( (fTagInfo == TagInfo.tiNull) & (fTagObj == null) )
                return;

            TElMessagePart messagePart = GetMessagePartFromTagObject(fTagObj);

            if ( (messagePart != null) || ( MimeViewer_PlugControl.hashPlugControls.Count > 0 ) )
            {
                ICollection iCol = MimeViewer_PlugControl.hashPlugControls.Keys;
                IEnumerator iEn = iCol.GetEnumerator();
                while ( iEn.MoveNext() ) {
                  //MimeViewer_PlugControl ctrl = (MimeViewer_PlugControl) MimeViewer_PlugControl.hashPlugControls[iEn.Current];
                  // or:
                  // key is "plug control"
                  //
                  MimeViewer_PlugControl ctrl = (MimeViewer_PlugControl) iEn.Current;

                  if ( ctrl != null )
                  {
                   if ( ctrl.IsSupportedMessagePart(messagePart, fTagInfo, this) )
                   {
                    fPlugFrame = ctrl;
                    // init plugin icons
                    // todo: ???
                    return;
                   }
                  }

                }//of: while ( iEn.MoveNext() ) {
            }//of: if ( (messagePart != null) || ( MimeViewer_PlugControl.hashPlugControls.Count > 0 ) )
        }//of: protected void InitPluginFrame

        public void LinkTagObj(TagInfo tagInfo, Object tagObject)
        {
            fTagInfo = TagInfo.tiNull;
            if ( fTagInfo != tagInfo )
            {
                fTagInfo = tagInfo;
                // set icons
                // todo: ???
            }
            fTagObj = tagObject;
            InitPlugFrame();
        }

        public void UpdatePlugFrame()
        {
            if ( (fPlugFrame == null) || (fTagInfo == TagInfo.tiNull) || (fTagObj == null) )
                return;
            TElMessagePart messagePart = GetMessagePartFromTagObject(fTagObj);

            fPlugFrame.InitSafe(messagePart, fTagInfo, this, true);
        }//of: public void UpdatePlugFrame

    }//of: public class TreeNodeInfo

    public class TreeNodeInfoOptions : TreeNodeInfo
    {
        protected TagInfo fOptions;
        public TreeNodeInfoOptions(TreeNodeCollection owner, TagInfo tagInfo, Object tagObject)
            : base (owner, tagInfo, tagObject)
        {
            fLocked = true;
        }
    }//of: TreeNodeInfoOptions

}//of: namespace MimeViewer

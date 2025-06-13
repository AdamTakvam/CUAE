using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using mshtml;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for MetreosWebBrowser.
	/// </summary>
	public class MetreosWebBrowser : System.Windows.Forms.Form
	{
        private AxSHDocVw.AxWebBrowser browser;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MetreosWebBrowser()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            string url = "about:blank";
            object o = System.Reflection.Missing.Value;
            browser.Navigate ( url,ref o,ref o,ref o,ref o);
            AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler handler = 
                new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler 
                (this.browser_StatusTextChange);
            browser.StatusTextChange += handler;			
			
		}

        private void browser_StatusTextChange
            (object sender, AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEvent e)
        {
			
            mshtml.HTMLDocument doc = (mshtml.HTMLDocument)this.browser.Document;
            doc.body.innerHTML = "<H1>foo</H1>";
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MetreosWebBrowser));
            this.browser = new AxSHDocVw.AxWebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.browser)).BeginInit();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Enabled = true;
            this.browser.Location = new System.Drawing.Point(0, 64);
            this.browser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browser.OcxState")));
            this.browser.Size = new System.Drawing.Size(272, 150);
            this.browser.TabIndex = 0;
            // 
            // MetreosWebBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.browser);
            this.Name = "MetreosWebBrowser";
            this.Text = "MetreosWebBrowser";
            ((System.ComponentModel.ISupportInitialize)(this.browser)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

	}
}

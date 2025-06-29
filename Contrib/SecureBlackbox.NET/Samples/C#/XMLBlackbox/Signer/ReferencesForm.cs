using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SBXMLSec;

namespace SimpleSigner
{
	/// <summary>
	/// Summary description for ReferencesForm.
	/// </summary>
	public class ReferencesForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lbReferences;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnInfo;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ReferenceForm frmReference;

		public ReferencesForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			frmReference = new ReferenceForm();
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
					frmReference.Dispose();
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
			this.lbReferences = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnInfo = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbReferences
			// 
			this.lbReferences.Location = new System.Drawing.Point(8, 8);
			this.lbReferences.Name = "lbReferences";
			this.lbReferences.Size = new System.Drawing.Size(188, 212);
			this.lbReferences.TabIndex = 0;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(208, 16);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(208, 48);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnInfo
			// 
			this.btnInfo.Location = new System.Drawing.Point(208, 80);
			this.btnInfo.Name = "btnInfo";
			this.btnInfo.TabIndex = 3;
			this.btnInfo.Text = "Info";
			this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(208, 200);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			// 
			// ReferencesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 230);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnInfo);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lbReferences);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ReferencesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "References";
			this.ResumeLayout(false);

		}
		#endregion

		private TElXMLReferenceList FReferences = null;

		public TElXMLReferenceList References
		{
			get 
			{
				return FReferences;
			}
			set 
			{
				FReferences = value;
				UpdateReferences();
			}
		}

		private bool FVerify = false;

		public bool Verify 
		{
			get 
			{
				return FVerify;
			}
			set 
			{
				FVerify = value;
				btnAdd.Enabled = !FVerify;
				btnDelete.Enabled = !FVerify;
			}
		}

		private void btnInfo_Click(object sender, System.EventArgs e)
		{
			if (lbReferences.SelectedIndex >= 0)
			{
				frmReference.Reference = References.get_Reference(lbReferences.SelectedIndex);
				frmReference.Verify = Verify;
				if (frmReference.ShowDialog() == DialogResult.OK)
				{
					TElXMLReference Ref = frmReference.Reference;
				}
			}

			UpdateReferences();
		}

		private void UpdateReferences()
		{
			string s;
			lbReferences.Items.Clear();
			for (int i = 0; i < FReferences.Count; i++)
			{
				s = FReferences.get_Reference(i).ID;
				if (s != "")
					s = s + " - ";

				s = s + FReferences.get_Reference(i).URI;
				if (s == "")
					s = "none";

				lbReferences.Items.Add(s);
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			TElXMLReference Ref;
			Ref = new TElXMLReference();
			frmReference.Reference = Ref;
			frmReference.Verify = Verify;
			if (frmReference.ShowDialog() == DialogResult.OK)
			{
				FReferences.Add(frmReference.Reference);
				UpdateReferences();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (lbReferences.SelectedIndex >= 0)
				FReferences.Delete(lbReferences.SelectedIndex);

			UpdateReferences();
		}
	}
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace GryphonDNCSimpleQuery
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TextBox number;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button validate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label result;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.number = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.validate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.result = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // number
            // 
            this.number.Location = new System.Drawing.Point(0, 32);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(88, 20);
            this.number.TabIndex = 0;
            this.number.Text = "5126892237";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 8);
            this.label1.Name = "label1";
            this.label1.TabIndex = 1;
            this.label1.Text = "Phone Number";
            // 
            // validate
            // 
            this.validate.Location = new System.Drawing.Point(0, 56);
            this.validate.Name = "validate";
            this.validate.TabIndex = 2;
            this.validate.Text = "Validate";
            this.validate.Click += new System.EventHandler(this.validate_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 104);
            this.label2.Name = "label2";
            this.label2.TabIndex = 3;
            this.label2.Text = "Result:";
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(8, 128);
            this.result.Name = "result";
            this.result.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.result);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.validate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.number);
            this.Name = "Form1";
            this.Text = "Do Not Call";
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

        private void validate_Click(object sender, System.EventArgs e)
        {
            SqlConnection connection = new SqlConnection("server=12.40.50.180;uid=voipdncuser;pwd=n0$bugs");
            try
            {
                connection.Open();
            }
            catch(Exception exp)
            {
                result.Text = exp.Message;
                return;
            }

            // Capture disposition code--don't bug me
            SqlCommand command = connection.CreateCommand();
            try
            {
                command.Parameters.Add("@license", "3133-307A-38E5-43B6-9FD5-2CDB-C0AA-0E59");
                command.Parameters.Add("@caller", "x2121");
                command.Parameters.Add("@refkey", String.Empty);
                command.Parameters.Add("@number", "5126892237");
                command.Parameters.Add("@curfew", 1);
                command.Parameters.Add("@override", 0);

//                command.Parameters.Add("@CLIENTKEY", 5);
//                command.Parameters.Add("@ANI", String.Empty);
//                command.Parameters.Add("@NUMBER", number.Text);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "voIP_CertifyPhoneNumber";
                //command.CommandText = "voIP_CertifyCalledNumber";
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                object dma = reader["DMA"];
                object state =reader["State"];
            }
            catch(Exception exp)
            {
                // LogPhoneCertificationEvent
                result.Text = exp.Message;
            }
            finally
            {
                command.Dispose();
            }
            connection.Close();
        }
	}
}

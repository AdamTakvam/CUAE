using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Web;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using Metreos.Types.CiscoIpPhone;

namespace CookieTester
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class Form1 : System.Windows.Forms.Form
  {
    private const string executeDir = "CGI/Execute";
    private System.Windows.Forms.TextBox phoneIp;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button startButton;

    private string username;
    private string password;
    private string currentRequestIp;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox passwordValue;
    private System.Windows.Forms.TextBox usernameValue;

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
      this.phoneIp = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.startButton = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.passwordValue = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.usernameValue = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // phoneIp
      // 
      this.phoneIp.Location = new System.Drawing.Point(104, 8);
      this.phoneIp.Name = "phoneIp";
      this.phoneIp.TabIndex = 0;
      this.phoneIp.Text = "";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(0, 8);
      this.label1.Name = "label1";
      this.label1.TabIndex = 1;
      this.label1.Text = "Phone IP";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(8, 96);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(272, 360);
      this.textBox1.TabIndex = 4;
      this.textBox1.Text = "";
      // 
      // startButton
      // 
      this.startButton.Location = new System.Drawing.Point(104, 464);
      this.startButton.Name = "startButton";
      this.startButton.TabIndex = 3;
      this.startButton.Text = "Start";
      this.startButton.Click += new System.EventHandler(this.startButton_Click);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(0, 56);
      this.label2.Name = "label2";
      this.label2.TabIndex = 7;
      this.label2.Text = "Password";
      // 
      // passwordValue
      // 
      this.passwordValue.Location = new System.Drawing.Point(104, 56);
      this.passwordValue.Name = "passwordValue";
      this.passwordValue.TabIndex = 2;
      this.passwordValue.Text = "";
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(0, 32);
      this.label3.Name = "label3";
      this.label3.TabIndex = 9;
      this.label3.Text = "Username";
      // 
      // usernameValue
      // 
      this.usernameValue.Location = new System.Drawing.Point(104, 32);
      this.usernameValue.Name = "usernameValue";
      this.usernameValue.TabIndex = 1;
      this.usernameValue.Text = "";
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(288, 493);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.usernameValue);
      this.Controls.Add(this.passwordValue);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.phoneIp);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.startButton);
      this.Controls.Add(this.label1);
      this.Name = "Form1";
      this.Text = "Form1";
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

    private void startButton_Click(object sender, System.EventArgs e)
    {
      if(phoneIp.Text == String.Empty || phoneIp.Text == "!!!")
      {
        phoneIp.Text = "!!!";
        return;
      }

      username = usernameValue.Text; 
      password = passwordValue.Text;

      currentRequestIp = phoneIp.Text;

      MakeRequest();
      
    }

    private string GetIp()
    {
      IPAddress myIpAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
      return myIpAddress.ToString();
    }

    private void MakeRequest()
    {
      CiscoIPPhoneExecuteType execute = new CiscoIPPhoneExecuteType();

      execute.ExecuteItem = new CiscoIPPhoneExecuteItemType[1];
      execute.ExecuteItem[0] = new CiscoIPPhoneExecuteItemType();
      execute.ExecuteItem[0].URL = "http://" + GetIp() + "/CookieTesterPage/SetCookie.aspx";
      //execute.ExecuteItem[0].URL = "http://" + "192.168.1.110:8002" + "/CookieTesterPage/SetCookie.aspx";
      //execute.ExecuteItem[0].Priority = (ushort) 2;
      // execute.ExecuteItem[0].PrioritySpecified = true;

      string url = "http://" + currentRequestIp + "/" + executeDir;

      HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
      Execute ex = new Execute();
      ex.menu = execute;
      
      string bufferString = ex.ToString();
      byte[] buffer = System.Text.Encoding.ASCII.GetBytes(bufferString);

      request.ContentType = "application/x-www-form-urlencoded";
      request.ProtocolVersion = HttpVersion.Version10;
      request.Method = "POST";
      request.ContentLength = buffer.Length;
      request.TransferEncoding = null;
      request.KeepAlive = false;
      request.Expect = "";
      request.Credentials = new NetworkCredential(username, password);
            
      Stream writeStream = request.GetRequestStream();
      writeStream.Write(buffer, 0, buffer.Length);
            
      writeStream.Flush();
      writeStream.Close();

      HttpWebResponse response = null;

      try
      {
        response = (HttpWebResponse) request.GetResponse();
      }
      catch(System.Net.WebException e)
      {
        response = e.Response as HttpWebResponse;

        if(response != null)
        {
          if(response.StatusCode == HttpStatusCode.Unauthorized)
          {
            // Incorrect credentials.  This exception will be caught
            // when communicating with Cisco 7940/60 IP phones.  Cisco
            // 7970 IP phones do not respond with a 401.
            textBox1.AppendText("Unauthorized" + System.Environment.NewLine);
          }
          else
          {
            textBox1.AppendText(
              "Internal: Exception received from phone. Status code is " + response.StatusCode + System.Environment.NewLine);
          }

          response.Close();
        }
        else
        {
          textBox1.AppendText("Internal: Response was null in WebException handler." + System.Environment.NewLine);
        }
      }
      catch(Exception e)
      {
        textBox1.AppendText("No response received from phone" + System.Environment.NewLine);
        textBox1.AppendText(e.Message + System.Environment.NewLine);

        if(response != null)
        {
          response.Close();
        }
      }

      if(response == null)
        return;

      if(response.ContentLength > 0)
      {
        Stream readStream = response.GetResponseStream();
        buffer = new Byte[1024];
        int length = readStream.Read(buffer, 0, buffer.Length);
        string responseBody = System.Text.Encoding.ASCII.GetString(buffer, 0, length);

        textBox1.AppendText(responseBody + System.Environment.NewLine);

        readStream.Close();
      }

    }
  }
}
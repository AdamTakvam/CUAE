<%@ Page language="c#" Codebehind="MainForm.aspx.cs" AutoEventWireup="false" Inherits="PDFDemo.MainForm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>EldoS PDFBlackbox ASP.NET demo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">.main {
	FONT-SIZE: 8pt; FONT-FAMILY: Verdana, Arial
}
TABLE.main {
	BORDER-RIGHT: #000000 1px solid; BORDER-TOP: #000000 1px solid; BORDER-LEFT: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid
}
H2 {
	FONT-SIZE: 12pt; FONT-FAMILY: Verdana, Arial
}
.prompt {
	font-family: Verdana, Arial;
	font-size: 8pt;
}
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<h2>EldoS PDFBlackbox ASP.NET demo</h2>
		<asp:label class="prompt" id="lblPrompt" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 40px"
			runat="server" Width="400px" Height="16px"></asp:label><br />
		<form class="main" id="Form1" method="post" runat="server" enctype="multipart/form-data">
			<table cellSpacing="0" cellPadding="0" width="400" border="0">
				<tr>
					<td class="main" width="100%"><i>1. Please specify the PDF document that you wish to 
							process and the desired operation. Please note, that you need a private key 
							corresponding to your certificate if you need to sign or decrypt a document.</i>
					</td>
				</tr>
			</table>
			<table class="main" cellSpacing="0" cellPadding="8" width="400" bgColor="#ffffcc" border="0">
				<tr>
					<td class="main" width="100%">File to process:<br>
						<input type="file" size="40" name="document">
					</td>
				</tr>
				<tr>
					<td class="main" width="100%">
						Desired operation:<br>
						<select name="cbOperation">
							<option selected value="1">Encrypt document (password-based encryption)</option>
							<option value="2">Encrypt document (certificate-based encryption)</option>
							<option value="3">Sign document</option>
							<option value="4">Certify document</option>
							<option value="5">Decrypt and verify document</option>
						</select><br>
					</td>
				</tr>
			</table>
			<br>
			<table border="0" cellspacing="0" cellpadding="0" width="400">
				<tr>
					<td width="100%" class="main">
						<i>2. Please specify either a certificate file or a document encryption password 
							depending on a chosen operation. </i>
					</td>
				</tr>
			</table>
			<table class="main" border="0" cellspacing="0" cellpadding="8" width="400" bgcolor="#ffffcc">
				<tr>
					<td class="main" width="100%">
						Certificate file:<br>
						<input type="file" name="cert" size="40"><br>
						Certificate password (if needed):<br>
						<input type="password" name="certpass" size="20"><br>
					</td>
				</tr>
				<tr height="1" bgcolor="#000000">
					<td width="100%"></td>
				</tr>
				<tr>
					<td class="main" width="100%">
						Document encryption password:<br>
						<input type="password" name="docpass" size="20"><br>
					</td>
				</tr>
			</table>
			<br>
			<table border="0" cellspacing="0" cellpadding="0" width="400">
				<tr>
					<td width="100%" class="main" align="right">
						<input type="submit" name="submit" value="Proceed">
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

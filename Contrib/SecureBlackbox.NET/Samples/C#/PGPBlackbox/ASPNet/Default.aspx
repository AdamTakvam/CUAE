<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="PGPDemo.MainForm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MainForm</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp; <INPUT id="MAX_FILE_SIZE" style="Z-INDEX: 113; LEFT: 408px; WIDTH: 120px; POSITION: absolute; TOP: 192px; HEIGHT: 24px"
				type="hidden" size="14" value="10000000" name="MAX_FILE_SIZE">
			<DIV style="DISPLAY: inline; Z-INDEX: 101; LEFT: 16px; WIDTH: 680px; POSITION: absolute; TOP: 8px; HEIGHT: 24px"
				ms_positioning="FlowLayout">
				<h2>EldoS PGPBlackbox ASP.NET demo</h2>
			</DIV>
			<DIV style="DISPLAY: inline; Z-INDEX: 102; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 64px; HEIGHT: 24px"
				ms_positioning="FlowLayout">File to process:</DIV>
			<INPUT id="file" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 88px; HEIGHT: 24px"
				type="file" size="42" name="file"><SELECT id="operation" style="Z-INDEX: 104; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 144px; HEIGHT: 24px"
				name="operation">
				<OPTION value="1" selected>Encrypt source file</OPTION>
				<OPTION value="2">Sign source file</OPTION>
				<OPTION value="3">Encrypt and sign source file</OPTION>
				<OPTION value="4">Process protected file</OPTION>
			</SELECT>
			<DIV style="DISPLAY: inline; Z-INDEX: 105; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 120px; HEIGHT: 24px"
				ms_positioning="FlowLayout">Desired operation:</DIV>
			<DIV style="DISPLAY: inline; Z-INDEX: 106; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 176px; HEIGHT: 24px"
				ms_positioning="FlowLayout">Public key to use (for encryption/verifying):</DIV>
			<DIV style="DISPLAY: inline; Z-INDEX: 107; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 232px; HEIGHT: 24px"
				ms_positioning="FlowLayout">Secret key to use (for signing/decryption):</DIV>
			<asp:dropdownlist id="cbPublicKey" style="Z-INDEX: 108; LEFT: 16px; POSITION: absolute; TOP: 200px"
				runat="server" Width="369px" Height="32px"></asp:dropdownlist><asp:dropdownlist id="cbSecretKey" style="Z-INDEX: 109; LEFT: 16px; POSITION: absolute; TOP: 256px"
				runat="server" Width="368px" Height="24px"></asp:dropdownlist>
			<DIV style="DISPLAY: inline; Z-INDEX: 110; LEFT: 16px; WIDTH: 368px; POSITION: absolute; TOP: 288px; HEIGHT: 24px"
				ms_positioning="FlowLayout">Password for secret key:</DIV>
			<INPUT id="password" style="Z-INDEX: 111; LEFT: 16px; WIDTH: 144px; POSITION: absolute; TOP: 312px; HEIGHT: 24px"
				type="password" size="18" name="password"><INPUT id="submit" style="Z-INDEX: 112; LEFT: 152px; WIDTH: 104px; POSITION: absolute; TOP: 352px; HEIGHT: 32px"
				type="submit" value="Execute" name="submit" runat="server">
			<asp:Label id="lblMessage" style="Z-INDEX: 114; LEFT: 16px; POSITION: absolute; TOP: 40px"
				runat="server" Width="696px" Height="24px"></asp:Label></form>
	</body>
</HTML>

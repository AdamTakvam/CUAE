' is12-change-version.vbs
' Written by Hung Nguyen (hungng@cisco.com) on 2007.01.25
'
' Changes the product version in an Installshield 12 project file
' before compiling the installer.  Also changes the Product Code and
' Package Code so that the installer can also do seamless Major 
' Upgrades.
' 
' Our concept of a build number does not translate into Installshield world
' so it gets stored as a custom property (ProductBuild).
' Release version translates to their ProductVersion.
'
' Usage: is12-change-version.vbs project_file.ism VERSION BUILD_NUMBER
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Option Explicit: Dim oIPWI, oArgs, oMSI, oDB, oView

Set oArgs = WScript.Arguments
If oArgs.Count = 3 Then
	Dim pRelease, pBuild, pProjectfile
    Dim pProductName 
    Dim strProperty
    Dim strLogName
    Dim strMsiCommand

	' Grab and parse arguments
	pProjectFile = oArgs(0)
	pRelease = oArgs(1)
    pBuild = oArgs(2)
    
	' Open and modify project file
	Set oIPWI = CreateObject("SAAuto12.ISWiProject")
    
	oIPWI.OpenProject pProjectfile
	oIPWI.ProductVersion = pRelease
	oIPWI.PackageCode = oIPWI.GenerateGUID()
    ' Ignore these actions for the web server package
    If Not oIPWI.ProductCode = "{14C94376-0DA0-4C02-9EE9-C0C8C14F1C18}" Then
        pProductName = oIPWI.ProductName
        oIPWI.ProductName = pProductName & " " & pRelease & "." & pBuild
        oIPWI.ProductCode = oIPWI.GenerateGUID()
    End If
    
    Call SetBuildNumber
    
    ' It's important to save the file as a binary instead of XML so that the WindowsInstaller object can handle it
    oIPWI.UseXMLProjectFormat = False
	oIPWI.SaveProject
	oIPWI.CloseProject
    
    ' Modify the release information in the project so that the installer can automatically keep logs of itself for each version
    strLogName = pRelease & "." & pBuild & "-" & LCase(Replace(pProductName," ","_")) & "-install.log"
    strMsiCommand = "/lvx* ""C:\" & strLogName & """"
    
    Set oMSI = CreateObject("WindowsInstaller.Installer")
    Set oDB = oMSI.OpenDatabase(pProjectFile, 1)
    Set oView = oDB.OpenView("UPDATE `ISRelease` SET `MsiCommandLine` = '" & strMsiCommand & "' WHERE `ISRelease` = 'SetupFile'")
    oView.Execute
    oView.Close
    oDB.Commit
    
End If

Sub SetBuildNumber

    'Set Build number if needed
    On Error Resume Next
    Set strProperty = oIPWI.ISWiProperties.Item("ProductBuild")
    If Err.number = 0 Then
        strProperty.Value = pBuild
    End If
    
End Sub
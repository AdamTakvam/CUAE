' changeversion.vbs
' Written by Hung Nguyen (hungng@cisco.com) on 2006.08.17
'
' Changes the product version in an Installshield 10.5 project file
' before compiling the installer.  Also changes the Product Code and
' Package Code so that the installer can also do seamless Major 
' Upgrades.
'
' Usage: changeversion.vbs project_file.ism RELEASE BUILD
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Option Explicit: Dim oIPWI, oArgs

Set oArgs = WScript.Arguments
If oArgs.Count = 3 Then
	Dim pRelease, pBuild, pProjectfile
    Dim strProperty

	' Grab and parse arguments
	pProjectFile = oArgs(0)
	pRelease = oArgs(1)
    pBuild = oArgs(2)
    
	' Open and modify project file
	Set oIPWI = CreateObject("SAAuto1050.ISWiProject")

	oIPWI.OpenProject pProjectfile
	oIPWI.ProductVersion = pRelease
	oIPWI.ProductCode = oIPWI.GenerateGUID()
	oIPWI.PackageCode = oIPWI.GenerateGUID()
    
    'Set Build number if needed
    On Error Resume Next
    Set strProperty = oIPWI.ISWiProperties.Item("ProductBuild")
    If Err.number = 0 Then
        strProperty.Value = pBuild
    End If

	oIPWI.SaveProject
	oIPWI.CloseProject
End If
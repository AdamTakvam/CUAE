'---------------------------------------------------------------------------------------------------
' This function creates a virtual web directory on the specified web site
' and with the specified path and also create an application root if required.
'
'mkvirtdir [--computer|-c COMPUTER1, COMPUTER2, COMPUTER3]
'         <--website|-w WEBSITE>
'         <--virtualdir|-v NAME1,PATH1,ApplicationRoot>
'         [--help|-?]
'
'COMPUTER						Computer on which users exists
'WEBSITE1,WEBSITE2				Virtual Web Sites on which directories will be created
'NAME1,PATH1,ApplicationRoot	Virtual Directories name and path to create.
'
'Example 1		mkvirtdir -c MyComputer -w "Default Web Site"
'                       -v "Virtual Dir1","c:\inetpub\wwwroot\dir1",""
'
'Example 2		mkvirtdir -c MyComputer -w "Default Web Site"
'                       -v "Virtual Dir1","c:\inetpub\wwwroot\dir1","MyApplication"
'---------------------------------------------------------------------------------------------------
' Modified by Chris Crowe on April 3, 2001 from Microsoft Supplied Script : MKWebDir.VBS
'---------------------------------------------------------------------------------------------------


' Force explicit declaration of all variables.
Option Explicit

'On Error Resume Next

Dim oArgs, ArgNum

Dim ArgComputer, ArgWebSites, ArgVirtualDirs, ArgDirNames(), ArgDirPaths(), ArgApplicationRoots()
Dim DirIndex, ArgComputers

Set oArgs = WScript.Arguments
ArgComputers = Array("LocalHost")


ArgNum = 0
While ArgNum < oArgs.Count
	
	If (ArgNum + 1) >= oArgs.Count Then
		Call DisplayUsage
	End If	

	Select Case LCase(oArgs(ArgNum))
		Case "--computer","-c":
			ArgNum = ArgNum + 1
			ArgComputers = Split(oArgs(ArgNum), ",", -1)
		Case "--website","-w":
			ArgNum = ArgNum + 1
			ArgWebSites = oArgs(ArgNum)
		Case "--virtualdir","-v":
			ArgNum = ArgNum + 1
			ArgVirtualDirs = Split(oArgs(ArgNum), ",", -1)
		Case "--help","-?"
			Call DisplayUsage
	End Select	

	ArgNum = ArgNum + 1
Wend


ArgNum = 0
DirIndex = 0

ReDim ArgDirNames((UBound(ArgVirtualDirs)+1) \ 3)
ReDim ArgDirPaths((UBound(ArgVirtualDirs)+1) \ 3)
ReDim ArgApplicationRoots((UBound(ArgVirtualDirs)+1) \ 3)


if isArray(ArgVirtualDirs) then
	While ArgNum <= UBound(ArgVirtualDirs)
		ArgDirNames(DirIndex) = ArgVirtualDirs(ArgNum)
		If (ArgNum + 2) > UBound(ArgVirtualDirs) Then
			WScript.Echo "Error understanding virtual directories"
			Call DisplayUsage
		End If	
		ArgNum = ArgNum + 1
		ArgDirPaths(DirIndex) = ArgVirtualDirs(ArgNum)
		ArgNum = ArgNum + 1
		ArgApplicationRoots(DirIndex) = ArgVirtualDirs(ArgNum)
		ArgNum = ArgNum + 1
		DirIndex = DirIndex + 1
	Wend
end if 

If (ArgWebSites = "") Or (IsArray(ArgDirNames) = False or IsArray(ArgDirPaths) = False or _
   IsArray(ArgApplicationRoots) = False) Then
	Call DisplayUsage
Else
	Dim compIndex
	for compIndex = 0 to UBound(ArgComputers)
		Call ASTCreateVirtualWebDir(ArgComputers(compIndex),ArgWebSites,ArgDirNames, _
			ArgDirPaths, ArgApplicationRoots)
	next
End If

'---------------------------------------------------------------------------------
Sub Display(Msg)
	WScript.Echo Now & ". Error Code: " & Hex(Err) & " - " & Msg
End Sub

Sub Trace(Msg)
	WScript.Echo Now & " : " & Msg	
End Sub

Sub DisplayUsage()
	WScript.Echo "Usage: mkvirtdir [--computer|-c COMPUTER1,COMPUTER2]"
	WScript.Echo "                <--website|-w WEBSITE1>"
	WScript.Echo "                <--virtualdir|-v NAME1,PATH1,ApplicationRoot>"
	WScript.Echo "                [--help|-?]"	

	WScript.Echo ""
	WScript.Echo "Note, WEBSITE is the Web Site on which the directory will be created."
	WScript.Echo "The name can be specified as one of the following, in the priority specified:"
	WScript.Echo " Server Number (i.e. 1, 2, 10, etc.)"
	WScript.Echo " Server Description (i.e ""My Server"")"
	WScript.Echo " Server Host name (i.e. ""www.domain.com"")"
	WScript.Echo " IP Address (i.e., 127.0.0.1)"
	WScript.Echo ""
	WScript.Echo " ApplicationRoot must be specified but can be blank as in """" if"
	WScript.Echo " specified then an application root is created with the specified name."
	WScript.Echo ""
	WScript.Echo "Example with an application root"
	WScript.Echo "mkvirtdir -c MyComputer -w ""Default Web Site"""
	WScript.Echo "           -v ""dir1"",""c:\inetpub\wwwroot\dir1"",""MyApp"""
	WScript.Echo ""
	WScript.Echo "Example with no application root, just a virtual directory"	
	WScript.Echo "mkvirtdir -c MyComputer -w ""Default Web Site"""
	WScript.Echo "           -v ""dir1"",""c:\inetpub\wwwroot\dir1"","""""
	WScript.Quit
End Sub
'---------------------------------------------------------------------------------


Sub ASTCreateVirtualWebDir(ComputerName,WebSiteName,DirNames,DirPaths, AppRoots)
	Dim Computer, webSite, WebSiteID, vRoot, vDir, DirNum
	On Error Resume Next
	
	set webSite = findWeb(ComputerName, WebSiteName)
	if IsObject(webSite) then
		set vRoot = webSite.GetObject("IIsWebVirtualDir", "Root")
		Trace "Accessing root for " & webSite.ADsPath
		If (Err <> 0) Then
			Display "Unable to access root for " & webSite.ADsPath
		Else
			DirNum = 0
			If (IsArray(DirNames) = True) And (IsArray(DirPaths) = True) And _
				(UBound(DirNames) = UBound(DirPaths)) Then
				While DirNum < UBound(DirNames)
					'Create the new virtual directory
					Set vDir = vRoot.Create("IIsWebVirtualDir",DirNames(DirNum))
					If (Err <> 0) Then
						Display "Unable to create " & vRoot.ADsPath & "/" & DirNames(DirNum) &"."
					Else
						'Set the new virtual directory path
						vDir.AccessRead = true
						vDir.Path = DirPaths(DirNum)
						If (Err <> 0) Then
							Display "Unable to bind path " & DirPaths(DirNum) & " to " & vRootName & "/" & _
								DirNames(DirNum) & ". Path may be invalid."
						Else
							'Save the changes
							vDir.SetInfo
							If (Err <> 0) Then
								Display "Unable to save configuration for " & vRootName & "/" & _
									DirNames(DirNum) &"."
							Else
								' Create the application
								vDir.AppCreate2(2) ' IIS5 only (0=Low, 1=High, 2=Medium)
								' vdir.AppCreate(true) ' IIS4 can only use (false=low, true=High) 
								If (Err <> 0) Then
									Display "Unable to save configuration for " & vRootName & "/" & _
										DirNames(DirNum) &"."
								Else
									' This is the name you see on the virtual Directory tab in the MMC
									vDir.AppFriendlyName = AppRoots(DirNum)
									' This is so that ASP scripts will run
									vDir.AccessScript = true
									' Save the info to the metabase
									VDir.Setinfo
									If (Err <> 0) Then
										Display "Unable to save configuration for " & vRootName & "/" & _
											DirNames(DirNum) &"."
									Else
										Trace "Web virtual directory " & vRootName & "/" & _
											DirNames(DirNum) & " created successfully."
									end if
								End If
							End If
						End If

					End If
					Err = 0
					DirNum = DirNum + 1
				Wend
			End If
		End If
	else
		Display "Unable to find "& WebSiteName &" on "& ComputerName
	End if
	Trace "Done."
End Sub

function getBinding(bindstr)

	Dim one, two, ia, ip, hn
	
	one=Instr(bindstr,":")
	two=Instr((one+1),bindstr,":")
	
	ia=Mid(bindstr,1,(one-1))
	ip=Mid(bindstr,(one+1),((two-one)-1))
	hn=Mid(bindstr,(two+1))
	
	getBinding=Array(ia,ip,hn)
end function

Function findWeb(computer, webname)
	On Error Resume Next

	Dim websvc, site
	dim webinfo
	Dim aBinding, binding

	set websvc = GetObject("IIS://"&computer&"/W3svc")
	if (Err <> 0) then
		exit function
	end if
	' First try to open the webname.
	set site = websvc.GetObject("IIsWebServer", webname)
	if (Err = 0) and (not isNull(site)) then
		if (site.class = "IIsWebServer") then
			' Here we found a site that is a web server.
			set findWeb = site
			exit function
		end if
	end if
	err.clear
	for each site in websvc
		if site.class = "IIsWebServer" then
			'
			' First, check to see if the ServerComment
			' matches
			'
			If site.ServerComment = webname Then
				set findWeb = site
				exit function
			End If
			aBinding=site.ServerBindings
			if (IsArray(aBinding)) then
				if aBinding(0) = "" then
					binding = Null
				else
					binding = getBinding(aBinding(0))
				end if
			else 
				if aBinding = "" then
					binding = Null
				else
					binding = getBinding(aBinding)
				end if
			end if
			if IsArray(binding) then
				if (binding(2) = webname) or (binding(0) = webname) then
					set findWeb = site
					exit function
				End If
			end if 
		end if
	next
End Function

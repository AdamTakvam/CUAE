Const VirtualDirectoryPath = "/"
Const VirtualDirectoryName = "mce-management"
Const Servername = "localhost"
Const WebSiteInstanceID = 1

function DeleteVirtualDirectory()
on error resume next

'Get Default Web Site Object
metabasePath = "IIS://" & ServerName & "/W3svc/" & WebSiteInstanceID & "/Root" & mid(VirtualDirectoryPath,2)
set IISOBJ = GetObject(MetabasePath)
if (Err <> 0) then
	response.write "Error opening metabase path: " & MetabasePath & vbcrlf & vbcrlf & "Error = " & _
		Err.Description & " - Error Code = 0x" & hex(Err.Number)
	exit function
end if

'Delete Virtual Directory
IISOBJ.Delete "IIsWebVirtualDir",VirtualDirectoryName
if (Err <> 0) then
	response.write "Error deleteing virtual directory " & VirtualDirectoryName & vbcrlf & _
		vbcrlf & "Metabase path: " & MetabasePath & vbcrlf & vbcrlf & "Error = " & _
		Err.Description & " - Error Code = 0x" & hex(Err.Number)
	exit function
end if

WScript.Echo "Virtual directory " & VirtualDirectoryname & " has been deleted from " & Metabasepath

set IISOBJ = Nothing
end function

Call DeleteVirtualDirectory()


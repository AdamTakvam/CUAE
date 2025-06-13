'
' restart_service.vbs
'
' Written by Hung Nguyen on 2006.06.26
' Largely cannibalized from: http://www.activexperts.com/activmonitor/windowsmanagement/adminscripts/services/
'
' This script is simply made to restart services by stopping it, pinging it every
' second, and then starting it up again.  Specficially, this is for the Media Server,
' which takes so long to shutdown even "net stop ..." will give up on it before it
' finishes.
'
' Usage: restart_service.vbs SERVICE_NAME
'

Option Explicit: Dim oArgs, objWMIService, colServiceList, objService

Const RUNNING = "Running"
Const STARTING = "Starting"
Const STOPPING = "Stopping"
Const STOPPED = "Stopped"

Set oArgs = WScript.Arguments
If oArgs.Count = 1 Then
    Dim pServiceName, strComputer, strServiceState

    pServiceName = oArgs(0)
    strComputer = "."
    Set objWMIService = GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")

    ' Retrieve service info
    Set colServiceList = objWMIService.ExecQuery("Select * from Win32_Service where Name='" & pServiceName & "'")
    For each objService in colServiceList
        strServiceState = objService.State
        If strServiceState = STARTING Then
            WScript.Quit
        Elseif strServiceState = RUNNING Then
            objService.StopService()
        End If        
    Next
    
    Do While Not strServiceState = STOPPED
        WScript.Sleep 1000
        Set colServiceList = objWMIService.ExecQuery("Select * from Win32_Service where Name='" & pServiceName & "'")
        For each objService in colServiceList
            strServiceState = objService.State
        Next
    Loop    
    
    If strServiceState = STOPPED Then
        For each objService in colServiceList
            objService.StartService()
            WScript.Quit
        Next        
    End If
    
End If
WScript.Quit -1

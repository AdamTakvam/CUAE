; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CTestAppDlg
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "TestApp.h"

ClassCount=3
Class1=CTestAppApp
Class2=CTestAppDlg
Class3=CAboutDlg

ResourceCount=3
Resource1=IDD_ABOUTBOX
Resource2=IDR_MAINFRAME
Resource3=IDD_TESTAPP_DIALOG

[CLS:CTestAppApp]
Type=0
HeaderFile=TestApp.h
ImplementationFile=TestApp.cpp
Filter=N

[CLS:CTestAppDlg]
Type=0
HeaderFile=TestAppDlg.h
ImplementationFile=TestAppDlg.cpp
Filter=D
BaseClass=CDialog
VirtualFilter=dWC
LastObject=CTestAppDlg

[CLS:CAboutDlg]
Type=0
HeaderFile=TestAppDlg.h
ImplementationFile=TestAppDlg.cpp
Filter=D

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=4
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308480
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889

[DLG:IDD_TESTAPP_DIALOG]
Type=1
Class=CTestAppDlg
ControlCount=2
Control1=ID_BUTTON_INSTALL,button,1342242817
Control2=ID_BUTTON_UNINSTALL,button,1342242816


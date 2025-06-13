Metreos Communications Environment
Software Development Kit
Version 1.1

Last Updated: September 13, 2004

Overview
--------
This SDK contains documentation and examples for the Metreos Communications
Environment (MCE).  Furthermore, an installer for the Metreos visual Designer
and the Metreos Framework are included for developers wishing to build 
applications on the MCE.


Installation
------------
Before you begin make sure you have installed the Microsft .NET Software 
Development Kit (MS.NET SDK).  The MS.NET SDK conains prerequesites for the
Metreos Visual Designer and the examples included in this SDK.  The MS.NET
SDK can be downloaded from here:

http://www.microsoft.com/downloads/details.aspx?FamilyId=9B3A2CA6-3647-4070-9F41-A333C6B9181D&displaylang=en

1. Copy the SDK directory to a home on your file system.
2. Run "Setup.exe" from the "frameworK" directory under the SDK.  This will
   install the Metreos Framework which contains prerequesites for using the
   Metreos Visual Designer.
3. Run "Setup.exe" from the "designer" directory to install the Metreos
   Visual Designer.  After installation you should have a shortcut on your
   dekstop that will launch the designer.
   

Directory Structure
-------------------
Each example uses the following directory layout:

  Directory Name               Description
  --------------               ------------------------------------------------
  contrib                      3rd party applications or libraries used by
                               the example.  For example, the LdapDirectory
                               example includes a copy of the OpenLDAP server
                               for convenience.
                               
  doc                          Documentation files.
  
  mca                          The Metreos Visual Designer project is stored
                               in this directory.
                               
  media                        Audio files used by the application.
                               
  plugins                      If the example utilizes any native actions or
                               custom types, they will be included under this
                               directory.
                               
  sql                          Database creation scripts or other SQL commands
                               and queries.
                               
  web                          If the application utilizes a web interface it
                               will be included in this directory.


Building the Examples
---------------------
Each example application included in the SDK contains a batch file, 'build.bat'
that can be used to build the entire example.  Please note, 'build.bat'
assumes that you installed the Metreos Visual Designer in the standard location
at "C:\Program Files\Metreos\MaxDesigner\".  If this is not the case you may
override that setting by setting the environment variable 'DesignerPath' to
the location of the 'MaxDesigner.exe' executable.

To open the example using the Metreos Visual Designer do the following:
1. Launch the designer.
2. Click File->Open Project
3. Navigate to the example's directory under the SDK
4. Go into the 'mca' directory
5. Select the appropriate file that ends with '.max' 

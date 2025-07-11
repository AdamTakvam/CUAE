// NAnt - A .NET build tool
// Copyright (C) 2001 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Gerry Shaw (gerry_shaw@yahoo.com)
// Scott Hernandez (ScottHernandez@hotmail.com)
// Gert Driesen (gert.driesen@ardatis.com)

using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Make assembly as NOT visible to COM
[assembly: ComVisible(false)]

// Mark assembly CLS compliant
[assembly: CLSCompliant(true)]

[assembly: AssemblyTitle("NAnt")]
[assembly: AssemblyDescription("A .NET Build Tool")]
[assembly: AssemblyConfiguration("net-1.0.win32; release")]
[assembly: AssemblyCompany("http://nant.sourceforge.net")]
[assembly: AssemblyProduct("NAnt")]
[assembly: AssemblyCopyright("Copyright (C) 2001-2003 Gerry Shaw")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// build number expressed in number of days since 1/1/2000
[assembly: AssemblyVersion("0.84.1455.0")]
[assembly: AssemblyInformationalVersion("0.84")]

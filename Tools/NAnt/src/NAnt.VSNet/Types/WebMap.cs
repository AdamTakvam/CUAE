// NAnt - A .NET build tool
// Copyright (C) 2001-2003 Gerry Shaw
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
// Tom Cabanski (tcabanski@OAI.cc)

using System;
using System.Collections.Specialized;
using System.IO;

using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.Core.Util;

namespace NAnt.VSNet.Types {
    /// <summary>
    /// Represents a single mapping from URL project path to physical project 
    /// path.
    /// </summary>
    public class WebMap : Element {
        #region Private Instance Fields

        private string _url;
        private string _path;
        private bool _ifDefined = true;
        private bool _unlessDefined = false;

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// Specifies the URL of the project file.
        /// </summary>
        /// <value>
        /// The URL of the project file.
        /// </value>
        [TaskAttribute("url", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Url {
            get { return _url; }
            set { _url = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Specifies the actual path to the project file.
        /// </summary>
        /// <value>
        /// The actual path to the project file.
        /// </value>
        [TaskAttribute("path", Required=true)]
        [StringValidator(AllowEmpty=false)]
        public string Path {
            get { return _path; } 
            set { _path = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// Indicates if the URL of the project file should be mapped.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the URL of the project file should be 
        /// mapped; otherwise, <see langword="false" />.
        /// </value>
        [TaskAttribute("if")]
        [BooleanValidator()]
        public bool IfDefined {
            get { return _ifDefined; }
            set { _ifDefined = value; }
        }

        /// <summary>
        /// Indicates if the URL of the project file should not be mapped.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the URL of the project file should not 
        /// be mapped; otherwise, <see langword="false" />.
        /// </value>
        [TaskAttribute("unless")]
        [BooleanValidator()]
        public bool UnlessDefined {
            get { return _unlessDefined; }
            set { _unlessDefined = value; }
        }

        #endregion Public Instance Properties
    }
}

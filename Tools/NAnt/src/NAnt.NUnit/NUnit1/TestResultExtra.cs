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
// Ian MacLean (ian_maclean@another.com)

using NUnit.Framework;

namespace NAnt.NUnit1.Types {
    /// <summary>
    /// Decorates NUnits <see cref="TestResult" /> with extra information such as 
    /// run-time.
    ///</summary>
    public class TestResultExtra : TestResult {
        #region Public Instance Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResultExtra" />
        /// class.
        /// </summary>
        public TestResultExtra() {
        }

        #endregion Public Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Gets or sets the total run-time of a test.
        /// </summary>
        /// <value>The total run-time of a test.</value>
        public long RunTime {
            get { return _runTime; }
            set { _runTime = value; }
        }

        #endregion Public Instance Properties

        #region Private Instance Fields
		
        long _runTime;
		 
        #endregion Private Instance Fields
    }
}

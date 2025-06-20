// NAnt - A .NET build tool
// Copyright (C) 2001-2002 Gerry Shaw
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

using System;
using System.Globalization;
using System.Reflection;
using System.Xml;

using NAnt.Core.Attributes;
using NAnt.Core.Util;

namespace NAnt.Core {
    /// <summary>
    /// Provides the abstract base class for types.
    /// </summary>
    [Serializable()]
    public abstract class DataTypeBase : Element {
        #region Private Instance Fields

        private string _id;
        private string _refID;

        #endregion Private Instance Fields

        #region Public Instance Properties

        /// <summary>
        /// The ID used to be referenced later.
        /// </summary>
        [TaskAttribute("id" )]
        public string ID {
            get { return _id; }
            set { _id = StringUtils.ConvertEmptyToNull(value); }
        }

        /// <summary>
        /// The ID to use as the reference.
        /// </summary>
        [TaskAttribute("refid")]
        public string RefID {
            get { return _refID; }
            set { _refID = StringUtils.ConvertEmptyToNull(value); }
        }

        #endregion Public Instance Properties

        #region Override implementation of Element
        
        /// <summary>
        /// Gets the name of the datatype.
        /// </summary>
        /// <value>The name of the datatype.</value>
        public override string Name {
            get {
                string name = null;
                ElementNameAttribute elementName = (ElementNameAttribute) Attribute.GetCustomAttribute(GetType(), typeof(ElementNameAttribute));
                if (elementName != null) {
                    name = elementName.Name;
                }
                return name;
            }
        }

        protected override void InitializeElement(XmlNode elementNode) {
            if (Parent == null) {
                // output warning message
                Log(Level.Warning, "Parent property should be set on types" 
                    + " deriving from DataTypeBase to determine whether" 
                    + " the type is declared on a valid level.");

                // skip further tests
                return;
            }

            if (Parent.GetType() == typeof(Project) || Parent.GetType() == typeof(Target)) {
                if (StringUtils.IsNullOrEmpty(ID)) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "'id' is a required attribute for a <{0}> datatype declaration.", 
                        Name), Location);
                }
                if (!StringUtils.IsNullOrEmpty(RefID)) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "'refid' attribute is invalid for a <{0}> datatype declaration.", 
                        Name), Location);
                }
            } else {
                  if (!StringUtils.IsNullOrEmpty(ID)) {
                    throw new BuildException(string.Format(CultureInfo.InvariantCulture, 
                        "'id' is an invalid attribute for a <{0}> tag. Datatypes" 
                        + " can only be declared at Project or Target level.", 
                        Name), Location);
                }
            }
        }

        #endregion Override implementation of Element

        #region Public Instance Methods

        /// <summary>
        /// Should be overridden by derived classes. clones the referenced types 
        /// data into the current instance.
        /// </summary>
        public virtual void Reset( ) {
        }

        #endregion Public Instance Methods
    }
}

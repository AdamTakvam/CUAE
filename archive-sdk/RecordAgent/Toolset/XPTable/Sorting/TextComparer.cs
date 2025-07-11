/*
 * Copyright � 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.Collections;
using System.Windows.Forms;

using XPTable.Models;


namespace XPTable.Sorting
{
	/// <summary>
	/// An IComparer for sorting Cells that contain strings
	/// </summary>
	public class TextComparer : ComparerBase
	{
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the TextComparer class with the specified 
		/// TableModel, Column index and SortOrder
		/// </summary>
		/// <param name="tableModel">The TableModel that contains the data to be sorted</param>
		/// <param name="column">The index of the Column to be sorted</param>
		/// <param name="sortOrder">Specifies how the Column is to be sorted</param>
		public TextComparer(TableModel tableModel, int column, SortOrder sortOrder) : base(tableModel, column, sortOrder)
		{
			
		}

		#endregion


		#region Methods
		
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less 
		/// than, equal to or greater than the other
		/// </summary>
		/// <param name="a">First object to compare</param>
		/// <param name="b">Second object to compare</param>
		/// <returns>-1 if a is less than b, 1 if a is greater than b, or 0 if a equals b</returns>
		public override int Compare(object a, object b)
		{
			Cell cell1 = (Cell) a;
			Cell cell2 = (Cell) b;
			
			// check for null cells
			if (cell1 == null && cell2 == null)
			{
				return 0;
			}
			else if (cell1 == null)
			{
				return -1;
			}
			else if (cell2 == null)
			{
				return 1;
			}

			// check for null data
			if (cell1.Text == null && cell2.Text == null)
			{
				return 0;
			}
			else if (cell1.Text == null)
			{
				return -1;
			}
			
			return cell1.Text.CompareTo(cell2.Text);
		}

		#endregion
	}
}

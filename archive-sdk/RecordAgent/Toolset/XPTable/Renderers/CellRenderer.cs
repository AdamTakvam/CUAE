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
using System.Drawing;
using System.Windows.Forms;

using XPTable.Editors;
using XPTable.Events;
using XPTable.Models;
using XPTable.Themes;


namespace XPTable.Renderers
{
	/// <summary>
	/// Base class for Renderers that draw Cells
	/// </summary>
	public abstract class CellRenderer : Renderer, ICellRenderer
	{
		#region Class Data
		
		/// <summary>
		/// A string that specifies how a Cells contents are formatted
		/// </summary>
		private string format;

		/// <summary>
		/// The Brush used to draw disabled text
		/// </summary>
		private SolidBrush grayTextBrush;

		/// <summary>
		/// The amount of padding for the cell being rendered
		/// </summary>
		private CellPadding padding;
		
		#endregion


		#region Constructor

		/// <summary>
		/// Initializes a new instance of the CellRenderer class with default settings
		/// </summary>
		protected CellRenderer() : base()
		{
			this.format = "";

			this.grayTextBrush = new SolidBrush(SystemColors.GrayText);
			this.padding = CellPadding.Empty;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Releases the unmanaged resources used by the Renderer and 
		/// optionally releases the managed resources
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();

			if (this.grayTextBrush != null)
			{
				this.grayTextBrush.Dispose();
				this.grayTextBrush = null;
			}
		}


		/// <summary>
		/// Gets the renderer specific data used by the Renderer from 
		/// the specified Cell
		/// </summary>
		/// <param name="cell">The Cell to get the renderer data for</param>
		/// <returns>The renderer data for the specified Cell</returns>
		protected object GetRendererData(Cell cell)
		{
			return cell.RendererData;
		}


		/// <summary>
		/// Sets the specified renderer specific data used by the Renderer for 
		/// the specified Cell
		/// </summary>
		/// <param name="cell">The Cell for which the data is to be stored</param>
		/// <param name="value">The renderer specific data to be stored</param>
		protected void SetRendererData(Cell cell, object value)
		{
			cell.RendererData = value;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Overrides Renderer.ClientRectangle
		/// </summary>
		public override Rectangle ClientRectangle
		{
			get
			{
				Rectangle client = new Rectangle(this.Bounds.Location, this.Bounds.Size);
				
				// take borders into account
				client.Width -= Renderer.BorderWidth;
				client.Height -= Renderer.BorderWidth;

				// take cell padding into account
				client.X += this.Padding.Left + 1;
				client.Y += this.Padding.Top;
				client.Width -= this.Padding.Left + this.Padding.Right + 1;
				client.Height -= this.Padding.Top + this.Padding.Bottom;

				return client;
			}
		}


		/// <summary>
		/// Gets or sets the string that specifies how a Cells contents are formatted
		/// </summary>
		protected string Format
		{
			get
			{
				return this.format;
			}

			set
			{
				this.format = value;
			}
		}


		/// <summary>
		/// Gets the Brush used to draw disabled text
		/// </summary>
		protected Brush GrayTextBrush
		{
			get
			{
				return this.grayTextBrush;
			}
		}


		/// <summary>
		/// Gets or sets the amount of padding around the Cell being rendered
		/// </summary>
		protected CellPadding Padding
		{
			get
			{
				return this.padding;
			}

			set
			{
				this.padding = value;
			}
		}

		#endregion


		#region Events

		#region Focus

		/// <summary>
		/// Raises the GotFocus event
		/// </summary>
		/// <param name="e">A CellFocusEventArgs that contains the event data</param>
		public virtual void OnGotFocus(CellFocusEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
			
			e.Table.Invalidate(e.CellRect);
		}


		/// <summary>
		/// Raises the LostFocus event
		/// </summary>
		/// <param name="e">A CellFocusEventArgs that contains the event data</param>
		public virtual void OnLostFocus(CellFocusEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
			
			e.Table.Invalidate(e.CellRect);
		}

		#endregion

		#region Keys

		/// <summary>
		/// Raises the KeyDown event
		/// </summary>
		/// <param name="e">A CellKeyEventArgs that contains the event data</param>
		public virtual void OnKeyDown(CellKeyEventArgs e)
		{

		}


		/// <summary>
		/// Raises the KeyUp event
		/// </summary>
		/// <param name="e">A CellKeyEventArgs that contains the event data</param>
		public virtual void OnKeyUp(CellKeyEventArgs e)
		{

		}

		#endregion

		#region Mouse

		#region MouseEnter

		/// <summary>
		/// Raises the MouseEnter event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnMouseEnter(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;

			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
			
			bool tooltipActive = e.Table.ToolTip.Active;

			if (tooltipActive)
			{
				e.Table.ToolTip.Active = false;
			}

			e.Table.ResetMouseEventArgs();

			e.Table.ToolTip.SetToolTip(e.Table, e.Cell.ToolTipText);

			if (tooltipActive)
			{
				e.Table.ToolTip.Active = true;
			}
		}

		#endregion

		#region MouseLeave

		/// <summary>
		/// Raises the MouseLeave event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnMouseLeave(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
		}

		#endregion

		#region MouseUp

		/// <summary>
		/// Raises the MouseUp event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnMouseUp(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
		}

		#endregion

		#region MouseDown

		/// <summary>
		/// Raises the MouseDown event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnMouseDown(CellMouseEventArgs e)
		{
			if (!e.Table.Focused)
			{
				if (!(e.Table.IsEditing && e.Table.EditingCell == e.CellPos && e.Table.EditingCellEditor is IEditorUsesRendererButtons))
				{
					e.Table.Focus();
				}
			}
			
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
		}

		#endregion

		#region MouseMove

		/// <summary>
		/// Raises the MouseMove event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnMouseMove(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}
		}

		#endregion

		#region Click

		/// <summary>
		/// Raises the Click event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnClick(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}

			if (e.Table.EditStartAction == EditStartAction.SingleClick && e.Table.IsCellEditable(e.CellPos))
			{
				e.Table.EditCell(e.CellPos);
			}
		}


		/// <summary>
		/// Raises the DoubleClick event
		/// </summary>
		/// <param name="e">A CellMouseEventArgs that contains the event data</param>
		public virtual void OnDoubleClick(CellMouseEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell == null)
			{
				this.Padding = CellPadding.Empty;
			}
			else
			{
				this.Padding = e.Cell.Padding;
			}

			if (e.Table.EditStartAction == EditStartAction.DoubleClick && e.Table.IsCellEditable(e.CellPos))
			{
				e.Table.EditCell(e.CellPos);
			}
		}

		#endregion

		#endregion
			
		#region Paint

		/// <summary>
		/// Raises the PaintCell event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		public virtual void OnPaintCell(PaintCellEventArgs e)
		{
			this.Bounds = e.CellRect;
			
			if (e.Cell != null)
			{
				this.Padding = e.Cell.Padding;

				this.Alignment = e.Table.ColumnModel.Columns[e.Column].Alignment;
				this.LineAlignment = e.Table.TableModel.Rows[e.Row].Alignment;

				this.Format = e.Table.ColumnModel.Columns[e.Column].Format;

				this.Font = e.Cell.Font;
			}
			else
			{
				this.Padding = CellPadding.Empty;

				this.Alignment = ColumnAlignment.Left;
				this.LineAlignment = RowAlignment.Center;

				this.Format = "";

				this.Font = null;
			}

			// if the font is null, use the default font
			if (this.Font == null)
			{
				this.Font = Control.DefaultFont;
			}

			// paint the Cells background
			this.OnPaintBackground(e);

			// paint the Cells foreground
			this.OnPaint(e);
		}


		/// <summary>
		/// Raises the PaintBackground event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		protected virtual void OnPaintBackground(PaintCellEventArgs e)
		{
			if (e.Selected && !e.Table.HideSelection)
			{
				if (e.Table.Focused)
				{
					this.ForeColor = e.Table.SelectionForeColor;
					this.BackColor = e.Table.SelectionBackColor;
				}
				else
				{
					this.BackColor = e.Table.UnfocusedSelectionBackColor;
					this.ForeColor = e.Table.UnfocusedSelectionForeColor;
				}

				if (this.BackColor.A != 0)
				{
					e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
				}
			}
			else
			{
				this.ForeColor = e.Cell != null ? e.Cell.ForeColor : Color.Black;

				if (!e.Sorted || (e.Sorted && e.Table.SortedColumnBackColor.A < 255))
				{
					if (e.Cell != null)
					{
						if (e.Cell.BackColor.A < 255)
						{
							if (e.Row % 2 == 1)
							{
								if (e.Table.AlternatingRowColor.A != 0)
								{
									this.BackColor = e.Table.AlternatingRowColor;
									e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
								}
							}
						
							this.BackColor = e.Cell.BackColor;
							if (e.Cell.BackColor.A != 0)
							{
								e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
							}
						}
						else
						{
							this.BackColor = e.Cell.BackColor;
							if (e.Cell.BackColor.A != 0)
							{
								e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
							}
						}
					}
					else
					{
						if (e.Row % 2 == 1)
						{
							if (e.Table.AlternatingRowColor.A != 0)
							{
								this.BackColor = e.Table.AlternatingRowColor;
								e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
							}
						}
					}
					
					if (e.Sorted)
					{
						this.BackColor = e.Table.SortedColumnBackColor;
						if (e.Table.SortedColumnBackColor.A != 0)
						{
							e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
						}
					}
				}
				else
				{
					this.BackColor = e.Table.SortedColumnBackColor;
					e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
				}
			}
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		protected virtual void OnPaint(PaintCellEventArgs e)
		{
			
		}


		/// <summary>
		/// Raises the PaintBorder event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		/// <param name="pen">The pen used to draw the border</param>
		protected virtual void OnPaintBorder(PaintCellEventArgs e, Pen pen)
		{
			// bottom
			e.Graphics.DrawLine(pen, e.CellRect.Left, e.CellRect.Bottom, e.CellRect.Right, e.CellRect.Bottom);
			
			// right
			e.Graphics.DrawLine(pen, e.CellRect.Right, e.CellRect.Top, e.CellRect.Right, e.CellRect.Bottom);
		}

		#endregion

		#endregion
	}
}

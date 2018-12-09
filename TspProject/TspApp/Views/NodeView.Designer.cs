/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 02.11.2018
 * Time: 1:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TspApp
{
	partial class NodeView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label Rib;
		private System.Windows.Forms.Label Limit;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent(string rib, string limit)
		{
			this.Rib = new System.Windows.Forms.Label { Text = rib };
			this.Limit = new System.Windows.Forms.Label { Text = limit };
			this.SuspendLayout();
			// 
			// Rib
			// 
			this.Rib.AutoSize = true;
			this.Rib.BackColor = System.Drawing.Color.Transparent;
			this.Rib.Font = new System.Drawing.Font("Franklin Gothic Book", 15F);
			this.Rib.Margin = new System.Windows.Forms.Padding(2, 2, 0, 3);
			this.Rib.Name = "Rib";
			this.Rib.Click += new System.EventHandler(this.ShowOrHideDescription);
			// 
			// Limit
			// 
			this.Limit.AutoSize = true;
			this.Limit.BackColor = System.Drawing.Color.Transparent;
			this.Limit.Font = new System.Drawing.Font("Franklin Gothic Book", 10F);
			this.Limit.Margin = new System.Windows.Forms.Padding(0);
			this.Limit.Name = "Limit";
			this.Limit.Click += new System.EventHandler(this.ShowOrHideDescription);
			// 
			// NodeView
			// 
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.Rib, 0, 0);
			this.Controls.Add(this.Limit, 1, 0);
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Click += new System.EventHandler(this.ShowOrHideDescription);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
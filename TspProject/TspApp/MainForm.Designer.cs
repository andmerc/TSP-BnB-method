/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 15.10.2018
 * Time: 11:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TspApp
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox DropDown;
		private System.Windows.Forms.Button RandomFilling;
		private System.Windows.Forms.Button ClearButton;
		private System.Windows.Forms.Button CalculateButton;
		private System.Windows.Forms.Panel SolutionView;
		private System.Windows.Forms.Label Answer;
		
		/// <summary>
		/// Disposes resources used by the form.
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
		private void InitializeComponent()
		{
			this.CalculateButton = new System.Windows.Forms.Button();
			this.Answer = new System.Windows.Forms.Label();
			this.SolutionView = new System.Windows.Forms.Panel();
			this.DropDown = new System.Windows.Forms.ComboBox();
			this.ClearButton = new System.Windows.Forms.Button();
			this.RandomFilling = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// CalculateButton
			// 
			this.CalculateButton.Location = new System.Drawing.Point(310, 15);
			this.CalculateButton.Name = "CalculateButton";
			this.CalculateButton.Size = new System.Drawing.Size(120, 30);
			this.CalculateButton.TabIndex = 3;
			this.CalculateButton.Text = "Рассчитать маршрут";
			this.CalculateButton.UseVisualStyleBackColor = true;
			this.CalculateButton.Click += new System.EventHandler(this.SolveMatrix);
			// 
			// Answer
			// 
			this.Answer.AutoSize = true;
			this.Answer.Font = new System.Drawing.Font("Monotype Corsiva", 15F);
			this.Answer.Location = new System.Drawing.Point(0, 0);
			this.Answer.Margin = new System.Windows.Forms.Padding(0);
			this.Answer.Name = "Answer";
			this.Answer.Size = new System.Drawing.Size(0, 24);
			this.Answer.TabIndex = 5;
			this.Answer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Answer.Visible = false;
			// 
			// SolutionView
			// 
			this.SolutionView.AutoScroll = true;
			this.SolutionView.BackColor = System.Drawing.Color.White;
			this.SolutionView.Location = new System.Drawing.Point(440, 15);
			this.SolutionView.Margin = new System.Windows.Forms.Padding(0);
			this.SolutionView.MinimumSize = new System.Drawing.Size(410, 200);
			this.SolutionView.Name = "SolutionView";
			this.SolutionView.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
			this.SolutionView.Size = new System.Drawing.Size(410, 200);
			this.SolutionView.TabIndex = 4;
			this.SolutionView.Visible = false;
			// 
			// DropDown
			// 
			this.DropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DropDown.Font = new System.Drawing.Font("Franklin Gothic Book", 13.5F);
			this.DropDown.FormattingEnabled = true;
			this.DropDown.Items.AddRange(new object[] {
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10"});
			this.DropDown.Location = new System.Drawing.Point(15, 15);
			this.DropDown.Name = "DropDown";
			this.DropDown.Size = new System.Drawing.Size(50, 31);
			this.DropDown.TabIndex = 0;
			this.DropDown.SelectedValueChanged += new System.EventHandler(this.DropDownSelectedValueChanged);
			// 
			// ClearButton
			// 
			this.ClearButton.Location = new System.Drawing.Point(195, 15);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(110, 30);
			this.ClearButton.TabIndex = 2;
			this.ClearButton.Text = "Очистить матрицу";
			this.ClearButton.UseVisualStyleBackColor = true;
			this.ClearButton.Click += new System.EventHandler(this.ClearMatrix);
			// 
			// RandomFilling
			// 
			this.RandomFilling.Location = new System.Drawing.Point(70, 15);
			this.RandomFilling.Name = "RandomFilling";
			this.RandomFilling.Size = new System.Drawing.Size(120, 30);
			this.RandomFilling.TabIndex = 1;
			this.RandomFilling.Text = "Заполнить случайно";
			this.RandomFilling.UseVisualStyleBackColor = true;
			this.RandomFilling.Click += new System.EventHandler(this.SetRandomValues);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(864, 261);
			this.Controls.Add(this.DropDown);
			this.Controls.Add(this.SolutionView);
			this.Controls.Add(this.RandomFilling);
			this.Controls.Add(this.ClearButton);
			this.Controls.Add(this.CalculateButton);
			this.Controls.Add(this.Answer);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MinimumSize = new System.Drawing.Size(850, 300);
			this.Name = "MainForm";
			this.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Программа для решения задачи коммивояжера";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.SizeChanged += new System.EventHandler(this.SetSolutionViewSize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
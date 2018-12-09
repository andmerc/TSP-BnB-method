using System;
using System.Drawing;
using System.Windows.Forms;

namespace TspApp
{
	/// <summary>
	/// Абстрактный класс матрицы затрат
	/// </summary>
	public abstract partial class CostMatrixView : UserControl
	{		
		/// <summary>
		/// Ячейки матрицы затрат
		/// </summary>
		protected Control[,] Cells { get; set; }
		
		/// <summary>
		/// Базовый конструктор матрицы затрат
		/// </summary>
		protected CostMatrixView(Point location)
		{
			InitializeComponent();
			Location = location;
		}
		
		#region Специальные методы для конкретных матриц затрат
		
		/// <summary>
		/// Метод получения <c>Label</c> с определённым форматированием
		/// </summary>
		/// <param name="location">Позиция создаваемого <c>Label</c></param>
		/// <param name="text">Текст создаваемого <c>Label</c></param>
		protected Label GetLabel(Point location, string text)
		{
			return new Label {
				BackColor = location.X != 0 && location.Y != 0 ? Color.White : Color.WhiteSmoke,
				BorderStyle = BorderStyle.FixedSingle,
				Font = new Font("Arial", Constants.ShortDistance),
				Location = location,
				Size = new Size(Constants.LongDistance, Constants.LongDistance),
				Text = text,
				TextAlign = ContentAlignment.MiddleCenter
			};
		}
		/// <summary>
		/// Метод получения <c>TextBox</c> с определённым форматированием
		/// </summary>
		/// <param name="location">Позиция создаваемого <c>TextBox</c></param>
		/// <param name="text">Текст создаваемого <c>TextBox</c></param>
		/// <param name="isEnabled">Определяет будет ли активен создаваемый <c>TextBox</c></param>
		protected TextBox GetTextBox(Point location, string text, bool isEnabled)
		{
			return new TextBox {
				Enabled = isEnabled,
				Font = new Font("Franklin Gothic Book", Constants.LongDistance / 2),
				Location = location,
				Text = text,
				TextAlign = HorizontalAlignment.Center,
				Width = Constants.LongDistance
			};
		}
	}
	
	#endregion
}
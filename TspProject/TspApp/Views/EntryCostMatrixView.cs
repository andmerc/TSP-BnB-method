using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TspApp
{
	/// <summary>
	/// Класс вводимой матрицы затрат
	/// </summary>
	public class EntryCostMatrixView : CostMatrixView
	{
		/// <summary>
		/// Конструктор вводимой матрицы затрат
		/// </summary>
		/// <param name="countOfPoints">Количество пунктов в маршруте</param>
		/// <param name = "location">Позиция матрицы затрат</param>
		public EntryCostMatrixView(int countOfPoints, Point location = default(Point))
			: base(location)
		{
			Cells = GetMatrixOfCells(countOfPoints);
			Size = PreferredSize;
		}

		/// <summary>
		/// Метод заполнения матрицы ячеек значениями
		/// </summary>
		/// <param name="matrix">Матрица значений</param>
		public void SetCellsValues(string[,] matrix)
		{
			for (int i = 1; i < Cells.GetLength(0); i++)
				for (int j = 1; j < Cells.GetLength(1); j++)
					if (i != j) Cells[i, j].Text = matrix[i - 1, j - 1];
		}
		/// <summary>
		/// Метод получения значений ячеек матрицы
		/// </summary>
		public double[,] GetCellsValues()
		{
			var matrix = new double[Cells.GetLength(0) - 1, Cells.GetLength(1) - 1];
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
					matrix[i, j] = double.Parse(Cells[i + 1, j + 1].Text);
			return matrix;
		}
		
		/// <summary>
		/// Метод получения матрицы ячеек для вводимой матрицы затрат
		/// </summary>
		/// <param name="countOfPoints">Количество пунктов в маршруте</param>
		/// <returns>Матрица для ввода значений</returns>
		Control[,] GetMatrixOfCells(int countOfPoints)
		{
			var rand = new Random();
			var matrix = new Control[countOfPoints + 1, countOfPoints + 1];
			Point location;
			for (int i = 0; i < matrix.GetLength(0); i++) {
				for (int j = 0; j < matrix.GetLength(1); j++) {
					location = new Point((Constants.LongDistance - 1) * j, (Constants.LongDistance - 1) * i);
					var element = i == 0 || j == 0 ?
						GetLabel(location, i == 0 && j == 0 ? "" : (i + j).ToString()) as Control :
						GetTextBox(location, i != j ? "" : double.PositiveInfinity.ToString(), i != j) as Control;
					if (i != 0 && j != 0 && i != j)
						element.TextChanged += CellTextChanged;
					Controls.Add(matrix[i, j] = element);
				}
			}
			return matrix;
		}
		/// <summary>
		/// Обработчик события изменения текста в <c>TextBox</c>
		/// </summary>
		void CellTextChanged(object sender, EventArgs e)
		{
			var cell = sender as TextBox;
			string text = cell.Text, inf = double.PositiveInfinity.ToString();
			var valuesForNotExistingRoute = new [] { "0", "_", "-" };
			double number;
			if (valuesForNotExistingRoute.Any(text.Equals)) {
				cell.Text = inf;
			} else if (double.TryParse(text.Replace(inf, ""), out number)) {
				cell.Text = number.ToString();
			} else if (!text.Equals(inf) && text.Length > 0) {
				cell.Text = text.Remove(text.Length - 1);
			}
			cell.SelectionStart = text.Length;
		}
	}
}
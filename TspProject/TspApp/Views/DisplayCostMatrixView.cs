using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TspApp
{
	/// <summary>
	/// Класс отображаемой матрицы затрат
	/// </summary>
	public class DisplayCostMatrixView : CostMatrixView
	{
		/// <summary>
		/// Конструктор отображаемой матрицы затрат
		/// </summary>
		/// <param name="matrixOfValues">Матрица значений</param>
		/// <param name = "useHLines">Список отображаемых строк</param>
		/// <param name = "useVLines">Список отображаемых столбцов</param>
		/// <param name = "location">Позиция матрицы затрат</param>
		public DisplayCostMatrixView(double[,] matrixOfValues, List<int> useHLines, List<int> useVLines,
		    Point location = default(Point)) : base(location)
		{
			Cells = GetMatrixOfCells(GetFormattedMatrix(matrixOfValues, useHLines, useVLines));
			Size = PreferredSize;
		}

		/// <summary>
		/// Метод получения отформатированной матрицы (для неполных матриц - удаление столбцов и строк с бесконечностями)
		/// </summary>
		/// <param name="matrixOfValues">Матрица значений</param>
		/// <param name = "useHLines">Список отображаемых строк</param>
		/// <param name = "useVLines">Список отображаемых столбцов</param>
		/// <returns>Приведённая матрица значений</returns>
		double[,] GetFormattedMatrix(double[,] matrixOfValues, List<int> useHLines, List<int> useVLines)
		{
			var matrix = new double[useHLines.Count + 1, useVLines.Count + 1];
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
					if (i == 0 && j != 0 || j == 0 && i != 0) {
						matrix[i, j] = j == 0 ? useHLines[i - 1] + 1 : useVLines[j - 1] + 1;
					} else if (i != 0 && j != 0) {
						matrix[i, j] = matrixOfValues[useHLines[i - 1], useVLines[j - 1]];
					}
			return matrix;
		}
		/// <summary>
		/// Метод получения матрицы ячеек для отображаемой матрицы затрат
		/// </summary>
		/// <param name="matrixOfValues">Матрица значений</param>
		/// <returns>Матрица отображения значений</returns>
		Label[,] GetMatrixOfCells(double[,] matrixOfValues)
		{
			var matrix = new Label[matrixOfValues.GetLength(0), matrixOfValues.GetLength(1)];
			var text = "";
			for (int i = 0; i < matrix.GetLength(0); i++) {
				for (int j = 0; j < matrix.GetLength(1); j++) {
					if (j != 0 || i != 0) {
						text = matrixOfValues[i, j].ToString();
					}
					Controls.Add(matrix[i, j] = GetLabel(new Point((Constants.LongDistance - 1) * j, (Constants.LongDistance - 1) * i), text));
				}
			}
			return matrix;
		}
	}
}
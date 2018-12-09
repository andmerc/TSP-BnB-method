using System;

namespace TspApp
{
	/// <summary>
	/// Указывает способ расположения минимумов для матрицы
	/// </summary>
	public enum MinimumsType
	{
		/// <summary>
		/// Расположение минимумов по строкам
		/// </summary>
		MinimumsByRows = 0,
		/// <summary>
		/// Расположение минимумов по столбцам
		/// </summary>
		MinimumsByColumns = 1
	}
}
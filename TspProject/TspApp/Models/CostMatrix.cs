using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TspApp
{
	/// <summary>
	/// Класс матрицы затрат
	/// </summary>
	public class CostMatrix
	{
		/// <summary>
		/// Значения матрицы затрат
		/// </summary>
		readonly double[,] _matrixOfValues;
		/// <summary>
		/// Описание действий над матрицей узла решения
		/// </summary>
		StepDescription _description;
		/// <summary>
		/// Координаты нулей и их штрафы
		/// </summary>
		Dictionary<Point, double> _surcharges;
		/// <summary>
		/// Минимумы по строкам
		/// </summary>
		double[] _minimumsByRows;
		/// <summary>
		/// Минимумы по столбцам
		/// </summary>
		double[] _minimumsByColumns;
		/// <summary>
		/// Минимальная граница маршрута
		/// </summary>
		double _minimumLimit;
		
		/// <summary>
		/// Описание шагов создания матрицы затрат узла решения
		/// </summary>
		public StepDescription Description {
			get {
				return _description;
			}
		}
		/// <summary>
		/// Ребро с максимальным штрафом за неиспользование
		/// </summary>
		public Point RibWithMaxSurcharge {
			get {
				return _surcharges.FirstOrDefault(_ => _.Value.Equals(_surcharges.Values.ToList().Max())).Key;
			}
		}
		/// <summary>
		/// Минимальная граница маршрута
		/// </summary>
		public double MinimumLimit {
			get {
				return _minimumLimit;
			}
		}
		/// <summary>
		/// Флаг валидности матрицы
		/// </summary>
		/// <returns><c>true</c> если матрица имеет штрафы для нулей, иначе <c>false</c></returns>
		public bool IsValid {
			get {
				return _surcharges.Count > 0;
			}
		}
		
		/// <summary>
		/// Конструктор матрицы затрат с начальной минимальной границей
		/// </summary>
		/// <param name="matrix">Матрица значений</param>
		/// <param name="limit">Начальная минимальная граница</param>
		public CostMatrix(double[,] matrix, double limit)
		{
			_matrixOfValues = matrix;
			_minimumLimit = limit;
			_description = new StepDescription(_matrixOfValues);
			CastMatrix();
		}
		/// <summary>
		/// Конструктор матрицы затрат без начальной минимальной границы
		/// </summary>
		/// <param name="matrix">Матрица значений</param>
		public CostMatrix(double[,] matrix)
			: this(matrix, 0)
		{
		}

		#region Приватные методы
		
		/// <summary>
		/// Приведение матрицы затрат, вычисление минимальной границы и штрафов для нулей
		/// </summary>
		void CastMatrix()
		{
			_description.AddInfoBeforeMinimums(MinimumsType.MinimumsByRows);
			// Вычисление минимумов по строкам и вычитание их из исходной матрицы затрат
			_minimumsByRows = new double[_matrixOfValues.GetLength(0)];
			for (int i = 0; i < _matrixOfValues.GetLength(0); i++) {
				_minimumsByRows[i] = double.PositiveInfinity;
				for (int j = 0; j < _matrixOfValues.GetLength(1); j++)
					if (_matrixOfValues[i, j] < _minimumsByRows[i])
						_minimumsByRows[i] = _matrixOfValues[i, j];
				if (!double.IsPositiveInfinity(_minimumsByRows[i]) && !_minimumsByRows[i].Equals(0)) {
					for (int j = 0; j < _matrixOfValues.GetLength(1); j++)
						_matrixOfValues[i, j] -= _minimumsByRows[i];
				} else {
					_minimumsByRows[i] = 0;
				}
			}
			_description.AddMinimumsAndNewMatrix(_minimumsByRows, MinimumsType.MinimumsByRows);
			// Нахождение первичной границы для матрицы затрат
			_minimumLimit += _minimumsByRows.Sum();
			_description.AddLimit("Первичная", _minimumLimit);
			_description.AddInfoBeforeMinimums(MinimumsType.MinimumsByColumns);
			// Вычисление минимумов по столбцам и вычитание их из исходной матрицы затрат
			_minimumsByColumns = new double[_matrixOfValues.GetLength(1)];
			for (int i = 0; i < _matrixOfValues.GetLength(1); i++) {
				_minimumsByColumns[i] = double.PositiveInfinity;
				for (int j = 0; j < _matrixOfValues.GetLength(0); j++)
					if (_matrixOfValues[j, i] < _minimumsByColumns[i])
						_minimumsByColumns[i] = _matrixOfValues[j, i];
				if (!double.IsPositiveInfinity(_minimumsByColumns[i]) && !_minimumsByColumns[i].Equals(0)) {
					for (int j = 0; j < _matrixOfValues.GetLength(0); j++)
						_matrixOfValues[j, i] -= _minimumsByColumns[i];
				} else {
					_minimumsByColumns[i] = 0;
				}
			}
			_description.AddMinimumsAndNewMatrix(_minimumsByColumns, MinimumsType.MinimumsByColumns);
			// Нахождение итоговой границы для матрицы затрат
			_minimumLimit += _minimumsByColumns.Sum();
			// Нахождение нулей и вычисление штрафов для них
			_surcharges = new Dictionary<Point, double>();
			for (int i = 0; i < _matrixOfValues.GetLength(0); i++)
				for (int j = 0; j < _matrixOfValues.GetLength(1); j++)
					if (_matrixOfValues[i, j].Equals(0))
						_surcharges.Add(new Point(i, j), GetSurcharge(i, j));
			_description.AddLimit("Итоговая", _minimumLimit);
			_description.AddSurcharges(IsValid, _surcharges);
		}
		/// <summary>
		/// Получение штрафа за неиспользование ребра
		/// </summary>
		/// <param name="i">Начальная вершина ребра</param>
		/// <param name="j">Конечая вершина ребра</param>
		/// <returns>Штраф за неиспользование ребра <c>[i,j]</c></returns>
		double GetSurcharge(int i, int j)
		{
			double minimumByRow = double.PositiveInfinity, minimumByColumn = double.PositiveInfinity;
			// Нахождение минимума в строке
			for (int k = 0; k < _matrixOfValues.GetLength(1); k++)
				if (k != j && _matrixOfValues[i, k] < minimumByRow)
					minimumByRow = _matrixOfValues[i, k];
			// Нахождение минимума в столбце
			for (int k = 0; k < _matrixOfValues.GetLength(0); k++)
				if (k != i && _matrixOfValues[k, j] < minimumByColumn)
					minimumByColumn = _matrixOfValues[k, j];
			// Если минимум не изменился, то штрафа нет. Обнуляем
			if (double.IsPositiveInfinity(minimumByRow))
				minimumByRow = 0;
			if (double.IsPositiveInfinity(minimumByColumn))
				minimumByColumn = 0;
			return minimumByRow + minimumByColumn;
		}
		/// <summary>
		/// Получение матрицы без лишних ребёр (лишние рёбра - от конечной вершины в пути до каждой из остальных)
		/// </summary>
		/// <param name="routeByRibs">Неупорядоченный список ребёр решения</param>
		/// <param name="matrix">Матрица из которой удаляются ненужные рёбра</param>
		/// <returns>Матрица без лишних рёбер</returns>
		void RemoveConfusions(double[,] matrix, List<Point> routeByRibs)
		{
			// Если есть только одно ребро или не хватает одного ребра для решения, то лишних рёбер нет
			if (routeByRibs.Count > 1 && routeByRibs.Count + 1 < matrix.GetLength(0)) {
				// Получаем все возможные вершины
				var vertexes = routeByRibs.SelectMany(_ => new List<int> { _.X, _.Y }).ToList();
				// Ищем начальные вершины для отрезков. Отрезок - как минимум три последовательных вершины маршрута 
				var routes = vertexes
					.Where(_ => vertexes.Count(v => v == _) == 1 && vertexes.IndexOf(_) % 2 == 0 &&
			             vertexes.Count(v => v == vertexes.ElementAt(vertexes.IndexOf(_) + 1)) == 2)
					.Select(_ => new List<int> { _ })
					.ToList();
				// Если начальные вершины были найдены, то для каждой строим маршрут
				if (routes.Count > 0) {
					Point rib;
					routes.ForEach(list => {
						for (int i = 0; i < routeByRibs.Count; i++) {
							rib = routeByRibs.FirstOrDefault(_ => _.X == list.Last());
							if (!rib.IsEmpty)
								list.Add(rib.Y);
						}
						// После построения маршрута отсекаем все рёбра от последней для каждой вершины
						var lastPoint = list.Last();
						list.ForEach(_ => matrix[lastPoint, _] = double.PositiveInfinity);
					});
				}
			}
		}
		
		#endregion
		
		/// <summary>
		/// Получение матрицы без заданного ребра
		/// </summary>
		/// <returns>Матрица с исключением ребра с максимальным штрафом за неиспользование</returns>
		public CostMatrix ExcludeRibWithoutConfusions(List<Point> routeByRibs)
		{
			var rib = RibWithMaxSurcharge;
			var matrix = _matrixOfValues.Clone() as double[,];
			matrix[rib.X, rib.Y] = double.PositiveInfinity;
			RemoveConfusions(matrix, routeByRibs);
			return new CostMatrix(matrix, _minimumLimit);
		}
		/// <summary>
		/// Получение матрицы без заданного ребра
		/// </summary>
		/// <returns>Матрица с включением ребра с максимальным штрафом за неиспользование</returns>
		public CostMatrix IncludeRibWithoutConfusions(List<Point> routeByRibs)
		{
			var rib = RibWithMaxSurcharge;
			var matrix = _matrixOfValues.Clone() as double[,];
			for (int i = 0; i < matrix.GetLength(1); i++)
				matrix[rib.X, i] = double.PositiveInfinity;
			for (int i = 0; i < matrix.GetLength(0); i++)
				matrix[i, rib.Y] = double.PositiveInfinity;
			matrix[rib.Y, rib.X] = double.PositiveInfinity;
			RemoveConfusions(matrix, routeByRibs);
			return new CostMatrix(matrix, _minimumLimit);
		}
	}
}
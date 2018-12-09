using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TspApp
{
	/// <summary>
	/// Класс для отображения подробного описания узла дерева решения
	/// </summary>
	public partial class StepDescription : Panel
	{
		/// <summary>
		/// Матрица значений описываемого узла решения
		/// </summary>
		readonly double[,] _matrixOfValues;
		/// <summary>
		/// Список задач для отрисовки описания
		/// </summary>
		List<Task> _renderTasks;
		/// <summary>
		/// Информация по отображению строк
		/// </summary>
		List<int> _useHorizontalLines;
		/// <summary>
		/// Информация по отображению столбцов
		/// </summary>
		List<int> _useVerticalLines;
		/// <summary>
		/// Определяет отрисованность описания
		/// </summary>
		bool _isRepresented;
		
		/// <summary>
		/// Узел решения, к которому прикреплено описание
		/// </summary>
		public NodeView Owner { get; set; }
		
		/// <summary>
		/// Конструктор описания узла решения
		/// </summary>
		/// <param name="matrixOfValues">Матрица значений</param>
		public StepDescription(double[,] matrixOfValues)
		{
			InitializeComponent();
			_matrixOfValues = matrixOfValues;
			InitializeDescription();
		}
		
		#region Приватные методы
		
		/// <summary>
		/// Метод инициализации основных параметров описания узла решения
		/// </summary>
		void InitializeDescription()
		{
			SetUsableLines();
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				var isStartingMatrix = true;
				for (int i = 0; isStartingMatrix && i < _matrixOfValues.GetLength(0); i++)
					for (int j = 0; isStartingMatrix && j < _matrixOfValues.GetLength(1); j++)
						isStartingMatrix &= i == j || !double.IsPositiveInfinity(_matrixOfValues[i, j]);
				if (isStartingMatrix)
					AddDescriptionText(DescriptionMessages.StartOfSolving);
				AddDescriptionText(DescriptionMessages.StartOfProcessing);
				AddMatrix(_matrixOfValues);
          	}
			_renderTasks = new List<Task>();
		}
		/// <summary>
		/// Метод для нахождения используемых строк и столбцов
		/// </summary>
		void SetUsableLines()
		{
			_useHorizontalLines = new List<int>();
			_useVerticalLines = new List<int>();
			var use = false;
			for (int d = 0; d < _matrixOfValues.Rank; d++)
				for (int i = 0; i < _matrixOfValues.GetLength(d); i++) {
					use = false;
					for (int j = 0; !use && j < _matrixOfValues.GetLength(Math.Abs(d - 1)); j++)
						use |= !double.IsPositiveInfinity(d == 0 ? _matrixOfValues[i, j] : _matrixOfValues[j, i]);
					if (use)
						(d == 0 ? _useHorizontalLines : _useVerticalLines).Add(i);
				}
		}
		/// <summary>
		/// Метод для поздней отрисовки необходимых элементов
		/// </summary>
		void Represent() {
			_renderTasks.ForEach(_ => _.RunSynchronously());
			_isRepresented = true;
		}
		
		#region Вспомогательные методы для добавления описания к узлу решения
		
		/// <summary>
		/// Метод добавления текстового описания
		/// </summary>
		/// <param name="text"></param>
		void AddDescriptionText(string text)
		{
			var label = new Label {
				AutoSize = true,
				BackColor = Color.Transparent,
				Font = new Font("Monotype Corsiva", 14),
				Location = GetLocation(),
				Margin = new Padding(0, 0, 30, 0),
				Text = text
			};
			Controls.Add(label);
		}
		/// <summary>
		/// Метод добавления текущей матрицы
		/// </summary>
		void AddMatrix(double[,] matrix)
		{
			Controls.Add(new DisplayCostMatrixView(matrix, _useHorizontalLines, _useVerticalLines, GetLocation()));
		}
		/// <summary>
		/// Метод добавления мимнимумов по строкам
		/// </summary>
		/// <param name="minimums">Минимумы по строкам</param>
		void AddMinimumsByRows(double[] minimums)
		{
			var labels = GetMinimumsForMatrix(minimums, MinimumsType.MinimumsByRows, GetLocation(true));
			foreach (var label in labels)
				Controls.Add(label);
		}
		/// <summary>
		/// Метод добавления мимнимумов по столбцам
		/// </summary>
		/// <param name="minimums">Минимумы по столбцам</param>
		void AddMinimumsByColumns(double[] minimums)
		{
			var labels = GetMinimumsForMatrix(minimums, MinimumsType.MinimumsByColumns, GetLocation());
			foreach (var label in labels)
				Controls.Add(label);
		}
		/// <summary>
		/// Метод добавления таблицы с штрафами за неиспользование рёбер
		/// </summary>
		/// <param name="surcharges">Рёбра и их штрафы</param>
		void AddSurcharges(Dictionary<Point, double> surcharges)
		{
			var table = new TableLayoutPanel {
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
				ColumnCount = 2,
				Location = GetLocation(),
				RowCount = surcharges.Count + 1
			};
			Label label;
			for (int i = 0; i < table.ColumnCount; i++) {
				for (int j = 0; j < table.RowCount; j++) {
					label = new Label {
						AutoSize = true,
						BackColor = Color.Transparent,
						Font = new Font("Arial", Constants.ShortDistance),
						Margin = new Padding(0),
						Text = i == 0 ? "Ребро" : "Штраф"
					};
					if (j > 0) {
						if (i == 0) {
							var rib = surcharges.Keys.ElementAt(j - 1);
							label.Text = string.Format("({0}:{1})", rib.X + 1, rib.Y + 1);
						} else {
							label.Text = surcharges.Values.ElementAt(j - 1).ToString();
						}
					}
					table.Controls.Add(label, i, j);
				}
			}
			Controls.Add(table);
		}
		
		#region Вспомогательные методы для методов добавления описания
		
		/// <summary>
		/// Метод получения координат для элемента
		/// </summary>
		/// <param name="forMinimums">Если <c>true</c>, то возвращает координаты для минимумов (по строкам)</param>
		/// <returns>Координаты элемента</returns>
		Point GetLocation(bool forMinimums = false)
		{
			if (Controls.Count == 0)
				return new Point(Constants.ShortDistance, Constants.ShortDistance);
			Control firstControl = Controls[0], lastControl = Controls[Controls.Count - 1];
			return forMinimums ?
				new Point(lastControl.Location.X + lastControl.Width + Constants.ShortDistance, lastControl.Location.Y) : 
				new Point(firstControl.Location.X, lastControl.Location.Y + lastControl.Height + Constants.ShortDistance);
		}
		/// <summary>
		/// Метод получения минимумов для текущей таблицы
		/// </summary>
		/// <param name="minimums">Массив минимумов</param>
		/// <param name="type">Тип минимумов (по столбцам или по строкам)</param>
		/// <param name="location">Положение минимумов относительно матрицы</param>
		/// <returns>Группа лейблов с минимумами для матрицы</returns>
		Label[] GetMinimumsForMatrix(double[] minimums, MinimumsType type, Point location = default(Point))
		{
			var useLines = type == MinimumsType.MinimumsByRows ? _useHorizontalLines : _useVerticalLines;
			var labels = new Label[useLines.Count + 1];
			for (int i = 0; i < labels.Length; i++) {
				labels[i] = new Label {
					BackColor = i > 0 ? Color.White : Color.WhiteSmoke,
					BorderStyle = BorderStyle.FixedSingle,
					Font = new Font("Arial", Constants.ShortDistance),
					Location = location,
					Size = new Size(Constants.LongDistance, Constants.LongDistance),
					Text = i > 0 ? minimums[useLines[i - 1]].ToString() : "m",
					TextAlign = ContentAlignment.MiddleCenter
				};
				location.Offset((int)type * (Constants.LongDistance - 1), Math.Abs((int)type - 1) * (Constants.LongDistance - 1));
			}
			return labels;
		}
		
		#endregion
		
		#endregion

		/// <summary>
		/// Обработчик события смены родителя у описания узла решения
		/// </summary>
		void StepDescriptionParentChanged(object sender, EventArgs e) {
			if (Owner != null) {
				Owner.BorderStyle = Parent != null ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
				Owner.BackColor = Parent != null ? Color.LightSkyBlue : Color.WhiteSmoke;
				if (Parent != null) {
					var nodeTree = Parent.Controls[0];
					Location = new Point(nodeTree.Location.X, nodeTree.Location.Y + nodeTree.Height);
					if (!_isRepresented)
						Represent();
				}
			} else {
				throw new InvalidOperationException("Описание не прикреплено к узлу решения!");
			}
		}
		
		#endregion

		#region Методы для расширения описания
		
		/// <summary>
		/// Метод добавления информации перед нахождением минимумов
		/// </summary>
		/// <param name="type">Тип искомых минимумов</param>
		public void AddInfoBeforeMinimums(MinimumsType type)
		{
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				var matrix = _matrixOfValues.Clone() as double[,];
				_renderTasks.Add(new Task(() => {
					AddDescriptionText(string.Format(DescriptionMessages.FindingMinimums,
		            	type == MinimumsType.MinimumsByRows ? "строкам" : "столбцам"));
					AddMatrix(matrix);
	            }));
			}
		}
		/// <summary>
		/// Метод добавления найденных минимумов и новой матрицы
		/// </summary>
		/// <param name="minimums">Полученные минимумы</param>
		/// <param name="type">Тип полученных минимумов</param>
		public void AddMinimumsAndNewMatrix(double[] minimums, MinimumsType type)
		{
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				var matrix = _matrixOfValues.Clone() as double[,];
				_renderTasks.Add(new Task(() => {
					switch (type) {
						case MinimumsType.MinimumsByRows:
							AddMinimumsByRows(minimums);
							AddDescriptionText(string.Format(DescriptionMessages.SubtractingMinimums, "строкам"));
							break;
						case MinimumsType.MinimumsByColumns:
							AddMinimumsByColumns(minimums);
							AddDescriptionText(string.Format(DescriptionMessages.SubtractingMinimums, "столбцам"));
							break;
					}
					AddMatrix(matrix);
	            }));
        	}
		}
		/// <summary>
		/// Метод добавления таблицы штрафов за неиспользование
		/// </summary>
		/// <param name="isValid">Флаг валидности матрицы</param>
		/// <param name="surcharges">Штрафы за неиспользование</param>
		public void AddSurcharges(bool isValid, Dictionary<Point, double> surcharges)
		{
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				_renderTasks.Add(new Task(() => {
					if (isValid) {
						AddDescriptionText(DescriptionMessages.FindingSurcharges);
						AddSurcharges(surcharges);
					}
                }));
			}
		}
		/// <summary>
		/// Метод добавления найденной минимальной границы
		/// </summary>
		/// <param name="order">Порядок найденной границы</param>
		/// <param name="limit">Минимальная граница</param>
		public void AddLimit(string order, double limit)
		{
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				_renderTasks.Add(new Task(() =>
					AddDescriptionText(string.Format(DescriptionMessages.DisplayMinimumLimit, order, limit))));
			}
		}
		/// <summary>
		/// Метод добавления разделяющего ребра
		/// </summary>
		/// <param name="rib">Ребро</param>
		public void AddSeparatingRib(Point rib)
		{
			if (_useHorizontalLines.Count > 0 && _useVerticalLines.Count > 0) {
				_renderTasks.Add(new Task(() =>
					AddDescriptionText(string.Format(DescriptionMessages.DisplaySeparatingRib, rib.X + 1, rib.Y + 1))));
			}
		}
		/// <summary>
		/// Метод добавления полученного результата
		/// </summary>
		/// <param name="route"></param>
		/// <param name="routeLength"></param>
		public void AddResult(string route, double routeLength)
		{
			_renderTasks.Add(new Task(() =>
				AddDescriptionText(string.Format(DescriptionMessages.SolvingResult, route, routeLength))));
		}
	}
	
	#endregion
}
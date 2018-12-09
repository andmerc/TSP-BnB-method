using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace TspApp {
	/// <summary> Description of MainForm. </summary>
	public partial class MainForm : Form {
		/// <summary>
		/// Вводимая матрица
		/// </summary>
		EntryCostMatrixView _costMatrix;
		/// <summary>
		/// Выбранное количество пунктов маршрута
		/// </summary>
		int _countOfPoints { get { return int.Parse(DropDown.SelectedItem.ToString()); } }
		
		/// <summary>
		/// Инициализирующий конструктор для главной формы
		/// </summary>
		public MainForm()
		{
			var resourceManager = new ResourceManager("TspApp.Resources", Assembly.GetExecutingAssembly());
			Icon = (Icon)resourceManager.GetObject("iconTSP");
			InitializeComponent();
			DropDown.SelectedIndex = 2;
		}
		
		#region Обработчики событий
		
		/// <summary>
		/// Метод для создания вводимой матрицы затрат
		/// </summary>
		void DropDownSelectedValueChanged(object sender, EventArgs e)
		{
			Answer.Visible = SolutionView.Visible = false;
			if (_costMatrix != null) _costMatrix.Dispose();
			var x = (Constants.InputZoneLength - (Constants.LongDistance - 1) * _countOfPoints) / 2;
			_costMatrix = new EntryCostMatrixView(_countOfPoints, new Point(x, Constants.LongDistance * 2));
			Controls.Add(_costMatrix);
		}
		/// <summary>
		/// Метод для решения задачи для введённой матрицы затрат
		/// </summary>
		void SolveMatrix(object sender, EventArgs e)
		{
			var isCostMatrixValid = true;
			try
			{
				var matrix = _costMatrix.GetCellsValues();
				double sum, element;
				for (int d = 0; isCostMatrixValid && d < 2; d++)
				{
					for (int i = 0; isCostMatrixValid && i < matrix.GetLength(d); i++)
					{
						sum = 0;
						for (int j = 0; isCostMatrixValid && j < matrix.GetLength(Math.Abs(d - 1)); j++)
						{
							element = d < 1 ? matrix[i, j] : matrix[j, i];
							isCostMatrixValid &= element >= 0;
							if (!double.IsInfinity(element)) sum += element;
						}
						isCostMatrixValid &= !sum.Equals(0);
					}
				}
			}
			catch (FormatException)
			{
				isCostMatrixValid = false;
			}
			if (isCostMatrixValid)
			{
				SolutionView.Controls.Clear();
				var solution = new SolutionTree(_costMatrix.GetCellsValues());
				string format = "Маршрут: {0}\nДлина маршрута: {1}", route = string.Join("⇒", solution.Route);
				Answer.Text = string.Format(format, route, solution.RouteLength);
				var x = (Constants.InputZoneLength - Answer.Width + Constants.LongDistance) / 2;
				Answer.Location = new Point(x, Constants.LongDistance * 2 + _costMatrix.Height);
				SolutionView.Controls.Add(solution.SolutionTreeView);
				Answer.Visible = SolutionView.Visible = true;
			}
			else
			{
				MessageBox.Show("Матрица затрат некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		/// <summary>
		/// Метод для изменения размеров отображения решения
		/// </summary>
		void SetSolutionViewSize(object sender, EventArgs e)
		{
			var maxWidth = Width - Constants.InputZoneLength - Constants.MediumDistance * 3;
			SolutionView.Size = new Size(maxWidth, Height - Constants.ShortDistance * 7);
		}
		/// <summary>
		/// Метод заполнения матрицы затрат случайными значениями
		/// </summary>
		void SetRandomValues(object sender, EventArgs e)
		{
			Answer.Visible = SolutionView.Visible = false;
			var matrix = new string[_countOfPoints, _countOfPoints];
			var random = new Random();
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
					matrix[i, j] = (random.NextDouble() < 0.03 ?
						double.PositiveInfinity : random.Next(1, 100)).ToString();
			_costMatrix.SetCellsValues(matrix);
		}
		/// <summary>
		/// Метод очистки матрицы затрат
		/// </summary>
		void ClearMatrix(object sender, EventArgs e)
		{
			Answer.Visible = SolutionView.Visible = false;
			var matrix = new string[_countOfPoints, _countOfPoints];
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
					matrix[i, j] = default(string);
			_costMatrix.SetCellsValues(matrix);
		}
	}
	
	#endregion
}
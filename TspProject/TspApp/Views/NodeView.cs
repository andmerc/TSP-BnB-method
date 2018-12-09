using System;
using System.Drawing;
using System.Windows.Forms;

namespace TspApp
{
	/// <summary>
	/// Класс отображения узла решения дерева отображения решения
	/// </summary>
	public partial class NodeView : TableLayoutPanel
	{
		/// <summary>
		/// Подробное описание шага
		/// </summary>
		public StepDescription StepDescription { get; set; }
		/// <summary>
		/// Ссылка на узел с исключением ребра, найденного на прошлом шаге
		/// </summary>
		public NodeView Excluded { get; set; }
		/// <summary>
		/// Ссылка на узел с включением ребра, найденного на прошлом шаге
		/// </summary>
		public NodeView Included { get; set; }
		/// <summary>
		/// Флаг разделённости узла
		/// </summary>
		public bool IsSeparated {
			get {
				if (Excluded == null && Included != null || Excluded != null && Included == null)
					throw new NotSupportedException("Задан только один дочерний узел из двух необходимых");
				return Excluded != null && Included != null;
			}
		}
		
		/// <summary>
		/// Конструктор для корневого узла дерева решения
		/// </summary>
		/// <param name="nodeText">Текст корневого узла</param>
		/// <param name="matrix">Матрица корневого узла</param>
		public NodeView(string nodeText, CostMatrix matrix)
		{
			InitializeComponent(nodeText, matrix.MinimumLimit.ToString());
			StepDescription = matrix.Description;
			StepDescription.Owner = this;
		}		
		/// <summary>
		/// Конструктор для дочерних узлов дерева решения
		/// </summary>
		/// <param name="rib">Ребро (включённое или исключённое из множества маршрутов)</param>
		/// <param name="matrix">Матрица дочернего узла</param>
		/// <param name="type">Тип дочернего узла</param>
		public NodeView(Point rib, CostMatrix matrix, NodeType type)
			: this("(" + (rib.X + 1) + ":" + (rib.Y + 1) + ")", matrix)
		{
			if (type == NodeType.Exclusion) {
				if (!matrix.IsValid)
					Limit.Text = double.PositiveInfinity.ToString();
				var loc = new Point(Rib.Location.X + 3, Rib.Location.Y + 3);
				using (var g = Graphics.FromImage(BackgroundImage = new Bitmap(Width, Height)))
					g.DrawLine(new Pen(Color.Black, 1), loc, new Point(loc.X + Rib.Width - 9, loc.Y));
			}
		}
		
		/// <summary>
		/// Метод для отображения/скрывания описания узла решения
		/// </summary>
		void ShowOrHideDescription(object sender, EventArgs e)
		{
			var nodeParent = Parent as SolutionTreeView;
			if (nodeParent != null) {
				var parent = nodeParent.Parent as Panel;
				if (parent != null) {
					var displayed = false;
					if (parent.Controls.Count == 2) {
						displayed = parent.Controls[1] as StepDescription == StepDescription;
						parent.Controls.RemoveAt(1);
					}
					if (!displayed)
						parent.Controls.Add(StepDescription);
				}
			}
		}
	}
}
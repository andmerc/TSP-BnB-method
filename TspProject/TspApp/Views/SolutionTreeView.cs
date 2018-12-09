using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace TspApp
{
	/// <summary>
	/// Класс, описывающий отображение дерева решения
	/// </summary>
	public partial class SolutionTreeView : Panel
	{
		/// <summary>
		/// Конструктор дерева отображения по корневому узлу решения
		/// </summary>
		/// <param name="nodeView">Корневой узел решения</param>
		public SolutionTreeView(NodeView nodeView)
		{
			InitializeComponent();
			CreateTree(nodeView);
		}
		
		#region Приватные методы
		
		/// <summary>
		/// Метод создания дерева отображения
		/// </summary>
		/// <param name="rootNodeView">Корневой узел решения</param>
		void CreateTree(NodeView rootNodeView)
		{
			SetNodes(rootNodeView, Constants.ShortDistance, Constants.ShortDistance);
			BackgroundImage = new Bitmap(PreferredSize.Width, PreferredSize.Height);
			DrawLinesForNode(new Pen(Color.Black), rootNodeView);
		}
		/// <summary>
		/// Метод настройки узлов решения в редеве отображения
		/// </summary>
		/// <param name="node">Узел</param>
		/// <param name="x">Абсцисса положения узла</param>
		/// <param name="y">Ордината положения узла</param>
		void SetNodes(NodeView node, int x, int y)
		{
			Controls.Add(node);
			if (node.IsSeparated) {
				const int dy = Constants.ShortDistance * 5;
				SetNodes(node.Excluded, x, y + dy);
				SetNodes(node.Included, PreferredSize.Width + Constants.ShortDistance, y + dy);
				x = (node.Included.Location.X + node.Excluded.Location.X) / 2;
			}
			node.Location = new Point(x, y);
		}
		/// <summary>
		/// Метод отрисовки линий, соединяющих родительский и его дочерние узлы
		/// </summary>
		/// <param name="pen">Карандаш для отрисовки линий</param>
		/// <param name="node">Узел, от которого рисуются линии</param>
		void DrawLinesForNode(Pen pen, NodeView node)
		{
			if (node.IsSeparated) {
				Point nodeBottom = new Point(node.Location.X + node.Width / 2, node.Location.Y + node.Height),
				excTop = new Point(node.Excluded.Location.X + node.Excluded.Width / 2, node.Excluded.Location.Y - 1),
				incTop = new Point(node.Included.Location.X + node.Included.Width / 2, node.Included.Location.Y - 1);
				using (var g = Graphics.FromImage(BackgroundImage)) {
					g.DrawLine(pen, new Point(nodeBottom.X - Constants.ShortDistance / 2, nodeBottom.Y), excTop);
					g.DrawLine(pen, new Point(nodeBottom.X + Constants.ShortDistance / 2, nodeBottom.Y), incTop);
				}
				DrawLinesForNode(pen, node.Excluded);
				DrawLinesForNode(pen, node.Included);
			}
		}
		
		#endregion
	}
}
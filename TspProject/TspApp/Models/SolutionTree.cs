using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TspApp
{
	/// <summary>
	/// Класс дерева решения задачи коммивояжера методом ветвей и границ
	/// </summary>
	public class SolutionTree
	{
		/// <summary>
		/// Класс ветви решения
		/// </summary>
		class Node
		{
			/// <summary>
			/// Отображение узла решения
			/// </summary>
			public NodeView NodeView { get; set; }
			/// <summary>
			/// Неупорядоченный список ребёр решения
			/// </summary>
			public List<Point> RibsOfRoute { get; set; }
			/// <summary>
			/// Матрица затрат
			/// </summary>
			public CostMatrix Matrix { get; set; }
			
			/// <summary>
			/// Ветвь с исключением разделяющего ребра
			/// </summary>
			public Node Excluded { get; set; }
			/// <summary>
			/// Ветвь с включением разделяющего ребра
			/// </summary>
			public Node Included { get; set; }
			
			/// <summary>
			/// Конструктор корневого узла решения
			/// </summary>
			/// <param name="matrix">Матрица затрат</param>
			public Node(CostMatrix matrix) {
				RibsOfRoute = new List<Point>();
				Matrix = matrix;
				NodeView = new NodeView("Root", Matrix);				
			}
			/// <summary>
			/// Конструктор дочерних узлов решения
			/// </summary>
			/// <param name="parentNode">Родительский узел решения</param>
			/// <param name="type">Тип узла решения</param>
			public Node(Node parentNode, NodeType type) {
				switch (type) {
					case NodeType.Exclusion:
						RibsOfRoute = parentNode.RibsOfRoute.GetRange(0, parentNode.RibsOfRoute.Count);
						Matrix = parentNode.Matrix.ExcludeRibWithoutConfusions(RibsOfRoute);
						NodeView = new NodeView(parentNode.Matrix.RibWithMaxSurcharge, Matrix, type);
						parentNode.NodeView.Excluded = NodeView;
						break;
					case NodeType.Inclusion:
						RibsOfRoute = parentNode.RibsOfRoute
							.Concat(new List<Point> { parentNode.Matrix.RibWithMaxSurcharge }).ToList();
						Matrix = parentNode.Matrix.IncludeRibWithoutConfusions(RibsOfRoute);
						NodeView = new NodeView(parentNode.Matrix.RibWithMaxSurcharge, Matrix, type);
						parentNode.NodeView.Included = NodeView;
						break;
				}
			}
		}
		
		/// <summary>
		/// Исходная матрица
		/// </summary>
		readonly int _countOfPoints;
		
		/// <summary>
		/// Представление дерева решения
		/// </summary>
		public SolutionTreeView SolutionTreeView { get; set; }
		/// <summary>
		/// Маршрут движения
		/// </summary>
		public List<int> Route { get; set; }
		/// <summary>
		/// Длина маршрута
		/// </summary>
		public double RouteLength { get; set; }
		
		/// <summary>
		/// Конструктор дерева решения
		/// </summary>
		/// <param name="matrix">Исходная матрица значений</param>
		public SolutionTree(double[,] matrix)
		{
			_countOfPoints = matrix.GetLength(0);
			Solve(new Node(new CostMatrix(matrix)));
		}

		/// <summary>
		/// Метод решения задачи коммивояжёра
		/// </summary>
		/// <param name="rootNode">Главный узел решения</param>
		void Solve(Node rootNode)
		{
			var notSeparatedNodes = new List<Node> { rootNode };
			// Пока не найден путь, разделяем узлы на ветви
			while (Route == null) {
				// Ищем минимальную границу
				var limit = notSeparatedNodes.Min(_ => _.Matrix.MinimumLimit);
				// Находим узел, который соответствует минимальной границе
				var node = notSeparatedNodes.Last(_ => _.Matrix.MinimumLimit.Equals(limit));
				// Убираем его из списка неразделённых узлов
				notSeparatedNodes.Remove(node);
				// Если находим путь, то устанавливаем его длину
				if (TryGetRouteFromRibs(node.RibsOfRoute)) {
					RouteLength = node.Matrix.MinimumLimit;
					node.Matrix.Description.AddResult(string.Join("⇒", Route), RouteLength);
				// Иначе если матрица узла валидна, то разделяем узел на ветви
				} else if (node.Matrix.IsValid) {
					node.Matrix.Description.AddSeparatingRib(node.Matrix.RibWithMaxSurcharge);
					node.Excluded = new Node(node, NodeType.Exclusion);
					node.Included = new Node(node, NodeType.Inclusion);
					notSeparatedNodes.AddRange(new List<Node> { node.Excluded, node.Included });
				}
			}
			// После нахождения решения создаём дерево отображения
			SolutionTreeView = new SolutionTreeView(rootNode.NodeView);
		}
		/// <summary>
		/// Метод получения маршрута
		/// </summary>
		/// <param name="ribsOfRoute">Неупорядоченный список рёбер</param>
		/// <returns>Если найден цикл, то устанавливает маршрут и возвращает <c>true</c>, иначе <c>false</c></returns>
		bool TryGetRouteFromRibs(List<Point> ribsOfRoute)
		{
			// Если количество рёбер не совпадает с количеством пунктов, то нет смысла строить маршрут
			if (ribsOfRoute.Count != _countOfPoints)
				return false;
			// Иначе строим маршрут
			var list = new List<int> { 1 };
			for (int i = 0; i < ribsOfRoute.Count; i++) {
				var rib = ribsOfRoute.FirstOrDefault(_ => _.X == list.Last() - 1);
				if (!rib.IsEmpty)
					list.Add(rib.Y + 1);
			}
			
			var result = list.Count == _countOfPoints + 1;
			if (result)
				Route = list;
			return result;
		}
	}
}
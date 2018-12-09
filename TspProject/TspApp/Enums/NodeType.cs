using System;

namespace TspApp
{
	/// <summary>
	/// Указывает тип узла решения
	/// </summary>
	public enum NodeType
	{
		/// <summary>
		/// Узел с исключением ребра
		/// </summary>
		Exclusion = 0,
		/// <summary>
		/// Узел с включением ребра
		/// </summary>
		Inclusion = 1
	}
}


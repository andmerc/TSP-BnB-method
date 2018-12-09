using System;

namespace TspApp
{
	/// <summary>
	/// Класс для сообщений описания
	/// </summary>
	public static class DescriptionMessages
	{
		// Сообщения
		public static readonly string StartOfSolving		= "Начало решения задачи для введённой матрицы маршрутов.";
		public static readonly string StartOfProcessing 	= "Начало обработки матрицы:";
		public static readonly string FindingSurcharges 	= "Вычисляем штрафы за неиспользование нулевых рёбер полученной матрицы:";
		// Шаблоны для сообщений
		public static readonly string FindingMinimums 		= "Находим минимумы по {0}";
		public static readonly string SubtractingMinimums 	= "Вычитаем минимумы по {0}. Переходим к матрице:";
		public static readonly string DisplayMinimumLimit 	= "{0} минимальная граница маршрута на данном этапе - {1}";
		public static readonly string DisplaySeparatingRib 	= "Ребро с максимальным штрафом - ({0}:{1}). Разделяем по нему данное множество";
		public static readonly string SolvingResult 		= "Решение найдено!\nМинимальный маршрут: {0}\nДлина маршрута: {1}";
	}
}

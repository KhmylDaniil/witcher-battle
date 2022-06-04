namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Ограничения параметра
	/// </summary>
	public class ParameterBound
	{
		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected ParameterBound()
		{
		}

		/// <summary>
		/// Конструктор класса Ограничения параметра
		/// </summary>
		/// <param name="minValueParameter">Минимальное значение параметра</param>
		/// <param name="maxValueParameter">Максимальное значение параметра</param>
		public ParameterBound(
			int minValueParameter,
			int maxValueParameter
			)
		{
			MinValueParameter = minValueParameter;
			MaxValueParameter = maxValueParameter;
		}

		/// <summary>
		/// Минимальное значение параметра
		/// </summary>
		public int MinValueParameter { get; set; }

		/// <summary>
		/// Максимальное значение параметра
		/// </summary>
		public int MaxValueParameter { get; set; }
	}
}

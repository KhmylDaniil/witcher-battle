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
			double minValueParameter,
			double maxValueParameter
			)
		{
			MinValueParameter = minValueParameter;
			MaxValueParameter = maxValueParameter;
		}

		/// <summary>
		/// Минимальное значение параметра
		/// </summary>
		public double MinValueParameter { get; set; }

		/// <summary>
		/// Максимальное значение параметра
		/// </summary>
		public double MaxValueParameter { get; set; }
	}
}

namespace Wastelands.Service.Infrastructure.Options
{
	/// <summary>
	/// параметры хеширования
	/// </summary>
	public class HasherOptions
	{
		public const string SectionName = "HasherOptions";

		/// <summary>
		/// соль
		/// </summary>
		public string Salt { get; set; }
	}
}

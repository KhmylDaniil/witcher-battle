using System.Reflection;

namespace Sindie.ApiService.WebApi
{
	/// <summary>
	/// Данные о сборке
	/// </summary>
	public record AssemblyInformation(string Product, string Description, string Version)
	{
		/// <summary>
		/// Текущая сборка - веб
		/// </summary>
		public static readonly AssemblyInformation Current = new(typeof(AssemblyInformation).Assembly);

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="assembly">Сборка</param>
		public AssemblyInformation(Assembly assembly)
			: this(
				assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product,
				assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description,
				assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version)
		{
		}
	}
}

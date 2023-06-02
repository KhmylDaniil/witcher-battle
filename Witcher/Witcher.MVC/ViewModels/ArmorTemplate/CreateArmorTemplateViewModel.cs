using Witcher.Core.Contracts.ArmorTemplateRequests;

namespace Witcher.MVC.ViewModels.ArmorTemplate
{
	public class CreateArmorTemplateViewModel : CreateArmorTemplateCommand
	{
		/// <summary>
		/// Данные шаблонов тела
		/// </summary>
		public Dictionary<Guid, string> BodyTemplatesDictionary { get; set; }
	}
}

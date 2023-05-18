using Witcher.Core.Contracts.ItemRequests;

namespace Witcher.MVC.ViewModels.Item
{
	public class CreateItemViewModel : CreateItemCommand
	{
		/// <summary>
		/// Данные шаблонов предмета
		/// </summary>
		public Dictionary<Guid, string> ItemTemplatesDictionary { get; set; }
	}
}

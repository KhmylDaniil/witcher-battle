using Witcher.Core.Contracts.BattleRequests;

namespace Witcher.MVC.ViewModels.Battle
{
	/// <summary>
	/// Модель представления для команды создания существа в битве
	/// </summary>
	public class CreateCreatureCommandViewModel : CreateCreatureCommand
	{
		/// <summary>
		/// Данные шаблонов существа
		/// </summary>
		public Dictionary<Guid, string> CreatureTemplatesDictionary { get; set; }
	}
}

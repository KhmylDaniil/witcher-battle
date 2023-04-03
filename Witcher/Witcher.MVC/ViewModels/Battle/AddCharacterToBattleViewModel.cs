using Witcher.Core.Contracts.BattleRequests;

namespace Witcher.MVC.ViewModels.Battle
{
	/// <summary>
	/// Модель представления для команды добавления персонажа в битву
	/// </summary>
	public class AddCharacterToBattleViewModel : AddCharacterToBattleCommand
	{
		/// <summary>
		/// Данные персонажей
		/// </summary>
		public Dictionary<Guid, (string name, string owner)> CharactersDictionary { get; set; } = new();
	}
}

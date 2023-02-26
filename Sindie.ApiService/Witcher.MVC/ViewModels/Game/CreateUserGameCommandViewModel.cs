using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;

namespace Witcher.MVC.ViewModels.Game
{
	public class CreateUserGameCommandViewModel : CreateUserGameCommand
	{
		/// <summary>
		/// Данные пользователей
		/// </summary>
		public Dictionary<Guid, string> UserDictionary { get; set; }

		/// <summary>
		/// Данные ролей
		/// </summary>
		public Dictionary<Guid, string> GameRolesDictionary { get; set; } = new Dictionary<Guid, string>
		{
			{ GameRoles.MasterRoleId, GameRoles.MasterRoleName },
			{ GameRoles.PlayerRoleId, GameRoles.PlayerRoleName }
		};
	}
}

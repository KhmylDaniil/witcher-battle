using PilotProject.DbContext;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle;

namespace PilotProject.CommandBuilders
{
	internal class BattleCommandBuilder
	{
		/// <summary>
		/// Словарь для сбора данных о бое
		/// </summary>
		public static readonly Dictionary<string, Guid> PickedCreatureTemplates = new();

		internal static CreateBattleCommand FormCreateBattleCommand()
		{
			var creatures = new List<CreateBattleRequestItem>();

			foreach (var pickedCreature in PickedCreatureTemplates)
				creatures.Add(new CreateBattleRequestItem
				{
					CreatureTemplateId = pickedCreature.Value,
					Name = pickedCreature.Key
				});

			CreateBattleRequest request = new()
			{
				GameId = TestDbContext.GameId,
				ImgFileId = null,
				Name = "TestName",
				Description = null,
				Creatures = creatures
			};

			PickedCreatureTemplates.Clear();

			return CreateBattleCommandFromQuery(request);

			static CreateBattleCommand CreateBattleCommandFromQuery(CreateBattleRequest request)
				=> request == null
					? throw new ArgumentNullException(nameof(request))
					: new CreateBattleCommand(
						gameId: request.GameId,
						imgFileId: request.ImgFileId,
						name: request.Name,
						description: request.Description,
						creatures: request.Creatures);
		}

		internal static void PickCreature(Guid id)
		{
			Console.WriteLine($"Enter unique name for this creature");
			string name = string.Empty;
			while (string.IsNullOrEmpty(name))
				name = Console.ReadLine();

			PickedCreatureTemplates.Add(name, id);
		}
	}
}

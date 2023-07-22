using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CharacterRequests;

namespace Witcher.UnitTest.Core.Requests.CharacterRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteCharacterHandler"/>
	/// </summary>
	[TestClass]
	public sealed class DeleteCharacterHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Character _character;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteCharacterHandler"/>
		/// </summary>
		public DeleteCharacterHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);
			_character = Character.CreateForTest(game: _game, creatureTemlpate: _creatureTemplate);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _bodyTemplate, _creatureTemplate, _character));
		}

		/// <summary>
		/// Тест метода Handle - удаление персонажа по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteCreatureTemplate_ShouldReturnUnit()
		{
			var request = new DeleteCharacterCommand()
			{
				Id = _character.Id,
			};

			var newHandler = new DeleteCharacterHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.Characters
				.FirstOrDefault(x => x.Id == _character.Id));
		}
	}
}

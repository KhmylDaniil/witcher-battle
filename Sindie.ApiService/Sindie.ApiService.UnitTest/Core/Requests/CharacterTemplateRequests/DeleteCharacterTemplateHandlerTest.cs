using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.DeleteCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CharacterTemplateRequests.DeleteCharacterTemplate;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CharacterTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteCharacterTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteCharacterTemplateHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly CharacterTemplate _characterTemplate;

		/// <summary>
		/// Конструктор для <see cref="DeleteCharacterTemplateHandlerTest"/>
		/// </summary>
		public DeleteCharacterTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_characterTemplate = CharacterTemplate.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_characterTemplate));
		}

		/// <summary>
		/// Тест метода Handle, должен удалить шаблон персонажа и зависимые сущности
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteCharacterTemplate_ShouldReturnUnit()
		{
			var request = new DeleteCharacterTemplateCommand()
			{ Id = _characterTemplate.Id };

			var newHandler = new DeleteCharacterTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.CharacterTemplates
				.FirstOrDefault(x => x.Id == _characterTemplate.Id));
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteCreatureTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteCreatureTemplateByIdHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteCreatureTemplateByIdHandler"/>
		/// </summary>
		public DeleteCreatureTemplateByIdHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate, creatureType: CreatureType.Human);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _bodyTemplate, _creatureTemplate));
		}

		/// <summary>
		/// Тест метода Handle - удаление шаблона существа по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteCreatureTemplate_ShouldReturnUnit()
		{
			var request = new DeleteCreatureTemplateByIdCommand()
			{
				Id = _creatureTemplate.Id,
			};

			var newHandler = new DeleteCreatureTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.CreatureTemplates
				.FirstOrDefault(x => x.Id == _creatureTemplate.Id));
		}
	}
}

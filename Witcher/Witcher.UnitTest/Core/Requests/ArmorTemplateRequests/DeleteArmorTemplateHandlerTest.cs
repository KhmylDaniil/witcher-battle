using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.ArmorTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.ArmorTemplateRequests
{
	[TestClass]
	public class DeleteArmorTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ArmorTemplate _armorTemplate;
		private readonly BodyTemplate _bodyTemplate;

		public DeleteArmorTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_armorTemplate = ArmorTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _armorTemplate, _bodyTemplate));
		}


		[TestMethod]
		public async Task Handle_DeleteArmorTemplate_ShouldReturnUnit()
		{
			var request = new DeleteArmorTemplateCommand()
			{
				Id = _armorTemplate.Id,
			};

			var newHandler = new DeleteArmorTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.ItemTemplates
				.FirstOrDefault(x => x.Id == _armorTemplate.Id));
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{
	[TestClass]
	public sealed class DeleteWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;


		public DeleteWeaponTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate));
		}


		[TestMethod]
		public async Task Handle_DeleteWeaponTemplate_ShouldReturnUnit()
		{
			var request = new DeleteWeaponTemplateCommand()
			{
				Id = _weaponTemplate.Id,
			};

			var newHandler = new DeleteWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.ItemTemplates
				.FirstOrDefault(x => x.Id == _weaponTemplate.Id));
		}
	}
}

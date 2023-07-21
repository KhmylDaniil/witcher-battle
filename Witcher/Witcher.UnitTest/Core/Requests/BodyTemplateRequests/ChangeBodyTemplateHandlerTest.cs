using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BodyTemplateRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeBodyTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeBodyTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeBodyTemplateHandler"/>
		/// </summary>
		public ChangeBodyTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _bodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle - изменение шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBodyTemplate_ShouldReturnUnit()
		{
			var request = new ChangeBodyTemplateCommand()
			{
				Id = _bodyTemplate.Id,
				Name = "name",
				Description = "description"
			};

			var newHandler = new ChangeBodyTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();

			Assert.AreEqual(_game.Id, bodyTemplate.GameId);
			Assert.IsNotNull(bodyTemplate.Name);
			Assert.AreEqual(request.Name, bodyTemplate.Name);
			Assert.AreEqual(request.Description, bodyTemplate.Description);
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);
		}
	}
}

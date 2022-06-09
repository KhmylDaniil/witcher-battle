using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplateRequests
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
			var request = new ChangeBodyTemplateCommand(
				id: _bodyTemplate.Id,
				gameId: _game.Id,
				name: "name",
				description: "description",
				bodyTemplateParts: new List<ChangeBodyTemplateRequestItem>
				{
					new ChangeBodyTemplateRequestItem()
					{
						Name = "head",
						HitPenalty = 3,
						DamageModifier = 2,
						MinToHit = 1,
						MaxToHit = 5
					},
					new ChangeBodyTemplateRequestItem()
					{
						Name = "body",
						HitPenalty = 2,
						DamageModifier = 2,
						MinToHit = 6,
						MaxToHit = 10
					}
				});

			var newHandler = new ChangeBodyTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();

			Assert.AreEqual(request.GameId, bodyTemplate.GameId);
			Assert.IsNotNull(bodyTemplate.Name);
			Assert.AreEqual(request.Name, bodyTemplate.Name);
			Assert.AreEqual(request.Description, bodyTemplate.Description);
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);

			var head = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "head");
			Assert.IsNotNull(head);
			Assert.AreEqual(head.DamageModifier, 2);
			Assert.AreEqual(head.HitPenalty, 3);
			Assert.AreEqual(head.MinToHit, 1);
			Assert.AreEqual(head.MaxToHit, 5);

			var body = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "body");
			Assert.IsNotNull(body);
			Assert.AreEqual(body.DamageModifier, 2);
			Assert.AreEqual(body.HitPenalty, 2);
			Assert.AreEqual(body.MinToHit, 6);
			Assert.AreEqual(body.MaxToHit, 10);
		}
	}
}

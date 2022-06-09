using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplateById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="GetBodyTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class GetBodyTemplateByIdHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="GetBodyTemplateByIdHandler"/>
		/// </summary>
		public GetBodyTemplateByIdHandlerTest() : base()
		{

			_game = Game.CreateForTest();

			_bodyTemplate = BodyTemplate.CreateForTest(
				name: "testName",
				game: _game,
				bodyTemplateParts: new List<BodyTemplatePart>()
				{new BodyTemplatePart("head", 3, 3, 1, 3), new BodyTemplatePart("body", 1, 1, 4, 10)});

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _bodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle получение шаблона тела по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCharacterTemplate_ShouldReturn_GetCharacterTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetBodyTemplateByIdQuery()
			{
				GameId = _game.Id,
				Id = _bodyTemplate.Id
			};

			var newHandler = new GetBodyTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.GameId, _bodyTemplate.GameId);
			Assert.IsNotNull(_bodyTemplate.Name);
			Assert.AreEqual(result.Name, _bodyTemplate.Name);
			Assert.AreEqual(result.Description, _bodyTemplate.Description);
			Assert.IsNotNull(result.GetBodyTemplateByIdResponseItems);

			var head = result.GetBodyTemplateByIdResponseItems.FirstOrDefault(x => x.Name == "head");
			Assert.IsNotNull(head);
			Assert.AreEqual(head.DamageModifier, 3);
			Assert.AreEqual(head.HitPenalty, 3);
			Assert.AreEqual(head.MinToHit, 1);
			Assert.AreEqual(head.MaxToHit, 3);

			var body = result.GetBodyTemplateByIdResponseItems.FirstOrDefault(x => x.Name == "body");
			Assert.IsNotNull(body);
			Assert.AreEqual(body.DamageModifier, 1);
			Assert.AreEqual(body.HitPenalty, 1);
			Assert.AreEqual(body.MinToHit, 4);
			Assert.AreEqual(body.MaxToHit, 10);
		}
	}
}

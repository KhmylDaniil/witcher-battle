using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

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
			var request = new ChangeBodyTemplateRequest()
			{
				Id = _bodyTemplate.Id,
				GameId = _game.Id,
				Name = "name",
				Description = "description",
				BodyTemplateParts = new List<UpdateBodyTemplateRequestItem>
				{
					new UpdateBodyTemplateRequestItem()
					{
						Name = "head",
						BodyPartType = BodyPartType.Head,
						HitPenalty = 3,
						DamageModifier = 2,
						MinToHit = 1,
						MaxToHit = 5
					},
					new UpdateBodyTemplateRequestItem()
					{
						Name = "body",
						BodyPartType= BodyPartType.Torso,
						HitPenalty = 2,
						DamageModifier = 2,
						MinToHit = 6,
						MaxToHit = 10
					}
				}
			};

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
			Assert.AreEqual(head.BodyPartType, BodyPartType.Head);
			Assert.AreEqual(2, head.DamageModifier);
			Assert.AreEqual(3, head.HitPenalty);
			Assert.AreEqual(1, head.MinToHit);
			Assert.AreEqual(5, head.MaxToHit);

			var body = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "body");
			Assert.IsNotNull(body);
			Assert.AreEqual(body.BodyPartType, BodyPartType.Torso);
			Assert.AreEqual(2, body.DamageModifier);
			Assert.AreEqual(2, body.HitPenalty);
			Assert.AreEqual(6, body.MinToHit);
			Assert.AreEqual(10, body.MaxToHit);
		}
	}
}

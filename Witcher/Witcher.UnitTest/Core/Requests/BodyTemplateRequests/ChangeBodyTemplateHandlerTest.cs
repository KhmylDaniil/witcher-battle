using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BodyTemplateRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

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

			Assert.AreEqual(_game.Id, bodyTemplate.GameId);
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

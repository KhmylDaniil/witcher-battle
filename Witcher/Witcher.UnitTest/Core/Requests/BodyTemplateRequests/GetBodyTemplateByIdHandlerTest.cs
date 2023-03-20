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
	/// Тест для <see cref="GetBodyTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class GetBodyTemplateByIdHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _torso;
		private readonly BodyTemplatePart _head;

		/// <summary>
		/// Конструктор для теста <see cref="GetBodyTemplateByIdHandler"/>
		/// </summary>
		public GetBodyTemplateByIdHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_bodyTemplate = BodyTemplate.CreateForTest(
				name: "testName",
				game: _game);

			_head = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Head,
				hitPenalty: 3,
				damageModifier: 3,
				minToHit: 1,
				maxToHit: 3);

			_torso = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Torso,
				hitPenalty: 1,
				damageModifier: 1,
				minToHit: 4,
				maxToHit: 10);

			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _head, _torso};
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _bodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle получение шаблона тела по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCharacterTemplate_ShouldReturn_GetBodyTemplateByIdResponse()
		{
			var request = new GetBodyTemplateByIdQuery()
			{
				Id = _bodyTemplate.Id
			};

			var newHandler = new GetBodyTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Name);
			Assert.AreEqual(result.Name, _bodyTemplate.Name);
			Assert.AreEqual(result.Description, _bodyTemplate.Description);
			Assert.IsNotNull(result.GetBodyTemplateByIdResponseItems);

			var head = result.GetBodyTemplateByIdResponseItems.FirstOrDefault(x => x.Id == _head.Id);
			Assert.IsNotNull(head);
			Assert.AreEqual(head.Name, "Head");
			Assert.AreEqual(head.DamageModifier, 3);
			Assert.AreEqual(head.HitPenalty, 3);
			Assert.AreEqual(head.MinToHit, 1);
			Assert.AreEqual(head.MaxToHit, 3);

			var torso = result.GetBodyTemplateByIdResponseItems.FirstOrDefault(x => x.Id == _torso.Id);
			Assert.IsNotNull(torso);
			Assert.AreEqual(torso.Name, "Torso");
			Assert.AreEqual(torso.DamageModifier, 1);
			Assert.AreEqual(torso.HitPenalty, 1);
			Assert.AreEqual(torso.MinToHit, 4);
			Assert.AreEqual(torso.MaxToHit, 10);
		}
	}
}

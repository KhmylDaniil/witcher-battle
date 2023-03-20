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
	/// Тест для <see cref="CreateBodyTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class CreateBodyTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="CreateBodyTemplateHandler"/>
		/// </summary>
		public CreateBodyTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateBodyTemplate_ShouldReturnUnit()
		{
			var request = new CreateBodyTemplateCommand()
			{
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
						MaxToHit = 10
					}
				}
			};

			var newHandler = new CreateBodyTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();

			Assert.AreEqual(_game.Id, bodyTemplate.GameId);
			Assert.IsNotNull(bodyTemplate.Name);
			Assert.AreEqual(request.Name, bodyTemplate.Name);
			Assert.AreEqual(request.Description, bodyTemplate.Description);
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.FirstOrDefault();
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual("head", bodyTemplatePart.Name);
			Assert.AreEqual(bodyTemplatePart.BodyPartType, BodyPartType.Head);
			Assert.AreEqual(2, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(3, bodyTemplatePart.HitPenalty, 3);
			Assert.AreEqual(1, bodyTemplatePart.MinToHit);
			Assert.AreEqual(10,bodyTemplatePart.MaxToHit);
		}
	}
}

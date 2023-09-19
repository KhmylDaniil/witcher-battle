using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Drafts.BodyTemplateDrafts;
using Witcher.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Requests.BodyTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeBodyTemplatePartHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeBodyTemplatePartHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _humanBodyTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeBodyTemplatePartHandler"/>
		/// </summary>
		public ChangeBodyTemplatePartHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_humanBodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_humanBodyTemplate.CreateBodyTemplateParts(CreateBodyTemplatePartsDraft.CreateBodyPartsDraft());

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _humanBodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle - изменение части шаблона тела/поглощение части тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBodyTemplatePart_OneBodyTemplatePartIsChanged()
		{
			var request = new ChangeBodyTemplatePartCommand()
			{
				Id = _humanBodyTemplate.Id,
				Name = "void",
				BodyPartType = BodyPartType.Void,
				DamageModifier = 10,
				HitPenalty = 5,
				MinToHit = 7,
				MaxToHit = 8
			};

			var newHandler = new ChangeBodyTemplatePartHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.Find(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(BodyPartType.Void, bodyTemplatePart.BodyPartType);
			Assert.AreEqual(10, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(5, bodyTemplatePart.HitPenalty);
			Assert.AreEqual(7, bodyTemplatePart.MinToHit);
			Assert.AreEqual(8, bodyTemplatePart.MaxToHit);
		}

		/// <summary>
		/// Тест метода Handle - изменение части шаблона тела/заползание части тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBodyTemplatePart_TwoBodyTemplatePartsAreChanged()
		{
			var request = new ChangeBodyTemplatePartCommand()
			{
				Id = _humanBodyTemplate.Id,
				Name = "void",
				BodyPartType = BodyPartType.Void,
				DamageModifier = 10,
				HitPenalty = 5,
				MinToHit = 3,
				MaxToHit = 4
			};

			var newHandler = new ChangeBodyTemplatePartHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.Find(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(BodyPartType.Void, bodyTemplatePart.BodyPartType);
			Assert.AreEqual(10, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(5, bodyTemplatePart.HitPenalty);
			Assert.AreEqual(3, bodyTemplatePart.MinToHit);
			Assert.AreEqual(4, bodyTemplatePart.MaxToHit);

			var torso = bodyTemplate.BodyTemplateParts.Find(x => x.Name == Enum.GetName(BodyPartType.Torso));
			Assert.IsNotNull(torso);
			Assert.AreEqual(BodyPartType.Torso, torso.BodyPartType);
			Assert.AreEqual(1, torso.DamageModifier);
			Assert.AreEqual(1, torso.HitPenalty);
			Assert.AreEqual(2, torso.MinToHit);
			Assert.AreEqual(2, torso.MaxToHit);
		}

		/// <summary>
		/// Тест метода Handle - изменение части шаблона тела/разделение части тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBodyTemplatePart_BodyTemplatePartIsSplitted()
		{
			var request = new ChangeBodyTemplatePartCommand()
			{
				Id = _humanBodyTemplate.Id,
				Name = "void",
				BodyPartType = BodyPartType.Void,
				DamageModifier = 10,
				HitPenalty = 5,
				MinToHit = 3,
				MaxToHit = 3
			};

			var newHandler = new ChangeBodyTemplatePartHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.BodyTemplates.Count());
			var bodyTemplate = _dbContext.BodyTemplates.FirstOrDefault();
			Assert.IsNotNull(bodyTemplate.BodyTemplateParts);

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.Find(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(BodyPartType.Void, bodyTemplatePart.BodyPartType);
			Assert.AreEqual(10, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(5, bodyTemplatePart.HitPenalty);
			Assert.AreEqual(3, bodyTemplatePart.MinToHit);
			Assert.AreEqual(3, bodyTemplatePart.MaxToHit);

			var torso1 = bodyTemplate.BodyTemplateParts.Find(x => x.MaxToHit == 4);
			Assert.IsNotNull(torso1);
			Assert.AreEqual(torso1.Name, Enum.GetName(BodyPartType.Torso));
			Assert.AreEqual(BodyPartType.Torso, torso1.BodyPartType);
			Assert.AreEqual(1, torso1.DamageModifier);
			Assert.AreEqual(1, torso1.HitPenalty);
			Assert.AreEqual(4, torso1.MinToHit);

			var torso2 = bodyTemplate.BodyTemplateParts.Find(x => x.MaxToHit == 2);
			Assert.IsNotNull(torso2);
			Assert.AreEqual(torso2.Name, Enum.GetName(BodyPartType.Torso));
			Assert.AreEqual(BodyPartType.Torso, torso2.BodyPartType);
			Assert.AreEqual(1, torso2.DamageModifier);
			Assert.AreEqual(1, torso2.HitPenalty);
			Assert.AreEqual(2, torso2.MinToHit);
		}
	}
}

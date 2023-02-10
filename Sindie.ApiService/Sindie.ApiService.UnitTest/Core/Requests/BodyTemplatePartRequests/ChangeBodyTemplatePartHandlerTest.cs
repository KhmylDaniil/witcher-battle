using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Drafts.BodyTemplateDrafts;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;
using Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests;
using System.ComponentModel.DataAnnotations;
using Sindie.ApiService.Core.Requests.BodyTemplatePartRequests;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplatePartRequests
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
				GameId = _game.Id,
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

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(bodyTemplatePart.BodyPartType, BodyPartType.Void);
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
				GameId = _game.Id,
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

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(bodyTemplatePart.BodyPartType, BodyPartType.Void);
			Assert.AreEqual(10, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(5, bodyTemplatePart.HitPenalty);
			Assert.AreEqual(3, bodyTemplatePart.MinToHit);
			Assert.AreEqual(4, bodyTemplatePart.MaxToHit);

			var torso = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == Enum.GetName(BodyPartType.Torso));
			Assert.IsNotNull(torso);
			Assert.AreEqual(torso.BodyPartType, BodyPartType.Torso);
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
				GameId = _game.Id,
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

			var bodyTemplatePart = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "void");
			Assert.IsNotNull(bodyTemplatePart);
			Assert.AreEqual(bodyTemplatePart.BodyPartType, BodyPartType.Void);
			Assert.AreEqual(10, bodyTemplatePart.DamageModifier);
			Assert.AreEqual(5, bodyTemplatePart.HitPenalty);
			Assert.AreEqual(3, bodyTemplatePart.MinToHit);
			Assert.AreEqual(3, bodyTemplatePart.MaxToHit);

			var torso1 = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.MaxToHit == 4);
			Assert.IsNotNull(torso1);
			Assert.AreEqual(torso1.Name, Enum.GetName(BodyPartType.Torso));
			Assert.AreEqual(torso1.BodyPartType, BodyPartType.Torso);
			Assert.AreEqual(1, torso1.DamageModifier);
			Assert.AreEqual(1, torso1.HitPenalty);
			Assert.AreEqual(4, torso1.MinToHit);

			var torso2 = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.MaxToHit == 2);
			Assert.IsNotNull(torso2);
			Assert.AreEqual(torso2.Name, Enum.GetName(BodyPartType.Torso));
			Assert.AreEqual(torso2.BodyPartType, BodyPartType.Torso);
			Assert.AreEqual(1, torso2.DamageModifier);
			Assert.AreEqual(1, torso2.HitPenalty);
			Assert.AreEqual(2, torso2.MinToHit);
		}
	}
}

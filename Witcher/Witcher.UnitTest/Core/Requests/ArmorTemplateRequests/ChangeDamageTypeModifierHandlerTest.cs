using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Requests.ArmorTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.ArmorTemplateRequests
{
	[TestClass]
	public class ChangeDamageTypeModifierHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ArmorTemplate _armorTemplate;

		public ChangeDamageTypeModifierHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_armorTemplate = ArmorTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game));

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _armorTemplate));
		}

		/// <summary>
		/// Тест метода Handle - изменение модификатора по типу урона
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_UpdateDamageTypeModifier()
		{
			//create
			var request = new ChangeDamageTypeModifierForArmorTemplateCommand()
			{
				ArmorTemplateId = _armorTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Vulnerability
			};

			var newHandler = new ChangeDamageTypeModifierForArmorTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.ItemTemplates.Count());
			var armorTemplate = _dbContext.ItemTemplates.FirstOrDefault() as ArmorTemplate;
			Assert.IsNotNull(armorTemplate.DamageTypeModifiers);

			var modifier = armorTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNotNull(modifier);
			Assert.AreEqual(DamageTypeModifier.Vulnerability, modifier.DamageTypeModifier);

			//update
			request = new ChangeDamageTypeModifierForArmorTemplateCommand()
			{
				ArmorTemplateId = _armorTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Resistance
			};

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			armorTemplate = _dbContext.ItemTemplates.FirstOrDefault() as ArmorTemplate;
			Assert.IsNotNull(armorTemplate.DamageTypeModifiers);

			modifier = armorTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNotNull(modifier);
			Assert.AreEqual(DamageTypeModifier.Resistance, modifier.DamageTypeModifier);

			//delete
			request = new ChangeDamageTypeModifierForArmorTemplateCommand()
			{
				ArmorTemplateId = _armorTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Normal
			};

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			armorTemplate = _dbContext.ItemTemplates.FirstOrDefault() as ArmorTemplate;
			Assert.IsNotNull(armorTemplate.DamageTypeModifiers);

			modifier = armorTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNull(modifier);
		}
	}
}

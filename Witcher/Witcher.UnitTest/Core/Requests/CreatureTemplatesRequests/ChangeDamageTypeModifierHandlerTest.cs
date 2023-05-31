using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	[TestClass]
	public class ChangeDamageTypeModifierHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly CreatureTemplate _creatureTemplate;

		public ChangeDamageTypeModifierHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game));

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _creatureTemplate));
		}

		/// <summary>
		/// Тест метода Handle - изменение модификатора по типу урона
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_UpdateDamageTypeModifier()
		{
			//create
			var request = new ChangeDamageTypeModifierForCreatureTemplateCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Vulnerability
			};

			var newHandler = new ChangeDamageTypeModifierHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.DamageTypeModifiers);

			var modifier = creatureTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNotNull(modifier);
			Assert.AreEqual(DamageTypeModifier.Vulnerability, modifier.DamageTypeModifier);

			//update
			request = new ChangeDamageTypeModifierForCreatureTemplateCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Resistance
			};

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.DamageTypeModifiers);

			modifier = creatureTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNotNull(modifier);
			Assert.AreEqual(DamageTypeModifier.Resistance, modifier.DamageTypeModifier);

			//delete
			request = new ChangeDamageTypeModifierForCreatureTemplateCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				DamageType = DamageType.Slashing,
				DamageTypeModifier = DamageTypeModifier.Normal
			};

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.DamageTypeModifiers);

			modifier = creatureTemplate.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == DamageType.Slashing);
			Assert.IsNull(modifier);
		}
	}
}

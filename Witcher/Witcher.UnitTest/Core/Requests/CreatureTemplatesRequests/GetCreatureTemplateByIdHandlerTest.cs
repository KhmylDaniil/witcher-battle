using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="GetCreatureTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class GetCreatureTemplateByIdHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly Game _game;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly CreatureTemplateSkill _creatureTemplateSkill;
		private readonly CreatureTemplatePart _creatureTemplatePart;

		/// <summary>
		/// Конструктор для теста <see cref="GetCreatureTemplateByIdHandler"/>
		/// </summary>
		public GetCreatureTemplateByIdHandlerTest() : base()
		{

			_game = Game.CreateForTest();

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game, name: "human");

			_creatureTemplate = CreatureTemplate.CreateForTest(
				name: "testName",
				game: _game,
				creatureType: CreatureType.Human,
				bodyTemplate: _bodyTemplate);

			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 10,
				armor: 5);

			_creatureTemplateSkill = CreatureTemplateSkill.CreateForTest(
				creatureTemplate: _creatureTemplate,
				skill: Skill.Melee,
				value: 5);

			_ability = Ability.CreateForTest(
				game: _game,
				attackSkill: Skill.Melee);
			_ability.AppliedConditions.Add(new AppliedCondition(
				condition: Condition.Bleed,
				applyChance: 100));

			_creatureTemplate.CreatureTemplateSkills.Add(_creatureTemplateSkill);
			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);
			_creatureTemplate.Abilities.Add(_ability);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_bodyTemplate,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle получение шаблона существа по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCreatureTemplateById_ShouldReturn_GetCreatureTemplateByIdResponse()
		{
			var request = new GetCreatureTemplateByIdQuery()
			{
				Id = _creatureTemplate.Id
			};
			var newHandler = new GetCreatureTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.ImgFileId, _creatureTemplate.ImgFileId);
			Assert.AreEqual(result.BodyTemplateId, _bodyTemplate.Id);
			Assert.AreEqual(result.Name, "testName");
			Assert.AreEqual(result.CreatureType, CreatureType.Human);
			Assert.AreEqual(result.Int, _creatureTemplate.Int);
			Assert.AreEqual(result.Ref, _creatureTemplate.Ref);
			Assert.AreEqual(result.Dex, _creatureTemplate.Dex);
			Assert.AreEqual(result.Body, _creatureTemplate.Body);
			Assert.AreEqual(result.Emp, _creatureTemplate.Emp);
			Assert.AreEqual(result.Cra, _creatureTemplate.Cra);
			Assert.AreEqual(result.Emp, _creatureTemplate.Emp);
			Assert.AreEqual(result.Will, _creatureTemplate.Will);
			Assert.AreEqual(result.Speed, _creatureTemplate.Speed);
			Assert.AreEqual(result.Luck, _creatureTemplate.Luck);
			Assert.AreEqual(result.CreatedByUserId, _creatureTemplate.CreatedByUserId);
			Assert.AreEqual(result.ModifiedByUserId, _creatureTemplate.ModifiedByUserId);
			Assert.AreEqual(result.CreatedOn, _creatureTemplate.CreatedOn);
			Assert.AreEqual(result.ModifiedOn, _creatureTemplate.ModifiedOn);

			Assert.IsNotNull(result.CreatureTemplateParts);
			var creatureTemplatePart = result.CreatureTemplateParts.First();
			Assert.IsNotNull(creatureTemplatePart);
			Assert.AreEqual(creatureTemplatePart.Id, _creatureTemplatePart.Id);
			Assert.AreEqual(creatureTemplatePart.Name, "Torso");
			Assert.AreEqual(creatureTemplatePart.HitPenalty, 1);
			Assert.AreEqual(creatureTemplatePart.DamageModifier, 1);
			Assert.AreEqual(creatureTemplatePart.MinToHit, 1);
			Assert.AreEqual(creatureTemplatePart.MaxToHit, 10);
			Assert.AreEqual(creatureTemplatePart.Armor, 5);

			Assert.IsNotNull(result.CreatureTemplateSkills);
			var creatureTemplateParameter = result.CreatureTemplateSkills.First();
			Assert.IsNotNull(creatureTemplateParameter);
			Assert.AreEqual(creatureTemplateParameter.Id, _creatureTemplateSkill.Id);
			Assert.AreEqual(creatureTemplateParameter.Skill, Skill.Melee);
			Assert.AreEqual(creatureTemplateParameter.SkillValue, 5);

			Assert.IsNotNull(result.Abilities);
			var ability = result.Abilities.First();
			Assert.IsNotNull(ability);
			Assert.AreEqual(ability.Name, _ability.Name);
			Assert.AreEqual(ability.Id, _ability.Id);
			Assert.AreEqual(ability.AttackSkill, Skill.Melee);
			Assert.AreEqual(ability.Description, _ability.Description);
			Assert.AreEqual(ability.Accuracy, _ability.Accuracy);
			Assert.AreEqual(ability.AttackDiceQuantity, _ability.AttackDiceQuantity);
			Assert.AreEqual(ability.DamageModifier, _ability.DamageModifier);
			Assert.AreEqual(ability.AttackSpeed, _ability.AttackSpeed);

			Assert.IsNotNull(ability.AppliedConditions);
			var appliedCondition = ability.AppliedConditions.First();
			Assert.IsNotNull(appliedCondition);
			Assert.AreEqual(appliedCondition.Condition, Condition.Bleed);
			Assert.AreEqual(appliedCondition.ApplyChance, 100);
		}
	}
}

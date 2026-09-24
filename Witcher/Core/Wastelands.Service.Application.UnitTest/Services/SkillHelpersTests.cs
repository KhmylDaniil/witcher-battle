using FluentAssertions;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class SkillHelpersTests
	{
		// StatGroup — internal (как и SkillHelpers сам), поэтому тестовый метод не может принимать его
		// напрямую как public-параметр (CS0051) — сравниваем через int-приведение внутри тела метода.
		[DataTestMethod]
		[DataRow(Skill.Awareness, (int)StatGroup.Int)]
		[DataRow(Skill.Melee, (int)StatGroup.Str)]
		[DataRow(Skill.Dodge, (int)StatGroup.Rea)]
		[DataRow(Skill.Acrobatics, (int)StatGroup.Dex)]
		[DataRow(Skill.FirstAid, (int)StatGroup.Cra)]
		[DataRow(Skill.Charisma, (int)StatGroup.Emp)]
		[DataRow(Skill.Courage, (int)StatGroup.Wil)]
		public void GetStatGroup_ReturnsExpectedGroupPerSkill(Skill skill, int expectedGroup)
		{
			((int)SkillHelpers.GetStatGroup(skill)).Should().Be(expectedGroup);
		}

		[TestMethod]
		public void GetCharacterStatValue_ReturnsMatchingStat()
		{
			var character = TestBuilders.Character(stat: 8);
			// Все статы у TestBuilders.Character равны, кроме различия по имени — проверяем маппинг группы к нужному полю.
			SkillHelpers.GetCharacterStatValue(character, StatGroup.Str).Should().Be(character.Str);
			SkillHelpers.GetCharacterStatValue(character, StatGroup.Rea).Should().Be(character.Rea);
		}

		[TestMethod]
		public void GetCreatureTemplateStatValue_StrMapsToBody()
		{
			var template = TestBuilders.CreatureTemplate(stat: 9);

			SkillHelpers.GetCreatureTemplateStatValue(template, StatGroup.Str).Should().Be(template.Body);
		}

		[TestMethod]
		public void GetCreatureTemplateStatValue_ReaMapsToRef()
		{
			var template = TestBuilders.CreatureTemplate(stat: 9);

			SkillHelpers.GetCreatureTemplateStatValue(template, StatGroup.Rea).Should().Be(template.Ref);
		}

		[TestMethod]
		public void GetCreatureTemplateStatValue_WilMapsToWill()
		{
			var template = TestBuilders.CreatureTemplate(stat: 9);

			SkillHelpers.GetCreatureTemplateStatValue(template, StatGroup.Wil).Should().Be(template.Will);
		}

		[TestMethod]
		public void GetCharacterSkillValue_NoSkillEntry_ReturnsJustTheStat()
		{
			var character = TestBuilders.Character(stat: 8);

			SkillHelpers.GetCharacterSkillValue(character, Skill.Sword).Should().Be(8); // Sword -> Rea group, stat=8, no bonus.
		}

		[TestMethod]
		public void GetCharacterSkillValue_WithSkillEntry_AddsSkillBonus()
		{
			var character = TestBuilders.Character(stat: 8);
			character.Skills[Skill.Sword] = 4;

			SkillHelpers.GetCharacterSkillValue(character, Skill.Sword).Should().Be(12);
		}

		[TestMethod]
		public void GetCreatureTemplateSkillValue_WithSkillEntry_AddsSkillBonus()
		{
			var template = TestBuilders.CreatureTemplate(stat: 6);
			template.Skills[Skill.Brawl] = 2;

			SkillHelpers.GetCreatureTemplateSkillValue(template, Skill.Brawl).Should().Be(8); // Brawl -> Str group -> Body(6) + 2
		}

		[DataTestMethod]
		[DataRow(1, -4)]
		[DataRow(2, -4)]
		[DataRow(3, -2)]
		[DataRow(4, -2)]
		[DataRow(5, 0)]
		[DataRow(6, 0)]
		[DataRow(7, 2)]
		[DataRow(8, 2)]
		[DataRow(9, 4)]
		[DataRow(10, 4)]
		[DataRow(11, 6)]
		[DataRow(12, 6)]
		[DataRow(13, 8)]
		public void GetMeleeDamageBonus_MatchesBodyMeleeBonusTable(int str, int expectedBonus)
		{
			SkillHelpers.GetMeleeDamageBonus(str).Should().Be(expectedBonus);
		}
	}
}

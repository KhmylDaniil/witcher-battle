using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.AbilityRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeAbilityHandler"/>
		/// </summary>
		public ChangeAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeAbility_ShouldReturnUnit()
		{
			var request = new ChangeAbilityCommand()
			{
				Id = _ability.Id,
				Name = "newName",
				Description = "newDescription",
				AttackDiceQuantity = 2,
				DamageModifier = 4,
				AttackSpeed = 1,
				Accuracy = 1,
				AttackSkill = Skill.Staff,
				DamageType = DamageType.Piercing
			};

			var newHandler = new ChangeAbilityHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.IsNotNull(ability.Name);
			Assert.AreEqual(request.Name, ability.Name);
			Assert.AreEqual(request.Description, ability.Description);
			Assert.AreEqual(2, ability.AttackDiceQuantity);
			Assert.AreEqual(4, ability.DamageModifier);
			Assert.AreEqual(1, ability.AttackSpeed);
			Assert.AreEqual(1, ability.Accuracy);
			Assert.AreEqual(Skill.Staff, ability.AttackSkill);
			Assert.IsNotNull(ability.DefensiveSkills);
			Assert.AreEqual(DamageType.Piercing, ability.DamageType);
			Assert.IsNotNull(ability.AppliedConditions);
		}
	}
}

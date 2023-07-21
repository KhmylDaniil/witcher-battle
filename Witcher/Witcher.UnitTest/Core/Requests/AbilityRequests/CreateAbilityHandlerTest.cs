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
	/// Тест для <see cref="CreateAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class CreateAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="CreateAbilityHandler"/>
		/// </summary>
		public CreateAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game));
		}

		[TestMethod]
		public async Task Handle_CreateAbility_ShouldReturnUnit()
		{
			var request = new CreateAbilityCommand
			{
				Name = "name",
				Description = "description",
				AttackDiceQuantity = 2,
				DamageModifier = 4,
				AttackSpeed = 1,
				Accuracy = 1,
				AttackSkill = Skill.Melee,
				DamageType = DamageType.Slashing
			};

			var newHandler = new CreateAbilityHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault(x => x.Id == result);

			Assert.AreEqual(_game.Id, ability.GameId);
			Assert.IsNotNull(ability.Name);
			Assert.AreEqual(request.Name, ability.Name);
			Assert.AreEqual(request.Description, ability.Description);
			Assert.AreEqual(2, ability.AttackDiceQuantity);
			Assert.AreEqual(4, ability.DamageModifier);
			Assert.AreEqual(1, ability.AttackSpeed);
			Assert.AreEqual(1, ability.Accuracy);
			Assert.AreEqual(Skill.Melee, ability.AttackSkill);
			Assert.IsNotNull(ability.DefensiveSkills);
			Assert.AreEqual(DamageType.Slashing, ability.DamageType);
			Assert.IsNotNull(ability.AppliedConditions);
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests.GetAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="GetAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class GetAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _user;
		private readonly Game _game;
		private readonly Condition _condition;
		private readonly Skill _parameter;
		private readonly Ability _ability;
		private readonly DamageType _damageType;

		/// <summary>
		/// Конструктор для теста <see cref="GetAbilityHandler"/>
		/// </summary>
		public GetAbilityHandlerTest() : base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_parameter = Skill.CreateForTest(name: "attackParameter");
			_condition = Condition.CreateForTest(id: Conditions.BleedId);
			_damageType = DamageType.CreateForTest(id: DamageTypes.SilverId);

			_ability = Ability.CreateForTest(
				game: _game,
				name: "test",
				attackDiceQuantity: 3,
				attackSkill: _parameter,
				damageType: _damageType,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id);

			_ability.AppliedConditions.Add(new AppliedCondition(
				ability: _ability,
				condition: _condition,
				applyChance: 100));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_user,
				_game,
				_damageType,
				_condition,
				_parameter,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle получение списка способностей с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, названию, типу урона, количеству кубов атаки, параметру атаки, применяемому состоянию
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetAbility_ShouldReturn_GetAbilityResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetAbilityCommand(
				gameId: _game.Id,
				name: "test",
				attackSkillId: _parameter.Id,
				damageTypeId: _damageType.Id,
				conditionId: _condition.Id,
				minAttackDiceQuantity: 2,
				maxAttackDiceQuantity: 3,
				userName: "Author",
				creationMinTime: creationMinTime,
				creationMaxTime: creationMaxTime,
				modificationMinTime: modificationMinTime,
				modificationMaxTime: modificationMaxTime,
				pageSize: 2,
				pageNumber: 1,
				orderBy: null,
				isAscending: false);

			var newHandler = new GetAbilityHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.AbilitiesList.First();
			
			var ability = _dbContext.Abilities
				.Include(x => x.AttackSkill)

				.Include(x => x.AppliedConditions)
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(ability);
			Assert.IsTrue(ability.Name.Contains(request.Name));
			Assert.IsTrue(ability.AttackDiceQuantity >= request.MinAttackDiceQuantity
				&& ability.AttackDiceQuantity <= request.MaxAttackDiceQuantity);

			Assert.IsTrue(ability.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(ability.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			Assert.AreEqual(ability.AttackSkillId, _parameter.Id);
			Assert.AreEqual(ability.DamageTypeId, _damageType.Id);
			Assert.IsTrue(ability.AppliedConditions.Any(x => x.ConditionId == _condition.Id));


			var user = _dbContext.Users.FirstOrDefault(x => x.Id == ability.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));
		}
	}
}

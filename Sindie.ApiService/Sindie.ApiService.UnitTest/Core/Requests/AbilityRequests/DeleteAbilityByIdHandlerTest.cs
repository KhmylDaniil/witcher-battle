﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests.DeleteAbilityById;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteAbilityByIdHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteAbilityByIdHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;
		private readonly Parameter _parameter;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteAbilityByIdHandler"/>
		/// </summary>
		public DeleteAbilityByIdHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_parameter = Parameter.CreateForTest(game: _game);
			_ability = Ability.CreateForTest(game: _game, attackParameter: _parameter);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability, _parameter));
		}

		/// <summary>
		/// Тест метода Handle - удаление способности по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteAbility_ShouldReturnUnit()
		{
			var request = new DeleteAbilityByIdCommand()
			{
				Id = _ability.Id,
				GameId = _game.Id
			};

			var newHandler = new DeleteAbilityByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.CreatureTemplates
				.FirstOrDefault(x => x.Id == _ability.Id));
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.ModifierRequests.DeleteModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ModifierRequests.DeleteModifier;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ModifierRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteModifierHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteModifierHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Modifier _modifier;

		/// <summary>
		/// Конструктор для <see cref="DeleteModifierHandlerTest"/>
		/// </summary>
		public DeleteModifierHandlerTest(): base()
		{
			_game = Game.CreateForTest();
			_modifier = Modifier.CreateForTest(game: _game);
			
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_modifier));
		}

		/// <summary>
		/// Тест метода Handle, должен удалить игру и зависимые сущности
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteModifier_ShouldReturnUnit()
		{
			var request = new DeleteModifierCommand()
			{ Id = _modifier.Id };
			
			var newHandler = new DeleteModifierHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.Modifiers
				.FirstOrDefault(x => x.Id == _modifier.Id));
		}
	}
}

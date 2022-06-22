using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ParameterRequests.ChangeParameter;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ParameterRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeParameterHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeParameterHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Parameter _parameter;

		/// <summary>
		/// Тест для <see cref="ChangeParameterHandler"/>
		/// </summary>
		public ChangeParameterHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_parameter = Parameter.CreateForTest(
				name: "old",
				game: _game,
				description: "oldDescription",
				statName: "Ref",
				minValueParameter: 1,
				maxValueParameter: 2);
			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_game, _parameter));
		}

		/// <summary>
		/// Тест метода Handle - создание параметра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateParameter_ShouldReturnUnit()
		{
			var request = new ChangeParameterCommand(
				id: _parameter.Id,
				gameId: _game.Id,
				name: "Melee",
				description: "description",
				statName: "Ref",
				minValueParameter: 0,
				maxValueParameter: 10);

			var newHandler = new ChangeParameterHandler(_dbContext, AuthorizationService.Object);

			var result = newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var parameter = _dbContext.Parameters.FirstOrDefault();
			Assert.IsNotNull(parameter);

			Assert.AreEqual(parameter.Name, "Melee");
			Assert.AreEqual(parameter.Description, "description");
			Assert.AreEqual(parameter.StatName, "Ref");
			Assert.AreEqual(parameter.ParameterBounds.MinValueParameter, 0);
			Assert.AreEqual(parameter.ParameterBounds.MaxValueParameter, 10);
		}



	}
}

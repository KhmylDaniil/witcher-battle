using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ParameterRequests.CreateParameter;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ParameterRequests
{
	/// <summary>
	/// Тест для <see cref="CreateParameterHandler"/>
	/// </summary>
	[TestClass]
	public class CreateParameterHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		public CreateParameterHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_dbContext = CreateInMemoryContext(
				x => x.Add(_game));
		}

		/// <summary>
		/// Тест метода Handle - создание параметра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateParameter_ShouldReturnUnit()
		{
			var request = new CreateParameterCommand(
				gameId: _game.Id,
				name: "Melee",
				description: "description",
				statName: "Ref",
				minValueParameter: 0,
				maxValueParameter: 10);

			var newHandler = new CreateParameterHandler(_dbContext, AuthorizationService.Object);

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

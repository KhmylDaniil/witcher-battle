using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.DeleteBodyTemplateById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteBodyTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteBodyTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteBodyTemplateHandler"/>
		/// </summary>
		public DeleteBodyTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _bodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle - удаление шаблона тела по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteBodyTemplate_ShouldReturnUnit()
		{
			var request = new DeleteBodyTemplateByIdCommand()
			{
				Id = _bodyTemplate.Id,
				GameId = _game.Id
			};

			var newHandler = new DeleteBodyTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.BodyTemplates
				.FirstOrDefault(x => x.Id == _bodyTemplate.Id));
		}
	}
}

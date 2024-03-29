﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteCreatureTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteCreatureTemplateByIdHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteCreatureTemplateByIdHandler"/>
		/// </summary>
		public DeleteCreatureTemplateByIdHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate, creatureType: CreatureType.Human);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _bodyTemplate, _creatureTemplate));
		}

		/// <summary>
		/// Тест метода Handle - удаление шаблона существа по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteCreatureTemplate_ShouldReturnUnit()
		{
			var request = new DeleteCreatureTemplateByIdCommand()
			{
				Id = _creatureTemplate.Id,
			};

			var newHandler = new DeleteCreatureTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.CreatureTemplates
				.FirstOrDefault(x => x.Id == _creatureTemplate.Id));
		}
	}
}

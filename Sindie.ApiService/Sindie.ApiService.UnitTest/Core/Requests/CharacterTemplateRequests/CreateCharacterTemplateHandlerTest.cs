using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.CreateCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CharacterTemplateRequests.CreateCharacterTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CharacterTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="CreateCharacterTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class CreateCharacterTemplateHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly Interface _interface;
		private readonly Modifier _modifier;

		/// <summary>
		/// Конструктор для теста <see cref="CreateCharacterTemplateHandler"/>
		/// </summary>
		public CreateCharacterTemplateHandlerTest(): base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_interface = Interface.CreateForTest();
			_modifier = Modifier.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _imgFile, _interface, _modifier));
		}

		/// <summary>
		/// Тест метода Handle, создание шаблона персонажа и списка модификаторов шаблона персонажа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateCharacterTemplate_ShouldReturnUnit()
		{
			var request = new CreateCharacterTemplateCommand()
			{
				GameId = _game.Id,
				ImgFileId = _imgFile.Id,
				InterfaceId = _interface.Id,
				Name = "name",
				Description = "description",
				Modifiers = new List<Guid> { _modifier.Id },
			};

			var newHandler = new CreateCharacterTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			Assert.AreEqual(1, _dbContext.Modifiers.Count());
			var characterTemplate = _dbContext.CharacterTemplates.FirstOrDefault();

			Assert.AreEqual(request.GameId, characterTemplate.GameId);
			Assert.IsNotNull(characterTemplate.Name);
			Assert.AreEqual(request.Name, characterTemplate.Name);
			Assert.AreEqual(request.Description, characterTemplate.Description);
			Assert.AreEqual(request.ImgFileId, characterTemplate.ImgFileId);
			Assert.AreEqual(request.InterfaceId, characterTemplate.InterfaceId);

			var characterTemplateModifier = _dbContext.CharacterTemplateModifiers
				.FirstOrDefault(x => x.CharacterTemplateId == characterTemplate.Id);
			Assert.IsNotNull(characterTemplateModifier);
			Assert.IsTrue(characterTemplateModifier.ModifierId == _modifier.Id);
		}
	}
}

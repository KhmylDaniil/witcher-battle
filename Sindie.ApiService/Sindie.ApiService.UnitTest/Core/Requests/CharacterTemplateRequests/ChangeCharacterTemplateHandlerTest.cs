using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.ChangeCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CharacterTemplateRequests.ChangeCharacterTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CharacterTemplateRequests
{
	/// <summary>
	/// Тест обработчика изменения шаблона персонажа
	/// </summary>
	[TestClass]
	public class ChangeCharacterTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile1;
		private readonly ImgFile _imgFile2;
		private readonly Interface _interface;
		private readonly Modifier _modifier1;
		private readonly Modifier _modifier2;
		private readonly Character _character;
		private readonly CharacterTemplate _characterTemplate;

		/// <summary>
		/// Конструктор для теста<see cref="ChangeCharacterTemplateHandler"/>
		/// </summary>
		public ChangeCharacterTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile1 = ImgFile.CreateForTest();
			_imgFile2 = ImgFile.CreateForTest();
			_interface = Interface.CreateForTest();
			_modifier1 = Modifier.CreateForTest(game: _game);
			_modifier2 = Modifier.CreateForTest(game: _game);

			_character = Character.CreateForTest(
				instance: Instance.CreateForTest(game: _game));

			_characterTemplate = CharacterTemplate.CreateForTest(
				game: _game,
				imgFile: _imgFile1);
			_characterTemplate.CharacterTemplateModifiers.Add(
				new CharacterTemplateModifier(_modifier1, _characterTemplate));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile1,
				_imgFile2,
				_interface,
				_modifier1,
				_modifier2,
				_character,
				_characterTemplate));
		}

		/// <summary>
		/// Тест метода Handle, должен изменить шаблон персонажа и зависимые сущности
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCharacterTemplate_ShouldReturnUnit()
		{
			var request = new ChangeCharacterTemplateCommand()
			{
				Name = "new name",
				Description = "new description",
				Id = _characterTemplate.Id,
				GameId = _game.Id,
				ImgFileId = _imgFile2.Id,
				InterfaceId = _interface.Id,
				Modifiers = new List<Guid> { _modifier2.Id },
				Characters = new List<Guid> { _character.Id }
			};

			var newHandler = new ChangeCharacterTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var characterTemplate = _dbContext.CharacterTemplates
				.FirstOrDefault(x => x.Id == _characterTemplate.Id);
			Assert.IsNotNull(characterTemplate);
			Assert.AreEqual(characterTemplate.Name, request.Name);
			Assert.AreEqual(characterTemplate.Description, request.Description);
			Assert.AreEqual(characterTemplate.ImgFileId, request.ImgFileId);
			Assert.AreEqual(characterTemplate.InterfaceId, request.InterfaceId);

			var characterTemplateModifier = _dbContext.CharacterTemplateModifiers
				.FirstOrDefault(x => x.CharacterTemplateId == characterTemplate.Id);
			Assert.IsNotNull(characterTemplateModifier);
			Assert.IsTrue(characterTemplateModifier.ModifierId == _modifier2.Id);

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.CharacterTemplateId == characterTemplate.Id);
			Assert.IsNotNull(character);
			Assert.IsTrue(character.Id == _character.Id);
		}
	}
}
